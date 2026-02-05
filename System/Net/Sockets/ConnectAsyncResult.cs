using System;

namespace System.Net.Sockets
{
	// Token: 0x02000377 RID: 887
	internal class ConnectAsyncResult : ContextAwareResult
	{
		// Token: 0x06002124 RID: 8484 RVA: 0x0009EED3 File Offset: 0x0009D0D3
		internal ConnectAsyncResult(object myObject, EndPoint endPoint, object myState, AsyncCallback myCallBack)
			: base(myObject, myState, myCallBack)
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002125 RID: 8485 RVA: 0x0009EEE6 File Offset: 0x0009D0E6
		internal EndPoint RemoteEndPoint
		{
			get
			{
				return this.m_EndPoint;
			}
		}

		// Token: 0x04001E59 RID: 7769
		private EndPoint m_EndPoint;
	}
}
