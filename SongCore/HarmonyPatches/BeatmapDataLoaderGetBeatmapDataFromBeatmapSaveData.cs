using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using SongCore.Utilities;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000022 RID: 34
	[HarmonyPatch(typeof(BeatmapDataLoader))]
	[HarmonyPatch("GetBeatmapDataFromBeatmapSaveData", MethodType.Normal)]
	internal class BeatmapDataLoaderGetBeatmapDataFromBeatmapSaveData
	{
		// Token: 0x06000189 RID: 393 RVA: 0x000077FC File Offset: 0x000059FC
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].opcode == OpCodes.Ldelem_Ref && !(list[i + 1].opcode != OpCodes.Ldloc_S) && !(list[i + 2].opcode != OpCodes.Callvirt))
				{
					Type localType = ((LocalVariableInfo)list[i + 1].operand).LocalType;
					if (localType == typeof(ObstacleData) || localType == typeof(NoteData))
					{
						Logging.logger.Debug(string.Format("{0}Inserting Clamp Instruction", i));
						list.InsertRange(i, BeatmapDataLoaderGetBeatmapDataFromBeatmapSaveData.clampInstructions);
						i += BeatmapDataLoaderGetBeatmapDataFromBeatmapSaveData.clampInstructions.Count<CodeInstruction>();
					}
				}
			}
			return list.AsEnumerable<CodeInstruction>();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000078EF File Offset: 0x00005AEF
		private static int Clamp(int input, int min, int max)
		{
			return Math.Min(Math.Max(input, min), max);
		}

		// Token: 0x04000092 RID: 146
		private static readonly MethodInfo clampMethod = SymbolExtensions.GetMethodInfo(Expression.Lambda<Action>(Expression.Call(null, methodof(BeatmapDataLoaderGetBeatmapDataFromBeatmapSaveData.Clamp(int, int, int)), new Expression[]
		{
			Expression.Constant(0, typeof(int)),
			Expression.Constant(0, typeof(int)),
			Expression.Constant(0, typeof(int))
		}), Array.Empty<ParameterExpression>()));

		// Token: 0x04000093 RID: 147
		private static readonly CodeInstruction[] clampInstructions = new CodeInstruction[]
		{
			new CodeInstruction(OpCodes.Ldc_I4_0, null),
			new CodeInstruction(OpCodes.Ldc_I4_3, null),
			new CodeInstruction(OpCodes.Call, BeatmapDataLoaderGetBeatmapDataFromBeatmapSaveData.clampMethod)
		};
	}
}
