using System;
using Newtonsoft.Json.Linq;
using SongCore;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public class Song
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x0000370C File Offset: 0x0000190C
		public Song()
		{
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00023804 File Offset: 0x00021A04
		public Song(JObject jsonNode, bool scoreSaber)
		{
			if (scoreSaber)
			{
				this.scoreSaber = scoreSaber;
				this.ConstructFromScoreSaber(jsonNode);
				return;
			}
			this.levelAuthorName = (string)jsonNode["metadata"]["levelAuthorName"];
			this.songAuthorName = (string)jsonNode["metadata"]["songAuthorName"];
			this.songName = (string)jsonNode["metadata"]["songName"];
			this.songSubName = (string)jsonNode["metadata"]["songSubName"];
			this.bpm = (float)jsonNode["metadata"]["bpm"];
			this.duration = (float)jsonNode["metadata"]["duration"];
			this.downloads = (int)jsonNode["stats"]["downloads"];
			this.plays = (int)jsonNode["stats"]["plays"];
			this.upVotes = (int)jsonNode["stats"]["upVotes"];
			this.downVotes = (int)jsonNode["stats"]["downVotes"];
			this.rating = (float)jsonNode["stats"]["rating"];
			this.heat = (float)jsonNode["stats"]["heat"];
			this.description = (string)jsonNode["description"];
			this._id = (string)jsonNode["_id"];
			this.key = (string)jsonNode["key"];
			this.name = (string)jsonNode["name"];
			this.ownerid = (string)jsonNode["uploader"]["_id"];
			this.ownerName = (string)jsonNode["uploader"]["username"];
			this.hash = (string)jsonNode["hash"];
			this.hash = this.hash.ToLower();
			this.uploaded = (string)jsonNode["uploaded"];
			this.downloadURL = (string)jsonNode["downloadURL"];
			this.coverURL = (string)jsonNode["coverURL"];
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00023AB0 File Offset: 0x00021CB0
		public void ConstructFromScoreSaber(JObject jsonNode)
		{
			this._id = "";
			this.ownerid = "";
			this.downloads = 0;
			this.upVotes = 0;
			this.downVotes = 0;
			this.plays = 0;
			this.description = "";
			this.uploaded = "";
			this.rating = 0f;
			this.heat = 0f;
			this.key = "";
			this.name = "";
			this.ownerName = "";
			this.downloadURL = "";
			this.songName = (string)jsonNode["name"];
			this.songSubName = (string)jsonNode["songSubName"];
			this.levelAuthorName = (string)jsonNode["levelAuthorName"];
			this.songAuthorName = (string)jsonNode["songAuthorName"];
			this.bpm = (float)(int)jsonNode["bpm"];
			string beatSaverURL = PluginSetting.BeatSaverURL;
			JToken jtoken = jsonNode["image"];
			this.coverURL = beatSaverURL + ((jtoken != null) ? jtoken.ToString() : null);
			this.hash = (string)jsonNode["id"];
			this.hash = this.hash.ToLower();
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00023C04 File Offset: 0x00021E04
		public static Song FromSearchNode(JObject jsonNode)
		{
			Song song = new Song();
			song.levelAuthorName = (string)jsonNode["metadata"]["levelAuthorName"];
			song.songAuthorName = (string)jsonNode["metadata"]["songAuthorName"];
			song.songName = (string)jsonNode["metadata"]["songName"];
			song.songSubName = (string)jsonNode["metadata"]["songSubName"];
			song.bpm = (float)jsonNode["metadata"]["bpm"];
			song.downloads = (int)jsonNode["stats"]["downloads"];
			song.plays = (int)jsonNode["stats"]["plays"];
			song.upVotes = (int)jsonNode["stats"]["upVotes"];
			song.downVotes = (int)jsonNode["stats"]["downVotes"];
			song.rating = (float)jsonNode["stats"]["rating"];
			song.heat = (float)jsonNode["stats"]["heat"];
			song.description = (string)jsonNode["description"];
			song._id = (string)jsonNode["_id"];
			song.key = (string)jsonNode["key"];
			song.name = (string)jsonNode["name"];
			song.ownerid = (string)jsonNode["uploader"]["_id"];
			song.ownerName = (string)jsonNode["uploader"]["username"];
			song.hash = (string)jsonNode["hash"];
			song.hash = song.hash.ToLower();
			song.uploaded = (string)jsonNode["uploaded"];
			song.downloadURL = (string)jsonNode["downloadURL"];
			song.coverURL = (string)jsonNode["coverURL"];
			return song;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00023E7A File Offset: 0x0002207A
		public bool Compare(Song compareTo)
		{
			return compareTo.hash == this.hash;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00023E90 File Offset: 0x00022090
		public Song(CustomPreviewBeatmapLevel _data)
		{
			this.songName = _data.songName;
			this.songSubName = _data.songSubName;
			this.songAuthorName = _data.songAuthorName;
			this.levelAuthorName = _data.levelAuthorName;
			this.path = _data.customLevelPath;
			this.hash = Collections.hashForLevelID(_data.levelID).ToLower();
		}

		// Token: 0x04000429 RID: 1065
		public string levelAuthorName;

		// Token: 0x0400042A RID: 1066
		public string songAuthorName;

		// Token: 0x0400042B RID: 1067
		public string songName;

		// Token: 0x0400042C RID: 1068
		public string songSubName;

		// Token: 0x0400042D RID: 1069
		public float bpm;

		// Token: 0x0400042E RID: 1070
		public float duration;

		// Token: 0x0400042F RID: 1071
		public int downloads;

		// Token: 0x04000430 RID: 1072
		public int plays;

		// Token: 0x04000431 RID: 1073
		public int upVotes;

		// Token: 0x04000432 RID: 1074
		public int downVotes;

		// Token: 0x04000433 RID: 1075
		public float rating;

		// Token: 0x04000434 RID: 1076
		public float heat;

		// Token: 0x04000435 RID: 1077
		public string description;

		// Token: 0x04000436 RID: 1078
		public string _id;

		// Token: 0x04000437 RID: 1079
		public string key;

		// Token: 0x04000438 RID: 1080
		public string name;

		// Token: 0x04000439 RID: 1081
		public string ownerid;

		// Token: 0x0400043A RID: 1082
		public string ownerName;

		// Token: 0x0400043B RID: 1083
		public string hash;

		// Token: 0x0400043C RID: 1084
		public string uploaded;

		// Token: 0x0400043D RID: 1085
		public string downloadURL;

		// Token: 0x0400043E RID: 1086
		public string coverURL;

		// Token: 0x0400043F RID: 1087
		public string img;

		// Token: 0x04000440 RID: 1088
		public string path;

		// Token: 0x04000441 RID: 1089
		public bool scoreSaber;

		// Token: 0x04000442 RID: 1090
		public SongQueueState songQueueState;

		// Token: 0x04000443 RID: 1091
		public float downloadingProgress;
	}
}
