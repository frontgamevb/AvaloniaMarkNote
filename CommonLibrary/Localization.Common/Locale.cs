using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Localization.Common;

public class Locale
{
    public static Locale? Instance { get; private set; }

    public static void Init(params ILocale[] locales)
    {
        Debug.Assert(Instance is null);
        Instance = new(locales);
    }

    public static void Init(string cultureName, params ILocale[] locales)
    {
        Init(locales);
        if(!string.IsNullOrEmpty(cultureName)) SetLanguage(cultureName);
    }

    public static void SetLanguage(string cultureName)
    {
        Debug.Assert(Instance is not null && !string.IsNullOrEmpty(cultureName));
        var cultureInfo = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentCulture = cultureInfo;      // 시간, 통화 등
        Thread.CurrentThread.CurrentUICulture = cultureInfo;    // 언어
        Instance._currentLocaleMap = Instance.GetLocaleMapFromCultureName(cultureName);
    }

    public static string Tr(string key) => Instance?.ReadString(key) ??
        $"Not Found: {CultureInfo.CurrentCulture.Name}:{key}";

    public string this[string key] => Tr(key);

    private readonly Dictionary<string, string> _defaultLocaleMap;
    private readonly Collection<LocaleRecord> _localeRecords = new();
    private Dictionary<string, string> _currentLocaleMap;

    private record LocaleRecord(string LangaugeName, string LanguageCode, string? CountryCode,
        Dictionary<string, string> KeyValuePairs);

    private Locale(params ILocale[] locales)
    {
        Debug.Assert(locales.Length > 0);
        foreach(var locale in locales)
            _localeRecords.Add(new(locale.LanguageName, locale.LanguageCode,
                locale.CountryCode, ConvertClassToMap(locale)));
        _defaultLocaleMap = _localeRecords[0].KeyValuePairs;
        _currentLocaleMap = GetLocaleMapFromCultureName();
    }

    private static Dictionary<string, string> ConvertClassToMap(ILocale locale)
    {
        var keyValuePairs = new Dictionary<string, string>();

        foreach(var field in locale.GetType().GetFields(
            BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public))
            if(!field.IsSpecialName && field.GetValue(null) is not null)
                keyValuePairs.Add(field.Name, field.GetValue(null)!.ToString() ?? string.Empty);
        return keyValuePairs;
    }

    private Dictionary<string, string> GetLocaleMapFromCultureName(string cultureName = "")
    {
        if(string.IsNullOrWhiteSpace(cultureName)) cultureName = Thread.CurrentThread.CurrentUICulture.Name;

        // ko-KR과 같은 지역화 코드에 맞는 dictionary를 찾는다.
        foreach(var localeRecord in _localeRecords)
            if(cultureName == $"{localeRecord.LanguageCode}-{localeRecord.CountryCode}")
                return localeRecord.KeyValuePairs;

        // 없을 경우 ko와 같은 언어 코드에 맞는 dictionary를 찾는다.
        foreach(var localeRecord in _localeRecords)
            if(cultureName[..2] == localeRecord.LanguageCode)
                return localeRecord.KeyValuePairs;

        return _defaultLocaleMap;
    }

    private string? ReadString(string key)
    {
        if(_currentLocaleMap.TryGetValue(key, out var value) || _defaultLocaleMap == _currentLocaleMap) return value;
        _ = _defaultLocaleMap.TryGetValue(key, out var defaultValue);
        return defaultValue;
    }
}