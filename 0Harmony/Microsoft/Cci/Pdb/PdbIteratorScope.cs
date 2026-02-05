using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000316 RID: 790
	internal sealed class PdbIteratorScope : ILocalScope
	{
		// Token: 0x0600122E RID: 4654 RVA: 0x0003CD6F File Offset: 0x0003AF6F
		internal PdbIteratorScope(uint offset, uint length)
		{
			this.offset = offset;
			this.length = length;
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x0003CD85 File Offset: 0x0003AF85
		public uint Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x0003CD8D File Offset: 0x0003AF8D
		public uint Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x04000F2E RID: 3886
		private uint offset;

		// Token: 0x04000F2F RID: 3887
		private uint length;
	}
}
