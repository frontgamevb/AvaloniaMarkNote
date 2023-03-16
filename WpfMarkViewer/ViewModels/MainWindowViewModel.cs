using Mvvm.Wpf;
using PropertyChanged.SourceGenerator;
using System.ComponentModel;
using WpfMarkViewer.Views;

namespace WpfMarkViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    [Notify] public object? _titleBarContent;
    [Notify] public object? _sideBarContent;
    [Notify] public object? _sideToolBarContent;

    public MainWindowViewModel(MainWindow view) : base(view)
    {
        TitleBarView = new() { DataContext = this };
        BindAllMethods(TitleBarView);
        TitleBarContent = TitleBarView;

        SideBarView = new() { DataContext = this };
        BindAllMethods(SideBarView);
        SideBarContent = SideBarView;

        SideToolBarView = new() { DataContext = this };
        BindAllMethods(SideToolBarView);
        SideToolBarContent = SideToolBarView;
    }

    public Settings Settings { get; } = App.Settings!;

    private TitleBarView TitleBarView { get; }

    private SideBarView SideBarView { get; }

    private SideToolBarView SideToolBarView { get; }
}