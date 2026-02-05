using System;
using System.Collections.Generic;
using System.Linq;

namespace HarmonyLib
{
	// Token: 0x0200009E RID: 158
	public static class CollectionExtensions
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0000F400 File Offset: 0x0000D600
		public static void Do<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			if (sequence == null)
			{
				return;
			}
			foreach (T t in sequence)
			{
				action(t);
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000F42E File Offset: 0x0000D62E
		public static void DoIf<T>(this IEnumerable<T> sequence, Func<T, bool> condition, Action<T> action)
		{
			sequence.Where(condition).Do(action);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000F43D File Offset: 0x0000D63D
		public static IEnumerable<T> AddItem<T>(this IEnumerable<T> sequence, T item)
		{
			return (sequence ?? Enumerable.Empty<T>()).Concat(new T[] { item });
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000F45C File Offset: 0x0000D65C
		public static T[] AddToArray<T>(this T[] sequence, T item)
		{
			return sequence.AddItem(item).ToArray<T>();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000F46C File Offset: 0x0000D66C
		public static T[] AddRangeToArray<T>(this T[] sequence, T[] items)
		{
			return (sequence ?? Enumerable.Empty<T>()).Concat(items).ToArray<T>();
		}
	}
}
