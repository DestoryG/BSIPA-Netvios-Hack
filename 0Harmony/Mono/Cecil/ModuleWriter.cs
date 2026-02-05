using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020000C8 RID: 200
	internal static class ModuleWriter
	{
		// Token: 0x06000564 RID: 1380 RVA: 0x00019000 File Offset: 0x00017200
		public static void WriteModule(ModuleDefinition module, Disposable<Stream> stream, WriterParameters parameters)
		{
			using (stream)
			{
				ModuleWriter.Write(module, stream, parameters);
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00019038 File Offset: 0x00017238
		private static void Write(ModuleDefinition module, Disposable<Stream> stream, WriterParameters parameters)
		{
			if ((module.Attributes & ModuleAttributes.ILOnly) == (ModuleAttributes)0)
			{
				throw new NotSupportedException("Writing mixed-mode assemblies is not supported");
			}
			if (module.HasImage && module.ReadingMode == ReadingMode.Deferred)
			{
				ImmediateModuleReader immediateModuleReader = new ImmediateModuleReader(module.Image);
				immediateModuleReader.ReadModule(module, false);
				immediateModuleReader.ReadSymbols(module);
			}
			module.MetadataSystem.Clear();
			if (module.symbol_reader != null)
			{
				module.symbol_reader.Dispose();
			}
			AssemblyNameDefinition assemblyNameDefinition = ((module.assembly != null) ? module.assembly.Name : null);
			string fileName = stream.value.GetFileName();
			uint num = parameters.Timestamp ?? module.timestamp;
			ISymbolWriterProvider symbolWriterProvider = parameters.SymbolWriterProvider;
			if (symbolWriterProvider == null && parameters.WriteSymbols)
			{
				symbolWriterProvider = new DefaultSymbolWriterProvider();
			}
			if (parameters.HasStrongNameKey && assemblyNameDefinition != null)
			{
				assemblyNameDefinition.PublicKey = CryptoService.GetPublicKey(parameters);
				module.Attributes |= ModuleAttributes.StrongNameSigned;
			}
			if (parameters.DeterministicMvid)
			{
				module.Mvid = Guid.Empty;
			}
			MetadataBuilder metadataBuilder = new MetadataBuilder(module, fileName, num, symbolWriterProvider);
			try
			{
				module.metadata_builder = metadataBuilder;
				using (ISymbolWriter symbolWriter = ModuleWriter.GetSymbolWriter(module, fileName, symbolWriterProvider, parameters))
				{
					metadataBuilder.SetSymbolWriter(symbolWriter);
					ModuleWriter.BuildMetadata(module, metadataBuilder);
					if (parameters.DeterministicMvid)
					{
						metadataBuilder.ComputeDeterministicMvid();
					}
					ImageWriter imageWriter = ImageWriter.CreateWriter(module, metadataBuilder, stream);
					stream.value.SetLength(0L);
					imageWriter.WriteImage();
					if (parameters.HasStrongNameKey)
					{
						CryptoService.StrongName(stream.value, imageWriter, parameters);
					}
				}
			}
			finally
			{
				module.metadata_builder = null;
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000191DC File Offset: 0x000173DC
		private static void BuildMetadata(ModuleDefinition module, MetadataBuilder metadata)
		{
			if (!module.HasImage)
			{
				metadata.BuildMetadata();
				return;
			}
			module.Read<MetadataBuilder, MetadataBuilder>(metadata, delegate(MetadataBuilder builder, MetadataReader _)
			{
				builder.BuildMetadata();
				return builder;
			});
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00019214 File Offset: 0x00017414
		private static ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fq_name, ISymbolWriterProvider symbol_writer_provider, WriterParameters parameters)
		{
			if (symbol_writer_provider == null)
			{
				return null;
			}
			if (parameters.SymbolStream != null)
			{
				return symbol_writer_provider.GetSymbolWriter(module, parameters.SymbolStream);
			}
			return symbol_writer_provider.GetSymbolWriter(module, fq_name);
		}
	}
}
