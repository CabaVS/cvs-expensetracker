namespace CabaVS.ExpenseTracker.Web.Configuration.Models;

internal sealed class IdentityServerConfigurationModel
{
    public string? Authority { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Scope { get; set; }
}