using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022E RID: 558
	internal class NativePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x06001175 RID: 4469 RVA: 0x00038B12 File Offset: 0x00036D12
		internal NativePdbWriter(ModuleDefinition module, SymWriter writer)
		{
			this.module = module;
			this.metadata = module.metadata_builder;
			this.writer = writer;
			this.documents = new Dictionary<string, SymDocumentWriter>();
			this.import_info_to_parent = new Dictionary<ImportDebugInformation, MetadataToken>();
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00038B4A File Offset: 0x00036D4A
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new NativePdbReaderProvider();
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00038B54 File Offset: 0x00036D54
		public ImageDebugHeader GetDebugHeader()
		{
			ImageDebugDirectory imageDebugDirectory;
			byte[] debugInfo = this.writer.GetDebugInfo(out imageDebugDirectory);
			imageDebugDirectory.TimeDateStamp = (int)this.module.timestamp;
			return new ImageDebugHeader(new ImageDebugHeaderEntry(imageDebugDirectory, debugInfo));
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00038B90 File Offset: 0x00036D90
		public void Write(MethodDebugInformation info)
		{
			int num = info.method.MetadataToken.ToInt32();
			if (!info.HasSequencePoints && info.scope == null && !info.HasCustomDebugInformations && info.StateMachineKickOffMethod == null)
			{
				return;
			}
			this.writer.OpenMethod(num);
			if (!info.sequence_points.IsNullOrEmpty<SequencePoint>())
			{
				this.DefineSequencePoints(info.sequence_points);
			}
			MetadataToken metadataToken = default(MetadataToken);
			if (info.scope != null)
			{
				this.DefineScope(info.scope, info, out metadataToken);
			}
			this.DefineCustomMetadata(info, metadataToken);
			this.writer.CloseMethod();
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00038C2C File Offset: 0x00036E2C
		private void DefineCustomMetadata(MethodDebugInformation info, MetadataToken import_parent)
		{
			CustomMetadataWriter customMetadataWriter = new CustomMetadataWriter(this.writer);
			if (import_parent.RID != 0U)
			{
				customMetadataWriter.WriteForwardInfo(import_parent);
			}
			else if (info.scope != null && info.scope.Import != null && info.scope.Import.HasTargets)
			{
				customMetadataWriter.WriteUsingInfo(info.scope.Import);
			}
			if (info.Method.HasCustomAttributes)
			{
				foreach (CustomAttribute customAttribute in info.Method.CustomAttributes)
				{
					TypeReference attributeType = customAttribute.AttributeType;
					if (attributeType.IsTypeOf("System.Runtime.CompilerServices", "IteratorStateMachineAttribute") || attributeType.IsTypeOf("System.Runtime.CompilerServices", "AsyncStateMachineAttribute"))
					{
						TypeReference typeReference = customAttribute.ConstructorArguments[0].Value as TypeReference;
						if (typeReference != null)
						{
							customMetadataWriter.WriteForwardIterator(typeReference);
						}
					}
				}
			}
			if (info.HasCustomDebugInformations)
			{
				StateMachineScopeDebugInformation stateMachineScopeDebugInformation = info.CustomDebugInformations.FirstOrDefault((CustomDebugInformation cdi) => cdi.Kind == CustomDebugInformationKind.StateMachineScope) as StateMachineScopeDebugInformation;
				if (stateMachineScopeDebugInformation != null)
				{
					customMetadataWriter.WriteIteratorScopes(stateMachineScopeDebugInformation, info);
				}
			}
			customMetadataWriter.WriteCustomMetadata();
			this.DefineAsyncCustomMetadata(info);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00038D8C File Offset: 0x00036F8C
		private void DefineAsyncCustomMetadata(MethodDebugInformation info)
		{
			if (!info.HasCustomDebugInformations)
			{
				return;
			}
			foreach (CustomDebugInformation customDebugInformation in info.CustomDebugInformations)
			{
				AsyncMethodBodyDebugInformation asyncMethodBodyDebugInformation = customDebugInformation as AsyncMethodBodyDebugInformation;
				if (asyncMethodBodyDebugInformation != null)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						BinaryStreamWriter binaryStreamWriter = new BinaryStreamWriter(memoryStream);
						binaryStreamWriter.WriteUInt32((info.StateMachineKickOffMethod != null) ? info.StateMachineKickOffMethod.MetadataToken.ToUInt32() : 0U);
						binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.CatchHandler.Offset);
						binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.Resumes.Count);
						for (int i = 0; i < asyncMethodBodyDebugInformation.Resumes.Count; i++)
						{
							binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.Yields[i].Offset);
							binaryStreamWriter.WriteUInt32(asyncMethodBodyDebugInformation.resume_methods[i].MetadataToken.ToUInt32());
							binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.Resumes[i].Offset);
						}
						this.writer.DefineCustomMetadata("asyncMethodInfo", memoryStream.ToArray());
					}
				}
			}
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00038F04 File Offset: 0x00037104
		private void DefineScope(ScopeDebugInformation scope, MethodDebugInformation info, out MetadataToken import_parent)
		{
			int offset = scope.Start.Offset;
			int num = (scope.End.IsEndOfMethod ? info.code_size : scope.End.Offset);
			import_parent = new MetadataToken(0U);
			this.writer.OpenScope(offset);
			if (scope.Import != null && scope.Import.HasTargets && !this.import_info_to_parent.TryGetValue(info.scope.Import, out import_parent))
			{
				foreach (ImportTarget importTarget in scope.Import.Targets)
				{
					ImportTargetKind kind = importTarget.Kind;
					if (kind <= ImportTargetKind.ImportType)
					{
						if (kind != ImportTargetKind.ImportNamespace)
						{
							if (kind == ImportTargetKind.ImportType)
							{
								this.writer.UsingNamespace("T" + TypeParser.ToParseable(importTarget.type, true));
							}
						}
						else
						{
							this.writer.UsingNamespace("U" + importTarget.@namespace);
						}
					}
					else if (kind != ImportTargetKind.DefineNamespaceAlias)
					{
						if (kind == ImportTargetKind.DefineTypeAlias)
						{
							this.writer.UsingNamespace("A" + importTarget.Alias + " T" + TypeParser.ToParseable(importTarget.type, true));
						}
					}
					else
					{
						this.writer.UsingNamespace("A" + importTarget.Alias + " U" + importTarget.@namespace);
					}
				}
				this.import_info_to_parent.Add(info.scope.Import, info.method.MetadataToken);
			}
			int num2 = info.local_var_token.ToInt32();
			if (!scope.variables.IsNullOrEmpty<VariableDebugInformation>())
			{
				for (int i = 0; i < scope.variables.Count; i++)
				{
					VariableDebugInformation variableDebugInformation = scope.variables[i];
					this.DefineLocalVariable(variableDebugInformation, num2, offset, num);
				}
			}
			if (!scope.constants.IsNullOrEmpty<ConstantDebugInformation>())
			{
				for (int j = 0; j < scope.constants.Count; j++)
				{
					ConstantDebugInformation constantDebugInformation = scope.constants[j];
					this.DefineConstant(constantDebugInformation);
				}
			}
			if (!scope.scopes.IsNullOrEmpty<ScopeDebugInformation>())
			{
				for (int k = 0; k < scope.scopes.Count; k++)
				{
					MetadataToken metadataToken;
					this.DefineScope(scope.scopes[k], info, out metadataToken);
				}
			}
			this.writer.CloseScope(num);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000391A0 File Offset: 0x000373A0
		private void DefineSequencePoints(Collection<SequencePoint> sequence_points)
		{
			for (int i = 0; i < sequence_points.Count; i++)
			{
				SequencePoint sequencePoint = sequence_points[i];
				this.writer.DefineSequencePoints(this.GetDocument(sequencePoint.Document), new int[] { sequencePoint.Offset }, new int[] { sequencePoint.StartLine }, new int[] { sequencePoint.StartColumn }, new int[] { sequencePoint.EndLine }, new int[] { sequencePoint.EndColumn });
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00039228 File Offset: 0x00037428
		private void DefineLocalVariable(VariableDebugInformation variable, int local_var_token, int start_offset, int end_offset)
		{
			this.writer.DefineLocalVariable2(variable.Name, variable.Attributes, local_var_token, variable.Index, 0, 0, start_offset, end_offset);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00039258 File Offset: 0x00037458
		private void DefineConstant(ConstantDebugInformation constant)
		{
			uint num = this.metadata.AddStandAloneSignature(this.metadata.GetConstantTypeBlobIndex(constant.ConstantType));
			MetadataToken metadataToken = new MetadataToken(TokenType.Signature, num);
			this.writer.DefineConstant2(constant.Name, constant.Value, metadataToken.ToInt32());
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000392B0 File Offset: 0x000374B0
		private SymDocumentWriter GetDocument(Document document)
		{
			if (document == null)
			{
				return null;
			}
			SymDocumentWriter symDocumentWriter;
			if (this.documents.TryGetValue(document.Url, out symDocumentWriter))
			{
				return symDocumentWriter;
			}
			symDocumentWriter = this.writer.DefineDocument(document.Url, document.LanguageGuid, document.LanguageVendorGuid, document.TypeGuid);
			if (!document.Hash.IsNullOrEmpty<byte>())
			{
				symDocumentWriter.SetCheckSum(document.HashAlgorithmGuid, document.Hash);
			}
			this.documents[document.Url] = symDocumentWriter;
			return symDocumentWriter;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00039330 File Offset: 0x00037530
		public void Dispose()
		{
			MethodDefinition entryPoint = this.module.EntryPoint;
			if (entryPoint != null)
			{
				this.writer.SetUserEntryPoint(entryPoint.MetadataToken.ToInt32());
			}
			this.writer.Close();
		}

		// Token: 0x04000A1B RID: 2587
		private readonly ModuleDefinition module;

		// Token: 0x04000A1C RID: 2588
		private readonly MetadataBuilder metadata;

		// Token: 0x04000A1D RID: 2589
		private readonly SymWriter writer;

		// Token: 0x04000A1E RID: 2590
		private readonly Dictionary<string, SymDocumentWriter> documents;

		// Token: 0x04000A1F RID: 2591
		private readonly Dictionary<ImportDebugInformation, MetadataToken> import_info_to_parent;
	}
}
