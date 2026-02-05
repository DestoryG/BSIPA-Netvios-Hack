using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace System.Net
{
	// Token: 0x02000161 RID: 353
	[global::__DynamicallyInvokable]
	public class SocketAddress
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00043BAC File Offset: 0x00041DAC
		[global::__DynamicallyInvokable]
		public AddressFamily Family
		{
			[global::__DynamicallyInvokable]
			get
			{
				return (AddressFamily)((int)this.m_Buffer[0] | ((int)this.m_Buffer[1] << 8));
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00043BCE File Offset: 0x00041DCE
		[global::__DynamicallyInvokable]
		public int Size
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_Size;
			}
		}

		// Token: 0x170002F2 RID: 754
		[global::__DynamicallyInvokable]
		public byte this[int offset]
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (offset < 0 || offset >= this.Size)
				{
					throw new IndexOutOfRangeException();
				}
				return this.m_Buffer[offset];
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (offset < 0 || offset >= this.Size)
				{
					throw new IndexOutOfRangeException();
				}
				if (this.m_Buffer[offset] != value)
				{
					this.m_changed = true;
				}
				this.m_Buffer[offset] = value;
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00043C23 File Offset: 0x00041E23
		[global::__DynamicallyInvokable]
		public SocketAddress(AddressFamily family)
			: this(family, 32)
		{
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00043C30 File Offset: 0x00041E30
		[global::__DynamicallyInvokable]
		public SocketAddress(AddressFamily family, int size)
		{
			if (size < 2)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			this.m_Size = size;
			this.m_Buffer = new byte[(size / IntPtr.Size + 2) * IntPtr.Size];
			this.m_Buffer[0] = (byte)family;
			this.m_Buffer[1] = (byte)(family >> 8);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00043C90 File Offset: 0x00041E90
		internal SocketAddress(IPAddress ipAddress)
			: this(ipAddress.AddressFamily, (ipAddress.AddressFamily == AddressFamily.InterNetwork) ? 16 : 28)
		{
			this.m_Buffer[2] = 0;
			this.m_Buffer[3] = 0;
			if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
			{
				this.m_Buffer[4] = 0;
				this.m_Buffer[5] = 0;
				this.m_Buffer[6] = 0;
				this.m_Buffer[7] = 0;
				long scopeId = ipAddress.ScopeId;
				this.m_Buffer[24] = (byte)scopeId;
				this.m_Buffer[25] = (byte)(scopeId >> 8);
				this.m_Buffer[26] = (byte)(scopeId >> 16);
				this.m_Buffer[27] = (byte)(scopeId >> 24);
				byte[] addressBytes = ipAddress.GetAddressBytes();
				for (int i = 0; i < addressBytes.Length; i++)
				{
					this.m_Buffer[8 + i] = addressBytes[i];
				}
				return;
			}
			this.m_Buffer[4] = (byte)ipAddress.m_Address;
			this.m_Buffer[5] = (byte)(ipAddress.m_Address >> 8);
			this.m_Buffer[6] = (byte)(ipAddress.m_Address >> 16);
			this.m_Buffer[7] = (byte)(ipAddress.m_Address >> 24);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00043D9D File Offset: 0x00041F9D
		internal SocketAddress(IPAddress ipaddress, int port)
			: this(ipaddress)
		{
			this.m_Buffer[2] = (byte)(port >> 8);
			this.m_Buffer[3] = (byte)port;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00043DBC File Offset: 0x00041FBC
		internal IPAddress GetIPAddress()
		{
			if (this.Family == AddressFamily.InterNetworkV6)
			{
				byte[] array = new byte[16];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.m_Buffer[i + 8];
				}
				long num = (long)(((int)this.m_Buffer[27] << 24) + ((int)this.m_Buffer[26] << 16) + ((int)this.m_Buffer[25] << 8) + (int)this.m_Buffer[24]);
				return new IPAddress(array, num);
			}
			if (this.Family == AddressFamily.InterNetwork)
			{
				long num2 = (long)((int)(this.m_Buffer[4] & byte.MaxValue) | (((int)this.m_Buffer[5] << 8) & 65280) | (((int)this.m_Buffer[6] << 16) & 16711680) | ((int)this.m_Buffer[7] << 24)) & (long)((ulong)(-1));
				return new IPAddress(num2);
			}
			throw new SocketException(SocketError.AddressFamilyNotSupported);
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00043E8C File Offset: 0x0004208C
		internal IPEndPoint GetIPEndPoint()
		{
			IPAddress ipaddress = this.GetIPAddress();
			int num = (((int)this.m_Buffer[2] << 8) & 65280) | (int)this.m_Buffer[3];
			return new IPEndPoint(ipaddress, num);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00043EC4 File Offset: 0x000420C4
		internal void CopyAddressSizeIntoBuffer()
		{
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size] = (byte)this.m_Size;
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 1] = (byte)(this.m_Size >> 8);
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 2] = (byte)(this.m_Size >> 16);
			this.m_Buffer[this.m_Buffer.Length - IntPtr.Size + 3] = (byte)(this.m_Size >> 24);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00043F4F File Offset: 0x0004214F
		internal int GetAddressSizeOffset()
		{
			return this.m_Buffer.Length - IntPtr.Size;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00043F5F File Offset: 0x0004215F
		internal unsafe void SetSize(IntPtr ptr)
		{
			this.m_Size = *(int*)(void*)ptr;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00043F70 File Offset: 0x00042170
		[global::__DynamicallyInvokable]
		public override bool Equals(object comparand)
		{
			SocketAddress socketAddress = comparand as SocketAddress;
			if (socketAddress == null || this.Size != socketAddress.Size)
			{
				return false;
			}
			for (int i = 0; i < this.Size; i++)
			{
				if (this[i] != socketAddress[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00043FBC File Offset: 0x000421BC
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			if (this.m_changed)
			{
				this.m_changed = false;
				this.m_hash = 0;
				int num = this.Size & -4;
				int i;
				for (i = 0; i < num; i += 4)
				{
					this.m_hash ^= (int)this.m_Buffer[i] | ((int)this.m_Buffer[i + 1] << 8) | ((int)this.m_Buffer[i + 2] << 16) | ((int)this.m_Buffer[i + 3] << 24);
				}
				if ((this.Size & 3) != 0)
				{
					int num2 = 0;
					int num3 = 0;
					while (i < this.Size)
					{
						num2 |= (int)this.m_Buffer[i] << num3;
						num3 += 8;
						i++;
					}
					this.m_hash ^= num2;
				}
			}
			return this.m_hash;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0004407C File Offset: 0x0004227C
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 2; i < this.Size; i++)
			{
				if (i > 2)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(this[i].ToString(NumberFormatInfo.InvariantInfo));
			}
			return string.Concat(new string[]
			{
				this.Family.ToString(),
				":",
				this.Size.ToString(NumberFormatInfo.InvariantInfo),
				":{",
				stringBuilder.ToString(),
				"}"
			});
		}

		// Token: 0x040011B0 RID: 4528
		internal const int IPv6AddressSize = 28;

		// Token: 0x040011B1 RID: 4529
		internal const int IPv4AddressSize = 16;

		// Token: 0x040011B2 RID: 4530
		internal int m_Size;

		// Token: 0x040011B3 RID: 4531
		internal byte[] m_Buffer;

		// Token: 0x040011B4 RID: 4532
		private const int WriteableOffset = 2;

		// Token: 0x040011B5 RID: 4533
		private const int MaxSize = 32;

		// Token: 0x040011B6 RID: 4534
		private bool m_changed = true;

		// Token: 0x040011B7 RID: 4535
		private int m_hash;
	}
}
