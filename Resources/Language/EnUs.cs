using Localization.Common;

namespace Language;

public class EnUs : ILocale
{
    public string CountryCode => "US";
    public string LanguageCode => "en";
    public string LanguageName => "English";

    public const string Title = "Avalonia MarkViewer";

    // Word

    public const string File = "File";
    public const string Folder = "Folder";
    public const string Open = "Open";

    public const string View = "View";
    public const string Switch = "Switch";
    public const string Dark = "Dark";
    public const string Light = "Light";
    public const string Theme = "Theme";
    public const string Fullscreen = "Full Screen";

    public const string Option = "Option";
    public const string Language = "Language";

    public const string Help = "Help";

    // MenuItem

    public const string FileMenuItem = $"_{File}";
    public const string OpenFileMenuItem = $"_{Open} {File}";
    public const string OpenFolderMenuItem = $"{Open} {Folder}";
    public const string ExitMenuItem = "E_xit";

    public const string ViewMenuItem = $"_{View}";
    public const string LightDarkMenuItem = $"{Switch} {Light} & {Dark}";
    public const string FullscreenMenuItem = Fullscreen;

    public const string OptionMenuItem = $"_{Option}";
    public const string LanguageMenuItem = $"_{Language}";

    public const string HelpMenuItem = $"_{Help}";
    public const string AboutMenuItem = $"_About {Title}";
}