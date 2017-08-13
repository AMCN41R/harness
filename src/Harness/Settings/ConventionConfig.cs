using System;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization.Conventions;

namespace Harness.Settings
{
    public class ConventionConfig
    {
        /// <summary>
        /// Gets or sets the name of the convention set.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the conventions.
        /// </summary>
        public IConventionPack ConventionPack { get; set; }

        /// <summary>
        /// Gets or sets the filter to select the types that the 
        /// conventions should be applied to.
        /// </summary>
        public Expression<Func<Type, bool>> Filter { get; set; }
    }
}