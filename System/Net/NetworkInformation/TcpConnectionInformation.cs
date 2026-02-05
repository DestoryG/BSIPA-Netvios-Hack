using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000305 RID: 773
	[global::__DynamicallyInvokable]
	public abstract class TcpConnectionInformation
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001B61 RID: 7009
		[global::__DynamicallyInvokable]
		public abstract IPEndPoint LocalEndPoint
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001B62 RID: 7010
		[global::__DynamicallyInvokable]
		public abstract IPEndPoint RemoteEndPoint
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001B63 RID: 7011
		[global::__DynamicallyInvokable]
		public abstract TcpState State
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x00081F64 File Offset: 0x00080164
		[global::__DynamicallyInvokable]
		protected TcpConnectionInformation()
		{
		}
	}
}
