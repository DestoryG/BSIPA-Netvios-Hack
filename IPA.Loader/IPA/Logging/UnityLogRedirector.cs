using System;
using UnityEngine;

namespace IPA.Logging
{
	// Token: 0x02000036 RID: 54
	internal static class UnityLogRedirector
	{
		// Token: 0x06000153 RID: 339 RVA: 0x00005C97 File Offset: 0x00003E97
		public static Logger.Level LogTypeToLevel(LogType type)
		{
			switch (type)
			{
			case 0:
				return Logger.Level.Error;
			case 1:
				return Logger.Level.Debug;
			case 2:
				return Logger.Level.Warning;
			case 3:
				return Logger.Level.Info;
			case 4:
				return Logger.Level.Critical;
			default:
				return Logger.Level.Info;
			}
		}
	}
}
