using System;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020000C2 RID: 194
	internal abstract class ModuleReader
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x00013A04 File Offset: 0x00011C04
		protected ModuleReader(Image image, ReadingMode mode)
		{
			this.module = new ModuleDefinition(image);
			this.module.ReadingMode = mode;
		}

		// Token: 0x0600046D RID: 1133
		protected abstract void ReadModule();

		// Token: 0x0600046E RID: 1134
		public abstract void ReadSymbols(ModuleDefinition module);

		// Token: 0x0600046F RID: 1135 RVA: 0x00013A24 File Offset: 0x00011C24
		protected void ReadModuleManifest(MetadataReader reader)
		{
			reader.Populate(this.module);
			this.ReadAssembly(reader);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00013A3C File Offset: 0x00011C3C
		private void ReadAssembly(MetadataReader reader)
		{
			AssemblyNameDefinition assemblyNameDefinition = reader.ReadAssemblyNameDefinition();
			if (assemblyNameDefinition == null)
			{
				this.module.kind = ModuleKind.NetModule;
				return;
			}
			AssemblyDefinition assemblyDefinition = new AssemblyDefinition();
			assemblyDefinition.Name = assemblyNameDefinition;
			this.module.assembly = assemblyDefinition;
			assemblyDefinition.main_module = this.module;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00013A88 File Offset: 0x00011C88
		public static ModuleDefinition CreateModule(Image image, ReaderParameters parameters)
		{
			ModuleReader moduleReader = ModuleReader.CreateModuleReader(image, parameters.ReadingMode);
			ModuleDefinition moduleDefinition = moduleReader.module;
			if (parameters.assembly_resolver != null)
			{
				moduleDefinition.assembly_resolver = Disposable.NotOwned<IAssemblyResolver>(parameters.assembly_resolver);
			}
			if (parameters.metadata_resolver != null)
			{
				moduleDefinition.metadata_resolver = parameters.metadata_resolver;
			}
			if (parameters.metadata_importer_provider != null)
			{
				moduleDefinition.metadata_importer = parameters.metadata_importer_provider.GetMetadataImporter(moduleDefinition);
			}
			if (parameters.reflection_importer_provider != null)
			{
				moduleDefinition.reflection_importer = parameters.reflection_importer_provider.GetReflectionImporter(moduleDefinition);
			}
			ModuleReader.GetMetadataKind(moduleDefinition, parameters);
			moduleReader.ReadModule();
			ModuleReader.ReadSymbols(moduleDefinition, parameters);
			moduleReader.ReadSymbols(moduleDefinition);
			if (parameters.ReadingMode == ReadingMode.Immediate)
			{
				moduleDefinition.MetadataSystem.Clear();
			}
			return moduleDefinition;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00013B38 File Offset: 0x00011D38
		private static void ReadSymbols(ModuleDefinition module, ReaderParameters parameters)
		{
			ISymbolReaderProvider symbolReaderProvider = parameters.SymbolReaderProvider;
			if (symbolReaderProvider == null && parameters.ReadSymbols)
			{
				symbolReaderProvider = new DefaultSymbolReaderProvider();
			}
			if (symbolReaderProvider != null)
			{
				module.SymbolReaderProvider = symbolReaderProvider;
				ISymbolReader symbolReader = ((parameters.SymbolStream != null) ? symbolReaderProvider.GetSymbolReader(module, parameters.SymbolStream) : symbolReaderProvider.GetSymbolReader(module, module.FileName));
				if (symbolReader != null)
				{
					try
					{
						module.ReadSymbols(symbolReader, parameters.ThrowIfSymbolsAreNotMatching);
					}
					catch (Exception)
					{
						symbolReader.Dispose();
						throw;
					}
				}
			}
			if (module.Image.HasDebugTables())
			{
				module.ReadSymbols(new PortablePdbReader(module.Image, module));
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00013BD8 File Offset: 0x00011DD8
		private static void GetMetadataKind(ModuleDefinition module, ReaderParameters parameters)
		{
			if (!parameters.ApplyWindowsRuntimeProjections)
			{
				module.MetadataKind = MetadataKind.Ecma335;
				return;
			}
			string runtimeVersion = module.RuntimeVersion;
			if (!runtimeVersion.Contains("WindowsRuntime"))
			{
				module.MetadataKind = MetadataKind.Ecma335;
				return;
			}
			if (runtimeVersion.Contains("CLR"))
			{
				module.MetadataKind = MetadataKind.ManagedWindowsMetadata;
				return;
			}
			module.MetadataKind = MetadataKind.WindowsMetadata;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00013C2D File Offset: 0x00011E2D
		private static ModuleReader CreateModuleReader(Image image, ReadingMode mode)
		{
			if (mode == ReadingMode.Immediate)
			{
				return new ImmediateModuleReader(image);
			}
			if (mode != ReadingMode.Deferred)
			{
				throw new ArgumentException();
			}
			return new DeferredModuleReader(image);
		}

		// Token: 0x04000239 RID: 569
		protected readonly ModuleDefinition module;
	}
}
