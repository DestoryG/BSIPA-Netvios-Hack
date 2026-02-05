using System;
using System.IO;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x02000007 RID: 7
	public class Config
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00003048 File Offset: 0x00001248
		public string FilePath { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600001E RID: 30 RVA: 0x00003050 File Offset: 0x00001250
		// (remove) Token: 0x0600001F RID: 31 RVA: 0x00003088 File Offset: 0x00001288
		public event Action<Config> ConfigChangedEvent;

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000030BD File Offset: 0x000012BD
		public Vector2 ScreenPosition
		{
			get
			{
				return new Vector2((float)this.screenPosX, (float)this.screenPosY);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000030D2 File Offset: 0x000012D2
		public Vector2 ScreenSize
		{
			get
			{
				return new Vector2((float)this.screenWidth, (float)this.screenHeight);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000030E7 File Offset: 0x000012E7
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00003100 File Offset: 0x00001300
		public Vector3 Position
		{
			get
			{
				return new Vector3(this.posx, this.posy, this.posz);
			}
			set
			{
				this.posx = value.x;
				this.posy = value.y;
				this.posz = value.z;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00003126 File Offset: 0x00001326
		public Vector3 DefaultPosition
		{
			get
			{
				return new Vector3(0f, 2f, -1.2f);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000313C File Offset: 0x0000133C
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00003155 File Offset: 0x00001355
		public Vector3 Rotation
		{
			get
			{
				return new Vector3(this.angx, this.angy, this.angz);
			}
			set
			{
				this.angx = value.x;
				this.angy = value.y;
				this.angz = value.z;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000317B File Offset: 0x0000137B
		public Vector3 DefaultRotation
		{
			get
			{
				return new Vector3(15f, 0f, 0f);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00003191 File Offset: 0x00001391
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000031AA File Offset: 0x000013AA
		public Vector3 FirstPersonPositionOffset
		{
			get
			{
				return new Vector3(this.firstPersonPosOffsetX, this.firstPersonPosOffsetY, this.firstPersonPosOffsetZ);
			}
			set
			{
				this.firstPersonPosOffsetX = value.x;
				this.firstPersonPosOffsetY = value.y;
				this.firstPersonPosOffsetZ = value.z;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000031D0 File Offset: 0x000013D0
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000031E9 File Offset: 0x000013E9
		public Vector3 FirstPersonRotationOffset
		{
			get
			{
				return new Vector3(this.firstPersonRotOffsetX, this.firstPersonRotOffsetY, this.firstPersonRotOffsetZ);
			}
			set
			{
				this.firstPersonRotOffsetX = value.x;
				this.firstPersonRotOffsetY = value.y;
				this.firstPersonRotOffsetZ = value.z;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000320F File Offset: 0x0000140F
		public Vector3 DefaultFirstPersonPositionOffset
		{
			get
			{
				return new Vector3(0f, 0f, 0f);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000320F File Offset: 0x0000140F
		public Vector3 DefaultFirstPersonRotationOffset
		{
			get
			{
				return new Vector3(0f, 0f, 0f);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003228 File Offset: 0x00001428
		public Config(string filePath)
		{
			this.FilePath = filePath;
			if (!Directory.Exists(Path.GetDirectoryName(this.FilePath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this.FilePath));
			}
			if (File.Exists(this.FilePath))
			{
				this.Load();
				string text = File.ReadAllText(this.FilePath);
				if (!text.Contains("fitToCanvas") && Path.GetFileName(this.FilePath) == Plugin.MainCamera + ".cfg")
				{
					this.fitToCanvas = true;
				}
				if (text.Contains("rotx"))
				{
					Config.OldRotConfig oldRotConfig = new Config.OldRotConfig();
					ConfigSerializer.LoadConfig(oldRotConfig, this.FilePath);
					Vector3 eulerAngles = new Quaternion(oldRotConfig.rotx, oldRotConfig.roty, oldRotConfig.rotz, oldRotConfig.rotw).eulerAngles;
					this.angx = eulerAngles.x;
					this.angy = eulerAngles.y;
					this.angz = eulerAngles.z;
				}
			}
			this.Save();
			this._configWatcher = new FileSystemWatcher(Path.GetDirectoryName(this.FilePath))
			{
				NotifyFilter = NotifyFilters.LastWrite,
				Filter = Path.GetFileName(this.FilePath),
				EnableRaisingEvents = true
			};
			this._configWatcher.Changed += this.ConfigWatcherOnChanged;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003444 File Offset: 0x00001644
		~Config()
		{
			this._configWatcher.Changed -= this.ConfigWatcherOnChanged;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003484 File Offset: 0x00001684
		public void Save()
		{
			this._saving = true;
			ConfigSerializer.SaveConfig(this, this.FilePath);
			this._saving = false;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000034A0 File Offset: 0x000016A0
		public void Load()
		{
			ConfigSerializer.LoadConfig(this, this.FilePath);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000034AF File Offset: 0x000016AF
		private void ConfigWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
		{
			if (this._saving)
			{
				this._saving = false;
				return;
			}
			this.Load();
			if (this.ConfigChangedEvent != null)
			{
				this.ConfigChangedEvent(this);
			}
		}

		// Token: 0x04000012 RID: 18
		public float fov = 90f;

		// Token: 0x04000013 RID: 19
		public int antiAliasing = 2;

		// Token: 0x04000014 RID: 20
		public float renderScale = 1f;

		// Token: 0x04000015 RID: 21
		public float positionSmooth = 10f;

		// Token: 0x04000016 RID: 22
		public float rotationSmooth = 5f;

		// Token: 0x04000017 RID: 23
		public float cam360Smoothness = 2f;

		// Token: 0x04000018 RID: 24
		public bool cam360RotateControlNew = true;

		// Token: 0x04000019 RID: 25
		public bool thirdPerson;

		// Token: 0x0400001A RID: 26
		public bool showThirdPersonCamera = true;

		// Token: 0x0400001B RID: 27
		public bool use360Camera;

		// Token: 0x0400001C RID: 28
		public float posx;

		// Token: 0x0400001D RID: 29
		public float posy = 2f;

		// Token: 0x0400001E RID: 30
		public float posz = -1.2f;

		// Token: 0x0400001F RID: 31
		public float angx = 15f;

		// Token: 0x04000020 RID: 32
		public float angy;

		// Token: 0x04000021 RID: 33
		public float angz;

		// Token: 0x04000022 RID: 34
		public float firstPersonPosOffsetX;

		// Token: 0x04000023 RID: 35
		public float firstPersonPosOffsetY;

		// Token: 0x04000024 RID: 36
		public float firstPersonPosOffsetZ;

		// Token: 0x04000025 RID: 37
		public float firstPersonRotOffsetX;

		// Token: 0x04000026 RID: 38
		public float firstPersonRotOffsetY;

		// Token: 0x04000027 RID: 39
		public float firstPersonRotOffsetZ;

		// Token: 0x04000028 RID: 40
		public float cam360ForwardOffset = -2f;

		// Token: 0x04000029 RID: 41
		public float cam360XTilt = 10f;

		// Token: 0x0400002A RID: 42
		public float cam360ZTilt;

		// Token: 0x0400002B RID: 43
		public float cam360YTilt;

		// Token: 0x0400002C RID: 44
		public float cam360UpOffset = 2.2f;

		// Token: 0x0400002D RID: 45
		public float cam360RightOffset;

		// Token: 0x0400002E RID: 46
		public int screenWidth = Screen.width;

		// Token: 0x0400002F RID: 47
		public int screenHeight = Screen.height;

		// Token: 0x04000030 RID: 48
		public int screenPosX;

		// Token: 0x04000031 RID: 49
		public int screenPosY;

		// Token: 0x04000032 RID: 50
		public int layer = -1000;

		// Token: 0x04000033 RID: 51
		public bool fitToCanvas;

		// Token: 0x04000034 RID: 52
		public bool transparentWalls;

		// Token: 0x04000035 RID: 53
		public bool forceFirstPersonUpRight;

		// Token: 0x04000036 RID: 54
		public bool avatar = true;

		// Token: 0x04000037 RID: 55
		public string debri = "link";

		// Token: 0x04000038 RID: 56
		public string movementScriptPath = string.Empty;

		// Token: 0x0400003A RID: 58
		private readonly FileSystemWatcher _configWatcher;

		// Token: 0x0400003B RID: 59
		private bool _saving;

		// Token: 0x02000024 RID: 36
		public class OldRotConfig
		{
			// Token: 0x040000C1 RID: 193
			public float rotx;

			// Token: 0x040000C2 RID: 194
			public float roty;

			// Token: 0x040000C3 RID: 195
			public float rotz;

			// Token: 0x040000C4 RID: 196
			public float rotw;
		}
	}
}
