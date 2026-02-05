using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000020 RID: 32
	public sealed class MdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00005D75 File Offset: 0x00003F75
		public MdbWriter(Guid mvid, string assembly)
		{
			this.mvid = mvid;
			this.writer = new MonoSymbolWriter(assembly);
			this.source_files = new Dictionary<string, MdbWriter.SourceFile>();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005D9B File Offset: 0x00003F9B
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new MdbReaderProvider();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005DA4 File Offset: 0x00003FA4
		private MdbWriter.SourceFile GetSourceFile(Document document)
		{
			string url = document.Url;
			MdbWriter.SourceFile sourceFile;
			if (this.source_files.TryGetValue(url, out sourceFile))
			{
				return sourceFile;
			}
			SourceFileEntry sourceFileEntry = this.writer.DefineDocument(url, null, (document.Hash != null && document.Hash.Length == 16) ? document.Hash : null);
			sourceFile = new MdbWriter.SourceFile(this.writer.DefineCompilationUnit(sourceFileEntry), sourceFileEntry);
			this.source_files.Add(url, sourceFile);
			return sourceFile;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005E18 File Offset: 0x00004018
		private void Populate(Collection<SequencePoint> sequencePoints, int[] offsets, int[] startRows, int[] endRows, int[] startCols, int[] endCols, out MdbWriter.SourceFile file)
		{
			MdbWriter.SourceFile sourceFile = null;
			for (int i = 0; i < sequencePoints.Count; i++)
			{
				SequencePoint sequencePoint = sequencePoints[i];
				offsets[i] = sequencePoint.Offset;
				if (sourceFile == null)
				{
					sourceFile = this.GetSourceFile(sequencePoint.Document);
				}
				startRows[i] = sequencePoint.StartLine;
				endRows[i] = sequencePoint.EndLine;
				startCols[i] = sequencePoint.StartColumn;
				endCols[i] = sequencePoint.EndColumn;
			}
			file = sourceFile;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005E84 File Offset: 0x00004084
		public void Write(MethodDebugInformation info)
		{
			MdbWriter.SourceMethod sourceMethod = new MdbWriter.SourceMethod(info.method);
			Collection<SequencePoint> sequencePoints = info.SequencePoints;
			int count = sequencePoints.Count;
			if (count == 0)
			{
				return;
			}
			int[] array = new int[count];
			int[] array2 = new int[count];
			int[] array3 = new int[count];
			int[] array4 = new int[count];
			int[] array5 = new int[count];
			MdbWriter.SourceFile sourceFile;
			this.Populate(sequencePoints, array, array2, array3, array4, array5, out sourceFile);
			SourceMethodBuilder sourceMethodBuilder = this.writer.OpenMethod(sourceFile.CompilationUnit, 0, sourceMethod);
			for (int i = 0; i < count; i++)
			{
				sourceMethodBuilder.MarkSequencePoint(array[i], sourceFile.CompilationUnit.SourceFile, array2[i], array4[i], array3[i], array5[i], false);
			}
			if (info.scope != null)
			{
				this.WriteRootScope(info.scope, info);
			}
			this.writer.CloseMethod();
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005F5A File Offset: 0x0000415A
		private void WriteRootScope(ScopeDebugInformation scope, MethodDebugInformation info)
		{
			this.WriteScopeVariables(scope);
			if (scope.HasScopes)
			{
				this.WriteScopes(scope.Scopes, info);
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005F78 File Offset: 0x00004178
		private void WriteScope(ScopeDebugInformation scope, MethodDebugInformation info)
		{
			this.writer.OpenScope(scope.Start.Offset);
			this.WriteScopeVariables(scope);
			if (scope.HasScopes)
			{
				this.WriteScopes(scope.Scopes, info);
			}
			this.writer.CloseScope(scope.End.IsEndOfMethod ? info.code_size : scope.End.Offset);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005FEC File Offset: 0x000041EC
		private void WriteScopes(Collection<ScopeDebugInformation> scopes, MethodDebugInformation info)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				this.WriteScope(scopes[i], info);
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006018 File Offset: 0x00004218
		private void WriteScopeVariables(ScopeDebugInformation scope)
		{
			if (!scope.HasVariables)
			{
				return;
			}
			foreach (VariableDebugInformation variableDebugInformation in scope.variables)
			{
				if (!string.IsNullOrEmpty(variableDebugInformation.Name))
				{
					this.writer.DefineLocalVariable(variableDebugInformation.Index, variableDebugInformation.Name);
				}
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006094 File Offset: 0x00004294
		public ImageDebugHeader GetDebugHeader()
		{
			return new ImageDebugHeader();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000609B File Offset: 0x0000429B
		public void Dispose()
		{
			this.writer.WriteSymbolFile(this.mvid);
		}

		// Token: 0x040000A2 RID: 162
		private readonly Guid mvid;

		// Token: 0x040000A3 RID: 163
		private readonly MonoSymbolWriter writer;

		// Token: 0x040000A4 RID: 164
		private readonly Dictionary<string, MdbWriter.SourceFile> source_files;

		// Token: 0x02000027 RID: 39
		private class SourceFile : ISourceFile
		{
			// Token: 0x1700002A RID: 42
			// (get) Token: 0x06000100 RID: 256 RVA: 0x00006112 File Offset: 0x00004312
			public SourceFileEntry Entry
			{
				get
				{
					return this.entry;
				}
			}

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x06000101 RID: 257 RVA: 0x0000611A File Offset: 0x0000431A
			public CompileUnitEntry CompilationUnit
			{
				get
				{
					return this.compilation_unit;
				}
			}

			// Token: 0x06000102 RID: 258 RVA: 0x00006122 File Offset: 0x00004322
			public SourceFile(CompileUnitEntry comp_unit, SourceFileEntry entry)
			{
				this.compilation_unit = comp_unit;
				this.entry = entry;
			}

			// Token: 0x040000B8 RID: 184
			private readonly CompileUnitEntry compilation_unit;

			// Token: 0x040000B9 RID: 185
			private readonly SourceFileEntry entry;
		}

		// Token: 0x02000028 RID: 40
		private class SourceMethod : IMethodDef
		{
			// Token: 0x1700002C RID: 44
			// (get) Token: 0x06000103 RID: 259 RVA: 0x00006138 File Offset: 0x00004338
			public string Name
			{
				get
				{
					return this.method.Name;
				}
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x06000104 RID: 260 RVA: 0x00006148 File Offset: 0x00004348
			public int Token
			{
				get
				{
					return this.method.MetadataToken.ToInt32();
				}
			}

			// Token: 0x06000105 RID: 261 RVA: 0x00006168 File Offset: 0x00004368
			public SourceMethod(MethodDefinition method)
			{
				this.method = method;
			}

			// Token: 0x040000BA RID: 186
			private readonly MethodDefinition method;
		}
	}
}
