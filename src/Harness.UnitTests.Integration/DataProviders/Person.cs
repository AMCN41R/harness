using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Bson;

namespace Harness.UnitTests.Integration.DataProviders
{
    public class PersonDataProvider : IDataProvider
    {
        public IEnumerable<object> GetData()
        {
            return new List<Person>
            {
                new Person {FirstName = "Peter", LastName = "Venkman", Age = 31},
                new Person {FirstName = "Ray", LastName = "Stantz", Age = 32},
                new Person {FirstName = "Egon", LastName = "Spengler", Age = 33}
            };
        }
    }

    public class Person
    {
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
    }

    public static class EqualityExtensions
    {
        public static bool IsEqual(this Person a, Person b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            return
                a != null 
                && string.Equals(a.FirstName, b.FirstName)
                && string.Equals(a.LastName, b.LastName)
                && a.Age == b.Age;
        }
    }
}
