using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CustomAvatar.Avatar;
using CustomAvatar.Tracking;
using CustomAvatar.Utilities;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace CustomAvatar
{
	// Token: 0x0200000F RID: 15
	public class AvatarManager
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002714 File Offset: 0x00000914
		public static AvatarManager instance
		{
			get
			{
				bool flag = AvatarManager._instance == null;
				if (flag)
				{
					AvatarManager._instance = new AvatarManager();
				}
				return AvatarManager._instance;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002743 File Offset: 0x00000943
		internal AvatarTailor avatarTailor { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000274B File Offset: 0x0000094B
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002753 File Offset: 0x00000953
		internal SpawnedAvatar currentlySpawnedAvatar { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000015 RID: 21 RVA: 0x0000275C File Offset: 0x0000095C
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x00002794 File Offset: 0x00000994
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<SpawnedAvatar> avatarChanged;

		// Token: 0x06000017 RID: 23 RVA: 0x000027C9 File Offset: 0x000009C9
		private AvatarManager()
		{
			this.avatarTailor = new AvatarTailor();
			Plugin.instance.sceneTransitionDidFinish += this.OnSceneTransitionDidFinish;
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002808 File Offset: 0x00000A08
		~AvatarManager()
		{
			Plugin.instance.sceneTransitionDidFinish -= this.OnSceneTransitionDidFinish;
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000285C File Offset: 0x00000A5C
		public void GetAvatarsAsync(Action<LoadedAvatar> success, Action<Exception> error)
		{
			Plugin.logger.Info("Loading all avatars from " + AvatarManager.kCustomAvatarsPath);
			foreach (string text in this.GetAvatarFileNames())
			{
				PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(LoadedAvatar.FromFileCoroutine(text, success, error));
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000028B4 File Offset: 0x00000AB4
		public void LoadAvatarFromSettingsAsync()
		{
			string previousAvatarPath = SettingsManager.settings.previousAvatarPath;
			bool flag = string.IsNullOrEmpty(previousAvatarPath);
			if (!flag)
			{
				bool flag2 = !File.Exists(Path.Combine(AvatarManager.kCustomAvatarsPath, previousAvatarPath));
				if (flag2)
				{
					Plugin.logger.Warn("Previously loaded avatar no longer exists; reverting to default");
				}
				else
				{
					this.SwitchToAvatarAsync(previousAvatarPath);
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000290C File Offset: 0x00000B0C
		public void SwitchToAvatarAsync(string filePath)
		{
			PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(LoadedAvatar.FromFileCoroutine(filePath, delegate(LoadedAvatar avatar)
			{
				Plugin.logger.Info("Successfully loaded avatar " + avatar.descriptor.name);
				this.SwitchToAvatar(avatar);
			}, delegate(Exception ex)
			{
				Plugin.logger.Error("Failed to load avatar: " + ex.Message);
			}));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000294C File Offset: 0x00000B4C
		public void SwitchToAvatar(LoadedAvatar avatar)
		{
			SpawnedAvatar currentlySpawnedAvatar = this.currentlySpawnedAvatar;
			bool flag = ((currentlySpawnedAvatar != null) ? currentlySpawnedAvatar.customAvatar : null) == avatar;
			if (!flag)
			{
				SpawnedAvatar currentlySpawnedAvatar2 = this.currentlySpawnedAvatar;
				if (currentlySpawnedAvatar2 != null)
				{
					currentlySpawnedAvatar2.Destroy();
				}
				this.currentlySpawnedAvatar = null;
				SettingsManager.settings.previousAvatarPath = ((avatar != null) ? avatar.fullPath : null);
				bool flag2 = avatar == null;
				if (!flag2)
				{
					this.currentlySpawnedAvatar = AvatarManager.SpawnAvatar(avatar, new VRAvatarInput());
					Action<SpawnedAvatar> action = this.avatarChanged;
					if (action != null)
					{
						action(this.currentlySpawnedAvatar);
					}
					this.ResizeCurrentAvatar();
					SpawnedAvatar currentlySpawnedAvatar3 = this.currentlySpawnedAvatar;
					if (currentlySpawnedAvatar3 != null)
					{
						currentlySpawnedAvatar3.OnFirstPersonEnabledChanged();
					}
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000029F4 File Offset: 0x00000BF4
		public void SwitchToNextAvatar()
		{
			string[] avatarFileNames = this.GetAvatarFileNames();
			int num = Array.IndexOf<string>(avatarFileNames, this.currentlySpawnedAvatar.customAvatar.fullPath);
			num = (num + 1) % avatarFileNames.Length;
			this.SwitchToAvatarAsync(avatarFileNames[num]);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002A34 File Offset: 0x00000C34
		public void SwitchToPreviousAvatar()
		{
			string[] avatarFileNames = this.GetAvatarFileNames();
			int num = Array.IndexOf<string>(avatarFileNames, this.currentlySpawnedAvatar.customAvatar.fullPath);
			num = (num + avatarFileNames.Length - 1) % avatarFileNames.Length;
			this.SwitchToAvatarAsync(avatarFileNames[num]);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A78 File Offset: 0x00000C78
		public void ResizeCurrentAvatar()
		{
			bool flag = this.currentlySpawnedAvatar != null;
			if (flag)
			{
				this.avatarTailor.ResizeAvatar(this.currentlySpawnedAvatar);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002AA8 File Offset: 0x00000CA8
		private void OnSceneLoaded(Scene newScene, LoadSceneMode mode)
		{
			SpawnedAvatar currentlySpawnedAvatar = this.currentlySpawnedAvatar;
			if (currentlySpawnedAvatar != null)
			{
				currentlySpawnedAvatar.OnFirstPersonEnabledChanged();
			}
			SpawnedAvatar currentlySpawnedAvatar2 = this.currentlySpawnedAvatar;
			if (currentlySpawnedAvatar2 != null)
			{
				AvatarEventsPlayer eventsPlayer = currentlySpawnedAvatar2.eventsPlayer;
				if (eventsPlayer != null)
				{
					eventsPlayer.Restart();
				}
			}
			bool flag = newScene.name == "HealthWarning" && SettingsManager.settings.calibrateFullBodyTrackingOnStart;
			if (flag)
			{
				this.avatarTailor.CalibrateFullBodyTracking();
			}
			this.ResizeCurrentAvatar();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B20 File Offset: 0x00000D20
		private void OnSceneTransitionDidFinish(ScenesTransitionSetupDataSO setupData, DiContainer container)
		{
			string name = SceneManager.GetActiveScene().name;
			bool flag;
			if (name == "GameCore")
			{
				SpawnedAvatar currentlySpawnedAvatar = this.currentlySpawnedAvatar;
				flag = ((currentlySpawnedAvatar != null) ? currentlySpawnedAvatar.eventsPlayer : null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.currentlySpawnedAvatar.eventsPlayer.LevelStartedEvent();
			}
			bool flag3;
			if (name == "MenuCore")
			{
				SpawnedAvatar currentlySpawnedAvatar2 = this.currentlySpawnedAvatar;
				flag3 = ((currentlySpawnedAvatar2 != null) ? currentlySpawnedAvatar2.eventsPlayer : null);
			}
			else
			{
				flag3 = false;
			}
			bool flag4 = flag3;
			if (flag4)
			{
				this.currentlySpawnedAvatar.eventsPlayer.MenuEnteredEvent();
			}
			this.ResizeCurrentAvatar();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002BC0 File Offset: 0x00000DC0
		private static SpawnedAvatar SpawnAvatar(LoadedAvatar customAvatar, AvatarInput input)
		{
			bool flag = customAvatar == null;
			if (flag)
			{
				throw new ArgumentNullException("customAvatar");
			}
			bool flag2 = input == null;
			if (flag2)
			{
				throw new ArgumentNullException("input");
			}
			return new SpawnedAvatar(customAvatar, input);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C04 File Offset: 0x00000E04
		private string[] GetAvatarFileNames()
		{
			return (from f in Directory.GetFiles(AvatarManager.kCustomAvatarsPath, "*.avatar")
				select this.GetRelativePath(AvatarManager.kCustomAvatarsPath, f)).ToArray<string>();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C3C File Offset: 0x00000E3C
		private string GetRelativePath(string rootDirectoryPath, string path)
		{
			string fullPath = Path.GetFullPath(rootDirectoryPath);
			string fullPath2 = Path.GetFullPath(path);
			bool flag = !fullPath2.StartsWith(fullPath);
			string text;
			if (flag)
			{
				text = fullPath2;
			}
			else
			{
				text = fullPath2.Substring(fullPath.Length + 1);
			}
			return text;
		}

		// Token: 0x04000049 RID: 73
		public static readonly string kCustomAvatarsPath = Path.GetFullPath("CustomAvatars");

		// Token: 0x0400004A RID: 74
		private static AvatarManager _instance;
	}
}
