using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Library.Common.Mvvm;
using Log.Common;
using System.Reflection;

namespace Mvvm.Avalonia;

public class ViewModelBase
{
    private const string CanExecutePrefix = "Can";
    private const string CommandSuffix = "Command";
    private const string IconSuffix = "Icon";

    private readonly Dictionary<string, (MethodInfo method, DelegateCommand? delegateCommand)>
        _methodMap = new();

    protected ViewModelBase(Window view)
    {
        View = view;
        GetAllPublicMethods();
        BindAllMethods();
    }

    protected Window? View { get; }

    protected void Close() => View?.Close();

    protected void BindAllMethods(Control? view = null)
    {
        view ??= View!;
        foreach (var method in _methodMap)
        {
            var control = view.Find<Control>(method.Key);
            if (control is null) continue;

            _ = _methodMap.TryGetValue(CanExecutePrefix + method.Key, out var canExecuteMethod);
            BindControlToMethod(control, method.Value.method, canExecuteMethod.method);
        }
    }

    protected void BindControlToMethod(Control control, MethodInfo method, MethodInfo? canExecuteMethod = null)
    {
        var name = method.Name;
        ExDebug.Assert(control.GetType().GetProperties().Any(x => x.Name == CommandSuffix),
            Texts.ControlHasNoCommandProperty, View!.Name ?? string.Empty, name);

        // Bind the control to the method.
        var delegateCommand = new DelegateCommand((Action)method.CreateDelegate(typeof(Action), this),
            (Func<bool>?)canExecuteMethod?.CreateDelegate(typeof(Func<bool>), this));
        _methodMap[name] = (method, delegateCommand);
        _ = control.Bind(Button.CommandProperty, new Binding { Source = delegateCommand });
        ExDebug.Print(Texts.ControlBoundToMethod, GetType().Name, name);

        // Set KeyGesture if any.
        if (Application.Current!.TryFindResource(name + nameof(KeyGesture), out var resource) &&
            resource is KeyGesture keyGesture)
        {
            HotKeyManager.SetHotKey(control, keyGesture);
            if (control is MenuItem menuItem) menuItem.InputGesture = keyGesture;
        }

        // Set Icon if any.
        if (control is MenuItem item && Application.Current!.TryFindResource(name + IconSuffix, out var icon)
            && icon is PathIcon)
            item.Icon = icon;
    }

    private void GetAllPublicMethods()
    {
        foreach (var method in GetType().GetMethods(
            BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            if (!method.IsSpecialName) _methodMap.Add(method.Name, (method, null));
    }
}