using System;

namespace BeatSaverSharp.Exceptions
{
	// Token: 0x02000019 RID: 25
	public class InvalidSteamIDException : Exception
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00003B25 File Offset: 0x00001D25
		public InvalidSteamIDException(string steamID)
		{
			this.SteamID = steamID;
		}

		// Token: 0x0400005D RID: 93
		public readonly string SteamID;
	}
}
