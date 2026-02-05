using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BD RID: 189
	internal class TypeParser
	{
		// Token: 0x060007FB RID: 2043 RVA: 0x000183F2 File Offset: 0x000165F2
		private TypeParser(string fullname)
		{
			this.fullname = fullname;
			this.length = fullname.Length;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00018410 File Offset: 0x00016610
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

		// Token: 0x060007FD RID: 2045 RVA: 0x00018474 File Offset: 0x00016674
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

		// Token: 0x060007FE RID: 2046 RVA: 0x000184C4 File Offset: 0x000166C4
		private static bool TryGetArity(string name, out int arity)
		{
			arity = 0;
			int num = name.LastIndexOf('`');
			return num != -1 && TypeParser.ParseInt32(name.Substring(num + 1), out arity);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000184F2 File Offset: 0x000166F2
		private static bool ParseInt32(string value, out int result)
		{
			return int.TryParse(value, out result);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000184FC File Offset: 0x000166FC
		private static void TryAddArity(string name, ref int arity)
		{
			int num;
			if (!TypeParser.TryGetArity(name, out num))
			{
				return;
			}
			arity += num;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001851C File Offset: 0x0001671C
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

		// Token: 0x06000802 RID: 2050 RVA: 0x000185A3 File Offset: 0x000167A3
		private static bool IsDelimiter(char chr)
		{
			return "+,[]*&".IndexOf(chr) != -1;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000185B6 File Offset: 0x000167B6
		private void TryParseWhiteSpace()
		{
			while (this.position < this.length && char.IsWhiteSpace(this.fullname[this.position]))
			{
				this.position++;
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000185F0 File Offset: 0x000167F0
		private string[] ParseNestedNames()
		{
			string[] array = null;
			while (this.TryParse('+'))
			{
				TypeParser.Add<string>(ref array, this.ParsePart());
			}
			return array;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00018619 File Offset: 0x00016819
		private bool TryParse(char chr)
		{
			if (this.position < this.length && this.fullname[this.position] == chr)
			{
				this.position++;
				return true;
			}
			return false;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001864E File Offset: 0x0001684E
		private static void Add<T>(ref T[] array, T item)
		{
			array = array.Add(item);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001865C File Offset: 0x0001685C
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
						c = this.fullname[this.position];
						if (c != '*')
						{
							if (c == ']')
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

		// Token: 0x06000808 RID: 2056 RVA: 0x00018764 File Offset: 0x00016964
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

		// Token: 0x06000809 RID: 2057 RVA: 0x000187EC File Offset: 0x000169EC
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

		// Token: 0x0600080A RID: 2058 RVA: 0x00018864 File Offset: 0x00016A64
		public static TypeReference ParseType(ModuleDefinition module, string fullname, bool typeDefinitionOnly = false)
		{
			if (string.IsNullOrEmpty(fullname))
			{
				return null;
			}
			TypeParser typeParser = new TypeParser(fullname);
			return TypeParser.GetTypeReference(module, typeParser.ParseType(true), typeDefinitionOnly);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00018890 File Offset: 0x00016A90
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

		// Token: 0x0600080C RID: 2060 RVA: 0x000188C4 File Offset: 0x00016AC4
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

		// Token: 0x0600080D RID: 2061 RVA: 0x00018970 File Offset: 0x00016B70
		private static TypeReference TryCreateGenericInstanceType(TypeReference type, TypeParser.Type type_info)
		{
			TypeParser.Type[] generic_arguments = type_info.generic_arguments;
			if (generic_arguments.IsNullOrEmpty<TypeParser.Type>())
			{
				return type;
			}
			GenericInstanceType genericInstanceType = new GenericInstanceType(type);
			Collection<TypeReference> genericArguments = genericInstanceType.GenericArguments;
			for (int i = 0; i < generic_arguments.Length; i++)
			{
				genericArguments.Add(TypeParser.GetTypeReference(type.Module, generic_arguments[i], false));
			}
			return genericInstanceType;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000189C0 File Offset: 0x00016BC0
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

		// Token: 0x0600080F RID: 2063 RVA: 0x000189FC File Offset: 0x00016BFC
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

		// Token: 0x06000810 RID: 2064 RVA: 0x00018A70 File Offset: 0x00016C70
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

		// Token: 0x06000811 RID: 2065 RVA: 0x00018AAC File Offset: 0x00016CAC
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

		// Token: 0x06000812 RID: 2066 RVA: 0x00018AEC File Offset: 0x00016CEC
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

		// Token: 0x06000813 RID: 2067 RVA: 0x00018B4B File Offset: 0x00016D4B
		private static bool TryCurrentModule(ModuleDefinition module, TypeParser.Type type_info)
		{
			return string.IsNullOrEmpty(type_info.assembly) || (module.assembly != null && module.assembly.Name.FullName == type_info.assembly);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00018B84 File Offset: 0x00016D84
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

		// Token: 0x06000815 RID: 2069 RVA: 0x00018BAC File Offset: 0x00016DAC
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

		// Token: 0x06000816 RID: 2070 RVA: 0x00018BF0 File Offset: 0x00016DF0
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

		// Token: 0x06000817 RID: 2071 RVA: 0x00018C84 File Offset: 0x00016E84
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

		// Token: 0x06000818 RID: 2072 RVA: 0x00018CD0 File Offset: 0x00016ED0
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

		// Token: 0x06000819 RID: 2073 RVA: 0x00018E0A File Offset: 0x0001700A
		private static bool RequiresFullyQualifiedName(TypeReference type, bool top_level)
		{
			return type.Scope != type.Module && (!(type.Scope.Name == "mscorlib") || !top_level);
		}

		// Token: 0x040002AA RID: 682
		private readonly string fullname;

		// Token: 0x040002AB RID: 683
		private readonly int length;

		// Token: 0x040002AC RID: 684
		private int position;

		// Token: 0x0200014A RID: 330
		private class Type
		{
			// Token: 0x0400074F RID: 1871
			public const int Ptr = -1;

			// Token: 0x04000750 RID: 1872
			public const int ByRef = -2;

			// Token: 0x04000751 RID: 1873
			public const int SzArray = -3;

			// Token: 0x04000752 RID: 1874
			public string type_fullname;

			// Token: 0x04000753 RID: 1875
			public string[] nested_names;

			// Token: 0x04000754 RID: 1876
			public int arity;

			// Token: 0x04000755 RID: 1877
			public int[] specs;

			// Token: 0x04000756 RID: 1878
			public TypeParser.Type[] generic_arguments;

			// Token: 0x04000757 RID: 1879
			public string assembly;
		}
	}
}
