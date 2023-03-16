using Avalonia.Data;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml;
using Localization.Common;

namespace Localization.Avalonia;

public class LocalizeExtension : MarkupExtension
{
    public LocalizeExtension(string key) => Key = key;

    public string Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ReflectionBindingExtension($"[{Key}]")
        {
            Mode = BindingMode.OneWay,
            Source = Locale.Instance
        }.ProvideValue(serviceProvider);
}