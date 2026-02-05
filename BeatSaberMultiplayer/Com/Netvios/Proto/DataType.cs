using System;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto
{
	// Token: 0x02000004 RID: 4
	public enum DataType
	{
		// Token: 0x0400000A RID: 10
		[OriginalName("Ping")]
		Ping,
		// Token: 0x0400000B RID: 11
		[OriginalName("Login")]
		Login,
		// Token: 0x0400000C RID: 12
		[OriginalName("Renew")]
		Renew,
		// Token: 0x0400000D RID: 13
		[OriginalName("Logout")]
		Logout,
		// Token: 0x0400000E RID: 14
		[OriginalName("GetPlayer")]
		GetPlayer,
		// Token: 0x0400000F RID: 15
		[OriginalName("SongList")]
		SongList,
		// Token: 0x04000010 RID: 16
		[OriginalName("RoomList")]
		RoomList,
		// Token: 0x04000011 RID: 17
		[OriginalName("GetRoom")]
		GetRoom,
		// Token: 0x04000012 RID: 18
		[OriginalName("CreateRoom")]
		CreateRoom,
		// Token: 0x04000013 RID: 19
		[OriginalName("JoinRoom")]
		JoinRoom,
		// Token: 0x04000014 RID: 20
		[OriginalName("ExitRoom")]
		ExitRoom,
		// Token: 0x04000015 RID: 21
		[OriginalName("KickOutRoomPlayer")]
		KickOutRoomPlayer,
		// Token: 0x04000016 RID: 22
		[OriginalName("StartGame")]
		StartGame,
		// Token: 0x04000017 RID: 23
		[OriginalName("ChangeRoomOwner")]
		ChangeRoomOwner,
		// Token: 0x04000018 RID: 24
		[OriginalName("ModifyPersonalCfg")]
		ModifyPersonalCfg,
		// Token: 0x04000019 RID: 25
		[OriginalName("ModifyRoomCfg")]
		ModifyRoomCfg,
		// Token: 0x0400001A RID: 26
		[OriginalName("ModifySongCfg")]
		ModifySongCfg,
		// Token: 0x0400001B RID: 27
		[OriginalName("RoomSubmitScore")]
		RoomSubmitScore,
		// Token: 0x0400001C RID: 28
		[OriginalName("RoomBroadcast")]
		RoomBroadcast,
		// Token: 0x0400001D RID: 29
		[OriginalName("FastMatch")]
		FastMatch,
		// Token: 0x0400001E RID: 30
		[OriginalName("AutoMatch")]
		AutoMatch,
		// Token: 0x0400001F RID: 31
		[OriginalName("ModifyNickname")]
		ModifyNickname = 22,
		// Token: 0x04000020 RID: 32
		[OriginalName("KickedOutNotice")]
		KickedOutNotice = 32,
		// Token: 0x04000021 RID: 33
		[OriginalName("RoomUpdatedNotice")]
		RoomUpdatedNotice,
		// Token: 0x04000022 RID: 34
		[OriginalName("KickedOutRoomNotice")]
		KickedOutRoomNotice,
		// Token: 0x04000023 RID: 35
		[OriginalName("StartGameNotice")]
		StartGameNotice,
		// Token: 0x04000024 RID: 36
		[OriginalName("RoomSubmitScoreNotice")]
		RoomSubmitScoreNotice,
		// Token: 0x04000025 RID: 37
		[OriginalName("RoomBroadcastNotice")]
		RoomBroadcastNotice,
		// Token: 0x04000026 RID: 38
		[OriginalName("AutoMatchNotice")]
		AutoMatchNotice,
		// Token: 0x04000027 RID: 39
		[OriginalName("HeadphoneOnNotice")]
		HeadphoneOnNotice
	}
}
