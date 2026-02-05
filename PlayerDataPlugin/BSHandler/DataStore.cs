using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NetViosCommon.Utility;
using Newtonsoft.Json;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x02000013 RID: 19
	internal class DataStore : Singleton<DataStore>
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003203 File Offset: 0x00001403
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000031FA File Offset: 0x000013FA
		public int LastModifiedScore { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003214 File Offset: 0x00001414
		// (set) Token: 0x0600008B RID: 139 RVA: 0x0000320B File Offset: 0x0000140B
		public bool SongDidFinish { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003225 File Offset: 0x00001425
		// (set) Token: 0x0600008D RID: 141 RVA: 0x0000321C File Offset: 0x0000141C
		public string LevelID { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003236 File Offset: 0x00001436
		// (set) Token: 0x0600008F RID: 143 RVA: 0x0000322D File Offset: 0x0000142D
		public string LevelName { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003247 File Offset: 0x00001447
		// (set) Token: 0x06000091 RID: 145 RVA: 0x0000323E File Offset: 0x0000143E
		public string LevelAuthorName { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003258 File Offset: 0x00001458
		// (set) Token: 0x06000093 RID: 147 RVA: 0x0000324F File Offset: 0x0000144F
		public string SongAuthorName { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003269 File Offset: 0x00001469
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00003260 File Offset: 0x00001460
		public float LevelDuration { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000327A File Offset: 0x0000147A
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003271 File Offset: 0x00001471
		public float LevelBPM { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009A RID: 154 RVA: 0x0000328B File Offset: 0x0000148B
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003282 File Offset: 0x00001482
		public string Difficulty { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009C RID: 156 RVA: 0x0000329C File Offset: 0x0000149C
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003293 File Offset: 0x00001493
		public LevelCompletionResults Results { get; private set; }

		// Token: 0x0600009D RID: 157 RVA: 0x00002078 File Offset: 0x00000278
		public void Init()
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000032A4 File Offset: 0x000014A4
		public void UploadZipData()
		{
			if (Singleton<Player>.Instance.IsAnonymous)
			{
				return;
			}
			Recorder recorder = this.mRecorder;
			if (recorder == null)
			{
				return;
			}
			recorder.ZipDataAndUpload(this.LastModifiedScore);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000032C9 File Offset: 0x000014C9
		public void Restore()
		{
			this.ClearGameData();
			if (Singleton<Player>.Instance.IsAnonymous)
			{
				return;
			}
			this.mRecorder = new Recorder();
			this.mRecorder.Init();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000032F4 File Offset: 0x000014F4
		public void DisposeRecord()
		{
			if (Singleton<Player>.Instance.IsAnonymous)
			{
				return;
			}
			this.mRecorder.Close();
			this.mRecorder = null;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003318 File Offset: 0x00001518
		public void ClearGameData()
		{
			this.SongDidFinish = false;
			this.LevelID = "";
			this.LevelBPM = 0f;
			this.LevelName = "";
			this.LevelAuthorName = "";
			this.SongAuthorName = "";
			this.LevelDuration = 0f;
			this.Results = null;
			this.LastModifiedScore = -1;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000337C File Offset: 0x0000157C
		public void SavePlayerActionRecord(Dictionary<string, object> dict)
		{
			if (Singleton<Player>.Instance.IsAnonymous)
			{
				return;
			}
			string text = JsonConvert.SerializeObject(dict);
			this.mRecorder.Write(text);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000033A9 File Offset: 0x000015A9
		public void SetLastModifiedScoreForActionRecord(int score)
		{
			if (Singleton<Player>.Instance.IsAnonymous)
			{
				return;
			}
			this.LastModifiedScore = score;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000033C0 File Offset: 0x000015C0
		public void SaveScoreDataForActionRecord(int rawScore, int modifiedScore)
		{
			if (Singleton<Player>.Instance.IsAnonymous)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["rawScore"] = rawScore.ToString();
			dictionary["modifiedScore"] = modifiedScore.ToString();
			string text = JsonConvert.SerializeObject(dictionary);
			Recorder recorder = this.mRecorder;
			if (recorder == null)
			{
				return;
			}
			recorder.Write(text);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000341C File Offset: 0x0000161C
		public void SetSongData(IDifficultyBeatmap difficultyBeatmap)
		{
			if (difficultyBeatmap == null)
			{
				Logger.log.Error("[SetLevelID] beatmap is NULL");
				return;
			}
			this.LevelID = difficultyBeatmap.level.levelID.Replace("custom_level_", "");
			this.LevelBPM = difficultyBeatmap.level.beatsPerMinute;
			this.Difficulty = difficultyBeatmap.difficulty.ToString();
			this.LevelName = difficultyBeatmap.level.songName;
			this.LevelAuthorName = difficultyBeatmap.level.levelAuthorName;
			this.SongAuthorName = difficultyBeatmap.level.songAuthorName;
			this.LevelDuration = difficultyBeatmap.level.songDuration;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000034CD File Offset: 0x000016CD
		public void SetSongDidFinish(bool value)
		{
			this.SongDidFinish = value;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000034D6 File Offset: 0x000016D6
		public void SetLevelCompletionResults(LevelCompletionResults results)
		{
			if (results == null)
			{
				Logger.log.Error("LevelCompletionResults is NULL");
			}
			this.Results = results;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000034F4 File Offset: 0x000016F4
		private static string MakeSign(Dictionary<string, object> dict)
		{
			byte[] bytes = Encoding.ASCII.GetBytes("522b103fa1fd4f3798b4dd5387513da5");
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			List<Tuple<string, object>> list = new List<Tuple<string, object>>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<Tuple<string, object>> list4 = new List<Tuple<string, object>>();
			List<string> list5 = new List<string>();
			list3.Add("appkey");
			list3.Add("game_data");
			list3.Add("sign");
			foreach (string text5 in dict.Keys)
			{
				if (!list3.Contains(text5))
				{
					list2.Add(text5);
				}
			}
			list2.Sort();
			foreach (string text6 in list2)
			{
				list.Add(new Tuple<string, object>(text6, dict[text6]));
			}
			Dictionary<string, object> dictionary = dict["game_data"] as Dictionary<string, object>;
			foreach (string text7 in dictionary.Keys)
			{
				list5.Add(text7);
			}
			list5.Sort();
			foreach (string text8 in list5)
			{
				list4.Add(new Tuple<string, object>(text8, dictionary[text8]));
			}
			string text9 = text;
			string item = list[0].Item1;
			string text10 = "=";
			object item2 = list[0].Item2;
			text = text9 + item + text10 + ((item2 != null) ? item2.ToString() : null);
			for (int i = 1; i < list.Count; i++)
			{
				string[] array = new string[5];
				array[0] = text;
				array[1] = "&";
				array[2] = list[i].Item1;
				array[3] = "=";
				int num = 4;
				object item3 = list[i].Item2;
				array[num] = ((item3 != null) ? item3.ToString() : null);
				text = string.Concat(array);
			}
			string text11 = text2;
			string item4 = list4[0].Item1;
			string text12 = "=";
			object item5 = list4[0].Item2;
			text2 = text11 + item4 + text12 + ((item5 != null) ? item5.ToString() : null);
			for (int j = 1; j < list4.Count; j++)
			{
				string[] array2 = new string[5];
				array2[0] = text2;
				array2[1] = "&";
				array2[2] = list4[j].Item1;
				array2[3] = "=";
				int num2 = 4;
				object item6 = list4[j].Item2;
				array2[num2] = ((item6 != null) ? item6.ToString() : null);
				text2 = string.Concat(array2);
			}
			text3 = string.Concat(new string[] { text3, text, "&", text2, "&key=522b103fa1fd4f3798b4dd5387513da5" });
			using (HMACSHA256 hmacsha = new HMACSHA256(bytes))
			{
				text4 = BitConverter.ToString(hmacsha.ComputeHash(Encoding.ASCII.GetBytes(text3))).Replace("-", "").ToLower();
			}
			Logger.log.Debug("Result: " + text4);
			return text4;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003888 File Offset: 0x00001A88
		public async void UploadDataAsync()
		{
			if (this.LevelID.EndsWith(" WIP"))
			{
				Logger.log.Info("=wip song=");
			}
			else
			{
				string text = "";
				string text2 = "";
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				if (this.Results == null)
				{
					Logger.log.Error("[UploadDataAsync] Results is null!");
				}
				else
				{
					dictionary2["maxCombo"] = this.Results.maxCombo;
					dictionary2["rawScore"] = this.Results.rawScore;
					dictionary2["modifiedScore"] = this.Results.modifiedScore;
					dictionary2["rank"] = RankModel.GetRankName(this.Results.rank);
					dictionary2["endSongTime"] = (int)(this.Results.endSongTime * 1000f);
					dictionary2["levelEndStateType"] = this.Results.levelEndStateType.ToString();
					dictionary2["missedCount"] = this.Results.missedCount;
					dictionary2["goodCutsCount"] = this.Results.goodCutsCount;
					dictionary2["notGoodCount"] = this.Results.notGoodCount;
					dictionary2["badCutsCount"] = this.Results.badCutsCount;
					dictionary2["okCount"] = this.Results.okCount;
					dictionary2["songDidFinish"] = (this.SongDidFinish ? "true" : "false");
					dictionary2["songDuration"] = (int)(this.Results.songDuration * 1000f);
					dictionary2["leftHandMovementDistance"] = (int)(this.Results.leftHandMovementDistance * 100f);
					dictionary2["rightHandMovementDistance"] = (int)(this.Results.rightHandMovementDistance * 100f);
					dictionary2["leftSaberMovementDistance"] = (int)(this.Results.leftSaberMovementDistance * 100f);
					dictionary2["rightSaberMovementDistance"] = (int)(this.Results.rightSaberMovementDistance * 100f);
					dictionary2["levelId"] = this.LevelID;
					dictionary2["levelBPM"] = (int)this.LevelBPM;
					dictionary2["difficulty"] = this.Difficulty;
					dictionary["game_data"] = dictionary2;
					dictionary["app"] = "beatsaber";
					dictionary["appkey"] = "e46905222c21b572";
					Singleton<Player>.Instance.FillPlayerInfoData(ref dictionary);
					dictionary["sign"] = DataStore.MakeSign(dictionary);
					text2 = JsonConvert.SerializeObject(dictionary);
					using (Http http = new Http())
					{
						try
						{
							string text3 = Const.BaseUrl;
							if (Singleton<Player>.Instance.Channel == ChannelEnum.netvios)
							{
								text3 += "/sync/game/play";
							}
							else
							{
								text3 += "/beatsaber/play";
								http.Headers["token"] = Singleton<Player>.Instance.Token;
							}
							text = await http.UploadStringTaskAsync(new Uri(text3), "POST", text2);
						}
						catch (Exception ex)
						{
							Logger.log.Error("UpdateDataAsync Exception:" + ex.ToString());
							return;
						}
					}
					Http http = null;
					Logger.log.Info("UploadDataAsync Result:" + text);
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000038C0 File Offset: 0x00001AC0
		public void RecordSongInfo()
		{
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["levelId"] = this.LevelID;
				dictionary["levelName"] = this.LevelName;
				dictionary["levelAuthorName"] = this.LevelAuthorName;
				dictionary["songAuthorName"] = this.SongAuthorName;
				dictionary["levelDuration"] = this.LevelDuration;
				dictionary["levelBpm"] = this.LevelBPM;
				Singleton<SongRecorder>.Instance.Init().Write(dictionary).Close();
			}
			catch (Exception ex)
			{
				Logger.log.Error("record song error: " + ex.Message);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003988 File Offset: 0x00001B88
		public void Close()
		{
			this.DisposeRecord();
		}

		// Token: 0x04000032 RID: 50
		private Recorder mRecorder;
	}
}
