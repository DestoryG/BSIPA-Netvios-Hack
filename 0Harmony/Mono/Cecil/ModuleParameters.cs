using System;

namespace Mono.Cecil
{
	// Token: 0x0200014C RID: 332
	internal sealed class ModuleParameters
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00023E1D File Offset: 0x0002201D
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x00023E25 File Offset: 0x00022025
		public ModuleKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				this.kind = value;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00023E2E File Offset: 0x0002202E
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x00023E36 File Offset: 0x00022036
		public TargetRuntime Runtime
		{
			get
			{
				return this.runtime;
			}
			set
			{
				this.runtime = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x00023E3F File Offset: 0x0002203F
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x00023E47 File Offset: 0x00022047
		public uint? Timestamp
		{
			get
			{
				return this.timestamp;
			}
			set
			{
				this.timestamp = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x00023E50 File Offset: 0x00022050
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x00023E58 File Offset: 0x00022058
		public TargetArchitecture Architecture
		{
			get
			{
				return this.architecture;
			}
			set
			{
				this.architecture = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x00023E61 File Offset: 0x00022061
		// (set) Token: 0x060009DF RID: 2527 RVA: 0x00023E69 File Offset: 0x00022069
		public IAssemblyResolver AssemblyResolver
		{
			get
			{
				return this.assembly_resolver;
			}
			set
			{
				this.assembly_resolver = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x00023E72 File Offset: 0x00022072
		// (set) Token: 0x060009E1 RID: 2529 RVA: 0x00023E7A File Offset: 0x0002207A
		public IMetadataResolver MetadataResolver
		{
			get
			{
				return this.metadata_resolver;
			}
			set
			{
				this.metadata_resolver = value;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x00023E83 File Offset: 0x00022083
		// (set) Token: 0x060009E3 RID: 2531 RVA: 0x00023E8B File Offset: 0x0002208B
		public IMetadataImporterProvider MetadataImporterProvider
		{
			get
			{
				return this.metadata_importer_provider;
			}
			set
			{
				this.metadata_importer_provider = value;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00023E94 File Offset: 0x00022094
		// (set) Token: 0x060009E5 RID: 2533 RVA: 0x00023E9C File Offset: 0x0002209C
		public IReflectionImporterProvider ReflectionImporterProvider
		{
			get
			{
				return this.reflection_importer_provider;
			}
			set
			{
				this.reflection_importer_provider = value;
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00023EA5 File Offset: 0x000220A5
		public ModuleParameters()
		{
			this.kind = ModuleKind.Dll;
			this.Runtime = ModuleParameters.GetCurrentRuntime();
			this.architecture = TargetArchitecture.I386;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00023ECA File Offset: 0x000220CA
		private static TargetRuntime GetCurrentRuntime()
		{
			return typeof(object).Assembly.ImageRuntimeVersion.ParseRuntime();
		}

		// Token: 0x0400039F RID: 927
		private ModuleKind kind;

		// Token: 0x040003A0 RID: 928
		private TargetRuntime runtime;

		// Token: 0x040003A1 RID: 929
		private uint? timestamp;

		// Token: 0x040003A2 RID: 930
		private TargetArchitecture architecture;

		// Token: 0x040003A3 RID: 931
		private IAssemblyResolver assembly_resolver;

		// Token: 0x040003A4 RID: 932
		private IMetadataResolver metadata_resolver;

		// Token: 0x040003A5 RID: 933
		private IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x040003A6 RID: 934
		private IReflectionImporterProvider reflection_importer_provider;
	}
}
