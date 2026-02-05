using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BS_Utils.Utilities;
using IPA.Logging;
using UnityEngine;

namespace BS_Utils.Gameplay
{
	// Token: 0x0200000C RID: 12
	public class ScoreSubmission
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000446D File Offset: 0x0000266D
		public static bool Disabled
		{
			get
			{
				return ScoreSubmission.disabled;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004474 File Offset: 0x00002674
		public static bool ProlongedDisabled
		{
			get
			{
				return ScoreSubmission.prolongedDisable;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000447B File Offset: 0x0000267B
		public static string LastDisabledModString
		{
			get
			{
				if (ScoreSubmission.LastDisablers == null || ScoreSubmission.LastDisablers.Length == 0)
				{
					return string.Empty;
				}
				return string.Join(", ", ScoreSubmission.LastDisablers);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000044A4 File Offset: 0x000026A4
		public static string ModString
		{
			get
			{
				string text = "";
				for (int i = 0; i < ScoreSubmission.ModList.Count; i++)
				{
					if (i == 0)
					{
						text += ScoreSubmission.ModList[i];
					}
					else
					{
						text = text + ", " + ScoreSubmission.ModList[i];
					}
				}
				return text;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000044FC File Offset: 0x000026FC
		public static string ProlongedModString
		{
			get
			{
				string text = "";
				for (int i = 0; i < ScoreSubmission.ProlongedModList.Count; i++)
				{
					if (i == 0)
					{
						text += ScoreSubmission.ProlongedModList[i];
					}
					else
					{
						text = text + ", " + ScoreSubmission.ProlongedModList[i];
					}
				}
				return text;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004553 File Offset: 0x00002753
		public static bool WasDisabled
		{
			get
			{
				return ScoreSubmission._wasDisabled;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000455A File Offset: 0x0000275A
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004573 File Offset: 0x00002773
		public static string[] LastDisablers
		{
			get
			{
				if (ScoreSubmission._lastDisablers == null)
				{
					return Array.Empty<string>();
				}
				return ScoreSubmission._lastDisablers.ToArray<string>();
			}
			internal set
			{
				ScoreSubmission._lastDisablers = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000457B File Offset: 0x0000277B
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004582 File Offset: 0x00002782
		internal static List<string> ModList { get; set; } = new List<string>(0);

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000458A File Offset: 0x0000278A
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00004591 File Offset: 0x00002791
		internal static List<string> ProlongedModList { get; set; } = new List<string>(0);

		// Token: 0x060000C7 RID: 199 RVA: 0x0000459C File Offset: 0x0000279C
		public static void DisableSubmission(string mod)
		{
			if (!ScoreSubmission.disabled)
			{
				Plugin.ApplyHarmonyPatches();
				ScoreSubmission.disabled = true;
				ScoreSubmission.ModList.Clear();
				if (!ScoreSubmission.eventSubscribed)
				{
					Plugin.LevelDidFinishEvent += ScoreSubmission.LevelData_didFinishEvent;
					ScoreSubmission.eventSubscribed = true;
				}
			}
			if (!ScoreSubmission.ModList.Contains(mod))
			{
				ScoreSubmission.ModList.Add(mod);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000045FC File Offset: 0x000027FC
		internal static void DisableScoreSaberScoreSubmission()
		{
			StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSO = Resources.FindObjectsOfTypeAll<StandardLevelScenesTransitionSetupDataSO>().FirstOrDefault<StandardLevelScenesTransitionSetupDataSO>();
			if (standardLevelScenesTransitionSetupDataSO == null)
			{
				BS_Utils.Utilities.Logger.Log("ScoreSubmission: StandardLevelScenesTransitionSetupDataSO not found - exiting...", IPA.Logging.Logger.Level.Warning);
				return;
			}
			ScoreSubmission.DisableEvent(standardLevelScenesTransitionSetupDataSO, "didFinishEvent", "Five");
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000463C File Offset: 0x0000283C
		private static void LevelData_didFinishEvent(StandardLevelScenesTransitionSetupDataSO arg1, LevelCompletionResults arg2)
		{
			ScoreSubmission._wasDisabled = ScoreSubmission.disabled;
			ScoreSubmission._lastDisablers = ScoreSubmission.ModList.ToArray();
			ScoreSubmission.disabled = false;
			ScoreSubmission.ModList.Clear();
			Plugin.LevelDidFinishEvent -= ScoreSubmission.LevelData_didFinishEvent;
			if (ScoreSubmission.RemovedFive != null)
			{
				StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSO = Resources.FindObjectsOfTypeAll<StandardLevelScenesTransitionSetupDataSO>().FirstOrDefault<StandardLevelScenesTransitionSetupDataSO>();
				standardLevelScenesTransitionSetupDataSO.didFinishEvent -= ScoreSubmission.RemovedFive;
				standardLevelScenesTransitionSetupDataSO.didFinishEvent += ScoreSubmission.RemovedFive;
				ScoreSubmission.RemovedFive = null;
			}
			ScoreSubmission.eventSubscribed = false;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000046B7 File Offset: 0x000028B7
		public static void ProlongedDisableSubmission(string mod)
		{
			if (!ScoreSubmission.prolongedDisable)
			{
				Plugin.ApplyHarmonyPatches();
				ScoreSubmission.prolongedDisable = true;
			}
			if (!ScoreSubmission.ProlongedModList.Contains(mod))
			{
				ScoreSubmission.ProlongedModList.Add(mod);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000046E3 File Offset: 0x000028E3
		public static void RemoveProlongedDisable(string mod)
		{
			ScoreSubmission.ProlongedModList.Remove(mod);
			if (ScoreSubmission.ProlongedModList.Count == 0)
			{
				ScoreSubmission.prolongedDisable = false;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004704 File Offset: 0x00002904
		private static bool DisableEvent(object target, string eventName, string delegateName)
		{
			FieldInfo field = target.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MulticastDelegate multicastDelegate = field.GetValue(target) as MulticastDelegate;
			bool flag = false;
			if (multicastDelegate != null)
			{
				Delegate[] invocationList = multicastDelegate.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					if (@delegate.Method.Name == delegateName)
					{
						ScoreSubmission.RemovedFive = (Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>)@delegate;
						target.GetType().GetEvent(eventName).RemoveEventHandler(target, @delegate);
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004790 File Offset: 0x00002990
		private static void LogEvents(object target, string eventName)
		{
			FieldInfo field = target.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MulticastDelegate multicastDelegate = field.GetValue(target) as MulticastDelegate;
			if (multicastDelegate == null)
			{
				BS_Utils.Utilities.Logger.Log("ScoreSubmission: Unable to get eventDelegate from StandardLevelScenesTransitionSetupDataSO - exiting...", IPA.Logging.Logger.Level.Debug);
			}
			Delegate[] invocationList = multicastDelegate.GetInvocationList();
			BS_Utils.Utilities.Logger.Log("ScoreSubmission: Getting list of delegates for didFinish event...", IPA.Logging.Logger.Level.Debug);
			foreach (Delegate @delegate in invocationList)
			{
				BS_Utils.Utilities.Logger.Log(string.Format("ScoreSubmission: Found delegate named '{0}' by Module '{1}', part of Assembly '{2}'", @delegate.Method.Name, @delegate.Method.Module.Name, @delegate.Method.Module.Assembly.FullName), IPA.Logging.Logger.Level.Debug);
			}
		}

		// Token: 0x0400003F RID: 63
		public static bool eventSubscribed = false;

		// Token: 0x04000040 RID: 64
		internal static bool disabled = false;

		// Token: 0x04000041 RID: 65
		internal static bool prolongedDisable = false;

		// Token: 0x04000044 RID: 68
		internal static bool _wasDisabled = false;

		// Token: 0x04000045 RID: 69
		private static string[] _lastDisablers;

		// Token: 0x04000046 RID: 70
		private static Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> RemovedFive;
	}
}
