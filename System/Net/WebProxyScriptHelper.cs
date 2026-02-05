using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Net
{
	// Token: 0x02000192 RID: 402
	internal class WebProxyScriptHelper : IReflect
	{
		// Token: 0x06000F7C RID: 3964 RVA: 0x00050724 File Offset: 0x0004E924
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return null;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00050727 File Offset: 0x0004E927
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0005072A File Offset: 0x0004E92A
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return new MethodInfo[0];
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00050732 File Offset: 0x0004E932
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00050735 File Offset: 0x0004E935
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return new FieldInfo[0];
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0005073D File Offset: 0x0004E93D
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00050740 File Offset: 0x0004E940
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return null;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00050743 File Offset: 0x0004E943
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return new PropertyInfo[0];
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0005074B File Offset: 0x0004E94B
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return new MemberInfo[]
			{
				new WebProxyScriptHelper.MyMethodInfo(name)
			};
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0005075C File Offset: 0x0004E95C
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return new MemberInfo[0];
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00050764 File Offset: 0x0004E964
		object IReflect.InvokeMember(string name, BindingFlags bindingAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return null;
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x00050767 File Offset: 0x0004E967
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0005076C File Offset: 0x0004E96C
		public bool isPlainHostName(string hostName)
		{
			if (hostName == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.isPlainHostName()", "hostName" }));
				}
				throw new ArgumentNullException("hostName");
			}
			return hostName.IndexOf('.') == -1;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000507C4 File Offset: 0x0004E9C4
		public bool dnsDomainIs(string host, string domain)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.dnsDomainIs()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			if (domain == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.dnsDomainIs()", "domain" }));
				}
				throw new ArgumentNullException("domain");
			}
			int num = host.LastIndexOf(domain);
			return num != -1 && num + domain.Length == host.Length;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00050870 File Offset: 0x0004EA70
		public bool localHostOrDomainIs(string host, string hostDom)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.localHostOrDomainIs()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			if (hostDom == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.localHostOrDomainIs()", "hostDom" }));
				}
				throw new ArgumentNullException("hostDom");
			}
			if (this.isPlainHostName(host))
			{
				int num = hostDom.IndexOf('.');
				if (num > 0)
				{
					hostDom = hostDom.Substring(0, num);
				}
			}
			return string.Compare(host, hostDom, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00050928 File Offset: 0x0004EB28
		public bool isResolvable(string host)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.isResolvable()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			IPHostEntry iphostEntry = null;
			try
			{
				iphostEntry = Dns.InternalGetHostByName(host);
			}
			catch
			{
			}
			if (iphostEntry == null)
			{
				return false;
			}
			for (int i = 0; i < iphostEntry.AddressList.Length; i++)
			{
				if (iphostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x000509C0 File Offset: 0x0004EBC0
		public string dnsResolve(string host)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.dnsResolve()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			IPHostEntry iphostEntry = null;
			try
			{
				iphostEntry = Dns.InternalGetHostByName(host);
			}
			catch
			{
			}
			if (iphostEntry == null)
			{
				return string.Empty;
			}
			for (int i = 0; i < iphostEntry.AddressList.Length; i++)
			{
				if (iphostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
				{
					return iphostEntry.AddressList[i].ToString();
				}
			}
			return string.Empty;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00050A6C File Offset: 0x0004EC6C
		public string myIpAddress()
		{
			IPAddress[] localAddresses = NclUtilities.LocalAddresses;
			for (int i = 0; i < localAddresses.Length; i++)
			{
				if (!IPAddress.IsLoopback(localAddresses[i]) && localAddresses[i].AddressFamily == AddressFamily.InterNetwork)
				{
					return localAddresses[i].ToString();
				}
			}
			return string.Empty;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x00050AB0 File Offset: 0x0004ECB0
		public int dnsDomainLevels(string host)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.dnsDomainLevels()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			int num = 0;
			int num2 = 0;
			while ((num = host.IndexOf('.', num)) != -1)
			{
				num2++;
				num++;
			}
			return num2;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00050B1C File Offset: 0x0004ED1C
		public bool isInNet(string host, string pattern, string mask)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.isInNet()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			if (pattern == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.isInNet()", "pattern" }));
				}
				throw new ArgumentNullException("pattern");
			}
			if (mask == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.isInNet()", "mask" }));
				}
				throw new ArgumentNullException("mask");
			}
			try
			{
				IPAddress ipaddress = IPAddress.Parse(host);
				IPAddress ipaddress2 = IPAddress.Parse(pattern);
				IPAddress ipaddress3 = IPAddress.Parse(mask);
				byte[] addressBytes = ipaddress3.GetAddressBytes();
				byte[] addressBytes2 = ipaddress.GetAddressBytes();
				byte[] addressBytes3 = ipaddress2.GetAddressBytes();
				if (addressBytes.Length != addressBytes2.Length || addressBytes.Length != addressBytes3.Length)
				{
					return false;
				}
				for (int i = 0; i < addressBytes.Length; i++)
				{
					if ((addressBytes3[i] & addressBytes[i]) != (addressBytes2[i] & addressBytes[i]))
					{
						return false;
					}
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00050C74 File Offset: 0x0004EE74
		public bool shExpMatch(string host, string pattern)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.shExpMatch()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			if (pattern == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.shExpMatch()", "pattern" }));
				}
				throw new ArgumentNullException("pattern");
			}
			bool flag;
			try
			{
				ShellExpression shellExpression = new ShellExpression(pattern);
				flag = shellExpression.IsMatch(host);
			}
			catch (FormatException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00050D2C File Offset: 0x0004EF2C
		public bool weekdayRange(string wd1, [Optional] object wd2, [Optional] object gmt)
		{
			if (wd1 == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.weekdayRange()", "wd1" }));
				}
				throw new ArgumentNullException("wd1");
			}
			string text = null;
			string text2 = null;
			if (gmt != null && gmt != DBNull.Value && gmt != Missing.Value)
			{
				text = gmt as string;
				if (text == null)
				{
					throw new ArgumentException(SR.GetString("net_param_not_string", new object[] { gmt.GetType().FullName }), "gmt");
				}
			}
			if (wd2 != null && wd2 != DBNull.Value && gmt != Missing.Value)
			{
				text2 = wd2 as string;
				if (text2 == null)
				{
					throw new ArgumentException(SR.GetString("net_param_not_string", new object[] { wd2.GetType().FullName }), "wd2");
				}
			}
			if (text != null)
			{
				if (!WebProxyScriptHelper.isGMT(text))
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.weekdayRange()", "gmt" }));
					}
					throw new ArgumentException(SR.GetString("net_proxy_not_gmt"), "gmt");
				}
				return WebProxyScriptHelper.weekdayRangeInternal(DateTime.UtcNow, WebProxyScriptHelper.dayOfWeek(wd1), WebProxyScriptHelper.dayOfWeek(text2));
			}
			else
			{
				if (text2 == null)
				{
					return WebProxyScriptHelper.weekdayRangeInternal(DateTime.Now, WebProxyScriptHelper.dayOfWeek(wd1), WebProxyScriptHelper.dayOfWeek(wd1));
				}
				if (WebProxyScriptHelper.isGMT(text2))
				{
					return WebProxyScriptHelper.weekdayRangeInternal(DateTime.UtcNow, WebProxyScriptHelper.dayOfWeek(wd1), WebProxyScriptHelper.dayOfWeek(wd1));
				}
				return WebProxyScriptHelper.weekdayRangeInternal(DateTime.Now, WebProxyScriptHelper.dayOfWeek(wd1), WebProxyScriptHelper.dayOfWeek(text2));
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00050EBF File Offset: 0x0004F0BF
		private static bool isGMT(string gmt)
		{
			return string.Compare(gmt, "GMT", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00050ED0 File Offset: 0x0004F0D0
		private static DayOfWeek dayOfWeek(string weekDay)
		{
			if (weekDay != null && weekDay.Length == 3)
			{
				if (weekDay[0] == 'T' || weekDay[0] == 't')
				{
					if ((weekDay[1] == 'U' || weekDay[1] == 'u') && (weekDay[2] == 'E' || weekDay[2] == 'e'))
					{
						return DayOfWeek.Tuesday;
					}
					if ((weekDay[1] == 'H' || weekDay[1] == 'h') && (weekDay[2] == 'U' || weekDay[2] == 'u'))
					{
						return DayOfWeek.Thursday;
					}
				}
				if (weekDay[0] == 'S' || weekDay[0] == 's')
				{
					if ((weekDay[1] == 'U' || weekDay[1] == 'u') && (weekDay[2] == 'N' || weekDay[2] == 'n'))
					{
						return DayOfWeek.Sunday;
					}
					if ((weekDay[1] == 'A' || weekDay[1] == 'a') && (weekDay[2] == 'T' || weekDay[2] == 't'))
					{
						return DayOfWeek.Saturday;
					}
				}
				if ((weekDay[0] == 'M' || weekDay[0] == 'm') && (weekDay[1] == 'O' || weekDay[1] == 'o') && (weekDay[2] == 'N' || weekDay[2] == 'n'))
				{
					return DayOfWeek.Monday;
				}
				if ((weekDay[0] == 'W' || weekDay[0] == 'w') && (weekDay[1] == 'E' || weekDay[1] == 'e') && (weekDay[2] == 'D' || weekDay[2] == 'd'))
				{
					return DayOfWeek.Wednesday;
				}
				if ((weekDay[0] == 'F' || weekDay[0] == 'f') && (weekDay[1] == 'R' || weekDay[1] == 'r') && (weekDay[2] == 'I' || weekDay[2] == 'i'))
				{
					return DayOfWeek.Friday;
				}
			}
			return (DayOfWeek)(-1);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x000510A0 File Offset: 0x0004F2A0
		private static bool weekdayRangeInternal(DateTime now, DayOfWeek wd1, DayOfWeek wd2)
		{
			if (wd1 < DayOfWeek.Sunday || wd2 < DayOfWeek.Sunday)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_invalid_parameter", new object[] { "WebProxyScriptHelper.weekdayRange()" }));
				}
				throw new ArgumentException(SR.GetString("net_proxy_invalid_dayofweek"), (wd1 < DayOfWeek.Sunday) ? "wd1" : "wd2");
			}
			if (wd1 <= wd2)
			{
				return wd1 <= now.DayOfWeek && now.DayOfWeek <= wd2;
			}
			return wd2 >= now.DayOfWeek || now.DayOfWeek >= wd1;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00051135 File Offset: 0x0004F335
		public string getClientVersion()
		{
			return "1.0";
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0005113C File Offset: 0x0004F33C
		public unsafe string sortIpAddressList(string IPAddressList)
		{
			if (IPAddressList == null || IPAddressList.Length == 0)
			{
				return string.Empty;
			}
			string[] array = IPAddressList.Split(new char[] { ';' });
			if (array.Length > WebProxyScriptHelper.MAX_IPADDRESS_LIST_LENGTH)
			{
				throw new ArgumentException(string.Format(SR.GetString("net_max_ip_address_list_length_exceeded"), WebProxyScriptHelper.MAX_IPADDRESS_LIST_LENGTH), "IPAddressList");
			}
			if (array.Length == 1)
			{
				return IPAddressList;
			}
			SocketAddress[] array2 = new SocketAddress[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Trim();
				if (array[i].Length == 0)
				{
					throw new ArgumentException(SR.GetString("dns_bad_ip_address"), "IPAddressList");
				}
				SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetworkV6, 28);
				SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAStringToAddress(array[i], AddressFamily.InterNetworkV6, IntPtr.Zero, socketAddress.m_Buffer, ref socketAddress.m_Size);
				if (socketError != SocketError.Success)
				{
					SocketAddress socketAddress2 = new SocketAddress(AddressFamily.InterNetwork, 16);
					socketError = UnsafeNclNativeMethods.OSSOCK.WSAStringToAddress(array[i], AddressFamily.InterNetwork, IntPtr.Zero, socketAddress2.m_Buffer, ref socketAddress2.m_Size);
					if (socketError != SocketError.Success)
					{
						throw new ArgumentException(SR.GetString("dns_bad_ip_address"), "IPAddressList");
					}
					IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 0);
					IPEndPoint ipendPoint2 = (IPEndPoint)ipendPoint.Create(socketAddress2);
					byte[] addressBytes = ipendPoint2.Address.GetAddressBytes();
					byte[] array3 = new byte[16];
					for (int j = 0; j < 10; j++)
					{
						array3[j] = 0;
					}
					array3[10] = byte.MaxValue;
					array3[11] = byte.MaxValue;
					array3[12] = addressBytes[0];
					array3[13] = addressBytes[1];
					array3[14] = addressBytes[2];
					array3[15] = addressBytes[3];
					IPAddress ipaddress = new IPAddress(array3);
					IPEndPoint ipendPoint3 = new IPEndPoint(ipaddress, ipendPoint2.Port);
					socketAddress = ipendPoint3.Serialize();
				}
				array2[i] = socketAddress;
			}
			int num = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.SOCKET_ADDRESS_LIST)) + (array2.Length - 1) * Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.SOCKET_ADDRESS));
			Dictionary<IntPtr, KeyValuePair<SocketAddress, string>> dictionary = new Dictionary<IntPtr, KeyValuePair<SocketAddress, string>>();
			GCHandle[] array4 = new GCHandle[array2.Length];
			for (int k = 0; k < array2.Length; k++)
			{
				array4[k] = GCHandle.Alloc(array2[k].m_Buffer, GCHandleType.Pinned);
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			string text;
			try
			{
				UnsafeNclNativeMethods.OSSOCK.SOCKET_ADDRESS_LIST* ptr = (UnsafeNclNativeMethods.OSSOCK.SOCKET_ADDRESS_LIST*)(void*)intPtr;
				ptr->iAddressCount = array2.Length;
				UnsafeNclNativeMethods.OSSOCK.SOCKET_ADDRESS* ptr2 = &ptr->Addresses;
				for (int l = 0; l < ptr->iAddressCount; l++)
				{
					ptr2[l].iSockaddrLength = 28;
					ptr2[l].lpSockAddr = array4[l].AddrOfPinnedObject();
					dictionary[ptr2[l].lpSockAddr] = new KeyValuePair<SocketAddress, string>(array2[l], array[l]);
				}
				Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
				int num2 = socket.IOControl((IOControlCode)((ulong)(-939524071)), intPtr, num, intPtr, num);
				StringBuilder stringBuilder = new StringBuilder();
				for (int m = 0; m < ptr->iAddressCount; m++)
				{
					IntPtr lpSockAddr = ptr2[m].lpSockAddr;
					stringBuilder.Append(dictionary[lpSockAddr].Value);
					if (m != ptr->iAddressCount - 1)
					{
						stringBuilder.Append(";");
					}
				}
				text = stringBuilder.ToString();
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				for (int n = 0; n < array4.Length; n++)
				{
					if (array4[n].IsAllocated)
					{
						array4[n].Free();
					}
				}
			}
			return text;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x000514F8 File Offset: 0x0004F6F8
		public bool isInNetEx(string ipAddress, string ipPrefix)
		{
			if (ipAddress == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.isResolvable()", "ipAddress" }));
				}
				throw new ArgumentNullException("ipAddress");
			}
			if (ipPrefix == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.isResolvable()", "ipPrefix" }));
				}
				throw new ArgumentNullException("ipPrefix");
			}
			IPAddress ipaddress;
			if (!IPAddress.TryParse(ipAddress, out ipaddress))
			{
				throw new FormatException(SR.GetString("dns_bad_ip_address"));
			}
			int num = ipPrefix.IndexOf("/");
			if (num < 0)
			{
				throw new FormatException(SR.GetString("net_bad_ip_address_prefix"));
			}
			string[] array = ipPrefix.Split(new char[] { '/' });
			if (array.Length != 2 || array[0] == null || array[0].Length == 0 || array[1] == null || array[1].Length == 0 || array[1].Length > 2)
			{
				throw new FormatException(SR.GetString("net_bad_ip_address_prefix"));
			}
			IPAddress ipaddress2;
			if (!IPAddress.TryParse(array[0], out ipaddress2))
			{
				throw new FormatException(SR.GetString("dns_bad_ip_address"));
			}
			int num2 = 0;
			if (!int.TryParse(array[1], out num2))
			{
				throw new FormatException(SR.GetString("net_bad_ip_address_prefix"));
			}
			if (ipaddress.AddressFamily != ipaddress2.AddressFamily)
			{
				throw new FormatException(SR.GetString("net_bad_ip_address_prefix"));
			}
			if ((ipaddress.AddressFamily == AddressFamily.InterNetworkV6 && (num2 < 1 || num2 > 64)) || (ipaddress.AddressFamily == AddressFamily.InterNetwork && (num2 < 1 || num2 > 32)))
			{
				throw new FormatException(SR.GetString("net_bad_ip_address_prefix"));
			}
			byte[] addressBytes = ipaddress2.GetAddressBytes();
			byte b = (byte)(num2 / 8);
			byte b2 = (byte)(num2 % 8);
			byte b3 = b;
			if (b2 != 0)
			{
				if ((255 & ((int)addressBytes[(int)b] << (int)b2)) != 0)
				{
					throw new FormatException(SR.GetString("net_bad_ip_address_prefix"));
				}
				b3 += 1;
			}
			int num3 = ((ipaddress2.AddressFamily == AddressFamily.InterNetworkV6) ? 16 : 4);
			while ((int)b3 < num3)
			{
				byte[] array2 = addressBytes;
				byte b4 = b3;
				b3 = b4 + 1;
				if (array2[(int)b4])
				{
					throw new FormatException(SR.GetString("net_bad_ip_address_prefix"));
				}
			}
			byte[] addressBytes2 = ipaddress.GetAddressBytes();
			for (b3 = 0; b3 < b; b3 += 1)
			{
				if (addressBytes2[(int)b3] != addressBytes[(int)b3])
				{
					return false;
				}
			}
			if (b2 > 0)
			{
				byte b5 = addressBytes2[(int)b];
				byte b6 = addressBytes[(int)b];
				b5 = (byte)(b5 >> (int)(8 - b2));
				b5 = (byte)(b5 << (int)(8 - b2));
				if (b5 != b6)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0005177C File Offset: 0x0004F97C
		public string myIpAddressEx()
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				IPAddress[] localAddresses = NclUtilities.LocalAddresses;
				for (int i = 0; i < localAddresses.Length; i++)
				{
					if (!IPAddress.IsLoopback(localAddresses[i]))
					{
						stringBuilder.Append(localAddresses[i].ToString());
						if (i != localAddresses.Length - 1)
						{
							stringBuilder.Append(";");
						}
					}
				}
			}
			catch
			{
			}
			if (stringBuilder.Length <= 0)
			{
				return string.Empty;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x000517FC File Offset: 0x0004F9FC
		public string dnsResolveEx(string host)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.dnsResolve()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			IPHostEntry iphostEntry = null;
			try
			{
				iphostEntry = Dns.InternalGetHostByName(host);
			}
			catch
			{
			}
			if (iphostEntry == null)
			{
				return string.Empty;
			}
			IPAddress[] addressList = iphostEntry.AddressList;
			if (addressList.Length == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < addressList.Length; i++)
			{
				stringBuilder.Append(addressList[i].ToString());
				if (i != addressList.Length - 1)
				{
					stringBuilder.Append(";");
				}
			}
			if (stringBuilder.Length <= 0)
			{
				return string.Empty;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x000518CC File Offset: 0x0004FACC
		public bool isResolvableEx(string host)
		{
			if (host == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_called_with_null_parameter", new object[] { "WebProxyScriptHelper.dnsResolve()", "host" }));
				}
				throw new ArgumentNullException("host");
			}
			IPHostEntry iphostEntry = null;
			try
			{
				iphostEntry = Dns.InternalGetHostByName(host);
			}
			catch
			{
			}
			if (iphostEntry == null)
			{
				return false;
			}
			IPAddress[] addressList = iphostEntry.AddressList;
			return addressList.Length != 0;
		}

		// Token: 0x040012C7 RID: 4807
		private static int MAX_IPADDRESS_LIST_LENGTH = 1024;

		// Token: 0x02000742 RID: 1858
		private class MyMethodInfo : MethodInfo
		{
			// Token: 0x060041D0 RID: 16848 RVA: 0x00111B7D File Offset: 0x0010FD7D
			public MyMethodInfo(string name)
			{
				this.name = name;
			}

			// Token: 0x17000F07 RID: 3847
			// (get) Token: 0x060041D1 RID: 16849 RVA: 0x00111B8C File Offset: 0x0010FD8C
			public override Type ReturnType
			{
				get
				{
					Type type = null;
					if (string.Compare(this.name, "isPlainHostName", StringComparison.Ordinal) == 0)
					{
						type = typeof(bool);
					}
					else if (string.Compare(this.name, "dnsDomainIs", StringComparison.Ordinal) == 0)
					{
						type = typeof(bool);
					}
					else if (string.Compare(this.name, "localHostOrDomainIs", StringComparison.Ordinal) == 0)
					{
						type = typeof(bool);
					}
					else if (string.Compare(this.name, "isResolvable", StringComparison.Ordinal) == 0)
					{
						type = typeof(bool);
					}
					else if (string.Compare(this.name, "dnsResolve", StringComparison.Ordinal) == 0)
					{
						type = typeof(string);
					}
					else if (string.Compare(this.name, "myIpAddress", StringComparison.Ordinal) == 0)
					{
						type = typeof(string);
					}
					else if (string.Compare(this.name, "dnsDomainLevels", StringComparison.Ordinal) == 0)
					{
						type = typeof(int);
					}
					else if (string.Compare(this.name, "isInNet", StringComparison.Ordinal) == 0)
					{
						type = typeof(bool);
					}
					else if (string.Compare(this.name, "shExpMatch", StringComparison.Ordinal) == 0)
					{
						type = typeof(bool);
					}
					else if (string.Compare(this.name, "weekdayRange", StringComparison.Ordinal) == 0)
					{
						type = typeof(bool);
					}
					else if (Socket.OSSupportsIPv6)
					{
						if (string.Compare(this.name, "dnsResolveEx", StringComparison.Ordinal) == 0)
						{
							type = typeof(string);
						}
						else if (string.Compare(this.name, "isResolvableEx", StringComparison.Ordinal) == 0)
						{
							type = typeof(bool);
						}
						else if (string.Compare(this.name, "myIpAddressEx", StringComparison.Ordinal) == 0)
						{
							type = typeof(string);
						}
						else if (string.Compare(this.name, "isInNetEx", StringComparison.Ordinal) == 0)
						{
							type = typeof(bool);
						}
						else if (string.Compare(this.name, "sortIpAddressList", StringComparison.Ordinal) == 0)
						{
							type = typeof(string);
						}
						else if (string.Compare(this.name, "getClientVersion", StringComparison.Ordinal) == 0)
						{
							type = typeof(string);
						}
					}
					return type;
				}
			}

			// Token: 0x17000F08 RID: 3848
			// (get) Token: 0x060041D2 RID: 16850 RVA: 0x00111DC5 File Offset: 0x0010FFC5
			public override ICustomAttributeProvider ReturnTypeCustomAttributes
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000F09 RID: 3849
			// (get) Token: 0x060041D3 RID: 16851 RVA: 0x00111DC8 File Offset: 0x0010FFC8
			public override RuntimeMethodHandle MethodHandle
			{
				get
				{
					return default(RuntimeMethodHandle);
				}
			}

			// Token: 0x17000F0A RID: 3850
			// (get) Token: 0x060041D4 RID: 16852 RVA: 0x00111DDE File Offset: 0x0010FFDE
			public override MethodAttributes Attributes
			{
				get
				{
					return MethodAttributes.Public;
				}
			}

			// Token: 0x17000F0B RID: 3851
			// (get) Token: 0x060041D5 RID: 16853 RVA: 0x00111DE1 File Offset: 0x0010FFE1
			public override string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x17000F0C RID: 3852
			// (get) Token: 0x060041D6 RID: 16854 RVA: 0x00111DE9 File Offset: 0x0010FFE9
			public override Type DeclaringType
			{
				get
				{
					return typeof(WebProxyScriptHelper.MyMethodInfo);
				}
			}

			// Token: 0x17000F0D RID: 3853
			// (get) Token: 0x060041D7 RID: 16855 RVA: 0x00111DF5 File Offset: 0x0010FFF5
			public override Type ReflectedType
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060041D8 RID: 16856 RVA: 0x00111DF8 File Offset: 0x0010FFF8
			public override object[] GetCustomAttributes(bool inherit)
			{
				return null;
			}

			// Token: 0x060041D9 RID: 16857 RVA: 0x00111DFB File Offset: 0x0010FFFB
			public override object[] GetCustomAttributes(Type type, bool inherit)
			{
				return null;
			}

			// Token: 0x060041DA RID: 16858 RVA: 0x00111DFE File Offset: 0x0010FFFE
			public override bool IsDefined(Type type, bool inherit)
			{
				return type.Equals(typeof(WebProxyScriptHelper));
			}

			// Token: 0x060041DB RID: 16859 RVA: 0x00111E10 File Offset: 0x00110010
			public override object Invoke(object target, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
			{
				return typeof(WebProxyScriptHelper).GetMethod(this.name, (BindingFlags)(-1)).Invoke(target, (BindingFlags)(-1), binder, args, culture);
			}

			// Token: 0x060041DC RID: 16860 RVA: 0x00111E34 File Offset: 0x00110034
			public override ParameterInfo[] GetParameters()
			{
				return typeof(WebProxyScriptHelper).GetMethod(this.name, (BindingFlags)(-1)).GetParameters();
			}

			// Token: 0x060041DD RID: 16861 RVA: 0x00111E5E File Offset: 0x0011005E
			public override MethodImplAttributes GetMethodImplementationFlags()
			{
				return MethodImplAttributes.IL;
			}

			// Token: 0x060041DE RID: 16862 RVA: 0x00111E61 File Offset: 0x00110061
			public override MethodInfo GetBaseDefinition()
			{
				return null;
			}

			// Token: 0x17000F0E RID: 3854
			// (get) Token: 0x060041DF RID: 16863 RVA: 0x00111E64 File Offset: 0x00110064
			public override Module Module
			{
				get
				{
					return base.GetType().Module;
				}
			}

			// Token: 0x040031C5 RID: 12741
			private string name;
		}
	}
}
