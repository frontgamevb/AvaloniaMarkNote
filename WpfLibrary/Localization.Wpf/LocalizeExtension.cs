using Localization.Common;
using System.Windows.Data;
using System.Windows.Markup;

namespace Localization.Wpf;

public class LocalizeExtension : MarkupExtension
{
    public LocalizeExtension(string key) => Key = key;

    public string Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) => new Binding($"[{Key}]") {
        Mode = BindingMode.OneWay,
        Source = Locale.Instance,
    }.ProvideValue(serviceProvider);
}