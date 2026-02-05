using System;
using System.Collections.Generic;

namespace Mono.Cecil.PE
{
	// Token: 0x020000D0 RID: 208
	internal sealed class ByteBufferEqualityComparer : IEqualityComparer<ByteBuffer>
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x0001B9A4 File Offset: 0x00019BA4
		public bool Equals(ByteBuffer x, ByteBuffer y)
		{
			if (x.length != y.length)
			{
				return false;
			}
			byte[] buffer = x.buffer;
			byte[] buffer2 = y.buffer;
			for (int i = 0; i < x.length; i++)
			{
				if (buffer[i] != buffer2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001B9EC File Offset: 0x00019BEC
		public int GetHashCode(ByteBuffer buffer)
		{
			int num = -2128831035;
			byte[] buffer2 = buffer.buffer;
			for (int i = 0; i < buffer.length; i++)
			{
				num = (num ^ (int)buffer2[i]) * 16777619;
			}
			return num;
		}
	}
}
