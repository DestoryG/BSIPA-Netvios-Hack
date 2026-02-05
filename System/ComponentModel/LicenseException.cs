using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200057E RID: 1406
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class LicenseException : SystemException
	{
		// Token: 0x060033F6 RID: 13302 RVA: 0x000E3F34 File Offset: 0x000E2134
		public LicenseException(Type type)
			: this(type, null, SR.GetString("LicExceptionTypeOnly", new object[] { type.FullName }))
		{
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000E3F57 File Offset: 0x000E2157
		public LicenseException(Type type, object instance)
			: this(type, null, SR.GetString("LicExceptionTypeAndInstance", new object[]
			{
				type.FullName,
				instance.GetType().FullName
			}))
		{
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x000E3F88 File Offset: 0x000E2188
		public LicenseException(Type type, object instance, string message)
			: base(message)
		{
			this.type = type;
			this.instance = instance;
			base.HResult = -2146232063;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000E3FAA File Offset: 0x000E21AA
		public LicenseException(Type type, object instance, string message, Exception innerException)
			: base(message, innerException)
		{
			this.type = type;
			this.instance = instance;
			base.HResult = -2146232063;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000E3FD0 File Offset: 0x000E21D0
		protected LicenseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.type = (Type)info.GetValue("type", typeof(Type));
			this.instance = info.GetValue("instance", typeof(object));
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060033FB RID: 13307 RVA: 0x000E4020 File Offset: 0x000E2220
		public Type LicensedType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000E4028 File Offset: 0x000E2228
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("type", this.type);
			info.AddValue("instance", this.instance);
			base.GetObjectData(info, context);
		}

		// Token: 0x040029BA RID: 10682
		private Type type;

		// Token: 0x040029BB RID: 10683
		private object instance;
	}
}
