using Localization.Common;

namespace Language;

public class KoKr : ILocale
{
    public string CountryCode => "KR";
    public string LanguageCode => "ko";
    public string LanguageName => "한국어";

    public const string Title = "Avalonia MarkViewer";

    // Word

    public const string File = "파일";
    public const string Folder = "폴더";
    public const string Open = "열기";

    public const string View = "보기";
    public const string Switch = "전환";
    public const string Dark = "Dark";
    public const string Light = "Light";
    public const string Fullscreen = "전체 화면";

    public const string Option = "옵션";
    public const string Language = "언어";

    public const string Help = "도움말";

    // MenuItem

    public const string FileMenuItem = $"{File}(_F)";
    public const string OpenFileMenuItem = $"{File} {Open}(_O)";
    public const string OpenFolderMenuItem = $"{Folder} {Open}";
    public const string ExitMenuItem = "끝내기(_X)";

    public const string ViewMenuItem = $"{View}(_V)";
    public const string LightDarkMenuItem = $"{Light} & {Dark} {Switch}";
    public const string FullscreenMenuItem = Fullscreen;

    public const string OptionMenuItem = $"{Option}(_O)";
    public const string LanguageMenuItem = $"{Language}(_L)";

    public const string HelpMenuItem = $"{Help}(_H)";
    public const string AboutMenuItem = $"{Title} 정보(_A)";
}