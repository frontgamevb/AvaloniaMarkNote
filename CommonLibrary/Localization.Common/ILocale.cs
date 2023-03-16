namespace Localization.Common;

public interface ILocale
{
    public abstract string CountryCode { get; }
    public abstract string LanguageCode { get; }
    public abstract string LanguageName { get; }
}