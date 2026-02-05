using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMultiplayer.Configuration;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.UI.ViewControllers.InGame;
using HMUI;
using UnityEngine;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000062 RID: 98
	internal class VoipSettingViewController : BSMLResourceViewController
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0001C5DE File Offset: 0x0001A7DE
		public override string ResourceName
		{
			get
			{
				return string.Join(".", new string[]
				{
					base.GetType().Namespace,
					base.GetType().Name,
					"bsml"
				});
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600078A RID: 1930 RVA: 0x0001F95C File Offset: 0x0001DB5C
		// (remove) Token: 0x0600078B RID: 1931 RVA: 0x0001F994 File Offset: 0x0001DB94
		public event Action<string> voiceChatMicrophoneChangedEvent;

		// Token: 0x0600078C RID: 1932 RVA: 0x0001F9C9 File Offset: 0x0001DBC9
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			if (firstActivation)
			{
				AudioSettings.OnAudioConfigurationChanged += new AudioSettings.AudioConfigurationChangeHandler(this.UpdateMicrophoneList);
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001EFB6 File Offset: 0x0001D1B6
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x0001F9E7 File Offset: 0x0001DBE7
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x0001F9F3 File Offset: 0x0001DBF3
		[UIValue("custom-voice-enabled")]
		public bool CustomVoiceEnabled
		{
			get
			{
				return PluginConfig.Instance.CustomVoiceEnabled;
			}
			set
			{
				PluginConfig.Instance.CustomVoiceEnabled = value;
				InGameController instance = InGameController.instance;
				if (instance == null)
				{
					return;
				}
				instance.ChangeVoiceEnabled(false);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0001FA10 File Offset: 0x0001DC10
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		[UIValue("volume-adjust")]
		public float volumeAdjust
		{
			get
			{
				return PluginConfig.Instance.VolumeAdjust;
			}
			set
			{
				PluginConfig.Instance.VolumeAdjust = value;
				InGameController instance = InGameController.instance;
				if (instance == null)
				{
					return;
				}
				instance.SetVoIPVolume(value);
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001FA39 File Offset: 0x0001DC39
		[UIAction("percent-formatter")]
		public string OnFormatPercent(float obj)
		{
			return string.Format("{0}%", obj * 100f);
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0001FA51 File Offset: 0x0001DC51
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x0001FA5D File Offset: 0x0001DC5D
		[UIValue("microphone-enabled")]
		public bool MicrophoneEnabled
		{
			get
			{
				return PluginConfig.Instance.MicphoneEnabled;
			}
			set
			{
				PluginConfig.Instance.MicphoneEnabled = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0001FA6A File Offset: 0x0001DC6A
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x0001FA76 File Offset: 0x0001DC76
		[UIValue("microphone-option")]
		public string MicrophoneOption
		{
			get
			{
				return PluginConfig.Instance.MicphoneName;
			}
			set
			{
				PluginConfig.Instance.MicphoneName = this.ConvertLocation(value).ToString();
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0001FA90 File Offset: 0x0001DC90
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x000196A0 File Offset: 0x000178A0
		[UIValue("microphone-option-list")]
		public List<object> MicrophoneOptionList
		{
			get
			{
				string[] names = Enum.GetNames(typeof(MicrophoneOpt));
				List<object> list = new List<object>();
				for (int i = 0; i < names.Length; i++)
				{
					list.Add(this.ConvertLocation(names[i]));
				}
				foreach (string text in Microphone.devices)
				{
					list.Add(this.ConvertLocation(text));
				}
				return list;
			}
			private set
			{
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000196A0 File Offset: 0x000178A0
		private void UpdateMicrophoneList(bool deviceWasChanged)
		{
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001FAFD File Offset: 0x0001DCFD
		[UIAction("change-micphone")]
		private void ChangeMicphone(string micphoneName)
		{
			Action<string> action = this.voiceChatMicrophoneChangedEvent;
			if (action == null)
			{
				return;
			}
			action(this.MicrophoneOption);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001FB18 File Offset: 0x0001DD18
		private object ConvertLocation(string eStr)
		{
			if (eStr.ToLower().Contains("vive"))
			{
				return "vive";
			}
			if (eStr.ToLower().Contains("steam"))
			{
				return "steam";
			}
			if (eStr.ToLower().Contains("oculus"))
			{
				return "oculus";
			}
			if (eStr.ToLower().Contains("麦克风"))
			{
				return "麦克风";
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(eStr);
			if (num <= 1885483882U)
			{
				if (num <= 513712005U)
				{
					if (num != 309063606U)
					{
						if (num != 474567577U)
						{
							if (num == 513712005U)
							{
								if (eStr == "Right")
								{
									return "右扳机";
								}
							}
						}
						else if (eStr == "按键说话")
						{
							return "KeyOpen";
						}
					}
					else if (eStr == "Auto")
					{
						return "默认";
					}
				}
				else if (num <= 803035898U)
				{
					if (num != 558955756U)
					{
						if (num == 803035898U)
						{
							if (eStr == "保持开启")
							{
								return "KeepLive";
							}
						}
					}
					else if (eStr == "左扳机")
					{
						return "Left";
					}
				}
				else if (num != 1161488940U)
				{
					if (num == 1885483882U)
					{
						if (eStr == "KeepLive")
						{
							return "保持开启";
						}
					}
				}
				else if (eStr == "KeyOpen")
				{
					return "按键说话";
				}
			}
			else if (num <= 2845680283U)
			{
				if (num != 2457286800U)
				{
					if (num != 2622834813U)
					{
						if (num == 2845680283U)
						{
							if (eStr == "右扳机")
							{
								return "Right";
							}
						}
					}
					else if (eStr == "LeftOrRight")
					{
						return "左或者右";
					}
				}
				else if (eStr == "Left")
				{
					return "左扳机";
				}
			}
			else if (num <= 3977034263U)
			{
				if (num != 3362684001U)
				{
					if (num == 3977034263U)
					{
						if (eStr == "左右扳机")
						{
							return "LeftAndRight";
						}
					}
				}
				else if (eStr == "左或者右")
				{
					return "LeftOrRight";
				}
			}
			else if (num != 4170051729U)
			{
				if (num == 4252212153U)
				{
					if (eStr == "默认")
					{
						return "Auto";
					}
				}
			}
			else if (eStr == "LeftAndRight")
			{
				return "左右扳机";
			}
			return eStr;
		}
	}
}
