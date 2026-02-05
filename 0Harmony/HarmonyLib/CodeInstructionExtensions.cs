using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200009D RID: 157
	public static class CodeInstructionExtensions
	{
		// Token: 0x060002FA RID: 762 RVA: 0x0000EB7C File Offset: 0x0000CD7C
		public static bool OperandIs(this CodeInstruction code, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (code.operand == null)
			{
				return false;
			}
			Type type = value.GetType();
			Type type2 = code.operand.GetType();
			if (AccessTools.IsInteger(type) && AccessTools.IsNumber(type2))
			{
				return Convert.ToInt64(code.operand) == Convert.ToInt64(value);
			}
			if (AccessTools.IsFloatingPoint(type) && AccessTools.IsNumber(type2))
			{
				return Convert.ToDouble(code.operand) == Convert.ToDouble(value);
			}
			return object.Equals(code.operand, value);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000EC08 File Offset: 0x0000CE08
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool OperandIs(this CodeInstruction code, MemberInfo value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return object.Equals(code.operand, value);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000EC2A File Offset: 0x0000CE2A
		public static bool Is(this CodeInstruction code, OpCode opcode, object operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000EC43 File Offset: 0x0000CE43
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool Is(this CodeInstruction code, OpCode opcode, MemberInfo operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000EC5C File Offset: 0x0000CE5C
		public static bool IsLdarg(this CodeInstruction code, int? n = null)
		{
			return ((n == null || n.Value == 0) && code.opcode == OpCodes.Ldarg_0) || ((n == null || n.Value == 1) && code.opcode == OpCodes.Ldarg_1) || ((n == null || n.Value == 2) && code.opcode == OpCodes.Ldarg_2) || ((n == null || n.Value == 3) && code.opcode == OpCodes.Ldarg_3) || (code.opcode == OpCodes.Ldarg && (n == null || n.Value == Convert.ToInt32(code.operand))) || (code.opcode == OpCodes.Ldarg_S && (n == null || n.Value == Convert.ToInt32(code.operand)));
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000ED68 File Offset: 0x0000CF68
		public static bool IsLdarga(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Ldarga) || !(code.opcode != OpCodes.Ldarga_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		public static bool IsStarg(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Starg) || !(code.opcode != OpCodes.Starg_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000EE0E File Offset: 0x0000D00E
		public static bool IsLdloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return CodeInstructionExtensions.loadVarCodes.Contains(code.opcode) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000EE35 File Offset: 0x0000D035
		public static bool IsStloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return CodeInstructionExtensions.storeVarCodes.Contains(code.opcode) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000EE5C File Offset: 0x0000D05C
		public static bool Branches(this CodeInstruction code, out Label? label)
		{
			if (CodeInstructionExtensions.branchCodes.Contains(code.opcode))
			{
				label = new Label?((Label)code.operand);
				return true;
			}
			label = null;
			return false;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000EE90 File Offset: 0x0000D090
		public static bool Calls(this CodeInstruction code, MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			return (!(code.opcode != OpCodes.Call) || !(code.opcode != OpCodes.Callvirt)) && object.Equals(code.operand, method);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000EEE3 File Offset: 0x0000D0E3
		public static bool LoadsConstant(this CodeInstruction code)
		{
			return CodeInstructionExtensions.constantLoadingCodes.Contains(code.opcode);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000EEF8 File Offset: 0x0000D0F8
		public static bool LoadsConstant(this CodeInstruction code, long number)
		{
			OpCode opcode = code.opcode;
			return (number == -1L && opcode == OpCodes.Ldc_I4_M1) || (number == 0L && opcode == OpCodes.Ldc_I4_0) || (number == 1L && opcode == OpCodes.Ldc_I4_1) || (number == 2L && opcode == OpCodes.Ldc_I4_2) || (number == 3L && opcode == OpCodes.Ldc_I4_3) || (number == 4L && opcode == OpCodes.Ldc_I4_4) || (number == 5L && opcode == OpCodes.Ldc_I4_5) || (number == 6L && opcode == OpCodes.Ldc_I4_6) || (number == 7L && opcode == OpCodes.Ldc_I4_7) || (number == 8L && opcode == OpCodes.Ldc_I4_8) || ((!(opcode != OpCodes.Ldc_I4) || !(opcode != OpCodes.Ldc_I4_S) || !(opcode != OpCodes.Ldc_I8)) && Convert.ToInt64(code.operand) == number);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000F009 File Offset: 0x0000D209
		public static bool LoadsConstant(this CodeInstruction code, double number)
		{
			return (!(code.opcode != OpCodes.Ldc_R4) || !(code.opcode != OpCodes.Ldc_R8)) && Convert.ToDouble(code.operand) == number;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000F03F File Offset: 0x0000D23F
		public static bool LoadsConstant(this CodeInstruction code, Enum e)
		{
			return code.LoadsConstant(Convert.ToInt64(e));
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000F050 File Offset: 0x0000D250
		public static bool LoadsField(this CodeInstruction code, FieldInfo field, bool byAddress = false)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			OpCode opCode = (field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld);
			if (!byAddress && code.opcode == opCode && object.Equals(code.operand, field))
			{
				return true;
			}
			OpCode opCode2 = (field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda);
			return byAddress && code.opcode == opCode2 && object.Equals(code.operand, field);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
		public static bool StoresField(this CodeInstruction code, FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			OpCode opCode = (field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld);
			return code.opcode == opCode && object.Equals(code.operand, field);
		}

		// Token: 0x040001BF RID: 447
		private static readonly HashSet<OpCode> loadVarCodes = new HashSet<OpCode>
		{
			OpCodes.Ldloc_0,
			OpCodes.Ldloc_1,
			OpCodes.Ldloc_2,
			OpCodes.Ldloc_3,
			OpCodes.Ldloc,
			OpCodes.Ldloca,
			OpCodes.Ldloc_S,
			OpCodes.Ldloca_S
		};

		// Token: 0x040001C0 RID: 448
		private static readonly HashSet<OpCode> storeVarCodes = new HashSet<OpCode>
		{
			OpCodes.Stloc_0,
			OpCodes.Stloc_1,
			OpCodes.Stloc_2,
			OpCodes.Stloc_3,
			OpCodes.Stloc,
			OpCodes.Stloc_S
		};

		// Token: 0x040001C1 RID: 449
		private static readonly HashSet<OpCode> branchCodes = new HashSet<OpCode>
		{
			OpCodes.Br_S,
			OpCodes.Brfalse_S,
			OpCodes.Brtrue_S,
			OpCodes.Beq_S,
			OpCodes.Bge_S,
			OpCodes.Bgt_S,
			OpCodes.Ble_S,
			OpCodes.Blt_S,
			OpCodes.Bne_Un_S,
			OpCodes.Bge_Un_S,
			OpCodes.Bgt_Un_S,
			OpCodes.Ble_Un_S,
			OpCodes.Blt_Un_S,
			OpCodes.Br,
			OpCodes.Brfalse,
			OpCodes.Brtrue,
			OpCodes.Beq,
			OpCodes.Bge,
			OpCodes.Bgt,
			OpCodes.Ble,
			OpCodes.Blt,
			OpCodes.Bne_Un,
			OpCodes.Bge_Un,
			OpCodes.Bgt_Un,
			OpCodes.Ble_Un,
			OpCodes.Blt_Un
		};

		// Token: 0x040001C2 RID: 450
		private static readonly HashSet<OpCode> constantLoadingCodes = new HashSet<OpCode>
		{
			OpCodes.Ldc_I4_M1,
			OpCodes.Ldc_I4_0,
			OpCodes.Ldc_I4_1,
			OpCodes.Ldc_I4_2,
			OpCodes.Ldc_I4_3,
			OpCodes.Ldc_I4_4,
			OpCodes.Ldc_I4_5,
			OpCodes.Ldc_I4_6,
			OpCodes.Ldc_I4_7,
			OpCodes.Ldc_I4_8,
			OpCodes.Ldc_I4,
			OpCodes.Ldc_I4_S,
			OpCodes.Ldc_I8,
			OpCodes.Ldc_R4,
			OpCodes.Ldc_R8
		};
	}
}
