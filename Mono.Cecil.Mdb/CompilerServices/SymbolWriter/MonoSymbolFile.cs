using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000008 RID: 8
	public class MonoSymbolFile : IDisposable
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000020CC File Offset: 0x000002CC
		public MonoSymbolFile()
		{
			this.ot = new OffsetTable();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002108 File Offset: 0x00000308
		public int AddSource(SourceFileEntry source)
		{
			this.sources.Add(source);
			return this.sources.Count;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002121 File Offset: 0x00000321
		public int AddCompileUnit(CompileUnitEntry entry)
		{
			this.comp_units.Add(entry);
			return this.comp_units.Count;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000213A File Offset: 0x0000033A
		public void AddMethod(MethodEntry entry)
		{
			this.methods.Add(entry);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002148 File Offset: 0x00000348
		public MethodEntry DefineMethod(CompileUnitEntry comp_unit, int token, ScopeVariable[] scope_vars, LocalVariableEntry[] locals, LineNumberEntry[] lines, CodeBlockEntry[] code_blocks, string real_name, MethodEntry.Flags flags, int namespace_id)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			MethodEntry methodEntry = new MethodEntry(this, comp_unit, token, scope_vars, locals, lines, code_blocks, real_name, flags, namespace_id);
			this.AddMethod(methodEntry);
			return methodEntry;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002181 File Offset: 0x00000381
		internal void DefineAnonymousScope(int id)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			if (this.anonymous_scopes == null)
			{
				this.anonymous_scopes = new Dictionary<int, AnonymousScopeEntry>();
			}
			this.anonymous_scopes.Add(id, new AnonymousScopeEntry(id));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021B6 File Offset: 0x000003B6
		internal void DefineCapturedVariable(int scope_id, string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedVariable(name, captured_name, kind);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000021DB File Offset: 0x000003DB
		internal void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedScope(id, captured_name);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002200 File Offset: 0x00000400
		internal int GetNextTypeIndex()
		{
			int num = this.last_type_index + 1;
			this.last_type_index = num;
			return num;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002220 File Offset: 0x00000420
		internal int GetNextMethodIndex()
		{
			int num = this.last_method_index + 1;
			this.last_method_index = num;
			return num;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002240 File Offset: 0x00000440
		internal int GetNextNamespaceIndex()
		{
			int num = this.last_namespace_index + 1;
			this.last_namespace_index = num;
			return num;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002260 File Offset: 0x00000460
		private void Write(MyBinaryWriter bw, Guid guid)
		{
			bw.Write(5037318119232611860L);
			bw.Write(this.MajorVersion);
			bw.Write(this.MinorVersion);
			bw.Write(guid.ToByteArray());
			long position = bw.BaseStream.Position;
			this.ot.Write(bw, this.MajorVersion, this.MinorVersion);
			this.methods.Sort();
			for (int i = 0; i < this.methods.Count; i++)
			{
				this.methods[i].Index = i + 1;
			}
			this.ot.DataSectionOffset = (int)bw.BaseStream.Position;
			foreach (SourceFileEntry sourceFileEntry in this.sources)
			{
				sourceFileEntry.WriteData(bw);
			}
			foreach (CompileUnitEntry compileUnitEntry in this.comp_units)
			{
				compileUnitEntry.WriteData(bw);
			}
			foreach (MethodEntry methodEntry in this.methods)
			{
				methodEntry.WriteData(this, bw);
			}
			this.ot.DataSectionSize = (int)bw.BaseStream.Position - this.ot.DataSectionOffset;
			this.ot.MethodTableOffset = (int)bw.BaseStream.Position;
			for (int j = 0; j < this.methods.Count; j++)
			{
				this.methods[j].Write(bw);
			}
			this.ot.MethodTableSize = (int)bw.BaseStream.Position - this.ot.MethodTableOffset;
			this.ot.SourceTableOffset = (int)bw.BaseStream.Position;
			for (int k = 0; k < this.sources.Count; k++)
			{
				this.sources[k].Write(bw);
			}
			this.ot.SourceTableSize = (int)bw.BaseStream.Position - this.ot.SourceTableOffset;
			this.ot.CompileUnitTableOffset = (int)bw.BaseStream.Position;
			for (int l = 0; l < this.comp_units.Count; l++)
			{
				this.comp_units[l].Write(bw);
			}
			this.ot.CompileUnitTableSize = (int)bw.BaseStream.Position - this.ot.CompileUnitTableOffset;
			this.ot.AnonymousScopeCount = ((this.anonymous_scopes != null) ? this.anonymous_scopes.Count : 0);
			this.ot.AnonymousScopeTableOffset = (int)bw.BaseStream.Position;
			if (this.anonymous_scopes != null)
			{
				foreach (AnonymousScopeEntry anonymousScopeEntry in this.anonymous_scopes.Values)
				{
					anonymousScopeEntry.Write(bw);
				}
			}
			this.ot.AnonymousScopeTableSize = (int)bw.BaseStream.Position - this.ot.AnonymousScopeTableOffset;
			this.ot.TypeCount = this.last_type_index;
			this.ot.MethodCount = this.methods.Count;
			this.ot.SourceCount = this.sources.Count;
			this.ot.CompileUnitCount = this.comp_units.Count;
			this.ot.TotalFileSize = (int)bw.BaseStream.Position;
			bw.Seek((int)position, SeekOrigin.Begin);
			this.ot.Write(bw, this.MajorVersion, this.MinorVersion);
			bw.Seek(0, SeekOrigin.End);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002670 File Offset: 0x00000870
		public void CreateSymbolFile(Guid guid, FileStream fs)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.Write(new MyBinaryWriter(fs), guid);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002690 File Offset: 0x00000890
		private MonoSymbolFile(Stream stream)
		{
			this.reader = new MyBinaryReader(stream);
			try
			{
				long num = this.reader.ReadInt64();
				int num2 = this.reader.ReadInt32();
				int num3 = this.reader.ReadInt32();
				if (num != 5037318119232611860L)
				{
					throw new MonoSymbolFileException("Symbol file is not a valid", new object[0]);
				}
				if (num2 != 50)
				{
					throw new MonoSymbolFileException("Symbol file has version {0} but expected {1}", new object[] { num2, 50 });
				}
				if (num3 != 0)
				{
					throw new MonoSymbolFileException("Symbol file has version {0}.{1} but expected {2}.{3}", new object[] { num2, num3, 50, 0 });
				}
				this.MajorVersion = num2;
				this.MinorVersion = num3;
				this.guid = new Guid(this.reader.ReadBytes(16));
				this.ot = new OffsetTable(this.reader, num2, num3);
			}
			catch (Exception ex)
			{
				throw new MonoSymbolFileException("Cannot read symbol file", ex);
			}
			this.source_file_hash = new Dictionary<int, SourceFileEntry>();
			this.compile_unit_hash = new Dictionary<int, CompileUnitEntry>();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027E8 File Offset: 0x000009E8
		public static MonoSymbolFile ReadSymbolFile(Assembly assembly)
		{
			string text = assembly.Location + ".mdb";
			Guid moduleVersionId = assembly.GetModules()[0].ModuleVersionId;
			return MonoSymbolFile.ReadSymbolFile(text, moduleVersionId);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002819 File Offset: 0x00000A19
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename)
		{
			return MonoSymbolFile.ReadSymbolFile(new FileStream(mdbFilename, FileMode.Open, FileAccess.Read));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002828 File Offset: 0x00000A28
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename, Guid assemblyGuid)
		{
			MonoSymbolFile monoSymbolFile = MonoSymbolFile.ReadSymbolFile(mdbFilename);
			if (assemblyGuid != monoSymbolFile.guid)
			{
				throw new MonoSymbolFileException("Symbol file `{0}' does not match assembly", new object[] { mdbFilename });
			}
			return monoSymbolFile;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002860 File Offset: 0x00000A60
		public static MonoSymbolFile ReadSymbolFile(Stream stream)
		{
			return new MonoSymbolFile(stream);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002868 File Offset: 0x00000A68
		public int CompileUnitCount
		{
			get
			{
				return this.ot.CompileUnitCount;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002875 File Offset: 0x00000A75
		public int SourceCount
		{
			get
			{
				return this.ot.SourceCount;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002882 File Offset: 0x00000A82
		public int MethodCount
		{
			get
			{
				return this.ot.MethodCount;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000288F File Offset: 0x00000A8F
		public int TypeCount
		{
			get
			{
				return this.ot.TypeCount;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000289C File Offset: 0x00000A9C
		public int AnonymousScopeCount
		{
			get
			{
				return this.ot.AnonymousScopeCount;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000028A9 File Offset: 0x00000AA9
		public int NamespaceCount
		{
			get
			{
				return this.last_namespace_index;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000028B1 File Offset: 0x00000AB1
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000028B9 File Offset: 0x00000AB9
		public OffsetTable OffsetTable
		{
			get
			{
				return this.ot;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028C4 File Offset: 0x00000AC4
		public SourceFileEntry GetSourceFile(int index)
		{
			if (index < 1 || index > this.ot.SourceCount)
			{
				throw new ArgumentException();
			}
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			SourceFileEntry sourceFileEntry2;
			lock (this)
			{
				SourceFileEntry sourceFileEntry;
				if (this.source_file_hash.TryGetValue(index, out sourceFileEntry))
				{
					sourceFileEntry2 = sourceFileEntry;
				}
				else
				{
					long position = this.reader.BaseStream.Position;
					this.reader.BaseStream.Position = (long)(this.ot.SourceTableOffset + SourceFileEntry.Size * (index - 1));
					sourceFileEntry = new SourceFileEntry(this, this.reader);
					this.source_file_hash.Add(index, sourceFileEntry);
					this.reader.BaseStream.Position = position;
					sourceFileEntry2 = sourceFileEntry;
				}
			}
			return sourceFileEntry2;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000299C File Offset: 0x00000B9C
		public SourceFileEntry[] Sources
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				SourceFileEntry[] array = new SourceFileEntry[this.SourceCount];
				for (int i = 0; i < this.SourceCount; i++)
				{
					array[i] = this.GetSourceFile(i + 1);
				}
				return array;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000029E4 File Offset: 0x00000BE4
		public CompileUnitEntry GetCompileUnit(int index)
		{
			if (index < 1 || index > this.ot.CompileUnitCount)
			{
				throw new ArgumentException();
			}
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			CompileUnitEntry compileUnitEntry2;
			lock (this)
			{
				CompileUnitEntry compileUnitEntry;
				if (this.compile_unit_hash.TryGetValue(index, out compileUnitEntry))
				{
					compileUnitEntry2 = compileUnitEntry;
				}
				else
				{
					long position = this.reader.BaseStream.Position;
					this.reader.BaseStream.Position = (long)(this.ot.CompileUnitTableOffset + CompileUnitEntry.Size * (index - 1));
					compileUnitEntry = new CompileUnitEntry(this, this.reader);
					this.compile_unit_hash.Add(index, compileUnitEntry);
					this.reader.BaseStream.Position = position;
					compileUnitEntry2 = compileUnitEntry;
				}
			}
			return compileUnitEntry2;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002ABC File Offset: 0x00000CBC
		public CompileUnitEntry[] CompileUnits
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				CompileUnitEntry[] array = new CompileUnitEntry[this.CompileUnitCount];
				for (int i = 0; i < this.CompileUnitCount; i++)
				{
					array[i] = this.GetCompileUnit(i + 1);
				}
				return array;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B04 File Offset: 0x00000D04
		private void read_methods()
		{
			lock (this)
			{
				if (this.method_token_hash == null)
				{
					this.method_token_hash = new Dictionary<int, MethodEntry>();
					this.method_list = new List<MethodEntry>();
					long position = this.reader.BaseStream.Position;
					this.reader.BaseStream.Position = (long)this.ot.MethodTableOffset;
					for (int i = 0; i < this.MethodCount; i++)
					{
						MethodEntry methodEntry = new MethodEntry(this, this.reader, i + 1);
						this.method_token_hash.Add(methodEntry.Token, methodEntry);
						this.method_list.Add(methodEntry);
					}
					this.reader.BaseStream.Position = position;
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public MethodEntry GetMethodByToken(int token)
		{
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			MethodEntry methodEntry2;
			lock (this)
			{
				this.read_methods();
				MethodEntry methodEntry;
				this.method_token_hash.TryGetValue(token, out methodEntry);
				methodEntry2 = methodEntry;
			}
			return methodEntry2;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C3C File Offset: 0x00000E3C
		public MethodEntry GetMethod(int index)
		{
			if (index < 1 || index > this.ot.MethodCount)
			{
				throw new ArgumentException();
			}
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			MethodEntry methodEntry;
			lock (this)
			{
				this.read_methods();
				methodEntry = this.method_list[index - 1];
			}
			return methodEntry;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002CB0 File Offset: 0x00000EB0
		public MethodEntry[] Methods
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				MethodEntry[] array2;
				lock (this)
				{
					this.read_methods();
					MethodEntry[] array = new MethodEntry[this.MethodCount];
					this.method_list.CopyTo(array, 0);
					array2 = array;
				}
				return array2;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002D18 File Offset: 0x00000F18
		public int FindSource(string file_name)
		{
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			int num2;
			lock (this)
			{
				if (this.source_name_hash == null)
				{
					this.source_name_hash = new Dictionary<string, int>();
					for (int i = 0; i < this.ot.SourceCount; i++)
					{
						SourceFileEntry sourceFile = this.GetSourceFile(i + 1);
						this.source_name_hash.Add(sourceFile.FileName, i);
					}
				}
				int num;
				if (!this.source_name_hash.TryGetValue(file_name, out num))
				{
					num2 = -1;
				}
				else
				{
					num2 = num;
				}
			}
			return num2;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002DBC File Offset: 0x00000FBC
		public AnonymousScopeEntry GetAnonymousScope(int id)
		{
			if (this.reader == null)
			{
				throw new InvalidOperationException();
			}
			AnonymousScopeEntry anonymousScopeEntry2;
			lock (this)
			{
				if (this.anonymous_scopes != null)
				{
					AnonymousScopeEntry anonymousScopeEntry;
					this.anonymous_scopes.TryGetValue(id, out anonymousScopeEntry);
					anonymousScopeEntry2 = anonymousScopeEntry;
				}
				else
				{
					this.anonymous_scopes = new Dictionary<int, AnonymousScopeEntry>();
					this.reader.BaseStream.Position = (long)this.ot.AnonymousScopeTableOffset;
					for (int i = 0; i < this.ot.AnonymousScopeCount; i++)
					{
						AnonymousScopeEntry anonymousScopeEntry = new AnonymousScopeEntry(this.reader);
						this.anonymous_scopes.Add(anonymousScopeEntry.ID, anonymousScopeEntry);
					}
					anonymousScopeEntry2 = this.anonymous_scopes[id];
				}
			}
			return anonymousScopeEntry2;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002E88 File Offset: 0x00001088
		internal MyBinaryReader BinaryReader
		{
			get
			{
				if (this.reader == null)
				{
					throw new InvalidOperationException();
				}
				return this.reader;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002E9E File Offset: 0x0000109E
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002EA7 File Offset: 0x000010A7
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.reader != null)
			{
				this.reader.Close();
				this.reader = null;
			}
		}

		// Token: 0x04000001 RID: 1
		private List<MethodEntry> methods = new List<MethodEntry>();

		// Token: 0x04000002 RID: 2
		private List<SourceFileEntry> sources = new List<SourceFileEntry>();

		// Token: 0x04000003 RID: 3
		private List<CompileUnitEntry> comp_units = new List<CompileUnitEntry>();

		// Token: 0x04000004 RID: 4
		private Dictionary<int, AnonymousScopeEntry> anonymous_scopes;

		// Token: 0x04000005 RID: 5
		private OffsetTable ot;

		// Token: 0x04000006 RID: 6
		private int last_type_index;

		// Token: 0x04000007 RID: 7
		private int last_method_index;

		// Token: 0x04000008 RID: 8
		private int last_namespace_index;

		// Token: 0x04000009 RID: 9
		public readonly int MajorVersion = 50;

		// Token: 0x0400000A RID: 10
		public readonly int MinorVersion;

		// Token: 0x0400000B RID: 11
		public int NumLineNumbers;

		// Token: 0x0400000C RID: 12
		private MyBinaryReader reader;

		// Token: 0x0400000D RID: 13
		private Dictionary<int, SourceFileEntry> source_file_hash;

		// Token: 0x0400000E RID: 14
		private Dictionary<int, CompileUnitEntry> compile_unit_hash;

		// Token: 0x0400000F RID: 15
		private List<MethodEntry> method_list;

		// Token: 0x04000010 RID: 16
		private Dictionary<int, MethodEntry> method_token_hash;

		// Token: 0x04000011 RID: 17
		private Dictionary<string, int> source_name_hash;

		// Token: 0x04000012 RID: 18
		private Guid guid;

		// Token: 0x04000013 RID: 19
		internal int LineNumberCount;

		// Token: 0x04000014 RID: 20
		internal int LocalCount;

		// Token: 0x04000015 RID: 21
		internal int StringSize;

		// Token: 0x04000016 RID: 22
		internal int LineNumberSize;

		// Token: 0x04000017 RID: 23
		internal int ExtendedLineNumberSize;
	}
}
