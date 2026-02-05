using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000042 RID: 66
	internal class StructReturnBuffer
	{
		// Token: 0x0600014B RID: 331 RVA: 0x000091E0 File Offset: 0x000073E0
		private static int SizeOf(Type type)
		{
			int num;
			if (StructReturnBuffer.sizes.TryGetValue(type, out num))
			{
				return num;
			}
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("SizeOfType", typeof(int), new Type[0]);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Sizeof, type);
			ilgenerator.Emit(OpCodes.Ret);
			num = (int)dynamicMethodDefinition.Generate().Invoke(null, null);
			StructReturnBuffer.sizes.Add(type, num);
			return num;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009254 File Offset: 0x00007454
		internal static bool NeedsFix(MethodBase method)
		{
			Type returnedType = AccessTools.GetReturnedType(method);
			if (!AccessTools.IsStruct(returnedType))
			{
				return false;
			}
			if (!AccessTools.IsMonoRuntime && method.IsStatic)
			{
				return false;
			}
			int num = StructReturnBuffer.SizeOf(returnedType);
			return !StructReturnBuffer.specialSizes.Contains(num) && StructReturnBuffer.HasStructReturnBuffer();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000092A0 File Offset: 0x000074A0
		private static bool HasStructReturnBuffer()
		{
			if (!StructReturnBuffer.hasTestResult)
			{
				Sandbox.hasStructReturnBuffer = false;
				new StructReturnBuffer();
				MethodBase methodBase = AccessTools.DeclaredMethod(typeof(Sandbox), "GetStruct", null, null);
				MethodInfo methodInfo = AccessTools.DeclaredMethod(typeof(Sandbox), "GetStructReplacement", null, null);
				Memory.DetourMethod(methodBase, methodInfo);
				new Sandbox().GetStruct(Sandbox.magicValue, Sandbox.magicValue);
				StructReturnBuffer.hasTestResult = true;
			}
			return Sandbox.hasStructReturnBuffer;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00009314 File Offset: 0x00007514
		internal static void ArgumentShifter(List<CodeInstruction> instructions, bool shiftArgZero)
		{
			foreach (CodeInstruction codeInstruction in instructions)
			{
				if (codeInstruction.opcode == OpCodes.Ldarg_3)
				{
					codeInstruction.opcode = OpCodes.Ldarg;
					codeInstruction.operand = 4;
				}
				else if (codeInstruction.opcode == OpCodes.Ldarg_2)
				{
					codeInstruction.opcode = OpCodes.Ldarg_3;
				}
				else if (codeInstruction.opcode == OpCodes.Ldarg_1)
				{
					codeInstruction.opcode = OpCodes.Ldarg_2;
				}
				else if (shiftArgZero && codeInstruction.opcode == OpCodes.Ldarg_0)
				{
					codeInstruction.opcode = OpCodes.Ldarg_1;
				}
				else if (codeInstruction.opcode == OpCodes.Ldarg || codeInstruction.opcode == OpCodes.Ldarg_S || codeInstruction.opcode == OpCodes.Ldarga || codeInstruction.opcode == OpCodes.Ldarga_S || codeInstruction.opcode == OpCodes.Starg || codeInstruction.opcode == OpCodes.Starg_S)
				{
					short num = Convert.ToInt16(codeInstruction.operand);
					if (num > 0 || shiftArgZero)
					{
						codeInstruction.operand = (int)(num + 1);
					}
				}
			}
		}

		// Token: 0x040000D4 RID: 212
		private static readonly Dictionary<Type, int> sizes = new Dictionary<Type, int>();

		// Token: 0x040000D5 RID: 213
		private static readonly HashSet<int> specialSizes = new HashSet<int> { 1, 2, 4, 8 };

		// Token: 0x040000D6 RID: 214
		internal static bool hasTestResult;
	}
}
