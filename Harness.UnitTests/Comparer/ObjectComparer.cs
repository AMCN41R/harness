using System;
using System.Collections;
using System.Reflection;

namespace Harness.UnitTests.Comparer
{
    public static class ObjectComparer
    {
        public static void AssertObjectsAreEqual(object expected, object actual)
        {
            var properties = expected.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var expectedValue = prop.GetValue(expected);
                var actualValue = prop.GetValue(actual);

                if (expectedValue is IList)
                {
                    AssertObjectListsAreEqual((IList)expectedValue, (IList)actualValue);
                    continue;
                }

                if (expectedValue is DateTime)
                {
                    var exp = expectedValue.ToString();
                    var act = actualValue.ToString();

                    if (!Equals(exp, act))
                    {
                        throw new ComparerException(
                            PropertyMismatchMessage(prop, expectedValue, actualValue)
                            );
                    }
                    continue;
                }

                if (!Equals(expectedValue, actualValue))
                {
                    throw new ComparerException(
                        PropertyMismatchMessage(prop, expectedValue, actualValue)
                        );
                }
            }
        }

        public static void AssertObjectListsAreEqual(IList expected, IList actual)
        {
            if (expected.Count != actual.Count)
            {
                throw new ComparerException(
                    "Expected and Actual IList count mismatch. " +
                    $"Expected IEnumerable contains {expected.Count} elements but " +
                    $"Actual IEnumerable contains {actual.Count} elements.");
            }

            for (var i = 0; i < expected.Count; i++)
            {
                AssertObjectsAreEqual(expected[i], actual[i]);
            }
        }

        private static string PropertyMismatchMessage(
            PropertyInfo prop,
            object expectedValue,
            object actualValue)
        {
            var propNameString =
                    prop.DeclaringType != null
                        ? $"{prop.DeclaringType.Name}.{prop.Name}"
                        : $"{prop.Name}";

            return
                $"Property {propNameString} does not match." +
                Environment.NewLine +
                $"Expected: {expectedValue} but was: {actualValue}";

        }
    }
}
