using System.Collections.Generic;
using System.Linq;
using Harness.Settings;

namespace Harness.Tests.Unit
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
                (x, y) => (x.Collections == null && y.Collections == null) || (x.Collections != null && x.Collections.SequenceEqual(y.Collections, CollectionConfigComparer()))
            );
        }

        public static IEqualityComparer<ConventionConfig> ConventionConfigComparer()
        {
            return new GenericComparer<ConventionConfig>(
                (x, y) => string.Equals(x.Name, y.Name),
                (x, y) => x.ConventionPack.Conventions.Count() == y.ConventionPack.Conventions.Count(),
                (x, y) =>
                {
                    var xTypes = x.ConventionPack.Conventions.Select(c => c.GetType()).ToList();
                    var yTypes = y.ConventionPack.Conventions.Select(c => c.GetType()).ToList();
                    var diff = xTypes.Except(yTypes);
                    return !diff.Any();
                },
                (x, y) => Neleus.LambdaCompare.Lambda.ExpressionsEqual(x.Filter, y.Filter)
            );
        }

        public static IEqualityComparer<HarnessConfiguration> HarnessConfigurationComparer()
        {
            return new GenericComparer<HarnessConfiguration>(
                (x, y) => (x.Conventions == null && y.Conventions == null) || ConventionConfigComparer().Equals(x.Conventions, y.Conventions),
                (x, y) => (x.Databases == null && y.Databases == null) || (x.Databases != null && x.Databases.SequenceEqual(y.Databases, DatabaseConfigComparer()))
            );
        }
    }
}
