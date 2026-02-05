using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000679 RID: 1657
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerResults
	{
		// Token: 0x06003D10 RID: 15632 RVA: 0x000FB401 File Offset: 0x000F9601
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public CompilerResults(TempFileCollection tempFiles)
		{
			this.tempFiles = tempFiles;
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06003D11 RID: 15633 RVA: 0x000FB426 File Offset: 0x000F9626
		// (set) Token: 0x06003D12 RID: 15634 RVA: 0x000FB42E File Offset: 0x000F962E
		public TempFileCollection TempFiles
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.tempFiles;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.tempFiles = value;
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06003D13 RID: 15635 RVA: 0x000FB438 File Offset: 0x000F9638
		// (set) Token: 0x06003D14 RID: 15636 RVA: 0x000FB45C File Offset: 0x000F965C
		[Obsolete("CAS policy is obsolete and will be removed in a future release of the .NET Framework. Please see http://go2.microsoft.com/fwlink/?LinkId=131738 for more information.")]
		public Evidence Evidence
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				Evidence evidence = null;
				if (this.evidence != null)
				{
					evidence = this.evidence.Clone();
				}
				return evidence;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			set
			{
				if (value != null)
				{
					this.evidence = value.Clone();
					return;
				}
				this.evidence = null;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06003D15 RID: 15637 RVA: 0x000FB478 File Offset: 0x000F9678
		// (set) Token: 0x06003D16 RID: 15638 RVA: 0x000FB4C5 File Offset: 0x000F96C5
		public Assembly CompiledAssembly
		{
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlEvidence)]
			get
			{
				if (this.compiledAssembly == null && this.pathToAssembly != null)
				{
					this.compiledAssembly = Assembly.Load(new AssemblyName
					{
						CodeBase = this.pathToAssembly
					}, this.evidence);
				}
				return this.compiledAssembly;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.compiledAssembly = value;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06003D17 RID: 15639 RVA: 0x000FB4CE File Offset: 0x000F96CE
		public CompilerErrorCollection Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x000FB4D6 File Offset: 0x000F96D6
		public StringCollection Output
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.output;
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06003D19 RID: 15641 RVA: 0x000FB4DE File Offset: 0x000F96DE
		// (set) Token: 0x06003D1A RID: 15642 RVA: 0x000FB4E6 File Offset: 0x000F96E6
		public string PathToAssembly
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.pathToAssembly;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.pathToAssembly = value;
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06003D1B RID: 15643 RVA: 0x000FB4EF File Offset: 0x000F96EF
		// (set) Token: 0x06003D1C RID: 15644 RVA: 0x000FB4F7 File Offset: 0x000F96F7
		public int NativeCompilerReturnValue
		{
			get
			{
				return this.nativeCompilerReturnValue;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.nativeCompilerReturnValue = value;
			}
		}

		// Token: 0x04002C88 RID: 11400
		private CompilerErrorCollection errors = new CompilerErrorCollection();

		// Token: 0x04002C89 RID: 11401
		private StringCollection output = new StringCollection();

		// Token: 0x04002C8A RID: 11402
		private Assembly compiledAssembly;

		// Token: 0x04002C8B RID: 11403
		private string pathToAssembly;

		// Token: 0x04002C8C RID: 11404
		private int nativeCompilerReturnValue;

		// Token: 0x04002C8D RID: 11405
		private TempFileCollection tempFiles;

		// Token: 0x04002C8E RID: 11406
		private Evidence evidence;
	}
}
