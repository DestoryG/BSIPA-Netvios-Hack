using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Policy;
using Microsoft.Win32.SafeHandles;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000678 RID: 1656
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerParameters
	{
		// Token: 0x06003CEE RID: 15598 RVA: 0x000FB214 File Offset: 0x000F9414
		public CompilerParameters()
			: this(null, null)
		{
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x000FB21E File Offset: 0x000F941E
		public CompilerParameters(string[] assemblyNames)
			: this(assemblyNames, null, false)
		{
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x000FB229 File Offset: 0x000F9429
		public CompilerParameters(string[] assemblyNames, string outputName)
			: this(assemblyNames, outputName, false)
		{
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x000FB234 File Offset: 0x000F9434
		public CompilerParameters(string[] assemblyNames, string outputName, bool includeDebugInformation)
		{
			if (assemblyNames != null)
			{
				this.ReferencedAssemblies.AddRange(assemblyNames);
			}
			this.outputName = outputName;
			this.includeDebugInformation = includeDebugInformation;
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06003CF2 RID: 15602 RVA: 0x000FB297 File Offset: 0x000F9497
		// (set) Token: 0x06003CF3 RID: 15603 RVA: 0x000FB29F File Offset: 0x000F949F
		public string CoreAssemblyFileName
		{
			get
			{
				return this.coreAssemblyFileName;
			}
			set
			{
				this.coreAssemblyFileName = value;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06003CF4 RID: 15604 RVA: 0x000FB2A8 File Offset: 0x000F94A8
		// (set) Token: 0x06003CF5 RID: 15605 RVA: 0x000FB2B0 File Offset: 0x000F94B0
		public bool GenerateExecutable
		{
			get
			{
				return this.generateExecutable;
			}
			set
			{
				this.generateExecutable = value;
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06003CF6 RID: 15606 RVA: 0x000FB2B9 File Offset: 0x000F94B9
		// (set) Token: 0x06003CF7 RID: 15607 RVA: 0x000FB2C1 File Offset: 0x000F94C1
		public bool GenerateInMemory
		{
			get
			{
				return this.generateInMemory;
			}
			set
			{
				this.generateInMemory = value;
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06003CF8 RID: 15608 RVA: 0x000FB2CA File Offset: 0x000F94CA
		public StringCollection ReferencedAssemblies
		{
			get
			{
				return this.assemblyNames;
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06003CF9 RID: 15609 RVA: 0x000FB2D2 File Offset: 0x000F94D2
		// (set) Token: 0x06003CFA RID: 15610 RVA: 0x000FB2DA File Offset: 0x000F94DA
		public string MainClass
		{
			get
			{
				return this.mainClass;
			}
			set
			{
				this.mainClass = value;
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06003CFB RID: 15611 RVA: 0x000FB2E3 File Offset: 0x000F94E3
		// (set) Token: 0x06003CFC RID: 15612 RVA: 0x000FB2EB File Offset: 0x000F94EB
		public string OutputAssembly
		{
			get
			{
				return this.outputName;
			}
			set
			{
				this.outputName = value;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06003CFD RID: 15613 RVA: 0x000FB2F4 File Offset: 0x000F94F4
		// (set) Token: 0x06003CFE RID: 15614 RVA: 0x000FB30F File Offset: 0x000F950F
		public TempFileCollection TempFiles
		{
			get
			{
				if (this.tempFiles == null)
				{
					this.tempFiles = new TempFileCollection();
				}
				return this.tempFiles;
			}
			set
			{
				this.tempFiles = value;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06003CFF RID: 15615 RVA: 0x000FB318 File Offset: 0x000F9518
		// (set) Token: 0x06003D00 RID: 15616 RVA: 0x000FB320 File Offset: 0x000F9520
		public bool IncludeDebugInformation
		{
			get
			{
				return this.includeDebugInformation;
			}
			set
			{
				this.includeDebugInformation = value;
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06003D01 RID: 15617 RVA: 0x000FB329 File Offset: 0x000F9529
		// (set) Token: 0x06003D02 RID: 15618 RVA: 0x000FB331 File Offset: 0x000F9531
		public bool TreatWarningsAsErrors
		{
			get
			{
				return this.treatWarningsAsErrors;
			}
			set
			{
				this.treatWarningsAsErrors = value;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06003D03 RID: 15619 RVA: 0x000FB33A File Offset: 0x000F953A
		// (set) Token: 0x06003D04 RID: 15620 RVA: 0x000FB342 File Offset: 0x000F9542
		public int WarningLevel
		{
			get
			{
				return this.warningLevel;
			}
			set
			{
				this.warningLevel = value;
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06003D05 RID: 15621 RVA: 0x000FB34B File Offset: 0x000F954B
		// (set) Token: 0x06003D06 RID: 15622 RVA: 0x000FB353 File Offset: 0x000F9553
		public string CompilerOptions
		{
			get
			{
				return this.compilerOptions;
			}
			set
			{
				this.compilerOptions = value;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06003D07 RID: 15623 RVA: 0x000FB35C File Offset: 0x000F955C
		// (set) Token: 0x06003D08 RID: 15624 RVA: 0x000FB364 File Offset: 0x000F9564
		public string Win32Resource
		{
			get
			{
				return this.win32Resource;
			}
			set
			{
				this.win32Resource = value;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x000FB36D File Offset: 0x000F956D
		[ComVisible(false)]
		public StringCollection EmbeddedResources
		{
			get
			{
				return this.embeddedResources;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x000FB375 File Offset: 0x000F9575
		[ComVisible(false)]
		public StringCollection LinkedResources
		{
			get
			{
				return this.linkedResources;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x000FB37D File Offset: 0x000F957D
		// (set) Token: 0x06003D0C RID: 15628 RVA: 0x000FB398 File Offset: 0x000F9598
		public IntPtr UserToken
		{
			get
			{
				if (this.userToken != null)
				{
					return this.userToken.DangerousGetHandle();
				}
				return IntPtr.Zero;
			}
			set
			{
				if (this.userToken != null)
				{
					this.userToken.Close();
				}
				this.userToken = new SafeUserTokenHandle(value, false);
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06003D0D RID: 15629 RVA: 0x000FB3BA File Offset: 0x000F95BA
		internal SafeUserTokenHandle SafeUserToken
		{
			get
			{
				return this.userToken;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06003D0E RID: 15630 RVA: 0x000FB3C4 File Offset: 0x000F95C4
		// (set) Token: 0x06003D0F RID: 15631 RVA: 0x000FB3E8 File Offset: 0x000F95E8
		[Obsolete("CAS policy is obsolete and will be removed in a future release of the .NET Framework. Please see http://go2.microsoft.com/fwlink/?LinkId=131738 for more information.")]
		public Evidence Evidence
		{
			get
			{
				Evidence evidence = null;
				if (this.evidence != null)
				{
					evidence = this.evidence.Clone();
				}
				return evidence;
			}
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

		// Token: 0x04002C78 RID: 11384
		[OptionalField]
		private string coreAssemblyFileName = string.Empty;

		// Token: 0x04002C79 RID: 11385
		private StringCollection assemblyNames = new StringCollection();

		// Token: 0x04002C7A RID: 11386
		[OptionalField]
		private StringCollection embeddedResources = new StringCollection();

		// Token: 0x04002C7B RID: 11387
		[OptionalField]
		private StringCollection linkedResources = new StringCollection();

		// Token: 0x04002C7C RID: 11388
		private string outputName;

		// Token: 0x04002C7D RID: 11389
		private string mainClass;

		// Token: 0x04002C7E RID: 11390
		private bool generateInMemory;

		// Token: 0x04002C7F RID: 11391
		private bool includeDebugInformation;

		// Token: 0x04002C80 RID: 11392
		private int warningLevel = -1;

		// Token: 0x04002C81 RID: 11393
		private string compilerOptions;

		// Token: 0x04002C82 RID: 11394
		private string win32Resource;

		// Token: 0x04002C83 RID: 11395
		private bool treatWarningsAsErrors;

		// Token: 0x04002C84 RID: 11396
		private bool generateExecutable;

		// Token: 0x04002C85 RID: 11397
		private TempFileCollection tempFiles;

		// Token: 0x04002C86 RID: 11398
		[NonSerialized]
		private SafeUserTokenHandle userToken;

		// Token: 0x04002C87 RID: 11399
		private Evidence evidence;
	}
}
