using System;
using System.Collections.Generic;
using NetViosCommon.Utility;
using Newtonsoft.Json;
using PlayerDataPlugin.BSHandler;
using UnityEngine;

namespace PlayerDataPlugin
{
	// Token: 0x02000006 RID: 6
	internal class Player : Singleton<Player>
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002257 File Offset: 0x00000457
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002248 File Offset: 0x00000448
		public string SdkData
		{
			get
			{
				return this.sdkDataInJson;
			}
			set
			{
				this.sdkDataInJson = value;
				this.ParseSDKData();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000225F File Offset: 0x0000045F
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002267 File Offset: 0x00000467
		public bool IsAnonymous { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002279 File Offset: 0x00000479
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002270 File Offset: 0x00000470
		public string WxUnionID { get; private set; } = "";

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000228A File Offset: 0x0000048A
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002281 File Offset: 0x00000481
		public string GameExtraData { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000229B File Offset: 0x0000049B
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002292 File Offset: 0x00000492
		public string Token { get; private set; } = "";

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000022AC File Offset: 0x000004AC
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000022A3 File Offset: 0x000004A3
		public string ID { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000022BD File Offset: 0x000004BD
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000022B4 File Offset: 0x000004B4
		public ChannelEnum Channel { get; private set; }

		// Token: 0x06000026 RID: 38 RVA: 0x00002078 File Offset: 0x00000278
		public void Init()
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000022C5 File Offset: 0x000004C5
		public string GetSDKJson()
		{
			return this.SdkData;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000022D0 File Offset: 0x000004D0
		private void ParseSDKData()
		{
			Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(this.SdkData);
			object obj = dictionary[Const.APP_CHANNEL];
			string text = ((obj != null) ? obj.ToString() : null);
			dictionary.Remove(Const.APP_CHANNEL);
			try
			{
				ChannelEnum channelEnum = ChannelEnum.unknown;
				Enum.TryParse<ChannelEnum>(text, out channelEnum);
				this.Channel = channelEnum;
			}
			catch (Exception)
			{
				this.Channel = ChannelEnum.unknown;
			}
			if (this.Channel == ChannelEnum.dianxin)
			{
				if (dictionary.ContainsKey(Const.CLOUD_TOKEN))
				{
					this.Token = dictionary[Const.CLOUD_TOKEN].ToString();
				}
				this.ID = this.Token;
				this.IsAnonymous = string.IsNullOrEmpty(this.ID);
				return;
			}
			if (this.Channel == ChannelEnum.netvios)
			{
				if (dictionary.ContainsKey(Const.NETVIOS_UNIONID))
				{
					this.WxUnionID = dictionary[Const.NETVIOS_UNIONID].ToString();
				}
				this.ID = this.WxUnionID;
				this.IsAnonymous = string.IsNullOrEmpty(this.ID);
				this.GameExtraData = dictionary[Const.NETVIOS_EXTRA_DATA].ToString();
				return;
			}
			if (this.Channel == ChannelEnum.yidong)
			{
				Application.Quit();
				return;
			}
			Application.Quit();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000023F8 File Offset: 0x000005F8
		public void FillPlayerInfoData(ref Dictionary<string, object> dict)
		{
			dict[Const.APP_CHANNEL] = this.Channel.ToString();
			ChannelEnum channel = this.Channel;
			if (this.Channel == ChannelEnum.netvios)
			{
				dict[Const.NETVIOS_UNIONID] = this.WxUnionID;
				dict[Const.NETVIOS_EXTRA_DATA] = this.GameExtraData;
			}
			if (this.Channel == ChannelEnum.yidong)
			{
				Application.Quit();
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002468 File Offset: 0x00000668
		public void OnUpdate(float deltaTime)
		{
			this.mTime += deltaTime;
			if (this.mTime >= 1f)
			{
				this.mTime = 0f;
				Saber[] array = Resources.FindObjectsOfTypeAll<Saber>();
				for (int i = 0; i < array.Length; i++)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["handlePos"] = array[i].handlePos.ToString();
					dictionary["handleRot"] = array[i].handleRot.ToString();
					dictionary["saberBladeTopPos"] = array[i].saberBladeTopPos.ToString();
					dictionary["saberBladeBottomPos"] = array[i].saberBladeBottomPos.ToString();
					Singleton<DataStore>.Instance.SavePlayerActionRecord(dictionary);
				}
			}
		}

		// Token: 0x04000007 RID: 7
		private float mTime;

		// Token: 0x04000008 RID: 8
		private const float INTERVAL = 1f;

		// Token: 0x04000009 RID: 9
		private string sdkDataInJson;
	}
}
