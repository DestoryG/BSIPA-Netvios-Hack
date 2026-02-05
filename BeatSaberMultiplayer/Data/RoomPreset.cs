using System;
using System.IO;
using Newtonsoft.Json;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000097 RID: 151
	[Serializable]
	public class RoomPreset
	{
		// Token: 0x06000990 RID: 2448 RVA: 0x0000370C File Offset: 0x0000190C
		public RoomPreset()
		{
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00026CAA File Offset: 0x00024EAA
		public RoomPreset(RoomSettings roomSettings)
		{
			this.settings = roomSettings;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00026CB9 File Offset: 0x00024EB9
		public static RoomPreset LoadPreset(string path)
		{
			if (File.Exists(path))
			{
				RoomPreset roomPreset = JsonConvert.DeserializeObject<RoomPreset>(File.ReadAllText(path));
				roomPreset.path = path;
				return roomPreset;
			}
			return null;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00026CD7 File Offset: 0x00024ED7
		public string GetName()
		{
			return Path.GetFileNameWithoutExtension(this.path);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00026CE4 File Offset: 0x00024EE4
		public RoomSettings GetRoomSettings()
		{
			return this.settings;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00026CEC File Offset: 0x00024EEC
		public void SavePreset()
		{
			if (!string.IsNullOrEmpty(this.path))
			{
				File.WriteAllText(this.path, JsonConvert.SerializeObject(this));
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00026D0C File Offset: 0x00024F0C
		public void SavePreset(string newPath)
		{
			if (!string.IsNullOrEmpty(newPath))
			{
				File.WriteAllText(newPath, JsonConvert.SerializeObject(this));
				this.path = newPath;
			}
		}

		// Token: 0x040004CB RID: 1227
		public RoomSettings settings;

		// Token: 0x040004CC RID: 1228
		[NonSerialized]
		private string path;
	}
}
