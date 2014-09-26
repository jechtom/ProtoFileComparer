using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    public static class Extensions
    {
        public static IEnumerable<FullJoinGroup<TResult, TKey>> FullJoinSingleRow<TValue, TKey, TResult>(
            this IEnumerable<TValue> left,
            IEnumerable<TValue> right,
            Func<TValue, TKey> keySelector,
            Func<TValue, TResult> valueSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            return FullJoin(left, right, keySelector, keySelector,
                (l, r, k) => new FullJoinGroup<TResult, TKey>()
                {
                    Left = l.Select(valueSelector).SingleOrDefault(),
                    Right = r.Select(valueSelector).SingleOrDefault(),
                    Key = k
                }
                , comparer);
        }

        public static IEnumerable<TResult> FullJoin<TValue, TKey, TResult>(
            this IEnumerable<TValue> left,
            IEnumerable<TValue> right,
            Func<TValue, TKey> keySelector,
            Func<IEnumerable<TValue>, IEnumerable<TValue>, TKey, TResult> projection,
            IEqualityComparer<TKey> comparer = null)
        {
            return FullJoin(left, right, keySelector, keySelector, projection, comparer);
        }

        public static IEnumerable<TResult> FullJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<IEnumerable<TLeft>, IEnumerable<TRight>, TKey, TResult> projection,
            IEqualityComparer<TKey> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var leftSet = left.ToLookup(leftKeySelector, comparer);
            var rightSet = right.ToLookup(rightKeySelector, comparer);

            // union
            var keys = new HashSet<TKey>(leftSet.Select(p => p.Key), comparer);
            keys.UnionWith(rightSet.Select(p => p.Key));

            // result projection
            var result = keys.Select(k => projection(leftSet[k], rightSet[k], k));
            return result;
        }
    }
}
