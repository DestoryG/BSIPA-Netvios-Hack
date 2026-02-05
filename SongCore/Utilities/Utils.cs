using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using IPA.Loader;
using IPA.Old;
using SongCore.Data;
using TMPro;
using UnityEngine;

namespace SongCore.Utilities
{
	// Token: 0x0200001A RID: 26
	public static class Utils
	{
		// Token: 0x0600015B RID: 347 RVA: 0x00006B38 File Offset: 0x00004D38
		public static bool IsModInstalled(string ModName)
		{
			using (IEnumerator<IPlugin> enumerator = PluginManager.Plugins.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Name == ModName)
					{
						return true;
					}
				}
			}
			using (IEnumerator<PluginMetadata> enumerator2 = PluginManager.AllPlugins.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.Id == ModName)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006BD4 File Offset: 0x00004DD4
		public static Color ColorFromMapColor(ExtraSongData.MapColor mapColor)
		{
			return new Color(mapColor.r, mapColor.g, mapColor.b);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006BED File Offset: 0x00004DED
		public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
		{
			if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
			{
				return defaultValue;
			}
			return (TEnum)((object)Enum.Parse(typeof(TEnum), strEnumValue));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006C18 File Offset: 0x00004E18
		public static bool IsDirectoryEmpty(string path)
		{
			return !Directory.EnumerateFileSystemEntries(path).Any<string>();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006C28 File Offset: 0x00004E28
		public static void GrantAccess(string file)
		{
			if (!Directory.Exists(file))
			{
				Directory.CreateDirectory(file);
			}
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(file);
				DirectorySecurity accessControl = directoryInfo.GetAccessControl();
				accessControl.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
				directoryInfo.SetAccessControl(accessControl);
			}
			catch
			{
				Logging.logger.Error("Exception trying to Grant access to " + file);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006C9C File Offset: 0x00004E9C
		public static string TrimEnd(this string text, string value)
		{
			if (!text.EndsWith(value))
			{
				return text;
			}
			return text.Remove(text.LastIndexOf(value));
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006CB6 File Offset: 0x00004EB6
		public static TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 anchoredPosition)
		{
			return Utils.CreateText(parent, text, anchoredPosition, new Vector2(60f, 10f));
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006CD0 File Offset: 0x00004ED0
		public static TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 anchoredPosition, Vector2 sizeDelta)
		{
			GameObject gameObject = new GameObject("CustomUIText");
			gameObject.SetActive(false);
			TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = Object.Instantiate<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First((TMP_FontAsset t) => t.name == "Teko-Medium SDF No Glow"));
			textMeshProUGUI.rectTransform.SetParent(parent, false);
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = 4f;
			textMeshProUGUI.color = Color.white;
			textMeshProUGUI.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			textMeshProUGUI.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			textMeshProUGUI.rectTransform.sizeDelta = sizeDelta;
			textMeshProUGUI.rectTransform.anchoredPosition = anchoredPosition;
			gameObject.SetActive(true);
			return textMeshProUGUI;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006DA6 File Offset: 0x00004FA6
		public static Sprite LoadSpriteRaw(byte[] image, float PixelsPerUnit = 100f)
		{
			return Utils.LoadSpriteFromTexture(Utils.LoadTextureRaw(image), PixelsPerUnit);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006DB4 File Offset: 0x00004FB4
		public static Sprite LoadSpriteFromTexture(Texture2D SpriteTexture, float PixelsPerUnit = 100f)
		{
			if (SpriteTexture)
			{
				return Sprite.Create(SpriteTexture, new Rect(0f, 0f, (float)SpriteTexture.width, (float)SpriteTexture.height), new Vector2(0f, 0f), PixelsPerUnit);
			}
			return null;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006DF3 File Offset: 0x00004FF3
		public static Sprite LoadSpriteFromFile(string FilePath, float PixelsPerUnit = 100f)
		{
			return Utils.LoadSpriteFromTexture(Utils.LoadTextureFromFile(FilePath), PixelsPerUnit);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006E01 File Offset: 0x00005001
		public static Sprite LoadSpriteFromResources(string resourcePath, float PixelsPerUnit = 100f)
		{
			return Utils.LoadSpriteRaw(Utils.GetResource(Assembly.GetCallingAssembly(), resourcePath), PixelsPerUnit);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006E14 File Offset: 0x00005014
		public static byte[] GetResource(Assembly asm, string ResourceName)
		{
			Stream manifestResourceStream = asm.GetManifestResourceStream(ResourceName);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			return array;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006E48 File Offset: 0x00005048
		public static void PrintHierarchy(Transform transform, string spacing = "|-> ")
		{
			spacing = spacing.Insert(1, "  ");
			foreach (Transform transform2 in transform.Cast<Transform>().ToList<Transform>())
			{
				Console.WriteLine(spacing + transform2.name);
				Utils.PrintHierarchy(transform2, "|" + spacing);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006ECC File Offset: 0x000050CC
		public static Texture2D LoadTextureRaw(byte[] file)
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

		// Token: 0x0600016A RID: 362 RVA: 0x00006EF6 File Offset: 0x000050F6
		public static Texture2D LoadTextureFromFile(string FilePath)
		{
			if (File.Exists(FilePath))
			{
				return Utils.LoadTextureRaw(File.ReadAllBytes(FilePath));
			}
			return null;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006F0D File Offset: 0x0000510D
		public static Texture2D LoadTextureFromResources(string resourcePath)
		{
			return Utils.LoadTextureRaw(Utils.GetResource(Assembly.GetCallingAssembly(), resourcePath));
		}
	}
}
