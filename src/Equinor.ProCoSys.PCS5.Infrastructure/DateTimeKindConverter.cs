using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Equinor.ProCoSys.PCS5.Infrastructure
{
    public class DateTimeKindConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeKindConverter() : base(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        {}
    }
}
