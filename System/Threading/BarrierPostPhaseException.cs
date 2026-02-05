using System;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading
{
	// Token: 0x020003D5 RID: 981
	[global::__DynamicallyInvokable]
	[Serializable]
	public class BarrierPostPhaseException : Exception
	{
		// Token: 0x060025BA RID: 9658 RVA: 0x000AF438 File Offset: 0x000AD638
		[global::__DynamicallyInvokable]
		public BarrierPostPhaseException()
			: this(null)
		{
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x000AF441 File Offset: 0x000AD641
		[global::__DynamicallyInvokable]
		public BarrierPostPhaseException(Exception innerException)
			: this(null, innerException)
		{
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000AF44B File Offset: 0x000AD64B
		[global::__DynamicallyInvokable]
		public BarrierPostPhaseException(string message)
			: this(message, null)
		{
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000AF455 File Offset: 0x000AD655
		[global::__DynamicallyInvokable]
		public BarrierPostPhaseException(string message, Exception innerException)
			: base((message == null) ? SR.GetString("BarrierPostPhaseException") : message, innerException)
		{
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000AF46E File Offset: 0x000AD66E
		[SecurityCritical]
		protected BarrierPostPhaseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
