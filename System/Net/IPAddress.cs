using System;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Net
{
	// Token: 0x0200014B RID: 331
	[global::__DynamicallyInvokable]
	[Serializable]
	public class IPAddress
	{
		// Token: 0x06000B6F RID: 2927 RVA: 0x0003E635 File Offset: 0x0003C835
		[global::__DynamicallyInvokable]
		public IPAddress(long newAddress)
		{
			if (newAddress < 0L || newAddress > (long)((ulong)(-1)))
			{
				throw new ArgumentOutOfRangeException("newAddress");
			}
			this.m_Address = newAddress;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0003E66C File Offset: 0x0003C86C
		[global::__DynamicallyInvokable]
		public IPAddress(byte[] address, long scopeid)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Length != 16)
			{
				throw new ArgumentException(SR.GetString("dns_bad_ip_address"), "address");
			}
			this.m_Family = AddressFamily.InterNetworkV6;
			for (int i = 0; i < 8; i++)
			{
				this.m_Numbers[i] = (ushort)((int)address[i * 2] * 256 + (int)address[i * 2 + 1]);
			}
			if (scopeid < 0L || scopeid > (long)((ulong)(-1)))
			{
				throw new ArgumentOutOfRangeException("scopeid");
			}
			this.m_ScopeId = scopeid;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0003E708 File Offset: 0x0003C908
		private IPAddress(ushort[] address, uint scopeid)
		{
			this.m_Family = AddressFamily.InterNetworkV6;
			this.m_Numbers = address;
			this.m_ScopeId = (long)((ulong)scopeid);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0003E73C File Offset: 0x0003C93C
		[global::__DynamicallyInvokable]
		public IPAddress(byte[] address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Length != 4 && address.Length != 16)
			{
				throw new ArgumentException(SR.GetString("dns_bad_ip_address"), "address");
			}
			if (address.Length == 4)
			{
				this.m_Family = AddressFamily.InterNetwork;
				this.m_Address = (long)(((int)address[3] << 24) | ((int)address[2] << 16) | ((int)address[1] << 8) | (int)address[0]) & (long)((ulong)(-1));
				return;
			}
			this.m_Family = AddressFamily.InterNetworkV6;
			for (int i = 0; i < 8; i++)
			{
				this.m_Numbers[i] = (ushort)((int)address[i * 2] * 256 + (int)address[i * 2 + 1]);
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0003E7F1 File Offset: 0x0003C9F1
		internal IPAddress(int newAddress)
		{
			this.m_Address = (long)newAddress & (long)((ulong)(-1));
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0003E817 File Offset: 0x0003CA17
		[global::__DynamicallyInvokable]
		public static bool TryParse(string ipString, out IPAddress address)
		{
			address = IPAddress.InternalParse(ipString, true);
			return address != null;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0003E827 File Offset: 0x0003CA27
		[global::__DynamicallyInvokable]
		public static IPAddress Parse(string ipString)
		{
			return IPAddress.InternalParse(ipString, false);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0003E830 File Offset: 0x0003CA30
		private unsafe static IPAddress InternalParse(string ipString, bool tryParse)
		{
			if (ipString == null)
			{
				if (tryParse)
				{
					return null;
				}
				throw new ArgumentNullException("ipString");
			}
			else
			{
				if (ipString.IndexOf(':') != -1)
				{
					SocketException ex;
					if (Socket.OSSupportsIPv6)
					{
						byte[] array = new byte[16];
						SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetworkV6, 28);
						if (UnsafeNclNativeMethods.OSSOCK.WSAStringToAddress(ipString, AddressFamily.InterNetworkV6, IntPtr.Zero, socketAddress.m_Buffer, ref socketAddress.m_Size) == SocketError.Success)
						{
							for (int i = 0; i < 16; i++)
							{
								array[i] = socketAddress[i + 8];
							}
							long num = (long)(((int)socketAddress[27] << 24) + ((int)socketAddress[26] << 16) + ((int)socketAddress[25] << 8) + (int)socketAddress[24]);
							return new IPAddress(array, num);
						}
						if (tryParse)
						{
							return null;
						}
						ex = new SocketException();
					}
					else
					{
						int num2 = 0;
						if (ipString[0] != '[')
						{
							ipString += "]";
						}
						else
						{
							num2 = 1;
						}
						int length = ipString.Length;
						fixed (string text = ipString)
						{
							char* ptr = text;
							if (ptr != null)
							{
								ptr += RuntimeHelpers.OffsetToStringData / 2;
							}
							if (IPv6AddressHelper.IsValidStrict(ptr, num2, ref length) || length != ipString.Length)
							{
								ushort[] array2 = new ushort[8];
								string text2 = null;
								ushort[] array3;
								ushort* ptr2;
								if ((array3 = array2) == null || array3.Length == 0)
								{
									ptr2 = null;
								}
								else
								{
									ptr2 = &array3[0];
								}
								IPv6AddressHelper.Parse(ipString, ptr2, 0, ref text2);
								array3 = null;
								if (text2 == null || text2.Length == 0)
								{
									return new IPAddress(array2, 0U);
								}
								text2 = text2.Substring(1);
								uint num3;
								if (uint.TryParse(text2, NumberStyles.None, null, out num3))
								{
									return new IPAddress(array2, num3);
								}
							}
						}
						if (tryParse)
						{
							return null;
						}
						ex = new SocketException(SocketError.InvalidArgument);
					}
					throw new FormatException(SR.GetString("dns_bad_ip_address"), ex);
				}
				Socket.InitializeSockets();
				int length2 = ipString.Length;
				long num4;
				fixed (string text3 = ipString)
				{
					char* ptr3 = text3;
					if (ptr3 != null)
					{
						ptr3 += RuntimeHelpers.OffsetToStringData / 2;
					}
					num4 = IPv4AddressHelper.ParseNonCanonical(ptr3, 0, ref length2, true);
				}
				if (num4 != -1L && length2 == ipString.Length)
				{
					num4 = ((num4 & 255L) << 24) | (((num4 & 65280L) << 8) | (((num4 & 16711680L) >> 8) | ((num4 & (long)((ulong)(-16777216))) >> 24)));
					return new IPAddress(num4);
				}
				if (tryParse)
				{
					return null;
				}
				throw new FormatException(SR.GetString("dns_bad_ip_address"));
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0003EA7C File Offset: 0x0003CC7C
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x0003EA99 File Offset: 0x0003CC99
		[Obsolete("This property has been deprecated. It is address family dependent. Please use IPAddress.Equals method to perform comparisons. http://go.microsoft.com/fwlink/?linkid=14202")]
		public long Address
		{
			get
			{
				if (this.m_Family == AddressFamily.InterNetworkV6)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				return this.m_Address;
			}
			set
			{
				if (this.m_Family == AddressFamily.InterNetworkV6)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				if (this.m_Address != value)
				{
					this.m_ToString = null;
					this.m_Address = value;
				}
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0003EAC8 File Offset: 0x0003CCC8
		[global::__DynamicallyInvokable]
		public byte[] GetAddressBytes()
		{
			byte[] array;
			if (this.m_Family == AddressFamily.InterNetworkV6)
			{
				array = new byte[16];
				int num = 0;
				for (int i = 0; i < 8; i++)
				{
					array[num++] = (byte)((this.m_Numbers[i] >> 8) & 255);
					array[num++] = (byte)(this.m_Numbers[i] & 255);
				}
			}
			else
			{
				array = new byte[]
				{
					(byte)this.m_Address,
					(byte)(this.m_Address >> 8),
					(byte)(this.m_Address >> 16),
					(byte)(this.m_Address >> 24)
				};
			}
			return array;
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0003EB5D File Offset: 0x0003CD5D
		[global::__DynamicallyInvokable]
		public AddressFamily AddressFamily
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Family;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x0003EB65 File Offset: 0x0003CD65
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x0003EB84 File Offset: 0x0003CD84
		[global::__DynamicallyInvokable]
		public long ScopeId
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.m_Family == AddressFamily.InterNetwork)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				return this.m_ScopeId;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (this.m_Family == AddressFamily.InterNetwork)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				if (value < 0L || value > (long)((ulong)(-1)))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.m_ScopeId != value)
				{
					this.m_Address = value;
					this.m_ScopeId = value;
					this.m_ToString = null;
				}
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0003EBD8 File Offset: 0x0003CDD8
		[global::__DynamicallyInvokable]
		public unsafe override string ToString()
		{
			if (this.m_ToString == null)
			{
				if (this.m_Family == AddressFamily.InterNetworkV6)
				{
					int num = 256;
					StringBuilder stringBuilder = new StringBuilder(num);
					if (Socket.OSSupportsIPv6)
					{
						SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetworkV6, 28);
						int num2 = 8;
						for (int i = 0; i < 8; i++)
						{
							socketAddress[num2++] = (byte)(this.m_Numbers[i] >> 8);
							socketAddress[num2++] = (byte)this.m_Numbers[i];
						}
						if (this.m_ScopeId > 0L)
						{
							socketAddress[24] = (byte)this.m_ScopeId;
							socketAddress[25] = (byte)(this.m_ScopeId >> 8);
							socketAddress[26] = (byte)(this.m_ScopeId >> 16);
							socketAddress[27] = (byte)(this.m_ScopeId >> 24);
						}
						SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAAddressToString(socketAddress.m_Buffer, socketAddress.m_Size, IntPtr.Zero, stringBuilder, ref num);
						if (socketError != SocketError.Success)
						{
							throw new SocketException();
						}
					}
					else
					{
						string text = string.Format(CultureInfo.InvariantCulture, "{0:x4}:{1:x4}:{2:x4}:{3:x4}:{4:x4}:{5:x4}:{6}.{7}.{8}.{9}", new object[]
						{
							this.m_Numbers[0],
							this.m_Numbers[1],
							this.m_Numbers[2],
							this.m_Numbers[3],
							this.m_Numbers[4],
							this.m_Numbers[5],
							(this.m_Numbers[6] >> 8) & 255,
							(int)(this.m_Numbers[6] & 255),
							(this.m_Numbers[7] >> 8) & 255,
							(int)(this.m_Numbers[7] & 255)
						});
						stringBuilder.Append(text);
						if (this.m_ScopeId != 0L)
						{
							stringBuilder.Append('%').Append((uint)this.m_ScopeId);
						}
					}
					this.m_ToString = stringBuilder.ToString();
				}
				else
				{
					int num3 = 15;
					char* ptr = stackalloc char[(UIntPtr)30];
					int num4 = (int)((this.m_Address >> 24) & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					ptr[(IntPtr)(--num3) * 2] = '.';
					num4 = (int)((this.m_Address >> 16) & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					ptr[(IntPtr)(--num3) * 2] = '.';
					num4 = (int)((this.m_Address >> 8) & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					ptr[(IntPtr)(--num3) * 2] = '.';
					num4 = (int)(this.m_Address & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					this.m_ToString = new string(ptr, num3, 15 - num3);
				}
			}
			return this.m_ToString;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0003EF07 File Offset: 0x0003D107
		[global::__DynamicallyInvokable]
		public static long HostToNetworkOrder(long host)
		{
			return (((long)IPAddress.HostToNetworkOrder((int)host) & (long)((ulong)(-1))) << 32) | ((long)IPAddress.HostToNetworkOrder((int)(host >> 32)) & (long)((ulong)(-1)));
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0003EF26 File Offset: 0x0003D126
		[global::__DynamicallyInvokable]
		public static int HostToNetworkOrder(int host)
		{
			return (((int)IPAddress.HostToNetworkOrder((short)host) & 65535) << 16) | ((int)IPAddress.HostToNetworkOrder((short)(host >> 16)) & 65535);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0003EF49 File Offset: 0x0003D149
		[global::__DynamicallyInvokable]
		public static short HostToNetworkOrder(short host)
		{
			return (short)(((int)(host & 255) << 8) | ((host >> 8) & 255));
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0003EF5F File Offset: 0x0003D15F
		[global::__DynamicallyInvokable]
		public static long NetworkToHostOrder(long network)
		{
			return IPAddress.HostToNetworkOrder(network);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0003EF67 File Offset: 0x0003D167
		[global::__DynamicallyInvokable]
		public static int NetworkToHostOrder(int network)
		{
			return IPAddress.HostToNetworkOrder(network);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0003EF6F File Offset: 0x0003D16F
		[global::__DynamicallyInvokable]
		public static short NetworkToHostOrder(short network)
		{
			return IPAddress.HostToNetworkOrder(network);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0003EF78 File Offset: 0x0003D178
		[global::__DynamicallyInvokable]
		public static bool IsLoopback(IPAddress address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.m_Family == AddressFamily.InterNetworkV6)
			{
				return address.Equals(IPAddress.IPv6Loopback);
			}
			return (address.m_Address & 255L) == (IPAddress.Loopback.m_Address & 255L);
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0003EFC9 File Offset: 0x0003D1C9
		internal bool IsBroadcast
		{
			get
			{
				return this.m_Family != AddressFamily.InterNetworkV6 && this.m_Address == IPAddress.Broadcast.m_Address;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0003EFE9 File Offset: 0x0003D1E9
		[global::__DynamicallyInvokable]
		public bool IsIPv6Multicast
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Family == AddressFamily.InterNetworkV6 && (this.m_Numbers[0] & 65280) == 65280;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0003F00C File Offset: 0x0003D20C
		[global::__DynamicallyInvokable]
		public bool IsIPv6LinkLocal
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Family == AddressFamily.InterNetworkV6 && (this.m_Numbers[0] & 65472) == 65152;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0003F02F File Offset: 0x0003D22F
		[global::__DynamicallyInvokable]
		public bool IsIPv6SiteLocal
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Family == AddressFamily.InterNetworkV6 && (this.m_Numbers[0] & 65472) == 65216;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0003F052 File Offset: 0x0003D252
		[global::__DynamicallyInvokable]
		public bool IsIPv6Teredo
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Family == AddressFamily.InterNetworkV6 && this.m_Numbers[0] == 8193 && this.m_Numbers[1] == 0;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0003F07C File Offset: 0x0003D27C
		[global::__DynamicallyInvokable]
		public bool IsIPv4MappedToIPv6
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.AddressFamily != AddressFamily.InterNetworkV6)
				{
					return false;
				}
				for (int i = 0; i < 5; i++)
				{
					if (this.m_Numbers[i] != 0)
					{
						return false;
					}
				}
				return this.m_Numbers[5] == ushort.MaxValue;
			}
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0003F0BC File Offset: 0x0003D2BC
		internal bool Equals(object comparandObj, bool compareScopeId)
		{
			IPAddress ipaddress = comparandObj as IPAddress;
			if (ipaddress == null)
			{
				return false;
			}
			if (this.m_Family != ipaddress.m_Family)
			{
				return false;
			}
			if (this.m_Family == AddressFamily.InterNetworkV6)
			{
				for (int i = 0; i < 8; i++)
				{
					if (ipaddress.m_Numbers[i] != this.m_Numbers[i])
					{
						return false;
					}
				}
				return ipaddress.m_ScopeId == this.m_ScopeId || !compareScopeId;
			}
			return ipaddress.m_Address == this.m_Address;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0003F134 File Offset: 0x0003D334
		[global::__DynamicallyInvokable]
		public override bool Equals(object comparand)
		{
			return this.Equals(comparand, true);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0003F13E File Offset: 0x0003D33E
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			if (this.m_Family == AddressFamily.InterNetworkV6)
			{
				if (this.m_HashCode == 0)
				{
					this.m_HashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.ToString());
				}
				return this.m_HashCode;
			}
			return (int)this.m_Address;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0003F178 File Offset: 0x0003D378
		internal IPAddress Snapshot()
		{
			AddressFamily family = this.m_Family;
			if (family == AddressFamily.InterNetwork)
			{
				return new IPAddress(this.m_Address);
			}
			if (family != AddressFamily.InterNetworkV6)
			{
				throw new InternalException();
			}
			return new IPAddress(this.m_Numbers, (uint)this.m_ScopeId);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0003F1BC File Offset: 0x0003D3BC
		[global::__DynamicallyInvokable]
		public IPAddress MapToIPv6()
		{
			if (this.AddressFamily == AddressFamily.InterNetworkV6)
			{
				return this;
			}
			return new IPAddress(new ushort[]
			{
				0,
				0,
				0,
				0,
				0,
				ushort.MaxValue,
				(ushort)(((this.m_Address & 65280L) >> 8) | ((this.m_Address & 255L) << 8)),
				(ushort)(((this.m_Address & (long)((ulong)(-16777216))) >> 24) | ((this.m_Address & 16711680L) >> 8))
			}, 0U);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0003F234 File Offset: 0x0003D434
		[global::__DynamicallyInvokable]
		public IPAddress MapToIPv4()
		{
			if (this.AddressFamily == AddressFamily.InterNetwork)
			{
				return this;
			}
			long num = (long)((ulong)(((uint)(this.m_Numbers[6] & 65280) >> 8) | (uint)((uint)(this.m_Numbers[6] & 255) << 8) | ((((uint)(this.m_Numbers[7] & 65280) >> 8) | (uint)((uint)(this.m_Numbers[7] & 255) << 8)) << 16)));
			return new IPAddress(num);
		}

		// Token: 0x040010F0 RID: 4336
		[global::__DynamicallyInvokable]
		public static readonly IPAddress Any = new IPAddress(0);

		// Token: 0x040010F1 RID: 4337
		[global::__DynamicallyInvokable]
		public static readonly IPAddress Loopback = new IPAddress(16777343);

		// Token: 0x040010F2 RID: 4338
		[global::__DynamicallyInvokable]
		public static readonly IPAddress Broadcast = new IPAddress((long)((ulong)(-1)));

		// Token: 0x040010F3 RID: 4339
		[global::__DynamicallyInvokable]
		public static readonly IPAddress None = IPAddress.Broadcast;

		// Token: 0x040010F4 RID: 4340
		internal const long LoopbackMask = 255L;

		// Token: 0x040010F5 RID: 4341
		internal long m_Address;

		// Token: 0x040010F6 RID: 4342
		[NonSerialized]
		internal string m_ToString;

		// Token: 0x040010F7 RID: 4343
		[global::__DynamicallyInvokable]
		public static readonly IPAddress IPv6Any = new IPAddress(new byte[16], 0L);

		// Token: 0x040010F8 RID: 4344
		[global::__DynamicallyInvokable]
		public static readonly IPAddress IPv6Loopback = new IPAddress(new byte[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 1
		}, 0L);

		// Token: 0x040010F9 RID: 4345
		[global::__DynamicallyInvokable]
		public static readonly IPAddress IPv6None = new IPAddress(new byte[16], 0L);

		// Token: 0x040010FA RID: 4346
		private AddressFamily m_Family = AddressFamily.InterNetwork;

		// Token: 0x040010FB RID: 4347
		private ushort[] m_Numbers = new ushort[8];

		// Token: 0x040010FC RID: 4348
		private long m_ScopeId;

		// Token: 0x040010FD RID: 4349
		private int m_HashCode;

		// Token: 0x040010FE RID: 4350
		internal const int IPv4AddressBytes = 4;

		// Token: 0x040010FF RID: 4351
		internal const int IPv6AddressBytes = 16;

		// Token: 0x04001100 RID: 4352
		internal const int NumberOfLabels = 8;
	}
}
