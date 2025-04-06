using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CabaVS.ExpenseTracker.Persistence.Converters;

internal sealed class StringArrayToCommaSeparatedStringConverter()
    : ValueConverter<string[], string>(
        strArray => string.Join(Separator, strArray),
        str => str.Split(Separator, StringSplitOptions.None))
{
    private const char Separator = ',';
}

internal sealed class StringArrayToCommaSeparatedStringComparer()
    : ValueComparer<string[]>(
        (arr1, arr2) => arr1!.SequenceEqual(arr2!),
        arr => arr.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode(StringComparison.InvariantCulture))),
        arr => arr.ToArray());
