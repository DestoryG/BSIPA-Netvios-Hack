using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;
using Mono.Security.Cryptography;

namespace Mono.Cecil
{
	// Token: 0x020000B6 RID: 182
	internal static class Mixin
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x00010FAD File Offset: 0x0000F1AD
		public static bool IsNullOrEmpty<T>(this T[] self)
		{
			return self == null || self.Length == 0;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00010FB9 File Offset: 0x0000F1B9
		public static bool IsNullOrEmpty<T>(this Collection<T> self)
		{
			return self == null || self.size == 0;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00010FC9 File Offset: 0x0000F1C9
		public static T[] Resize<T>(this T[] self, int length)
		{
			Array.Resize<T>(ref self, length);
			return self;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00010FD4 File Offset: 0x0000F1D4
		public static T[] Add<T>(this T[] self, T item)
		{
			if (self == null)
			{
				self = new T[] { item };
				return self;
			}
			self = self.Resize(self.Length + 1);
			self[self.Length - 1] = item;
			return self;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00011008 File Offset: 0x0000F208
		public static Version CheckVersion(Version version)
		{
			if (version == null)
			{
				return Mixin.ZeroVersion;
			}
			if (version.Build == -1)
			{
				return new Version(version.Major, version.Minor, 0, 0);
			}
			if (version.Revision == -1)
			{
				return new Version(version.Major, version.Minor, version.Build, 0);
			}
			return version;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00011064 File Offset: 0x0000F264
		public static bool TryGetUniqueDocument(this MethodDebugInformation info, out Document document)
		{
			document = info.SequencePoints[0].Document;
			for (int i = 1; i < info.SequencePoints.Count; i++)
			{
				if (info.SequencePoints[i].Document != document)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000110B4 File Offset: 0x0000F2B4
		public static void ResolveConstant(this IConstantProvider self, ref object constant, ModuleDefinition module)
		{
			if (module == null)
			{
				constant = Mixin.NoValue;
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (constant == Mixin.NotResolved)
				{
					if (module.HasImage())
					{
						constant = module.Read<IConstantProvider, object>(self, (IConstantProvider provider, MetadataReader reader) => reader.ReadConstant(provider));
					}
					else
					{
						constant = Mixin.NoValue;
					}
				}
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00011140 File Offset: 0x0000F340
		public static bool GetHasCustomAttributes(this ICustomAttributeProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ICustomAttributeProvider, bool>(self, (ICustomAttributeProvider provider, MetadataReader reader) => reader.HasCustomAttributes(provider));
			}
			return false;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00011174 File Offset: 0x0000F374
		public static Collection<CustomAttribute> GetCustomAttributes(this ICustomAttributeProvider self, ref Collection<CustomAttribute> variable, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ICustomAttributeProvider, Collection<CustomAttribute>>(ref variable, self, (ICustomAttributeProvider provider, MetadataReader reader) => reader.ReadCustomAttributes(provider));
			}
			Interlocked.CompareExchange<Collection<CustomAttribute>>(ref variable, new Collection<CustomAttribute>(), null);
			return variable;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000111C0 File Offset: 0x0000F3C0
		public static bool ContainsGenericParameter(this IGenericInstance self)
		{
			Collection<TypeReference> genericArguments = self.GenericArguments;
			for (int i = 0; i < genericArguments.Count; i++)
			{
				if (genericArguments[i].ContainsGenericParameter)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000111F8 File Offset: 0x0000F3F8
		public static void GenericInstanceFullName(this IGenericInstance self, StringBuilder builder)
		{
			builder.Append("<");
			Collection<TypeReference> genericArguments = self.GenericArguments;
			for (int i = 0; i < genericArguments.Count; i++)
			{
				if (i > 0)
				{
					builder.Append(",");
				}
				builder.Append(genericArguments[i].FullName);
			}
			builder.Append(">");
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00011258 File Offset: 0x0000F458
		public static bool GetHasGenericParameters(this IGenericParameterProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IGenericParameterProvider, bool>(self, (IGenericParameterProvider provider, MetadataReader reader) => reader.HasGenericParameters(provider));
			}
			return false;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001128C File Offset: 0x0000F48C
		public static Collection<GenericParameter> GetGenericParameters(this IGenericParameterProvider self, ref Collection<GenericParameter> collection, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IGenericParameterProvider, Collection<GenericParameter>>(ref collection, self, (IGenericParameterProvider provider, MetadataReader reader) => reader.ReadGenericParameters(provider));
			}
			Interlocked.CompareExchange<Collection<GenericParameter>>(ref collection, new GenericParameterCollection(self), null);
			return collection;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000112D9 File Offset: 0x0000F4D9
		public static bool GetHasMarshalInfo(this IMarshalInfoProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IMarshalInfoProvider, bool>(self, (IMarshalInfoProvider provider, MetadataReader reader) => reader.HasMarshalInfo(provider));
			}
			return false;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001130B File Offset: 0x0000F50B
		public static MarshalInfo GetMarshalInfo(this IMarshalInfoProvider self, ref MarshalInfo variable, ModuleDefinition module)
		{
			if (!module.HasImage())
			{
				return null;
			}
			return module.Read<IMarshalInfoProvider, MarshalInfo>(ref variable, self, (IMarshalInfoProvider provider, MetadataReader reader) => reader.ReadMarshalInfo(provider));
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001133E File Offset: 0x0000F53E
		public static bool GetAttributes(this uint self, uint attributes)
		{
			return (self & attributes) > 0U;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00011346 File Offset: 0x0000F546
		public static uint SetAttributes(this uint self, uint attributes, bool value)
		{
			if (value)
			{
				return self | attributes;
			}
			return self & ~attributes;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00011353 File Offset: 0x0000F553
		public static bool GetMaskedAttributes(this uint self, uint mask, uint attributes)
		{
			return (self & mask) == attributes;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001135B File Offset: 0x0000F55B
		public static uint SetMaskedAttributes(this uint self, uint mask, uint attributes, bool value)
		{
			if (value)
			{
				self &= ~mask;
				return self | attributes;
			}
			return self & ~(mask & attributes);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001133E File Offset: 0x0000F53E
		public static bool GetAttributes(this ushort self, ushort attributes)
		{
			return (self & attributes) > 0;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00011370 File Offset: 0x0000F570
		public static ushort SetAttributes(this ushort self, ushort attributes, bool value)
		{
			if (value)
			{
				return self | attributes;
			}
			return self & ~attributes;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001137F File Offset: 0x0000F57F
		public static bool GetMaskedAttributes(this ushort self, ushort mask, uint attributes)
		{
			return (long)(self & mask) == (long)((ulong)attributes);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00011389 File Offset: 0x0000F589
		public static ushort SetMaskedAttributes(this ushort self, ushort mask, uint attributes, bool value)
		{
			if (value)
			{
				self &= ~mask;
				return (ushort)((uint)self | attributes);
			}
			return (ushort)((uint)self & ~((uint)mask & attributes));
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000113A1 File Offset: 0x0000F5A1
		public static bool HasImplicitThis(this IMethodSignature self)
		{
			return self.HasThis && !self.ExplicitThis;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000113B8 File Offset: 0x0000F5B8
		public static void MethodSignatureFullName(this IMethodSignature self, StringBuilder builder)
		{
			builder.Append("(");
			if (self.HasParameters)
			{
				Collection<ParameterDefinition> parameters = self.Parameters;
				for (int i = 0; i < parameters.Count; i++)
				{
					ParameterDefinition parameterDefinition = parameters[i];
					if (i > 0)
					{
						builder.Append(",");
					}
					if (parameterDefinition.ParameterType.IsSentinel)
					{
						builder.Append("...,");
					}
					builder.Append(parameterDefinition.ParameterType.FullName);
				}
			}
			builder.Append(")");
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00011440 File Offset: 0x0000F640
		public static void CheckModule(ModuleDefinition module)
		{
			if (module == null)
			{
				throw new ArgumentNullException(Mixin.Argument.module.ToString());
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00011468 File Offset: 0x0000F668
		public static bool TryGetAssemblyNameReference(this ModuleDefinition module, AssemblyNameReference name_reference, out AssemblyNameReference assembly_reference)
		{
			Collection<AssemblyNameReference> assemblyReferences = module.AssemblyReferences;
			for (int i = 0; i < assemblyReferences.Count; i++)
			{
				AssemblyNameReference assemblyNameReference = assemblyReferences[i];
				if (Mixin.Equals(name_reference, assemblyNameReference))
				{
					assembly_reference = assemblyNameReference;
					return true;
				}
			}
			assembly_reference = null;
			return false;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000114A8 File Offset: 0x0000F6A8
		private static bool Equals(byte[] a, byte[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null)
			{
				return false;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000114E3 File Offset: 0x0000F6E3
		private static bool Equals<T>(T a, T b) where T : class, IEquatable<T>
		{
			return a == b || (a != null && a.Equals(b));
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001150C File Offset: 0x0000F70C
		private static bool Equals(AssemblyNameReference a, AssemblyNameReference b)
		{
			return a == b || (!(a.Name != b.Name) && Mixin.Equals<Version>(a.Version, b.Version) && !(a.Culture != b.Culture) && Mixin.Equals(a.PublicKeyToken, b.PublicKeyToken));
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00011574 File Offset: 0x0000F774
		public static ParameterDefinition GetParameter(this Mono.Cecil.Cil.MethodBody self, int index)
		{
			MethodDefinition method = self.method;
			if (method.HasThis)
			{
				if (index == 0)
				{
					return self.ThisParameter;
				}
				index--;
			}
			Collection<ParameterDefinition> parameters = method.Parameters;
			if (index < 0 || index >= parameters.size)
			{
				return null;
			}
			return parameters[index];
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000115BC File Offset: 0x0000F7BC
		public static VariableDefinition GetVariable(this Mono.Cecil.Cil.MethodBody self, int index)
		{
			Collection<VariableDefinition> variables = self.Variables;
			if (index < 0 || index >= variables.size)
			{
				return null;
			}
			return variables[index];
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000115E6 File Offset: 0x0000F7E6
		public static bool GetSemantics(this MethodDefinition self, MethodSemanticsAttributes semantics)
		{
			return (self.SemanticsAttributes & semantics) > MethodSemanticsAttributes.None;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000115F3 File Offset: 0x0000F7F3
		public static void SetSemantics(this MethodDefinition self, MethodSemanticsAttributes semantics, bool value)
		{
			if (value)
			{
				self.SemanticsAttributes |= semantics;
				return;
			}
			self.SemanticsAttributes &= ~semantics;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00011617 File Offset: 0x0000F817
		public static bool IsVarArg(this IMethodSignature self)
		{
			return self.CallingConvention == MethodCallingConvention.VarArg;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00011624 File Offset: 0x0000F824
		public static int GetSentinelPosition(this IMethodSignature self)
		{
			if (!self.HasParameters)
			{
				return -1;
			}
			Collection<ParameterDefinition> parameters = self.Parameters;
			for (int i = 0; i < parameters.Count; i++)
			{
				if (parameters[i].ParameterType.IsSentinel)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001166C File Offset: 0x0000F86C
		public static void CheckName(object name)
		{
			if (name == null)
			{
				throw new ArgumentNullException(Mixin.Argument.name.ToString());
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00011694 File Offset: 0x0000F894
		public static void CheckName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.name.ToString());
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000116C0 File Offset: 0x0000F8C0
		public static void CheckFileName(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.fileName.ToString());
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000116EC File Offset: 0x0000F8EC
		public static void CheckFullName(string fullName)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.fullName.ToString());
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00011718 File Offset: 0x0000F918
		public static void CheckStream(object stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(Mixin.Argument.stream.ToString());
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001173D File Offset: 0x0000F93D
		public static void CheckWriteSeek(Stream stream)
		{
			if (!stream.CanWrite || !stream.CanSeek)
			{
				throw new ArgumentException("Stream must be writable and seekable.");
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001175A File Offset: 0x0000F95A
		public static void CheckReadSeek(Stream stream)
		{
			if (!stream.CanRead || !stream.CanSeek)
			{
				throw new ArgumentException("Stream must be readable and seekable.");
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00011778 File Offset: 0x0000F978
		public static void CheckType(object type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(Mixin.Argument.type.ToString());
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001179D File Offset: 0x0000F99D
		public static void CheckType(object type, Mixin.Argument argument)
		{
			if (type == null)
			{
				throw new ArgumentNullException(argument.ToString());
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000117B8 File Offset: 0x0000F9B8
		public static void CheckField(object field)
		{
			if (field == null)
			{
				throw new ArgumentNullException(Mixin.Argument.field.ToString());
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000117E0 File Offset: 0x0000F9E0
		public static void CheckMethod(object method)
		{
			if (method == null)
			{
				throw new ArgumentNullException(Mixin.Argument.method.ToString());
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00011808 File Offset: 0x0000FA08
		public static void CheckParameters(object parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(Mixin.Argument.parameters.ToString());
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00011830 File Offset: 0x0000FA30
		public static uint GetTimestamp()
		{
			return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001185F File Offset: 0x0000FA5F
		public static bool HasImage(this ModuleDefinition self)
		{
			return self != null && self.HasImage;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001186C File Offset: 0x0000FA6C
		public static string GetFileName(this Stream self)
		{
			FileStream fileStream = self as FileStream;
			if (fileStream == null)
			{
				return string.Empty;
			}
			return Path.GetFullPath(fileStream.Name);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00011894 File Offset: 0x0000FA94
		public static TargetRuntime ParseRuntime(this string self)
		{
			if (string.IsNullOrEmpty(self))
			{
				return TargetRuntime.Net_4_0;
			}
			switch (self[1])
			{
			case '1':
				if (self[3] != '0')
				{
					return TargetRuntime.Net_1_1;
				}
				return TargetRuntime.Net_1_0;
			case '2':
				return TargetRuntime.Net_2_0;
			}
			return TargetRuntime.Net_4_0;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000118E0 File Offset: 0x0000FAE0
		public static string RuntimeVersionString(this TargetRuntime runtime)
		{
			switch (runtime)
			{
			case TargetRuntime.Net_1_0:
				return "v1.0.3705";
			case TargetRuntime.Net_1_1:
				return "v1.1.4322";
			case TargetRuntime.Net_2_0:
				return "v2.0.50727";
			}
			return "v4.0.30319";
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00011911 File Offset: 0x0000FB11
		public static bool IsWindowsMetadata(this ModuleDefinition module)
		{
			return module.MetadataKind > MetadataKind.Ecma335;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001191C File Offset: 0x0000FB1C
		public static byte[] ReadAll(this Stream self)
		{
			MemoryStream memoryStream = new MemoryStream((int)self.Length);
			byte[] array = new byte[1024];
			int num;
			while ((num = self.Read(array, 0, array.Length)) != 0)
			{
				memoryStream.Write(array, 0, num);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00010C51 File Offset: 0x0000EE51
		public static void Read(object o)
		{
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00011961 File Offset: 0x0000FB61
		public static bool GetHasSecurityDeclarations(this ISecurityDeclarationProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ISecurityDeclarationProvider, bool>(self, (ISecurityDeclarationProvider provider, MetadataReader reader) => reader.HasSecurityDeclarations(provider));
			}
			return false;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00011994 File Offset: 0x0000FB94
		public static Collection<SecurityDeclaration> GetSecurityDeclarations(this ISecurityDeclarationProvider self, ref Collection<SecurityDeclaration> variable, ModuleDefinition module)
		{
			if (module.HasImage)
			{
				return module.Read<ISecurityDeclarationProvider, Collection<SecurityDeclaration>>(ref variable, self, (ISecurityDeclarationProvider provider, MetadataReader reader) => reader.ReadSecurityDeclarations(provider));
			}
			Interlocked.CompareExchange<Collection<SecurityDeclaration>>(ref variable, new Collection<SecurityDeclaration>(), null);
			return variable;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000119E0 File Offset: 0x0000FBE0
		public static TypeReference GetEnumUnderlyingType(this TypeDefinition self)
		{
			Collection<FieldDefinition> fields = self.Fields;
			for (int i = 0; i < fields.Count; i++)
			{
				FieldDefinition fieldDefinition = fields[i];
				if (!fieldDefinition.IsStatic)
				{
					return fieldDefinition.FieldType;
				}
			}
			throw new ArgumentException();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00011A24 File Offset: 0x0000FC24
		public static TypeDefinition GetNestedType(this TypeDefinition self, string fullname)
		{
			if (!self.HasNestedTypes)
			{
				return null;
			}
			Collection<TypeDefinition> nestedTypes = self.NestedTypes;
			for (int i = 0; i < nestedTypes.Count; i++)
			{
				TypeDefinition typeDefinition = nestedTypes[i];
				if (typeDefinition.TypeFullName() == fullname)
				{
					return typeDefinition;
				}
			}
			return null;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00011A6C File Offset: 0x0000FC6C
		public static bool IsPrimitive(this ElementType self)
		{
			return self - ElementType.Boolean <= 11 || self - ElementType.I <= 1;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00011A7F File Offset: 0x0000FC7F
		public static string TypeFullName(this TypeReference self)
		{
			if (!string.IsNullOrEmpty(self.Namespace))
			{
				return self.Namespace + "." + self.Name;
			}
			return self.Name;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00011AAB File Offset: 0x0000FCAB
		public static bool IsTypeOf(this TypeReference self, string @namespace, string name)
		{
			return self.Name == name && self.Namespace == @namespace;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00011ACC File Offset: 0x0000FCCC
		public static bool IsTypeSpecification(this TypeReference type)
		{
			ElementType etype = type.etype;
			switch (etype)
			{
			case ElementType.Ptr:
			case ElementType.ByRef:
			case ElementType.Var:
			case ElementType.Array:
			case ElementType.GenericInst:
			case ElementType.FnPtr:
			case ElementType.SzArray:
			case ElementType.MVar:
			case ElementType.CModReqD:
			case ElementType.CModOpt:
				break;
			case ElementType.ValueType:
			case ElementType.Class:
			case ElementType.TypedByRef:
			case (ElementType)23:
			case ElementType.I:
			case ElementType.U:
			case (ElementType)26:
			case ElementType.Object:
				return false;
			default:
				if (etype != ElementType.Sentinel && etype != ElementType.Pinned)
				{
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00011B3E File Offset: 0x0000FD3E
		public static TypeDefinition CheckedResolve(this TypeReference self)
		{
			TypeDefinition typeDefinition = self.Resolve();
			if (typeDefinition == null)
			{
				throw new ResolutionException(self);
			}
			return typeDefinition;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00011B50 File Offset: 0x0000FD50
		public static bool TryGetCoreLibraryReference(this ModuleDefinition module, out AssemblyNameReference reference)
		{
			Collection<AssemblyNameReference> assemblyReferences = module.AssemblyReferences;
			for (int i = 0; i < assemblyReferences.Count; i++)
			{
				reference = assemblyReferences[i];
				if (Mixin.IsCoreLibrary(reference))
				{
					return true;
				}
			}
			reference = null;
			return false;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00011B90 File Offset: 0x0000FD90
		public static bool IsCoreLibrary(this ModuleDefinition module)
		{
			if (module.Assembly == null)
			{
				return false;
			}
			if (!Mixin.IsCoreLibrary(module.Assembly.Name))
			{
				return false;
			}
			if (module.HasImage)
			{
				if (module.Read<ModuleDefinition, bool>(module, (ModuleDefinition m, MetadataReader reader) => reader.image.GetTableLength(Table.AssemblyRef) > 0))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00011BEE File Offset: 0x0000FDEE
		public static void KnownValueType(this TypeReference type)
		{
			if (!type.IsDefinition)
			{
				type.IsValueType = true;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00011C00 File Offset: 0x0000FE00
		private static bool IsCoreLibrary(AssemblyNameReference reference)
		{
			string name = reference.Name;
			return name == "mscorlib" || name == "System.Runtime" || name == "System.Private.CoreLib" || name == "netstandard";
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00011C48 File Offset: 0x0000FE48
		public static ImageDebugHeaderEntry GetCodeViewEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.CodeView);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00011C51 File Offset: 0x0000FE51
		public static ImageDebugHeaderEntry GetDeterministicEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.Deterministic);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00011C5C File Offset: 0x0000FE5C
		public static ImageDebugHeader AddDeterministicEntry(this ImageDebugHeader header)
		{
			ImageDebugHeaderEntry imageDebugHeaderEntry = new ImageDebugHeaderEntry(new ImageDebugDirectory
			{
				Type = ImageDebugType.Deterministic
			}, Empty<byte>.Array);
			if (header == null)
			{
				return new ImageDebugHeader(imageDebugHeaderEntry);
			}
			ImageDebugHeaderEntry[] array = new ImageDebugHeaderEntry[header.Entries.Length + 1];
			Array.Copy(header.Entries, array, header.Entries.Length);
			array[array.Length - 1] = imageDebugHeaderEntry;
			return new ImageDebugHeader(array);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00011CC2 File Offset: 0x0000FEC2
		public static ImageDebugHeaderEntry GetEmbeddedPortablePdbEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.EmbeddedPortablePdb);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00011CCC File Offset: 0x0000FECC
		private static ImageDebugHeaderEntry GetEntry(this ImageDebugHeader header, ImageDebugType type)
		{
			if (!header.HasEntries)
			{
				return null;
			}
			for (int i = 0; i < header.Entries.Length; i++)
			{
				ImageDebugHeaderEntry imageDebugHeaderEntry = header.Entries[i];
				if (imageDebugHeaderEntry.Directory.Type == type)
				{
					return imageDebugHeaderEntry;
				}
			}
			return null;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00011D10 File Offset: 0x0000FF10
		public static string GetPdbFileName(string assemblyFileName)
		{
			return Path.ChangeExtension(assemblyFileName, ".pdb");
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00011D1D File Offset: 0x0000FF1D
		public static string GetMdbFileName(string assemblyFileName)
		{
			return assemblyFileName + ".mdb";
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00011D2C File Offset: 0x0000FF2C
		public static bool IsPortablePdb(string fileName)
		{
			bool flag;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				flag = Mixin.IsPortablePdb(fileStream);
			}
			return flag;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00011D68 File Offset: 0x0000FF68
		public static bool IsPortablePdb(Stream stream)
		{
			if (stream.Length < 4L)
			{
				return false;
			}
			long position = stream.Position;
			bool flag;
			try
			{
				flag = new BinaryReader(stream).ReadUInt32() == 1112167234U;
			}
			finally
			{
				stream.Position = position;
			}
			return flag;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00011DB8 File Offset: 0x0000FFB8
		public static uint ReadCompressedUInt32(this byte[] data, ref int position)
		{
			uint num;
			if ((data[position] & 128) == 0)
			{
				num = (uint)data[position];
				position++;
			}
			else if ((data[position] & 64) == 0)
			{
				num = ((uint)data[position] & 4294967167U) << 8;
				num |= (uint)data[position + 1];
				position += 2;
			}
			else
			{
				num = ((uint)data[position] & 4294967103U) << 24;
				num |= (uint)((uint)data[position + 1] << 16);
				num |= (uint)((uint)data[position + 2] << 8);
				num |= (uint)data[position + 3];
				position += 4;
			}
			return num;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00011E3C File Offset: 0x0001003C
		public static MetadataToken GetMetadataToken(this CodedIndex self, uint data)
		{
			uint num;
			TokenType tokenType;
			switch (self)
			{
			case CodedIndex.TypeDefOrRef:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.TypeDef;
					break;
				case 1U:
					tokenType = TokenType.TypeRef;
					break;
				case 2U:
					tokenType = TokenType.TypeSpec;
					break;
				default:
					goto IL_05BB;
				}
				break;
			case CodedIndex.HasConstant:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.Field;
					break;
				case 1U:
					tokenType = TokenType.Param;
					break;
				case 2U:
					tokenType = TokenType.Property;
					break;
				default:
					goto IL_05BB;
				}
				break;
			case CodedIndex.HasCustomAttribute:
				num = data >> 5;
				switch (data & 31U)
				{
				case 0U:
					tokenType = TokenType.Method;
					break;
				case 1U:
					tokenType = TokenType.Field;
					break;
				case 2U:
					tokenType = TokenType.TypeRef;
					break;
				case 3U:
					tokenType = TokenType.TypeDef;
					break;
				case 4U:
					tokenType = TokenType.Param;
					break;
				case 5U:
					tokenType = TokenType.InterfaceImpl;
					break;
				case 6U:
					tokenType = TokenType.MemberRef;
					break;
				case 7U:
					tokenType = TokenType.Module;
					break;
				case 8U:
					tokenType = TokenType.Permission;
					break;
				case 9U:
					tokenType = TokenType.Property;
					break;
				case 10U:
					tokenType = TokenType.Event;
					break;
				case 11U:
					tokenType = TokenType.Signature;
					break;
				case 12U:
					tokenType = TokenType.ModuleRef;
					break;
				case 13U:
					tokenType = TokenType.TypeSpec;
					break;
				case 14U:
					tokenType = TokenType.Assembly;
					break;
				case 15U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 16U:
					tokenType = TokenType.File;
					break;
				case 17U:
					tokenType = TokenType.ExportedType;
					break;
				case 18U:
					tokenType = TokenType.ManifestResource;
					break;
				case 19U:
					tokenType = TokenType.GenericParam;
					break;
				case 20U:
					tokenType = TokenType.GenericParamConstraint;
					break;
				case 21U:
					tokenType = TokenType.MethodSpec;
					break;
				default:
					goto IL_05BB;
				}
				break;
			case CodedIndex.HasFieldMarshal:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_05BB;
					}
					tokenType = TokenType.Param;
				}
				else
				{
					tokenType = TokenType.Field;
				}
				break;
			}
			case CodedIndex.HasDeclSecurity:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.TypeDef;
					break;
				case 1U:
					tokenType = TokenType.Method;
					break;
				case 2U:
					tokenType = TokenType.Assembly;
					break;
				default:
					goto IL_05BB;
				}
				break;
			case CodedIndex.MemberRefParent:
				num = data >> 3;
				switch (data & 7U)
				{
				case 0U:
					tokenType = TokenType.TypeDef;
					break;
				case 1U:
					tokenType = TokenType.TypeRef;
					break;
				case 2U:
					tokenType = TokenType.ModuleRef;
					break;
				case 3U:
					tokenType = TokenType.Method;
					break;
				case 4U:
					tokenType = TokenType.TypeSpec;
					break;
				default:
					goto IL_05BB;
				}
				break;
			case CodedIndex.HasSemantics:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_05BB;
					}
					tokenType = TokenType.Property;
				}
				else
				{
					tokenType = TokenType.Event;
				}
				break;
			}
			case CodedIndex.MethodDefOrRef:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_05BB;
					}
					tokenType = TokenType.MemberRef;
				}
				else
				{
					tokenType = TokenType.Method;
				}
				break;
			}
			case CodedIndex.MemberForwarded:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_05BB;
					}
					tokenType = TokenType.Method;
				}
				else
				{
					tokenType = TokenType.Field;
				}
				break;
			}
			case CodedIndex.Implementation:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.File;
					break;
				case 1U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 2U:
					tokenType = TokenType.ExportedType;
					break;
				default:
					goto IL_05BB;
				}
				break;
			case CodedIndex.CustomAttributeType:
			{
				num = data >> 3;
				uint num2 = data & 7U;
				if (num2 != 2U)
				{
					if (num2 != 3U)
					{
						goto IL_05BB;
					}
					tokenType = TokenType.MemberRef;
				}
				else
				{
					tokenType = TokenType.Method;
				}
				break;
			}
			case CodedIndex.ResolutionScope:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.Module;
					break;
				case 1U:
					tokenType = TokenType.ModuleRef;
					break;
				case 2U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 3U:
					tokenType = TokenType.TypeRef;
					break;
				default:
					goto IL_05BB;
				}
				break;
			case CodedIndex.TypeOrMethodDef:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_05BB;
					}
					tokenType = TokenType.Method;
				}
				else
				{
					tokenType = TokenType.TypeDef;
				}
				break;
			}
			case CodedIndex.HasCustomDebugInformation:
				num = data >> 5;
				switch (data & 31U)
				{
				case 0U:
					tokenType = TokenType.Method;
					break;
				case 1U:
					tokenType = TokenType.Field;
					break;
				case 2U:
					tokenType = TokenType.TypeRef;
					break;
				case 3U:
					tokenType = TokenType.TypeDef;
					break;
				case 4U:
					tokenType = TokenType.Param;
					break;
				case 5U:
					tokenType = TokenType.InterfaceImpl;
					break;
				case 6U:
					tokenType = TokenType.MemberRef;
					break;
				case 7U:
					tokenType = TokenType.Module;
					break;
				case 8U:
					tokenType = TokenType.Permission;
					break;
				case 9U:
					tokenType = TokenType.Property;
					break;
				case 10U:
					tokenType = TokenType.Event;
					break;
				case 11U:
					tokenType = TokenType.Signature;
					break;
				case 12U:
					tokenType = TokenType.ModuleRef;
					break;
				case 13U:
					tokenType = TokenType.TypeSpec;
					break;
				case 14U:
					tokenType = TokenType.Assembly;
					break;
				case 15U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 16U:
					tokenType = TokenType.File;
					break;
				case 17U:
					tokenType = TokenType.ExportedType;
					break;
				case 18U:
					tokenType = TokenType.ManifestResource;
					break;
				case 19U:
					tokenType = TokenType.GenericParam;
					break;
				case 20U:
					tokenType = TokenType.GenericParamConstraint;
					break;
				case 21U:
					tokenType = TokenType.MethodSpec;
					break;
				case 22U:
					tokenType = TokenType.Document;
					break;
				case 23U:
					tokenType = TokenType.LocalScope;
					break;
				case 24U:
					tokenType = TokenType.LocalVariable;
					break;
				case 25U:
					tokenType = TokenType.LocalConstant;
					break;
				case 26U:
					tokenType = TokenType.ImportScope;
					break;
				default:
					goto IL_05BB;
				}
				break;
			default:
				goto IL_05BB;
			}
			return new MetadataToken(tokenType, num);
			IL_05BB:
			return MetadataToken.Zero;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001240C File Offset: 0x0001060C
		public static uint CompressMetadataToken(this CodedIndex self, MetadataToken token)
		{
			uint num = 0U;
			if (token.RID == 0U)
			{
				return num;
			}
			switch (self)
			{
			case CodedIndex.TypeDefOrRef:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.TypeRef)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.TypeDef)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.TypeSpec)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.HasConstant:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Field)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Param)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.Property)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.HasCustomAttribute:
			{
				num = token.RID << 5;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.Event)
				{
					if (tokenType <= TokenType.Method)
					{
						if (tokenType <= TokenType.TypeRef)
						{
							if (tokenType == TokenType.Module)
							{
								return num | 7U;
							}
							if (tokenType == TokenType.TypeRef)
							{
								return num | 2U;
							}
						}
						else
						{
							if (tokenType == TokenType.TypeDef)
							{
								return num | 3U;
							}
							if (tokenType == TokenType.Field)
							{
								return num | 1U;
							}
							if (tokenType == TokenType.Method)
							{
								return num | 0U;
							}
						}
					}
					else if (tokenType <= TokenType.MemberRef)
					{
						if (tokenType == TokenType.Param)
						{
							return num | 4U;
						}
						if (tokenType == TokenType.InterfaceImpl)
						{
							return num | 5U;
						}
						if (tokenType == TokenType.MemberRef)
						{
							return num | 6U;
						}
					}
					else
					{
						if (tokenType == TokenType.Permission)
						{
							return num | 8U;
						}
						if (tokenType == TokenType.Signature)
						{
							return num | 11U;
						}
						if (tokenType == TokenType.Event)
						{
							return num | 10U;
						}
					}
				}
				else if (tokenType <= TokenType.AssemblyRef)
				{
					if (tokenType <= TokenType.ModuleRef)
					{
						if (tokenType == TokenType.Property)
						{
							return num | 9U;
						}
						if (tokenType == TokenType.ModuleRef)
						{
							return num | 12U;
						}
					}
					else
					{
						if (tokenType == TokenType.TypeSpec)
						{
							return num | 13U;
						}
						if (tokenType == TokenType.Assembly)
						{
							return num | 14U;
						}
						if (tokenType == TokenType.AssemblyRef)
						{
							return num | 15U;
						}
					}
				}
				else if (tokenType <= TokenType.ManifestResource)
				{
					if (tokenType == TokenType.File)
					{
						return num | 16U;
					}
					if (tokenType == TokenType.ExportedType)
					{
						return num | 17U;
					}
					if (tokenType == TokenType.ManifestResource)
					{
						return num | 18U;
					}
				}
				else
				{
					if (tokenType == TokenType.GenericParam)
					{
						return num | 19U;
					}
					if (tokenType == TokenType.MethodSpec)
					{
						return num | 21U;
					}
					if (tokenType == TokenType.GenericParamConstraint)
					{
						return num | 20U;
					}
				}
				break;
			}
			case CodedIndex.HasFieldMarshal:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Field)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Param)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.HasDeclSecurity:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.TypeDef)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Method)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.Assembly)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.MemberRefParent:
			{
				num = token.RID << 3;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.TypeDef)
				{
					if (tokenType == TokenType.TypeRef)
					{
						return num | 1U;
					}
					if (tokenType == TokenType.TypeDef)
					{
						return num | 0U;
					}
				}
				else
				{
					if (tokenType == TokenType.Method)
					{
						return num | 3U;
					}
					if (tokenType == TokenType.ModuleRef)
					{
						return num | 2U;
					}
					if (tokenType == TokenType.TypeSpec)
					{
						return num | 4U;
					}
				}
				break;
			}
			case CodedIndex.HasSemantics:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Event)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Property)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.MethodDefOrRef:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Method)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.MemberRef)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.MemberForwarded:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Field)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Method)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.Implementation:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.AssemblyRef)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.File)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.ExportedType)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.CustomAttributeType:
			{
				num = token.RID << 3;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Method)
				{
					return num | 2U;
				}
				if (tokenType == TokenType.MemberRef)
				{
					return num | 3U;
				}
				break;
			}
			case CodedIndex.ResolutionScope:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.TypeRef)
				{
					if (tokenType == TokenType.Module)
					{
						return num | 0U;
					}
					if (tokenType == TokenType.TypeRef)
					{
						return num | 3U;
					}
				}
				else
				{
					if (tokenType == TokenType.ModuleRef)
					{
						return num | 1U;
					}
					if (tokenType == TokenType.AssemblyRef)
					{
						return num | 2U;
					}
				}
				break;
			}
			case CodedIndex.TypeOrMethodDef:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.TypeDef)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Method)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.HasCustomDebugInformation:
			{
				num = token.RID << 5;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.ModuleRef)
				{
					if (tokenType <= TokenType.Param)
					{
						if (tokenType <= TokenType.TypeDef)
						{
							if (tokenType == TokenType.Module)
							{
								return num | 7U;
							}
							if (tokenType == TokenType.TypeRef)
							{
								return num | 2U;
							}
							if (tokenType == TokenType.TypeDef)
							{
								return num | 3U;
							}
						}
						else
						{
							if (tokenType == TokenType.Field)
							{
								return num | 1U;
							}
							if (tokenType == TokenType.Method)
							{
								return num | 0U;
							}
							if (tokenType == TokenType.Param)
							{
								return num | 4U;
							}
						}
					}
					else if (tokenType <= TokenType.Permission)
					{
						if (tokenType == TokenType.InterfaceImpl)
						{
							return num | 5U;
						}
						if (tokenType == TokenType.MemberRef)
						{
							return num | 6U;
						}
						if (tokenType == TokenType.Permission)
						{
							return num | 8U;
						}
					}
					else if (tokenType <= TokenType.Event)
					{
						if (tokenType == TokenType.Signature)
						{
							return num | 11U;
						}
						if (tokenType == TokenType.Event)
						{
							return num | 10U;
						}
					}
					else
					{
						if (tokenType == TokenType.Property)
						{
							return num | 9U;
						}
						if (tokenType == TokenType.ModuleRef)
						{
							return num | 12U;
						}
					}
				}
				else if (tokenType <= TokenType.GenericParam)
				{
					if (tokenType <= TokenType.AssemblyRef)
					{
						if (tokenType == TokenType.TypeSpec)
						{
							return num | 13U;
						}
						if (tokenType == TokenType.Assembly)
						{
							return num | 14U;
						}
						if (tokenType == TokenType.AssemblyRef)
						{
							return num | 15U;
						}
					}
					else if (tokenType <= TokenType.ExportedType)
					{
						if (tokenType == TokenType.File)
						{
							return num | 16U;
						}
						if (tokenType == TokenType.ExportedType)
						{
							return num | 17U;
						}
					}
					else
					{
						if (tokenType == TokenType.ManifestResource)
						{
							return num | 18U;
						}
						if (tokenType == TokenType.GenericParam)
						{
							return num | 19U;
						}
					}
				}
				else if (tokenType <= TokenType.Document)
				{
					if (tokenType == TokenType.MethodSpec)
					{
						return num | 21U;
					}
					if (tokenType == TokenType.GenericParamConstraint)
					{
						return num | 20U;
					}
					if (tokenType == TokenType.Document)
					{
						return num | 22U;
					}
				}
				else if (tokenType <= TokenType.LocalVariable)
				{
					if (tokenType == TokenType.LocalScope)
					{
						return num | 23U;
					}
					if (tokenType == TokenType.LocalVariable)
					{
						return num | 24U;
					}
				}
				else
				{
					if (tokenType == TokenType.LocalConstant)
					{
						return num | 25U;
					}
					if (tokenType == TokenType.ImportScope)
					{
						return num | 26U;
					}
				}
				break;
			}
			}
			throw new ArgumentException();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00012B10 File Offset: 0x00010D10
		public static int GetSize(this CodedIndex self, Func<Table, int> counter)
		{
			int num;
			Table[] array;
			switch (self)
			{
			case CodedIndex.TypeDefOrRef:
				num = 2;
				array = new Table[]
				{
					Table.TypeDef,
					Table.TypeRef,
					Table.TypeSpec
				};
				break;
			case CodedIndex.HasConstant:
				num = 2;
				array = new Table[]
				{
					Table.Field,
					Table.Param,
					Table.Property
				};
				break;
			case CodedIndex.HasCustomAttribute:
				num = 5;
				array = new Table[]
				{
					Table.Method,
					Table.Field,
					Table.TypeRef,
					Table.TypeDef,
					Table.Param,
					Table.InterfaceImpl,
					Table.MemberRef,
					Table.Module,
					Table.DeclSecurity,
					Table.Property,
					Table.Event,
					Table.StandAloneSig,
					Table.ModuleRef,
					Table.TypeSpec,
					Table.Assembly,
					Table.AssemblyRef,
					Table.File,
					Table.ExportedType,
					Table.ManifestResource,
					Table.GenericParam,
					Table.GenericParamConstraint,
					Table.MethodSpec
				};
				break;
			case CodedIndex.HasFieldMarshal:
				num = 1;
				array = new Table[]
				{
					Table.Field,
					Table.Param
				};
				break;
			case CodedIndex.HasDeclSecurity:
				num = 2;
				array = new Table[]
				{
					Table.TypeDef,
					Table.Method,
					Table.Assembly
				};
				break;
			case CodedIndex.MemberRefParent:
				num = 3;
				array = new Table[]
				{
					Table.TypeDef,
					Table.TypeRef,
					Table.ModuleRef,
					Table.Method,
					Table.TypeSpec
				};
				break;
			case CodedIndex.HasSemantics:
				num = 1;
				array = new Table[]
				{
					Table.Event,
					Table.Property
				};
				break;
			case CodedIndex.MethodDefOrRef:
				num = 1;
				array = new Table[]
				{
					Table.Method,
					Table.MemberRef
				};
				break;
			case CodedIndex.MemberForwarded:
				num = 1;
				array = new Table[]
				{
					Table.Field,
					Table.Method
				};
				break;
			case CodedIndex.Implementation:
				num = 2;
				array = new Table[]
				{
					Table.File,
					Table.AssemblyRef,
					Table.ExportedType
				};
				break;
			case CodedIndex.CustomAttributeType:
				num = 3;
				array = new Table[]
				{
					Table.Method,
					Table.MemberRef
				};
				break;
			case CodedIndex.ResolutionScope:
				num = 2;
				array = new Table[]
				{
					Table.Module,
					Table.ModuleRef,
					Table.AssemblyRef,
					Table.TypeRef
				};
				break;
			case CodedIndex.TypeOrMethodDef:
				num = 1;
				array = new Table[]
				{
					Table.TypeDef,
					Table.Method
				};
				break;
			case CodedIndex.HasCustomDebugInformation:
				num = 5;
				array = new Table[]
				{
					Table.Method,
					Table.Field,
					Table.TypeRef,
					Table.TypeDef,
					Table.Param,
					Table.InterfaceImpl,
					Table.MemberRef,
					Table.Module,
					Table.DeclSecurity,
					Table.Property,
					Table.Event,
					Table.StandAloneSig,
					Table.ModuleRef,
					Table.TypeSpec,
					Table.Assembly,
					Table.AssemblyRef,
					Table.File,
					Table.ExportedType,
					Table.ManifestResource,
					Table.GenericParam,
					Table.GenericParamConstraint,
					Table.MethodSpec,
					Table.Document,
					Table.LocalScope,
					Table.LocalVariable,
					Table.LocalConstant,
					Table.ImportScope
				};
				break;
			default:
				throw new ArgumentException();
			}
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num2 = Math.Max(counter(array[i]), num2);
			}
			if (num2 >= 1 << 16 - num)
			{
				return 4;
			}
			return 2;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00012DE8 File Offset: 0x00010FE8
		public static RSA CreateRSA(this WriterParameters writer_parameters)
		{
			if (writer_parameters.StrongNameKeyBlob != null)
			{
				return CryptoConvert.FromCapiKeyBlob(writer_parameters.StrongNameKeyBlob);
			}
			string strongNameKeyContainer;
			byte[] array;
			if (writer_parameters.StrongNameKeyContainer != null)
			{
				strongNameKeyContainer = writer_parameters.StrongNameKeyContainer;
			}
			else if (!Mixin.TryGetKeyContainer(writer_parameters.StrongNameKeyPair, out array, out strongNameKeyContainer))
			{
				return CryptoConvert.FromCapiKeyBlob(array);
			}
			return new RSACryptoServiceProvider(new CspParameters
			{
				Flags = CspProviderFlags.UseMachineKeyStore,
				KeyContainerName = strongNameKeyContainer,
				KeyNumber = 2
			});
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00012E54 File Offset: 0x00011054
		private static bool TryGetKeyContainer(ISerializable key_pair, out byte[] key, out string key_container)
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(StrongNameKeyPair), new FormatterConverter());
			key_pair.GetObjectData(serializationInfo, default(StreamingContext));
			key = (byte[])serializationInfo.GetValue("_keyPairArray", typeof(byte[]));
			key_container = serializationInfo.GetString("_keyPairContainer");
			return key_container != null;
		}

		// Token: 0x040001F1 RID: 497
		public static Version ZeroVersion = new Version(0, 0, 0, 0);

		// Token: 0x040001F2 RID: 498
		public const int NotResolvedMarker = -2;

		// Token: 0x040001F3 RID: 499
		public const int NoDataMarker = -1;

		// Token: 0x040001F4 RID: 500
		internal static object NoValue = new object();

		// Token: 0x040001F5 RID: 501
		internal static object NotResolved = new object();

		// Token: 0x040001F6 RID: 502
		public const string mscorlib = "mscorlib";

		// Token: 0x040001F7 RID: 503
		public const string system_runtime = "System.Runtime";

		// Token: 0x040001F8 RID: 504
		public const string system_private_corelib = "System.Private.CoreLib";

		// Token: 0x040001F9 RID: 505
		public const string netstandard = "netstandard";

		// Token: 0x040001FA RID: 506
		public const int TableCount = 58;

		// Token: 0x040001FB RID: 507
		public const int CodedIndexCount = 14;

		// Token: 0x020000B7 RID: 183
		public enum Argument
		{
			// Token: 0x040001FD RID: 509
			name,
			// Token: 0x040001FE RID: 510
			fileName,
			// Token: 0x040001FF RID: 511
			fullName,
			// Token: 0x04000200 RID: 512
			stream,
			// Token: 0x04000201 RID: 513
			type,
			// Token: 0x04000202 RID: 514
			method,
			// Token: 0x04000203 RID: 515
			field,
			// Token: 0x04000204 RID: 516
			parameters,
			// Token: 0x04000205 RID: 517
			module,
			// Token: 0x04000206 RID: 518
			modifierType,
			// Token: 0x04000207 RID: 519
			eventType,
			// Token: 0x04000208 RID: 520
			fieldType,
			// Token: 0x04000209 RID: 521
			declaringType,
			// Token: 0x0400020A RID: 522
			returnType,
			// Token: 0x0400020B RID: 523
			propertyType,
			// Token: 0x0400020C RID: 524
			interfaceType,
			// Token: 0x0400020D RID: 525
			constraintType
		}
	}
}
