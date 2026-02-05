using System;
using System.IO;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F4 RID: 500
	internal class DefaultSymbolReaderProvider : ISymbolReaderProvider
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x0003388C File Offset: 0x00031A8C
		public DefaultSymbolReaderProvider()
			: this(true)
		{
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00033895 File Offset: 0x00031A95
		public DefaultSymbolReaderProvider(bool throwIfNoSymbol)
		{
			this.throw_if_no_symbol = throwIfNoSymbol;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000338A4 File Offset: 0x00031AA4
		public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
		{
			if (module.Image.HasDebugTables())
			{
				return null;
			}
			if (module.HasDebugHeader && module.GetDebugHeader().GetEmbeddedPortablePdbEntry() != null)
			{
				return new EmbeddedPortablePdbReaderProvider().GetSymbolReader(module, fileName);
			}
			if (File.Exists(Mixin.GetPdbFileName(fileName)))
			{
				if (Mixin.IsPortablePdb(Mixin.GetPdbFileName(fileName)))
				{
					return new PortablePdbReaderProvider().GetSymbolReader(module, fileName);
				}
				try
				{
					return SymbolProvider.GetReaderProvider(SymbolKind.NativePdb).GetSymbolReader(module, fileName);
				}
				catch (Exception)
				{
				}
			}
			if (File.Exists(Mixin.GetMdbFileName(fileName)))
			{
				try
				{
					return SymbolProvider.GetReaderProvider(SymbolKind.Mdb).GetSymbolReader(module, fileName);
				}
				catch (Exception)
				{
				}
			}
			if (this.throw_if_no_symbol)
			{
				throw new SymbolsNotFoundException(string.Format("No symbol found for file: {0}", fileName));
			}
			return null;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00033978 File Offset: 0x00031B78
		public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
		{
			if (module.Image.HasDebugTables())
			{
				return null;
			}
			if (module.HasDebugHeader && module.GetDebugHeader().GetEmbeddedPortablePdbEntry() != null)
			{
				return new EmbeddedPortablePdbReaderProvider().GetSymbolReader(module, "");
			}
			Mixin.CheckStream(symbolStream);
			Mixin.CheckReadSeek(symbolStream);
			long position = symbolStream.Position;
			BinaryStreamReader binaryStreamReader = new BinaryStreamReader(symbolStream);
			int num = binaryStreamReader.ReadInt32();
			symbolStream.Position = position;
			if (num == 1112167234)
			{
				return new PortablePdbReaderProvider().GetSymbolReader(module, symbolStream);
			}
			byte[] array = binaryStreamReader.ReadBytes("Microsoft C/C++ MSF 7.00".Length);
			symbolStream.Position = position;
			bool flag = true;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != (byte)"Microsoft C/C++ MSF 7.00"[i])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				try
				{
					return SymbolProvider.GetReaderProvider(SymbolKind.NativePdb).GetSymbolReader(module, symbolStream);
				}
				catch (Exception)
				{
				}
			}
			long num2 = binaryStreamReader.ReadInt64();
			symbolStream.Position = position;
			if (num2 == 5037318119232611860L)
			{
				try
				{
					return SymbolProvider.GetReaderProvider(SymbolKind.Mdb).GetSymbolReader(module, symbolStream);
				}
				catch (Exception)
				{
				}
			}
			if (this.throw_if_no_symbol)
			{
				throw new SymbolsNotFoundException(string.Format("No symbols found in stream", new object[0]));
			}
			return null;
		}

		// Token: 0x04000949 RID: 2377
		private readonly bool throw_if_no_symbol;
	}
}
