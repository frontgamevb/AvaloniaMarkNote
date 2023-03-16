using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaMarkViewer.Views;
using Language;
using Localization.Common;
using Log.Common;

namespace AvaloniaMarkViewer;

public partial class App : Application
{
    public override void Initialize()
    {
        LogBase.AddFileListener();
        ExTrace.Print("Start");
        Locale.Init(new EnUs(), new KoKr());

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}