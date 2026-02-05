using System;

namespace IPA.Netvios
{
	// Token: 0x02000028 RID: 40
	internal class Config
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x0000459B File Offset: 0x0000279B
		internal static string GetModHost()
		{
			return "https://beatsaberbbs.com";
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000045A2 File Offset: 0x000027A2
		internal static string GetModApiUrlPrefix()
		{
			return Config.GetModHost() + "/api/mod/v1/ipa/plugins";
		}

		// Token: 0x0400003C RID: 60
		private const string _devModHost = "http://dev.beatsaberbbs.com";

		// Token: 0x0400003D RID: 61
		private const string _pubModHost = "https://beatsaberbbs.com";
	}
}
