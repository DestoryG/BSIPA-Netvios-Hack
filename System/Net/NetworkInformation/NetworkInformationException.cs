using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002DF RID: 735
	[global::__DynamicallyInvokable]
	[Serializable]
	public class NetworkInformationException : Win32Exception
	{
		// Token: 0x060019E7 RID: 6631 RVA: 0x0007E1DD File Offset: 0x0007C3DD
		[global::__DynamicallyInvokable]
		public NetworkInformationException()
			: base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0007E1EA File Offset: 0x0007C3EA
		[global::__DynamicallyInvokable]
		public NetworkInformationException(int errorCode)
			: base(errorCode)
		{
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0007E1F3 File Offset: 0x0007C3F3
		internal NetworkInformationException(SocketError socketError)
			: base((int)socketError)
		{
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x0007E1FC File Offset: 0x0007C3FC
		protected NetworkInformationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x0007E206 File Offset: 0x0007C406
		[global::__DynamicallyInvokable]
		public override int ErrorCode
		{
			[global::__DynamicallyInvokable]
			get
			{
				return base.NativeErrorCode;
			}
		}
	}
}
