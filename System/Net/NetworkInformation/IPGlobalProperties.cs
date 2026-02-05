using System;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A1 RID: 673
	[global::__DynamicallyInvokable]
	public abstract class IPGlobalProperties
	{
		// Token: 0x0600190E RID: 6414 RVA: 0x0007DC75 File Offset: 0x0007BE75
		[global::__DynamicallyInvokable]
		public static IPGlobalProperties GetIPGlobalProperties()
		{
			new NetworkInformationPermission(NetworkInformationAccess.Read).Demand();
			return new SystemIPGlobalProperties();
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0007DC87 File Offset: 0x0007BE87
		internal static IPGlobalProperties InternalGetIPGlobalProperties()
		{
			return new SystemIPGlobalProperties();
		}

		// Token: 0x06001910 RID: 6416
		[global::__DynamicallyInvokable]
		public abstract IPEndPoint[] GetActiveUdpListeners();

		// Token: 0x06001911 RID: 6417
		[global::__DynamicallyInvokable]
		public abstract IPEndPoint[] GetActiveTcpListeners();

		// Token: 0x06001912 RID: 6418
		[global::__DynamicallyInvokable]
		public abstract TcpConnectionInformation[] GetActiveTcpConnections();

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001913 RID: 6419
		[global::__DynamicallyInvokable]
		public abstract string DhcpScopeName
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001914 RID: 6420
		[global::__DynamicallyInvokable]
		public abstract string DomainName
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001915 RID: 6421
		[global::__DynamicallyInvokable]
		public abstract string HostName
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001916 RID: 6422
		[global::__DynamicallyInvokable]
		public abstract bool IsWinsProxy
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001917 RID: 6423
		[global::__DynamicallyInvokable]
		public abstract NetBiosNodeType NodeType
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06001918 RID: 6424
		[global::__DynamicallyInvokable]
		public abstract TcpStatistics GetTcpIPv4Statistics();

		// Token: 0x06001919 RID: 6425
		[global::__DynamicallyInvokable]
		public abstract TcpStatistics GetTcpIPv6Statistics();

		// Token: 0x0600191A RID: 6426
		[global::__DynamicallyInvokable]
		public abstract UdpStatistics GetUdpIPv4Statistics();

		// Token: 0x0600191B RID: 6427
		[global::__DynamicallyInvokable]
		public abstract UdpStatistics GetUdpIPv6Statistics();

		// Token: 0x0600191C RID: 6428
		[global::__DynamicallyInvokable]
		public abstract IcmpV4Statistics GetIcmpV4Statistics();

		// Token: 0x0600191D RID: 6429
		[global::__DynamicallyInvokable]
		public abstract IcmpV6Statistics GetIcmpV6Statistics();

		// Token: 0x0600191E RID: 6430
		[global::__DynamicallyInvokable]
		public abstract IPGlobalStatistics GetIPv4GlobalStatistics();

		// Token: 0x0600191F RID: 6431
		[global::__DynamicallyInvokable]
		public abstract IPGlobalStatistics GetIPv6GlobalStatistics();

		// Token: 0x06001920 RID: 6432 RVA: 0x0007DC8E File Offset: 0x0007BE8E
		[global::__DynamicallyInvokable]
		public virtual UnicastIPAddressInformationCollection GetUnicastAddresses()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0007DC95 File Offset: 0x0007BE95
		[global::__DynamicallyInvokable]
		public virtual IAsyncResult BeginGetUnicastAddresses(AsyncCallback callback, object state)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0007DC9C File Offset: 0x0007BE9C
		[global::__DynamicallyInvokable]
		public virtual UnicastIPAddressInformationCollection EndGetUnicastAddresses(IAsyncResult asyncResult)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0007DCA3 File Offset: 0x0007BEA3
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual Task<UnicastIPAddressInformationCollection> GetUnicastAddressesAsync()
		{
			return Task<UnicastIPAddressInformationCollection>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetUnicastAddresses), new Func<IAsyncResult, UnicastIPAddressInformationCollection>(this.EndGetUnicastAddresses), null);
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0007DCCA File Offset: 0x0007BECA
		[global::__DynamicallyInvokable]
		protected IPGlobalProperties()
		{
		}
	}
}
