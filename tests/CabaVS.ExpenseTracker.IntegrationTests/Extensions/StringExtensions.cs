namespace CabaVS.ExpenseTracker.IntegrationTests.Extensions;

internal static class StringExtensions
{
    public static string TakeFirstChars(this string str, int count) => 
        new(str.ToCharArray().Take(count).ToArray());
}
