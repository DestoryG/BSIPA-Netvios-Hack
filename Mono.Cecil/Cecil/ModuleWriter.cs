using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x0200001A RID: 26
	internal static class ModuleWriter
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x0000AB88 File Offset: 0x00008D88
		public static void WriteModule(ModuleDefinition module, Disposable<Stream> stream, WriterParameters parameters)
		{
			using (stream)
			{
				ModuleWriter.Write(module, stream, parameters);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000ABC0 File Offset: 0x00008DC0
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
			if (parameters.StrongNameKeyPair != null && assemblyNameDefinition != null)
			{
				assemblyNameDefinition.PublicKey = parameters.StrongNameKeyPair.PublicKey;
				module.Attributes |= ModuleAttributes.StrongNameSigned;
			}
			MetadataBuilder metadataBuilder = new MetadataBuilder(module, fileName, num, symbolWriterProvider);
			try
			{
				module.metadata_builder = metadataBuilder;
				using (ISymbolWriter symbolWriter = ModuleWriter.GetSymbolWriter(module, fileName, symbolWriterProvider, parameters))
				{
					metadataBuilder.SetSymbolWriter(symbolWriter);
					ModuleWriter.BuildMetadata(module, metadataBuilder);
					ImageWriter imageWriter = ImageWriter.CreateWriter(module, metadataBuilder, stream);
					stream.value.SetLength(0L);
					imageWriter.WriteImage();
					if (parameters.StrongNameKeyPair != null)
					{
						CryptoService.StrongName(stream.value, imageWriter, parameters.StrongNameKeyPair);
					}
				}
			}
			finally
			{
				module.metadata_builder = null;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000AD4C File Offset: 0x00008F4C
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

		// Token: 0x060001F9 RID: 505 RVA: 0x0000AD84 File Offset: 0x00008F84
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
