using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUTV.Extensions
{
    //public static class SystemExtensions
    //{
    //    public static string GetCurrentUrl
    //    {
    //        get {
    //            Context.Request.Host@Context.Request.Path
    //        }
    //    }
    //}
    static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(
             this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return YieldBatchElements(enumerator, batchSize - 1);
        }

        private static IEnumerable<T> YieldBatchElements<T>(
            IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
                yield return source.Current;
        }
    }
}
