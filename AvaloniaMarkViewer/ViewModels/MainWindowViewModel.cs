using AvaloniaMarkViewer.Views;
using Mvvm.Avalonia;
using PropertyChanged.SourceGenerator;
using System.ComponentModel;

namespace AvaloniaMarkViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    [Notify] public object? _titleBarContent;

    public MainWindowViewModel(MainWindow view) : base(view)
    {
        TitleBarView = new();
        BindAllMethods(TitleBarView);
        TitleBarContent = TitleBarView;
    }

    private TitleBarView TitleBarView { get; }

    public void ExitMenuItem() => Close();
}