using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000401 RID: 1025
	[Serializable]
	public class InternalBufferOverflowException : SystemException
	{
		// Token: 0x06002694 RID: 9876 RVA: 0x000B19C8 File Offset: 0x000AFBC8
		public InternalBufferOverflowException()
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000B19DB File Offset: 0x000AFBDB
		public InternalBufferOverflowException(string message)
			: base(message)
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000B19EF File Offset: 0x000AFBEF
		public InternalBufferOverflowException(string message, Exception inner)
			: base(message, inner)
		{
			base.HResult = -2146232059;
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000B1A04 File Offset: 0x000AFC04
		protected InternalBufferOverflowException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
