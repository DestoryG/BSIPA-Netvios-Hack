using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x020000E8 RID: 232
	internal class XmlObjectSerializerReadContextComplex : XmlObjectSerializerReadContext
	{
		// Token: 0x06000D50 RID: 3408 RVA: 0x000383A6 File Offset: 0x000365A6
		internal XmlObjectSerializerReadContextComplex(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
			: base(serializer, rootTypeDataContract, dataContractResolver)
		{
			this.mode = SerializationMode.SharedContract;
			this.preserveObjectReferences = serializer.PreserveObjectReferences;
			this.dataContractSurrogate = serializer.DataContractSurrogate;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000383D0 File Offset: 0x000365D0
		internal XmlObjectSerializerReadContextComplex(NetDataContractSerializer serializer)
			: base(serializer)
		{
			this.mode = SerializationMode.SharedType;
			this.preserveObjectReferences = true;
			this.binder = serializer.Binder;
			this.surrogateSelector = serializer.SurrogateSelector;
			this.assemblyFormat = serializer.AssemblyFormat;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003840B File Offset: 0x0003660B
		internal XmlObjectSerializerReadContextComplex(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
			: base(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject)
		{
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x00038418 File Offset: 0x00036618
		internal override SerializationMode Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00038420 File Offset: 0x00036620
		internal override DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
		{
			DataContract dataContract = null;
			if (this.mode == SerializationMode.SharedType && this.surrogateSelector != null)
			{
				dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(this.surrogateSelector, base.GetStreamingContext(), typeHandle, null, ref this.surrogateDataContracts);
			}
			if (dataContract == null)
			{
				return base.GetDataContract(id, typeHandle);
			}
			if (this.IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(dataContract.UnderlyingType) })));
			}
			return dataContract;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000384A0 File Offset: 0x000366A0
		internal override DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = null;
			if (this.mode == SerializationMode.SharedType && this.surrogateSelector != null)
			{
				dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(this.surrogateSelector, base.GetStreamingContext(), typeHandle, type, ref this.surrogateDataContracts);
			}
			if (dataContract == null)
			{
				return base.GetDataContract(typeHandle, type);
			}
			if (this.IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(dataContract.UnderlyingType) })));
			}
			return dataContract;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00038520 File Offset: 0x00036720
		public override object InternalDeserialize(XmlReaderDelegator xmlReader, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle, string name, string ns)
		{
			if (this.mode != SerializationMode.SharedContract)
			{
				return this.InternalDeserializeInSharedTypeMode(xmlReader, declaredTypeID, Type.GetTypeFromHandle(declaredTypeHandle), name, ns);
			}
			if (this.dataContractSurrogate == null)
			{
				return base.InternalDeserialize(xmlReader, declaredTypeID, declaredTypeHandle, name, ns);
			}
			return this.InternalDeserializeWithSurrogate(xmlReader, Type.GetTypeFromHandle(declaredTypeHandle), null, name, ns);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00038570 File Offset: 0x00036770
		internal override object InternalDeserialize(XmlReaderDelegator xmlReader, Type declaredType, string name, string ns)
		{
			if (this.mode != SerializationMode.SharedContract)
			{
				return this.InternalDeserializeInSharedTypeMode(xmlReader, -1, declaredType, name, ns);
			}
			if (this.dataContractSurrogate == null)
			{
				return base.InternalDeserialize(xmlReader, declaredType, name, ns);
			}
			return this.InternalDeserializeWithSurrogate(xmlReader, declaredType, null, name, ns);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000385A7 File Offset: 0x000367A7
		internal override object InternalDeserialize(XmlReaderDelegator xmlReader, Type declaredType, DataContract dataContract, string name, string ns)
		{
			if (this.mode != SerializationMode.SharedContract)
			{
				return this.InternalDeserializeInSharedTypeMode(xmlReader, -1, declaredType, name, ns);
			}
			if (this.dataContractSurrogate == null)
			{
				return base.InternalDeserialize(xmlReader, declaredType, dataContract, name, ns);
			}
			return this.InternalDeserializeWithSurrogate(xmlReader, declaredType, dataContract, name, ns);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x000385E4 File Offset: 0x000367E4
		private object InternalDeserializeInSharedTypeMode(XmlReaderDelegator xmlReader, int declaredTypeID, Type declaredType, string name, string ns)
		{
			object obj = null;
			if (base.TryHandleNullOrRef(xmlReader, declaredType, name, ns, ref obj))
			{
				return obj;
			}
			string clrAssembly = this.attributes.ClrAssembly;
			string clrType = this.attributes.ClrType;
			DataContract dataContract;
			if (clrAssembly != null && clrType != null)
			{
				Assembly assembly;
				Type type;
				dataContract = this.ResolveDataContractInSharedTypeMode(clrAssembly, clrType, out assembly, out type);
				if (dataContract == null)
				{
					if (assembly == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Assembly '{0}' was not found.", new object[] { clrAssembly })));
					}
					if (type == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("CLR type '{1}' in assembly '{0}' is not found.", new object[] { assembly.FullName, clrType })));
					}
				}
				if (declaredType != null && declaredType.IsArray)
				{
					dataContract = ((declaredTypeID < 0) ? base.GetDataContract(declaredType) : this.GetDataContract(declaredTypeID, declaredType.TypeHandle));
				}
			}
			else
			{
				if (clrAssembly != null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(xmlReader, SR.GetString("Attribute was not found for CLR type '{1}' in namespace '{0}'. XML reader node is on {2}, '{4}' node in '{3}' namespace.", new object[] { "http://schemas.microsoft.com/2003/10/Serialization/", "Type", xmlReader.NodeType, xmlReader.NamespaceURI, xmlReader.LocalName }))));
				}
				if (clrType != null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(xmlReader, SR.GetString("Attribute was not found for CLR type '{1}' in namespace '{0}'. XML reader node is on {2}, '{4}' node in '{3}' namespace.", new object[] { "http://schemas.microsoft.com/2003/10/Serialization/", "Assembly", xmlReader.NodeType, xmlReader.NamespaceURI, xmlReader.LocalName }))));
				}
				if (declaredType == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(xmlReader, SR.GetString("Attribute was not found for CLR type '{1}' in namespace '{0}'. XML reader node is on {2}, '{4}' node in '{3}' namespace.", new object[] { "http://schemas.microsoft.com/2003/10/Serialization/", "Type", xmlReader.NodeType, xmlReader.NamespaceURI, xmlReader.LocalName }))));
				}
				dataContract = ((declaredTypeID < 0) ? base.GetDataContract(declaredType) : this.GetDataContract(declaredTypeID, declaredType.TypeHandle));
			}
			return this.ReadDataContractValue(dataContract, xmlReader);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000387F4 File Offset: 0x000369F4
		private object InternalDeserializeWithSurrogate(XmlReaderDelegator xmlReader, Type declaredType, DataContract surrogateDataContract, string name, string ns)
		{
			if (TD.DCDeserializeWithSurrogateStartIsEnabled())
			{
				TD.DCDeserializeWithSurrogateStart(declaredType.FullName);
			}
			DataContract dataContract = surrogateDataContract ?? base.GetDataContract(DataContractSurrogateCaller.GetDataContractType(this.dataContractSurrogate, declaredType));
			if (this.IsGetOnlyCollection && dataContract.UnderlyingType != declaredType)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(declaredType) })));
			}
			this.ReadAttributes(xmlReader);
			string objectId = base.GetObjectId();
			object obj = base.InternalDeserialize(xmlReader, name, ns, declaredType, ref dataContract);
			object deserializedObject = DataContractSurrogateCaller.GetDeserializedObject(this.dataContractSurrogate, obj, dataContract.UnderlyingType, declaredType);
			base.ReplaceDeserializedObject(objectId, obj, deserializedObject);
			if (TD.DCDeserializeWithSurrogateStopIsEnabled())
			{
				TD.DCDeserializeWithSurrogateStop();
			}
			return deserializedObject;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000388AC File Offset: 0x00036AAC
		private Type ResolveDataContractTypeInSharedTypeMode(string assemblyName, string typeName, out Assembly assembly)
		{
			assembly = null;
			Type type = null;
			if (this.binder != null)
			{
				type = this.binder.BindToType(assemblyName, typeName);
			}
			if (type == null)
			{
				XmlObjectSerializerReadContextComplex.XmlObjectDataContractTypeKey xmlObjectDataContractTypeKey = new XmlObjectSerializerReadContextComplex.XmlObjectDataContractTypeKey(assemblyName, typeName);
				XmlObjectSerializerReadContextComplex.XmlObjectDataContractTypeInfo xmlObjectDataContractTypeInfo = (XmlObjectSerializerReadContextComplex.XmlObjectDataContractTypeInfo)XmlObjectSerializerReadContextComplex.dataContractTypeCache[xmlObjectDataContractTypeKey];
				if (xmlObjectDataContractTypeInfo == null)
				{
					if (this.assemblyFormat == FormatterAssemblyStyle.Full)
					{
						if (assemblyName == "0")
						{
							assembly = Globals.TypeOfInt.Assembly;
						}
						else
						{
							assembly = Assembly.Load(assemblyName);
						}
						if (assembly != null)
						{
							type = assembly.GetType(typeName);
						}
					}
					else
					{
						assembly = XmlObjectSerializerReadContextComplex.ResolveSimpleAssemblyName(assemblyName);
						if (assembly != null)
						{
							try
							{
								type = assembly.GetType(typeName);
							}
							catch (TypeLoadException)
							{
							}
							catch (FileNotFoundException)
							{
							}
							catch (FileLoadException)
							{
							}
							catch (BadImageFormatException)
							{
							}
							if (type == null)
							{
								type = Type.GetType(typeName, new Func<AssemblyName, Assembly>(XmlObjectSerializerReadContextComplex.ResolveSimpleAssemblyName), new Func<Assembly, string, bool, Type>(new XmlObjectSerializerReadContextComplex.TopLevelAssemblyTypeResolver(assembly).ResolveType), false);
							}
						}
					}
					if (!(type != null))
					{
						return type;
					}
					XmlObjectSerializerReadContextComplex.CheckTypeForwardedTo(assembly, type.Assembly, type);
					xmlObjectDataContractTypeInfo = new XmlObjectSerializerReadContextComplex.XmlObjectDataContractTypeInfo(assembly, type);
					Hashtable hashtable = XmlObjectSerializerReadContextComplex.dataContractTypeCache;
					lock (hashtable)
					{
						if (!XmlObjectSerializerReadContextComplex.dataContractTypeCache.ContainsKey(xmlObjectDataContractTypeKey))
						{
							XmlObjectSerializerReadContextComplex.dataContractTypeCache[xmlObjectDataContractTypeKey] = xmlObjectDataContractTypeInfo;
						}
						return type;
					}
				}
				assembly = xmlObjectDataContractTypeInfo.Assembly;
				type = xmlObjectDataContractTypeInfo.Type;
			}
			return type;
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00038A40 File Offset: 0x00036C40
		private DataContract ResolveDataContractInSharedTypeMode(string assemblyName, string typeName, out Assembly assembly, out Type type)
		{
			type = this.ResolveDataContractTypeInSharedTypeMode(assemblyName, typeName, out assembly);
			if (type != null)
			{
				return base.GetDataContract(type);
			}
			return null;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00038A64 File Offset: 0x00036C64
		protected override DataContract ResolveDataContractFromTypeName()
		{
			if (this.mode == SerializationMode.SharedContract)
			{
				return base.ResolveDataContractFromTypeName();
			}
			if (this.attributes.ClrAssembly != null && this.attributes.ClrType != null)
			{
				Assembly assembly;
				Type type;
				return this.ResolveDataContractInSharedTypeMode(this.attributes.ClrAssembly, this.attributes.ClrType, out assembly, out type);
			}
			return null;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00038ABC File Offset: 0x00036CBC
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool CheckIfTypeSerializableForSharedTypeMode(Type memberType)
		{
			ISurrogateSelector surrogateSelector;
			return this.surrogateSelector.GetSurrogate(memberType, base.GetStreamingContext(), out surrogateSelector) != null;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00038AE0 File Offset: 0x00036CE0
		internal override void CheckIfTypeSerializable(Type memberType, bool isMemberTypeSerializable)
		{
			if (this.mode == SerializationMode.SharedType && this.surrogateSelector != null && this.CheckIfTypeSerializableForSharedTypeMode(memberType))
			{
				return;
			}
			if (this.dataContractSurrogate == null)
			{
				base.CheckIfTypeSerializable(memberType, isMemberTypeSerializable);
				return;
			}
			while (memberType.IsArray)
			{
				memberType = memberType.GetElementType();
			}
			memberType = DataContractSurrogateCaller.GetDataContractType(this.dataContractSurrogate, memberType);
			if (!DataContract.IsTypeSerializable(memberType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", new object[] { memberType })));
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00038B60 File Offset: 0x00036D60
		internal override Type GetSurrogatedType(Type type)
		{
			if (this.dataContractSurrogate == null)
			{
				return base.GetSurrogatedType(type);
			}
			type = DataContract.UnwrapNullableType(type);
			Type surrogatedType = DataContractSerializer.GetSurrogatedType(this.dataContractSurrogate, type);
			if (this.IsGetOnlyCollection && surrogatedType != type)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", new object[] { DataContract.GetClrTypeFullName(type) })));
			}
			return surrogatedType;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00038BC8 File Offset: 0x00036DC8
		internal override int GetArraySize()
		{
			if (!this.preserveObjectReferences)
			{
				return -1;
			}
			return this.attributes.ArraySZSize;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00038BDF File Offset: 0x00036DDF
		private static Assembly ResolveSimpleAssemblyName(AssemblyName assemblyName)
		{
			return XmlObjectSerializerReadContextComplex.ResolveSimpleAssemblyName(assemblyName.FullName);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00038BEC File Offset: 0x00036DEC
		private static Assembly ResolveSimpleAssemblyName(string assemblyName)
		{
			Assembly assembly;
			if (assemblyName == "0")
			{
				assembly = Globals.TypeOfInt.Assembly;
			}
			else
			{
				assembly = Assembly.LoadWithPartialName(assemblyName);
				if (assembly == null)
				{
					assembly = Assembly.LoadWithPartialName(new AssemblyName(assemblyName)
					{
						Version = null
					}.FullName);
				}
			}
			return assembly;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00038C3C File Offset: 0x00036E3C
		[SecuritySafeCritical]
		private static void CheckTypeForwardedTo(Assembly sourceAssembly, Assembly destinationAssembly, Type resolvedType)
		{
			if (sourceAssembly != destinationAssembly && !NetDataContractSerializer.UnsafeTypeForwardingEnabled && !sourceAssembly.IsFullyTrusted && !destinationAssembly.PermissionSet.IsSubsetOf(sourceAssembly.PermissionSet))
			{
				TypeInformation typeInformation = NetDataContractSerializer.GetTypeInformation(resolvedType);
				if (typeInformation.HasTypeForwardedFrom)
				{
					Assembly assembly = null;
					try
					{
						assembly = Assembly.Load(typeInformation.AssemblyString);
					}
					catch
					{
					}
					if (assembly == sourceAssembly)
					{
						return;
					}
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Cannot deserialize forwarded type '{0}'.", new object[] { DataContract.GetClrTypeFullName(resolvedType) })));
			}
		}

		// Token: 0x04000566 RID: 1382
		private static Hashtable dataContractTypeCache = new Hashtable();

		// Token: 0x04000567 RID: 1383
		private bool preserveObjectReferences;

		// Token: 0x04000568 RID: 1384
		protected IDataContractSurrogate dataContractSurrogate;

		// Token: 0x04000569 RID: 1385
		private SerializationMode mode;

		// Token: 0x0400056A RID: 1386
		private SerializationBinder binder;

		// Token: 0x0400056B RID: 1387
		private ISurrogateSelector surrogateSelector;

		// Token: 0x0400056C RID: 1388
		private FormatterAssemblyStyle assemblyFormat;

		// Token: 0x0400056D RID: 1389
		private Hashtable surrogateDataContracts;

		// Token: 0x0200017E RID: 382
		private sealed class TopLevelAssemblyTypeResolver
		{
			// Token: 0x060014ED RID: 5357 RVA: 0x0005497C File Offset: 0x00052B7C
			public TopLevelAssemblyTypeResolver(Assembly topLevelAssembly)
			{
				this.topLevelAssembly = topLevelAssembly;
			}

			// Token: 0x060014EE RID: 5358 RVA: 0x0005498B File Offset: 0x00052B8B
			public Type ResolveType(Assembly assembly, string simpleTypeName, bool ignoreCase)
			{
				if (assembly == null)
				{
					assembly = this.topLevelAssembly;
				}
				return assembly.GetType(simpleTypeName, false, ignoreCase);
			}

			// Token: 0x04000A38 RID: 2616
			private Assembly topLevelAssembly;
		}

		// Token: 0x0200017F RID: 383
		private class XmlObjectDataContractTypeInfo
		{
			// Token: 0x060014EF RID: 5359 RVA: 0x000549A7 File Offset: 0x00052BA7
			public XmlObjectDataContractTypeInfo(Assembly assembly, Type type)
			{
				this.assembly = assembly;
				this.type = type;
			}

			// Token: 0x1700045F RID: 1119
			// (get) Token: 0x060014F0 RID: 5360 RVA: 0x000549BD File Offset: 0x00052BBD
			public Assembly Assembly
			{
				get
				{
					return this.assembly;
				}
			}

			// Token: 0x17000460 RID: 1120
			// (get) Token: 0x060014F1 RID: 5361 RVA: 0x000549C5 File Offset: 0x00052BC5
			public Type Type
			{
				get
				{
					return this.type;
				}
			}

			// Token: 0x04000A39 RID: 2617
			private Assembly assembly;

			// Token: 0x04000A3A RID: 2618
			private Type type;
		}

		// Token: 0x02000180 RID: 384
		private class XmlObjectDataContractTypeKey
		{
			// Token: 0x060014F2 RID: 5362 RVA: 0x000549CD File Offset: 0x00052BCD
			public XmlObjectDataContractTypeKey(string assemblyName, string typeName)
			{
				this.assemblyName = assemblyName;
				this.typeName = typeName;
			}

			// Token: 0x060014F3 RID: 5363 RVA: 0x000549E4 File Offset: 0x00052BE4
			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				XmlObjectSerializerReadContextComplex.XmlObjectDataContractTypeKey xmlObjectDataContractTypeKey = obj as XmlObjectSerializerReadContextComplex.XmlObjectDataContractTypeKey;
				return xmlObjectDataContractTypeKey != null && !(this.assemblyName != xmlObjectDataContractTypeKey.assemblyName) && !(this.typeName != xmlObjectDataContractTypeKey.typeName);
			}

			// Token: 0x060014F4 RID: 5364 RVA: 0x00054A30 File Offset: 0x00052C30
			public override int GetHashCode()
			{
				int num = 0;
				if (this.assemblyName != null)
				{
					num = this.assemblyName.GetHashCode();
				}
				if (this.typeName != null)
				{
					num ^= this.typeName.GetHashCode();
				}
				return num;
			}

			// Token: 0x04000A3B RID: 2619
			private string assemblyName;

			// Token: 0x04000A3C RID: 2620
			private string typeName;
		}
	}
}
