using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Specialized
{
	// Token: 0x020003B9 RID: 953
	internal class BackCompatibleStringComparer : IEqualityComparer
	{
		// Token: 0x060023E2 RID: 9186 RVA: 0x000A8AD1 File Offset: 0x000A6CD1
		internal BackCompatibleStringComparer()
		{
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000A8ADC File Offset: 0x000A6CDC
		public unsafe static int GetHashCode(string obj)
		{
			char* ptr = obj;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			int num = 5381;
			char* ptr2 = ptr;
			int num2;
			while ((num2 = (int)(*ptr2)) != 0)
			{
				num = ((num << 5) + num) ^ num2;
				ptr2++;
			}
			return num;
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000A8B1A File Offset: 0x000A6D1A
		bool IEqualityComparer.Equals(object a, object b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000A8B24 File Offset: 0x000A6D24
		public virtual int GetHashCode(object o)
		{
			string text = o as string;
			if (text == null)
			{
				return o.GetHashCode();
			}
			return BackCompatibleStringComparer.GetHashCode(text);
		}

		// Token: 0x04001FEA RID: 8170
		internal static IEqualityComparer Default = new BackCompatibleStringComparer();
	}
}
