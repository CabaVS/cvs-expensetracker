using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CabaVS.ExpenseTracker.Web.Pages;

public class LoginModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage("/Index");
}