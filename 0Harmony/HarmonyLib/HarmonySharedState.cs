using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000019 RID: 25
	internal static class HarmonySharedState
	{
		// Token: 0x06000090 RID: 144 RVA: 0x000046A0 File Offset: 0x000028A0
		private static Dictionary<MethodBase, byte[]> GetState()
		{
			string text = "HarmonySharedState";
			Dictionary<MethodBase, byte[]> dictionary;
			lock (text)
			{
				Assembly assembly = HarmonySharedState.SharedStateAssembly();
				if (assembly == null)
				{
					HarmonySharedState.CreateModule();
					assembly = HarmonySharedState.SharedStateAssembly();
					if (assembly == null)
					{
						throw new Exception("Cannot find or create harmony shared state");
					}
				}
				Type type = assembly.GetType("HarmonySharedState");
				FieldInfo field = type.GetField("version");
				if (field == null)
				{
					throw new Exception("Cannot find harmony state version field");
				}
				HarmonySharedState.actualVersion = (int)field.GetValue(null);
				FieldInfo field2 = type.GetField("state");
				if (field2 == null)
				{
					throw new Exception("Cannot find harmony state field");
				}
				if (field2.GetValue(null) == null)
				{
					field2.SetValue(null, new Dictionary<MethodBase, byte[]>());
				}
				dictionary = (Dictionary<MethodBase, byte[]>)field2.GetValue(null);
			}
			return dictionary;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004788 File Offset: 0x00002988
		private static void CreateModule()
		{
			ModuleParameters moduleParameters = new ModuleParameters
			{
				Kind = ModuleKind.Dll,
				ReflectionImporterProvider = MMReflectionImporter.Provider
			};
			using (ModuleDefinition moduleDefinition = ModuleDefinition.CreateModule("HarmonySharedState", moduleParameters))
			{
				Mono.Cecil.TypeAttributes typeAttributes = Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed;
				TypeDefinition typeDefinition = new TypeDefinition("", "HarmonySharedState", typeAttributes)
				{
					BaseType = moduleDefinition.TypeSystem.Object
				};
				moduleDefinition.Types.Add(typeDefinition);
				typeDefinition.Fields.Add(new FieldDefinition("state", Mono.Cecil.FieldAttributes.FamANDAssem | Mono.Cecil.FieldAttributes.Family | Mono.Cecil.FieldAttributes.Static, moduleDefinition.ImportReference(typeof(Dictionary<MethodBase, byte[]>))));
				FieldDefinition fieldDefinition = new FieldDefinition("version", Mono.Cecil.FieldAttributes.FamANDAssem | Mono.Cecil.FieldAttributes.Family | Mono.Cecil.FieldAttributes.Static, moduleDefinition.ImportReference(typeof(int)))
				{
					Constant = 100
				};
				typeDefinition.Fields.Add(fieldDefinition);
				ReflectionHelper.Load(moduleDefinition);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004870 File Offset: 0x00002A70
		private static Assembly SharedStateAssembly()
		{
			return (from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.FullName.StartsWith("Microsoft.VisualStudio")
				select a).FirstOrDefault((Assembly a) => a.GetName().Name.Contains("HarmonySharedState"));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000048D0 File Offset: 0x00002AD0
		internal static PatchInfo GetPatchInfo(MethodBase method)
		{
			byte[] valueSafe = HarmonySharedState.GetState().GetValueSafe(method);
			if (valueSafe == null)
			{
				return null;
			}
			return PatchInfoSerialization.Deserialize(valueSafe);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000048F4 File Offset: 0x00002AF4
		internal static IEnumerable<MethodBase> GetPatchedMethods()
		{
			return HarmonySharedState.GetState().Keys.AsEnumerable<MethodBase>();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004905 File Offset: 0x00002B05
		internal static void UpdatePatchInfo(MethodBase method, PatchInfo patchInfo)
		{
			HarmonySharedState.GetState()[method] = patchInfo.Serialize();
		}

		// Token: 0x0400004A RID: 74
		private const string name = "HarmonySharedState";

		// Token: 0x0400004B RID: 75
		internal const int internalVersion = 100;

		// Token: 0x0400004C RID: 76
		internal static int actualVersion = -1;
	}
}
