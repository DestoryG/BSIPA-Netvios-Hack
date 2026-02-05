using System;
using System.Collections.Generic;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000038 RID: 56
	internal static class ListExtensions
	{
		// Token: 0x06000255 RID: 597 RVA: 0x00011993 File Offset: 0x0000FB93
		public static bool IsEmpty<T>(this List<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000119A3 File Offset: 0x0000FBA3
		public static T Head<T>(this List<T> list)
		{
			return list[0];
		}

		// Token: 0x06000257 RID: 599 RVA: 0x000119AC File Offset: 0x0000FBAC
		public static List<T> Tail<T>(this List<T> list)
		{
			T[] array = new T[list.Count];
			list.CopyTo(array, 0);
			List<T> list2 = new List<T>(array);
			list2.RemoveAt(0);
			return list2;
		}
	}
}
