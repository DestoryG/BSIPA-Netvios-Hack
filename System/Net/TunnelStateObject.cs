using System;

namespace System.Net
{
	// Token: 0x020001A1 RID: 417
	internal struct TunnelStateObject
	{
		// Token: 0x06000FF5 RID: 4085 RVA: 0x00053813 File Offset: 0x00051A13
		internal TunnelStateObject(HttpWebRequest r, Connection c)
		{
			this.Connection = c;
			this.OriginalRequest = r;
		}

		// Token: 0x0400132A RID: 4906
		internal Connection Connection;

		// Token: 0x0400132B RID: 4907
		internal HttpWebRequest OriginalRequest;
	}
}
