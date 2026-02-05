using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000063 RID: 99
	internal static class LocalAppContext
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0001E7BA File Offset: 0x0001C9BA
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x0001E7C1 File Offset: 0x0001C9C1
		private static bool DisableCaching { get; set; }

		// Token: 0x0600044E RID: 1102 RVA: 0x0001E7C9 File Offset: 0x0001C9C9
		static LocalAppContext()
		{
			global::System.AppContextDefaultValues.PopulateDefaultValues();
			LocalAppContext.DisableCaching = LocalAppContext.IsSwitchEnabled("TestSwitch.LocalAppContext.DisableCaching");
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001E800 File Offset: 0x0001CA00
		public static bool IsSwitchEnabled(string switchName)
		{
			bool flag;
			if (LocalAppContext.s_canForwardCalls && LocalAppContext.TryGetSwitchFromCentralAppContext(switchName, out flag))
			{
				return flag;
			}
			return LocalAppContext.IsSwitchEnabledLocal(switchName);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001E82C File Offset: 0x0001CA2C
		private static bool IsSwitchEnabledLocal(string switchName)
		{
			Dictionary<string, bool> dictionary = LocalAppContext.s_switchMap;
			bool flag3;
			bool flag2;
			lock (dictionary)
			{
				flag2 = LocalAppContext.s_switchMap.TryGetValue(switchName, out flag3);
			}
			return flag2 && flag3;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001E87C File Offset: 0x0001CA7C
		private static bool SetupDelegate()
		{
			Type type = typeof(object).Assembly.GetType("System.AppContext");
			if (type == null)
			{
				return false;
			}
			MethodInfo method = type.GetMethod("TryGetSwitch", BindingFlags.Static | BindingFlags.Public, null, new Type[]
			{
				typeof(string),
				typeof(bool).MakeByRefType()
			}, null);
			if (method == null)
			{
				return false;
			}
			LocalAppContext.TryGetSwitchFromCentralAppContext = (LocalAppContext.TryGetSwitchDelegate)Delegate.CreateDelegate(typeof(LocalAppContext.TryGetSwitchDelegate), method);
			return true;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001E909 File Offset: 0x0001CB09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool GetCachedSwitchValue(string switchName, ref int switchValue)
		{
			return switchValue >= 0 && (switchValue > 0 || LocalAppContext.GetCachedSwitchValueInternal(switchName, ref switchValue));
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001E920 File Offset: 0x0001CB20
		private static bool GetCachedSwitchValueInternal(string switchName, ref int switchValue)
		{
			if (LocalAppContext.DisableCaching)
			{
				return LocalAppContext.IsSwitchEnabled(switchName);
			}
			bool flag = LocalAppContext.IsSwitchEnabled(switchName);
			switchValue = (flag ? 1 : (-1));
			return flag;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001E94C File Offset: 0x0001CB4C
		internal static void DefineSwitchDefault(string switchName, bool initialValue)
		{
			LocalAppContext.s_switchMap[switchName] = initialValue;
		}

		// Token: 0x0400052F RID: 1327
		private static LocalAppContext.TryGetSwitchDelegate TryGetSwitchFromCentralAppContext;

		// Token: 0x04000530 RID: 1328
		private static bool s_canForwardCalls = LocalAppContext.SetupDelegate();

		// Token: 0x04000531 RID: 1329
		private static Dictionary<string, bool> s_switchMap = new Dictionary<string, bool>();

		// Token: 0x04000532 RID: 1330
		private static readonly object s_syncLock = new object();

		// Token: 0x020006E5 RID: 1765
		// (Invoke) Token: 0x0600403D RID: 16445
		private delegate bool TryGetSwitchDelegate(string switchName, out bool value);
	}
}
