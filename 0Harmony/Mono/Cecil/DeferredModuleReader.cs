using System;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020000C4 RID: 196
	internal sealed class DeferredModuleReader : ModuleReader
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x00014214 File Offset: 0x00012414
		public DeferredModuleReader(Image image)
			: base(image, ReadingMode.Deferred)
		{
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001421E File Offset: 0x0001241E
		protected override void ReadModule()
		{
			this.module.Read<ModuleDefinition>(this.module, delegate(ModuleDefinition _, MetadataReader reader)
			{
				base.ReadModuleManifest(reader);
			});
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00010C51 File Offset: 0x0000EE51
		public override void ReadSymbols(ModuleDefinition module)
		{
		}
	}
}
