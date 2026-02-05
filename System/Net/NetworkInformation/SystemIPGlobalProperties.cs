using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F6 RID: 758
	internal class SystemIPGlobalProperties : IPGlobalProperties
	{
		// Token: 0x06001AA3 RID: 6819 RVA: 0x000803ED File Offset: 0x0007E5ED
		internal SystemIPGlobalProperties()
		{
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000803F8 File Offset: 0x0007E5F8
		internal static FixedInfo GetFixedInfo()
		{
			uint num = 0U;
			SafeLocalFree safeLocalFree = null;
			FixedInfo fixedInfo = default(FixedInfo);
			uint num2 = UnsafeNetInfoNativeMethods.GetNetworkParams(SafeLocalFree.Zero, ref num);
			while (num2 == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
					num2 = UnsafeNetInfoNativeMethods.GetNetworkParams(safeLocalFree, ref num);
					if (num2 == 0U)
					{
						fixedInfo = new FixedInfo((FIXED_INFO)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(FIXED_INFO)));
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (num2 != 0U)
			{
				throw new NetworkInformationException((int)num2);
			}
			return fixedInfo;
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x00080480 File Offset: 0x0007E680
		internal FixedInfo FixedInfo
		{
			get
			{
				if (!this.fixedInfoInitialized)
				{
					lock (this)
					{
						if (!this.fixedInfoInitialized)
						{
							this.fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
							this.fixedInfoInitialized = true;
						}
					}
				}
				return this.fixedInfo;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x000804E0 File Offset: 0x0007E6E0
		public override string HostName
		{
			get
			{
				if (SystemIPGlobalProperties.hostName == null)
				{
					object obj = SystemIPGlobalProperties.syncObject;
					lock (obj)
					{
						if (SystemIPGlobalProperties.hostName == null)
						{
							SystemIPGlobalProperties.hostName = this.FixedInfo.HostName;
							SystemIPGlobalProperties.domainName = this.FixedInfo.DomainName;
						}
					}
				}
				return SystemIPGlobalProperties.hostName;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x0008055C File Offset: 0x0007E75C
		public override string DomainName
		{
			get
			{
				if (SystemIPGlobalProperties.domainName == null)
				{
					object obj = SystemIPGlobalProperties.syncObject;
					lock (obj)
					{
						if (SystemIPGlobalProperties.domainName == null)
						{
							SystemIPGlobalProperties.hostName = this.FixedInfo.HostName;
							SystemIPGlobalProperties.domainName = this.FixedInfo.DomainName;
						}
					}
				}
				return SystemIPGlobalProperties.domainName;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001AA8 RID: 6824 RVA: 0x000805D8 File Offset: 0x0007E7D8
		public override NetBiosNodeType NodeType
		{
			get
			{
				return this.FixedInfo.NodeType;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001AA9 RID: 6825 RVA: 0x000805F4 File Offset: 0x0007E7F4
		public override string DhcpScopeName
		{
			get
			{
				return this.FixedInfo.ScopeId;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x00080610 File Offset: 0x0007E810
		public override bool IsWinsProxy
		{
			get
			{
				return this.FixedInfo.EnableProxy;
			}
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x0008062C File Offset: 0x0007E82C
		public override TcpConnectionInformation[] GetActiveTcpConnections()
		{
			List<TcpConnectionInformation> list = new List<TcpConnectionInformation>();
			List<SystemTcpConnectionInformation> allTcpConnections = this.GetAllTcpConnections();
			foreach (TcpConnectionInformation tcpConnectionInformation in allTcpConnections)
			{
				if (tcpConnectionInformation.State != TcpState.Listen)
				{
					list.Add(tcpConnectionInformation);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00080698 File Offset: 0x0007E898
		public override IPEndPoint[] GetActiveTcpListeners()
		{
			List<IPEndPoint> list = new List<IPEndPoint>();
			List<SystemTcpConnectionInformation> allTcpConnections = this.GetAllTcpConnections();
			foreach (TcpConnectionInformation tcpConnectionInformation in allTcpConnections)
			{
				if (tcpConnectionInformation.State == TcpState.Listen)
				{
					list.Add(tcpConnectionInformation.LocalEndPoint);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00080708 File Offset: 0x0007E908
		private List<SystemTcpConnectionInformation> GetAllTcpConnections()
		{
			uint num = 0U;
			uint num2 = 0U;
			SafeLocalFree safeLocalFree = null;
			List<SystemTcpConnectionInformation> list = new List<SystemTcpConnectionInformation>();
			if (Socket.OSSupportsIPv4)
			{
				num2 = UnsafeNetInfoNativeMethods.GetTcpTable(SafeLocalFree.Zero, ref num, true);
				while (num2 == 122U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
						num2 = UnsafeNetInfoNativeMethods.GetTcpTable(safeLocalFree, ref num, true);
						if (num2 == 0U)
						{
							IntPtr intPtr = safeLocalFree.DangerousGetHandle();
							MibTcpTable mibTcpTable = (MibTcpTable)Marshal.PtrToStructure(intPtr, typeof(MibTcpTable));
							if (mibTcpTable.numberOfEntries > 0U)
							{
								intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibTcpTable.numberOfEntries));
								int num3 = 0;
								while ((long)num3 < (long)((ulong)mibTcpTable.numberOfEntries))
								{
									MibTcpRow mibTcpRow = (MibTcpRow)Marshal.PtrToStructure(intPtr, typeof(MibTcpRow));
									list.Add(new SystemTcpConnectionInformation(mibTcpRow));
									intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibTcpRow));
									num3++;
								}
							}
						}
					}
					finally
					{
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (num2 != 0U && num2 != 232U)
				{
					throw new NetworkInformationException((int)num2);
				}
			}
			if (Socket.OSSupportsIPv6)
			{
				num = 0U;
				num2 = UnsafeNetInfoNativeMethods.GetExtendedTcpTable(SafeLocalFree.Zero, ref num, true, 23U, TcpTableClass.TcpTableOwnerPidAll, 0U);
				while (num2 == 122U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
						num2 = UnsafeNetInfoNativeMethods.GetExtendedTcpTable(safeLocalFree, ref num, true, 23U, TcpTableClass.TcpTableOwnerPidAll, 0U);
						if (num2 == 0U)
						{
							IntPtr intPtr2 = safeLocalFree.DangerousGetHandle();
							MibTcp6TableOwnerPid mibTcp6TableOwnerPid = (MibTcp6TableOwnerPid)Marshal.PtrToStructure(intPtr2, typeof(MibTcp6TableOwnerPid));
							if (mibTcp6TableOwnerPid.numberOfEntries > 0U)
							{
								intPtr2 = (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(mibTcp6TableOwnerPid.numberOfEntries));
								int num4 = 0;
								while ((long)num4 < (long)((ulong)mibTcp6TableOwnerPid.numberOfEntries))
								{
									MibTcp6RowOwnerPid mibTcp6RowOwnerPid = (MibTcp6RowOwnerPid)Marshal.PtrToStructure(intPtr2, typeof(MibTcp6RowOwnerPid));
									list.Add(new SystemTcpConnectionInformation(mibTcp6RowOwnerPid));
									intPtr2 = (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(mibTcp6RowOwnerPid));
									num4++;
								}
							}
						}
					}
					finally
					{
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (num2 != 0U && num2 != 232U)
				{
					throw new NetworkInformationException((int)num2);
				}
			}
			return list;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00080948 File Offset: 0x0007EB48
		public override IPEndPoint[] GetActiveUdpListeners()
		{
			uint num = 0U;
			uint num2 = 0U;
			SafeLocalFree safeLocalFree = null;
			List<IPEndPoint> list = new List<IPEndPoint>();
			if (Socket.OSSupportsIPv4)
			{
				num2 = UnsafeNetInfoNativeMethods.GetUdpTable(SafeLocalFree.Zero, ref num, true);
				while (num2 == 122U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
						num2 = UnsafeNetInfoNativeMethods.GetUdpTable(safeLocalFree, ref num, true);
						if (num2 == 0U)
						{
							IntPtr intPtr = safeLocalFree.DangerousGetHandle();
							MibUdpTable mibUdpTable = (MibUdpTable)Marshal.PtrToStructure(intPtr, typeof(MibUdpTable));
							if (mibUdpTable.numberOfEntries > 0U)
							{
								intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibUdpTable.numberOfEntries));
								int num3 = 0;
								while ((long)num3 < (long)((ulong)mibUdpTable.numberOfEntries))
								{
									MibUdpRow mibUdpRow = (MibUdpRow)Marshal.PtrToStructure(intPtr, typeof(MibUdpRow));
									int num4 = ((int)mibUdpRow.localPort1 << 8) | (int)mibUdpRow.localPort2;
									list.Add(new IPEndPoint((long)((ulong)mibUdpRow.localAddr), num4));
									intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibUdpRow));
									num3++;
								}
							}
						}
					}
					finally
					{
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (num2 != 0U && num2 != 232U)
				{
					throw new NetworkInformationException((int)num2);
				}
			}
			if (Socket.OSSupportsIPv6)
			{
				num = 0U;
				num2 = UnsafeNetInfoNativeMethods.GetExtendedUdpTable(SafeLocalFree.Zero, ref num, true, 23U, UdpTableClass.UdpTableOwnerPid, 0U);
				while (num2 == 122U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
						num2 = UnsafeNetInfoNativeMethods.GetExtendedUdpTable(safeLocalFree, ref num, true, 23U, UdpTableClass.UdpTableOwnerPid, 0U);
						if (num2 == 0U)
						{
							IntPtr intPtr2 = safeLocalFree.DangerousGetHandle();
							MibUdp6TableOwnerPid mibUdp6TableOwnerPid = (MibUdp6TableOwnerPid)Marshal.PtrToStructure(intPtr2, typeof(MibUdp6TableOwnerPid));
							if (mibUdp6TableOwnerPid.numberOfEntries > 0U)
							{
								intPtr2 = (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(mibUdp6TableOwnerPid.numberOfEntries));
								int num5 = 0;
								while ((long)num5 < (long)((ulong)mibUdp6TableOwnerPid.numberOfEntries))
								{
									MibUdp6RowOwnerPid mibUdp6RowOwnerPid = (MibUdp6RowOwnerPid)Marshal.PtrToStructure(intPtr2, typeof(MibUdp6RowOwnerPid));
									int num6 = ((int)mibUdp6RowOwnerPid.localPort1 << 8) | (int)mibUdp6RowOwnerPid.localPort2;
									list.Add(new IPEndPoint(new IPAddress(mibUdp6RowOwnerPid.localAddr, (long)((ulong)mibUdp6RowOwnerPid.localScopeId)), num6));
									intPtr2 = (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(mibUdp6RowOwnerPid));
									num5++;
								}
							}
						}
					}
					finally
					{
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (num2 != 0U && num2 != 232U)
				{
					throw new NetworkInformationException((int)num2);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00080BD4 File Offset: 0x0007EDD4
		public override IPGlobalStatistics GetIPv4GlobalStatistics()
		{
			return new SystemIPGlobalStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00080BDC File Offset: 0x0007EDDC
		public override IPGlobalStatistics GetIPv6GlobalStatistics()
		{
			return new SystemIPGlobalStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x00080BE5 File Offset: 0x0007EDE5
		public override TcpStatistics GetTcpIPv4Statistics()
		{
			return new SystemTcpStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x00080BED File Offset: 0x0007EDED
		public override TcpStatistics GetTcpIPv6Statistics()
		{
			return new SystemTcpStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00080BF6 File Offset: 0x0007EDF6
		public override UdpStatistics GetUdpIPv4Statistics()
		{
			return new SystemUdpStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00080BFE File Offset: 0x0007EDFE
		public override UdpStatistics GetUdpIPv6Statistics()
		{
			return new SystemUdpStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x00080C07 File Offset: 0x0007EE07
		public override IcmpV4Statistics GetIcmpV4Statistics()
		{
			return new SystemIcmpV4Statistics();
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00080C0E File Offset: 0x0007EE0E
		public override IcmpV6Statistics GetIcmpV6Statistics()
		{
			return new SystemIcmpV6Statistics();
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00080C18 File Offset: 0x0007EE18
		public override UnicastIPAddressInformationCollection GetUnicastAddresses()
		{
			using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
			{
				if (!TeredoHelper.UnsafeNotifyStableUnicastIpAddressTable(new Action<object>(SystemIPGlobalProperties.StableUnicastAddressTableCallback), manualResetEvent))
				{
					manualResetEvent.WaitOne();
				}
			}
			return SystemIPGlobalProperties.GetUnicastAddressTable();
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00080C68 File Offset: 0x0007EE68
		public override IAsyncResult BeginGetUnicastAddresses(AsyncCallback callback, object state)
		{
			ContextAwareResult contextAwareResult = new ContextAwareResult(false, false, this, state, callback);
			contextAwareResult.StartPostingAsyncOp(false);
			if (TeredoHelper.UnsafeNotifyStableUnicastIpAddressTable(new Action<object>(SystemIPGlobalProperties.StableUnicastAddressTableCallback), contextAwareResult))
			{
				contextAwareResult.InvokeCallback();
			}
			contextAwareResult.FinishPostingAsyncOp();
			return contextAwareResult;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00080CAC File Offset: 0x0007EEAC
		public override UnicastIPAddressInformationCollection EndGetUnicastAddresses(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			ContextAwareResult contextAwareResult = asyncResult as ContextAwareResult;
			if (contextAwareResult == null || contextAwareResult.AsyncObject == null || contextAwareResult.AsyncObject.GetType() != typeof(SystemIPGlobalProperties))
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"));
			}
			if (contextAwareResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndGetStableUnicastAddresses" }));
			}
			contextAwareResult.InternalWaitForCompletion();
			contextAwareResult.EndCalled = true;
			return SystemIPGlobalProperties.GetUnicastAddressTable();
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x00080D40 File Offset: 0x0007EF40
		private static void StableUnicastAddressTableCallback(object param)
		{
			EventWaitHandle eventWaitHandle = param as EventWaitHandle;
			if (eventWaitHandle != null)
			{
				eventWaitHandle.Set();
				return;
			}
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)param;
			lazyAsyncResult.InvokeCallback();
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00080D6C File Offset: 0x0007EF6C
		private static UnicastIPAddressInformationCollection GetUnicastAddressTable()
		{
			UnicastIPAddressInformationCollection unicastIPAddressInformationCollection = new UnicastIPAddressInformationCollection();
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				UnicastIPAddressInformationCollection unicastAddresses = allNetworkInterfaces[i].GetIPProperties().UnicastAddresses;
				foreach (UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
				{
					if (!unicastIPAddressInformationCollection.Contains(unicastIPAddressInformation))
					{
						unicastIPAddressInformationCollection.InternalAdd(unicastIPAddressInformation);
					}
				}
			}
			return unicastIPAddressInformationCollection;
		}

		// Token: 0x04001AA6 RID: 6822
		private FixedInfo fixedInfo;

		// Token: 0x04001AA7 RID: 6823
		private bool fixedInfoInitialized;

		// Token: 0x04001AA8 RID: 6824
		private static volatile string hostName = null;

		// Token: 0x04001AA9 RID: 6825
		private static volatile string domainName = null;

		// Token: 0x04001AAA RID: 6826
		private static object syncObject = new object();
	}
}
