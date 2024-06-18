using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Converters;

internal sealed class ArrayToCommaSeparatedStringsConverter() : ValueConverter<string[], string>(
    array => string.Join(',', array),
    commaSeparated => commaSeparated.Split(',', StringSplitOptions.None));