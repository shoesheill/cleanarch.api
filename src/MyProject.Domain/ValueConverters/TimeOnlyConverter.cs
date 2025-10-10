using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace MyProject.Domain.ValueConverters;

public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
        v => v.ToTimeSpan(),
        v => TimeOnly.FromTimeSpan(v))
    {
    }
}
