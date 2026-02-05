using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020000DA RID: 218
	[global::__DynamicallyInvokable]
	[Serializable]
	public class CookieException : FormatException, ISerializable
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x000297CA File Offset: 0x000279CA
		[global::__DynamicallyInvokable]
		public CookieException()
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000297D2 File Offset: 0x000279D2
		internal CookieException(string message)
			: base(message)
		{
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000297DB File Offset: 0x000279DB
		internal CookieException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000297E5 File Offset: 0x000279E5
		protected CookieException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000297EF File Offset: 0x000279EF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x000297F9 File Offset: 0x000279F9
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
