using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D2 RID: 466
	internal sealed class PortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0003245A File Offset: 0x0003065A
		private bool IsEmbedded
		{
			get
			{
				return this.writer == null;
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00032468 File Offset: 0x00030668
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

		// Token: 0x06000E8B RID: 3723 RVA: 0x000324B6 File Offset: 0x000306B6
		internal PortablePdbWriter(MetadataBuilder pdb_metadata, ModuleDefinition module, ImageWriter writer)
			: this(pdb_metadata, module)
		{
			this.writer = writer;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x000324C7 File Offset: 0x000306C7
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new PortablePdbReaderProvider();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x000324D0 File Offset: 0x000306D0
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

		// Token: 0x06000E8E RID: 3726 RVA: 0x000325B6 File Offset: 0x000307B6
		public void Write(MethodDebugInformation info)
		{
			this.CheckMethodDebugInformationTable();
			this.pdb_metadata.AddMethodDebugInformation(info);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x000325CC File Offset: 0x000307CC
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

		// Token: 0x06000E90 RID: 3728 RVA: 0x0003261C File Offset: 0x0003081C
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.WritePdbFile();
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00032630 File Offset: 0x00030830
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

		// Token: 0x06000E92 RID: 3730 RVA: 0x00032688 File Offset: 0x00030888
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

		// Token: 0x06000E93 RID: 3731 RVA: 0x0003275C File Offset: 0x0003095C
		private void WriteTableHeap()
		{
			this.pdb_metadata.table_heap.string_offsets = this.pdb_metadata.string_heap.WriteStrings();
			this.pdb_metadata.table_heap.ComputeTableInformations();
			this.pdb_metadata.table_heap.WriteTableHeap();
		}

		// Token: 0x040008DA RID: 2266
		private readonly MetadataBuilder pdb_metadata;

		// Token: 0x040008DB RID: 2267
		private readonly ModuleDefinition module;

		// Token: 0x040008DC RID: 2268
		private readonly ImageWriter writer;

		// Token: 0x040008DD RID: 2269
		private MetadataBuilder module_metadata;
	}
}
