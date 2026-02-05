using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BeatSaverDownloader.Misc
{
	// Token: 0x0200001E RID: 30
	internal class Sprites
	{
		// Token: 0x0600014B RID: 331 RVA: 0x000061F4 File Offset: 0x000043F4
		public static void ConvertToSprites()
		{
			Plugin.log.Info("Creating sprites...");
			Sprites.AddToFavorites = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.AddToFavorites.png", 100f);
			Sprites.RemoveFromFavorites = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.RemoveFromFavorites.png", 100f);
			Sprites.StarFull = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.StarFull.png", 100f);
			Sprites.StarEmpty = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.StarEmpty.png", 100f);
			Sprites.BeastSaberLogo = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.BeastSaberLogo.png", 100f);
			Sprites.ReviewIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.ReviewIcon.png", 100f);
			Sprites.ThumbUp = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.ThumbUp.png", 100f);
			Sprites.ThumbDown = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.ThumbDown.png", 100f);
			Sprites.PlaylistIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.PlaylistIcon.png", 100f);
			Sprites.SongIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.SongIcon.png", 100f);
			Sprites.DownloadIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.DownloadIcon.png", 100f);
			Sprites.PlayIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.PlayIcon.png", 100f);
			Sprites.DoubleArrow = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.DoubleArrow.png", 100f);
			Sprites.RandomIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.RandomIcon.png", 100f);
			Sprites.DeleteIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.DeleteIcon.png", 100f);
			Sprites.BeatSaverIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.BeatSaver.png", 100f);
			Sprites.ScoreSaberIcon = Sprites.LoadSpriteFromResources("BeatSaverDownloader.Assets.ScoreSaber.png", 100f);
			Plugin.log.Info("Creating sprites... Done!");
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006373 File Offset: 0x00004573
		public static string SpriteToBase64(Sprite input)
		{
			return Convert.ToBase64String(ImageConversion.EncodeToPNG(input.texture));
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006388 File Offset: 0x00004588
		public static Sprite Base64ToSprite(string input)
		{
			string text = input;
			if (input.Contains(","))
			{
				text = input.Substring(input.IndexOf(','));
			}
			Texture2D texture2D = Sprites.Base64ToTexture2D(text);
			return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.one / 2f);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000063EC File Offset: 0x000045EC
		public static Texture2D Base64ToTexture2D(string encodedData)
		{
			byte[] array = Convert.FromBase64String(encodedData);
			Texture2D texture2D = new Texture2D(2, 2);
			if (ImageConversion.LoadImage(texture2D, array))
			{
				return texture2D;
			}
			return null;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006414 File Offset: 0x00004614
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

		// Token: 0x06000150 RID: 336 RVA: 0x0000643E File Offset: 0x0000463E
		public static Texture2D LoadTextureFromFile(string FilePath)
		{
			if (File.Exists(FilePath))
			{
				return Sprites.LoadTextureRaw(File.ReadAllBytes(FilePath));
			}
			return null;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006455 File Offset: 0x00004655
		public static Texture2D LoadTextureFromResources(string resourcePath)
		{
			return Sprites.LoadTextureRaw(Sprites.GetResource(Assembly.GetCallingAssembly(), resourcePath));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006467 File Offset: 0x00004667
		public static Sprite LoadSpriteRaw(byte[] image, float PixelsPerUnit = 100f)
		{
			return Sprites.LoadSpriteFromTexture(Sprites.LoadTextureRaw(image), PixelsPerUnit);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006475 File Offset: 0x00004675
		public static Sprite LoadSpriteFromTexture(Texture2D SpriteTexture, float PixelsPerUnit = 100f)
		{
			if (SpriteTexture)
			{
				return Sprite.Create(SpriteTexture, new Rect(0f, 0f, (float)SpriteTexture.width, (float)SpriteTexture.height), new Vector2(0f, 0f), PixelsPerUnit);
			}
			return null;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000064B4 File Offset: 0x000046B4
		public static Sprite LoadSpriteFromFile(string FilePath, float PixelsPerUnit = 100f)
		{
			return Sprites.LoadSpriteFromTexture(Sprites.LoadTextureFromFile(FilePath), PixelsPerUnit);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000064C2 File Offset: 0x000046C2
		public static Sprite LoadSpriteFromResources(string resourcePath, float PixelsPerUnit = 100f)
		{
			return Sprites.LoadSpriteRaw(Sprites.GetResource(Assembly.GetCallingAssembly(), resourcePath), PixelsPerUnit);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000064D8 File Offset: 0x000046D8
		public static byte[] GetResource(Assembly asm, string ResourceName)
		{
			Stream manifestResourceStream = asm.GetManifestResourceStream(ResourceName);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			return array;
		}

		// Token: 0x04000084 RID: 132
		public static Sprite AddToFavorites;

		// Token: 0x04000085 RID: 133
		public static Sprite RemoveFromFavorites;

		// Token: 0x04000086 RID: 134
		public static Sprite StarFull;

		// Token: 0x04000087 RID: 135
		public static Sprite StarEmpty;

		// Token: 0x04000088 RID: 136
		public static Sprite DoubleArrow;

		// Token: 0x04000089 RID: 137
		public static Sprite BeastSaberLogo;

		// Token: 0x0400008A RID: 138
		public static Sprite ReviewIcon;

		// Token: 0x0400008B RID: 139
		public static Sprite ThumbUp;

		// Token: 0x0400008C RID: 140
		public static Sprite ThumbDown;

		// Token: 0x0400008D RID: 141
		public static Sprite PlaylistIcon;

		// Token: 0x0400008E RID: 142
		public static Sprite SongIcon;

		// Token: 0x0400008F RID: 143
		public static Sprite DownloadIcon;

		// Token: 0x04000090 RID: 144
		public static Sprite PlayIcon;

		// Token: 0x04000091 RID: 145
		public static Sprite RandomIcon;

		// Token: 0x04000092 RID: 146
		public static Sprite DeleteIcon;

		// Token: 0x04000093 RID: 147
		public static Sprite BeatSaverIcon;

		// Token: 0x04000094 RID: 148
		public static Sprite ScoreSaberIcon;
	}
}
