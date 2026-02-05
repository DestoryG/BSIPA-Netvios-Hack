using System;

namespace System.Net
{
	// Token: 0x020000DC RID: 220
	internal class SystemNetworkCredential : NetworkCredential
	{
		// Token: 0x06000783 RID: 1923 RVA: 0x00029C1E File Offset: 0x00027E1E
		private SystemNetworkCredential()
			: base(string.Empty, string.Empty, string.Empty)
		{
		}

		// Token: 0x04000D1E RID: 3358
		internal static readonly SystemNetworkCredential defaultCredential = new SystemNetworkCredential();
	}
}
