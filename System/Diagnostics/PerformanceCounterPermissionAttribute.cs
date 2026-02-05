using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004EB RID: 1259
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public class PerformanceCounterPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x000D6C52 File Offset: 0x000D4E52
		public PerformanceCounterPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this.categoryName = "*";
			this.machineName = ".";
			this.permissionAccess = PerformanceCounterPermissionAccess.Write;
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06002F81 RID: 12161 RVA: 0x000D6C78 File Offset: 0x000D4E78
		// (set) Token: 0x06002F82 RID: 12162 RVA: 0x000D6C80 File Offset: 0x000D4E80
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.categoryName = value;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06002F83 RID: 12163 RVA: 0x000D6C97 File Offset: 0x000D4E97
		// (set) Token: 0x06002F84 RID: 12164 RVA: 0x000D6C9F File Offset: 0x000D4E9F
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[] { "MachineName", value }));
				}
				this.machineName = value;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06002F85 RID: 12165 RVA: 0x000D6CD2 File Offset: 0x000D4ED2
		// (set) Token: 0x06002F86 RID: 12166 RVA: 0x000D6CDA File Offset: 0x000D4EDA
		public PerformanceCounterPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
			set
			{
				this.permissionAccess = value;
			}
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000D6CE3 File Offset: 0x000D4EE3
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new PerformanceCounterPermission(PermissionState.Unrestricted);
			}
			return new PerformanceCounterPermission(this.PermissionAccess, this.MachineName, this.CategoryName);
		}

		// Token: 0x04002800 RID: 10240
		private string categoryName;

		// Token: 0x04002801 RID: 10241
		private string machineName;

		// Token: 0x04002802 RID: 10242
		private PerformanceCounterPermissionAccess permissionAccess;
	}
}
