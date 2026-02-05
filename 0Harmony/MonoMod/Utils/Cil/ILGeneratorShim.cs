using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoMod.Utils.Cil
{
	// Token: 0x02000344 RID: 836
	internal abstract class ILGeneratorShim
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001359 RID: 4953
		public abstract int ILOffset { get; }

		// Token: 0x0600135A RID: 4954
		public abstract void BeginCatchBlock(Type exceptionType);

		// Token: 0x0600135B RID: 4955
		public abstract void BeginExceptFilterBlock();

		// Token: 0x0600135C RID: 4956
		public abstract Label BeginExceptionBlock();

		// Token: 0x0600135D RID: 4957
		public abstract void BeginFaultBlock();

		// Token: 0x0600135E RID: 4958
		public abstract void BeginFinallyBlock();

		// Token: 0x0600135F RID: 4959
		public abstract void BeginScope();

		// Token: 0x06001360 RID: 4960
		public abstract LocalBuilder DeclareLocal(Type localType);

		// Token: 0x06001361 RID: 4961
		public abstract LocalBuilder DeclareLocal(Type localType, bool pinned);

		// Token: 0x06001362 RID: 4962
		public abstract Label DefineLabel();

		// Token: 0x06001363 RID: 4963
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode);

		// Token: 0x06001364 RID: 4964
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, byte arg);

		// Token: 0x06001365 RID: 4965
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, double arg);

		// Token: 0x06001366 RID: 4966
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, short arg);

		// Token: 0x06001367 RID: 4967
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, int arg);

		// Token: 0x06001368 RID: 4968
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, long arg);

		// Token: 0x06001369 RID: 4969
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, ConstructorInfo con);

		// Token: 0x0600136A RID: 4970
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, Label label);

		// Token: 0x0600136B RID: 4971
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, Label[] labels);

		// Token: 0x0600136C RID: 4972
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, LocalBuilder local);

		// Token: 0x0600136D RID: 4973
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, SignatureHelper signature);

		// Token: 0x0600136E RID: 4974
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, FieldInfo field);

		// Token: 0x0600136F RID: 4975
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, MethodInfo meth);

		// Token: 0x06001370 RID: 4976
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, sbyte arg);

		// Token: 0x06001371 RID: 4977
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, float arg);

		// Token: 0x06001372 RID: 4978
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, string str);

		// Token: 0x06001373 RID: 4979
		public abstract void Emit(global::System.Reflection.Emit.OpCode opcode, Type cls);

		// Token: 0x06001374 RID: 4980
		public abstract void EmitCall(global::System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes);

		// Token: 0x06001375 RID: 4981
		public abstract void EmitCalli(global::System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes);

		// Token: 0x06001376 RID: 4982
		public abstract void EmitCalli(global::System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes);

		// Token: 0x06001377 RID: 4983
		public abstract void EmitWriteLine(LocalBuilder localBuilder);

		// Token: 0x06001378 RID: 4984
		public abstract void EmitWriteLine(FieldInfo fld);

		// Token: 0x06001379 RID: 4985
		public abstract void EmitWriteLine(string value);

		// Token: 0x0600137A RID: 4986
		public abstract void EndExceptionBlock();

		// Token: 0x0600137B RID: 4987
		public abstract void EndScope();

		// Token: 0x0600137C RID: 4988
		public abstract void MarkLabel(Label loc);

		// Token: 0x0600137D RID: 4989
		public abstract void ThrowException(Type excType);

		// Token: 0x0600137E RID: 4990
		public abstract void UsingNamespace(string usingNamespace);

		// Token: 0x0600137F RID: 4991 RVA: 0x00046B24 File Offset: 0x00044D24
		public ILGenerator GetProxy()
		{
			return (ILGenerator)ILGeneratorShim.ILGeneratorBuilder.GenerateProxy().MakeGenericType(new Type[] { base.GetType() }).GetConstructors()[0].Invoke(new object[] { this });
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00046B5A File Offset: 0x00044D5A
		public static Type GetProxyType<TShim>() where TShim : ILGeneratorShim
		{
			return ILGeneratorShim.GetProxyType(typeof(TShim));
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00046B6B File Offset: 0x00044D6B
		public static Type GetProxyType(Type tShim)
		{
			return ILGeneratorShim.ProxyType.MakeGenericType(new Type[] { tShim });
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00046B81 File Offset: 0x00044D81
		public static Type ProxyType
		{
			get
			{
				return ILGeneratorShim.ILGeneratorBuilder.GenerateProxy();
			}
		}

		// Token: 0x02000345 RID: 837
		internal static class ILGeneratorBuilder
		{
			// Token: 0x06001384 RID: 4996 RVA: 0x00046B88 File Offset: 0x00044D88
			public static Type GenerateProxy()
			{
				if (ILGeneratorShim.ILGeneratorBuilder.ProxyType != null)
				{
					return ILGeneratorShim.ILGeneratorBuilder.ProxyType;
				}
				Type typeFromHandle = typeof(ILGenerator);
				Type typeFromHandle2 = typeof(ILGeneratorShim);
				Assembly assembly;
				using (ModuleDefinition moduleDefinition = ModuleDefinition.CreateModule("MonoMod.Utils.Cil.ILGeneratorProxy", new ModuleParameters
				{
					Kind = ModuleKind.Dll
				}))
				{
					CustomAttribute customAttribute = new CustomAttribute(moduleDefinition.ImportReference(DynamicMethodDefinition.c_IgnoresAccessChecksToAttribute));
					customAttribute.ConstructorArguments.Add(new CustomAttributeArgument(moduleDefinition.TypeSystem.String, typeof(ILGeneratorShim).Assembly.GetName().Name));
					moduleDefinition.Assembly.CustomAttributes.Add(customAttribute);
					TypeDefinition typeDefinition = new TypeDefinition("MonoMod.Utils.Cil", "ILGeneratorProxy", Mono.Cecil.TypeAttributes.Public)
					{
						BaseType = moduleDefinition.ImportReference(typeFromHandle)
					};
					moduleDefinition.Types.Add(typeDefinition);
					TypeReference typeReference = moduleDefinition.ImportReference(typeFromHandle2);
					GenericParameter genericParameter = new GenericParameter("TTarget", typeDefinition);
					genericParameter.Constraints.Add(new GenericParameterConstraint(typeReference));
					typeDefinition.GenericParameters.Add(genericParameter);
					FieldDefinition fieldDefinition = new FieldDefinition("Target", Mono.Cecil.FieldAttributes.Public, genericParameter);
					typeDefinition.Fields.Add(fieldDefinition);
					MethodDefinition methodDefinition = new MethodDefinition(".ctor", Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.HideBySig | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName, moduleDefinition.TypeSystem.Void);
					methodDefinition.Parameters.Add(new ParameterDefinition(genericParameter));
					typeDefinition.Methods.Add(methodDefinition);
					ILProcessor ilprocessor = methodDefinition.Body.GetILProcessor();
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_1);
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Stfld, fieldDefinition);
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);
					foreach (MethodInfo methodInfo in typeFromHandle.GetMethods(BindingFlags.Instance | BindingFlags.Public))
					{
						MethodInfo method = typeFromHandle2.GetMethod(methodInfo.Name, (from p in methodInfo.GetParameters()
							select p.ParameterType).ToArray<Type>());
						if (!(method == null))
						{
							MethodDefinition methodDefinition2 = new MethodDefinition(methodInfo.Name, Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Virtual | Mono.Cecil.MethodAttributes.HideBySig, moduleDefinition.ImportReference(methodInfo.ReturnType))
							{
								HasThis = true
							};
							foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
							{
								methodDefinition2.Parameters.Add(new ParameterDefinition(moduleDefinition.ImportReference(parameterInfo.ParameterType)));
							}
							typeDefinition.Methods.Add(methodDefinition2);
							ilprocessor = methodDefinition2.Body.GetILProcessor();
							ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
							ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldfld, fieldDefinition);
							foreach (ParameterDefinition parameterDefinition in methodDefinition2.Parameters)
							{
								ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg, parameterDefinition);
							}
							ilprocessor.Emit(method.IsVirtual ? Mono.Cecil.Cil.OpCodes.Callvirt : Mono.Cecil.Cil.OpCodes.Call, ilprocessor.Body.Method.Module.ImportReference(method));
							ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);
						}
					}
					assembly = ReflectionHelper.Load(moduleDefinition);
					assembly.SetMonoCorlibInternal(true);
				}
				return ILGeneratorShim.ILGeneratorBuilder.ProxyType = assembly.GetType("MonoMod.Utils.Cil.ILGeneratorProxy");
			}

			// Token: 0x04000FD5 RID: 4053
			public const string Namespace = "MonoMod.Utils.Cil";

			// Token: 0x04000FD6 RID: 4054
			public const string Name = "ILGeneratorProxy";

			// Token: 0x04000FD7 RID: 4055
			public const string FullName = "MonoMod.Utils.Cil.ILGeneratorProxy";

			// Token: 0x04000FD8 RID: 4056
			public const string TargetName = "Target";

			// Token: 0x04000FD9 RID: 4057
			private static Type ProxyType;
		}
	}
}
