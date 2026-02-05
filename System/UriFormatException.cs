using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200003F RID: 63
	[global::__DynamicallyInvokable]
	[Serializable]
	public class UriFormatException : FormatException, ISerializable
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x0001A90C File Offset: 0x00018B0C
		[global::__DynamicallyInvokable]
		public UriFormatException()
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001A914 File Offset: 0x00018B14
		[global::__DynamicallyInvokable]
		public UriFormatException(string textString)
			: base(textString)
		{
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001A91D File Offset: 0x00018B1D
		[global::__DynamicallyInvokable]
		public UriFormatException(string textString, Exception e)
			: base(textString, e)
		{
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001A927 File Offset: 0x00018B27
		protected UriFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001A931 File Offset: 0x00018B31
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
