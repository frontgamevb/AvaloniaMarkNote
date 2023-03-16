using Language;
using Library.Common.Serialization;
using Localization.Common;
using Log.Common;
using System.Windows;

namespace WpfMarkViewer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static Settings? Settings { get; private set; }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        LogBase.AddFileListener();
        Settings = TextJson.Deserialize<Settings>() ?? new();
        Locale.Init(Settings.CultureName, new EnUs(), new KoKr());

        new MainWindow().Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        TextJson.Serialize(Settings);
        base.OnExit(e);
    }
}