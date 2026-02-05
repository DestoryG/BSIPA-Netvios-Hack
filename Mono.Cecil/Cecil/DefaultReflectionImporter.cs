using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000075 RID: 117
	public class DefaultReflectionImporter : IReflectionImporter
	{
		// Token: 0x06000497 RID: 1175 RVA: 0x000120EC File Offset: 0x000102EC
		public DefaultReflectionImporter(ModuleDefinition module)
		{
			Mixin.CheckModule(module);
			this.module = module;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00012101 File Offset: 0x00010301
		private TypeReference ImportType(Type type, ImportGenericContext context)
		{
			return this.ImportType(type, context, DefaultReflectionImporter.ImportGenericKind.Open);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001210C File Offset: 0x0001030C
		private TypeReference ImportType(Type type, ImportGenericContext context, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			if (DefaultReflectionImporter.IsTypeSpecification(type) || DefaultReflectionImporter.ImportOpenGenericType(type, import_kind))
			{
				return this.ImportTypeSpecification(type, context);
			}
			TypeReference typeReference = new TypeReference(string.Empty, type.Name, this.module, this.ImportScope(type), type.IsValueType());
			typeReference.etype = DefaultReflectionImporter.ImportElementType(type);
			if (DefaultReflectionImporter.IsNestedType(type))
			{
				typeReference.DeclaringType = this.ImportType(type.DeclaringType, context, import_kind);
			}
			else
			{
				typeReference.Namespace = type.Namespace ?? string.Empty;
			}
			if (type.IsGenericType())
			{
				DefaultReflectionImporter.ImportGenericParameters(typeReference, type.GetGenericArguments());
			}
			return typeReference;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000121AB File Offset: 0x000103AB
		protected virtual IMetadataScope ImportScope(Type type)
		{
			return this.ImportScope(type.Assembly());
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000121B9 File Offset: 0x000103B9
		private static bool ImportOpenGenericType(Type type, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return type.IsGenericType() && type.IsGenericTypeDefinition() && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000121D1 File Offset: 0x000103D1
		private static bool ImportOpenGenericMethod(MethodBase method, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return method.IsGenericMethod && method.IsGenericMethodDefinition && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000121E9 File Offset: 0x000103E9
		private static bool IsNestedType(Type type)
		{
			return type.IsNested;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000121F4 File Offset: 0x000103F4
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
			if (type.IsGenericType())
			{
				return this.ImportGenericInstance(type, context);
			}
			if (type.IsGenericParameter)
			{
				return DefaultReflectionImporter.ImportGenericParameter(type, context);
			}
			throw new NotSupportedException(type.FullName);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00012284 File Offset: 0x00010484
		private static TypeReference ImportGenericParameter(Type type, ImportGenericContext context)
		{
			if (context.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			if (type.DeclaringMethod() != null)
			{
				return context.MethodParameter(DefaultReflectionImporter.NormalizeMethodName(type.DeclaringMethod()), type.GenericParameterPosition);
			}
			if (type.DeclaringType != null)
			{
				return context.TypeParameter(DefaultReflectionImporter.NormalizeTypeFullName(type.DeclaringType), type.GenericParameterPosition);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000122F3 File Offset: 0x000104F3
		private static string NormalizeMethodName(MethodBase method)
		{
			return DefaultReflectionImporter.NormalizeTypeFullName(method.DeclaringType) + "." + method.Name;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00012310 File Offset: 0x00010510
		private static string NormalizeTypeFullName(Type type)
		{
			if (DefaultReflectionImporter.IsNestedType(type))
			{
				return DefaultReflectionImporter.NormalizeTypeFullName(type.DeclaringType) + "/" + type.Name;
			}
			return type.FullName;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001233C File Offset: 0x0001053C
		private TypeReference ImportGenericInstance(Type type, ImportGenericContext context)
		{
			TypeReference typeReference = this.ImportType(type.GetGenericTypeDefinition(), context, DefaultReflectionImporter.ImportGenericKind.Definition);
			GenericInstanceType genericInstanceType = new GenericInstanceType(typeReference);
			Type[] genericArguments = type.GetGenericArguments();
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

		// Token: 0x060004A3 RID: 1187 RVA: 0x000123B8 File Offset: 0x000105B8
		private static bool IsTypeSpecification(Type type)
		{
			return type.HasElementType || DefaultReflectionImporter.IsGenericInstance(type) || type.IsGenericParameter;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000123D2 File Offset: 0x000105D2
		private static bool IsGenericInstance(Type type)
		{
			return type.IsGenericType() && !type.IsGenericTypeDefinition();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000123E8 File Offset: 0x000105E8
		private static ElementType ImportElementType(Type type)
		{
			ElementType elementType;
			if (!DefaultReflectionImporter.type_etype_mapping.TryGetValue(type, out elementType))
			{
				return ElementType.None;
			}
			return elementType;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00012407 File Offset: 0x00010607
		protected AssemblyNameReference ImportScope(Assembly assembly)
		{
			return this.ImportReference(assembly.GetName());
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00012418 File Offset: 0x00010618
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

		// Token: 0x060004A8 RID: 1192 RVA: 0x00012488 File Offset: 0x00010688
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

		// Token: 0x060004A9 RID: 1193 RVA: 0x000124D8 File Offset: 0x000106D8
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

		// Token: 0x060004AA RID: 1194 RVA: 0x00012558 File Offset: 0x00010758
		private static FieldInfo ResolveFieldDefinition(FieldInfo field)
		{
			return field.Module.ResolveField(field.MetadataToken);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001256B File Offset: 0x0001076B
		private static MethodBase ResolveMethodDefinition(MethodBase method)
		{
			return method.Module.ResolveMethod(method.MetadataToken);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00012580 File Offset: 0x00010780
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

		// Token: 0x060004AD RID: 1197 RVA: 0x000126E4 File Offset: 0x000108E4
		private static void ImportGenericParameters(IGenericParameterProvider provider, Type[] arguments)
		{
			Collection<GenericParameter> genericParameters = provider.GenericParameters;
			for (int i = 0; i < arguments.Length; i++)
			{
				genericParameters.Add(new GenericParameter(arguments[i].Name, provider));
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001271A File Offset: 0x0001091A
		private static bool IsMethodSpecification(MethodBase method)
		{
			return method.IsGenericMethod && !method.IsGenericMethodDefinition;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00012730 File Offset: 0x00010930
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

		// Token: 0x060004B0 RID: 1200 RVA: 0x000127C4 File Offset: 0x000109C4
		private static bool HasCallingConvention(MethodBase method, CallingConventions conventions)
		{
			return (method.CallingConvention & conventions) > (CallingConventions)0;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000127D1 File Offset: 0x000109D1
		public virtual TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			return this.ImportType(type, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000127ED File Offset: 0x000109ED
		public virtual FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			return this.ImportField(field, ImportGenericContext.For(context));
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00012802 File Offset: 0x00010A02
		public virtual MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			return this.ImportMethod(method, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x040000E3 RID: 227
		protected readonly ModuleDefinition module;

		// Token: 0x040000E4 RID: 228
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

		// Token: 0x02000143 RID: 323
		private enum ImportGenericKind
		{
			// Token: 0x04000721 RID: 1825
			Definition,
			// Token: 0x04000722 RID: 1826
			Open
		}
	}
}
