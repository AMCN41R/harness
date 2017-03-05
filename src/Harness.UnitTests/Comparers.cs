using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.Settings;

namespace Harness.UnitTests
{
    internal static class Comparers
    {
        public static IEqualityComparer<CollectionConfig> CollectionConfigComparer()
        {
            return new GenericComparer<CollectionConfig>(
                (x, y) => x.DropFirst == y.DropFirst,
                (x, y) => string.Equals(x.CollectionName, y.CollectionName),
                (x, y) => string.Equals(x.DataFileLocation, y.DataFileLocation)
            );
        }

        public static IEqualityComparer<DatabaseConfig> DatabaseConfigComparer()
        {
            return new GenericComparer<DatabaseConfig>(
                (x, y) => x.DropFirst == y.DropFirst,
                (x, y) => string.Equals(x.DatabaseName, y.DatabaseName),
                (x, y) => string.Equals(x.CollectionNameSuffix, y.CollectionNameSuffix),
                (x, y) => string.Equals(x.ConnectionString, y.ConnectionString),
                (x, y) => (x.Collections == null && y.Collections == null) || x.Collections.SequenceEqual(y.Collections, CollectionConfigComparer())
            );
        }

        public static IEqualityComparer<HarnessConfiguration> HarnessConfigurationComparer()
        {
            return new GenericComparer<HarnessConfiguration>(
                (x, y) => (x.Databases == null && y.Databases == null) || x.Databases.SequenceEqual(y.Databases, DatabaseConfigComparer())
            );
        }
    }
}
