using Xunit.Abstractions;
using Xunit.Sdk;

namespace CabaVS.ExpenseTracker.IntegrationTests;

public sealed class TestCaseOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

#pragma warning disable CA1062
        foreach (TTestCase testCase in testCases)
#pragma warning restore CA1062
        {
            var order = testCase
                .TestMethod
                .Method
                .GetCustomAttributes(typeof(TestOrderAttribute))
                .SingleOrDefault()?
                .GetNamedArgument<int>(nameof(TestOrderAttribute.Order)) ?? int.MaxValue;

            GetOrCreate(sortedMethods, order).Add(testCase);
        }

        foreach ((_, List<TTestCase> methods) in sortedMethods)
        {
            methods.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
            
            foreach (TTestCase testCase in methods)
            {
                yield return testCase;
            }
        }
    }
    
    private static TValue GetOrCreate<TKey, TValue>(SortedDictionary<TKey, TValue> dictionary, TKey key) 
        where TKey : notnull
        where TValue : new()
    {
        if (dictionary.TryGetValue(key, out TValue? result))
        {
            return result;
        }

        result = new TValue();
        dictionary[key] = result;

        return result;
    }
}
