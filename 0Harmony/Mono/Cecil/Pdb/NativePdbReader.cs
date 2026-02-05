using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Cci.Pdb;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022C RID: 556
	internal class NativePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x00038113 File Offset: 0x00036313
		internal NativePdbReader(Disposable<Stream> file)
		{
			this.pdb_file = file;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00038143 File Offset: 0x00036343
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new NativePdbWriterProvider();
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0003814C File Offset: 0x0003634C
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

		// Token: 0x06001165 RID: 4453 RVA: 0x0003220E File Offset: 0x0003040E
		private static int ReadInt32(byte[] bytes, int start)
		{
			return (int)bytes[start] | ((int)bytes[start + 1] << 8) | ((int)bytes[start + 2] << 16) | ((int)bytes[start + 3] << 24);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000381D4 File Offset: 0x000363D4
		private bool PopulateFunctions()
		{
			using (this.pdb_file)
			{
				PdbInfo pdbInfo = PdbFile.LoadFunctions(this.pdb_file.value);
				if (this.guid != pdbInfo.Guid)
				{
					return false;
				}
				foreach (PdbFunction pdbFunction in pdbInfo.Functions)
				{
					this.functions.Add(pdbFunction.token, pdbFunction);
				}
			}
			return true;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00038268 File Offset: 0x00036468
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

		// Token: 0x06001168 RID: 4456 RVA: 0x000384B4 File Offset: 0x000366B4
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

		// Token: 0x06001169 RID: 4457 RVA: 0x000384F0 File Offset: 0x000366F0
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

		// Token: 0x0600116A RID: 4458 RVA: 0x000386C4 File Offset: 0x000368C4
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

		// Token: 0x0600116B RID: 4459 RVA: 0x00038774 File Offset: 0x00036974
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

		// Token: 0x0600116C RID: 4460 RVA: 0x000387D4 File Offset: 0x000369D4
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
							char c2 = text[num + 1];
							if (c2 != 'T')
							{
								if (c2 == 'U')
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

		// Token: 0x0600116D RID: 4461 RVA: 0x00038984 File Offset: 0x00036B84
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

		// Token: 0x0600116E RID: 4462 RVA: 0x000389C8 File Offset: 0x00036BC8
		private void ReadLines(PdbLines lines, MethodDebugInformation info)
		{
			Document document = this.GetDocument(lines.file);
			PdbLine[] lines2 = lines.lines;
			for (int i = 0; i < lines2.Length; i++)
			{
				NativePdbReader.ReadLine(lines2[i], document, info);
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00038A08 File Offset: 0x00036C08
		private static void ReadLine(PdbLine line, Document document, MethodDebugInformation info)
		{
			SequencePoint sequencePoint = new SequencePoint((int)line.offset, document);
			sequencePoint.StartLine = (int)line.lineBegin;
			sequencePoint.StartColumn = (int)line.colBegin;
			sequencePoint.EndLine = (int)line.lineEnd;
			sequencePoint.EndColumn = (int)line.colEnd;
			info.sequence_points.Add(sequencePoint);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00038A60 File Offset: 0x00036C60
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
				LanguageGuid = source.language,
				LanguageVendorGuid = source.vendor,
				TypeGuid = source.doctype,
				HashAlgorithmGuid = source.checksumAlgorithm,
				Hash = source.checksum
			};
			this.documents.Add(name, document);
			return document;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00038AD8 File Offset: 0x00036CD8
		public void Dispose()
		{
			this.pdb_file.Dispose();
		}

		// Token: 0x04000A13 RID: 2579
		private int age;

		// Token: 0x04000A14 RID: 2580
		private Guid guid;

		// Token: 0x04000A15 RID: 2581
		private readonly Disposable<Stream> pdb_file;

		// Token: 0x04000A16 RID: 2582
		private readonly Dictionary<string, Document> documents = new Dictionary<string, Document>();

		// Token: 0x04000A17 RID: 2583
		private readonly Dictionary<uint, PdbFunction> functions = new Dictionary<uint, PdbFunction>();

		// Token: 0x04000A18 RID: 2584
		private readonly Dictionary<PdbScope, ImportDebugInformation> imports = new Dictionary<PdbScope, ImportDebugInformation>();
	}
}
