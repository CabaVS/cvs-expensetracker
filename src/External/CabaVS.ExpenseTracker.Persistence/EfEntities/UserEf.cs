﻿namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class UserEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string UserName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}
