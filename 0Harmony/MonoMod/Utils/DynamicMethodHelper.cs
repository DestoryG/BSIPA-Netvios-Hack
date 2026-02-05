using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoMod.Utils
{
	// Token: 0x02000329 RID: 809
	internal static class DynamicMethodHelper
	{
		// Token: 0x06001275 RID: 4725 RVA: 0x0003FFF8 File Offset: 0x0003E1F8
		public static object GetReference(int id)
		{
			return DynamicMethodHelper.References[id];
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00040005 File Offset: 0x0003E205
		public static void SetReference(int id, object obj)
		{
			DynamicMethodHelper.References[id] = obj;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00040014 File Offset: 0x0003E214
		private static int AddReference(object obj)
		{
			List<object> references = DynamicMethodHelper.References;
			int num;
			lock (references)
			{
				DynamicMethodHelper.References.Add(obj);
				num = DynamicMethodHelper.References.Count - 1;
			}
			return num;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00040068 File Offset: 0x0003E268
		public static void FreeReference(int id)
		{
			DynamicMethodHelper.References[id] = null;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00040078 File Offset: 0x0003E278
		public static DynamicMethod Stub(this DynamicMethod dm)
		{
			ILGenerator ilgenerator = dm.GetILGenerator();
			for (int i = 0; i < 32; i++)
			{
				ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Nop);
			}
			if (dm.ReturnType != typeof(void))
			{
				ilgenerator.DeclareLocal(dm.ReturnType);
				ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldloca_S, 0);
				ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Initobj, dm.ReturnType);
				ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ldloc_0);
			}
			ilgenerator.Emit(global::System.Reflection.Emit.OpCodes.Ret);
			return dm;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000400FC File Offset: 0x0003E2FC
		public static DynamicMethodDefinition Stub(this DynamicMethodDefinition dmd)
		{
			ILProcessor ilprocessor = dmd.GetILProcessor();
			for (int i = 0; i < 32; i++)
			{
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Nop);
			}
			if (dmd.Definition.ReturnType != dmd.Definition.Module.TypeSystem.Void)
			{
				ilprocessor.Body.Variables.Add(new VariableDefinition(dmd.Definition.ReturnType));
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloca_S, 0);
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Initobj, dmd.Definition.ReturnType);
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_0);
			}
			ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);
			return dmd;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000401A4 File Offset: 0x0003E3A4
		public static int EmitReference<T>(this ILGenerator il, T obj)
		{
			Type typeFromHandle = typeof(T);
			int num = DynamicMethodHelper.AddReference(obj);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4, num);
			il.Emit(global::System.Reflection.Emit.OpCodes.Call, DynamicMethodHelper._GetReference);
			if (typeFromHandle.IsValueType)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Unbox_Any, typeFromHandle);
			}
			return num;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000401FC File Offset: 0x0003E3FC
		public static int EmitReference<T>(this ILProcessor il, T obj)
		{
			ModuleDefinition module = il.Body.Method.Module;
			Type typeFromHandle = typeof(T);
			int num = DynamicMethodHelper.AddReference(obj);
			il.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, num);
			il.Emit(Mono.Cecil.Cil.OpCodes.Call, module.ImportReference(DynamicMethodHelper._GetReference));
			if (typeFromHandle.IsValueType)
			{
				il.Emit(Mono.Cecil.Cil.OpCodes.Unbox_Any, module.ImportReference(typeFromHandle));
			}
			return num;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00040270 File Offset: 0x0003E470
		public static int EmitGetReference<T>(this ILGenerator il, int id)
		{
			Type typeFromHandle = typeof(T);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4, id);
			il.Emit(global::System.Reflection.Emit.OpCodes.Call, DynamicMethodHelper._GetReference);
			if (typeFromHandle.IsValueType)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Unbox_Any, typeFromHandle);
			}
			return id;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000402BC File Offset: 0x0003E4BC
		public static int EmitGetReference<T>(this ILProcessor il, int id)
		{
			ModuleDefinition module = il.Body.Method.Module;
			Type typeFromHandle = typeof(T);
			il.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, id);
			il.Emit(Mono.Cecil.Cil.OpCodes.Call, module.ImportReference(DynamicMethodHelper._GetReference));
			if (typeFromHandle.IsValueType)
			{
				il.Emit(Mono.Cecil.Cil.OpCodes.Unbox_Any, module.ImportReference(typeFromHandle));
			}
			return id;
		}

		// Token: 0x04000F6A RID: 3946
		private static List<object> References = new List<object>();

		// Token: 0x04000F6B RID: 3947
		private static readonly MethodInfo _GetMethodFromHandle = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) });

		// Token: 0x04000F6C RID: 3948
		private static readonly MethodInfo _GetReference = typeof(DynamicMethodHelper).GetMethod("GetReference");
	}
}
