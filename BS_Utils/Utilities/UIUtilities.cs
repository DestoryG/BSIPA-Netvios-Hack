using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BS_Utils.Utilities
{
	// Token: 0x02000008 RID: 8
	public class UIUtilities
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00004100 File Offset: 0x00002300
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

		// Token: 0x06000096 RID: 150 RVA: 0x0000412A File Offset: 0x0000232A
		public static Texture2D LoadTextureFromFile(string FilePath)
		{
			if (File.Exists(FilePath))
			{
				return UIUtilities.LoadTextureRaw(File.ReadAllBytes(FilePath));
			}
			return null;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004141 File Offset: 0x00002341
		public static Texture2D LoadTextureFromResources(string resourcePath)
		{
			return UIUtilities.LoadTextureRaw(UIUtilities.GetResource(Assembly.GetCallingAssembly(), resourcePath));
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004153 File Offset: 0x00002353
		public static Sprite LoadSpriteRaw(byte[] image, float PixelsPerUnit = 100f)
		{
			return UIUtilities.LoadSpriteFromTexture(UIUtilities.LoadTextureRaw(image), PixelsPerUnit);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004161 File Offset: 0x00002361
		public static Sprite LoadSpriteFromTexture(Texture2D SpriteTexture, float PixelsPerUnit = 100f)
		{
			if (SpriteTexture)
			{
				return Sprite.Create(SpriteTexture, new Rect(0f, 0f, (float)SpriteTexture.width, (float)SpriteTexture.height), new Vector2(0f, 0f), PixelsPerUnit);
			}
			return null;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000041A0 File Offset: 0x000023A0
		public static Sprite LoadSpriteFromFile(string FilePath, float PixelsPerUnit = 100f)
		{
			return UIUtilities.LoadSpriteFromTexture(UIUtilities.LoadTextureFromFile(FilePath), PixelsPerUnit);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000041AE File Offset: 0x000023AE
		public static Sprite LoadSpriteFromResources(string resourcePath, float PixelsPerUnit = 100f)
		{
			return UIUtilities.LoadSpriteRaw(UIUtilities.GetResource(Assembly.GetCallingAssembly(), resourcePath), PixelsPerUnit);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000041C4 File Offset: 0x000023C4
		public static byte[] GetResource(Assembly asm, string ResourceName)
		{
			Stream manifestResourceStream = asm.GetManifestResourceStream(ResourceName);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			return array;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000041F8 File Offset: 0x000023F8
		public static void PrintHierarchy(Transform transform, string spacing = "|-> ")
		{
			spacing = spacing.Insert(1, "  ");
			List<Transform> list = transform.Cast<Transform>().ToList<Transform>();
			foreach (Transform transform2 in list)
			{
				Console.WriteLine(spacing + transform2.name);
				UIUtilities.PrintHierarchy(transform2, "|" + spacing);
			}
		}
	}
}
