using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CabaVS.ExpenseTracker.Persistence.Converters;

internal sealed class DateOnlyToDateTimeConverter()
    : ValueConverter<DateOnly, DateTime>(
        dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
        dateTime => DateOnly.FromDateTime(dateTime));
