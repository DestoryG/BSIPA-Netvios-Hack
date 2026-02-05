using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CustomSaber.Utilities
{
	// Token: 0x02000012 RID: 18
	public class Utils
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003E5C File Offset: 0x0000205C
		public static IEnumerable<string> GetFileNames(string path, IEnumerable<string> filters, SearchOption searchOption, bool returnShortPath = false)
		{
			IList<string> list = new List<string>();
			foreach (string text in filters)
			{
				IEnumerable<string> files = Directory.GetFiles(path, text, searchOption);
				if (returnShortPath)
				{
					foreach (string text2 in files)
					{
						string text3 = text2.Replace(path, "");
						bool flag = text3.Length > 0 && text3.StartsWith("\\");
						if (flag)
						{
							text3 = text3.Substring(1, text3.Length - 1);
						}
						bool flag2 = !string.IsNullOrWhiteSpace(text3) && !list.Contains(text3);
						if (flag2)
						{
							list.Add(text3);
						}
					}
				}
				else
				{
					list = list.Union(files).ToList<string>();
				}
			}
			return list.Distinct<string>();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003F88 File Offset: 0x00002188
		public static string SafeUnescape(string text)
		{
			string text2;
			try
			{
				bool flag = string.IsNullOrWhiteSpace(text);
				if (flag)
				{
					text2 = string.Empty;
				}
				else
				{
					text2 = text;
					text2 = text2.Replace("\\n", "\n");
					text2 = text2.Replace("\\t", "\t");
				}
			}
			catch
			{
				text2 = text;
			}
			return text2;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003FF0 File Offset: 0x000021F0
		public static Sprite GetDefaultCoverImage()
		{
			bool flag = !Utils.defaultCoverImage;
			if (flag)
			{
				try
				{
					Utils.defaultCoverImage = Utils.LoadSpriteFromResources("CustomSaber.Resources.fa-magic.png", 100f);
					Utils.defaultCoverImage.texture.wrapMode = 1;
				}
				catch
				{
				}
			}
			return Utils.defaultCoverImage;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004058 File Offset: 0x00002258
		public static Sprite GetRandomCoverImage()
		{
			string defaultSabersImageFolder = "CustomSaber.Resources.DefaultSabers";
			IList<string> list = (from file in Assembly.GetExecutingAssembly().GetManifestResourceNames()
				where file.StartsWith(defaultSabersImageFolder)
				select file).ToList<string>();
			int num = new Random().Next(list.Count);
			string text = list[num];
			return Utils.LoadSpriteFromResources(text, 100f);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000040C4 File Offset: 0x000022C4
		public static Sprite GetErrorCoverImage()
		{
			bool flag = !Utils.errorCoverImage;
			if (flag)
			{
				try
				{
					Utils.errorCoverImage = Utils.LoadSpriteFromResources("CustomSaber.Resources.fa-magic-error.png", 100f);
					Utils.errorCoverImage.texture.wrapMode = 1;
				}
				catch
				{
				}
			}
			return Utils.errorCoverImage;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000412C File Offset: 0x0000232C
		public static Texture2D LoadTextureRaw(byte[] file)
		{
			bool flag = file.Count<byte>() > 0;
			if (flag)
			{
				Texture2D texture2D = new Texture2D(2, 2);
				bool flag2 = ImageConversion.LoadImage(texture2D, file);
				if (flag2)
				{
					return texture2D;
				}
			}
			return null;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004168 File Offset: 0x00002368
		public static Texture2D LoadTextureFromFile(string filePath)
		{
			bool flag = File.Exists(filePath);
			Texture2D texture2D;
			if (flag)
			{
				texture2D = Utils.LoadTextureRaw(File.ReadAllBytes(filePath));
			}
			else
			{
				texture2D = null;
			}
			return texture2D;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004194 File Offset: 0x00002394
		public static Texture2D LoadTextureFromResources(string resourcePath)
		{
			return Utils.LoadTextureRaw(Utils.LoadFromResource(resourcePath));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000041B4 File Offset: 0x000023B4
		public static Sprite LoadSpriteFromResources(string resourcePath, float pixelsPerUnit = 100f)
		{
			return Utils.LoadSpriteRaw(Utils.LoadFromResource(resourcePath), pixelsPerUnit);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000041D4 File Offset: 0x000023D4
		public static Sprite LoadSpriteRaw(byte[] image, float pixelsPerUnit = 100f)
		{
			return Utils.LoadSpriteFromTexture(Utils.LoadTextureRaw(image), pixelsPerUnit);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000041F4 File Offset: 0x000023F4
		public static Sprite LoadSpriteFromTexture(Texture2D spriteTexture, float pixelsPerUnit = 100f)
		{
			bool flag = spriteTexture;
			Sprite sprite;
			if (flag)
			{
				sprite = Sprite.Create(spriteTexture, new Rect(0f, 0f, (float)spriteTexture.width, (float)spriteTexture.height), new Vector2(0f, 0f), pixelsPerUnit);
			}
			else
			{
				sprite = null;
			}
			return sprite;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004248 File Offset: 0x00002448
		public static Sprite LoadSpriteFromFile(string filePath, float pixelsPerUnit = 100f)
		{
			return Utils.LoadSpriteFromTexture(Utils.LoadTextureFromFile(filePath), pixelsPerUnit);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004268 File Offset: 0x00002468
		public static byte[] LoadFromResource(string resourcePath)
		{
			return Utils.GetResource(Assembly.GetCallingAssembly(), resourcePath);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004288 File Offset: 0x00002488
		public static byte[] GetResource(Assembly assembly, string resourcePath)
		{
			Stream manifestResourceStream = assembly.GetManifestResourceStream(resourcePath);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			return array;
		}

		// Token: 0x0400004C RID: 76
		private static Sprite defaultCoverImage = null;

		// Token: 0x0400004D RID: 77
		private static Sprite errorCoverImage = null;
	}
}
