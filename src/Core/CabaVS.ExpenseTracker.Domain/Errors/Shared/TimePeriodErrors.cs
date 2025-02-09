using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Errors.Shared;

public static class TimePeriodErrors
{
    public static Error StartDateIsGreaterThanEndDate() =>
        new("TimePeriod.StartDateIsGreaterThanEndDate", "Start date cannot be greater than end date.");
}
