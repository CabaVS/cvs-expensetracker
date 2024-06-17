using System.Data;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Converters;

internal sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly date)
        => parameter.Value = date.ToDateTime(TimeOnly.MinValue);
    public override DateOnly Parse(object value)
        => DateOnly.FromDateTime((DateTime)value);
}