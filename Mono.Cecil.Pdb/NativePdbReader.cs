using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Cci;
using Microsoft.Cci.Pdb;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000007 RID: 7
	public class NativePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x06000101 RID: 257 RVA: 0x0000231F File Offset: 0x0000051F
		internal NativePdbReader(Disposable<Stream> file)
		{
			this.pdb_file = file;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000234F File Offset: 0x0000054F
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new NativePdbWriterProvider();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00002358 File Offset: 0x00000558
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			if (!header.HasEntries)
			{
				return false;
			}
			ImageDebugHeaderEntry codeViewEntry = header.GetCodeViewEntry();
			if (codeViewEntry == null)
			{
				return false;
			}
			if (codeViewEntry.Directory.Type != ImageDebugType.CodeView)
			{
				return false;
			}
			byte[] data = codeViewEntry.Data;
			if (data.Length < 24)
			{
				return false;
			}
			if (NativePdbReader.ReadInt32(data, 0) != 1396986706)
			{
				return false;
			}
			byte[] array = new byte[16];
			Buffer.BlockCopy(data, 4, array, 0, 16);
			this.guid = new Guid(array);
			this.age = NativePdbReader.ReadInt32(data, 20);
			return this.PopulateFunctions();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000023DE File Offset: 0x000005DE
		private static int ReadInt32(byte[] bytes, int start)
		{
			return (int)bytes[start] | ((int)bytes[start + 1] << 8) | ((int)bytes[start + 2] << 16) | ((int)bytes[start + 3] << 24);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00002400 File Offset: 0x00000600
		private bool PopulateFunctions()
		{
			using (this.pdb_file)
			{
				Dictionary<uint, PdbTokenLine> dictionary;
				string text;
				int num;
				Guid guid;
				PdbFunction[] array = PdbFile.LoadFunctions(this.pdb_file.value, out dictionary, out text, out num, out guid);
				if (this.guid != guid)
				{
					return false;
				}
				foreach (PdbFunction pdbFunction in array)
				{
					this.functions.Add(pdbFunction.token, pdbFunction);
				}
			}
			return true;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00002498 File Offset: 0x00000698
		public MethodDebugInformation Read(MethodDefinition method)
		{
			MetadataToken metadataToken = method.MetadataToken;
			PdbFunction pdbFunction;
			if (!this.functions.TryGetValue(metadataToken.ToUInt32(), out pdbFunction))
			{
				return null;
			}
			MethodDebugInformation methodDebugInformation = new MethodDebugInformation(method);
			this.ReadSequencePoints(pdbFunction, methodDebugInformation);
			MethodDebugInformation methodDebugInformation2 = methodDebugInformation;
			ScopeDebugInformation scopeDebugInformation2;
			if (pdbFunction.scopes.IsNullOrEmpty<PdbScope>())
			{
				ScopeDebugInformation scopeDebugInformation = new ScopeDebugInformation();
				scopeDebugInformation.Start = new InstructionOffset(0);
				scopeDebugInformation2 = scopeDebugInformation;
				scopeDebugInformation.End = new InstructionOffset((int)pdbFunction.length);
			}
			else
			{
				scopeDebugInformation2 = this.ReadScopeAndLocals(pdbFunction.scopes[0], methodDebugInformation);
			}
			methodDebugInformation2.scope = scopeDebugInformation2;
			if (pdbFunction.tokenOfMethodWhoseUsingInfoAppliesToThisMethod != method.MetadataToken.ToUInt32() && pdbFunction.tokenOfMethodWhoseUsingInfoAppliesToThisMethod != 0U)
			{
				methodDebugInformation.scope.import = this.GetImport(pdbFunction.tokenOfMethodWhoseUsingInfoAppliesToThisMethod, method.Module);
			}
			if (pdbFunction.scopes.Length > 1)
			{
				for (int i = 1; i < pdbFunction.scopes.Length; i++)
				{
					ScopeDebugInformation scopeDebugInformation3 = this.ReadScopeAndLocals(pdbFunction.scopes[i], methodDebugInformation);
					if (!NativePdbReader.AddScope(methodDebugInformation.scope.Scopes, scopeDebugInformation3))
					{
						methodDebugInformation.scope.Scopes.Add(scopeDebugInformation3);
					}
				}
			}
			if (pdbFunction.iteratorScopes != null)
			{
				StateMachineScopeDebugInformation stateMachineScopeDebugInformation = new StateMachineScopeDebugInformation();
				foreach (ILocalScope localScope in pdbFunction.iteratorScopes)
				{
					stateMachineScopeDebugInformation.Scopes.Add(new StateMachineScope((int)localScope.Offset, (int)(localScope.Offset + localScope.Length + 1U)));
				}
				methodDebugInformation.CustomDebugInformations.Add(stateMachineScopeDebugInformation);
			}
			if (pdbFunction.synchronizationInformation != null)
			{
				AsyncMethodBodyDebugInformation asyncMethodBodyDebugInformation = new AsyncMethodBodyDebugInformation((int)pdbFunction.synchronizationInformation.GeneratedCatchHandlerOffset);
				foreach (PdbSynchronizationPoint pdbSynchronizationPoint in pdbFunction.synchronizationInformation.synchronizationPoints)
				{
					asyncMethodBodyDebugInformation.Yields.Add(new InstructionOffset((int)pdbSynchronizationPoint.SynchronizeOffset));
					asyncMethodBodyDebugInformation.Resumes.Add(new InstructionOffset((int)pdbSynchronizationPoint.ContinuationOffset));
					asyncMethodBodyDebugInformation.ResumeMethods.Add(method);
				}
				methodDebugInformation.CustomDebugInformations.Add(asyncMethodBodyDebugInformation);
				methodDebugInformation.StateMachineKickOffMethod = (MethodDefinition)method.Module.LookupToken((int)pdbFunction.synchronizationInformation.kickoffMethodToken);
			}
			return methodDebugInformation;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000026E4 File Offset: 0x000008E4
		private Collection<ScopeDebugInformation> ReadScopeAndLocals(PdbScope[] scopes, MethodDebugInformation info)
		{
			Collection<ScopeDebugInformation> collection = new Collection<ScopeDebugInformation>(scopes.Length);
			foreach (PdbScope pdbScope in scopes)
			{
				if (pdbScope != null)
				{
					collection.Add(this.ReadScopeAndLocals(pdbScope, info));
				}
			}
			return collection;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00002720 File Offset: 0x00000920
		private ScopeDebugInformation ReadScopeAndLocals(PdbScope scope, MethodDebugInformation info)
		{
			ScopeDebugInformation scopeDebugInformation = new ScopeDebugInformation();
			scopeDebugInformation.Start = new InstructionOffset((int)scope.offset);
			scopeDebugInformation.End = new InstructionOffset((int)(scope.offset + scope.length));
			if (!scope.slots.IsNullOrEmpty<PdbSlot>())
			{
				scopeDebugInformation.variables = new Collection<VariableDebugInformation>(scope.slots.Length);
				foreach (PdbSlot pdbSlot in scope.slots)
				{
					if ((pdbSlot.flags & 1) == 0)
					{
						VariableDebugInformation variableDebugInformation = new VariableDebugInformation((int)pdbSlot.slot, pdbSlot.name);
						if ((pdbSlot.flags & 4) != 0)
						{
							variableDebugInformation.IsDebuggerHidden = true;
						}
						scopeDebugInformation.variables.Add(variableDebugInformation);
					}
				}
			}
			if (!scope.constants.IsNullOrEmpty<PdbConstant>())
			{
				scopeDebugInformation.constants = new Collection<ConstantDebugInformation>(scope.constants.Length);
				foreach (PdbConstant pdbConstant in scope.constants)
				{
					TypeReference typeReference = info.Method.Module.Read<PdbConstant, TypeReference>(pdbConstant, (PdbConstant c, MetadataReader r) => r.ReadConstantSignature(new MetadataToken(c.token)));
					object obj = pdbConstant.value;
					if (typeReference != null && !typeReference.IsValueType && obj is int && (int)obj == 0)
					{
						obj = null;
					}
					scopeDebugInformation.constants.Add(new ConstantDebugInformation(pdbConstant.name, typeReference, obj));
				}
			}
			if (!scope.usedNamespaces.IsNullOrEmpty<string>())
			{
				ImportDebugInformation import;
				if (this.imports.TryGetValue(scope, out import))
				{
					scopeDebugInformation.import = import;
				}
				else
				{
					import = NativePdbReader.GetImport(scope, info.Method.Module);
					this.imports.Add(scope, import);
					scopeDebugInformation.import = import;
				}
			}
			scopeDebugInformation.scopes = this.ReadScopeAndLocals(scope.scopes, info);
			return scopeDebugInformation;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000028F4 File Offset: 0x00000AF4
		private static bool AddScope(Collection<ScopeDebugInformation> scopes, ScopeDebugInformation scope)
		{
			foreach (ScopeDebugInformation scopeDebugInformation in scopes)
			{
				if (scopeDebugInformation.HasScopes && NativePdbReader.AddScope(scopeDebugInformation.Scopes, scope))
				{
					return true;
				}
				if (scope.Start.Offset >= scopeDebugInformation.Start.Offset && scope.End.Offset <= scopeDebugInformation.End.Offset)
				{
					scopeDebugInformation.Scopes.Add(scope);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000029A4 File Offset: 0x00000BA4
		private ImportDebugInformation GetImport(uint token, ModuleDefinition module)
		{
			PdbFunction pdbFunction;
			if (!this.functions.TryGetValue(token, out pdbFunction))
			{
				return null;
			}
			if (pdbFunction.scopes.Length != 1)
			{
				return null;
			}
			PdbScope pdbScope = pdbFunction.scopes[0];
			ImportDebugInformation import;
			if (this.imports.TryGetValue(pdbScope, out import))
			{
				return import;
			}
			import = NativePdbReader.GetImport(pdbScope, module);
			this.imports.Add(pdbScope, import);
			return import;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00002A04 File Offset: 0x00000C04
		private static ImportDebugInformation GetImport(PdbScope scope, ModuleDefinition module)
		{
			if (scope.usedNamespaces.IsNullOrEmpty<string>())
			{
				return null;
			}
			ImportDebugInformation importDebugInformation = new ImportDebugInformation();
			foreach (string text in scope.usedNamespaces)
			{
				if (!string.IsNullOrEmpty(text))
				{
					ImportTarget importTarget = null;
					string text2 = text.Substring(1);
					char c = text[0];
					if (c <= '@')
					{
						if (c != '*')
						{
							if (c == '@')
							{
								if (!text2.StartsWith("P:"))
								{
									goto IL_0194;
								}
								importTarget = new ImportTarget(ImportTargetKind.ImportNamespace)
								{
									@namespace = text2.Substring(2)
								};
							}
						}
						else
						{
							importTarget = new ImportTarget(ImportTargetKind.ImportNamespace)
							{
								@namespace = text2
							};
						}
					}
					else if (c != 'A')
					{
						if (c != 'T')
						{
							if (c == 'U')
							{
								importTarget = new ImportTarget(ImportTargetKind.ImportNamespace)
								{
									@namespace = text2
								};
							}
						}
						else
						{
							TypeReference typeReference = TypeParser.ParseType(module, text2, false);
							if (typeReference != null)
							{
								importTarget = new ImportTarget(ImportTargetKind.ImportType)
								{
									type = typeReference
								};
							}
						}
					}
					else
					{
						int num = text.IndexOf(' ');
						if (num < 0)
						{
							importTarget = new ImportTarget(ImportTargetKind.ImportNamespace)
							{
								@namespace = text
							};
						}
						else
						{
							string text3 = text.Substring(1, num - 1);
							string text4 = text.Substring(num + 2);
							c = text[num + 1];
							if (c != 'T')
							{
								if (c == 'U')
								{
									importTarget = new ImportTarget(ImportTargetKind.DefineNamespaceAlias)
									{
										alias = text3,
										@namespace = text4
									};
								}
							}
							else
							{
								TypeReference typeReference2 = TypeParser.ParseType(module, text4, false);
								if (typeReference2 != null)
								{
									importTarget = new ImportTarget(ImportTargetKind.DefineTypeAlias)
									{
										alias = text3,
										type = typeReference2
									};
								}
							}
						}
					}
					if (importTarget != null)
					{
						importDebugInformation.Targets.Add(importTarget);
					}
				}
				IL_0194:;
			}
			return importDebugInformation;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00002BB4 File Offset: 0x00000DB4
		private void ReadSequencePoints(PdbFunction function, MethodDebugInformation info)
		{
			if (function.lines == null)
			{
				return;
			}
			info.sequence_points = new Collection<SequencePoint>();
			foreach (PdbLines pdbLines in function.lines)
			{
				this.ReadLines(pdbLines, info);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00002BF8 File Offset: 0x00000DF8
		private void ReadLines(PdbLines lines, MethodDebugInformation info)
		{
			Document document = this.GetDocument(lines.file);
			PdbLine[] lines2 = lines.lines;
			for (int i = 0; i < lines2.Length; i++)
			{
				NativePdbReader.ReadLine(lines2[i], document, info);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00002C38 File Offset: 0x00000E38
		private static void ReadLine(PdbLine line, Document document, MethodDebugInformation info)
		{
			SequencePoint sequencePoint = new SequencePoint((int)line.offset, document);
			sequencePoint.StartLine = (int)line.lineBegin;
			sequencePoint.StartColumn = (int)line.colBegin;
			sequencePoint.EndLine = (int)line.lineEnd;
			sequencePoint.EndColumn = (int)line.colEnd;
			info.sequence_points.Add(sequencePoint);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00002C90 File Offset: 0x00000E90
		private Document GetDocument(PdbSource source)
		{
			string name = source.name;
			Document document;
			if (this.documents.TryGetValue(name, out document))
			{
				return document;
			}
			document = new Document(name)
			{
				Language = source.language.ToLanguage(),
				LanguageVendor = source.vendor.ToVendor(),
				Type = source.doctype.ToType()
			};
			this.documents.Add(name, document);
			return document;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00002D00 File Offset: 0x00000F00
		public void Dispose()
		{
			this.pdb_file.Dispose();
		}

		// Token: 0x04000004 RID: 4
		private int age;

		// Token: 0x04000005 RID: 5
		private Guid guid;

		// Token: 0x04000006 RID: 6
		private readonly Disposable<Stream> pdb_file;

		// Token: 0x04000007 RID: 7
		private readonly Dictionary<string, Document> documents = new Dictionary<string, Document>();

		// Token: 0x04000008 RID: 8
		private readonly Dictionary<uint, PdbFunction> functions = new Dictionary<uint, PdbFunction>();

		// Token: 0x04000009 RID: 9
		private readonly Dictionary<PdbScope, ImportDebugInformation> imports = new Dictionary<PdbScope, ImportDebugInformation>();
	}
}
