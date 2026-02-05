using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004FC RID: 1276
	[Designer("System.Diagnostics.Design.ProcessModuleDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ProcessModule : Component
	{
		// Token: 0x06003054 RID: 12372 RVA: 0x000DB55C File Offset: 0x000D975C
		internal ProcessModule(ModuleInfo moduleInfo)
		{
			this.moduleInfo = moduleInfo;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000DB571 File Offset: 0x000D9771
		internal void EnsureNtProcessInfo()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x000DB590 File Offset: 0x000D9790
		[MonitoringDescription("ProcModModuleName")]
		public string ModuleName
		{
			get
			{
				return this.moduleInfo.baseName;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06003057 RID: 12375 RVA: 0x000DB59D File Offset: 0x000D979D
		[MonitoringDescription("ProcModFileName")]
		public string FileName
		{
			get
			{
				return this.moduleInfo.fileName;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x000DB5AA File Offset: 0x000D97AA
		[MonitoringDescription("ProcModBaseAddress")]
		public IntPtr BaseAddress
		{
			get
			{
				return this.moduleInfo.baseOfDll;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x000DB5B7 File Offset: 0x000D97B7
		[MonitoringDescription("ProcModModuleMemorySize")]
		public int ModuleMemorySize
		{
			get
			{
				return this.moduleInfo.sizeOfImage;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x000DB5C4 File Offset: 0x000D97C4
		[MonitoringDescription("ProcModEntryPointAddress")]
		public IntPtr EntryPointAddress
		{
			get
			{
				this.EnsureNtProcessInfo();
				return this.moduleInfo.entryPoint;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600305B RID: 12379 RVA: 0x000DB5D7 File Offset: 0x000D97D7
		[Browsable(false)]
		public FileVersionInfo FileVersionInfo
		{
			get
			{
				if (this.fileVersionInfo == null)
				{
					this.fileVersionInfo = FileVersionInfo.GetVersionInfo(this.FileName);
				}
				return this.fileVersionInfo;
			}
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000DB5F8 File Offset: 0x000D97F8
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[]
			{
				base.ToString(),
				this.ModuleName
			});
		}

		// Token: 0x04002887 RID: 10375
		internal ModuleInfo moduleInfo;

		// Token: 0x04002888 RID: 10376
		private FileVersionInfo fileVersionInfo;
	}
}
