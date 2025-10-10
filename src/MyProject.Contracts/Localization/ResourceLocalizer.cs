using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Microsoft.Extensions.Localization;

namespace MyProject.Contracts.Localization;

public class ResourceLocalizer : IResourceLocalizer
{
    public IStringLocalizer<Module> Module { get; } =
        new CustomStringLocalizer<Module>("Module");

    public IStringLocalizer<Error> Error { get; } =
        new CustomStringLocalizer<Error>("Error");

    public IStringLocalizer<Message> Message { get; } =
        new CustomStringLocalizer<Message>("Message");
}

internal class CustomStringLocalizer<T> : IStringLocalizer<T>
{
    private readonly ResourceManager _resourceManager;

    public CustomStringLocalizer(string resourceName)
    {
        var assembly = typeof(ResourceLocalizer).Assembly;
        var baseName = $"{typeof(AssemblyMarker).Namespace}.Resources.{resourceName}.{resourceName}";
        _resourceManager = new ResourceManager(baseName, assembly);
    }

    public LocalizedString this[string name]
    {
        get
        {
            try
            {
                var culture = CultureInfo.CurrentUICulture;
                var value = _resourceManager.GetString(name, culture);

                if (value != null)
                    return new LocalizedString(name, value, false);

                // Fallback to invariant culture
                value = _resourceManager.GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
            catch
            {
                return new LocalizedString(name, name, true);
            }
        }
    }

    public LocalizedString this[string name, params object[] arguments] => this[name];

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return [];
    }
}