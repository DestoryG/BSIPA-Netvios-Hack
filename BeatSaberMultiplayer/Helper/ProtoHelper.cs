using System;
using System.Collections.Generic;
using BeatSaberMultiplayer.Data;
using Com.Netvios.Proto.Outbound;
using Google.Protobuf.Collections;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000078 RID: 120
	internal class ProtoHelper
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x00024444 File Offset: 0x00022644
		public static void FormatNetviosOutboundData(string roomId, Com.Netvios.Proto.Outbound.RoomCfg roomCfg, Com.Netvios.Proto.Outbound.SongCfg songCfg, RepeatedField<RoomPlayer> players, out BeatSaberMultiplayer.Data.RoomCfg outRoomCfg, out BeatSaberMultiplayer.Data.SongCfg outSongCfg, out Player[] outPlayers)
		{
			outRoomCfg = new BeatSaberMultiplayer.Data.RoomCfg(roomId, roomCfg.RoomName, roomCfg.IsPrivate, roomCfg.MaxPlayers, (float)roomCfg.ResultDisplaySeconds);
			List<Player> list = new List<Player>();
			foreach (RoomPlayer roomPlayer in players)
			{
				PlayerCfg playerCfg = new PlayerCfg(roomPlayer.PlayerId, roomPlayer.PersonalCfg.HeadphoneOn, roomPlayer.PersonalCfg.MicrophoneOn);
				Player player = new Player(roomPlayer.AppChannel, roomPlayer.PlayerId, roomPlayer.Nickname, roomPlayer.Avatar, roomPlayer.Score, roomPlayer.Status, playerCfg);
				list.Add(player);
			}
			outPlayers = list.ToArray();
			outSongCfg = new BeatSaberMultiplayer.Data.SongCfg(songCfg.SongId, songCfg.SongName, songCfg.Mode, songCfg.Difficulty, songCfg.SongCoverImg, songCfg.SongDuration, songCfg.Rules);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00024540 File Offset: 0x00022740
		public static void FormatNetviosOutboundData(RepeatedField<RoomPlayer> players, out Player[] outPlayers)
		{
			List<Player> list = new List<Player>();
			foreach (RoomPlayer roomPlayer in players)
			{
				PlayerCfg playerCfg = new PlayerCfg(roomPlayer.PlayerId, roomPlayer.PersonalCfg.HeadphoneOn, roomPlayer.PersonalCfg.MicrophoneOn);
				Player player = new Player(roomPlayer.AppChannel, roomPlayer.PlayerId, roomPlayer.Nickname, roomPlayer.Avatar, roomPlayer.Score, roomPlayer.Status, playerCfg);
				list.Add(player);
			}
			outPlayers = list.ToArray();
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000245E8 File Offset: 0x000227E8
		public static void FormatNetviosOutboundData(string roomId, Com.Netvios.Proto.Outbound.RoomCfg roomCfg, out BeatSaberMultiplayer.Data.RoomCfg outRoomCfg)
		{
			outRoomCfg = new BeatSaberMultiplayer.Data.RoomCfg(roomId, roomCfg.RoomName, roomCfg.IsPrivate, roomCfg.MaxPlayers, (float)roomCfg.ResultDisplaySeconds);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0002460B File Offset: 0x0002280B
		public static void FormatNetviosOutboundData(Com.Netvios.Proto.Outbound.SongCfg songCfg, out BeatSaberMultiplayer.Data.SongCfg outSongCfg)
		{
			outSongCfg = new BeatSaberMultiplayer.Data.SongCfg(songCfg.SongId, songCfg.SongName, songCfg.Mode, songCfg.Difficulty, songCfg.SongCoverImg, songCfg.SongDuration, songCfg.Rules);
		}
	}
}
