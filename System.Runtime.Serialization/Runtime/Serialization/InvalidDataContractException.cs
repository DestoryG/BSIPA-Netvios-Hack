using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000097 RID: 151
	[Serializable]
	public class InvalidDataContractException : Exception
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x0002CBDF File Offset: 0x0002ADDF
		public InvalidDataContractException()
		{
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0002CBE7 File Offset: 0x0002ADE7
		public InvalidDataContractException(string message)
			: base(message)
		{
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0002CBF0 File Offset: 0x0002ADF0
		public InvalidDataContractException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0002CBFA File Offset: 0x0002ADFA
		protected InvalidDataContractException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
