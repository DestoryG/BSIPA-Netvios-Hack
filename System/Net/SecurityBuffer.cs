using System;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000132 RID: 306
	internal class SecurityBuffer
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x0003DCB4 File Offset: 0x0003BEB4
		public SecurityBuffer(byte[] data, int offset, int size, BufferType tokentype)
		{
			this.offset = ((data == null || offset < 0) ? 0 : Math.Min(offset, data.Length));
			this.size = ((data == null || size < 0) ? 0 : Math.Min(size, data.Length - this.offset));
			this.type = tokentype;
			this.token = ((size == 0) ? null : data);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003DD15 File Offset: 0x0003BF15
		public SecurityBuffer(byte[] data, BufferType tokentype)
		{
			this.size = ((data == null) ? 0 : data.Length);
			this.type = tokentype;
			this.token = ((this.size == 0) ? null : data);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003DD45 File Offset: 0x0003BF45
		public SecurityBuffer(int size, BufferType tokentype)
		{
			this.size = size;
			this.type = tokentype;
			this.token = ((size == 0) ? null : new byte[size]);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003DD6D File Offset: 0x0003BF6D
		public SecurityBuffer(ChannelBinding binding)
		{
			this.size = ((binding == null) ? 0 : binding.Size);
			this.type = BufferType.ChannelBindings;
			this.unmanagedToken = binding;
		}

		// Token: 0x04001039 RID: 4153
		public int size;

		// Token: 0x0400103A RID: 4154
		public BufferType type;

		// Token: 0x0400103B RID: 4155
		public byte[] token;

		// Token: 0x0400103C RID: 4156
		public SafeHandle unmanagedToken;

		// Token: 0x0400103D RID: 4157
		public int offset;
	}
}
