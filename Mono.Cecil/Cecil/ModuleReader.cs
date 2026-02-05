using System;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x02000015 RID: 21
	internal abstract class ModuleReader
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00005629 File Offset: 0x00003829
		protected ModuleReader(Image image, ReadingMode mode)
		{
			this.module = new ModuleDefinition(image);
			this.module.ReadingMode = mode;
		}

		// Token: 0x06000104 RID: 260
		protected abstract void ReadModule();

		// Token: 0x06000105 RID: 261
		public abstract void ReadSymbols(ModuleDefinition module);

		// Token: 0x06000106 RID: 262 RVA: 0x00005649 File Offset: 0x00003849
		protected void ReadModuleManifest(MetadataReader reader)
		{
			reader.Populate(this.module);
			this.ReadAssembly(reader);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005660 File Offset: 0x00003860
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

		// Token: 0x06000108 RID: 264 RVA: 0x000056AC File Offset: 0x000038AC
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

		// Token: 0x06000109 RID: 265 RVA: 0x0000575C File Offset: 0x0000395C
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

		// Token: 0x0600010A RID: 266 RVA: 0x000057FC File Offset: 0x000039FC
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

		// Token: 0x0600010B RID: 267 RVA: 0x00005851 File Offset: 0x00003A51
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

		// Token: 0x04000036 RID: 54
		protected readonly ModuleDefinition module;
	}
}
