using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000206 RID: 518
	internal class MonoSymbolFile : IDisposable
	{
		// Token: 0x06000F67 RID: 3943 RVA: 0x00033DB4 File Offset: 0x00031FB4
		public MonoSymbolFile()
		{
			this.ot = new OffsetTable();
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00033DF0 File Offset: 0x00031FF0
		public int AddSource(SourceFileEntry source)
		{
			this.sources.Add(source);
			return this.sources.Count;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00033E09 File Offset: 0x00032009
		public int AddCompileUnit(CompileUnitEntry entry)
		{
			this.comp_units.Add(entry);
			return this.comp_units.Count;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00033E22 File Offset: 0x00032022
		public void AddMethod(MethodEntry entry)
		{
			this.methods.Add(entry);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00033E30 File Offset: 0x00032030
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

		// Token: 0x06000F6C RID: 3948 RVA: 0x00033E69 File Offset: 0x00032069
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

		// Token: 0x06000F6D RID: 3949 RVA: 0x00033E9E File Offset: 0x0003209E
		internal void DefineCapturedVariable(int scope_id, string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedVariable(name, captured_name, kind);
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00033EC3 File Offset: 0x000320C3
		internal void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.anonymous_scopes[scope_id].AddCapturedScope(id, captured_name);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00033EE8 File Offset: 0x000320E8
		internal int GetNextTypeIndex()
		{
			int num = this.last_type_index + 1;
			this.last_type_index = num;
			return num;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00033F08 File Offset: 0x00032108
		internal int GetNextMethodIndex()
		{
			int num = this.last_method_index + 1;
			this.last_method_index = num;
			return num;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00033F28 File Offset: 0x00032128
		internal int GetNextNamespaceIndex()
		{
			int num = this.last_namespace_index + 1;
			this.last_namespace_index = num;
			return num;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00033F48 File Offset: 0x00032148
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

		// Token: 0x06000F73 RID: 3955 RVA: 0x00034358 File Offset: 0x00032558
		public void CreateSymbolFile(Guid guid, FileStream fs)
		{
			if (this.reader != null)
			{
				throw new InvalidOperationException();
			}
			this.Write(new MyBinaryWriter(fs), guid);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00034378 File Offset: 0x00032578
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

		// Token: 0x06000F75 RID: 3957 RVA: 0x000344D0 File Offset: 0x000326D0
		public static MonoSymbolFile ReadSymbolFile(Assembly assembly)
		{
			string text = assembly.Location + ".mdb";
			Guid moduleVersionId = assembly.GetModules()[0].ModuleVersionId;
			return MonoSymbolFile.ReadSymbolFile(text, moduleVersionId);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00034501 File Offset: 0x00032701
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename)
		{
			return MonoSymbolFile.ReadSymbolFile(new FileStream(mdbFilename, FileMode.Open, FileAccess.Read));
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00034510 File Offset: 0x00032710
		public static MonoSymbolFile ReadSymbolFile(string mdbFilename, Guid assemblyGuid)
		{
			MonoSymbolFile monoSymbolFile = MonoSymbolFile.ReadSymbolFile(mdbFilename);
			if (assemblyGuid != monoSymbolFile.guid)
			{
				throw new MonoSymbolFileException("Symbol file `{0}' does not match assembly", new object[] { mdbFilename });
			}
			return monoSymbolFile;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00034548 File Offset: 0x00032748
		public static MonoSymbolFile ReadSymbolFile(Stream stream)
		{
			return new MonoSymbolFile(stream);
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x00034550 File Offset: 0x00032750
		public int CompileUnitCount
		{
			get
			{
				return this.ot.CompileUnitCount;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0003455D File Offset: 0x0003275D
		public int SourceCount
		{
			get
			{
				return this.ot.SourceCount;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0003456A File Offset: 0x0003276A
		public int MethodCount
		{
			get
			{
				return this.ot.MethodCount;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x00034577 File Offset: 0x00032777
		public int TypeCount
		{
			get
			{
				return this.ot.TypeCount;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00034584 File Offset: 0x00032784
		public int AnonymousScopeCount
		{
			get
			{
				return this.ot.AnonymousScopeCount;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x00034591 File Offset: 0x00032791
		public int NamespaceCount
		{
			get
			{
				return this.last_namespace_index;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x00034599 File Offset: 0x00032799
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x000345A1 File Offset: 0x000327A1
		public OffsetTable OffsetTable
		{
			get
			{
				return this.ot;
			}
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000345AC File Offset: 0x000327AC
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

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x00034684 File Offset: 0x00032884
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

		// Token: 0x06000F83 RID: 3971 RVA: 0x000346CC File Offset: 0x000328CC
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

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x000347A4 File Offset: 0x000329A4
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

		// Token: 0x06000F85 RID: 3973 RVA: 0x000347EC File Offset: 0x000329EC
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

		// Token: 0x06000F86 RID: 3974 RVA: 0x000348C8 File Offset: 0x00032AC8
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

		// Token: 0x06000F87 RID: 3975 RVA: 0x00034924 File Offset: 0x00032B24
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

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x00034998 File Offset: 0x00032B98
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

		// Token: 0x06000F89 RID: 3977 RVA: 0x00034A00 File Offset: 0x00032C00
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

		// Token: 0x06000F8A RID: 3978 RVA: 0x00034AA4 File Offset: 0x00032CA4
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

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00034B70 File Offset: 0x00032D70
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

		// Token: 0x06000F8C RID: 3980 RVA: 0x00034B86 File Offset: 0x00032D86
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00034B8F File Offset: 0x00032D8F
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.reader != null)
			{
				this.reader.Close();
				this.reader = null;
			}
		}

		// Token: 0x04000956 RID: 2390
		private List<MethodEntry> methods = new List<MethodEntry>();

		// Token: 0x04000957 RID: 2391
		private List<SourceFileEntry> sources = new List<SourceFileEntry>();

		// Token: 0x04000958 RID: 2392
		private List<CompileUnitEntry> comp_units = new List<CompileUnitEntry>();

		// Token: 0x04000959 RID: 2393
		private Dictionary<int, AnonymousScopeEntry> anonymous_scopes;

		// Token: 0x0400095A RID: 2394
		private OffsetTable ot;

		// Token: 0x0400095B RID: 2395
		private int last_type_index;

		// Token: 0x0400095C RID: 2396
		private int last_method_index;

		// Token: 0x0400095D RID: 2397
		private int last_namespace_index;

		// Token: 0x0400095E RID: 2398
		public readonly int MajorVersion = 50;

		// Token: 0x0400095F RID: 2399
		public readonly int MinorVersion;

		// Token: 0x04000960 RID: 2400
		public int NumLineNumbers;

		// Token: 0x04000961 RID: 2401
		private MyBinaryReader reader;

		// Token: 0x04000962 RID: 2402
		private Dictionary<int, SourceFileEntry> source_file_hash;

		// Token: 0x04000963 RID: 2403
		private Dictionary<int, CompileUnitEntry> compile_unit_hash;

		// Token: 0x04000964 RID: 2404
		private List<MethodEntry> method_list;

		// Token: 0x04000965 RID: 2405
		private Dictionary<int, MethodEntry> method_token_hash;

		// Token: 0x04000966 RID: 2406
		private Dictionary<string, int> source_name_hash;

		// Token: 0x04000967 RID: 2407
		private Guid guid;

		// Token: 0x04000968 RID: 2408
		internal int LineNumberCount;

		// Token: 0x04000969 RID: 2409
		internal int LocalCount;

		// Token: 0x0400096A RID: 2410
		internal int StringSize;

		// Token: 0x0400096B RID: 2411
		internal int LineNumberSize;

		// Token: 0x0400096C RID: 2412
		internal int ExtendedLineNumberSize;
	}
}
