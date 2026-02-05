using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200010E RID: 270
	public sealed class PortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0002324E File Offset: 0x0002144E
		private bool IsEmbedded
		{
			get
			{
				return this.writer == null;
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002325C File Offset: 0x0002145C
		internal PortablePdbWriter(MetadataBuilder pdb_metadata, ModuleDefinition module)
		{
			this.pdb_metadata = pdb_metadata;
			this.module = module;
			this.module_metadata = module.metadata_builder;
			if (this.module_metadata != pdb_metadata)
			{
				this.pdb_metadata.metadata_builder = this.module_metadata;
			}
			pdb_metadata.AddCustomDebugInformations(module);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000232AA File Offset: 0x000214AA
		internal PortablePdbWriter(MetadataBuilder pdb_metadata, ModuleDefinition module, ImageWriter writer)
			: this(pdb_metadata, module)
		{
			this.writer = writer;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000232BB File Offset: 0x000214BB
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new PortablePdbReaderProvider();
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000232C4 File Offset: 0x000214C4
		public ImageDebugHeader GetDebugHeader()
		{
			if (this.IsEmbedded)
			{
				return new ImageDebugHeader();
			}
			ImageDebugDirectory imageDebugDirectory = new ImageDebugDirectory
			{
				MajorVersion = 256,
				MinorVersion = 20557,
				Type = ImageDebugType.CodeView,
				TimeDateStamp = (int)this.module.timestamp
			};
			ByteBuffer byteBuffer = new ByteBuffer();
			byteBuffer.WriteUInt32(1396986706U);
			byteBuffer.WriteBytes(this.module.Mvid.ToByteArray());
			byteBuffer.WriteUInt32(1U);
			byteBuffer.WriteBytes(Encoding.UTF8.GetBytes(this.writer.BaseStream.GetFileName()));
			byteBuffer.WriteByte(0);
			byte[] array = new byte[byteBuffer.length];
			Buffer.BlockCopy(byteBuffer.buffer, 0, array, 0, byteBuffer.length);
			imageDebugDirectory.SizeOfData = array.Length;
			return new ImageDebugHeader(new ImageDebugHeaderEntry(imageDebugDirectory, array));
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000233AA File Offset: 0x000215AA
		public void Write(MethodDebugInformation info)
		{
			this.CheckMethodDebugInformationTable();
			this.pdb_metadata.AddMethodDebugInformation(info);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000233C0 File Offset: 0x000215C0
		private void CheckMethodDebugInformationTable()
		{
			MethodDebugInformationTable table = this.pdb_metadata.table_heap.GetTable<MethodDebugInformationTable>(Table.MethodDebugInformation);
			if (table.length > 0)
			{
				return;
			}
			table.rows = new Row<uint, uint>[this.module_metadata.method_rid - 1U];
			table.length = table.rows.Length;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00023410 File Offset: 0x00021610
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.WritePdbFile();
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00023424 File Offset: 0x00021624
		private void WritePdbFile()
		{
			this.WritePdbHeap();
			this.WriteTableHeap();
			this.writer.BuildMetadataTextMap();
			this.writer.WriteMetadataHeader();
			this.writer.WriteMetadata();
			this.writer.Flush();
			this.writer.stream.Dispose();
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002347C File Offset: 0x0002167C
		private void WritePdbHeap()
		{
			PdbHeapBuffer pdb_heap = this.pdb_metadata.pdb_heap;
			pdb_heap.WriteBytes(this.module.Mvid.ToByteArray());
			pdb_heap.WriteUInt32(this.module_metadata.timestamp);
			pdb_heap.WriteUInt32(this.module_metadata.entry_point.ToUInt32());
			MetadataTable[] tables = this.module_metadata.table_heap.tables;
			ulong num = 0UL;
			for (int i = 0; i < tables.Length; i++)
			{
				if (tables[i] != null && tables[i].Length != 0)
				{
					num |= 1UL << i;
				}
			}
			pdb_heap.WriteUInt64(num);
			for (int j = 0; j < tables.Length; j++)
			{
				if (tables[j] != null && tables[j].Length != 0)
				{
					pdb_heap.WriteUInt32((uint)tables[j].Length);
				}
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00023550 File Offset: 0x00021750
		private void WriteTableHeap()
		{
			this.pdb_metadata.table_heap.string_offsets = this.pdb_metadata.string_heap.WriteStrings();
			this.pdb_metadata.table_heap.ComputeTableInformations();
			this.pdb_metadata.table_heap.WriteTableHeap();
		}

		// Token: 0x0400067B RID: 1659
		private readonly MetadataBuilder pdb_metadata;

		// Token: 0x0400067C RID: 1660
		private readonly ModuleDefinition module;

		// Token: 0x0400067D RID: 1661
		private readonly ImageWriter writer;

		// Token: 0x0400067E RID: 1662
		private MetadataBuilder module_metadata;
	}
}
