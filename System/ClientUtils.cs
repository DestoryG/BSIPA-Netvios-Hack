using System;
using System.Security;
using System.Threading;

namespace System
{
	// Token: 0x0200005B RID: 91
	internal static class ClientUtils
	{
		// Token: 0x06000409 RID: 1033 RVA: 0x0001D54E File Offset: 0x0001B74E
		public static bool IsCriticalException(Exception ex)
		{
			return ex is NullReferenceException || ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is ExecutionEngineException || ex is IndexOutOfRangeException || ex is AccessViolationException;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001D58B File Offset: 0x0001B78B
		public static bool IsSecurityOrCriticalException(Exception ex)
		{
			return ex is SecurityException || ClientUtils.IsCriticalException(ex);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001D5A0 File Offset: 0x0001B7A0
		public static int GetBitCount(uint x)
		{
			int num = 0;
			while (x > 0U)
			{
				x &= x - 1U;
				num++;
			}
			return num;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001D5C4 File Offset: 0x0001B7C4
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001D5E4 File Offset: 0x0001B7E4
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue, int maxNumberOfBitsOn)
		{
			bool flag = value >= minValue && value <= maxValue;
			return flag && ClientUtils.GetBitCount((uint)value) <= maxNumberOfBitsOn;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001D618 File Offset: 0x0001B818
		public static bool IsEnumValid_Masked(Enum enumValue, int value, uint mask)
		{
			return ((long)value & (long)((ulong)mask)) == (long)value;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001D630 File Offset: 0x0001B830
		public static bool IsEnumValid_NotSequential(Enum enumValue, int value, params int[] enumValues)
		{
			for (int i = 0; i < enumValues.Length; i++)
			{
				if (enumValues[i] == value)
				{
					return true;
				}
			}
			return false;
		}
	}
}
