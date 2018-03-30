using System;
using System.Collections.Generic;
using System.Linq;

namespace Harness.Tests.Unit
{
    public class GenericComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool>[] Actions { get; }

        private Func<T, int> HashCodeAction { get; }

        public GenericComparer(params Func<T, T, bool>[] actions)
            : this(null, actions)
        {
        }

        public GenericComparer(Func<T, int> getHashCode, params Func<T, T, bool>[] actions)
        {
            this.HashCodeAction = getHashCode;
            this.Actions = actions;
        }

        public bool Equals(T x, T y)
        {
            return this.Actions.All(action => action(x, y));
        }

        public int GetHashCode(T obj)
        {
            if (this.HashCodeAction == null)
            {
                throw new NotImplementedException();
            }

            return this.HashCodeAction(obj);
        }
    }
}