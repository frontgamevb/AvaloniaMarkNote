using Avalonia.Controls;
using AvaloniaMarkViewer.ViewModels;

namespace AvaloniaMarkViewer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(this);
    }
}