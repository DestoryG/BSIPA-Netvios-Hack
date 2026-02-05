using System;
using System.Collections.Generic;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000003 RID: 3
	internal static class Functional
	{
		// Token: 0x06000017 RID: 23 RVA: 0x000026B0 File Offset: 0x000008B0
		public static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
		{
			Func<A, R> g = null;
			g = f((A a) => g(a));
			return g;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026E8 File Offset: 0x000008E8
		public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Functional.PrependIterator<TSource>(source, element);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026FF File Offset: 0x000008FF
		private static IEnumerable<TSource> PrependIterator<TSource>(IEnumerable<TSource> source, TSource element)
		{
			yield return element;
			foreach (TSource tsource in source)
			{
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}
	}
}
