using System.Windows;
using System.Windows.Controls;

namespace Library.Wpf.UserControls;

/// <summary>
/// ControlBox.xaml에 대한 상호 작용 논리
/// </summary>
public partial class ControlBox : UserControl
{
    public ControlBox()
    {
        InitializeComponent();
        Loaded += (_, _) => Onloaded();
    }

    private void Onloaded()
    {
        var window = Window.GetWindow(this);

        WindowMinimizeButton.Click += (_, _) => window.WindowState = WindowState.Minimized;
        WindowMaximizeButton.Click += (_, _) => window.WindowState
            = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        WindowCloseButton.Click += (_, _) => window.Close();
    }
}