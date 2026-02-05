using System;
using System.Security;

namespace System.Collections.Generic
{
	// Token: 0x020003CF RID: 975
	internal class BitHelper
	{
		// Token: 0x06002546 RID: 9542 RVA: 0x000AD75A File Offset: 0x000AB95A
		[SecurityCritical]
		internal unsafe BitHelper(int* bitArrayPtr, int length)
		{
			this.m_arrayPtr = bitArrayPtr;
			this.m_length = length;
			this.useStackAlloc = true;
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000AD777 File Offset: 0x000AB977
		internal BitHelper(int[] bitArray, int length)
		{
			this.m_array = bitArray;
			this.m_length = length;
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000AD790 File Offset: 0x000AB990
		[SecurityCritical]
		internal unsafe void MarkBit(int bitPosition)
		{
			if (this.useStackAlloc)
			{
				int num = bitPosition / 32;
				if (num < this.m_length && num >= 0)
				{
					this.m_arrayPtr[num] |= 1 << bitPosition % 32;
					return;
				}
			}
			else
			{
				int num2 = bitPosition / 32;
				if (num2 < this.m_length && num2 >= 0)
				{
					this.m_array[num2] |= 1 << bitPosition % 32;
				}
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000AD7FC File Offset: 0x000AB9FC
		[SecurityCritical]
		internal unsafe bool IsMarked(int bitPosition)
		{
			if (this.useStackAlloc)
			{
				int num = bitPosition / 32;
				return num < this.m_length && num >= 0 && (this.m_arrayPtr[num] & (1 << bitPosition % 32)) != 0;
			}
			int num2 = bitPosition / 32;
			return num2 < this.m_length && num2 >= 0 && (this.m_array[num2] & (1 << bitPosition % 32)) != 0;
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000AD868 File Offset: 0x000ABA68
		internal static int ToIntArrayLength(int n)
		{
			if (n <= 0)
			{
				return 0;
			}
			return (n - 1) / 32 + 1;
		}

		// Token: 0x04002040 RID: 8256
		private const byte MarkedBitFlag = 1;

		// Token: 0x04002041 RID: 8257
		private const byte IntSize = 32;

		// Token: 0x04002042 RID: 8258
		private int m_length;

		// Token: 0x04002043 RID: 8259
		[SecurityCritical]
		private unsafe int* m_arrayPtr;

		// Token: 0x04002044 RID: 8260
		private int[] m_array;

		// Token: 0x04002045 RID: 8261
		private bool useStackAlloc;
	}
}
