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

        public override bool Equals(object obj)
        {
            if (obj?.GetType() != typeof(Person))
            {
                return false;
            }

            var person = (Person)obj;

            return
                string.Equals(person.FirstName, this.FirstName)
                && string.Equals(person.LastName, this.LastName)
                && person.Age == this.Age;
        }
    }
}
