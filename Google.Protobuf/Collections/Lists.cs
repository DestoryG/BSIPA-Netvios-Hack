using System;
using System.Collections.Generic;

namespace Google.Protobuf.Collections
{
	// Token: 0x02000084 RID: 132
	public static class Lists
	{
		// Token: 0x06000843 RID: 2115 RVA: 0x0001D48C File Offset: 0x0001B68C
		public static bool Equals<T>(List<T> left, List<T> right)
		{
			if (left == right)
			{
				return true;
			}
			if (left == null || right == null)
			{
				return false;
			}
			if (left.Count != right.Count)
			{
				return false;
			}
			IEqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < left.Count; i++)
			{
				if (!@default.Equals(left[i], right[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
		public static int GetHashCode<T>(List<T> list)
		{
			if (list == null)
			{
				return 0;
			}
			int num = 31;
			foreach (T t in list)
			{
				num = num * 29 + t.GetHashCode();
			}
			return num;
		}
	}
}
