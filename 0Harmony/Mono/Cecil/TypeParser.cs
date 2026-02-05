using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200017A RID: 378
	internal class TypeParser
	{
		// Token: 0x06000BC6 RID: 3014 RVA: 0x0002710A File Offset: 0x0002530A
		private TypeParser(string fullname)
		{
			this.fullname = fullname;
			this.length = fullname.Length;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00027128 File Offset: 0x00025328
		private TypeParser.Type ParseType(bool fq_name)
		{
			TypeParser.Type type = new TypeParser.Type();
			type.type_fullname = this.ParsePart();
			type.nested_names = this.ParseNestedNames();
			if (TypeParser.TryGetArity(type))
			{
				type.generic_arguments = this.ParseGenericArguments(type.arity);
			}
			type.specs = this.ParseSpecs();
			if (fq_name)
			{
				type.assembly = this.ParseAssemblyName();
			}
			return type;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002718C File Offset: 0x0002538C
		private static bool TryGetArity(TypeParser.Type type)
		{
			int num = 0;
			TypeParser.TryAddArity(type.type_fullname, ref num);
			string[] nested_names = type.nested_names;
			if (!nested_names.IsNullOrEmpty<string>())
			{
				for (int i = 0; i < nested_names.Length; i++)
				{
					TypeParser.TryAddArity(nested_names[i], ref num);
				}
			}
			type.arity = num;
			return num > 0;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000271DC File Offset: 0x000253DC
		private static bool TryGetArity(string name, out int arity)
		{
			arity = 0;
			int num = name.LastIndexOf('`');
			return num != -1 && TypeParser.ParseInt32(name.Substring(num + 1), out arity);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002720A File Offset: 0x0002540A
		private static bool ParseInt32(string value, out int result)
		{
			return int.TryParse(value, out result);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00027214 File Offset: 0x00025414
		private static void TryAddArity(string name, ref int arity)
		{
			int num;
			if (!TypeParser.TryGetArity(name, out num))
			{
				return;
			}
			arity += num;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00027234 File Offset: 0x00025434
		private string ParsePart()
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (this.position < this.length && !TypeParser.IsDelimiter(this.fullname[this.position]))
			{
				if (this.fullname[this.position] == '\\')
				{
					this.position++;
				}
				StringBuilder stringBuilder2 = stringBuilder;
				string text = this.fullname;
				int num = this.position;
				this.position = num + 1;
				stringBuilder2.Append(text[num]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000272BB File Offset: 0x000254BB
		private static bool IsDelimiter(char chr)
		{
			return "+,[]*&".IndexOf(chr) != -1;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000272CE File Offset: 0x000254CE
		private void TryParseWhiteSpace()
		{
			while (this.position < this.length && char.IsWhiteSpace(this.fullname[this.position]))
			{
				this.position++;
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00027308 File Offset: 0x00025508
		private string[] ParseNestedNames()
		{
			string[] array = null;
			while (this.TryParse('+'))
			{
				TypeParser.Add<string>(ref array, this.ParsePart());
			}
			return array;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00027331 File Offset: 0x00025531
		private bool TryParse(char chr)
		{
			if (this.position < this.length && this.fullname[this.position] == chr)
			{
				this.position++;
				return true;
			}
			return false;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00027366 File Offset: 0x00025566
		private static void Add<T>(ref T[] array, T item)
		{
			array = array.Add(item);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00027374 File Offset: 0x00025574
		private int[] ParseSpecs()
		{
			int[] array = null;
			while (this.position < this.length)
			{
				char c = this.fullname[this.position];
				if (c != '&')
				{
					if (c != '*')
					{
						if (c != '[')
						{
							return array;
						}
						this.position++;
						char c2 = this.fullname[this.position];
						if (c2 != '*')
						{
							if (c2 == ']')
							{
								this.position++;
								TypeParser.Add<int>(ref array, -3);
							}
							else
							{
								int num = 1;
								while (this.TryParse(','))
								{
									num++;
								}
								TypeParser.Add<int>(ref array, num);
								this.TryParse(']');
							}
						}
						else
						{
							this.position++;
							TypeParser.Add<int>(ref array, 1);
						}
					}
					else
					{
						this.position++;
						TypeParser.Add<int>(ref array, -1);
					}
				}
				else
				{
					this.position++;
					TypeParser.Add<int>(ref array, -2);
				}
			}
			return array;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0002747C File Offset: 0x0002567C
		private TypeParser.Type[] ParseGenericArguments(int arity)
		{
			TypeParser.Type[] array = null;
			if (this.position == this.length || this.fullname[this.position] != '[')
			{
				return array;
			}
			this.TryParse('[');
			for (int i = 0; i < arity; i++)
			{
				bool flag = this.TryParse('[');
				TypeParser.Add<TypeParser.Type>(ref array, this.ParseType(flag));
				if (flag)
				{
					this.TryParse(']');
				}
				this.TryParse(',');
				this.TryParseWhiteSpace();
			}
			this.TryParse(']');
			return array;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00027504 File Offset: 0x00025704
		private string ParseAssemblyName()
		{
			if (!this.TryParse(','))
			{
				return string.Empty;
			}
			this.TryParseWhiteSpace();
			int num = this.position;
			while (this.position < this.length)
			{
				char c = this.fullname[this.position];
				if (c == '[' || c == ']')
				{
					break;
				}
				this.position++;
			}
			return this.fullname.Substring(num, this.position - num);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0002757C File Offset: 0x0002577C
		public static TypeReference ParseType(ModuleDefinition module, string fullname, bool typeDefinitionOnly = false)
		{
			if (string.IsNullOrEmpty(fullname))
			{
				return null;
			}
			TypeParser typeParser = new TypeParser(fullname);
			return TypeParser.GetTypeReference(module, typeParser.ParseType(true), typeDefinitionOnly);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000275A8 File Offset: 0x000257A8
		private static TypeReference GetTypeReference(ModuleDefinition module, TypeParser.Type type_info, bool type_def_only)
		{
			TypeReference typeReference;
			if (!TypeParser.TryGetDefinition(module, type_info, out typeReference))
			{
				if (type_def_only)
				{
					return null;
				}
				typeReference = TypeParser.CreateReference(type_info, module, TypeParser.GetMetadataScope(module, type_info));
			}
			return TypeParser.CreateSpecs(typeReference, type_info);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000275DC File Offset: 0x000257DC
		private static TypeReference CreateSpecs(TypeReference type, TypeParser.Type type_info)
		{
			type = TypeParser.TryCreateGenericInstanceType(type, type_info);
			int[] specs = type_info.specs;
			if (specs.IsNullOrEmpty<int>())
			{
				return type;
			}
			for (int i = 0; i < specs.Length; i++)
			{
				switch (specs[i])
				{
				case -3:
					type = new ArrayType(type);
					break;
				case -2:
					type = new ByReferenceType(type);
					break;
				case -1:
					type = new PointerType(type);
					break;
				default:
				{
					ArrayType arrayType = new ArrayType(type);
					arrayType.Dimensions.Clear();
					for (int j = 0; j < specs[i]; j++)
					{
						arrayType.Dimensions.Add(default(ArrayDimension));
					}
					type = arrayType;
					break;
				}
				}
			}
			return type;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00027688 File Offset: 0x00025888
		private static TypeReference TryCreateGenericInstanceType(TypeReference type, TypeParser.Type type_info)
		{
			TypeParser.Type[] generic_arguments = type_info.generic_arguments;
			if (generic_arguments.IsNullOrEmpty<TypeParser.Type>())
			{
				return type;
			}
			GenericInstanceType genericInstanceType = new GenericInstanceType(type, generic_arguments.Length);
			Collection<TypeReference> genericArguments = genericInstanceType.GenericArguments;
			for (int i = 0; i < generic_arguments.Length; i++)
			{
				genericArguments.Add(TypeParser.GetTypeReference(type.Module, generic_arguments[i], false));
			}
			return genericInstanceType;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x000276DC File Offset: 0x000258DC
		public static void SplitFullName(string fullname, out string @namespace, out string name)
		{
			int num = fullname.LastIndexOf('.');
			if (num == -1)
			{
				@namespace = string.Empty;
				name = fullname;
				return;
			}
			@namespace = fullname.Substring(0, num);
			name = fullname.Substring(num + 1);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00027718 File Offset: 0x00025918
		private static TypeReference CreateReference(TypeParser.Type type_info, ModuleDefinition module, IMetadataScope scope)
		{
			string text;
			string text2;
			TypeParser.SplitFullName(type_info.type_fullname, out text, out text2);
			TypeReference typeReference = new TypeReference(text, text2, module, scope);
			MetadataSystem.TryProcessPrimitiveTypeReference(typeReference);
			TypeParser.AdjustGenericParameters(typeReference);
			string[] nested_names = type_info.nested_names;
			if (nested_names.IsNullOrEmpty<string>())
			{
				return typeReference;
			}
			for (int i = 0; i < nested_names.Length; i++)
			{
				typeReference = new TypeReference(string.Empty, nested_names[i], module, null)
				{
					DeclaringType = typeReference
				};
				TypeParser.AdjustGenericParameters(typeReference);
			}
			return typeReference;
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0002778C File Offset: 0x0002598C
		private static void AdjustGenericParameters(TypeReference type)
		{
			int num;
			if (!TypeParser.TryGetArity(type.Name, out num))
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				type.GenericParameters.Add(new GenericParameter(type));
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000277C8 File Offset: 0x000259C8
		private static IMetadataScope GetMetadataScope(ModuleDefinition module, TypeParser.Type type_info)
		{
			if (string.IsNullOrEmpty(type_info.assembly))
			{
				return module.TypeSystem.CoreLibrary;
			}
			AssemblyNameReference assemblyNameReference = AssemblyNameReference.Parse(type_info.assembly);
			AssemblyNameReference assemblyNameReference2;
			if (!module.TryGetAssemblyNameReference(assemblyNameReference, out assemblyNameReference2))
			{
				return assemblyNameReference;
			}
			return assemblyNameReference2;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00027808 File Offset: 0x00025A08
		private static bool TryGetDefinition(ModuleDefinition module, TypeParser.Type type_info, out TypeReference type)
		{
			type = null;
			if (!TypeParser.TryCurrentModule(module, type_info))
			{
				return false;
			}
			TypeDefinition typeDefinition = module.GetType(type_info.type_fullname);
			if (typeDefinition == null)
			{
				return false;
			}
			string[] nested_names = type_info.nested_names;
			if (!nested_names.IsNullOrEmpty<string>())
			{
				for (int i = 0; i < nested_names.Length; i++)
				{
					TypeDefinition nestedType = typeDefinition.GetNestedType(nested_names[i]);
					if (nestedType == null)
					{
						return false;
					}
					typeDefinition = nestedType;
				}
			}
			type = typeDefinition;
			return true;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00027867 File Offset: 0x00025A67
		private static bool TryCurrentModule(ModuleDefinition module, TypeParser.Type type_info)
		{
			return string.IsNullOrEmpty(type_info.assembly) || (module.assembly != null && module.assembly.Name.FullName == type_info.assembly);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x000278A0 File Offset: 0x00025AA0
		public static string ToParseable(TypeReference type, bool top_level = true)
		{
			if (type == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			TypeParser.AppendType(type, stringBuilder, true, top_level);
			return stringBuilder.ToString();
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x000278C8 File Offset: 0x00025AC8
		private static void AppendNamePart(string part, StringBuilder name)
		{
			foreach (char c in part)
			{
				if (TypeParser.IsDelimiter(c))
				{
					name.Append('\\');
				}
				name.Append(c);
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002790C File Offset: 0x00025B0C
		private static void AppendType(TypeReference type, StringBuilder name, bool fq_name, bool top_level)
		{
			TypeReference elementType = type.GetElementType();
			TypeReference declaringType = elementType.DeclaringType;
			if (declaringType != null)
			{
				TypeParser.AppendType(declaringType, name, false, top_level);
				name.Append('+');
			}
			string @namespace = type.Namespace;
			if (!string.IsNullOrEmpty(@namespace))
			{
				TypeParser.AppendNamePart(@namespace, name);
				name.Append('.');
			}
			TypeParser.AppendNamePart(elementType.Name, name);
			if (!fq_name)
			{
				return;
			}
			if (type.IsTypeSpecification())
			{
				TypeParser.AppendTypeSpecification((TypeSpecification)type, name);
			}
			if (TypeParser.RequiresFullyQualifiedName(type, top_level))
			{
				name.Append(", ");
				name.Append(TypeParser.GetScopeFullName(type));
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x000279A0 File Offset: 0x00025BA0
		private static string GetScopeFullName(TypeReference type)
		{
			IMetadataScope scope = type.Scope;
			MetadataScopeType metadataScopeType = scope.MetadataScopeType;
			if (metadataScopeType == MetadataScopeType.AssemblyNameReference)
			{
				return ((AssemblyNameReference)scope).FullName;
			}
			if (metadataScopeType != MetadataScopeType.ModuleDefinition)
			{
				throw new ArgumentException();
			}
			return ((ModuleDefinition)scope).Assembly.Name.FullName;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x000279EC File Offset: 0x00025BEC
		private static void AppendTypeSpecification(TypeSpecification type, StringBuilder name)
		{
			if (type.ElementType.IsTypeSpecification())
			{
				TypeParser.AppendTypeSpecification((TypeSpecification)type.ElementType, name);
			}
			ElementType etype = type.etype;
			switch (etype)
			{
			case ElementType.Ptr:
				name.Append('*');
				return;
			case ElementType.ByRef:
				name.Append('&');
				return;
			case ElementType.ValueType:
			case ElementType.Class:
			case ElementType.Var:
				return;
			case ElementType.Array:
				break;
			case ElementType.GenericInst:
			{
				Collection<TypeReference> genericArguments = ((GenericInstanceType)type).GenericArguments;
				name.Append('[');
				for (int i = 0; i < genericArguments.Count; i++)
				{
					if (i > 0)
					{
						name.Append(',');
					}
					TypeReference typeReference = genericArguments[i];
					bool flag = typeReference.Scope != typeReference.Module;
					if (flag)
					{
						name.Append('[');
					}
					TypeParser.AppendType(typeReference, name, true, false);
					if (flag)
					{
						name.Append(']');
					}
				}
				name.Append(']');
				return;
			}
			default:
				if (etype != ElementType.SzArray)
				{
					return;
				}
				break;
			}
			ArrayType arrayType = (ArrayType)type;
			if (arrayType.IsVector)
			{
				name.Append("[]");
				return;
			}
			name.Append('[');
			for (int j = 1; j < arrayType.Rank; j++)
			{
				name.Append(',');
			}
			name.Append(']');
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00027B26 File Offset: 0x00025D26
		private static bool RequiresFullyQualifiedName(TypeReference type, bool top_level)
		{
			return type.Scope != type.Module && (!(type.Scope.Name == "mscorlib") || !top_level);
		}

		// Token: 0x040004F3 RID: 1267
		private readonly string fullname;

		// Token: 0x040004F4 RID: 1268
		private readonly int length;

		// Token: 0x040004F5 RID: 1269
		private int position;

		// Token: 0x0200017B RID: 379
		private class Type
		{
			// Token: 0x040004F6 RID: 1270
			public const int Ptr = -1;

			// Token: 0x040004F7 RID: 1271
			public const int ByRef = -2;

			// Token: 0x040004F8 RID: 1272
			public const int SzArray = -3;

			// Token: 0x040004F9 RID: 1273
			public string type_fullname;

			// Token: 0x040004FA RID: 1274
			public string[] nested_names;

			// Token: 0x040004FB RID: 1275
			public int arity;

			// Token: 0x040004FC RID: 1276
			public int[] specs;

			// Token: 0x040004FD RID: 1277
			public TypeParser.Type[] generic_arguments;

			// Token: 0x040004FE RID: 1278
			public string assembly;
		}
	}
}
