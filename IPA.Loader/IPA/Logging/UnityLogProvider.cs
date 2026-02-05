using System;

namespace IPA.Logging
{
	// Token: 0x02000035 RID: 53
	internal static class UnityLogProvider
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00005C7C File Offset: 0x00003E7C
		public static Logger UnityLogger
		{
			get
			{
				Logger logger;
				if ((logger = UnityLogProvider.Logger) == null)
				{
					logger = (UnityLogProvider.Logger = new StandardLogger("UnityEngine"));
				}
				return logger;
			}
		}

		// Token: 0x0400007B RID: 123
		internal static Logger Logger;
	}
}
