using System;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000324 RID: 804
	internal sealed class DMDEmitDynamicMethodGenerator : DMDGenerator<DMDEmitDynamicMethodGenerator>
	{
		// Token: 0x06001256 RID: 4694 RVA: 0x0003EAF0 File Offset: 0x0003CCF0
		protected override MethodInfo _Generate(DynamicMethodDefinition dmd, object context)
		{
			MethodBase originalMethod = dmd.OriginalMethod;
			MethodDefinition definition = dmd.Definition;
			Type[] array;
			if (originalMethod != null)
			{
				ParameterInfo[] parameters = originalMethod.GetParameters();
				int num = 0;
				if (!originalMethod.IsStatic)
				{
					num++;
					array = new Type[parameters.Length + 1];
					array[0] = originalMethod.GetThisParamType();
				}
				else
				{
					array = new Type[parameters.Length];
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i + num] = parameters[i].ParameterType;
				}
			}
			else
			{
				int num2 = 0;
				if (definition.HasThis)
				{
					num2++;
					array = new Type[definition.Parameters.Count + 1];
					Type type = definition.DeclaringType.ResolveReflection();
					if (type.IsValueType)
					{
						type = type.MakeByRefType();
					}
					array[0] = type;
				}
				else
				{
					array = new Type[definition.Parameters.Count];
				}
				for (int j = 0; j < definition.Parameters.Count; j++)
				{
					array[j + num2] = definition.Parameters[j].ParameterType.ResolveReflection();
				}
			}
			string text = "DMD<" + (((originalMethod != null) ? originalMethod.GetID(null, null, true, false, true) : null) ?? definition.GetID(null, null, true, true)) + ">";
			Type typeFromHandle = typeof(void);
			Type[] array2 = array;
			Type type2;
			if ((type2 = ((originalMethod != null) ? originalMethod.DeclaringType : null)) == null)
			{
				type2 = dmd.OwnerType ?? typeof(DynamicMethodDefinition);
			}
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeFromHandle, array2, type2, true);
			FieldInfo dynamicMethod_returnType = DMDEmitDynamicMethodGenerator._DynamicMethod_returnType;
			object obj = dynamicMethod;
			MethodInfo methodInfo = originalMethod as MethodInfo;
			Type type3;
			if ((type3 = ((methodInfo != null) ? methodInfo.ReturnType : null)) == null)
			{
				TypeReference returnType = definition.ReturnType;
				type3 = ((returnType != null) ? returnType.ResolveReflection() : null);
			}
			dynamicMethod_returnType.SetValue(obj, type3);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			_DMDEmit.Generate(dmd, dynamicMethod, ilgenerator);
			return dynamicMethod;
		}

		// Token: 0x04000F4E RID: 3918
		private static readonly FieldInfo _DynamicMethod_returnType = typeof(DynamicMethod).GetField("returnType", BindingFlags.Instance | BindingFlags.NonPublic) ?? typeof(DynamicMethod).GetField("m_returnType", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
