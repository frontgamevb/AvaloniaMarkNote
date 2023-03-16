using Library.Common.Mvvm;
using Log.Common;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Mvvm.Wpf;

public class ViewModelBase
{
    private const string CanExecutePrefix = "Can";
    private const string CommandSuffix = "Command";

    private readonly Dictionary<string, (MethodInfo method, DelegateCommand? delegateCommand)>
        _methodMap = new();

    /// <summary>Binding View and ViewModel.</summary>
    /// <param name="view">View to bind.</param>
    protected ViewModelBase(Window view, bool bindOrNot = false)
    {
        View = view;
        GetAllPublicMethods();
        if(bindOrNot) BindAllMethods();
    }

    /// <summary>View property to attach to the ViewModel.</summary>
    protected Window? View { get; }

    /// <summary>Close View.</summary>
    protected void Close() => View?.Close();

    /// <summary>Bind a method in the ViewModel to a control of the same name in the View.</summary>
    protected void BindAllMethods(Control? view = null)
    {
        view ??= View!;
        foreach(var method in _methodMap)
        {
            var obj = view.FindName(method.Key);
            if(obj is Control control)
            {
                _ = _methodMap.TryGetValue(CanExecutePrefix + method.Key, out var canExecuteMethod);
                BindControlToMethod(control, method.Value.method, canExecuteMethod.method);
            }
        }
    }

    /// <summary>
    /// Bind the control to the method.
    /// Set KeyGesture, InputGesture.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="method"></param>
    /// <param name="canExecuteMethod"></param>
    protected void BindControlToMethod(Control control, MethodInfo method, MethodInfo? canExecuteMethod = null)
    {
        var name = method.Name;
        ExDebug.Assert(control.GetType().GetProperties().Any(x => x.Name == CommandSuffix),
            Texts.ControlHasNoCommandProperty, View!.Name ?? string.Empty, name);

        // Bind the control to the method.
        var delegateCommand = new DelegateCommand((Action)method.CreateDelegate(typeof(Action), this),
            (Func<bool>?)canExecuteMethod?.CreateDelegate(typeof(Func<bool>), this));
        _methodMap[name] = (method, delegateCommand);
        _ = control.SetBinding(ButtonBase.CommandProperty, new Binding { Source = delegateCommand });
        ExDebug.Print(Texts.ControlBoundToMethod, GetType().Name, name);

        // Set KeyGesture if any.
        var obj = Application.Current!.TryFindResource(name + nameof(KeyGesture));
        if(obj is KeyGesture keyGesture)
        {
            _ = View.InputBindings.Add(new InputBinding(delegateCommand, keyGesture));
            if(control is MenuItem menuItem)
                menuItem.InputGestureText = keyGesture.Modifiers == ModifierKeys.None ? ""
                    : $"{keyGesture.Modifiers}+" + keyGesture.Key.ToString();
        }
    }

    protected void OnCanExecuteChanged(string methodName)
    {
        _ = _methodMap.TryGetValue(methodName, out var method);
        ExTrace.Assert(method.delegateCommand is not null,
            string.Format(Texts.CommandNotFound, GetType().Name, methodName));
        method.delegateCommand?.OnCanExecuteChanged();
    }

    private void GetAllPublicMethods()
    {
        foreach(var method in GetType().GetMethods(
            BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            if(!method.IsSpecialName) _methodMap.Add(method.Name, (method, null));
    }
}