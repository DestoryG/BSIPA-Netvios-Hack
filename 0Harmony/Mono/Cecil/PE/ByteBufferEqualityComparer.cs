using System;
using System.Collections.Generic;

namespace Mono.Cecil.PE
{
	// Token: 0x02000192 RID: 402
	internal sealed class ByteBufferEqualityComparer : IEqualityComparer<ByteBuffer>
	{
		// Token: 0x06000CBC RID: 3260 RVA: 0x0002AAA4 File Offset: 0x00028CA4
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

		// Token: 0x06000CBD RID: 3261 RVA: 0x0002AAEC File Offset: 0x00028CEC
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
