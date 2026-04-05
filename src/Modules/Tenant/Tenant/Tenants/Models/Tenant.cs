using Tenant.Tenants.Events;

namespace Tenant.Tenants.Models;

public class Tenant : Aggregate<Guid>
{
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string? LogoUrl { get; private set; }
    public bool IsActive { get; private set; }
    public string DefaultLocale { get; private set; } = "nl";
    public List<string> SupportedLocales { get; private set; } = ["nl"];

    public static Tenant Create(Guid id, string name, string slug, string? logoUrl, bool isActive,
        string defaultLocale = "nl", List<string>? supportedLocales = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        var tenant = new Tenant
        {
            Id = id,
            Name = name,
            Slug = slug,
            LogoUrl = logoUrl,
            IsActive = isActive,
            DefaultLocale = defaultLocale,
            SupportedLocales = supportedLocales ?? [defaultLocale]
        };

        tenant.AddDomainEvent(new TenantCreatedEvent(tenant));

        return tenant;
    }

    public void Update(string name, string slug, string? logoUrl, bool isActive,
        string? defaultLocale = null, List<string>? supportedLocales = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        Name = name;
        Slug = slug;
        LogoUrl = logoUrl;
        IsActive = isActive;
        if (defaultLocale is not null) DefaultLocale = defaultLocale;
        if (supportedLocales is not null) SupportedLocales = supportedLocales;
    }
}
