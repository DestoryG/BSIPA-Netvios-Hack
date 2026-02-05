using System;

namespace SongCore
{
	// Token: 0x02000010 RID: 16
	public class NetviosConfig
	{
		// Token: 0x060000BC RID: 188 RVA: 0x0000371B File Offset: 0x0000191B
		public static string GetAPINetviosLevelsURI(string version = "v1")
		{
			if (version.Length == 0)
			{
				version = "v1";
			}
			return "https://beatsaberbbs.com" + "/api/mod/" + version + "/maps/list";
		}

		// Token: 0x04000021 RID: 33
		private const string API_HOST_DEV = "http://dev.beatsaberbbs.com";

		// Token: 0x04000022 RID: 34
		private const string API_HOST_PUB = "https://beatsaberbbs.com";

		// Token: 0x04000023 RID: 35
		private const string API_URI_PREFIX = "/api/mod";

		// Token: 0x04000024 RID: 36
		private const string LEVELS_URI = "/maps/list";
	}
}
