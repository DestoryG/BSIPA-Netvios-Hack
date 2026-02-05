using System;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200002F RID: 47
	public static class BeatSaberInboundMessageReflection
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00011357 File Offset: 0x0000F557
		public static FileDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.descriptor;
			}
		}

		// Token: 0x04000206 RID: 518
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[]
		{
			"Ch1CZWF0U2FiZXJJbmJvdW5kTWVzc2FnZS5wcm90bxIZY29tLm5ldHZpb3Mu", "cHJvdG8uaW5ib3VuZBoeZ29vZ2xlL3Byb3RvYnVmL3dyYXBwZXJzLnByb3Rv", "GhZCZWF0U2FiZXJNZXNzYWdlLnByb3RvIoQLCg1CZWF0U2FiZXJCb2R5EikK", "BHR5cGUYASABKA4yGy5jb20ubmV0dmlvcy5wcm90by5EYXRhVHlwZRIvCgRw", "aW5nGAIgASgLMh8uY29tLm5ldHZpb3MucHJvdG8uaW5ib3VuZC5QaW5nSAAS", "MQoFbG9naW4YAyABKAsyIC5jb20ubmV0dmlvcy5wcm90by5pbmJvdW5kLkxv", "Z2luSAASMQoFcmVuZXcYBCABKAsyIC5jb20ubmV0dmlvcy5wcm90by5pbmJv", "dW5kLlJlbmV3SAASMwoGbG9nb3V0GAUgASgLMiEuY29tLm5ldHZpb3MucHJv", "dG8uaW5ib3VuZC5Mb2dvdXRIABI5CglnZXRQbGF5ZXIYBiABKAsyJC5jb20u", "bmV0dmlvcy5wcm90by5pbmJvdW5kLkdldFBsYXllckgAEjcKCHNvbmdMaXN0",
			"GAcgASgLMiMuY29tLm5ldHZpb3MucHJvdG8uaW5ib3VuZC5Tb25nTGlzdEgA", "EjcKCHJvb21MaXN0GAggASgLMiMuY29tLm5ldHZpb3MucHJvdG8uaW5ib3Vu", "ZC5Sb29tTGlzdEgAEjUKB2dldFJvb20YCSABKAsyIi5jb20ubmV0dmlvcy5w", "cm90by5pbmJvdW5kLkdldFJvb21IABI7CgpjcmVhdGVSb29tGAogASgLMiUu", "Y29tLm5ldHZpb3MucHJvdG8uaW5ib3VuZC5DcmVhdGVSb29tSAASNwoIam9p", "blJvb20YCyABKAsyIy5jb20ubmV0dmlvcy5wcm90by5pbmJvdW5kLkpvaW5S", "b29tSAASNwoIZXhpdFJvb20YDCABKAsyIy5jb20ubmV0dmlvcy5wcm90by5p", "bmJvdW5kLkV4aXRSb29tSAASSQoRa2lja091dFJvb21QbGF5ZXIYDSABKAsy", "LC5jb20ubmV0dmlvcy5wcm90by5pbmJvdW5kLktpY2tPdXRSb29tUGxheWVy", "SAASOQoJc3RhcnRHYW1lGA4gASgLMiQuY29tLm5ldHZpb3MucHJvdG8uaW5i",
			"b3VuZC5TdGFydEdhbWVIABJBCg1yb29tQnJvYWRjYXN0GA8gASgLMiguY29t", "Lm5ldHZpb3MucHJvdG8uaW5ib3VuZC5Sb29tQnJvYWRjYXN0SAASRQoPY2hh", "bmdlUm9vbU93bmVyGBAgASgLMiouY29tLm5ldHZpb3MucHJvdG8uaW5ib3Vu", "ZC5DaGFuZ2VSb29tT3duZXJIABJJChFtb2RpZnlQZXJzb25hbENmZxgRIAEo", "CzIsLmNvbS5uZXR2aW9zLnByb3RvLmluYm91bmQuTW9kaWZ5UGVyc29uYWxD", "ZmdIABJBCg1tb2RpZnlSb29tQ2ZnGBIgASgLMiguY29tLm5ldHZpb3MucHJv", "dG8uaW5ib3VuZC5Nb2RpZnlSb29tQ2ZnSAASQQoNbW9kaWZ5U29uZ0NmZxgT", "IAEoCzIoLmNvbS5uZXR2aW9zLnByb3RvLmluYm91bmQuTW9kaWZ5U29uZ0Nm", "Z0gAEkUKD3Jvb21TdWJtaXRTY29yZRgUIAEoCzIqLmNvbS5uZXR2aW9zLnBy", "b3RvLmluYm91bmQuUm9vbVN1Ym1pdFNjb3JlSAASOQoJZmFzdE1hdGNoGBUg",
			"ASgLMiQuY29tLm5ldHZpb3MucHJvdG8uaW5ib3VuZC5GYXN0TWF0Y2hIABI5", "CglhdXRvTWF0Y2gYFiABKAsyJC5jb20ubmV0dmlvcy5wcm90by5pbmJvdW5k", "LkF1dG9NYXRjaEgAEkMKDm1vZGlmeU5pY2tuYW1lGBggASgLMikuY29tLm5l", "dHZpb3MucHJvdG8uaW5ib3VuZC5Nb2RpZnlOaWNrbmFtZUgAQgYKBGRhdGEi", "GAoEUGluZxIQCghzZXF1ZW5jZRgBIAEoBSJNCgVMb2dpbhITCgthcHBfY2hh", "bm5lbBgBIAEoCRINCgV0b2tlbhgCIAEoCRIQCghuaWNrbmFtZRgDIAEoCRIO", "CgZhdmF0YXIYBCABKAkiBwoFUmVuZXciCAoGTG9nb3V0IgsKCUdldFBsYXll", "ciIiCg5Nb2RpZnlOaWNrbmFtZRIQCghuaWNrbmFtZRgBIAEoCSIyCghSb29t", "TGlzdBITCgtwYWdlX251bWJlchgBIAEoBRIRCglwYWdlX3NpemUYAiABKAUi", "GgoHR2V0Um9vbRIPCgdyb29tX2lkGAEgASgJIrMBCgpDcmVhdGVSb29tEjMK",
			"B3Jvb21DZmcYASABKAsyIi5jb20ubmV0dmlvcy5wcm90by5pbmJvdW5kLlJv", "b21DZmcSMwoHc29uZ0NmZxgCIAEoCzIiLmNvbS5uZXR2aW9zLnByb3RvLmlu", "Ym91bmQuU29uZ0NmZxI7CgtwZXJzb25hbENmZxgDIAEoCzImLmNvbS5uZXR2", "aW9zLnByb3RvLmluYm91bmQuUGVyc29uYWxDZmciLQoISm9pblJvb20SDwoH", "cm9vbV9pZBgBIAEoCRIQCghwYXNzd29yZBgCIAEoCSIbCghFeGl0Um9vbRIP", "Cgdyb29tX2lkGAEgASgJIj4KEUtpY2tPdXRSb29tUGxheWVyEg8KB3Jvb21f", "aWQYASABKAkSGAoQdGFyZ2V0X3BsYXllcl9pZBgCIAEoAyIcCglTdGFydEdh", "bWUSDwoHcm9vbV9pZBgBIAEoCSI8Cg9DaGFuZ2VSb29tT3duZXISDwoHcm9v", "bV9pZBgBIAEoCRIYChB0YXJnZXRfcGxheWVyX2lkGAIgASgDIokBChFNb2Rp", "ZnlQZXJzb25hbENmZxIPCgdyb29tX2lkGAEgASgJEjAKDGhlYWRwaG9uZV9v",
			"bhgCIAEoCzIaLmdvb2dsZS5wcm90b2J1Zi5Cb29sVmFsdWUSMQoNbWljcm9w", "aG9uZV9vbhgDIAEoCzIaLmdvb2dsZS5wcm90b2J1Zi5Cb29sVmFsdWUimAEK", "DU1vZGlmeVJvb21DZmcSDwoHcm9vbV9pZBgBIAEoCRIRCglyb29tX25hbWUY", "AiABKAkSEwoLbWF4X3BsYXllcnMYAyABKAUSLgoIcGFzc3dvcmQYBCABKAsy", "HC5nb29nbGUucHJvdG9idWYuU3RyaW5nVmFsdWUSHgoWcmVzdWx0X2Rpc3Bs", "YXlfc2Vjb25kcxgFIAEoBSJiCg1Nb2RpZnlTb25nQ2ZnEg8KB3Jvb21faWQY", "ASABKAkSDwoHc29uZ19pZBgCIAEoCRIMCgRtb2RlGAMgASgJEhIKCmRpZmZp", "Y3VsdHkYBCABKAkSDQoFcnVsZXMYCiABKAkitQQKD1Jvb21TdWJtaXRTY29y", "ZRIPCgdyb29tX2lkGAEgASgJEhMKC2FwcF9jaGFubmVsGAIgASgJEhAKCGxl", "dmVsX2lkGAMgASgJEhIKCmRpZmZpY3VsdHkYBCABKAkSFwoPc29uZ19kaWRf",
			"ZmluaXNoGAUgASgIEhEKCWxldmVsX2JwbRgGIAEoBRIMCgRyYW5rGAcgASgJ", "EhEKCW1heF9jb21ibxgIIAEoBRIWCg5tb2RpZmllZF9zY29yZRgJIAEoBRIX", "Cg9nb29kX2N1dHNfY291bnQYCiABKAUSFgoOYmFkX2N1dHNfY291bnQYCyAB", "KAUSFAoMbWlzc2VkX2NvdW50GAwgASgFEhUKDWVuZF9zb25nX3RpbWUYDSAB", "KAUSFQoNc29uZ19kdXJhdGlvbhgOIAEoBRIjChtsZWZ0X2hhbmRfbW92ZW1l", "bnRfZGlzdGFuY2UYDyABKAUSJAoccmlnaHRfaGFuZF9tb3ZlbWVudF9kaXN0", "YW5jZRgQIAEoBRIkChxsZWZ0X3NhYmVyX21vdmVtZW50X2Rpc3RhbmNlGBEg", "ASgFEiUKHXJpZ2h0X3NhYmVyX21vdmVtZW50X2Rpc3RhbmNlGBIgASgFEhAK", "CG9rX2NvdW50GBMgASgFEhYKDm5vdF9nb29kX2NvdW50GBQgASgFEgwKBG1v", "ZGUYFSABKAkSEQoJcmF3X3Njb3JlGB4gASgFEhkKEWxldmVsRW5kU3RhdGVU",
			"eXBlGB8gASgJIlsKDVJvb21Ccm9hZGNhc3QSDwoHcm9vbV9pZBgBIAEoCRIM", "CgR0eXBlGAIgASgJEg8KB2NvbnRlbnQYAyABKAkSGgoSZXhjbHVkZV9wbGF5", "ZXJfaWRzGAQgAygDIkYKCFNvbmdMaXN0EhMKC3BhZ2VfbnVtYmVyGAEgASgF", "EhEKCXBhZ2Vfc2l6ZRgCIAEoBRISCgpxdWVyeV9qc29uGAMgASgJIhwKCUZh", "c3RNYXRjaBIPCgdzb25nX2lkGAEgASgJIhwKCUF1dG9NYXRjaBIPCgdzb25n", "X2lkGAEgASgJIjoKC1BlcnNvbmFsQ2ZnEhQKDGhlYWRwaG9uZV9vbhgBIAEo", "CBIVCg1taWNyb3Bob25lX29uGAIgASgIImMKB1Jvb21DZmcSEQoJcm9vbV9u", "YW1lGAEgASgJEhMKC21heF9wbGF5ZXJzGAIgASgFEhAKCHBhc3N3b3JkGAMg", "ASgJEh4KFnJlc3VsdF9kaXNwbGF5X3NlY29uZHMYBCABKAUiSwoHU29uZ0Nm", "ZxIPCgdzb25nX2lkGAEgASgJEgwKBG1vZGUYAiABKAkSEgoKZGlmZmljdWx0",
			"eRgDIAEoCRINCgVydWxlcxgKIAEoCUIxChhjb20ubmV0dmlvcy50Y3AucHJv", "dG8uaW5CFUJlYXRTYWJlckluYm91bmRQcm90b2IGcHJvdG8z"
		})), new FileDescriptor[]
		{
			WrappersReflection.Descriptor,
			BeatSaberMessageReflection.Descriptor
		}, new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(BeatSaberBody), BeatSaberBody.Parser, new string[]
			{
				"Type", "Ping", "Login", "Renew", "Logout", "GetPlayer", "SongList", "RoomList", "GetRoom", "CreateRoom",
				"JoinRoom", "ExitRoom", "KickOutRoomPlayer", "StartGame", "RoomBroadcast", "ChangeRoomOwner", "ModifyPersonalCfg", "ModifyRoomCfg", "ModifySongCfg", "RoomSubmitScore",
				"FastMatch", "AutoMatch", "ModifyNickname"
			}, new string[] { "Data" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(Ping), Ping.Parser, new string[] { "Sequence" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Login), Login.Parser, new string[] { "AppChannel", "Token", "Nickname", "Avatar" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Renew), Renew.Parser, null, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Logout), Logout.Parser, null, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetPlayer), GetPlayer.Parser, null, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifyNickname), ModifyNickname.Parser, new string[] { "Nickname" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomList), RoomList.Parser, new string[] { "PageNumber", "PageSize" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetRoom), GetRoom.Parser, new string[] { "RoomId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(CreateRoom), CreateRoom.Parser, new string[] { "RoomCfg", "SongCfg", "PersonalCfg" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(JoinRoom), JoinRoom.Parser, new string[] { "RoomId", "Password" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ExitRoom), ExitRoom.Parser, new string[] { "RoomId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(KickOutRoomPlayer), KickOutRoomPlayer.Parser, new string[] { "RoomId", "TargetPlayerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(StartGame), StartGame.Parser, new string[] { "RoomId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChangeRoomOwner), ChangeRoomOwner.Parser, new string[] { "RoomId", "TargetPlayerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifyPersonalCfg), ModifyPersonalCfg.Parser, new string[] { "RoomId", "HeadphoneOn", "MicrophoneOn" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifyRoomCfg), ModifyRoomCfg.Parser, new string[] { "RoomId", "RoomName", "MaxPlayers", "Password", "ResultDisplaySeconds" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifySongCfg), ModifySongCfg.Parser, new string[] { "RoomId", "SongId", "Mode", "Difficulty", "Rules" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomSubmitScore), RoomSubmitScore.Parser, new string[]
			{
				"RoomId", "AppChannel", "LevelId", "Difficulty", "SongDidFinish", "LevelBpm", "Rank", "MaxCombo", "ModifiedScore", "GoodCutsCount",
				"BadCutsCount", "MissedCount", "EndSongTime", "SongDuration", "LeftHandMovementDistance", "RightHandMovementDistance", "LeftSaberMovementDistance", "RightSaberMovementDistance", "OkCount", "NotGoodCount",
				"Mode", "RawScore", "LevelEndStateType"
			}, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomBroadcast), RoomBroadcast.Parser, new string[] { "RoomId", "Type", "Content", "ExcludePlayerIds" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(SongList), SongList.Parser, new string[] { "PageNumber", "PageSize", "QueryJson" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FastMatch), FastMatch.Parser, new string[] { "SongId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AutoMatch), AutoMatch.Parser, new string[] { "SongId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(PersonalCfg), PersonalCfg.Parser, new string[] { "HeadphoneOn", "MicrophoneOn" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomCfg), RoomCfg.Parser, new string[] { "RoomName", "MaxPlayers", "Password", "ResultDisplaySeconds" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(SongCfg), SongCfg.Parser, new string[] { "SongId", "Mode", "Difficulty", "Rules" }, null, null, null, null)
		}));
	}
}
