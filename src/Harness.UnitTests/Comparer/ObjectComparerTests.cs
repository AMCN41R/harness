using System;
using System.Collections.Generic;
using Xunit;

namespace Harness.UnitTests.Comparer
{
    public class ObjectComparerTests
    {
        [Fact]
        public void AssertObjectPropertyValuesAreEqual_PassTwoIdenticalClasses_ReturnsTrue()
        {
            var testObject1 = new TestComparerClass();
            var testObject2 = new TestComparerClass();

            ObjectComparer.AssertObjectsAreEqual(testObject1, testObject2);

        }

        [Fact]
        public void AssertObjectPropertyValuesAreEqual_PassTwoInstancesOfTheSameClassWithDifferentValues_ThrowsComparerException()
        {
            var testObject1 = new TestComparerClass();
            var testObject2 = new TestComparerClass { TestInteger = 11111 };

            Assert.Throws<ComparerException>(
                () => ObjectComparer.AssertObjectsAreEqual(testObject1, testObject2));

        }

        [Fact]
        public void AssertObjectPropertyValuesAreEqual_PassTwoInstancesOfTheSameClassWithDifferentValuesInListProperty_ThrowsComparerException()
        {
            var testObject1 = new TestComparerClass();
            var testObject2 = new TestComparerClass();
            testObject2.TestChildren[0].TestInteger = 11111;

            Assert.Throws<ComparerException>(
                () => ObjectComparer.AssertObjectsAreEqual(testObject1, testObject2));

        }

        [Fact]
        public void AssertObjectPropertyValuesAreEqual_PassTwoInstancesOfTheSameClassWithDifferentDateValues_ThrowsComparerException()
        {
            var testObject1 = new TestComparerClass();
            var testObject2 = new TestComparerClass { TestDate = DateTime.UtcNow };

            Assert.Throws<ComparerException>(
                () => ObjectComparer.AssertObjectsAreEqual(testObject1, testObject2));

        }

        [Fact]
        public void AssertListsAreEqual_PassTwoIdenticalLists_ReturnsTrue()
        {
            List<TestComparerClass> testList1 = new List<TestComparerClass> {
                new TestComparerClass(),
                new TestComparerClass(),
                new TestComparerClass()
            };

            List<TestComparerClass> testList2 = new List<TestComparerClass> {
                new TestComparerClass(),
                new TestComparerClass(),
                new TestComparerClass()
            };

            ObjectComparer.AssertObjectListsAreEqual(testList1, testList2);

        }

        [Fact]

        public void AssertListsAreEqual_PassTwoListsWithDifferentNumberOfItems_ThrowsObjectComparerException()
        {
            List<TestComparerClass> testList1 = new List<TestComparerClass> {
                new TestComparerClass(),
                new TestComparerClass(),
                new TestComparerClass()
            };

            List<TestComparerClass> testList2 = new List<TestComparerClass> {
                new TestComparerClass(),
                new TestComparerClass()
            };

            Assert.Throws<ComparerException>(
                () => ObjectComparer.AssertObjectListsAreEqual(testList1, testList2));

        }
    }

    public class TestComparerClass
    {
        public string TestString { get; set; }
        public int TestInteger { get; set; }
        public double TestDouble { get; set; }
        public decimal TestDecimal { get; set; }
        public DateTime TestDate { get; set; }
        public bool TestBoolean { get; set; }
        public char TestChar { get; set; }
        public Guid TestGuid { get; set; }
        public List<TestComparerChildClass> TestChildren { get; set; }
        public TestComparerClass()
        {
            TestString = "abc";
            TestInteger = 123;
            TestDouble = 456;
            TestDecimal = 78.9m;
            TestDate = new DateTime(2015, 1, 1);
            TestBoolean = true;
            TestChar = 'A';
            TestGuid = Guid.Parse("{B4C518D3-CA60-4924-BD39-37BAAD122806}");
            TestChildren = new List<TestComparerChildClass> {
            new TestComparerChildClass(),
            new TestComparerChildClass(),
            new TestComparerChildClass()
        };
        }
    }

    public class TestComparerChildClass
    {
        public string TestString { get; set; }
        public int TestInteger { get; set; }
        public double TestDouble { get; set; }
        public decimal TestDecimal { get; set; }
        public DateTime TestDate { get; set; }
        public bool TestBoolean { get; set; }
        public char TestChar { get; set; }
        public Guid TestGuid { get; set; }
        public TestComparerChildClass()
        {
            TestString = "qwerty";
            TestInteger = 65416;
            TestDouble = 196849;
            TestDecimal = 12.1256m;
            TestDate = new DateTime(2015, 10, 10);
            TestBoolean = false;
            TestChar = 'Z';
            TestGuid = Guid.Parse("{188A9E21-F32D-4077-81FD-E6474980BD10}");
        }
    }
}
