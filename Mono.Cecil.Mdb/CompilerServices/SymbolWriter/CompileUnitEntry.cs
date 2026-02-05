using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000011 RID: 17
	public class CompileUnitEntry : ICompileUnit
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000037DF File Offset: 0x000019DF
		public static int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000037E2 File Offset: 0x000019E2
		CompileUnitEntry ICompileUnit.Entry
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000037E5 File Offset: 0x000019E5
		public CompileUnitEntry(MonoSymbolFile file, SourceFileEntry source)
		{
			this.file = file;
			this.source = source;
			this.Index = file.AddCompileUnit(this);
			this.creating = true;
			this.namespaces = new List<NamespaceEntry>();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000381A File Offset: 0x00001A1A
		public void AddFile(SourceFileEntry file)
		{
			if (!this.creating)
			{
				throw new InvalidOperationException();
			}
			if (this.include_files == null)
			{
				this.include_files = new List<SourceFileEntry>();
			}
			this.include_files.Add(file);
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003849 File Offset: 0x00001A49
		public SourceFileEntry SourceFile
		{
			get
			{
				if (this.creating)
				{
					return this.source;
				}
				this.ReadData();
				return this.source;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003868 File Offset: 0x00001A68
		public int DefineNamespace(string name, string[] using_clauses, int parent)
		{
			if (!this.creating)
			{
				throw new InvalidOperationException();
			}
			int nextNamespaceIndex = this.file.GetNextNamespaceIndex();
			NamespaceEntry namespaceEntry = new NamespaceEntry(name, nextNamespaceIndex, using_clauses, parent);
			this.namespaces.Add(namespaceEntry);
			return nextNamespaceIndex;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000038A8 File Offset: 0x00001AA8
		internal void WriteData(MyBinaryWriter bw)
		{
			this.DataOffset = (int)bw.BaseStream.Position;
			bw.WriteLeb128(this.source.Index);
			int num = ((this.include_files != null) ? this.include_files.Count : 0);
			bw.WriteLeb128(num);
			if (this.include_files != null)
			{
				foreach (SourceFileEntry sourceFileEntry in this.include_files)
				{
					bw.WriteLeb128(sourceFileEntry.Index);
				}
			}
			bw.WriteLeb128(this.namespaces.Count);
			foreach (NamespaceEntry namespaceEntry in this.namespaces)
			{
				namespaceEntry.Write(this.file, bw);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000039A4 File Offset: 0x00001BA4
		internal void Write(BinaryWriter bw)
		{
			bw.Write(this.Index);
			bw.Write(this.DataOffset);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000039BE File Offset: 0x00001BBE
		internal CompileUnitEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.file = file;
			this.Index = reader.ReadInt32();
			this.DataOffset = reader.ReadInt32();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000039E5 File Offset: 0x00001BE5
		public void ReadAll()
		{
			this.ReadData();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000039F0 File Offset: 0x00001BF0
		private void ReadData()
		{
			if (this.creating)
			{
				throw new InvalidOperationException();
			}
			MonoSymbolFile monoSymbolFile = this.file;
			lock (monoSymbolFile)
			{
				if (this.namespaces == null)
				{
					MyBinaryReader binaryReader = this.file.BinaryReader;
					int num = (int)binaryReader.BaseStream.Position;
					binaryReader.BaseStream.Position = (long)this.DataOffset;
					int num2 = binaryReader.ReadLeb128();
					this.source = this.file.GetSourceFile(num2);
					int num3 = binaryReader.ReadLeb128();
					if (num3 > 0)
					{
						this.include_files = new List<SourceFileEntry>();
						for (int i = 0; i < num3; i++)
						{
							this.include_files.Add(this.file.GetSourceFile(binaryReader.ReadLeb128()));
						}
					}
					int num4 = binaryReader.ReadLeb128();
					this.namespaces = new List<NamespaceEntry>();
					for (int j = 0; j < num4; j++)
					{
						this.namespaces.Add(new NamespaceEntry(this.file, binaryReader));
					}
					binaryReader.BaseStream.Position = (long)num;
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003B18 File Offset: 0x00001D18
		public NamespaceEntry[] Namespaces
		{
			get
			{
				this.ReadData();
				NamespaceEntry[] array = new NamespaceEntry[this.namespaces.Count];
				this.namespaces.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003B4C File Offset: 0x00001D4C
		public SourceFileEntry[] IncludeFiles
		{
			get
			{
				this.ReadData();
				if (this.include_files == null)
				{
					return new SourceFileEntry[0];
				}
				SourceFileEntry[] array = new SourceFileEntry[this.include_files.Count];
				this.include_files.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04000049 RID: 73
		public readonly int Index;

		// Token: 0x0400004A RID: 74
		private int DataOffset;

		// Token: 0x0400004B RID: 75
		private MonoSymbolFile file;

		// Token: 0x0400004C RID: 76
		private SourceFileEntry source;

		// Token: 0x0400004D RID: 77
		private List<SourceFileEntry> include_files;

		// Token: 0x0400004E RID: 78
		private List<NamespaceEntry> namespaces;

		// Token: 0x0400004F RID: 79
		private bool creating;
	}
}
