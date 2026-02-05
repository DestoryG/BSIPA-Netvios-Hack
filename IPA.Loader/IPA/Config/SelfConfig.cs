using System;
using System.Collections.Generic;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using IPA.Logging;
using Newtonsoft.Json;

namespace IPA.Config
{
	// Token: 0x02000060 RID: 96
	internal class SelfConfig
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000D712 File Offset: 0x0000B912
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000D719 File Offset: 0x0000B919
		public static Config LoaderConfig { get; set; }

		// Token: 0x060002AC RID: 684 RVA: 0x0000D721 File Offset: 0x0000B921
		public static void Load()
		{
			SelfConfig.LoaderConfig = Config.GetConfigFor("Beat Saber IPA", new string[] { "json" });
			SelfConfig.Instance = SelfConfig.LoaderConfig.Generated(true);
			SelfConfig.Instance.OnReload();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000D75A File Offset: 0x0000B95A
		protected virtual void CopyFrom(SelfConfig cfg)
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000D75C File Offset: 0x0000B95C
		protected internal virtual void OnReload()
		{
			if (this.Regenerate)
			{
				this.CopyFrom(new SelfConfig
				{
					Regenerate = false
				});
			}
			StandardLogger.Configure();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000D77D File Offset: 0x0000B97D
		protected internal virtual void Changed()
		{
			Logger.log.Debug("SelfConfig Changed called");
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000D790 File Offset: 0x0000B990
		public static void ReadCommandLine(string[] args)
		{
			foreach (string arg in args)
			{
				if (arg != null)
				{
					if (!(arg == "--debug") && !(arg == "--mono-debug"))
					{
						if (!(arg == "--no-yeet"))
						{
							if (!(arg == "--condense-logs"))
							{
								if (!(arg == "--no-updates"))
								{
									if (arg == "--trace")
									{
										SelfConfig.CommandLineValues.Debug.ShowTrace = true;
									}
								}
								else
								{
									SelfConfig.CommandLineValues.Updates.AutoCheckUpdates = false;
									SelfConfig.CommandLineValues.Updates.AutoUpdate = false;
								}
							}
							else
							{
								SelfConfig.CommandLineValues.Debug.CondenseModLogs = true;
							}
						}
						else
						{
							SelfConfig.CommandLineValues.YeetMods = false;
						}
					}
					else
					{
						SelfConfig.CommandLineValues.Debug.ShowDebug = true;
						SelfConfig.CommandLineValues.Debug.ShowCallSource = true;
					}
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000D880 File Offset: 0x0000BA80
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000D888 File Offset: 0x0000BA88
		public virtual bool Regenerate { get; set; } = true;

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000D891 File Offset: 0x0000BA91
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000D899 File Offset: 0x0000BA99
		[NonNullable]
		public virtual SelfConfig.Updates_ Updates { get; set; } = new SelfConfig.Updates_();

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000D8A2 File Offset: 0x0000BAA2
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000D8AA File Offset: 0x0000BAAA
		[NonNullable]
		public virtual SelfConfig.Debug_ Debug { get; set; } = new SelfConfig.Debug_();

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000D8B3 File Offset: 0x0000BAB3
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000D8BB File Offset: 0x0000BABB
		public virtual bool YeetMods { get; set; } = true;

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000D8C4 File Offset: 0x0000BAC4
		public static bool YeetMods_
		{
			get
			{
				SelfConfig instance = SelfConfig.Instance;
				return (instance == null || instance.YeetMods) && SelfConfig.CommandLineValues.YeetMods;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000D8E5 File Offset: 0x0000BAE5
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000D8ED File Offset: 0x0000BAED
		[NonNullable]
		[UseConverter(typeof(CollectionConverter<string, HashSet<string>>))]
		public virtual HashSet<string> GameAssemblies { get; set; } = new HashSet<string> { "Main.dll", "Core.dll", "HMLib.dll", "HMUI.dll", "HMRendering.dll", "VRUI.dll" };

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000D8F6 File Offset: 0x0000BAF6
		public static HashSet<string> GameAssemblies_
		{
			get
			{
				SelfConfig instance = SelfConfig.Instance;
				HashSet<string> hashSet;
				if ((hashSet = ((instance != null) ? instance.GameAssemblies : null)) == null)
				{
					(hashSet = new HashSet<string>()).Add("Assembly-CSharp.dll");
				}
				return hashSet;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000D91E File Offset: 0x0000BB1E
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000D926 File Offset: 0x0000BB26
		[JsonProperty(Required = Required.DisallowNull)]
		public virtual string LastGameVersion { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000D92F File Offset: 0x0000BB2F
		public static string LastGameVersion_
		{
			get
			{
				SelfConfig instance = SelfConfig.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.LastGameVersion;
			}
		}

		// Token: 0x040000FE RID: 254
		public static SelfConfig Instance = new SelfConfig();

		// Token: 0x040000FF RID: 255
		internal const string IPAName = "Beat Saber IPA";

		// Token: 0x04000100 RID: 256
		internal const string IPAVersion = "4.0.6.0";

		// Token: 0x04000101 RID: 257
		internal static SelfConfig CommandLineValues = new SelfConfig();

		// Token: 0x02000126 RID: 294
		public class Updates_
		{
			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000175A8 File Offset: 0x000157A8
			// (set) Token: 0x060005C6 RID: 1478 RVA: 0x000175B0 File Offset: 0x000157B0
			public virtual bool AutoUpdate { get; set; }

			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000175BC File Offset: 0x000157BC
			public static bool AutoUpdate_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Updates_ updates = instance.Updates;
						flag = ((updates != null) ? new bool?(updates.AutoUpdate) : null);
					}
					return (flag ?? true) && SelfConfig.CommandLineValues.Updates.AutoUpdate;
				}
			}

			// Token: 0x170000D6 RID: 214
			// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00017622 File Offset: 0x00015822
			// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0001762A File Offset: 0x0001582A
			public virtual bool AutoCheckUpdates { get; set; }

			// Token: 0x170000D7 RID: 215
			// (get) Token: 0x060005CA RID: 1482 RVA: 0x00017634 File Offset: 0x00015834
			public static bool AutoCheckUpdates_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Updates_ updates = instance.Updates;
						flag = ((updates != null) ? new bool?(updates.AutoCheckUpdates) : null);
					}
					return (flag ?? true) && SelfConfig.CommandLineValues.Updates.AutoCheckUpdates;
				}
			}
		}

		// Token: 0x02000127 RID: 295
		public class Debug_
		{
			// Token: 0x170000D8 RID: 216
			// (get) Token: 0x060005CC RID: 1484 RVA: 0x000176A2 File Offset: 0x000158A2
			// (set) Token: 0x060005CD RID: 1485 RVA: 0x000176AA File Offset: 0x000158AA
			public virtual bool ShowCallSource { get; set; }

			// Token: 0x170000D9 RID: 217
			// (get) Token: 0x060005CE RID: 1486 RVA: 0x000176B4 File Offset: 0x000158B4
			public static bool ShowCallSource_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						flag = ((debug != null) ? new bool?(debug.ShowCallSource) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault() || SelfConfig.CommandLineValues.Debug.ShowCallSource;
				}
			}

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001770E File Offset: 0x0001590E
			// (set) Token: 0x060005D0 RID: 1488 RVA: 0x00017716 File Offset: 0x00015916
			public virtual bool ShowDebug { get; set; }

			// Token: 0x170000DB RID: 219
			// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00017720 File Offset: 0x00015920
			public static bool ShowDebug_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						flag = ((debug != null) ? new bool?(debug.ShowDebug) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault() || SelfConfig.CommandLineValues.Debug.ShowDebug;
				}
			}

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001777A File Offset: 0x0001597A
			// (set) Token: 0x060005D3 RID: 1491 RVA: 0x00017782 File Offset: 0x00015982
			public virtual bool CondenseModLogs { get; set; }

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001778C File Offset: 0x0001598C
			public static bool CondenseModLogs_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						flag = ((debug != null) ? new bool?(debug.CondenseModLogs) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault() || SelfConfig.CommandLineValues.Debug.CondenseModLogs;
				}
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x060005D5 RID: 1493 RVA: 0x000177E6 File Offset: 0x000159E6
			// (set) Token: 0x060005D6 RID: 1494 RVA: 0x000177EE File Offset: 0x000159EE
			public virtual bool ShowHandledErrorStackTraces { get; set; }

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x060005D7 RID: 1495 RVA: 0x000177F8 File Offset: 0x000159F8
			public static bool ShowHandledErrorStackTraces_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						flag = ((debug != null) ? new bool?(debug.ShowHandledErrorStackTraces) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault();
				}
			}

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001783F File Offset: 0x00015A3F
			// (set) Token: 0x060005D9 RID: 1497 RVA: 0x00017847 File Offset: 0x00015A47
			public virtual bool HideMessagesForPerformance { get; set; } = true;

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x060005DA RID: 1498 RVA: 0x00017850 File Offset: 0x00015A50
			public static bool HideMessagesForPerformance_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						flag = ((debug != null) ? new bool?(debug.HideMessagesForPerformance) : null);
					}
					return flag ?? true;
				}
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x060005DB RID: 1499 RVA: 0x000178A2 File Offset: 0x00015AA2
			// (set) Token: 0x060005DC RID: 1500 RVA: 0x000178AA File Offset: 0x00015AAA
			public virtual int HideLogThreshold { get; set; } = 512;

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x060005DD RID: 1501 RVA: 0x000178B4 File Offset: 0x00015AB4
			public static int HideLogThreshold_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					int? num;
					if (instance == null)
					{
						num = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						num = ((debug != null) ? new int?(debug.HideLogThreshold) : null);
					}
					int? num2 = num;
					if (num2 == null)
					{
						return 512;
					}
					return num2.GetValueOrDefault();
				}
			}

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001790A File Offset: 0x00015B0A
			// (set) Token: 0x060005DF RID: 1503 RVA: 0x00017912 File Offset: 0x00015B12
			public virtual bool ShowTrace { get; set; }

			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001791C File Offset: 0x00015B1C
			public static bool ShowTrace_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						flag = ((debug != null) ? new bool?(debug.ShowTrace) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault() || SelfConfig.CommandLineValues.Debug.ShowTrace;
				}
			}

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00017976 File Offset: 0x00015B76
			// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0001797E File Offset: 0x00015B7E
			public virtual bool SyncLogging { get; set; }

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00017988 File Offset: 0x00015B88
			public static bool SyncLogging_
			{
				get
				{
					SelfConfig instance = SelfConfig.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						SelfConfig.Debug_ debug = instance.Debug;
						flag = ((debug != null) ? new bool?(debug.SyncLogging) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault();
				}
			}
		}
	}
}
