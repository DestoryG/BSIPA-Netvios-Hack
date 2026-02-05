using System;

namespace Microsoft.Cci
{
	// Token: 0x02000015 RID: 21
	internal sealed class PdbIteratorScope : ILocalScope
	{
		// Token: 0x06000150 RID: 336 RVA: 0x00003BB3 File Offset: 0x00001DB3
		internal PdbIteratorScope(uint offset, uint length)
		{
			this.offset = offset;
			this.length = length;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00003BC9 File Offset: 0x00001DC9
		public uint Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00003BD1 File Offset: 0x00001DD1
		public uint Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x0400001E RID: 30
		private uint offset;

		// Token: 0x0400001F RID: 31
		private uint length;
	}
}
