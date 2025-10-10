using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUlid;

namespace MyProject.Domain.ValueConverters;

public static class UlidValueConverters
{
    public static readonly ValueConverter<Ulid?, string> UlidToStringConverter =
        new(
            v => v.HasValue ? v.Value.ToString() : null!,
            v => string.IsNullOrEmpty(v) ? null : Ulid.Parse(v)
        );
}