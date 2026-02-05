using System;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x02000017 RID: 23
	internal sealed class DeferredModuleReader : ModuleReader
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00005E08 File Offset: 0x00004008
		public DeferredModuleReader(Image image)
			: base(image, ReadingMode.Deferred)
		{
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005E12 File Offset: 0x00004012
		protected override void ReadModule()
		{
			this.module.Read<ModuleDefinition>(this.module, delegate(ModuleDefinition _, MetadataReader reader)
			{
				base.ReadModuleManifest(reader);
			});
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00002A0D File Offset: 0x00000C0D
		public override void ReadSymbols(ModuleDefinition module)
		{
		}
	}
}
