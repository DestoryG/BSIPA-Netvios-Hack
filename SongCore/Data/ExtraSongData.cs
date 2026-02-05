using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IPA.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore.Data
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public class ExtraSongData
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x000020C2 File Offset: 0x000002C2
		public ExtraSongData()
		{
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008826 File Offset: 0x00006A26
		[JsonConstructor]
		public ExtraSongData(string levelID, ExtraSongData.Contributor[] contributors, string customEnvironmentName, string customEnvironmentHash, ExtraSongData.DifficultyData[] difficulties)
		{
			this.contributors = contributors;
			this._customEnvironmentName = customEnvironmentName;
			this._customEnvironmentHash = customEnvironmentHash;
			this._difficulties = difficulties;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000884C File Offset: 0x00006A4C
		public ExtraSongData(string levelID, string songPath)
		{
			try
			{
				if (File.Exists(songPath + "/info.dat"))
				{
					JObject jobject = JObject.Parse(File.ReadAllText(songPath + "/info.dat"));
					List<ExtraSongData.Contributor> list = new List<ExtraSongData.Contributor>();
					if (jobject.ContainsKey("_customData"))
					{
						JObject jobject2 = (JObject)jobject["_customData"];
						if (jobject2.ContainsKey("_contributors"))
						{
							list.AddRange(jobject2["_contributors"].ToObject<ExtraSongData.Contributor[]>());
						}
						if (jobject2.ContainsKey("_customEnvironment"))
						{
							this._customEnvironmentName = (string)jobject2["_customEnvironment"];
						}
						if (jobject2.ContainsKey("_customEnvironmentHash"))
						{
							this._customEnvironmentHash = (string)jobject2["_customEnvironmentHash"];
						}
						if (jobject2.ContainsKey("_defaultCharacteristic"))
						{
							this._defaultCharacteristic = (string)jobject2["_defaultCharacteristic"];
						}
					}
					this.contributors = list.ToArray();
					List<ExtraSongData.DifficultyData> list2 = new List<ExtraSongData.DifficultyData>();
					foreach (JToken jtoken in ((JArray)jobject["_difficultyBeatmapSets"]))
					{
						JObject jobject3 = (JObject)jtoken;
						string text = (string)jobject3["_beatmapCharacteristicName"];
						foreach (JToken jtoken2 in ((JArray)jobject3["_difficultyBeatmaps"]))
						{
							JObject jobject4 = (JObject)jtoken2;
							List<string> list3 = new List<string>();
							List<string> list4 = new List<string>();
							List<string> list5 = new List<string>();
							List<string> list6 = new List<string>();
							string text2 = "";
							ExtraSongData.MapColor mapColor = null;
							ExtraSongData.MapColor mapColor2 = null;
							ExtraSongData.MapColor mapColor3 = null;
							ExtraSongData.MapColor mapColor4 = null;
							ExtraSongData.MapColor mapColor5 = null;
							BeatmapDifficulty beatmapDifficulty = ((string)jobject4["_difficulty"]).ToEnum(1);
							if (jobject4.ContainsKey("_customData"))
							{
								JObject jobject5 = (JObject)jobject4["_customData"];
								if (jobject5.ContainsKey("_difficultyLabel"))
								{
									text2 = (string)jobject5["_difficultyLabel"];
								}
								if (jobject5.ContainsKey("_colorLeft") && jobject5["_colorLeft"].Children().Count<JToken>() == 3)
								{
									mapColor = new ExtraSongData.MapColor(0f, 0f, 0f);
									mapColor.r = (float)jobject5["_colorLeft"]["r"];
									mapColor.g = (float)jobject5["_colorLeft"]["g"];
									mapColor.b = (float)jobject5["_colorLeft"]["b"];
								}
								if (jobject5.ContainsKey("_colorRight") && jobject5["_colorRight"].Children().Count<JToken>() == 3)
								{
									mapColor2 = new ExtraSongData.MapColor(0f, 0f, 0f);
									mapColor2.r = (float)jobject5["_colorRight"]["r"];
									mapColor2.g = (float)jobject5["_colorRight"]["g"];
									mapColor2.b = (float)jobject5["_colorRight"]["b"];
								}
								if (jobject5.ContainsKey("_envColorLeft") && jobject5["_envColorLeft"].Children().Count<JToken>() == 3)
								{
									mapColor3 = new ExtraSongData.MapColor(0f, 0f, 0f);
									mapColor3.r = (float)jobject5["_envColorLeft"]["r"];
									mapColor3.g = (float)jobject5["_envColorLeft"]["g"];
									mapColor3.b = (float)jobject5["_envColorLeft"]["b"];
								}
								if (jobject5.ContainsKey("_envColorRight") && jobject5["_envColorRight"].Children().Count<JToken>() == 3)
								{
									mapColor4 = new ExtraSongData.MapColor(0f, 0f, 0f);
									mapColor4.r = (float)jobject5["_envColorRight"]["r"];
									mapColor4.g = (float)jobject5["_envColorRight"]["g"];
									mapColor4.b = (float)jobject5["_envColorRight"]["b"];
								}
								if (jobject5.ContainsKey("_obstacleColor") && jobject5["_obstacleColor"].Children().Count<JToken>() == 3)
								{
									mapColor5 = new ExtraSongData.MapColor(0f, 0f, 0f);
									mapColor5.r = (float)jobject5["_obstacleColor"]["r"];
									mapColor5.g = (float)jobject5["_obstacleColor"]["g"];
									mapColor5.b = (float)jobject5["_obstacleColor"]["b"];
								}
								if (jobject5.ContainsKey("_warnings"))
								{
									list5.AddRange(((JArray)jobject5["_warnings"]).Select((JToken c) => (string)c));
								}
								if (jobject5.ContainsKey("_information"))
								{
									list6.AddRange(((JArray)jobject5["_information"]).Select((JToken c) => (string)c));
								}
								if (jobject5.ContainsKey("_suggestions"))
								{
									list4.AddRange(((JArray)jobject5["_suggestions"]).Select((JToken c) => (string)c));
								}
								if (jobject5.ContainsKey("_requirements"))
								{
									list3.AddRange(((JArray)jobject5["_requirements"]).Select((JToken c) => (string)c));
								}
							}
							ExtraSongData.RequirementData requirementData = new ExtraSongData.RequirementData
							{
								_requirements = list3.ToArray(),
								_suggestions = list4.ToArray(),
								_information = list6.ToArray(),
								_warnings = list5.ToArray()
							};
							list2.Add(new ExtraSongData.DifficultyData
							{
								_beatmapCharacteristicName = text,
								_difficulty = beatmapDifficulty,
								_difficultyLabel = text2,
								additionalDifficultyData = requirementData,
								_colorLeft = mapColor,
								_colorRight = mapColor2,
								_envColorLeft = mapColor3,
								_envColorRight = mapColor4,
								_obstacleColor = mapColor5
							});
						}
					}
					this._difficulties = list2.ToArray();
				}
			}
			catch (Exception ex)
			{
				Logging.Log(string.Format("Error in Level {0}: \n {1}", songPath, ex), Logger.Level.Error);
			}
		}

		// Token: 0x040000A7 RID: 167
		public ExtraSongData.Contributor[] contributors;

		// Token: 0x040000A8 RID: 168
		public string _customEnvironmentName;

		// Token: 0x040000A9 RID: 169
		public string _customEnvironmentHash;

		// Token: 0x040000AA RID: 170
		public ExtraSongData.DifficultyData[] _difficulties;

		// Token: 0x040000AB RID: 171
		public string _defaultCharacteristic;

		// Token: 0x0200005C RID: 92
		[Serializable]
		public class Contributor
		{
			// Token: 0x04000139 RID: 313
			public string _role;

			// Token: 0x0400013A RID: 314
			public string _name;

			// Token: 0x0400013B RID: 315
			public string _iconPath;

			// Token: 0x0400013C RID: 316
			[NonSerialized]
			public Sprite icon;
		}

		// Token: 0x0200005D RID: 93
		[Serializable]
		public class DifficultyData
		{
			// Token: 0x0400013D RID: 317
			public string _beatmapCharacteristicName;

			// Token: 0x0400013E RID: 318
			public BeatmapDifficulty _difficulty;

			// Token: 0x0400013F RID: 319
			public string _difficultyLabel;

			// Token: 0x04000140 RID: 320
			public ExtraSongData.RequirementData additionalDifficultyData;

			// Token: 0x04000141 RID: 321
			public ExtraSongData.MapColor _colorLeft;

			// Token: 0x04000142 RID: 322
			public ExtraSongData.MapColor _colorRight;

			// Token: 0x04000143 RID: 323
			public ExtraSongData.MapColor _envColorLeft;

			// Token: 0x04000144 RID: 324
			public ExtraSongData.MapColor _envColorRight;

			// Token: 0x04000145 RID: 325
			public ExtraSongData.MapColor _obstacleColor;
		}

		// Token: 0x0200005E RID: 94
		[Serializable]
		public class RequirementData
		{
			// Token: 0x04000146 RID: 326
			public string[] _requirements;

			// Token: 0x04000147 RID: 327
			public string[] _suggestions;

			// Token: 0x04000148 RID: 328
			public string[] _warnings;

			// Token: 0x04000149 RID: 329
			public string[] _information;
		}

		// Token: 0x0200005F RID: 95
		[Serializable]
		public class MapColor
		{
			// Token: 0x06000283 RID: 643 RVA: 0x0000B881 File Offset: 0x00009A81
			public MapColor(float r, float g, float b)
			{
				this.r = r;
				this.g = g;
				this.b = b;
			}

			// Token: 0x0400014A RID: 330
			public float r;

			// Token: 0x0400014B RID: 331
			public float g;

			// Token: 0x0400014C RID: 332
			public float b;
		}
	}
}
