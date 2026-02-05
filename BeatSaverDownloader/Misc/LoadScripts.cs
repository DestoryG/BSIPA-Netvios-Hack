using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatSaverDownloader.Misc
{
	// Token: 0x02000020 RID: 32
	internal class LoadScripts
	{
		// Token: 0x0600015F RID: 351 RVA: 0x000065F1 File Offset: 0x000047F1
		public static IEnumerator LoadSpriteCoroutine(string spritePath, Action<Sprite> finished)
		{
			if (LoadScripts._cachedSprites.ContainsKey(spritePath))
			{
				finished(LoadScripts._cachedSprites[spritePath]);
				yield break;
			}
			using (WWW www = new WWW(spritePath))
			{
				yield return www;
				Texture2D texture = www.texture;
				Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), Vector2.one * 0.5f, 100f, 1U);
				if (!LoadScripts._cachedSprites.ContainsKey(spritePath))
				{
					LoadScripts._cachedSprites.Add(spritePath, sprite);
				}
				finished(sprite);
			}
			WWW www = null;
			yield break;
			yield break;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006607 File Offset: 0x00004807
		public static IEnumerator LoadAudioCoroutine(string audioPath, object obj, string fieldName)
		{
			using (WWW www = new WWW(audioPath))
			{
				yield return www;
				obj.SetPrivateField(fieldName, www.GetAudioClip(true, true, 0));
			}
			WWW www = null;
			yield break;
			yield break;
		}

		// Token: 0x04000095 RID: 149
		public static Dictionary<string, Sprite> _cachedSprites = new Dictionary<string, Sprite>();
	}
}
