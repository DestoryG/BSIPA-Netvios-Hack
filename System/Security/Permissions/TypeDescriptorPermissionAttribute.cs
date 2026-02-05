using System;

namespace System.Security.Permissions
{
	// Token: 0x02000489 RID: 1161
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class TypeDescriptorPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002B03 RID: 11011 RVA: 0x000C39C3 File Offset: 0x000C1BC3
		public TypeDescriptorPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06002B04 RID: 11012 RVA: 0x000C39CC File Offset: 0x000C1BCC
		// (set) Token: 0x06002B05 RID: 11013 RVA: 0x000C39D4 File Offset: 0x000C1BD4
		public TypeDescriptorPermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				TypeDescriptorPermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x000C39E3 File Offset: 0x000C1BE3
		// (set) Token: 0x06002B07 RID: 11015 RVA: 0x000C39F0 File Offset: 0x000C1BF0
		public bool RestrictedRegistrationAccess
		{
			get
			{
				return (this.m_flags & TypeDescriptorPermissionFlags.RestrictedRegistrationAccess) > TypeDescriptorPermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | TypeDescriptorPermissionFlags.RestrictedRegistrationAccess) : (this.m_flags & ~TypeDescriptorPermissionFlags.RestrictedRegistrationAccess));
			}
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000C3A0E File Offset: 0x000C1C0E
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new TypeDescriptorPermission(PermissionState.Unrestricted);
			}
			return new TypeDescriptorPermission(this.m_flags);
		}

		// Token: 0x04002661 RID: 9825
		private TypeDescriptorPermissionFlags m_flags;
	}
}
