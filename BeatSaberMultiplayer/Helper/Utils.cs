using System;
using System.Text.RegularExpressions;
using BeatSaberMultiplayer.Data;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000087 RID: 135
	public static class Utils
	{
		// Token: 0x06000951 RID: 2385 RVA: 0x00025FBD File Offset: 0x000241BD
		public static Room.RoomStatus ForamtRoomStatus(string roomStatusStr)
		{
			return (Room.RoomStatus)Enum.Parse(typeof(Room.RoomStatus), roomStatusStr, true);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00025FD8 File Offset: 0x000241D8
		public static string Truncate(string str, int maxLength)
		{
			string text = str;
			if (Regex.Replace(text, "[一-龥]", "zz", RegexOptions.IgnoreCase).Length <= maxLength)
			{
				return text;
			}
			for (int i = text.Length; i >= 0; i--)
			{
				text = text.Substring(0, i);
				if (Regex.Replace(text, "[一-龥]", "zz", RegexOptions.IgnoreCase).Length <= maxLength - 3)
				{
					return text + "...";
				}
			}
			return "...";
		}
	}
}
