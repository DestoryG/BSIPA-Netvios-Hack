using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000332 RID: 818
	internal sealed class MMReflectionImporter : IReflectionImporter
	{
		// Token: 0x060012D7 RID: 4823 RVA: 0x00043C28 File Offset: 0x00041E28
		public MMReflectionImporter(ModuleDefinition module)
		{
			this.Module = module;
			this.Default = new DefaultReflectionImporter(module);
			this.ElementTypes = new Dictionary<Type, TypeReference>
			{
				{
					typeof(void),
					module.TypeSystem.Void
				},
				{
					typeof(bool),
					module.TypeSystem.Boolean
				},
				{
					typeof(char),
					module.TypeSystem.Char
				},
				{
					typeof(sbyte),
					module.TypeSystem.SByte
				},
				{
					typeof(byte),
					module.TypeSystem.Byte
				},
				{
					typeof(short),
					module.TypeSystem.Int16
				},
				{
					typeof(ushort),
					module.TypeSystem.UInt16
				},
				{
					typeof(int),
					module.TypeSystem.Int32
				},
				{
					typeof(uint),
					module.TypeSystem.UInt32
				},
				{
					typeof(long),
					module.TypeSystem.Int64
				},
				{
					typeof(ulong),
					module.TypeSystem.UInt64
				},
				{
					typeof(float),
					module.TypeSystem.Single
				},
				{
					typeof(double),
					module.TypeSystem.Double
				},
				{
					typeof(string),
					module.TypeSystem.String
				},
				{
					typeof(TypedReference),
					module.TypeSystem.TypedReference
				},
				{
					typeof(IntPtr),
					module.TypeSystem.IntPtr
				},
				{
					typeof(UIntPtr),
					module.TypeSystem.UIntPtr
				},
				{
					typeof(object),
					module.TypeSystem.Object
				}
			};
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00043E78 File Offset: 0x00042078
		public AssemblyNameReference ImportReference(AssemblyName asm)
		{
			AssemblyNameReference assemblyNameReference;
			if (this.CachedAsms.TryGetValue(asm, out assemblyNameReference))
			{
				return assemblyNameReference;
			}
			return this.CachedAsms[asm] = this.Default.ImportReference(asm);
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00043EB4 File Offset: 0x000420B4
		public TypeReference ImportModuleType(Module module, IGenericParameterProvider context)
		{
			TypeReference typeReference;
			if (this.CachedModuleTypes.TryGetValue(module, out typeReference))
			{
				return typeReference;
			}
			return this.CachedModuleTypes[module] = new TypeReference(string.Empty, "<Module>", this.Module, this.ImportReference(module.Assembly.GetName()));
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00043F08 File Offset: 0x00042108
		public TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			TypeReference typeReference;
			if (this.CachedTypes.TryGetValue(type, out typeReference))
			{
				return typeReference;
			}
			if (this.UseDefault)
			{
				return this.CachedTypes[type] = this.Default.ImportReference(type, context);
			}
			if (type.HasElementType)
			{
				if (type.IsByRef)
				{
					return this.CachedTypes[type] = new ByReferenceType(this.ImportReference(type.GetElementType(), context));
				}
				if (type.IsPointer)
				{
					return this.CachedTypes[type] = new PointerType(this.ImportReference(type.GetElementType(), context));
				}
				if (type.IsArray)
				{
					ArrayType arrayType = new ArrayType(this.ImportReference(type.GetElementType(), context), type.GetArrayRank());
					if (type != type.GetElementType().MakeArrayType())
					{
						for (int i = 0; i < arrayType.Rank; i++)
						{
							arrayType.Dimensions[i] = new ArrayDimension(new int?(0), null);
						}
					}
					return this.CachedTypes[type] = arrayType;
				}
			}
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				GenericInstanceType genericInstanceType = new GenericInstanceType(this.ImportReference(type.GetGenericTypeDefinition(), context));
				foreach (Type type2 in type.GetGenericArguments())
				{
					genericInstanceType.GenericArguments.Add(this.ImportReference(type2, context));
				}
				return genericInstanceType;
			}
			if (type.IsGenericParameter)
			{
				return this.CachedTypes[type] = MMReflectionImporter.ImportGenericParameter(type, context);
			}
			if (this.ElementTypes.TryGetValue(type, out typeReference))
			{
				return this.CachedTypes[type] = typeReference;
			}
			typeReference = new TypeReference(string.Empty, type.Name, this.Module, this.ImportReference(type.Assembly.GetName()), type.IsValueType);
			if (type.IsNested)
			{
				typeReference.DeclaringType = this.ImportReference(type.DeclaringType, context);
			}
			else if (type.Namespace != null)
			{
				typeReference.Namespace = type.Namespace;
			}
			if (type.IsGenericType)
			{
				foreach (Type type3 in type.GetGenericArguments())
				{
					typeReference.GenericParameters.Add(new GenericParameter(type3.Name, typeReference));
				}
			}
			return this.CachedTypes[type] = typeReference;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00044174 File Offset: 0x00042374
		private static TypeReference ImportGenericParameter(Type type, IGenericParameterProvider context)
		{
			MethodReference methodReference = context as MethodReference;
			if (methodReference != null)
			{
				if (type.DeclaringMethod != null)
				{
					return methodReference.GenericParameters[type.GenericParameterPosition];
				}
				context = methodReference.DeclaringType;
			}
			Type declaringType = type.DeclaringType;
			if (declaringType == null)
			{
				throw new InvalidOperationException();
			}
			TypeReference typeReference = context as TypeReference;
			if (typeReference != null)
			{
				while (typeReference != null)
				{
					TypeReference elementType = typeReference.GetElementType();
					if (elementType.Is(declaringType))
					{
						return elementType.GenericParameters[type.GenericParameterPosition];
					}
					if (typeReference.Is(declaringType))
					{
						return typeReference.GenericParameters[type.GenericParameterPosition];
					}
					typeReference = typeReference.DeclaringType;
				}
			}
			throw new NotSupportedException();
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00044224 File Offset: 0x00042424
		public FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			FieldReference fieldReference;
			if (this.CachedFields.TryGetValue(field, out fieldReference))
			{
				return fieldReference;
			}
			if (this.UseDefault)
			{
				return this.CachedFields[field] = this.Default.ImportReference(field, context);
			}
			Type declaringType = field.DeclaringType;
			TypeReference typeReference = ((declaringType != null) ? this.ImportReference(declaringType, context) : this.ImportModuleType(field.Module, context));
			FieldInfo fieldInfo = field;
			if (declaringType != null && declaringType.IsGenericType)
			{
				field = field.Module.ResolveField(field.MetadataToken);
			}
			return this.CachedFields[fieldInfo] = new FieldReference(field.Name, this.ImportReference(field.FieldType, typeReference), typeReference);
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000442E4 File Offset: 0x000424E4
		public MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			MethodReference methodReference;
			if (this.CachedMethods.TryGetValue(method, out methodReference))
			{
				return methodReference;
			}
			DynamicMethod dynamicMethod = method as DynamicMethod;
			if (dynamicMethod != null)
			{
				return new DynamicMethodReference(this.Module, dynamicMethod);
			}
			if (this.UseDefault)
			{
				return this.CachedMethods[method] = this.Default.ImportReference(method, context);
			}
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{
				GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(this.ImportReference((method as MethodInfo).GetGenericMethodDefinition(), context));
				foreach (Type type in method.GetGenericArguments())
				{
					genericInstanceMethod.GenericArguments.Add(this.ImportReference(type, context));
				}
				return this.CachedMethods[method] = genericInstanceMethod;
			}
			Type declaringType = method.DeclaringType;
			methodReference = new MethodReference(method.Name, this.ImportReference(typeof(void), context), (declaringType != null) ? this.ImportReference(declaringType, context) : this.ImportModuleType(method.Module, context));
			methodReference.HasThis = (method.CallingConvention & CallingConventions.HasThis) > (CallingConventions)0;
			methodReference.ExplicitThis = (method.CallingConvention & CallingConventions.ExplicitThis) > (CallingConventions)0;
			if ((method.CallingConvention & CallingConventions.VarArgs) != (CallingConventions)0)
			{
				methodReference.CallingConvention = MethodCallingConvention.VarArg;
			}
			MethodBase methodBase = method;
			if (declaringType != null && declaringType.IsGenericType)
			{
				method = method.Module.ResolveMethod(method.MetadataToken);
			}
			if (method.IsGenericMethodDefinition)
			{
				foreach (Type type2 in method.GetGenericArguments())
				{
					methodReference.GenericParameters.Add(new GenericParameter(type2.Name, methodReference));
				}
			}
			MethodReference methodReference2 = methodReference;
			MethodInfo methodInfo = method as MethodInfo;
			methodReference2.ReturnType = this.ImportReference(((methodInfo != null) ? methodInfo.ReturnType : null) ?? typeof(void), methodReference);
			foreach (ParameterInfo parameterInfo in method.GetParameters())
			{
				methodReference.Parameters.Add(new ParameterDefinition(parameterInfo.Name, (Mono.Cecil.ParameterAttributes)parameterInfo.Attributes, this.ImportReference(parameterInfo.ParameterType, methodReference)));
			}
			return this.CachedMethods[methodBase] = methodReference;
		}

		// Token: 0x04000F83 RID: 3971
		public static readonly IReflectionImporterProvider Provider = new MMReflectionImporter._Provider();

		// Token: 0x04000F84 RID: 3972
		public static readonly IReflectionImporterProvider ProviderNoDefault = new MMReflectionImporter._Provider
		{
			UseDefault = new bool?(false)
		};

		// Token: 0x04000F85 RID: 3973
		private readonly ModuleDefinition Module;

		// Token: 0x04000F86 RID: 3974
		private readonly DefaultReflectionImporter Default;

		// Token: 0x04000F87 RID: 3975
		private readonly Dictionary<AssemblyName, AssemblyNameReference> CachedAsms = new Dictionary<AssemblyName, AssemblyNameReference>();

		// Token: 0x04000F88 RID: 3976
		private readonly Dictionary<Module, TypeReference> CachedModuleTypes = new Dictionary<Module, TypeReference>();

		// Token: 0x04000F89 RID: 3977
		private readonly Dictionary<Type, TypeReference> CachedTypes = new Dictionary<Type, TypeReference>();

		// Token: 0x04000F8A RID: 3978
		private readonly Dictionary<FieldInfo, FieldReference> CachedFields = new Dictionary<FieldInfo, FieldReference>();

		// Token: 0x04000F8B RID: 3979
		private readonly Dictionary<MethodBase, MethodReference> CachedMethods = new Dictionary<MethodBase, MethodReference>();

		// Token: 0x04000F8C RID: 3980
		public bool UseDefault;

		// Token: 0x04000F8D RID: 3981
		private readonly Dictionary<Type, TypeReference> ElementTypes;

		// Token: 0x02000333 RID: 819
		private class _Provider : IReflectionImporterProvider
		{
			// Token: 0x060012DF RID: 4831 RVA: 0x00044548 File Offset: 0x00042748
			public IReflectionImporter GetReflectionImporter(ModuleDefinition module)
			{
				MMReflectionImporter mmreflectionImporter = new MMReflectionImporter(module);
				if (this.UseDefault != null)
				{
					mmreflectionImporter.UseDefault = this.UseDefault.Value;
				}
				return mmreflectionImporter;
			}

			// Token: 0x04000F8E RID: 3982
			public bool? UseDefault;
		}
	}
}
