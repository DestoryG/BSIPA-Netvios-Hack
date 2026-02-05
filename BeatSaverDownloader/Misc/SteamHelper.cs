using System;
using Steamworks;

namespace BeatSaverDownloader.Misc
{
	// Token: 0x02000022 RID: 34
	public static class SteamHelper
	{
		// Token: 0x04000097 RID: 151
		public static HAuthTicket lastTicket;

		// Token: 0x04000098 RID: 152
		public static EResult lastTicketResult;

		// Token: 0x04000099 RID: 153
		public static Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;
	}
}
