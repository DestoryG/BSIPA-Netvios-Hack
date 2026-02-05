using System;
using System.ComponentModel;

namespace System.Net.Security
{
	// Token: 0x02000351 RID: 849
	internal class ProtocolToken
	{
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x0008F8D3 File Offset: 0x0008DAD3
		internal bool Failed
		{
			get
			{
				return this.Status != SecurityStatus.OK && this.Status != SecurityStatus.ContinueNeeded;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x0008F8EF File Offset: 0x0008DAEF
		internal bool Done
		{
			get
			{
				return this.Status == SecurityStatus.OK;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x0008F8FA File Offset: 0x0008DAFA
		internal bool Renegotiate
		{
			get
			{
				return this.Status == SecurityStatus.Renegotiate;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x0008F909 File Offset: 0x0008DB09
		internal bool CloseConnection
		{
			get
			{
				return this.Status == SecurityStatus.ContextExpired;
			}
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0008F918 File Offset: 0x0008DB18
		internal ProtocolToken(byte[] data, SecurityStatus errorCode)
		{
			this.Status = errorCode;
			this.Payload = data;
			this.Size = ((data != null) ? data.Length : 0);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0008F93D File Offset: 0x0008DB3D
		internal Win32Exception GetException()
		{
			if (!this.Done)
			{
				return new Win32Exception((int)this.Status);
			}
			return null;
		}

		// Token: 0x04001CDB RID: 7387
		internal SecurityStatus Status;

		// Token: 0x04001CDC RID: 7388
		internal byte[] Payload;

		// Token: 0x04001CDD RID: 7389
		internal int Size;
	}
}
