using System;

namespace Mono.Cecil
{
	// Token: 0x02000094 RID: 148
	public sealed class ModuleParameters
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00015559 File Offset: 0x00013759
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x00015561 File Offset: 0x00013761
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

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001556A File Offset: 0x0001376A
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x00015572 File Offset: 0x00013772
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0001557B File Offset: 0x0001377B
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x00015583 File Offset: 0x00013783
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

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0001558C File Offset: 0x0001378C
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x00015594 File Offset: 0x00013794
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

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001559D File Offset: 0x0001379D
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x000155A5 File Offset: 0x000137A5
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

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x000155AE File Offset: 0x000137AE
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x000155B6 File Offset: 0x000137B6
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

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x000155BF File Offset: 0x000137BF
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x000155C7 File Offset: 0x000137C7
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

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x000155D0 File Offset: 0x000137D0
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x000155D8 File Offset: 0x000137D8
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

		// Token: 0x0600064D RID: 1613 RVA: 0x000155E1 File Offset: 0x000137E1
		public ModuleParameters()
		{
			this.kind = ModuleKind.Dll;
			this.Runtime = ModuleParameters.GetCurrentRuntime();
			this.architecture = TargetArchitecture.I386;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00015606 File Offset: 0x00013806
		private static TargetRuntime GetCurrentRuntime()
		{
			return typeof(object).Assembly.ImageRuntimeVersion.ParseRuntime();
		}

		// Token: 0x0400017F RID: 383
		private ModuleKind kind;

		// Token: 0x04000180 RID: 384
		private TargetRuntime runtime;

		// Token: 0x04000181 RID: 385
		private uint? timestamp;

		// Token: 0x04000182 RID: 386
		private TargetArchitecture architecture;

		// Token: 0x04000183 RID: 387
		private IAssemblyResolver assembly_resolver;

		// Token: 0x04000184 RID: 388
		private IMetadataResolver metadata_resolver;

		// Token: 0x04000185 RID: 389
		private IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x04000186 RID: 390
		private IReflectionImporterProvider reflection_importer_provider;
	}
}
