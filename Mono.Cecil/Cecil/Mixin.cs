using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;
using Mono.Security.Cryptography;

namespace Mono.Cecil
{
	// Token: 0x0200000C RID: 12
	internal static class Mixin
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00002C5C File Offset: 0x00000E5C
		public static bool IsNullOrEmpty<T>(this T[] self)
		{
			return self == null || self.Length == 0;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002C68 File Offset: 0x00000E68
		public static bool IsNullOrEmpty<T>(this Collection<T> self)
		{
			return self == null || self.size == 0;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002C78 File Offset: 0x00000E78
		public static T[] Resize<T>(this T[] self, int length)
		{
			Array.Resize<T>(ref self, length);
			return self;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002C83 File Offset: 0x00000E83
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

		// Token: 0x0600005A RID: 90 RVA: 0x00002CB4 File Offset: 0x00000EB4
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

		// Token: 0x0600005B RID: 91 RVA: 0x00002D10 File Offset: 0x00000F10
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

		// Token: 0x0600005C RID: 92 RVA: 0x00002D60 File Offset: 0x00000F60
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

		// Token: 0x0600005D RID: 93 RVA: 0x00002DEC File Offset: 0x00000FEC
		public static bool GetHasCustomAttributes(this ICustomAttributeProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ICustomAttributeProvider, bool>(self, (ICustomAttributeProvider provider, MetadataReader reader) => reader.HasCustomAttributes(provider));
			}
			return false;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002E20 File Offset: 0x00001020
		public static Collection<CustomAttribute> GetCustomAttributes(this ICustomAttributeProvider self, ref Collection<CustomAttribute> variable, ModuleDefinition module)
		{
			if (!module.HasImage())
			{
				Collection<CustomAttribute> collection;
				variable = (collection = new Collection<CustomAttribute>());
				return collection;
			}
			return module.Read<ICustomAttributeProvider, Collection<CustomAttribute>>(ref variable, self, (ICustomAttributeProvider provider, MetadataReader reader) => reader.ReadCustomAttributes(provider));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002E68 File Offset: 0x00001068
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

		// Token: 0x06000060 RID: 96 RVA: 0x00002EA0 File Offset: 0x000010A0
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

		// Token: 0x06000061 RID: 97 RVA: 0x00002F00 File Offset: 0x00001100
		public static bool GetHasGenericParameters(this IGenericParameterProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IGenericParameterProvider, bool>(self, (IGenericParameterProvider provider, MetadataReader reader) => reader.HasGenericParameters(provider));
			}
			return false;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002F34 File Offset: 0x00001134
		public static Collection<GenericParameter> GetGenericParameters(this IGenericParameterProvider self, ref Collection<GenericParameter> collection, ModuleDefinition module)
		{
			if (!module.HasImage())
			{
				Collection<GenericParameter> collection2;
				collection = (collection2 = new GenericParameterCollection(self));
				return collection2;
			}
			return module.Read<IGenericParameterProvider, Collection<GenericParameter>>(ref collection, self, (IGenericParameterProvider provider, MetadataReader reader) => reader.ReadGenericParameters(provider));
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002F7C File Offset: 0x0000117C
		public static bool GetHasMarshalInfo(this IMarshalInfoProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IMarshalInfoProvider, bool>(self, (IMarshalInfoProvider provider, MetadataReader reader) => reader.HasMarshalInfo(provider));
			}
			return false;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002FAE File Offset: 0x000011AE
		public static MarshalInfo GetMarshalInfo(this IMarshalInfoProvider self, ref MarshalInfo variable, ModuleDefinition module)
		{
			if (!module.HasImage())
			{
				return null;
			}
			return module.Read<IMarshalInfoProvider, MarshalInfo>(ref variable, self, (IMarshalInfoProvider provider, MetadataReader reader) => reader.ReadMarshalInfo(provider));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002FE1 File Offset: 0x000011E1
		public static bool GetAttributes(this uint self, uint attributes)
		{
			return (self & attributes) > 0U;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002FE9 File Offset: 0x000011E9
		public static uint SetAttributes(this uint self, uint attributes, bool value)
		{
			if (value)
			{
				return self | attributes;
			}
			return self & ~attributes;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002FF6 File Offset: 0x000011F6
		public static bool GetMaskedAttributes(this uint self, uint mask, uint attributes)
		{
			return (self & mask) == attributes;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002FFE File Offset: 0x000011FE
		public static uint SetMaskedAttributes(this uint self, uint mask, uint attributes, bool value)
		{
			if (value)
			{
				self &= ~mask;
				return self | attributes;
			}
			return self & ~(mask & attributes);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002FE1 File Offset: 0x000011E1
		public static bool GetAttributes(this ushort self, ushort attributes)
		{
			return (self & attributes) > 0;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003013 File Offset: 0x00001213
		public static ushort SetAttributes(this ushort self, ushort attributes, bool value)
		{
			if (value)
			{
				return self | attributes;
			}
			return self & ~attributes;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003022 File Offset: 0x00001222
		public static bool GetMaskedAttributes(this ushort self, ushort mask, uint attributes)
		{
			return (long)(self & mask) == (long)((ulong)attributes);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000302C File Offset: 0x0000122C
		public static ushort SetMaskedAttributes(this ushort self, ushort mask, uint attributes, bool value)
		{
			if (value)
			{
				self &= ~mask;
				return (ushort)((uint)self | attributes);
			}
			return (ushort)((uint)self & ~((uint)mask & attributes));
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003044 File Offset: 0x00001244
		public static bool HasImplicitThis(this IMethodSignature self)
		{
			return self.HasThis && !self.ExplicitThis;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000305C File Offset: 0x0000125C
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

		// Token: 0x0600006F RID: 111 RVA: 0x000030E4 File Offset: 0x000012E4
		public static void CheckModule(ModuleDefinition module)
		{
			if (module == null)
			{
				throw new ArgumentNullException(Mixin.Argument.module.ToString());
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000310C File Offset: 0x0000130C
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

		// Token: 0x06000071 RID: 113 RVA: 0x0000314C File Offset: 0x0000134C
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

		// Token: 0x06000072 RID: 114 RVA: 0x00003187 File Offset: 0x00001387
		private static bool Equals<T>(T a, T b) where T : class, IEquatable<T>
		{
			return a == b || (a != null && a.Equals(b));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000031B0 File Offset: 0x000013B0
		private static bool Equals(AssemblyNameReference a, AssemblyNameReference b)
		{
			return a == b || (!(a.Name != b.Name) && Mixin.Equals<Version>(a.Version, b.Version) && !(a.Culture != b.Culture) && Mixin.Equals(a.PublicKeyToken, b.PublicKeyToken));
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003218 File Offset: 0x00001418
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

		// Token: 0x06000075 RID: 117 RVA: 0x00003260 File Offset: 0x00001460
		public static VariableDefinition GetVariable(this Mono.Cecil.Cil.MethodBody self, int index)
		{
			Collection<VariableDefinition> variables = self.Variables;
			if (index < 0 || index >= variables.size)
			{
				return null;
			}
			return variables[index];
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000328A File Offset: 0x0000148A
		public static bool GetSemantics(this MethodDefinition self, MethodSemanticsAttributes semantics)
		{
			return (self.SemanticsAttributes & semantics) > MethodSemanticsAttributes.None;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003297 File Offset: 0x00001497
		public static void SetSemantics(this MethodDefinition self, MethodSemanticsAttributes semantics, bool value)
		{
			if (value)
			{
				self.SemanticsAttributes |= semantics;
				return;
			}
			self.SemanticsAttributes &= ~semantics;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000032BB File Offset: 0x000014BB
		public static bool IsVarArg(this IMethodSignature self)
		{
			return self.CallingConvention == MethodCallingConvention.VarArg;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000032C8 File Offset: 0x000014C8
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

		// Token: 0x0600007A RID: 122 RVA: 0x00003310 File Offset: 0x00001510
		public static void CheckName(object name)
		{
			if (name == null)
			{
				throw new ArgumentNullException(Mixin.Argument.name.ToString());
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003338 File Offset: 0x00001538
		public static void CheckName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.name.ToString());
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003364 File Offset: 0x00001564
		public static void CheckFileName(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.fileName.ToString());
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003390 File Offset: 0x00001590
		public static void CheckFullName(string fullName)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.fullName.ToString());
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000033BC File Offset: 0x000015BC
		public static void CheckStream(object stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(Mixin.Argument.stream.ToString());
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000033E1 File Offset: 0x000015E1
		public static void CheckWriteSeek(Stream stream)
		{
			if (!stream.CanWrite || !stream.CanSeek)
			{
				throw new ArgumentException("Stream must be writable and seekable.");
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000033FE File Offset: 0x000015FE
		public static void CheckReadSeek(Stream stream)
		{
			if (!stream.CanRead || !stream.CanSeek)
			{
				throw new ArgumentException("Stream must be readable and seekable.");
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000341C File Offset: 0x0000161C
		public static void CheckType(object type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(Mixin.Argument.type.ToString());
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003441 File Offset: 0x00001641
		public static void CheckType(object type, Mixin.Argument argument)
		{
			if (type == null)
			{
				throw new ArgumentNullException(argument.ToString());
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000345C File Offset: 0x0000165C
		public static void CheckField(object field)
		{
			if (field == null)
			{
				throw new ArgumentNullException(Mixin.Argument.field.ToString());
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003484 File Offset: 0x00001684
		public static void CheckMethod(object method)
		{
			if (method == null)
			{
				throw new ArgumentNullException(Mixin.Argument.method.ToString());
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000034AC File Offset: 0x000016AC
		public static void CheckParameters(object parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(Mixin.Argument.parameters.ToString());
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000034D4 File Offset: 0x000016D4
		public static uint GetTimestamp()
		{
			return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003503 File Offset: 0x00001703
		public static bool HasImage(this ModuleDefinition self)
		{
			return self != null && self.HasImage;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003510 File Offset: 0x00001710
		public static string GetFileName(this Stream self)
		{
			FileStream fileStream = self as FileStream;
			if (fileStream == null)
			{
				return string.Empty;
			}
			return Path.GetFullPath(fileStream.Name);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003538 File Offset: 0x00001738
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

		// Token: 0x0600008A RID: 138 RVA: 0x00003584 File Offset: 0x00001784
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

		// Token: 0x0600008B RID: 139 RVA: 0x000035B5 File Offset: 0x000017B5
		public static bool IsWindowsMetadata(this ModuleDefinition module)
		{
			return module.MetadataKind > MetadataKind.Ecma335;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000035C0 File Offset: 0x000017C0
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

		// Token: 0x0600008D RID: 141 RVA: 0x00002A0D File Offset: 0x00000C0D
		public static void Read(object o)
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003605 File Offset: 0x00001805
		public static bool GetHasSecurityDeclarations(this ISecurityDeclarationProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ISecurityDeclarationProvider, bool>(self, (ISecurityDeclarationProvider provider, MetadataReader reader) => reader.HasSecurityDeclarations(provider));
			}
			return false;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003638 File Offset: 0x00001838
		public static Collection<SecurityDeclaration> GetSecurityDeclarations(this ISecurityDeclarationProvider self, ref Collection<SecurityDeclaration> variable, ModuleDefinition module)
		{
			if (!module.HasImage())
			{
				Collection<SecurityDeclaration> collection;
				variable = (collection = new Collection<SecurityDeclaration>());
				return collection;
			}
			return module.Read<ISecurityDeclarationProvider, Collection<SecurityDeclaration>>(ref variable, self, (ISecurityDeclarationProvider provider, MetadataReader reader) => reader.ReadSecurityDeclarations(provider));
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003680 File Offset: 0x00001880
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

		// Token: 0x06000091 RID: 145 RVA: 0x000036C4 File Offset: 0x000018C4
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

		// Token: 0x06000092 RID: 146 RVA: 0x0000370C File Offset: 0x0000190C
		public static bool IsPrimitive(this ElementType self)
		{
			switch (self)
			{
			case ElementType.Boolean:
			case ElementType.Char:
			case ElementType.I1:
			case ElementType.U1:
			case ElementType.I2:
			case ElementType.U2:
			case ElementType.I4:
			case ElementType.U4:
			case ElementType.I8:
			case ElementType.U8:
			case ElementType.R4:
			case ElementType.R8:
			case ElementType.I:
			case ElementType.U:
				return true;
			}
			return false;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003786 File Offset: 0x00001986
		public static string TypeFullName(this TypeReference self)
		{
			if (!string.IsNullOrEmpty(self.Namespace))
			{
				return self.Namespace + "." + self.Name;
			}
			return self.Name;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000037B2 File Offset: 0x000019B2
		public static bool IsTypeOf(this TypeReference self, string @namespace, string name)
		{
			return self.Name == name && self.Namespace == @namespace;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000037D0 File Offset: 0x000019D0
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

		// Token: 0x06000096 RID: 150 RVA: 0x00003842 File Offset: 0x00001A42
		public static TypeDefinition CheckedResolve(this TypeReference self)
		{
			TypeDefinition typeDefinition = self.Resolve();
			if (typeDefinition == null)
			{
				throw new ResolutionException(self);
			}
			return typeDefinition;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003854 File Offset: 0x00001A54
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

		// Token: 0x06000098 RID: 152 RVA: 0x00003894 File Offset: 0x00001A94
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

		// Token: 0x06000099 RID: 153 RVA: 0x000038F2 File Offset: 0x00001AF2
		public static void KnownValueType(this TypeReference type)
		{
			if (!type.IsDefinition)
			{
				type.IsValueType = true;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003904 File Offset: 0x00001B04
		private static bool IsCoreLibrary(AssemblyNameReference reference)
		{
			string name = reference.Name;
			return name == "mscorlib" || name == "System.Runtime" || name == "System.Private.CoreLib" || name == "netstandard";
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000394C File Offset: 0x00001B4C
		public static ImageDebugHeaderEntry GetCodeViewEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.CodeView);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003955 File Offset: 0x00001B55
		public static ImageDebugHeaderEntry GetDeterministicEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.Deterministic);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003960 File Offset: 0x00001B60
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

		// Token: 0x0600009E RID: 158 RVA: 0x000039C6 File Offset: 0x00001BC6
		public static ImageDebugHeaderEntry GetEmbeddedPortablePdbEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.EmbeddedPortablePdb);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000039D0 File Offset: 0x00001BD0
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

		// Token: 0x060000A0 RID: 160 RVA: 0x00003A14 File Offset: 0x00001C14
		public static string GetPdbFileName(string assemblyFileName)
		{
			return Path.ChangeExtension(assemblyFileName, ".pdb");
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003A21 File Offset: 0x00001C21
		public static string GetMdbFileName(string assemblyFileName)
		{
			return assemblyFileName + ".mdb";
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003A30 File Offset: 0x00001C30
		public static bool IsPortablePdb(string fileName)
		{
			bool flag;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				flag = Mixin.IsPortablePdb(fileStream);
			}
			return flag;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003A6C File Offset: 0x00001C6C
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

		// Token: 0x060000A4 RID: 164 RVA: 0x00003ABC File Offset: 0x00001CBC
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

		// Token: 0x060000A5 RID: 165 RVA: 0x00003B40 File Offset: 0x00001D40
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

		// Token: 0x060000A6 RID: 166 RVA: 0x00004110 File Offset: 0x00002310
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

		// Token: 0x060000A7 RID: 167 RVA: 0x00004814 File Offset: 0x00002A14
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

		// Token: 0x060000A8 RID: 168 RVA: 0x00004AEC File Offset: 0x00002CEC
		public static RSA CreateRSA(this StrongNameKeyPair key_pair)
		{
			byte[] array;
			string text;
			if (!Mixin.TryGetKeyContainer(key_pair, out array, out text))
			{
				return CryptoConvert.FromCapiKeyBlob(array);
			}
			return new RSACryptoServiceProvider(new CspParameters
			{
				Flags = CspProviderFlags.UseMachineKeyStore,
				KeyContainerName = text,
				KeyNumber = 2
			});
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004B2C File Offset: 0x00002D2C
		private static bool TryGetKeyContainer(ISerializable key_pair, out byte[] key, out string key_container)
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(StrongNameKeyPair), new FormatterConverter());
			key_pair.GetObjectData(serializationInfo, default(StreamingContext));
			key = (byte[])serializationInfo.GetValue("_keyPairArray", typeof(byte[]));
			key_container = serializationInfo.GetString("_keyPairContainer");
			return key_container != null;
		}

		// Token: 0x0400000D RID: 13
		public static Version ZeroVersion = new Version(0, 0, 0, 0);

		// Token: 0x0400000E RID: 14
		public const int NotResolvedMarker = -2;

		// Token: 0x0400000F RID: 15
		public const int NoDataMarker = -1;

		// Token: 0x04000010 RID: 16
		internal static object NoValue = new object();

		// Token: 0x04000011 RID: 17
		internal static object NotResolved = new object();

		// Token: 0x04000012 RID: 18
		public const string mscorlib = "mscorlib";

		// Token: 0x04000013 RID: 19
		public const string system_runtime = "System.Runtime";

		// Token: 0x04000014 RID: 20
		public const string system_private_corelib = "System.Private.CoreLib";

		// Token: 0x04000015 RID: 21
		public const string netstandard = "netstandard";

		// Token: 0x04000016 RID: 22
		public const int TableCount = 58;

		// Token: 0x04000017 RID: 23
		public const int CodedIndexCount = 14;

		// Token: 0x02000139 RID: 313
		public enum Argument
		{
			// Token: 0x040006F4 RID: 1780
			name,
			// Token: 0x040006F5 RID: 1781
			fileName,
			// Token: 0x040006F6 RID: 1782
			fullName,
			// Token: 0x040006F7 RID: 1783
			stream,
			// Token: 0x040006F8 RID: 1784
			type,
			// Token: 0x040006F9 RID: 1785
			method,
			// Token: 0x040006FA RID: 1786
			field,
			// Token: 0x040006FB RID: 1787
			parameters,
			// Token: 0x040006FC RID: 1788
			module,
			// Token: 0x040006FD RID: 1789
			modifierType,
			// Token: 0x040006FE RID: 1790
			eventType,
			// Token: 0x040006FF RID: 1791
			fieldType,
			// Token: 0x04000700 RID: 1792
			declaringType,
			// Token: 0x04000701 RID: 1793
			returnType,
			// Token: 0x04000702 RID: 1794
			propertyType,
			// Token: 0x04000703 RID: 1795
			interfaceType
		}
	}
}
