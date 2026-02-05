using System;
using System.Collections.Generic;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B6 RID: 182
	internal static class UtilityTypeExtensions
	{
		// Token: 0x06000640 RID: 1600 RVA: 0x0001DE8B File Offset: 0x0001C08B
		private static IEnumerable<AggregateType> TypeAndBaseClasses(this AggregateType type)
		{
			for (AggregateType t = type; t != null; t = t.GetBaseClass())
			{
				yield return t;
			}
			yield break;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001DE9B File Offset: 0x0001C09B
		private static IEnumerable<AggregateType> TypeAndBaseClassInterfaces(this AggregateType type)
		{
			foreach (AggregateType aggregateType in type.TypeAndBaseClasses())
			{
				foreach (AggregateType aggregateType2 in aggregateType.GetIfacesAll().Items)
				{
					yield return aggregateType2;
				}
				CType[] array = null;
			}
			IEnumerator<AggregateType> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001DEAC File Offset: 0x0001C0AC
		public static IEnumerable<AggregateType> AllPossibleInterfaces(this CType type)
		{
			AggregateType aggregateType;
			if ((aggregateType = type as AggregateType) != null)
			{
				return aggregateType.TypeAndBaseClassInterfaces();
			}
			return Array.Empty<AggregateType>();
		}
	}
}
