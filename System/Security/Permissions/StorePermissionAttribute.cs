using System;

namespace System.Security.Permissions
{
	// Token: 0x02000485 RID: 1157
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class StorePermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002AE2 RID: 10978 RVA: 0x000C3486 File Offset: 0x000C1686
		public StorePermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x000C348F File Offset: 0x000C168F
		// (set) Token: 0x06002AE4 RID: 10980 RVA: 0x000C3497 File Offset: 0x000C1697
		public StorePermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				StorePermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x000C34A6 File Offset: 0x000C16A6
		// (set) Token: 0x06002AE6 RID: 10982 RVA: 0x000C34B3 File Offset: 0x000C16B3
		public bool CreateStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.CreateStore) > StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.CreateStore) : (this.m_flags & ~StorePermissionFlags.CreateStore));
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x000C34D1 File Offset: 0x000C16D1
		// (set) Token: 0x06002AE8 RID: 10984 RVA: 0x000C34DE File Offset: 0x000C16DE
		public bool DeleteStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.DeleteStore) > StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.DeleteStore) : (this.m_flags & ~StorePermissionFlags.DeleteStore));
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x000C34FC File Offset: 0x000C16FC
		// (set) Token: 0x06002AEA RID: 10986 RVA: 0x000C3509 File Offset: 0x000C1709
		public bool EnumerateStores
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.EnumerateStores) > StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.EnumerateStores) : (this.m_flags & ~StorePermissionFlags.EnumerateStores));
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000C3527 File Offset: 0x000C1727
		// (set) Token: 0x06002AEC RID: 10988 RVA: 0x000C3535 File Offset: 0x000C1735
		public bool OpenStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.OpenStore) > StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.OpenStore) : (this.m_flags & ~StorePermissionFlags.OpenStore));
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002AED RID: 10989 RVA: 0x000C3554 File Offset: 0x000C1754
		// (set) Token: 0x06002AEE RID: 10990 RVA: 0x000C3562 File Offset: 0x000C1762
		public bool AddToStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.AddToStore) > StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.AddToStore) : (this.m_flags & ~StorePermissionFlags.AddToStore));
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002AEF RID: 10991 RVA: 0x000C3581 File Offset: 0x000C1781
		// (set) Token: 0x06002AF0 RID: 10992 RVA: 0x000C358F File Offset: 0x000C178F
		public bool RemoveFromStore
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.RemoveFromStore) > StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.RemoveFromStore) : (this.m_flags & ~StorePermissionFlags.RemoveFromStore));
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000C35AE File Offset: 0x000C17AE
		// (set) Token: 0x06002AF2 RID: 10994 RVA: 0x000C35BF File Offset: 0x000C17BF
		public bool EnumerateCertificates
		{
			get
			{
				return (this.m_flags & StorePermissionFlags.EnumerateCertificates) > StorePermissionFlags.NoFlags;
			}
			set
			{
				this.m_flags = (value ? (this.m_flags | StorePermissionFlags.EnumerateCertificates) : (this.m_flags & ~StorePermissionFlags.EnumerateCertificates));
			}
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000C35E4 File Offset: 0x000C17E4
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new StorePermission(PermissionState.Unrestricted);
			}
			return new StorePermission(this.m_flags);
		}

		// Token: 0x04002652 RID: 9810
		private StorePermissionFlags m_flags;
	}
}
