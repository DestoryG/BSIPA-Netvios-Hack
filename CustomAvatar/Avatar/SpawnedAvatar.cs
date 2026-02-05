using System;
using CustomAvatar.Tracking;
using CustomAvatar.Utilities;
using UnityEngine;

namespace CustomAvatar.Avatar
{
	// Token: 0x0200003B RID: 59
	internal class SpawnedAvatar
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000984B File Offset: 0x00007A4B
		public LoadedAvatar customAvatar { get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00009853 File Offset: 0x00007A53
		public AvatarTracking tracking { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000985B File Offset: 0x00007A5B
		public AvatarEventsPlayer eventsPlayer { get; }

		// Token: 0x06000155 RID: 341 RVA: 0x00009864 File Offset: 0x00007A64
		public SpawnedAvatar(LoadedAvatar avatar, AvatarInput input)
		{
			if (avatar == null)
			{
				throw new ArgumentNullException("avatar");
			}
			this.customAvatar = avatar;
			this._gameObject = Object.Instantiate<GameObject>(this.customAvatar.gameObject);
			this._firstPersonExclusions = this._gameObject.GetComponentsInChildren<FirstPersonExclusion>();
			this.eventsPlayer = this._gameObject.AddComponent<AvatarEventsPlayer>();
			this.tracking = this._gameObject.AddComponent<AvatarTracking>();
			this.tracking.customAvatar = this.customAvatar;
			this.tracking.input = input;
			bool isIKAvatar = this.customAvatar.isIKAvatar;
			if (isIKAvatar)
			{
				AvatarIK avatarIK = this._gameObject.AddComponent<AvatarIK>();
				avatarIK.input = input;
			}
			bool supportsFingerTracking = this.customAvatar.supportsFingerTracking;
			if (supportsFingerTracking)
			{
				this._gameObject.AddComponent<AvatarFingerTracking>();
			}
			Object.DontDestroyOnLoad(this._gameObject);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009942 File Offset: 0x00007B42
		public void Destroy()
		{
			Object.Destroy(this._gameObject);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00009954 File Offset: 0x00007B54
		public void OnFirstPersonEnabledChanged()
		{
			this.SetChildrenToLayer(SettingsManager.settings.isAvatarVisibleInFirstPerson ? 10 : 3);
			foreach (FirstPersonExclusion firstPersonExclusion in this._firstPersonExclusions)
			{
				foreach (GameObject gameObject in firstPersonExclusion.exclude)
				{
					Plugin.logger.Debug("Excluding '" + gameObject.name + "' from first person view");
					gameObject.layer = 3;
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000099E4 File Offset: 0x00007BE4
		private void SetChildrenToLayer(int layer)
		{
			foreach (Transform transform in this._gameObject.GetComponentsInChildren<Transform>())
			{
				transform.gameObject.layer = layer;
			}
		}

		// Token: 0x040001BC RID: 444
		private readonly GameObject _gameObject;

		// Token: 0x040001BD RID: 445
		private readonly FirstPersonExclusion[] _firstPersonExclusions;
	}
}
