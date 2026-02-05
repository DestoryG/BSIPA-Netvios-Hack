using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.Sockets
{
	// Token: 0x02000364 RID: 868
	[global::__DynamicallyInvokable]
	[Serializable]
	public class SocketException : Win32Exception
	{
		// Token: 0x06001FC3 RID: 8131 RVA: 0x00094CDF File Offset: 0x00092EDF
		[global::__DynamicallyInvokable]
		public SocketException()
			: base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x00094CEC File Offset: 0x00092EEC
		internal SocketException(EndPoint endPoint)
			: base(Marshal.GetLastWin32Error())
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x00094D00 File Offset: 0x00092F00
		[global::__DynamicallyInvokable]
		public SocketException(int errorCode)
			: base(errorCode)
		{
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x00094D09 File Offset: 0x00092F09
		internal SocketException(int errorCode, EndPoint endPoint)
			: base(errorCode)
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00094D19 File Offset: 0x00092F19
		internal SocketException(SocketError socketError)
			: base((int)socketError)
		{
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x00094D22 File Offset: 0x00092F22
		protected SocketException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x00094D2C File Offset: 0x00092F2C
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x00094D34 File Offset: 0x00092F34
		[global::__DynamicallyInvokable]
		public override string Message
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.m_EndPoint == null)
				{
					return base.Message;
				}
				return base.Message + " " + this.m_EndPoint.ToString();
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x00094D60 File Offset: 0x00092F60
		[global::__DynamicallyInvokable]
		public SocketError SocketErrorCode
		{
			[global::__DynamicallyInvokable]
			get
			{
				return (SocketError)base.NativeErrorCode;
			}
		}

		// Token: 0x04001D6D RID: 7533
		[NonSerialized]
		private EndPoint m_EndPoint;
	}
}
