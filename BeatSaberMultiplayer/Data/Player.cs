using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x0200008F RID: 143
	public class Player
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x00026A1D File Offset: 0x00024C1D
		public Player(string udid)
		{
			this.udid = udid;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00026A2C File Offset: 0x00024C2C
		public Player(long playerId, string nickname)
		{
			this.playerId = playerId;
			this.nickname = nickname;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00026A42 File Offset: 0x00024C42
		public Player(long playerId, string nickname, int score)
		{
			this.playerId = playerId;
			this.nickname = nickname;
			this.score = score;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00026A5F File Offset: 0x00024C5F
		public Player(string openid, string unionid, string udid)
		{
			this.openid = openid;
			this.unionid = unionid;
			this.udid = udid;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00026A7C File Offset: 0x00024C7C
		public Player(string appChannel, string token)
		{
			this.appChannel = appChannel;
			this.token = token;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00026A94 File Offset: 0x00024C94
		public Player(string appChannel, string appid, string extraData, string merchantAccount, string openid, string sdkVersion, string udid, string unionid)
		{
			this.appChannel = appChannel;
			this.appid = appid;
			this.extraData = extraData;
			this.merchantAccount = merchantAccount;
			this.openid = openid;
			this.sdkVersion = sdkVersion;
			this.udid = udid;
			this.unionid = unionid;
			this.token = unionid;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00026AEC File Offset: 0x00024CEC
		public Player(string appChannel, long playerId, string nickname, string avatar, int score, string status, PlayerCfg playerCfg = null)
		{
			this.appChannel = appChannel;
			this.playerId = playerId;
			this.nickname = nickname;
			this.avatar = avatar;
			this.score = score;
			this.status = (PlayerStatus)Enum.Parse(typeof(PlayerStatus), status, true);
			this.playerCfg = playerCfg;
		}

		// Token: 0x040004A2 RID: 1186
		public long playerId;

		// Token: 0x040004A3 RID: 1187
		public string nickname;

		// Token: 0x040004A4 RID: 1188
		public string avatar;

		// Token: 0x040004A5 RID: 1189
		public int score;

		// Token: 0x040004A6 RID: 1190
		public PlayerStatus status;

		// Token: 0x040004A7 RID: 1191
		public PlayerCfg playerCfg;

		// Token: 0x040004A8 RID: 1192
		public string appChannel;

		// Token: 0x040004A9 RID: 1193
		public string appid;

		// Token: 0x040004AA RID: 1194
		public string extraData;

		// Token: 0x040004AB RID: 1195
		public string merchantAccount;

		// Token: 0x040004AC RID: 1196
		public string openid;

		// Token: 0x040004AD RID: 1197
		public string sdkVersion;

		// Token: 0x040004AE RID: 1198
		public string udid;

		// Token: 0x040004AF RID: 1199
		public string unionid;

		// Token: 0x040004B0 RID: 1200
		public string token;
	}
}
