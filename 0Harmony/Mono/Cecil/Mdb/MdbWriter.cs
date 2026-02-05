using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000224 RID: 548
	internal sealed class MdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x00037AB2 File Offset: 0x00035CB2
		public MdbWriter(Guid mvid, string assembly)
		{
			this.mvid = mvid;
			this.writer = new MonoSymbolWriter(assembly);
			this.source_files = new Dictionary<string, MdbWriter.SourceFile>();
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00037AD8 File Offset: 0x00035CD8
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new MdbReaderProvider();
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00037AE0 File Offset: 0x00035CE0
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

		// Token: 0x06001052 RID: 4178 RVA: 0x00037B54 File Offset: 0x00035D54
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

		// Token: 0x06001053 RID: 4179 RVA: 0x00037BC0 File Offset: 0x00035DC0
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

		// Token: 0x06001054 RID: 4180 RVA: 0x00037C96 File Offset: 0x00035E96
		private void WriteRootScope(ScopeDebugInformation scope, MethodDebugInformation info)
		{
			this.WriteScopeVariables(scope);
			if (scope.HasScopes)
			{
				this.WriteScopes(scope.Scopes, info);
			}
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00037CB4 File Offset: 0x00035EB4
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

		// Token: 0x06001056 RID: 4182 RVA: 0x00037D28 File Offset: 0x00035F28
		private void WriteScopes(Collection<ScopeDebugInformation> scopes, MethodDebugInformation info)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				this.WriteScope(scopes[i], info);
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00037D54 File Offset: 0x00035F54
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

		// Token: 0x06001058 RID: 4184 RVA: 0x00037DD0 File Offset: 0x00035FD0
		public ImageDebugHeader GetDebugHeader()
		{
			return new ImageDebugHeader();
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00037DD7 File Offset: 0x00035FD7
		public void Dispose()
		{
			this.writer.WriteSymbolFile(this.mvid);
		}

		// Token: 0x04000A0A RID: 2570
		private readonly Guid mvid;

		// Token: 0x04000A0B RID: 2571
		private readonly MonoSymbolWriter writer;

		// Token: 0x04000A0C RID: 2572
		private readonly Dictionary<string, MdbWriter.SourceFile> source_files;

		// Token: 0x02000225 RID: 549
		private class SourceFile : ISourceFile
		{
			// Token: 0x17000376 RID: 886
			// (get) Token: 0x0600105A RID: 4186 RVA: 0x00037DEA File Offset: 0x00035FEA
			public SourceFileEntry Entry
			{
				get
				{
					return this.entry;
				}
			}

			// Token: 0x17000377 RID: 887
			// (get) Token: 0x0600105B RID: 4187 RVA: 0x00037DF2 File Offset: 0x00035FF2
			public CompileUnitEntry CompilationUnit
			{
				get
				{
					return this.compilation_unit;
				}
			}

			// Token: 0x0600105C RID: 4188 RVA: 0x00037DFA File Offset: 0x00035FFA
			public SourceFile(CompileUnitEntry comp_unit, SourceFileEntry entry)
			{
				this.compilation_unit = comp_unit;
				this.entry = entry;
			}

			// Token: 0x04000A0D RID: 2573
			private readonly CompileUnitEntry compilation_unit;

			// Token: 0x04000A0E RID: 2574
			private readonly SourceFileEntry entry;
		}

		// Token: 0x02000226 RID: 550
		private class SourceMethod : IMethodDef
		{
			// Token: 0x17000378 RID: 888
			// (get) Token: 0x0600105D RID: 4189 RVA: 0x00037E10 File Offset: 0x00036010
			public string Name
			{
				get
				{
					return this.method.Name;
				}
			}

			// Token: 0x17000379 RID: 889
			// (get) Token: 0x0600105E RID: 4190 RVA: 0x00037E20 File Offset: 0x00036020
			public int Token
			{
				get
				{
					return this.method.MetadataToken.ToInt32();
				}
			}

			// Token: 0x0600105F RID: 4191 RVA: 0x00037E40 File Offset: 0x00036040
			public SourceMethod(MethodDefinition method)
			{
				this.method = method;
			}

			// Token: 0x04000A0F RID: 2575
			private readonly MethodDefinition method;
		}
	}
}
