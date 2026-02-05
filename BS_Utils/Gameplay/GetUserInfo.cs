using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using BS_Utils.Utilities;
using IPA.Logging;
using Steamworks;
using UnityEngine;

namespace BS_Utils.Gameplay
{
	// Token: 0x0200000D RID: 13
	public static class GetUserInfo
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004866 File Offset: 0x00002A66
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004889 File Offset: 0x00002A89
		public static VRPlatformHelper vRPlatformHelper
		{
			get
			{
				if (GetUserInfo._vRPlatformHelper == null)
				{
					GetUserInfo._vRPlatformHelper = Resources.FindObjectsOfTypeAll<VRPlatformHelper>().First<VRPlatformHelper>();
				}
				return GetUserInfo._vRPlatformHelper;
			}
			internal set
			{
				GetUserInfo._vRPlatformHelper = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004891 File Offset: 0x00002A91
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000048AF File Offset: 0x00002AAF
		public static PlatformUserModelSO PlatformUserModelSO
		{
			get
			{
				if (GetUserInfo._platformUserModel == null)
				{
					GetUserInfo._platformUserModel = ScriptableObject.CreateInstance<PlatformUserModelSO>();
				}
				return GetUserInfo._platformUserModel;
			}
			internal set
			{
				GetUserInfo._platformUserModel = value;
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000048B7 File Offset: 0x00002AB7
		static GetUserInfo()
		{
			GetUserInfo.UpdateUserInfo();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000048C0 File Offset: 0x00002AC0
		public static void UpdateUserInfo()
		{
			if (GetUserInfo.getUserActive)
			{
				return;
			}
			if (GetUserInfo.userID == 0UL || GetUserInfo.userName == null)
			{
				try
				{
					PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(GetUserInfo.GetUserCoroutine());
				}
				catch (Exception ex)
				{
					string text = "Unable to grab user! Exception: ";
					Exception ex2 = ex;
					BS_Utils.Utilities.Logger.Log(text + ((ex2 != null) ? ex2.ToString() : null), IPA.Logging.Logger.Level.Error);
				}
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004928 File Offset: 0x00002B28
		private static IEnumerator GetUserCoroutine()
		{
			GetUserInfo.getUserActive = true;
			WaitForSeconds wait = new WaitForSeconds(5f);
			int tries = 1;
			try
			{
				while (!GetUserInfo.foundUser && tries < 10)
				{
					if (!GetUserInfo.waitingForCompletion && !GetUserInfo.foundUser)
					{
						BS_Utils.Utilities.Logger.log.Debug(string.Format("Detected platform: {0}", GetUserInfo.PlatformUserModelSO.platformInfo.platform));
						try
						{
							GetUserInfo.waitingForCompletion = true;
							GetUserInfo.PlatformUserModelSO.GetUserInfo(new PlatformUserModelSO.GetUserInfoCompletionHandler(GetUserInfo.UserInfoCompletionHandler));
						}
						catch (Exception ex)
						{
							GetUserInfo.waitingForCompletion = false;
							BS_Utils.Utilities.Logger.log.Error("Error retrieving user info: " + ex.Message);
							BS_Utils.Utilities.Logger.log.Debug(ex);
						}
						int num = tries;
						tries = num + 1;
					}
					yield return wait;
				}
			}
			finally
			{
				GetUserInfo.getUserActive = false;
			}
			yield break;
			yield break;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004930 File Offset: 0x00002B30
		private static void UserInfoCompletionHandler(PlatformUserModelSO.GetUserInfoResult result, PlatformUserModelSO.UserInfo userInfo)
		{
			if (result == null)
			{
				BS_Utils.Utilities.Logger.log.Debug("UserInfo found: " + userInfo.userId + ": " + userInfo.userName);
				GetUserInfo.platformInfo = GetUserInfo.PlatformUserModelSO.platformInfo;
				ulong num;
				if (ulong.TryParse(userInfo.userId, out num))
				{
					GetUserInfo.userID = num;
				}
				else
				{
					BS_Utils.Utilities.Logger.log.Warn("Unable to parse " + userInfo.userId + " as a ulong.");
					GetUserInfo.userID = 0UL;
				}
				GetUserInfo.userName = userInfo.userName;
				if (GetUserInfo.PlatformUserModelSO.platformInfo.platform == 1)
				{
					GetUserInfo.GetSteamAvatar();
				}
				else if (GetUserInfo.PlatformUserModelSO.platformInfo.platform == 2)
				{
					GetUserInfo.userAvatar = GetUserInfo.LoadTextureFromResources("BS_Utils.Resources.oculus.png");
				}
				GetUserInfo.foundUser = true;
			}
			else
			{
				BS_Utils.Utilities.Logger.log.Error("Failed to retrieve user info.");
			}
			GetUserInfo.waitingForCompletion = false;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004A18 File Offset: 0x00002C18
		private static void GetSteamAvatar()
		{
			if (SteamManager.Initialized)
			{
				CSteamID steamID = SteamUser.GetSteamID();
				GetUserInfo.userAvatar = GetUserInfo.GetAvatar(steamID);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004A40 File Offset: 0x00002C40
		private static Texture2D GetAvatar(CSteamID steamUser)
		{
			int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(steamUser);
			uint num;
			uint num2;
			bool flag = SteamUtils.GetImageSize(largeFriendAvatar, ref num, ref num2);
			if (flag && num > 0U && num2 > 0U)
			{
				byte[] array = new byte[num * num2 * 4U];
				Texture2D texture2D = new Texture2D((int)num, (int)num2, 4, false, true);
				flag = SteamUtils.GetImageRGBA(largeFriendAvatar, array, (int)(num * num2 * 4U));
				if (flag)
				{
					texture2D.LoadRawTextureData(array);
					texture2D.Apply();
				}
				return texture2D;
			}
			Debug.LogError("Couldn't get avatar.");
			return new Texture2D(0, 0);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004AB7 File Offset: 0x00002CB7
		public static PlatformInfo GetPlatformInfo()
		{
			return GetUserInfo.platformInfo;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004ABE File Offset: 0x00002CBE
		public static string GetUserName()
		{
			return GetUserInfo.userName;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004AC5 File Offset: 0x00002CC5
		public static ulong GetUserID()
		{
			return GetUserInfo.userID;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004ACC File Offset: 0x00002CCC
		public static Texture2D GetUserAvatar()
		{
			return GetUserInfo.userAvatar;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004AD3 File Offset: 0x00002CD3
		internal static Texture2D LoadTextureFromResources(string resourcePath)
		{
			return GetUserInfo.LoadTextureRaw(GetUserInfo.GetResource(Assembly.GetCallingAssembly(), resourcePath));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004AE8 File Offset: 0x00002CE8
		internal static Texture2D LoadTextureRaw(byte[] file)
		{
			if (file.Count<byte>() > 0)
			{
				Texture2D texture2D = new Texture2D(2, 2);
				if (ImageConversion.LoadImage(texture2D, file))
				{
					return texture2D;
				}
			}
			return null;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004B14 File Offset: 0x00002D14
		internal static byte[] GetResource(Assembly asm, string ResourceName)
		{
			Stream manifestResourceStream = asm.GetManifestResourceStream(ResourceName);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			return array;
		}

		// Token: 0x04000047 RID: 71
		private static PlatformInfo platformInfo;

		// Token: 0x04000048 RID: 72
		private static string userName;

		// Token: 0x04000049 RID: 73
		private static ulong userID;

		// Token: 0x0400004A RID: 74
		private static Texture2D userAvatar;

		// Token: 0x0400004B RID: 75
		private static VRPlatformHelper _vRPlatformHelper;

		// Token: 0x0400004C RID: 76
		private static PlatformUserModelSO _platformUserModel;

		// Token: 0x0400004D RID: 77
		private static bool getUserActive;

		// Token: 0x0400004E RID: 78
		private static bool foundUser;

		// Token: 0x0400004F RID: 79
		private static bool waitingForCompletion;
	}
}
