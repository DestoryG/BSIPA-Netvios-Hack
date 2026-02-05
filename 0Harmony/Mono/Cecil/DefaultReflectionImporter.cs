using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200012B RID: 299
	internal class DefaultReflectionImporter : IReflectionImporter
	{
		// Token: 0x06000829 RID: 2089 RVA: 0x00020908 File Offset: 0x0001EB08
		public DefaultReflectionImporter(ModuleDefinition module)
		{
			Mixin.CheckModule(module);
			this.module = module;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0002091D File Offset: 0x0001EB1D
		private TypeReference ImportType(Type type, ImportGenericContext context)
		{
			return this.ImportType(type, context, DefaultReflectionImporter.ImportGenericKind.Open);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00020928 File Offset: 0x0001EB28
		private TypeReference ImportType(Type type, ImportGenericContext context, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			if (DefaultReflectionImporter.IsTypeSpecification(type) || DefaultReflectionImporter.ImportOpenGenericType(type, import_kind))
			{
				return this.ImportTypeSpecification(type, context);
			}
			TypeReference typeReference = new TypeReference(string.Empty, type.Name, this.module, this.ImportScope(type), type.IsValueType);
			typeReference.etype = DefaultReflectionImporter.ImportElementType(type);
			if (DefaultReflectionImporter.IsNestedType(type))
			{
				typeReference.DeclaringType = this.ImportType(type.DeclaringType, context, import_kind);
			}
			else
			{
				typeReference.Namespace = type.Namespace ?? string.Empty;
			}
			if (type.IsGenericType)
			{
				DefaultReflectionImporter.ImportGenericParameters(typeReference, type.GetGenericArguments());
			}
			return typeReference;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000209C7 File Offset: 0x0001EBC7
		protected virtual IMetadataScope ImportScope(Type type)
		{
			return this.ImportScope(type.Assembly);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000209D5 File Offset: 0x0001EBD5
		private static bool ImportOpenGenericType(Type type, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return type.IsGenericType && type.IsGenericTypeDefinition && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000209ED File Offset: 0x0001EBED
		private static bool ImportOpenGenericMethod(MethodBase method, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return method.IsGenericMethod && method.IsGenericMethodDefinition && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00020A05 File Offset: 0x0001EC05
		private static bool IsNestedType(Type type)
		{
			return type.IsNested;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00020A10 File Offset: 0x0001EC10
		private TypeReference ImportTypeSpecification(Type type, ImportGenericContext context)
		{
			if (type.IsByRef)
			{
				return new ByReferenceType(this.ImportType(type.GetElementType(), context));
			}
			if (type.IsPointer)
			{
				return new PointerType(this.ImportType(type.GetElementType(), context));
			}
			if (type.IsArray)
			{
				return new ArrayType(this.ImportType(type.GetElementType(), context), type.GetArrayRank());
			}
			if (type.IsGenericType)
			{
				return this.ImportGenericInstance(type, context);
			}
			if (type.IsGenericParameter)
			{
				return DefaultReflectionImporter.ImportGenericParameter(type, context);
			}
			throw new NotSupportedException(type.FullName);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00020AA0 File Offset: 0x0001ECA0
		private static TypeReference ImportGenericParameter(Type type, ImportGenericContext context)
		{
			if (context.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			if (type.DeclaringMethod != null)
			{
				return context.MethodParameter(DefaultReflectionImporter.NormalizeMethodName(type.DeclaringMethod), type.GenericParameterPosition);
			}
			if (type.DeclaringType != null)
			{
				return context.TypeParameter(DefaultReflectionImporter.NormalizeTypeFullName(type.DeclaringType), type.GenericParameterPosition);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00020B0F File Offset: 0x0001ED0F
		private static string NormalizeMethodName(MethodBase method)
		{
			return DefaultReflectionImporter.NormalizeTypeFullName(method.DeclaringType) + "." + method.Name;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00020B2C File Offset: 0x0001ED2C
		private static string NormalizeTypeFullName(Type type)
		{
			if (DefaultReflectionImporter.IsNestedType(type))
			{
				return DefaultReflectionImporter.NormalizeTypeFullName(type.DeclaringType) + "/" + type.Name;
			}
			return type.FullName;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00020B58 File Offset: 0x0001ED58
		private TypeReference ImportGenericInstance(Type type, ImportGenericContext context)
		{
			TypeReference typeReference = this.ImportType(type.GetGenericTypeDefinition(), context, DefaultReflectionImporter.ImportGenericKind.Definition);
			Type[] genericArguments = type.GetGenericArguments();
			GenericInstanceType genericInstanceType = new GenericInstanceType(typeReference, genericArguments.Length);
			Collection<TypeReference> genericArguments2 = genericInstanceType.GenericArguments;
			context.Push(typeReference);
			TypeReference typeReference2;
			try
			{
				for (int i = 0; i < genericArguments.Length; i++)
				{
					genericArguments2.Add(this.ImportType(genericArguments[i], context));
				}
				typeReference2 = genericInstanceType;
			}
			finally
			{
				context.Pop();
			}
			return typeReference2;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00020BD8 File Offset: 0x0001EDD8
		private static bool IsTypeSpecification(Type type)
		{
			return type.HasElementType || DefaultReflectionImporter.IsGenericInstance(type) || type.IsGenericParameter;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00020BF2 File Offset: 0x0001EDF2
		private static bool IsGenericInstance(Type type)
		{
			return type.IsGenericType && !type.IsGenericTypeDefinition;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00020C08 File Offset: 0x0001EE08
		private static ElementType ImportElementType(Type type)
		{
			ElementType elementType;
			if (!DefaultReflectionImporter.type_etype_mapping.TryGetValue(type, out elementType))
			{
				return ElementType.None;
			}
			return elementType;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00020C27 File Offset: 0x0001EE27
		protected AssemblyNameReference ImportScope(Assembly assembly)
		{
			return this.ImportReference(assembly.GetName());
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00020C38 File Offset: 0x0001EE38
		public virtual AssemblyNameReference ImportReference(AssemblyName name)
		{
			Mixin.CheckName(name);
			AssemblyNameReference assemblyNameReference;
			if (this.TryGetAssemblyNameReference(name, out assemblyNameReference))
			{
				return assemblyNameReference;
			}
			assemblyNameReference = new AssemblyNameReference(name.Name, name.Version)
			{
				PublicKeyToken = name.GetPublicKeyToken(),
				Culture = name.CultureInfo.Name,
				HashAlgorithm = (AssemblyHashAlgorithm)name.HashAlgorithm
			};
			this.module.AssemblyReferences.Add(assemblyNameReference);
			return assemblyNameReference;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00020CA8 File Offset: 0x0001EEA8
		private bool TryGetAssemblyNameReference(AssemblyName name, out AssemblyNameReference assembly_reference)
		{
			Collection<AssemblyNameReference> assemblyReferences = this.module.AssemblyReferences;
			for (int i = 0; i < assemblyReferences.Count; i++)
			{
				AssemblyNameReference assemblyNameReference = assemblyReferences[i];
				if (!(name.FullName != assemblyNameReference.FullName))
				{
					assembly_reference = assemblyNameReference;
					return true;
				}
			}
			assembly_reference = null;
			return false;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00020CF8 File Offset: 0x0001EEF8
		private FieldReference ImportField(FieldInfo field, ImportGenericContext context)
		{
			TypeReference typeReference = this.ImportType(field.DeclaringType, context);
			if (DefaultReflectionImporter.IsGenericInstance(field.DeclaringType))
			{
				field = DefaultReflectionImporter.ResolveFieldDefinition(field);
			}
			context.Push(typeReference);
			FieldReference fieldReference;
			try
			{
				fieldReference = new FieldReference
				{
					Name = field.Name,
					DeclaringType = typeReference,
					FieldType = this.ImportType(field.FieldType, context)
				};
			}
			finally
			{
				context.Pop();
			}
			return fieldReference;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00020D78 File Offset: 0x0001EF78
		private static FieldInfo ResolveFieldDefinition(FieldInfo field)
		{
			return field.Module.ResolveField(field.MetadataToken);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00020D8B File Offset: 0x0001EF8B
		private static MethodBase ResolveMethodDefinition(MethodBase method)
		{
			return method.Module.ResolveMethod(method.MetadataToken);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00020DA0 File Offset: 0x0001EFA0
		private MethodReference ImportMethod(MethodBase method, ImportGenericContext context, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			if (DefaultReflectionImporter.IsMethodSpecification(method) || DefaultReflectionImporter.ImportOpenGenericMethod(method, import_kind))
			{
				return this.ImportMethodSpecification(method, context);
			}
			TypeReference typeReference = this.ImportType(method.DeclaringType, context);
			if (DefaultReflectionImporter.IsGenericInstance(method.DeclaringType))
			{
				method = DefaultReflectionImporter.ResolveMethodDefinition(method);
			}
			MethodReference methodReference = new MethodReference
			{
				Name = method.Name,
				HasThis = DefaultReflectionImporter.HasCallingConvention(method, CallingConventions.HasThis),
				ExplicitThis = DefaultReflectionImporter.HasCallingConvention(method, CallingConventions.ExplicitThis),
				DeclaringType = this.ImportType(method.DeclaringType, context, DefaultReflectionImporter.ImportGenericKind.Definition)
			};
			if (DefaultReflectionImporter.HasCallingConvention(method, CallingConventions.VarArgs))
			{
				methodReference.CallingConvention &= MethodCallingConvention.VarArg;
			}
			if (method.IsGenericMethod)
			{
				DefaultReflectionImporter.ImportGenericParameters(methodReference, method.GetGenericArguments());
			}
			context.Push(methodReference);
			MethodReference methodReference2;
			try
			{
				MethodInfo methodInfo = method as MethodInfo;
				methodReference.ReturnType = ((methodInfo != null) ? this.ImportType(methodInfo.ReturnType, context) : this.ImportType(typeof(void), default(ImportGenericContext)));
				ParameterInfo[] parameters = method.GetParameters();
				Collection<ParameterDefinition> parameters2 = methodReference.Parameters;
				for (int i = 0; i < parameters.Length; i++)
				{
					parameters2.Add(new ParameterDefinition(this.ImportType(parameters[i].ParameterType, context)));
				}
				methodReference.DeclaringType = typeReference;
				methodReference2 = methodReference;
			}
			finally
			{
				context.Pop();
			}
			return methodReference2;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00020F04 File Offset: 0x0001F104
		private static void ImportGenericParameters(IGenericParameterProvider provider, Type[] arguments)
		{
			Collection<GenericParameter> genericParameters = provider.GenericParameters;
			for (int i = 0; i < arguments.Length; i++)
			{
				genericParameters.Add(new GenericParameter(arguments[i].Name, provider));
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00020F3A File Offset: 0x0001F13A
		private static bool IsMethodSpecification(MethodBase method)
		{
			return method.IsGenericMethod && !method.IsGenericMethodDefinition;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00020F50 File Offset: 0x0001F150
		private MethodReference ImportMethodSpecification(MethodBase method, ImportGenericContext context)
		{
			MethodInfo methodInfo = method as MethodInfo;
			if (methodInfo == null)
			{
				throw new InvalidOperationException();
			}
			MethodReference methodReference = this.ImportMethod(methodInfo.GetGenericMethodDefinition(), context, DefaultReflectionImporter.ImportGenericKind.Definition);
			GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(methodReference);
			Type[] genericArguments = method.GetGenericArguments();
			Collection<TypeReference> genericArguments2 = genericInstanceMethod.GenericArguments;
			context.Push(methodReference);
			MethodReference methodReference2;
			try
			{
				for (int i = 0; i < genericArguments.Length; i++)
				{
					genericArguments2.Add(this.ImportType(genericArguments[i], context));
				}
				methodReference2 = genericInstanceMethod;
			}
			finally
			{
				context.Pop();
			}
			return methodReference2;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		private static bool HasCallingConvention(MethodBase method, CallingConventions conventions)
		{
			return (method.CallingConvention & conventions) > (CallingConventions)0;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00020FF1 File Offset: 0x0001F1F1
		public virtual TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			return this.ImportType(type, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002100D File Offset: 0x0001F20D
		public virtual FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			return this.ImportField(field, ImportGenericContext.For(context));
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00021022 File Offset: 0x0001F222
		public virtual MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			return this.ImportMethod(method, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x040002FA RID: 762
		protected readonly ModuleDefinition module;

		// Token: 0x040002FB RID: 763
		private static readonly Dictionary<Type, ElementType> type_etype_mapping = new Dictionary<Type, ElementType>(18)
		{
			{
				typeof(void),
				ElementType.Void
			},
			{
				typeof(bool),
				ElementType.Boolean
			},
			{
				typeof(char),
				ElementType.Char
			},
			{
				typeof(sbyte),
				ElementType.I1
			},
			{
				typeof(byte),
				ElementType.U1
			},
			{
				typeof(short),
				ElementType.I2
			},
			{
				typeof(ushort),
				ElementType.U2
			},
			{
				typeof(int),
				ElementType.I4
			},
			{
				typeof(uint),
				ElementType.U4
			},
			{
				typeof(long),
				ElementType.I8
			},
			{
				typeof(ulong),
				ElementType.U8
			},
			{
				typeof(float),
				ElementType.R4
			},
			{
				typeof(double),
				ElementType.R8
			},
			{
				typeof(string),
				ElementType.String
			},
			{
				typeof(TypedReference),
				ElementType.TypedByRef
			},
			{
				typeof(IntPtr),
				ElementType.I
			},
			{
				typeof(UIntPtr),
				ElementType.U
			},
			{
				typeof(object),
				ElementType.Object
			}
		};

		// Token: 0x0200012C RID: 300
		private enum ImportGenericKind
		{
			// Token: 0x040002FD RID: 765
			Definition,
			// Token: 0x040002FE RID: 766
			Open
		}
	}
}
