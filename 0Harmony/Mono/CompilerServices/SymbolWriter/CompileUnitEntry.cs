using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000213 RID: 531
	internal class CompileUnitEntry : ICompileUnit
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x00035517 File Offset: 0x00033717
		public static int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00010978 File Offset: 0x0000EB78
		CompileUnitEntry ICompileUnit.Entry
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0003551A File Offset: 0x0003371A
		public CompileUnitEntry(MonoSymbolFile file, SourceFileEntry source)
		{
			this.file = file;
			this.source = source;
			this.Index = file.AddCompileUnit(this);
			this.creating = true;
			this.namespaces = new List<NamespaceEntry>();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0003554F File Offset: 0x0003374F
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

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0003557E File Offset: 0x0003377E
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

		// Token: 0x06000FBD RID: 4029 RVA: 0x0003559C File Offset: 0x0003379C
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

		// Token: 0x06000FBE RID: 4030 RVA: 0x000355DC File Offset: 0x000337DC
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

		// Token: 0x06000FBF RID: 4031 RVA: 0x000356D8 File Offset: 0x000338D8
		internal void Write(BinaryWriter bw)
		{
			bw.Write(this.Index);
			bw.Write(this.DataOffset);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000356F2 File Offset: 0x000338F2
		internal CompileUnitEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.file = file;
			this.Index = reader.ReadInt32();
			this.DataOffset = reader.ReadInt32();
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00035719 File Offset: 0x00033919
		public void ReadAll()
		{
			this.ReadData();
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00035724 File Offset: 0x00033924
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

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0003584C File Offset: 0x00033A4C
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

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x00035880 File Offset: 0x00033A80
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

		// Token: 0x040009AB RID: 2475
		public readonly int Index;

		// Token: 0x040009AC RID: 2476
		private int DataOffset;

		// Token: 0x040009AD RID: 2477
		private MonoSymbolFile file;

		// Token: 0x040009AE RID: 2478
		private SourceFileEntry source;

		// Token: 0x040009AF RID: 2479
		private List<SourceFileEntry> include_files;

		// Token: 0x040009B0 RID: 2480
		private List<NamespaceEntry> namespaces;

		// Token: 0x040009B1 RID: 2481
		private bool creating;
	}
}
