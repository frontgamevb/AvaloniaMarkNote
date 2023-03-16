using PropertyChanged.SourceGenerator;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows;

namespace WpfMarkViewer;

public partial class Settings : INotifyPropertyChanged
{
    [Notify] public double _mainWindowLeft;
    [Notify] public double _mainWindowTop;
    [Notify] public double _mainWindowHeight = 700;
    [Notify] public double _mainWindowWidth = 1280;
    [Notify] public WindowState _mainWindowState = WindowState.Normal;

    [Notify] public string _cultureName = string.Empty;

    [Notify] public bool _menuIsVisible = true;

    [JsonIgnore]
    public Visibility MenuVisibility => MenuIsVisible ? Visibility.Visible : Visibility.Hidden;

    public string RecentFolder { get; set; } = string.Empty;
    public string RecentFile { get; set; } = string.Empty;
}