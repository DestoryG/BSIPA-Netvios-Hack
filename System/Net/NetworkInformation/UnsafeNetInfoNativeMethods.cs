using System;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002D9 RID: 729
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNetInfoNativeMethods
	{
		// Token: 0x060019BD RID: 6589
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetAdaptersAddresses(AddressFamily family, uint flags, IntPtr pReserved, SafeLocalFree adapterAddresses, ref uint outBufLen);

		// Token: 0x060019BE RID: 6590
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetBestInterfaceEx(byte[] ipAddress, out int index);

		// Token: 0x060019BF RID: 6591
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIfEntry2(ref MibIfRow2 pIfRow);

		// Token: 0x060019C0 RID: 6592
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIpStatisticsEx(out MibIpStats statistics, AddressFamily family);

		// Token: 0x060019C1 RID: 6593
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpStatisticsEx(out MibTcpStats statistics, AddressFamily family);

		// Token: 0x060019C2 RID: 6594
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpStatisticsEx(out MibUdpStats statistics, AddressFamily family);

		// Token: 0x060019C3 RID: 6595
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIcmpStatistics(out MibIcmpInfo statistics);

		// Token: 0x060019C4 RID: 6596
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIcmpStatisticsEx(out MibIcmpInfoEx statistics, AddressFamily family);

		// Token: 0x060019C5 RID: 6597
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpTable(SafeLocalFree pTcpTable, ref uint dwOutBufLen, bool order);

		// Token: 0x060019C6 RID: 6598
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetExtendedTcpTable(SafeLocalFree pTcpTable, ref uint dwOutBufLen, bool order, uint IPVersion, TcpTableClass tableClass, uint reserved);

		// Token: 0x060019C7 RID: 6599
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpTable(SafeLocalFree pUdpTable, ref uint dwOutBufLen, bool order);

		// Token: 0x060019C8 RID: 6600
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetExtendedUdpTable(SafeLocalFree pUdpTable, ref uint dwOutBufLen, bool order, uint IPVersion, UdpTableClass tableClass, uint reserved);

		// Token: 0x060019C9 RID: 6601
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetNetworkParams(SafeLocalFree pFixedInfo, ref uint pOutBufLen);

		// Token: 0x060019CA RID: 6602
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetPerAdapterInfo(uint IfIndex, SafeLocalFree pPerAdapterInfo, ref uint pOutBufLen);

		// Token: 0x060019CB RID: 6603
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle IcmpCreateFile();

		// Token: 0x060019CC RID: 6604
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle Icmp6CreateFile();

		// Token: 0x060019CD RID: 6605
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern bool IcmpCloseHandle(IntPtr handle);

		// Token: 0x060019CE RID: 6606
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x060019CF RID: 6607
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x060019D0 RID: 6608
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6SendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, byte[] sourceSocketAddress, byte[] destSocketAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x060019D1 RID: 6609
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6SendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, byte[] sourceSocketAddress, byte[] destSocketAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x060019D2 RID: 6610
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("iphlpapi.dll")]
		internal static extern void FreeMibTable(IntPtr handle);

		// Token: 0x060019D3 RID: 6611
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("iphlpapi.dll")]
		internal static extern uint CancelMibChangeNotify2(IntPtr notificationHandle);

		// Token: 0x060019D4 RID: 6612
		[DllImport("iphlpapi.dll")]
		internal static extern uint NotifyStableUnicastIpAddressTable([In] AddressFamily addressFamily, out SafeFreeMibTable table, [MarshalAs(UnmanagedType.FunctionPtr)] [In] StableUnicastIpAddressTableDelegate callback, [In] IntPtr context, out SafeCancelMibChangeNotify notificationHandle);

		// Token: 0x04001A3E RID: 6718
		private const string IPHLPAPI = "iphlpapi.dll";
	}
}
