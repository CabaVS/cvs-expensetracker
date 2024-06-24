using Xunit.Abstractions;
using Xunit.Sdk;

namespace CabaVS.ExpenseTracker.IntegrationTests.Common;

public sealed class TestCaseOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (var testCase in testCases)
        {
            var order = testCase
                .TestMethod
                .Method
                .GetCustomAttributes(typeof(TestOrderAttribute))
                .SingleOrDefault()?
                .GetNamedArgument<int>(nameof(TestOrderAttribute.Order)) ?? int.MaxValue;

            GetOrCreate(sortedMethods, order).Add(testCase);
        }

        foreach (var (_, methods) in sortedMethods)
        {
            methods.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
            foreach (var testCase in methods)
                yield return testCase;
        }
    }
    
    private static TValue GetOrCreate<TKey, TValue>(SortedDictionary<TKey, TValue> dictionary, TKey key) 
        where TKey : notnull
        where TValue : new()
    {
        if (dictionary.TryGetValue(key, out var result)) return result;

        result = new TValue();
        dictionary[key] = result;

        return result;
    }
}