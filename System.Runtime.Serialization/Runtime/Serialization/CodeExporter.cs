using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization
{
	// Token: 0x02000066 RID: 102
	internal class CodeExporter
	{
		// Token: 0x0600073A RID: 1850 RVA: 0x00021478 File Offset: 0x0001F678
		internal CodeExporter(DataContractSet dataContractSet, ImportOptions options, CodeCompileUnit codeCompileUnit)
		{
			this.dataContractSet = dataContractSet;
			this.codeCompileUnit = codeCompileUnit;
			this.AddReferencedAssembly(Assembly.GetExecutingAssembly());
			this.options = options;
			this.namespaces = new Dictionary<string, string>();
			this.clrNamespaces = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<XmlQualifiedName, DataContract> keyValuePair in dataContractSet)
			{
				DataContract value = keyValuePair.Value;
				if (!value.IsBuiltInDataContract && !(value is CollectionDataContract))
				{
					ContractCodeDomInfo contractCodeDomInfo = this.GetContractCodeDomInfo(value);
					if (contractCodeDomInfo.IsProcessed && !contractCodeDomInfo.UsesWildcardNamespace)
					{
						string clrNamespace = contractCodeDomInfo.ClrNamespace;
						if (clrNamespace != null && !this.clrNamespaces.ContainsKey(clrNamespace))
						{
							this.clrNamespaces.Add(clrNamespace, value.StableName.Namespace);
							this.namespaces.Add(value.StableName.Namespace, clrNamespace);
						}
					}
				}
			}
			if (this.options != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair2 in options.Namespaces)
				{
					string key = keyValuePair2.Key;
					string text = keyValuePair2.Value;
					if (text == null)
					{
						text = string.Empty;
					}
					string text2;
					if (this.clrNamespaces.TryGetValue(text, out text2))
					{
						if (key != text2)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("CLR namespace is mapped multiple times. Current data contract namespace is '{0}', found '{1}' for CLR namespace '{2}'.", new object[] { text2, key, text })));
						}
					}
					else
					{
						this.clrNamespaces.Add(text, key);
					}
					string text3;
					if (this.namespaces.TryGetValue(key, out text3))
					{
						if (text != text3)
						{
							this.namespaces.Remove(key);
							this.namespaces.Add(key, text);
						}
					}
					else
					{
						this.namespaces.Add(key, text);
					}
				}
			}
			foreach (object obj in codeCompileUnit.Namespaces)
			{
				CodeNamespace codeNamespace = (CodeNamespace)obj;
				string text4 = codeNamespace.Name ?? string.Empty;
				if (!this.clrNamespaces.ContainsKey(text4))
				{
					this.clrNamespaces.Add(text4, null);
				}
				if (text4.Length == 0)
				{
					foreach (object obj2 in codeNamespace.Types)
					{
						CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj2;
						this.AddGlobalTypeName(codeTypeDeclaration.Name);
					}
				}
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00021764 File Offset: 0x0001F964
		private void AddReferencedAssembly(Assembly assembly)
		{
			string fileName = Path.GetFileName(assembly.Location);
			bool flag = false;
			using (StringEnumerator enumerator = this.codeCompileUnit.ReferencedAssemblies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (string.Compare(enumerator.Current, fileName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.codeCompileUnit.ReferencedAssemblies.Add(fileName);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x000217E8 File Offset: 0x0001F9E8
		private bool GenerateSerializableTypes
		{
			get
			{
				return this.options != null && this.options.GenerateSerializable;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x000217FF File Offset: 0x0001F9FF
		private bool GenerateInternalTypes
		{
			get
			{
				return this.options != null && this.options.GenerateInternal;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x00021816 File Offset: 0x0001FA16
		private bool EnableDataBinding
		{
			get
			{
				return this.options != null && this.options.EnableDataBinding;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0002182D File Offset: 0x0001FA2D
		private CodeDomProvider CodeProvider
		{
			get
			{
				if (this.options != null)
				{
					return this.options.CodeProvider;
				}
				return null;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00021844 File Offset: 0x0001FA44
		private bool SupportsDeclareEvents
		{
			[SecuritySafeCritical]
			get
			{
				return this.CodeProvider == null || this.CodeProvider.Supports(GeneratorSupport.DeclareEvents);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00021860 File Offset: 0x0001FA60
		private bool SupportsDeclareValueTypes
		{
			[SecuritySafeCritical]
			get
			{
				return this.CodeProvider == null || this.CodeProvider.Supports(GeneratorSupport.DeclareValueTypes);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0002187C File Offset: 0x0001FA7C
		private bool SupportsGenericTypeReference
		{
			[SecuritySafeCritical]
			get
			{
				return this.CodeProvider == null || this.CodeProvider.Supports(GeneratorSupport.GenericTypeReference);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00021898 File Offset: 0x0001FA98
		private bool SupportsAssemblyAttributes
		{
			[SecuritySafeCritical]
			get
			{
				return this.CodeProvider == null || this.CodeProvider.Supports(GeneratorSupport.AssemblyAttributes);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x000218B4 File Offset: 0x0001FAB4
		private bool SupportsPartialTypes
		{
			[SecuritySafeCritical]
			get
			{
				return this.CodeProvider == null || this.CodeProvider.Supports(GeneratorSupport.PartialTypes);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x000218D0 File Offset: 0x0001FAD0
		private bool SupportsNestedTypes
		{
			[SecuritySafeCritical]
			get
			{
				return this.CodeProvider == null || this.CodeProvider.Supports(GeneratorSupport.NestedTypes);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x000218EC File Offset: 0x0001FAEC
		private string FileExtension
		{
			[SecuritySafeCritical]
			get
			{
				if (this.CodeProvider != null)
				{
					return this.CodeProvider.FileExtension;
				}
				return string.Empty;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00021907 File Offset: 0x0001FB07
		private Dictionary<string, string> Namespaces
		{
			get
			{
				return this.namespaces;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0002190F File Offset: 0x0001FB0F
		private Dictionary<string, string> ClrNamespaces
		{
			get
			{
				return this.clrNamespaces;
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00021918 File Offset: 0x0001FB18
		private bool TryGetReferencedType(XmlQualifiedName stableName, DataContract dataContract, out Type type)
		{
			if (dataContract == null)
			{
				if (this.dataContractSet.TryGetReferencedCollectionType(stableName, dataContract, out type))
				{
					return true;
				}
				if (!this.dataContractSet.TryGetReferencedType(stableName, dataContract, out type))
				{
					return false;
				}
				if (CollectionDataContract.IsCollection(type))
				{
					type = null;
					return false;
				}
				return true;
			}
			else
			{
				if (dataContract is CollectionDataContract)
				{
					return this.dataContractSet.TryGetReferencedCollectionType(stableName, dataContract, out type);
				}
				XmlDataContract xmlDataContract = dataContract as XmlDataContract;
				if (xmlDataContract != null && xmlDataContract.IsAnonymous)
				{
					stableName = SchemaImporter.ImportActualType(xmlDataContract.XsdType.Annotation, stableName, dataContract.StableName);
				}
				return this.dataContractSet.TryGetReferencedType(stableName, dataContract, out type);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000219AC File Offset: 0x0001FBAC
		[SecurityCritical]
		internal void Export()
		{
			try
			{
				foreach (KeyValuePair<XmlQualifiedName, DataContract> keyValuePair in this.dataContractSet)
				{
					DataContract value = keyValuePair.Value;
					if (!value.IsBuiltInDataContract)
					{
						ContractCodeDomInfo contractCodeDomInfo = this.GetContractCodeDomInfo(value);
						if (!contractCodeDomInfo.IsProcessed)
						{
							if (value is ClassDataContract)
							{
								ClassDataContract classDataContract = (ClassDataContract)value;
								if (classDataContract.IsISerializable)
								{
									this.ExportISerializableDataContract(classDataContract, contractCodeDomInfo);
								}
								else
								{
									this.ExportClassDataContractHierarchy(classDataContract.StableName, classDataContract, contractCodeDomInfo, new Dictionary<XmlQualifiedName, object>());
								}
							}
							else if (value is CollectionDataContract)
							{
								this.ExportCollectionDataContract((CollectionDataContract)value, contractCodeDomInfo);
							}
							else if (value is EnumDataContract)
							{
								this.ExportEnumDataContract((EnumDataContract)value, contractCodeDomInfo);
							}
							else
							{
								if (!(value is XmlDataContract))
								{
									throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("An internal error has occurred. Unexpected contract type '{0}' for type '{1}' encountered.", new object[]
									{
										DataContract.GetClrTypeFullName(value.GetType()),
										DataContract.GetClrTypeFullName(value.UnderlyingType)
									})));
								}
								this.ExportXmlDataContract((XmlDataContract)value, contractCodeDomInfo);
							}
							contractCodeDomInfo.IsProcessed = true;
						}
					}
				}
				if (this.dataContractSet.DataContractSurrogate != null)
				{
					CodeNamespace[] array = new CodeNamespace[this.codeCompileUnit.Namespaces.Count];
					this.codeCompileUnit.Namespaces.CopyTo(array, 0);
					foreach (CodeNamespace codeNamespace in array)
					{
						this.InvokeProcessImportedType(codeNamespace.Types);
					}
				}
			}
			finally
			{
				CodeGenerator.ValidateIdentifiers(this.codeCompileUnit);
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00021B74 File Offset: 0x0001FD74
		private void ExportClassDataContractHierarchy(XmlQualifiedName typeName, ClassDataContract classContract, ContractCodeDomInfo contractCodeDomInfo, Dictionary<XmlQualifiedName, object> contractNamesInHierarchy)
		{
			if (contractNamesInHierarchy.ContainsKey(classContract.StableName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' in '{1}' namespace cannot be imported: {2}", new object[]
				{
					typeName.Name,
					typeName.Namespace,
					SR.GetString("Circular type reference was found for '{0}' in '{1}' namespace.", new object[]
					{
						classContract.StableName.Name,
						classContract.StableName.Namespace
					})
				})));
			}
			contractNamesInHierarchy.Add(classContract.StableName, null);
			ClassDataContract baseContract = classContract.BaseContract;
			if (baseContract != null)
			{
				ContractCodeDomInfo contractCodeDomInfo2 = this.GetContractCodeDomInfo(baseContract);
				if (!contractCodeDomInfo2.IsProcessed)
				{
					this.ExportClassDataContractHierarchy(typeName, baseContract, contractCodeDomInfo2, contractNamesInHierarchy);
					contractCodeDomInfo2.IsProcessed = true;
				}
			}
			this.ExportClassDataContract(classContract, contractCodeDomInfo);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00021C30 File Offset: 0x0001FE30
		private void InvokeProcessImportedType(CollectionBase collection)
		{
			object[] array = new object[collection.Count];
			((ICollection)collection).CopyTo(array, 0);
			object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				CodeTypeDeclaration codeTypeDeclaration = array2[i] as CodeTypeDeclaration;
				if (codeTypeDeclaration != null)
				{
					CodeTypeDeclaration codeTypeDeclaration2 = DataContractSurrogateCaller.ProcessImportedType(this.dataContractSet.DataContractSurrogate, codeTypeDeclaration, this.codeCompileUnit);
					if (codeTypeDeclaration2 != codeTypeDeclaration)
					{
						((IList)collection).Remove(codeTypeDeclaration);
						if (codeTypeDeclaration2 != null)
						{
							((IList)collection).Add(codeTypeDeclaration2);
						}
					}
					if (codeTypeDeclaration2 != null)
					{
						this.InvokeProcessImportedType(codeTypeDeclaration2.Members);
					}
				}
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00021CB0 File Offset: 0x0001FEB0
		internal CodeTypeReference GetCodeTypeReference(DataContract dataContract)
		{
			if (dataContract.IsBuiltInDataContract)
			{
				return this.GetCodeTypeReference(dataContract.UnderlyingType);
			}
			ContractCodeDomInfo contractCodeDomInfo = this.GetContractCodeDomInfo(dataContract);
			this.GenerateType(dataContract, contractCodeDomInfo);
			return contractCodeDomInfo.TypeReference;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00021CE8 File Offset: 0x0001FEE8
		private CodeTypeReference GetCodeTypeReference(Type type)
		{
			this.AddReferencedAssembly(type.Assembly);
			return new CodeTypeReference(type);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00021CFC File Offset: 0x0001FEFC
		internal CodeTypeReference GetElementTypeReference(DataContract dataContract, bool isElementTypeNullable)
		{
			CodeTypeReference codeTypeReference = this.GetCodeTypeReference(dataContract);
			if (dataContract.IsValueType && isElementTypeNullable)
			{
				codeTypeReference = this.WrapNullable(codeTypeReference);
			}
			return codeTypeReference;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00021D24 File Offset: 0x0001FF24
		private XmlQualifiedName GenericListName
		{
			get
			{
				return DataContract.GetStableName(Globals.TypeOfListGeneric);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00021D30 File Offset: 0x0001FF30
		private CollectionDataContract GenericListContract
		{
			get
			{
				return this.dataContractSet.GetDataContract(Globals.TypeOfListGeneric) as CollectionDataContract;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00021D47 File Offset: 0x0001FF47
		private XmlQualifiedName GenericDictionaryName
		{
			get
			{
				return DataContract.GetStableName(Globals.TypeOfDictionaryGeneric);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00021D53 File Offset: 0x0001FF53
		private CollectionDataContract GenericDictionaryContract
		{
			get
			{
				return this.dataContractSet.GetDataContract(Globals.TypeOfDictionaryGeneric) as CollectionDataContract;
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00021D6C File Offset: 0x0001FF6C
		private ContractCodeDomInfo GetContractCodeDomInfo(DataContract dataContract)
		{
			ContractCodeDomInfo contractCodeDomInfo = this.dataContractSet.GetContractCodeDomInfo(dataContract);
			if (contractCodeDomInfo == null)
			{
				contractCodeDomInfo = new ContractCodeDomInfo();
				this.dataContractSet.SetContractCodeDomInfo(dataContract, contractCodeDomInfo);
			}
			return contractCodeDomInfo;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00021DA0 File Offset: 0x0001FFA0
		private void GenerateType(DataContract dataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			if (!contractCodeDomInfo.IsProcessed)
			{
				CodeTypeReference referencedType = this.GetReferencedType(dataContract);
				if (referencedType != null)
				{
					contractCodeDomInfo.TypeReference = referencedType;
					contractCodeDomInfo.ReferencedTypeExists = true;
					return;
				}
				if (contractCodeDomInfo.TypeDeclaration == null)
				{
					string clrNamespace = this.GetClrNamespace(dataContract, contractCodeDomInfo);
					CodeNamespace codeNamespace = this.GetCodeNamespace(clrNamespace, dataContract.StableName.Namespace, contractCodeDomInfo);
					CodeTypeDeclaration codeTypeDeclaration = this.GetNestedType(dataContract, contractCodeDomInfo);
					if (codeTypeDeclaration == null)
					{
						string text = XmlConvert.DecodeName(dataContract.StableName.Name);
						text = CodeExporter.GetClrIdentifier(text, "GeneratedType");
						if (this.NamespaceContainsType(codeNamespace, text) || this.GlobalTypeNameConflicts(clrNamespace, text))
						{
							int num = 1;
							string text2;
							for (;;)
							{
								text2 = CodeExporter.AppendToValidClrIdentifier(text, num.ToString(NumberFormatInfo.InvariantInfo));
								if (!this.NamespaceContainsType(codeNamespace, text2) && !this.GlobalTypeNameConflicts(clrNamespace, text2))
								{
									break;
								}
								if (num == 2147483647)
								{
									goto Block_8;
								}
								num++;
							}
							text = text2;
							goto IL_00F9;
							Block_8:
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Cannot compute unique name for '{0}'.", new object[] { text })));
						}
						IL_00F9:
						codeTypeDeclaration = CodeExporter.CreateTypeDeclaration(text, dataContract);
						codeNamespace.Types.Add(codeTypeDeclaration);
						if (string.IsNullOrEmpty(clrNamespace))
						{
							this.AddGlobalTypeName(text);
						}
						contractCodeDomInfo.TypeReference = new CodeTypeReference((clrNamespace == null || clrNamespace.Length == 0) ? text : (clrNamespace + "." + text));
						if (this.GenerateInternalTypes)
						{
							codeTypeDeclaration.TypeAttributes = TypeAttributes.NotPublic;
						}
						else
						{
							codeTypeDeclaration.TypeAttributes = TypeAttributes.Public;
						}
					}
					if (this.dataContractSet.DataContractSurrogate != null)
					{
						codeTypeDeclaration.UserData.Add(CodeExporter.surrogateDataKey, this.dataContractSet.GetSurrogateData(dataContract));
					}
					contractCodeDomInfo.TypeDeclaration = codeTypeDeclaration;
				}
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00021F3C File Offset: 0x0002013C
		private CodeTypeDeclaration GetNestedType(DataContract dataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			if (!this.SupportsNestedTypes)
			{
				return null;
			}
			string name = dataContract.StableName.Name;
			int num = name.LastIndexOf('.');
			if (num <= 0)
			{
				return null;
			}
			string text = name.Substring(0, num);
			DataContract dataContract2 = this.dataContractSet[new XmlQualifiedName(text, dataContract.StableName.Namespace)];
			if (dataContract2 == null)
			{
				return null;
			}
			string text2 = XmlConvert.DecodeName(name.Substring(num + 1));
			text2 = CodeExporter.GetClrIdentifier(text2, "GeneratedType");
			ContractCodeDomInfo contractCodeDomInfo2 = this.GetContractCodeDomInfo(dataContract2);
			this.GenerateType(dataContract2, contractCodeDomInfo2);
			if (contractCodeDomInfo2.ReferencedTypeExists)
			{
				return null;
			}
			CodeTypeDeclaration typeDeclaration = contractCodeDomInfo2.TypeDeclaration;
			if (this.TypeContainsNestedType(typeDeclaration, text2))
			{
				int num2 = 1;
				string text3;
				for (;;)
				{
					text3 = CodeExporter.AppendToValidClrIdentifier(text2, num2.ToString(NumberFormatInfo.InvariantInfo));
					if (!this.TypeContainsNestedType(typeDeclaration, text3))
					{
						break;
					}
					num2++;
				}
				text2 = text3;
			}
			CodeTypeDeclaration codeTypeDeclaration = CodeExporter.CreateTypeDeclaration(text2, dataContract);
			typeDeclaration.Members.Add(codeTypeDeclaration);
			contractCodeDomInfo.TypeReference = new CodeTypeReference(contractCodeDomInfo2.TypeReference.BaseType + "+" + text2);
			if (this.GenerateInternalTypes)
			{
				codeTypeDeclaration.TypeAttributes = TypeAttributes.NestedAssembly;
			}
			else
			{
				codeTypeDeclaration.TypeAttributes = TypeAttributes.NestedPublic;
			}
			return codeTypeDeclaration;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00022074 File Offset: 0x00020274
		private static CodeTypeDeclaration CreateTypeDeclaration(string typeName, DataContract dataContract)
		{
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(typeName);
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(typeof(DebuggerStepThroughAttribute).FullName);
			CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration(typeof(GeneratedCodeAttribute).FullName);
			AssemblyName name = Assembly.GetExecutingAssembly().GetName();
			codeAttributeDeclaration2.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(name.Name)));
			codeAttributeDeclaration2.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(name.Version.ToString())));
			if (!(dataContract is EnumDataContract))
			{
				codeTypeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
			}
			codeTypeDeclaration.CustomAttributes.Add(codeAttributeDeclaration2);
			return codeTypeDeclaration;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00022120 File Offset: 0x00020320
		[SecuritySafeCritical]
		private CodeTypeReference GetReferencedType(DataContract dataContract)
		{
			Type type = null;
			CodeTypeReference codeTypeReference = this.GetSurrogatedTypeReference(dataContract);
			if (codeTypeReference != null)
			{
				return codeTypeReference;
			}
			if (this.TryGetReferencedType(dataContract.StableName, dataContract, out type) && !type.IsGenericTypeDefinition && !type.ContainsGenericParameters)
			{
				if (dataContract is XmlDataContract)
				{
					if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
					{
						XmlDataContract xmlDataContract = (XmlDataContract)dataContract;
						if (xmlDataContract.IsTypeDefinedOnImport)
						{
							if (!xmlDataContract.Equals(this.dataContractSet.GetDataContract(type)))
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced type '{0}' does not match the expected type '{1}' in '{2}' namespace.", new object[]
								{
									type.AssemblyQualifiedName,
									dataContract.StableName.Name,
									dataContract.StableName.Namespace
								})));
							}
						}
						else
						{
							xmlDataContract.IsValueType = type.IsValueType;
							xmlDataContract.IsTypeDefinedOnImport = true;
						}
						return this.GetCodeTypeReference(type);
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' must be IXmlSerializable. Contract type: '{1}', contract name: '{2}' in '{3}' namespace.", new object[]
					{
						DataContract.GetClrTypeFullName(type),
						DataContract.GetClrTypeFullName(Globals.TypeOfIXmlSerializable),
						dataContract.StableName.Name,
						dataContract.StableName.Namespace
					})));
				}
				else
				{
					if (this.dataContractSet.GetDataContract(type).Equals(dataContract))
					{
						codeTypeReference = this.GetCodeTypeReference(type);
						codeTypeReference.UserData.Add(CodeExporter.codeUserDataActualTypeKey, type);
						return codeTypeReference;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced type '{0}' does not match the expected type '{1}' in '{2}' namespace.", new object[]
					{
						type.AssemblyQualifiedName,
						dataContract.StableName.Name,
						dataContract.StableName.Namespace
					})));
				}
			}
			else
			{
				if (dataContract.GenericInfo == null)
				{
					return this.GetReferencedCollectionType(dataContract as CollectionDataContract);
				}
				XmlQualifiedName expandedStableName = dataContract.GenericInfo.GetExpandedStableName();
				if (expandedStableName != dataContract.StableName)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Generic type name mismatch. Expected '{0}' in '{1}' namespace, got '{2}' in '{3}' namespace instead.", new object[]
					{
						dataContract.StableName.Name,
						dataContract.StableName.Namespace,
						expandedStableName.Name,
						expandedStableName.Namespace
					})));
				}
				DataContract dataContract2;
				codeTypeReference = this.GetReferencedGenericType(dataContract.GenericInfo, out dataContract2);
				if (dataContract2 != null && !dataContract2.Equals(dataContract))
				{
					type = (Type)codeTypeReference.UserData[CodeExporter.codeUserDataActualTypeKey];
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced type '{0}' does not match the expected type '{1}' in '{2}' namespace.", new object[]
					{
						type.AssemblyQualifiedName,
						dataContract2.StableName.Name,
						dataContract2.StableName.Namespace
					})));
				}
				return codeTypeReference;
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000223AC File Offset: 0x000205AC
		private CodeTypeReference GetReferencedCollectionType(CollectionDataContract collectionContract)
		{
			if (collectionContract == null)
			{
				return null;
			}
			if (this.HasDefaultCollectionNames(collectionContract))
			{
				CodeTypeReference codeTypeReference;
				if (!this.TryGetReferencedDictionaryType(collectionContract, out codeTypeReference))
				{
					DataContract itemContract = collectionContract.ItemContract;
					if (collectionContract.IsDictionary)
					{
						this.GenerateKeyValueType(itemContract as ClassDataContract);
					}
					bool isItemTypeNullable = collectionContract.IsItemTypeNullable;
					if (!this.TryGetReferencedListType(itemContract, isItemTypeNullable, out codeTypeReference))
					{
						codeTypeReference = new CodeTypeReference(this.GetElementTypeReference(itemContract, isItemTypeNullable), 1);
					}
				}
				return codeTypeReference;
			}
			return null;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00022414 File Offset: 0x00020614
		private bool HasDefaultCollectionNames(CollectionDataContract collectionContract)
		{
			DataContract itemContract = collectionContract.ItemContract;
			if (collectionContract.ItemName != itemContract.StableName.Name)
			{
				return false;
			}
			if (collectionContract.IsDictionary && (collectionContract.KeyName != "Key" || collectionContract.ValueName != "Value"))
			{
				return false;
			}
			XmlQualifiedName arrayTypeName = itemContract.GetArrayTypeName(collectionContract.IsItemTypeNullable);
			return collectionContract.StableName.Name == arrayTypeName.Name && collectionContract.StableName.Namespace == arrayTypeName.Namespace;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000224B0 File Offset: 0x000206B0
		private bool TryGetReferencedDictionaryType(CollectionDataContract collectionContract, out CodeTypeReference typeReference)
		{
			if (collectionContract.IsDictionary && this.SupportsGenericTypeReference)
			{
				Type typeOfDictionaryGeneric;
				if (!this.TryGetReferencedType(this.GenericDictionaryName, this.GenericDictionaryContract, out typeOfDictionaryGeneric))
				{
					typeOfDictionaryGeneric = Globals.TypeOfDictionaryGeneric;
				}
				ClassDataContract classDataContract = collectionContract.ItemContract as ClassDataContract;
				DataMember dataMember = classDataContract.Members[0];
				DataMember dataMember2 = classDataContract.Members[1];
				CodeTypeReference elementTypeReference = this.GetElementTypeReference(dataMember.MemberTypeContract, dataMember.IsNullable);
				CodeTypeReference elementTypeReference2 = this.GetElementTypeReference(dataMember2.MemberTypeContract, dataMember2.IsNullable);
				if (elementTypeReference != null && elementTypeReference2 != null)
				{
					typeReference = this.GetCodeTypeReference(typeOfDictionaryGeneric);
					typeReference.TypeArguments.Add(elementTypeReference);
					typeReference.TypeArguments.Add(elementTypeReference2);
					return true;
				}
			}
			typeReference = null;
			return false;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00022570 File Offset: 0x00020770
		private bool TryGetReferencedListType(DataContract itemContract, bool isItemTypeNullable, out CodeTypeReference typeReference)
		{
			Type type;
			if (this.SupportsGenericTypeReference && this.TryGetReferencedType(this.GenericListName, this.GenericListContract, out type))
			{
				typeReference = this.GetCodeTypeReference(type);
				typeReference.TypeArguments.Add(this.GetElementTypeReference(itemContract, isItemTypeNullable));
				return true;
			}
			typeReference = null;
			return false;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000225C0 File Offset: 0x000207C0
		private CodeTypeReference GetSurrogatedTypeReference(DataContract dataContract)
		{
			IDataContractSurrogate dataContractSurrogate = this.dataContractSet.DataContractSurrogate;
			if (dataContractSurrogate != null)
			{
				Type referencedTypeOnImport = DataContractSurrogateCaller.GetReferencedTypeOnImport(dataContractSurrogate, dataContract.StableName.Name, dataContract.StableName.Namespace, this.dataContractSet.GetSurrogateData(dataContract));
				if (referencedTypeOnImport != null)
				{
					CodeTypeReference codeTypeReference = this.GetCodeTypeReference(referencedTypeOnImport);
					codeTypeReference.UserData.Add(CodeExporter.codeUserDataActualTypeKey, referencedTypeOnImport);
					return codeTypeReference;
				}
			}
			return null;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00022628 File Offset: 0x00020828
		private CodeTypeReference GetReferencedGenericType(GenericInfo genInfo, out DataContract dataContract)
		{
			dataContract = null;
			if (!this.SupportsGenericTypeReference)
			{
				return null;
			}
			Type type;
			if (this.TryGetReferencedType(genInfo.StableName, null, out type))
			{
				bool flag = type != Globals.TypeOfNullable;
				CodeTypeReference codeTypeReference = this.GetCodeTypeReference(type);
				codeTypeReference.UserData.Add(CodeExporter.codeUserDataActualTypeKey, type);
				if (genInfo.Parameters != null)
				{
					DataContract[] array = new DataContract[genInfo.Parameters.Count];
					for (int i = 0; i < genInfo.Parameters.Count; i++)
					{
						GenericInfo genericInfo = genInfo.Parameters[i];
						XmlQualifiedName expandedStableName = genericInfo.GetExpandedStableName();
						DataContract dataContract2 = this.dataContractSet[expandedStableName];
						CodeTypeReference codeTypeReference2;
						bool flag2;
						if (dataContract2 != null)
						{
							codeTypeReference2 = this.GetCodeTypeReference(dataContract2);
							flag2 = dataContract2.IsValueType;
						}
						else
						{
							codeTypeReference2 = this.GetReferencedGenericType(genericInfo, out dataContract2);
							flag2 = codeTypeReference2 != null && codeTypeReference2.ArrayRank == 0;
						}
						array[i] = dataContract2;
						if (dataContract2 == null)
						{
							flag = false;
						}
						if (codeTypeReference2 == null)
						{
							return null;
						}
						if (type == Globals.TypeOfNullable && !flag2)
						{
							return codeTypeReference2;
						}
						codeTypeReference.TypeArguments.Add(codeTypeReference2);
					}
					if (flag)
					{
						dataContract = DataContract.GetDataContract(type).BindGenericParameters(array, new Dictionary<DataContract, DataContract>());
					}
				}
				return codeTypeReference;
			}
			if (genInfo.Parameters != null)
			{
				return null;
			}
			dataContract = this.dataContractSet[genInfo.StableName];
			if (dataContract == null)
			{
				return null;
			}
			if (dataContract.GenericInfo != null)
			{
				return null;
			}
			return this.GetCodeTypeReference(dataContract);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00022798 File Offset: 0x00020998
		private bool NamespaceContainsType(CodeNamespace ns, string typeName)
		{
			foreach (object obj in ns.Types)
			{
				CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj;
				if (string.Compare(typeName, codeTypeDeclaration.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00022800 File Offset: 0x00020A00
		private bool GlobalTypeNameConflicts(string clrNamespace, string typeName)
		{
			return string.IsNullOrEmpty(clrNamespace) && this.clrNamespaces.ContainsKey(typeName);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00022818 File Offset: 0x00020A18
		private void AddGlobalTypeName(string typeName)
		{
			if (!this.clrNamespaces.ContainsKey(typeName))
			{
				this.clrNamespaces.Add(typeName, null);
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00022838 File Offset: 0x00020A38
		private bool TypeContainsNestedType(CodeTypeDeclaration containingType, string typeName)
		{
			foreach (object obj in containingType.Members)
			{
				CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
				if (codeTypeMember is CodeTypeDeclaration && string.Compare(typeName, ((CodeTypeDeclaration)codeTypeMember).Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000228B0 File Offset: 0x00020AB0
		private string GetNameForAttribute(string name)
		{
			string text = XmlConvert.DecodeName(name);
			if (string.CompareOrdinal(name, text) == 0)
			{
				return name;
			}
			string text2 = DataContract.EncodeLocalName(text);
			if (string.CompareOrdinal(name, text2) != 0)
			{
				return name;
			}
			return text;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000228E2 File Offset: 0x00020AE2
		private void AddSerializableAttribute(bool generateSerializable, CodeTypeDeclaration type, ContractCodeDomInfo contractCodeDomInfo)
		{
			if (generateSerializable)
			{
				type.CustomAttributes.Add(this.SerializableAttribute);
				this.AddImportStatement(Globals.TypeOfSerializableAttribute.Namespace, contractCodeDomInfo.CodeNamespace);
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00022910 File Offset: 0x00020B10
		private void ExportClassDataContract(ClassDataContract classDataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			this.GenerateType(classDataContract, contractCodeDomInfo);
			if (contractCodeDomInfo.ReferencedTypeExists)
			{
				return;
			}
			CodeTypeDeclaration typeDeclaration = contractCodeDomInfo.TypeDeclaration;
			if (this.SupportsPartialTypes)
			{
				typeDeclaration.IsPartial = true;
			}
			if (classDataContract.IsValueType && this.SupportsDeclareValueTypes)
			{
				typeDeclaration.IsStruct = true;
			}
			else
			{
				typeDeclaration.IsClass = true;
			}
			string nameForAttribute = this.GetNameForAttribute(classDataContract.StableName.Name);
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfDataContractAttribute));
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(nameForAttribute)));
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(classDataContract.StableName.Namespace)));
			if (classDataContract.IsReference)
			{
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("IsReference", new CodePrimitiveExpression(classDataContract.IsReference)));
			}
			typeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
			this.AddImportStatement(Globals.TypeOfDataContractAttribute.Namespace, contractCodeDomInfo.CodeNamespace);
			this.AddSerializableAttribute(this.GenerateSerializableTypes, typeDeclaration, contractCodeDomInfo);
			this.AddKnownTypes(classDataContract, contractCodeDomInfo);
			bool flag = this.EnableDataBinding && this.SupportsDeclareEvents;
			if (classDataContract.BaseContract == null)
			{
				if (!typeDeclaration.IsStruct)
				{
					typeDeclaration.BaseTypes.Add(Globals.TypeOfObject);
				}
				this.AddExtensionData(contractCodeDomInfo);
				this.AddPropertyChangedNotifier(contractCodeDomInfo, typeDeclaration.IsStruct);
			}
			else
			{
				ContractCodeDomInfo contractCodeDomInfo2 = this.GetContractCodeDomInfo(classDataContract.BaseContract);
				typeDeclaration.BaseTypes.Add(contractCodeDomInfo2.TypeReference);
				this.AddBaseMemberNames(contractCodeDomInfo2, contractCodeDomInfo);
				if (contractCodeDomInfo2.ReferencedTypeExists)
				{
					Type type = (Type)contractCodeDomInfo2.TypeReference.UserData[CodeExporter.codeUserDataActualTypeKey];
					this.ThrowIfReferencedBaseTypeSealed(type, classDataContract);
					if (!Globals.TypeOfIExtensibleDataObject.IsAssignableFrom(type))
					{
						this.AddExtensionData(contractCodeDomInfo);
					}
					if (!Globals.TypeOfIPropertyChange.IsAssignableFrom(type))
					{
						this.AddPropertyChangedNotifier(contractCodeDomInfo, typeDeclaration.IsStruct);
					}
					else
					{
						flag = false;
					}
				}
			}
			if (classDataContract.Members != null)
			{
				for (int i = 0; i < classDataContract.Members.Count; i++)
				{
					DataMember dataMember = classDataContract.Members[i];
					CodeTypeReference elementTypeReference = this.GetElementTypeReference(dataMember.MemberTypeContract, dataMember.IsNullable && dataMember.MemberTypeContract.IsValueType);
					string nameForAttribute2 = this.GetNameForAttribute(dataMember.Name);
					string memberName = this.GetMemberName(nameForAttribute2, contractCodeDomInfo);
					string memberName2 = this.GetMemberName(CodeExporter.AppendToValidClrIdentifier(memberName, "Field"), contractCodeDomInfo);
					CodeMemberField codeMemberField = new CodeMemberField();
					codeMemberField.Type = elementTypeReference;
					codeMemberField.Name = memberName2;
					codeMemberField.Attributes = MemberAttributes.Private;
					CodeMemberProperty codeMemberProperty = this.CreateProperty(elementTypeReference, memberName, memberName2, dataMember.MemberTypeContract.IsValueType && this.SupportsDeclareValueTypes, flag);
					if (this.dataContractSet.DataContractSurrogate != null)
					{
						codeMemberProperty.UserData.Add(CodeExporter.surrogateDataKey, this.dataContractSet.GetSurrogateData(dataMember));
					}
					CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfDataMemberAttribute));
					if (nameForAttribute2 != codeMemberProperty.Name)
					{
						codeAttributeDeclaration2.Arguments.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(nameForAttribute2)));
					}
					if (dataMember.IsRequired)
					{
						codeAttributeDeclaration2.Arguments.Add(new CodeAttributeArgument("IsRequired", new CodePrimitiveExpression(dataMember.IsRequired)));
					}
					if (!dataMember.EmitDefaultValue)
					{
						codeAttributeDeclaration2.Arguments.Add(new CodeAttributeArgument("EmitDefaultValue", new CodePrimitiveExpression(dataMember.EmitDefaultValue)));
					}
					if (dataMember.Order != 0)
					{
						codeAttributeDeclaration2.Arguments.Add(new CodeAttributeArgument("Order", new CodePrimitiveExpression(dataMember.Order)));
					}
					codeMemberProperty.CustomAttributes.Add(codeAttributeDeclaration2);
					if (this.GenerateSerializableTypes && !dataMember.IsRequired)
					{
						CodeAttributeDeclaration codeAttributeDeclaration3 = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfOptionalFieldAttribute));
						codeMemberField.CustomAttributes.Add(codeAttributeDeclaration3);
					}
					typeDeclaration.Members.Add(codeMemberField);
					typeDeclaration.Members.Add(codeMemberProperty);
				}
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00022D3B File Offset: 0x00020F3B
		private bool CanDeclareAssemblyAttribute(ContractCodeDomInfo contractCodeDomInfo)
		{
			return this.SupportsAssemblyAttributes && !contractCodeDomInfo.UsesWildcardNamespace;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00022D50 File Offset: 0x00020F50
		private bool NeedsExplicitNamespace(string dataContractNamespace, string clrNamespace)
		{
			return DataContract.GetDefaultStableNamespace(clrNamespace) != dataContractNamespace;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00022D60 File Offset: 0x00020F60
		internal ICollection<CodeTypeReference> GetKnownTypeReferences(DataContract dataContract)
		{
			Dictionary<XmlQualifiedName, DataContract> knownTypeContracts = this.GetKnownTypeContracts(dataContract);
			if (knownTypeContracts == null)
			{
				return null;
			}
			ICollection<DataContract> values = knownTypeContracts.Values;
			if (values == null || values.Count == 0)
			{
				return null;
			}
			List<CodeTypeReference> list = new List<CodeTypeReference>();
			foreach (DataContract dataContract2 in values)
			{
				list.Add(this.GetCodeTypeReference(dataContract2));
			}
			return list;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00022DD8 File Offset: 0x00020FD8
		private Dictionary<XmlQualifiedName, DataContract> GetKnownTypeContracts(DataContract dataContract)
		{
			if (this.dataContractSet.KnownTypesForObject != null && SchemaImporter.IsObjectContract(dataContract))
			{
				return this.dataContractSet.KnownTypesForObject;
			}
			if (dataContract is ClassDataContract)
			{
				ContractCodeDomInfo contractCodeDomInfo = this.GetContractCodeDomInfo(dataContract);
				if (!contractCodeDomInfo.IsProcessed)
				{
					this.GenerateType(dataContract, contractCodeDomInfo);
				}
				if (contractCodeDomInfo.ReferencedTypeExists)
				{
					return this.GetKnownTypeContracts((ClassDataContract)dataContract, new Dictionary<DataContract, object>());
				}
			}
			return null;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00022E44 File Offset: 0x00021044
		private Dictionary<XmlQualifiedName, DataContract> GetKnownTypeContracts(ClassDataContract dataContract, Dictionary<DataContract, object> handledContracts)
		{
			if (handledContracts.ContainsKey(dataContract))
			{
				return dataContract.KnownDataContracts;
			}
			handledContracts.Add(dataContract, null);
			if (dataContract.Members != null)
			{
				bool flag = false;
				foreach (DataMember dataMember in dataContract.Members)
				{
					DataContract memberTypeContract = dataMember.MemberTypeContract;
					if (!flag && this.dataContractSet.KnownTypesForObject != null && SchemaImporter.IsObjectContract(memberTypeContract))
					{
						this.AddKnownTypeContracts(dataContract, this.dataContractSet.KnownTypesForObject);
						flag = true;
					}
					else if (memberTypeContract is ClassDataContract)
					{
						ContractCodeDomInfo contractCodeDomInfo = this.GetContractCodeDomInfo(memberTypeContract);
						if (!contractCodeDomInfo.IsProcessed)
						{
							this.GenerateType(memberTypeContract, contractCodeDomInfo);
						}
						if (contractCodeDomInfo.ReferencedTypeExists)
						{
							this.AddKnownTypeContracts(dataContract, this.GetKnownTypeContracts((ClassDataContract)memberTypeContract, handledContracts));
						}
					}
				}
			}
			return dataContract.KnownDataContracts;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00022F2C File Offset: 0x0002112C
		[SecuritySafeCritical]
		private void AddKnownTypeContracts(ClassDataContract dataContract, Dictionary<XmlQualifiedName, DataContract> knownContracts)
		{
			if (knownContracts == null || knownContracts.Count == 0)
			{
				return;
			}
			if (dataContract.KnownDataContracts == null)
			{
				dataContract.KnownDataContracts = new Dictionary<XmlQualifiedName, DataContract>();
			}
			foreach (KeyValuePair<XmlQualifiedName, DataContract> keyValuePair in knownContracts)
			{
				if (dataContract.StableName != keyValuePair.Key && !dataContract.KnownDataContracts.ContainsKey(keyValuePair.Key) && !keyValuePair.Value.IsBuiltInDataContract)
				{
					dataContract.KnownDataContracts.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00022FE4 File Offset: 0x000211E4
		private void AddKnownTypes(ClassDataContract dataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			Dictionary<XmlQualifiedName, DataContract> knownTypeContracts = this.GetKnownTypeContracts(dataContract, new Dictionary<DataContract, object>());
			if (knownTypeContracts == null || knownTypeContracts.Count == 0)
			{
				return;
			}
			foreach (DataContract dataContract2 in ((IEnumerable<DataContract>)knownTypeContracts.Values))
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfKnownTypeAttribute));
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodeTypeOfExpression(this.GetCodeTypeReference(dataContract2))));
				contractCodeDomInfo.TypeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
			}
			this.AddImportStatement(Globals.TypeOfKnownTypeAttribute.Namespace, contractCodeDomInfo.CodeNamespace);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00023098 File Offset: 0x00021298
		private CodeTypeReference WrapNullable(CodeTypeReference memberType)
		{
			if (!this.SupportsGenericTypeReference)
			{
				return memberType;
			}
			CodeTypeReference codeTypeReference = this.GetCodeTypeReference(Globals.TypeOfNullable);
			codeTypeReference.TypeArguments.Add(memberType);
			return codeTypeReference;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000230BC File Offset: 0x000212BC
		private void AddExtensionData(ContractCodeDomInfo contractCodeDomInfo)
		{
			if (contractCodeDomInfo != null && contractCodeDomInfo.TypeDeclaration != null)
			{
				CodeTypeDeclaration typeDeclaration = contractCodeDomInfo.TypeDeclaration;
				typeDeclaration.BaseTypes.Add(DataContract.GetClrTypeFullName(Globals.TypeOfIExtensibleDataObject));
				CodeMemberField extensionDataObjectField = this.ExtensionDataObjectField;
				if (this.GenerateSerializableTypes)
				{
					CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfNonSerializedAttribute));
					extensionDataObjectField.CustomAttributes.Add(codeAttributeDeclaration);
				}
				typeDeclaration.Members.Add(extensionDataObjectField);
				contractCodeDomInfo.GetMemberNames().Add(extensionDataObjectField.Name, null);
				CodeMemberProperty extensionDataObjectProperty = this.ExtensionDataObjectProperty;
				typeDeclaration.Members.Add(extensionDataObjectProperty);
				contractCodeDomInfo.GetMemberNames().Add(extensionDataObjectProperty.Name, null);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00023168 File Offset: 0x00021368
		private void AddPropertyChangedNotifier(ContractCodeDomInfo contractCodeDomInfo, bool isValueType)
		{
			if (this.EnableDataBinding && this.SupportsDeclareEvents && contractCodeDomInfo != null && contractCodeDomInfo.TypeDeclaration != null)
			{
				CodeTypeDeclaration typeDeclaration = contractCodeDomInfo.TypeDeclaration;
				typeDeclaration.BaseTypes.Add(this.CodeTypeIPropertyChange);
				CodeMemberEvent propertyChangedEvent = this.PropertyChangedEvent;
				typeDeclaration.Members.Add(propertyChangedEvent);
				CodeMemberMethod raisePropertyChangedEventMethod = this.RaisePropertyChangedEventMethod;
				if (!isValueType)
				{
					raisePropertyChangedEventMethod.Attributes |= MemberAttributes.Family;
				}
				typeDeclaration.Members.Add(raisePropertyChangedEventMethod);
				contractCodeDomInfo.GetMemberNames().Add(propertyChangedEvent.Name, null);
				contractCodeDomInfo.GetMemberNames().Add(raisePropertyChangedEventMethod.Name, null);
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00023214 File Offset: 0x00021414
		private void ThrowIfReferencedBaseTypeSealed(Type baseType, DataContract dataContract)
		{
			if (baseType.IsSealed)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Cannod drive from sealed reference type '{2}', for '{0}' element in '{1}' namespace.", new object[]
				{
					dataContract.StableName.Name,
					dataContract.StableName.Namespace,
					DataContract.GetClrTypeFullName(baseType)
				})));
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002326C File Offset: 0x0002146C
		private void ExportEnumDataContract(EnumDataContract enumDataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			this.GenerateType(enumDataContract, contractCodeDomInfo);
			if (contractCodeDomInfo.ReferencedTypeExists)
			{
				return;
			}
			CodeTypeDeclaration typeDeclaration = contractCodeDomInfo.TypeDeclaration;
			typeDeclaration.IsEnum = true;
			typeDeclaration.BaseTypes.Add(EnumDataContract.GetBaseType(enumDataContract.BaseContractName));
			if (enumDataContract.IsFlags)
			{
				typeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfFlagsAttribute)));
				this.AddImportStatement(Globals.TypeOfFlagsAttribute.Namespace, contractCodeDomInfo.CodeNamespace);
			}
			string nameForAttribute = this.GetNameForAttribute(enumDataContract.StableName.Name);
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfDataContractAttribute));
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(nameForAttribute)));
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(enumDataContract.StableName.Namespace)));
			typeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
			this.AddImportStatement(Globals.TypeOfDataContractAttribute.Namespace, contractCodeDomInfo.CodeNamespace);
			if (enumDataContract.Members != null)
			{
				for (int i = 0; i < enumDataContract.Members.Count; i++)
				{
					string name = enumDataContract.Members[i].Name;
					long num = enumDataContract.Values[i];
					CodeMemberField codeMemberField = new CodeMemberField();
					if (enumDataContract.IsULong)
					{
						codeMemberField.InitExpression = new CodeSnippetExpression(enumDataContract.GetStringFromEnumValue(num));
					}
					else
					{
						codeMemberField.InitExpression = new CodePrimitiveExpression(num);
					}
					codeMemberField.Name = this.GetMemberName(name, contractCodeDomInfo);
					CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfEnumMemberAttribute));
					if (codeMemberField.Name != name)
					{
						codeAttributeDeclaration2.Arguments.Add(new CodeAttributeArgument("Value", new CodePrimitiveExpression(name)));
					}
					codeMemberField.CustomAttributes.Add(codeAttributeDeclaration2);
					typeDeclaration.Members.Add(codeMemberField);
				}
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002345C File Offset: 0x0002165C
		private void ExportISerializableDataContract(ClassDataContract dataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			this.GenerateType(dataContract, contractCodeDomInfo);
			if (contractCodeDomInfo.ReferencedTypeExists)
			{
				return;
			}
			if (DataContract.GetDefaultStableNamespace(contractCodeDomInfo.ClrNamespace) != dataContract.StableName.Namespace)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Invalid CLR namespace '{3}' is generated for ISerializable type '{0}' in '{1}' namespace. Data contract namespace from the URI would be generated as '{2}'.", new object[]
				{
					dataContract.StableName.Name,
					dataContract.StableName.Namespace,
					DataContract.GetDataContractNamespaceFromUri(dataContract.StableName.Namespace),
					contractCodeDomInfo.ClrNamespace
				})));
			}
			string nameForAttribute = this.GetNameForAttribute(dataContract.StableName.Name);
			int num = nameForAttribute.LastIndexOf('.');
			string text = ((num <= 0 || num == nameForAttribute.Length - 1) ? nameForAttribute : nameForAttribute.Substring(num + 1));
			if (contractCodeDomInfo.TypeDeclaration.Name != text)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Invalid CLR name '{2}' is generated for ISerializable type '{0}' in '{1}' namespace.", new object[]
				{
					dataContract.StableName.Name,
					dataContract.StableName.Namespace,
					contractCodeDomInfo.TypeDeclaration.Name
				})));
			}
			CodeTypeDeclaration typeDeclaration = contractCodeDomInfo.TypeDeclaration;
			if (this.SupportsPartialTypes)
			{
				typeDeclaration.IsPartial = true;
			}
			if (dataContract.IsValueType && this.SupportsDeclareValueTypes)
			{
				typeDeclaration.IsStruct = true;
			}
			else
			{
				typeDeclaration.IsClass = true;
			}
			this.AddSerializableAttribute(true, typeDeclaration, contractCodeDomInfo);
			this.AddKnownTypes(dataContract, contractCodeDomInfo);
			if (dataContract.BaseContract == null)
			{
				if (!typeDeclaration.IsStruct)
				{
					typeDeclaration.BaseTypes.Add(Globals.TypeOfObject);
				}
				typeDeclaration.BaseTypes.Add(DataContract.GetClrTypeFullName(Globals.TypeOfISerializable));
				typeDeclaration.Members.Add(this.ISerializableBaseConstructor);
				typeDeclaration.Members.Add(this.SerializationInfoField);
				typeDeclaration.Members.Add(this.SerializationInfoProperty);
				typeDeclaration.Members.Add(this.GetObjectDataMethod);
				this.AddPropertyChangedNotifier(contractCodeDomInfo, typeDeclaration.IsStruct);
				return;
			}
			ContractCodeDomInfo contractCodeDomInfo2 = this.GetContractCodeDomInfo(dataContract.BaseContract);
			this.GenerateType(dataContract.BaseContract, contractCodeDomInfo2);
			typeDeclaration.BaseTypes.Add(contractCodeDomInfo2.TypeReference);
			if (contractCodeDomInfo2.ReferencedTypeExists)
			{
				Type type = (Type)contractCodeDomInfo2.TypeReference.UserData[CodeExporter.codeUserDataActualTypeKey];
				this.ThrowIfReferencedBaseTypeSealed(type, dataContract);
			}
			typeDeclaration.Members.Add(this.ISerializableDerivedConstructor);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000236C0 File Offset: 0x000218C0
		private void GenerateKeyValueType(ClassDataContract keyValueContract)
		{
			if (keyValueContract != null && this.dataContractSet[keyValueContract.StableName] == null && this.dataContractSet.GetContractCodeDomInfo(keyValueContract) == null)
			{
				ContractCodeDomInfo contractCodeDomInfo = new ContractCodeDomInfo();
				this.dataContractSet.SetContractCodeDomInfo(keyValueContract, contractCodeDomInfo);
				this.ExportClassDataContract(keyValueContract, contractCodeDomInfo);
				contractCodeDomInfo.IsProcessed = true;
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00023718 File Offset: 0x00021918
		private void ExportCollectionDataContract(CollectionDataContract collectionContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			this.GenerateType(collectionContract, contractCodeDomInfo);
			if (contractCodeDomInfo.ReferencedTypeExists)
			{
				return;
			}
			string nameForAttribute = this.GetNameForAttribute(collectionContract.StableName.Name);
			if (!this.SupportsGenericTypeReference)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("For '{0}' in '{1}' namespace, generic type cannot be referenced as the base type.", new object[]
				{
					nameForAttribute,
					collectionContract.StableName.Namespace
				})));
			}
			DataContract itemContract = collectionContract.ItemContract;
			bool isItemTypeNullable = collectionContract.IsItemTypeNullable;
			CodeTypeReference codeTypeReference;
			bool flag = this.TryGetReferencedDictionaryType(collectionContract, out codeTypeReference);
			if (!flag)
			{
				if (collectionContract.IsDictionary)
				{
					this.GenerateKeyValueType(collectionContract.ItemContract as ClassDataContract);
				}
				if (!this.TryGetReferencedListType(itemContract, isItemTypeNullable, out codeTypeReference))
				{
					if (!this.SupportsGenericTypeReference)
					{
						string text = "ArrayOf" + itemContract.StableName.Name;
						string collectionNamespace = DataContract.GetCollectionNamespace(itemContract.StableName.Namespace);
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced base type does not exist. Data contract name: '{0}' in '{1}' namespace, expected type: '{2}' in '{3}' namespace. Collection can be '{4}' or '{5}'.", new object[]
						{
							nameForAttribute,
							collectionContract.StableName.Namespace,
							text,
							collectionNamespace,
							DataContract.GetClrTypeFullName(Globals.TypeOfIListGeneric),
							DataContract.GetClrTypeFullName(Globals.TypeOfICollectionGeneric)
						})));
					}
					codeTypeReference = this.GetCodeTypeReference(Globals.TypeOfListGeneric);
					codeTypeReference.TypeArguments.Add(this.GetElementTypeReference(itemContract, isItemTypeNullable));
				}
			}
			CodeTypeDeclaration typeDeclaration = contractCodeDomInfo.TypeDeclaration;
			typeDeclaration.BaseTypes.Add(codeTypeReference);
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfCollectionDataContractAttribute));
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(nameForAttribute)));
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(collectionContract.StableName.Namespace)));
			if (collectionContract.IsReference)
			{
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("IsReference", new CodePrimitiveExpression(collectionContract.IsReference)));
			}
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("ItemName", new CodePrimitiveExpression(this.GetNameForAttribute(collectionContract.ItemName))));
			if (flag)
			{
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("KeyName", new CodePrimitiveExpression(this.GetNameForAttribute(collectionContract.KeyName))));
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("ValueName", new CodePrimitiveExpression(this.GetNameForAttribute(collectionContract.ValueName))));
			}
			typeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
			this.AddImportStatement(Globals.TypeOfCollectionDataContractAttribute.Namespace, contractCodeDomInfo.CodeNamespace);
			this.AddSerializableAttribute(this.GenerateSerializableTypes, typeDeclaration, contractCodeDomInfo);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000239B4 File Offset: 0x00021BB4
		private void ExportXmlDataContract(XmlDataContract xmlDataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			this.GenerateType(xmlDataContract, contractCodeDomInfo);
			if (contractCodeDomInfo.ReferencedTypeExists)
			{
				return;
			}
			CodeTypeDeclaration typeDeclaration = contractCodeDomInfo.TypeDeclaration;
			if (this.SupportsPartialTypes)
			{
				typeDeclaration.IsPartial = true;
			}
			if (xmlDataContract.IsValueType)
			{
				typeDeclaration.IsStruct = true;
			}
			else
			{
				typeDeclaration.IsClass = true;
				typeDeclaration.BaseTypes.Add(Globals.TypeOfObject);
			}
			this.AddSerializableAttribute(this.GenerateSerializableTypes, typeDeclaration, contractCodeDomInfo);
			typeDeclaration.BaseTypes.Add(DataContract.GetClrTypeFullName(Globals.TypeOfIXmlSerializable));
			typeDeclaration.Members.Add(this.NodeArrayField);
			typeDeclaration.Members.Add(this.NodeArrayProperty);
			typeDeclaration.Members.Add(this.ReadXmlMethod);
			typeDeclaration.Members.Add(this.WriteXmlMethod);
			typeDeclaration.Members.Add(this.GetSchemaMethod);
			if (xmlDataContract.IsAnonymous && !xmlDataContract.HasRoot)
			{
				typeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfXmlSchemaProviderAttribute), new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(this.NullReference),
					new CodeAttributeArgument("IsAny", new CodePrimitiveExpression(true))
				}));
			}
			else
			{
				typeDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfXmlSchemaProviderAttribute), new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression("ExportSchema"))
				}));
				CodeMemberField codeMemberField = new CodeMemberField(Globals.TypeOfXmlQualifiedName, CodeExporter.typeNameFieldName);
				codeMemberField.Attributes |= (MemberAttributes)20483;
				XmlQualifiedName xmlQualifiedName = (xmlDataContract.IsAnonymous ? SchemaImporter.ImportActualType(xmlDataContract.XsdType.Annotation, xmlDataContract.StableName, xmlDataContract.StableName) : xmlDataContract.StableName);
				codeMemberField.InitExpression = new CodeObjectCreateExpression(Globals.TypeOfXmlQualifiedName, new CodeExpression[]
				{
					new CodePrimitiveExpression(xmlQualifiedName.Name),
					new CodePrimitiveExpression(xmlQualifiedName.Namespace)
				});
				typeDeclaration.Members.Add(codeMemberField);
				typeDeclaration.Members.Add(this.GetSchemaStaticMethod);
				bool flag = (xmlDataContract.TopLevelElementName != null && xmlDataContract.TopLevelElementName.Value != xmlDataContract.StableName.Name) || (xmlDataContract.TopLevelElementNamespace != null && xmlDataContract.TopLevelElementNamespace.Value != xmlDataContract.StableName.Namespace);
				if (flag || !xmlDataContract.IsTopLevelElementNullable)
				{
					CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfXmlRootAttribute));
					if (flag)
					{
						if (xmlDataContract.TopLevelElementName != null)
						{
							codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("ElementName", new CodePrimitiveExpression(xmlDataContract.TopLevelElementName.Value)));
						}
						if (xmlDataContract.TopLevelElementNamespace != null)
						{
							codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(xmlDataContract.TopLevelElementNamespace.Value)));
						}
					}
					if (!xmlDataContract.IsTopLevelElementNullable)
					{
						codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("IsNullable", new CodePrimitiveExpression(false)));
					}
					typeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
				}
			}
			this.AddPropertyChangedNotifier(contractCodeDomInfo, typeDeclaration.IsStruct);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00023CD0 File Offset: 0x00021ED0
		private CodeNamespace GetCodeNamespace(string clrNamespace, string dataContractNamespace, ContractCodeDomInfo contractCodeDomInfo)
		{
			if (contractCodeDomInfo.CodeNamespace != null)
			{
				return contractCodeDomInfo.CodeNamespace;
			}
			CodeNamespaceCollection codeNamespaceCollection = this.codeCompileUnit.Namespaces;
			foreach (object obj in codeNamespaceCollection)
			{
				CodeNamespace codeNamespace = (CodeNamespace)obj;
				if (codeNamespace.Name == clrNamespace)
				{
					contractCodeDomInfo.CodeNamespace = codeNamespace;
					return codeNamespace;
				}
			}
			CodeNamespace codeNamespace2 = new CodeNamespace(clrNamespace);
			codeNamespaceCollection.Add(codeNamespace2);
			if (this.CanDeclareAssemblyAttribute(contractCodeDomInfo) && this.NeedsExplicitNamespace(dataContractNamespace, clrNamespace))
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(DataContract.GetClrTypeFullName(Globals.TypeOfContractNamespaceAttribute));
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(dataContractNamespace)));
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("ClrNamespace", new CodePrimitiveExpression(clrNamespace)));
				this.codeCompileUnit.AssemblyCustomAttributes.Add(codeAttributeDeclaration);
			}
			contractCodeDomInfo.CodeNamespace = codeNamespace2;
			return codeNamespace2;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00023DE0 File Offset: 0x00021FE0
		private string GetMemberName(string memberName, ContractCodeDomInfo contractCodeDomInfo)
		{
			memberName = CodeExporter.GetClrIdentifier(memberName, "GeneratedMember");
			if (memberName == contractCodeDomInfo.TypeDeclaration.Name)
			{
				memberName = CodeExporter.AppendToValidClrIdentifier(memberName, "Member");
			}
			if (contractCodeDomInfo.GetMemberNames().ContainsKey(memberName))
			{
				int num = 1;
				string text;
				for (;;)
				{
					text = CodeExporter.AppendToValidClrIdentifier(memberName, num.ToString(NumberFormatInfo.InvariantInfo));
					if (!contractCodeDomInfo.GetMemberNames().ContainsKey(text))
					{
						break;
					}
					num++;
				}
				memberName = text;
			}
			contractCodeDomInfo.GetMemberNames().Add(memberName, null);
			return memberName;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00023E68 File Offset: 0x00022068
		private void AddBaseMemberNames(ContractCodeDomInfo baseContractCodeDomInfo, ContractCodeDomInfo contractCodeDomInfo)
		{
			if (!baseContractCodeDomInfo.ReferencedTypeExists)
			{
				Dictionary<string, object> memberNames = baseContractCodeDomInfo.GetMemberNames();
				Dictionary<string, object> memberNames2 = contractCodeDomInfo.GetMemberNames();
				foreach (KeyValuePair<string, object> keyValuePair in memberNames)
				{
					memberNames2.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00023ED8 File Offset: 0x000220D8
		[SecuritySafeCritical]
		private static string GetClrIdentifier(string identifier, string defaultIdentifier)
		{
			if (identifier.Length <= 511 && CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
			{
				return identifier;
			}
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			while (num < identifier.Length && stringBuilder.Length < 511)
			{
				char c = identifier[num];
				if (CodeExporter.IsValid(c))
				{
					if (flag && !CodeExporter.IsValidStart(c))
					{
						stringBuilder.Append("_");
					}
					stringBuilder.Append(c);
					flag = false;
				}
				num++;
			}
			if (stringBuilder.Length == 0)
			{
				return defaultIdentifier;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00023F64 File Offset: 0x00022164
		private static string AppendToValidClrIdentifier(string identifier, string appendString)
		{
			int num = 511 - identifier.Length;
			int length = appendString.Length;
			if (num < length)
			{
				identifier = identifier.Substring(0, 511 - length);
			}
			identifier += appendString;
			return identifier;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00023FA4 File Offset: 0x000221A4
		private string GetClrNamespace(DataContract dataContract, ContractCodeDomInfo contractCodeDomInfo)
		{
			string text = contractCodeDomInfo.ClrNamespace;
			bool flag = false;
			if (text == null)
			{
				if (!this.Namespaces.TryGetValue(dataContract.StableName.Namespace, out text))
				{
					if (this.Namespaces.TryGetValue(CodeExporter.wildcardNamespaceMapping, out text))
					{
						flag = true;
					}
					else
					{
						text = CodeExporter.GetClrNamespace(dataContract.StableName.Namespace);
						if (this.ClrNamespaces.ContainsKey(text))
						{
							int num = 1;
							string text2;
							for (;;)
							{
								text2 = ((text.Length == 0) ? "GeneratedNamespace" : text) + num.ToString(NumberFormatInfo.InvariantInfo);
								if (!this.ClrNamespaces.ContainsKey(text2))
								{
									break;
								}
								num++;
							}
							text = text2;
						}
						this.AddNamespacePair(dataContract.StableName.Namespace, text);
					}
				}
				contractCodeDomInfo.ClrNamespace = text;
				contractCodeDomInfo.UsesWildcardNamespace = flag;
			}
			return text;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00024073 File Offset: 0x00022273
		private void AddNamespacePair(string dataContractNamespace, string clrNamespace)
		{
			this.Namespaces.Add(dataContractNamespace, clrNamespace);
			this.ClrNamespaces.Add(clrNamespace, dataContractNamespace);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00024090 File Offset: 0x00022290
		private void AddImportStatement(string clrNamespace, CodeNamespace codeNamespace)
		{
			if (clrNamespace == codeNamespace.Name)
			{
				return;
			}
			CodeNamespaceImportCollection imports = codeNamespace.Imports;
			using (IEnumerator enumerator = imports.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((CodeNamespaceImport)enumerator.Current).Namespace == clrNamespace)
					{
						return;
					}
				}
			}
			imports.Add(new CodeNamespaceImport(clrNamespace));
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00024110 File Offset: 0x00022310
		private static string GetClrNamespace(string dataContractNamespace)
		{
			if (dataContractNamespace == null || dataContractNamespace.Length == 0)
			{
				return string.Empty;
			}
			Uri uri = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (Uri.TryCreate(dataContractNamespace, UriKind.RelativeOrAbsolute, out uri))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				if (!uri.IsAbsoluteUri)
				{
					CodeExporter.AddToNamespace(stringBuilder, uri.OriginalString, dictionary);
				}
				else
				{
					string absoluteUri = uri.AbsoluteUri;
					if (absoluteUri.StartsWith("http://schemas.datacontract.org/2004/07/", StringComparison.Ordinal))
					{
						CodeExporter.AddToNamespace(stringBuilder, absoluteUri.Substring("http://schemas.datacontract.org/2004/07/".Length), dictionary);
					}
					else
					{
						string host = uri.Host;
						if (host != null)
						{
							CodeExporter.AddToNamespace(stringBuilder, host, dictionary);
						}
						string pathAndQuery = uri.PathAndQuery;
						if (pathAndQuery != null)
						{
							CodeExporter.AddToNamespace(stringBuilder, pathAndQuery, dictionary);
						}
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				return string.Empty;
			}
			int num = stringBuilder.Length;
			if (stringBuilder[stringBuilder.Length - 1] == '.')
			{
				num--;
			}
			num = Math.Min(511, num);
			return stringBuilder.ToString(0, num);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00024200 File Offset: 0x00022400
		private static void AddToNamespace(StringBuilder builder, string fragment, Dictionary<string, object> fragments)
		{
			if (fragment == null)
			{
				return;
			}
			bool flag = true;
			int num = builder.Length;
			int num2 = 0;
			int num3 = 0;
			while (num3 < fragment.Length && builder.Length < 511)
			{
				char c = fragment[num3];
				if (CodeExporter.IsValid(c))
				{
					if (flag && !CodeExporter.IsValidStart(c))
					{
						builder.Append("_");
					}
					builder.Append(c);
					num2++;
					flag = false;
				}
				else if ((c == '.' || c == '/' || c == ':') && (builder.Length == 1 || (builder.Length > 1 && builder[builder.Length - 1] != '.')))
				{
					CodeExporter.AddNamespaceFragment(builder, num, num2, fragments);
					builder.Append('.');
					num = builder.Length;
					num2 = 0;
					flag = true;
				}
				num3++;
			}
			CodeExporter.AddNamespaceFragment(builder, num, num2, fragments);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000242D8 File Offset: 0x000224D8
		private static void AddNamespaceFragment(StringBuilder builder, int fragmentOffset, int fragmentLength, Dictionary<string, object> fragments)
		{
			if (fragmentLength == 0)
			{
				return;
			}
			string text = builder.ToString(fragmentOffset, fragmentLength);
			if (fragments.ContainsKey(text))
			{
				int num = 1;
				string text2;
				string text3;
				for (;;)
				{
					text2 = num.ToString(NumberFormatInfo.InvariantInfo);
					text3 = CodeExporter.AppendToValidClrIdentifier(text, text2);
					if (!fragments.ContainsKey(text3))
					{
						break;
					}
					if (num == 2147483647)
					{
						goto Block_4;
					}
					num++;
				}
				builder.Append(text2);
				text = text3;
				goto IL_006F;
				Block_4:
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Cannot compute unique name for '{0}'.", new object[] { text })));
			}
			IL_006F:
			fragments.Add(text, null);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002435C File Offset: 0x0002255C
		private static bool IsValidStart(char c)
		{
			return char.GetUnicodeCategory(c) != UnicodeCategory.DecimalDigitNumber;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002436C File Offset: 0x0002256C
		private static bool IsValid(char c)
		{
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			return unicodeCategory <= UnicodeCategory.SpacingCombiningMark || unicodeCategory == UnicodeCategory.DecimalDigitNumber || unicodeCategory == UnicodeCategory.ConnectorPunctuation;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00024390 File Offset: 0x00022590
		private CodeTypeReference CodeTypeIPropertyChange
		{
			get
			{
				return this.GetCodeTypeReference(typeof(INotifyPropertyChanged));
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x000243A2 File Offset: 0x000225A2
		private CodeThisReferenceExpression ThisReference
		{
			get
			{
				return new CodeThisReferenceExpression();
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x000243A9 File Offset: 0x000225A9
		private CodePrimitiveExpression NullReference
		{
			get
			{
				return new CodePrimitiveExpression(null);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x000243B1 File Offset: 0x000225B1
		private CodeParameterDeclarationExpression SerializationInfoParameter
		{
			get
			{
				return new CodeParameterDeclarationExpression(this.GetCodeTypeReference(Globals.TypeOfSerializationInfo), "info");
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x000243C8 File Offset: 0x000225C8
		private CodeParameterDeclarationExpression StreamingContextParameter
		{
			get
			{
				return new CodeParameterDeclarationExpression(this.GetCodeTypeReference(Globals.TypeOfStreamingContext), "context");
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x000243DF File Offset: 0x000225DF
		private CodeAttributeDeclaration SerializableAttribute
		{
			get
			{
				return new CodeAttributeDeclaration(this.GetCodeTypeReference(Globals.TypeOfSerializableAttribute));
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x000243F1 File Offset: 0x000225F1
		private CodeMemberProperty NodeArrayProperty
		{
			get
			{
				return this.CreateProperty(this.GetCodeTypeReference(Globals.TypeOfXmlNodeArray), "Nodes", "nodesField", false);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x0002440F File Offset: 0x0002260F
		private CodeMemberField NodeArrayField
		{
			get
			{
				return new CodeMemberField
				{
					Type = this.GetCodeTypeReference(Globals.TypeOfXmlNodeArray),
					Name = "nodesField",
					Attributes = MemberAttributes.Private
				};
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00024440 File Offset: 0x00022640
		private CodeMemberMethod ReadXmlMethod
		{
			get
			{
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = "ReadXml";
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(XmlReader), "reader");
				codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
				codeMemberMethod.Attributes = (MemberAttributes)24578;
				codeMemberMethod.ImplementationTypes.Add(Globals.TypeOfIXmlSerializable);
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = new CodeFieldReferenceExpression(this.ThisReference, "nodesField");
				codeAssignStatement.Right = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(this.GetCodeTypeReference(Globals.TypeOfXmlSerializableServices)), XmlSerializableServices.ReadNodesMethodName, new CodeExpression[]
				{
					new CodeArgumentReferenceExpression(codeParameterDeclarationExpression.Name)
				});
				codeMemberMethod.Statements.Add(codeAssignStatement);
				return codeMemberMethod;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x000244FC File Offset: 0x000226FC
		private CodeMemberMethod WriteXmlMethod
		{
			get
			{
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = "WriteXml";
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(XmlWriter), "writer");
				codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
				codeMemberMethod.Attributes = (MemberAttributes)24578;
				codeMemberMethod.ImplementationTypes.Add(Globals.TypeOfIXmlSerializable);
				codeMemberMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(this.GetCodeTypeReference(Globals.TypeOfXmlSerializableServices)), XmlSerializableServices.WriteNodesMethodName, new CodeExpression[]
				{
					new CodeArgumentReferenceExpression(codeParameterDeclarationExpression.Name),
					new CodePropertyReferenceExpression(this.ThisReference, "Nodes")
				}));
				return codeMemberMethod;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x000245A8 File Offset: 0x000227A8
		private CodeMemberMethod GetSchemaMethod
		{
			get
			{
				return new CodeMemberMethod
				{
					Name = "GetSchema",
					Attributes = (MemberAttributes)24578,
					ImplementationTypes = { Globals.TypeOfIXmlSerializable },
					ReturnType = this.GetCodeTypeReference(typeof(XmlSchema)),
					Statements = 
					{
						new CodeMethodReturnStatement(this.NullReference)
					}
				};
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x00024610 File Offset: 0x00022810
		private CodeMemberMethod GetSchemaStaticMethod
		{
			get
			{
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = "ExportSchema";
				codeMemberMethod.ReturnType = this.GetCodeTypeReference(Globals.TypeOfXmlQualifiedName);
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(Globals.TypeOfXmlSchemaSet, "schemas");
				codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
				codeMemberMethod.Attributes = (MemberAttributes)24579;
				codeMemberMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(this.GetCodeTypeReference(typeof(XmlSerializableServices))), XmlSerializableServices.AddDefaultSchemaMethodName, new CodeExpression[]
				{
					new CodeArgumentReferenceExpression(codeParameterDeclarationExpression.Name),
					new CodeFieldReferenceExpression(null, CodeExporter.typeNameFieldName)
				}));
				codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(null, CodeExporter.typeNameFieldName)));
				return codeMemberMethod;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x000246D4 File Offset: 0x000228D4
		private CodeConstructor ISerializableBaseConstructor
		{
			get
			{
				CodeConstructor codeConstructor = new CodeConstructor();
				codeConstructor.Attributes = MemberAttributes.Public;
				codeConstructor.Parameters.Add(this.SerializationInfoParameter);
				codeConstructor.Parameters.Add(this.StreamingContextParameter);
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = new CodePropertyReferenceExpression(this.ThisReference, "info");
				codeAssignStatement.Right = new CodeArgumentReferenceExpression("info");
				codeConstructor.Statements.Add(codeAssignStatement);
				if (this.EnableDataBinding && this.SupportsDeclareEvents && string.CompareOrdinal(this.FileExtension, "vb") != 0)
				{
					codeConstructor.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(this.ThisReference, this.PropertyChangedEvent.Name), this.NullReference));
				}
				return codeConstructor;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x000247A0 File Offset: 0x000229A0
		private CodeConstructor ISerializableDerivedConstructor
		{
			get
			{
				return new CodeConstructor
				{
					Attributes = MemberAttributes.Public,
					Parameters = { this.SerializationInfoParameter, this.StreamingContextParameter },
					BaseConstructorArgs = 
					{
						new CodeVariableReferenceExpression("info"),
						new CodeVariableReferenceExpression("context")
					}
				};
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0002480D File Offset: 0x00022A0D
		private CodeMemberField SerializationInfoField
		{
			get
			{
				return new CodeMemberField
				{
					Type = this.GetCodeTypeReference(Globals.TypeOfSerializationInfo),
					Name = "info",
					Attributes = MemberAttributes.Private
				};
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0002483B File Offset: 0x00022A3B
		private CodeMemberProperty SerializationInfoProperty
		{
			get
			{
				return this.CreateProperty(this.GetCodeTypeReference(Globals.TypeOfSerializationInfo), "SerializationInfo", "info", false);
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0002485C File Offset: 0x00022A5C
		private CodeMemberMethod GetObjectDataMethod
		{
			get
			{
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = "GetObjectData";
				codeMemberMethod.Parameters.Add(this.SerializationInfoParameter);
				codeMemberMethod.Parameters.Add(this.StreamingContextParameter);
				codeMemberMethod.Attributes = (MemberAttributes)24578;
				codeMemberMethod.ImplementationTypes.Add(Globals.TypeOfISerializable);
				CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
				codeConditionStatement.Condition = new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(this.ThisReference, "SerializationInfo"), CodeBinaryOperatorType.IdentityEquality, this.NullReference);
				codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement());
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement();
				codeVariableDeclarationStatement.Type = this.GetCodeTypeReference(Globals.TypeOfSerializationInfoEnumerator);
				codeVariableDeclarationStatement.Name = "enumerator";
				codeVariableDeclarationStatement.InitExpression = new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(this.ThisReference, "SerializationInfo"), "GetEnumerator", Array.Empty<CodeExpression>());
				CodeVariableDeclarationStatement codeVariableDeclarationStatement2 = new CodeVariableDeclarationStatement();
				codeVariableDeclarationStatement2.Type = this.GetCodeTypeReference(Globals.TypeOfSerializationEntry);
				codeVariableDeclarationStatement2.Name = "entry";
				codeVariableDeclarationStatement2.InitExpression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("enumerator"), "Current");
				CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement();
				CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("entry"), "Name");
				CodePropertyReferenceExpression codePropertyReferenceExpression2 = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("entry"), "Value");
				codeExpressionStatement.Expression = new CodeMethodInvokeExpression(new CodeArgumentReferenceExpression("info"), "AddValue", new CodeExpression[] { codePropertyReferenceExpression, codePropertyReferenceExpression2 });
				CodeIterationStatement codeIterationStatement = new CodeIterationStatement();
				codeIterationStatement.TestExpression = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("enumerator"), "MoveNext", Array.Empty<CodeExpression>());
				codeIterationStatement.InitStatement = (codeIterationStatement.IncrementStatement = new CodeSnippetStatement(string.Empty));
				codeIterationStatement.Statements.Add(codeVariableDeclarationStatement2);
				codeIterationStatement.Statements.Add(codeExpressionStatement);
				codeMemberMethod.Statements.Add(codeConditionStatement);
				codeMemberMethod.Statements.Add(codeVariableDeclarationStatement);
				codeMemberMethod.Statements.Add(codeIterationStatement);
				return codeMemberMethod;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00024A5D File Offset: 0x00022C5D
		private CodeMemberField ExtensionDataObjectField
		{
			get
			{
				return new CodeMemberField
				{
					Type = this.GetCodeTypeReference(Globals.TypeOfExtensionDataObject),
					Name = "extensionDataField",
					Attributes = MemberAttributes.Private
				};
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00024A8C File Offset: 0x00022C8C
		private CodeMemberProperty ExtensionDataObjectProperty
		{
			get
			{
				CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
				codeMemberProperty.Type = this.GetCodeTypeReference(Globals.TypeOfExtensionDataObject);
				codeMemberProperty.Name = "ExtensionData";
				codeMemberProperty.Attributes = (MemberAttributes)24578;
				codeMemberProperty.ImplementationTypes.Add(Globals.TypeOfIExtensibleDataObject);
				CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement();
				codeMethodReturnStatement.Expression = new CodeFieldReferenceExpression(this.ThisReference, "extensionDataField");
				codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = new CodeFieldReferenceExpression(this.ThisReference, "extensionDataField");
				codeAssignStatement.Right = new CodePropertySetValueReferenceExpression();
				codeMemberProperty.SetStatements.Add(codeAssignStatement);
				return codeMemberProperty;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00024B34 File Offset: 0x00022D34
		private CodeMemberMethod RaisePropertyChangedEventMethod
		{
			get
			{
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = "RaisePropertyChanged";
				codeMemberMethod.Attributes = MemberAttributes.Final;
				CodeArgumentReferenceExpression codeArgumentReferenceExpression = new CodeArgumentReferenceExpression("propertyName");
				codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), codeArgumentReferenceExpression.ParameterName));
				CodeVariableReferenceExpression codeVariableReferenceExpression = new CodeVariableReferenceExpression("propertyChanged");
				codeMemberMethod.Statements.Add(new CodeVariableDeclarationStatement(typeof(PropertyChangedEventHandler), codeVariableReferenceExpression.VariableName, new CodeEventReferenceExpression(this.ThisReference, this.PropertyChangedEvent.Name)));
				CodeConditionStatement codeConditionStatement = new CodeConditionStatement(new CodeBinaryOperatorExpression(codeVariableReferenceExpression, CodeBinaryOperatorType.IdentityInequality, this.NullReference), Array.Empty<CodeStatement>());
				codeMemberMethod.Statements.Add(codeConditionStatement);
				codeConditionStatement.TrueStatements.Add(new CodeDelegateInvokeExpression(codeVariableReferenceExpression, new CodeExpression[]
				{
					this.ThisReference,
					new CodeObjectCreateExpression(typeof(PropertyChangedEventArgs), new CodeExpression[] { codeArgumentReferenceExpression })
				}));
				return codeMemberMethod;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x00024C2C File Offset: 0x00022E2C
		private CodeMemberEvent PropertyChangedEvent
		{
			get
			{
				return new CodeMemberEvent
				{
					Attributes = MemberAttributes.Public,
					Name = "PropertyChanged",
					Type = this.GetCodeTypeReference(typeof(PropertyChangedEventHandler)),
					ImplementationTypes = { Globals.TypeOfIPropertyChange }
				};
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00024C7A File Offset: 0x00022E7A
		private CodeMemberProperty CreateProperty(CodeTypeReference type, string propertyName, string fieldName, bool isValueType)
		{
			return this.CreateProperty(type, propertyName, fieldName, isValueType, this.EnableDataBinding && this.SupportsDeclareEvents);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00024C98 File Offset: 0x00022E98
		private CodeMemberProperty CreateProperty(CodeTypeReference type, string propertyName, string fieldName, bool isValueType, bool raisePropertyChanged)
		{
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Type = type;
			codeMemberProperty.Name = propertyName;
			codeMemberProperty.Attributes = MemberAttributes.Final;
			if (this.GenerateInternalTypes)
			{
				codeMemberProperty.Attributes |= MemberAttributes.Assembly;
			}
			else
			{
				codeMemberProperty.Attributes |= MemberAttributes.Public;
			}
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement();
			codeMethodReturnStatement.Expression = new CodeFieldReferenceExpression(this.ThisReference, fieldName);
			codeMemberProperty.GetStatements.Add(codeMethodReturnStatement);
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
			codeAssignStatement.Left = new CodeFieldReferenceExpression(this.ThisReference, fieldName);
			codeAssignStatement.Right = new CodePropertySetValueReferenceExpression();
			if (raisePropertyChanged)
			{
				CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
				CodeExpression codeExpression = new CodeFieldReferenceExpression(this.ThisReference, fieldName);
				CodeExpression codeExpression2 = new CodePropertySetValueReferenceExpression();
				if (!isValueType)
				{
					codeExpression = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(Globals.TypeOfObject), "ReferenceEquals", new CodeExpression[] { codeExpression, codeExpression2 });
				}
				else
				{
					codeExpression = new CodeMethodInvokeExpression(codeExpression, "Equals", new CodeExpression[] { codeExpression2 });
				}
				codeExpression2 = new CodePrimitiveExpression(true);
				codeConditionStatement.Condition = new CodeBinaryOperatorExpression(codeExpression, CodeBinaryOperatorType.IdentityInequality, codeExpression2);
				codeConditionStatement.TrueStatements.Add(codeAssignStatement);
				codeConditionStatement.TrueStatements.Add(new CodeMethodInvokeExpression(this.ThisReference, this.RaisePropertyChangedEventMethod.Name, new CodeExpression[]
				{
					new CodePrimitiveExpression(propertyName)
				}));
				codeMemberProperty.SetStatements.Add(codeConditionStatement);
			}
			else
			{
				codeMemberProperty.SetStatements.Add(codeAssignStatement);
			}
			return codeMemberProperty;
		}

		// Token: 0x040002E3 RID: 739
		private DataContractSet dataContractSet;

		// Token: 0x040002E4 RID: 740
		private CodeCompileUnit codeCompileUnit;

		// Token: 0x040002E5 RID: 741
		private ImportOptions options;

		// Token: 0x040002E6 RID: 742
		private Dictionary<string, string> namespaces;

		// Token: 0x040002E7 RID: 743
		private Dictionary<string, string> clrNamespaces;

		// Token: 0x040002E8 RID: 744
		private static readonly string wildcardNamespaceMapping = "*";

		// Token: 0x040002E9 RID: 745
		private static readonly string typeNameFieldName = "typeName";

		// Token: 0x040002EA RID: 746
		private static readonly object codeUserDataActualTypeKey = new object();

		// Token: 0x040002EB RID: 747
		private static readonly object surrogateDataKey = typeof(IDataContractSurrogate);

		// Token: 0x040002EC RID: 748
		private const int MaxIdentifierLength = 511;
	}
}
