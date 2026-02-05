using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x020000F9 RID: 249
	[Serializable]
	public class HttpListenerException : Win32Exception
	{
		// Token: 0x060008F9 RID: 2297 RVA: 0x00032AE8 File Offset: 0x00030CE8
		public HttpListenerException()
			: base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00032AF5 File Offset: 0x00030CF5
		public HttpListenerException(int errorCode)
			: base(errorCode)
		{
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00032AFE File Offset: 0x00030CFE
		public HttpListenerException(int errorCode, string message)
			: base(errorCode, message)
		{
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00032B08 File Offset: 0x00030D08
		protected HttpListenerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00032B12 File Offset: 0x00030D12
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}
	}
}
