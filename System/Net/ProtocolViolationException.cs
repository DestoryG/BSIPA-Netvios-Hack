using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x02000156 RID: 342
	[global::__DynamicallyInvokable]
	[Serializable]
	public class ProtocolViolationException : InvalidOperationException, ISerializable
	{
		// Token: 0x06000C08 RID: 3080 RVA: 0x00041014 File Offset: 0x0003F214
		[global::__DynamicallyInvokable]
		public ProtocolViolationException()
		{
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0004101C File Offset: 0x0003F21C
		[global::__DynamicallyInvokable]
		public ProtocolViolationException(string message)
			: base(message)
		{
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00041025 File Offset: 0x0003F225
		protected ProtocolViolationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0004102F File Offset: 0x0003F22F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00041039 File Offset: 0x0003F239
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
