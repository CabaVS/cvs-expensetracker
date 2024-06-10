using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Converters;

internal sealed class DateOnlyConverter() : ValueConverter<DateOnly, DateTime>(
    dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
    dateTime => DateOnly.FromDateTime(dateTime));