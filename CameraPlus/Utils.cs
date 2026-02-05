using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using IPA.Loader;
using IPA.Logging;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x02000012 RID: 18
	internal class Utils
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00007E2C File Offset: 0x0000602C
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

		// Token: 0x0600008E RID: 142 RVA: 0x00007E56 File Offset: 0x00006056
		public static Texture2D LoadTextureFromResources(string resourcePath)
		{
			if (!Utils._cachedTextures.ContainsKey(resourcePath))
			{
				Utils._cachedTextures.Add(resourcePath, Utils.LoadTextureRaw(Utils.GetResource(Assembly.GetCallingAssembly(), resourcePath)));
			}
			return Utils._cachedTextures[resourcePath];
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00007E8B File Offset: 0x0000608B
		public static Image LoadImageFromResources(string resourcePath)
		{
			if (!Utils._cachedImages.ContainsKey(resourcePath))
			{
				Utils._cachedImages.Add(resourcePath, new Bitmap(Assembly.GetCallingAssembly().GetManifestResourceStream(resourcePath)));
			}
			return Utils._cachedImages[resourcePath];
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00007EC0 File Offset: 0x000060C0
		public static byte[] GetResource(Assembly asm, string ResourceName)
		{
			Stream manifestResourceStream = asm.GetManifestResourceStream(ResourceName);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			return array;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00007EF3 File Offset: 0x000060F3
		public static bool WithinRange(int numberToCheck, int bottom, int top)
		{
			return numberToCheck >= bottom && numberToCheck <= top;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007F04 File Offset: 0x00006104
		public static bool IsModInstalled(string modName)
		{
			Logger.Log("Looking in BSIPA for " + modName + ".", Logger.Level.Debug);
			if (PluginManager.GetPlugin(modName).Name == modName || PluginManager.GetPlugin(modName).Id == modName)
			{
				return true;
			}
			Logger.Log(modName + " not found in BSIPA. Looking through the legacy list instead...", Logger.Level.Debug);
			if (PluginManager.GetPlugin(modName).Name == modName)
			{
				return true;
			}
			Logger.Log(modName + " was not found.", Logger.Level.Debug);
			return false;
		}

		// Token: 0x0400009B RID: 155
		private static Dictionary<string, Texture2D> _cachedTextures = new Dictionary<string, Texture2D>();

		// Token: 0x0400009C RID: 156
		private static Dictionary<string, Image> _cachedImages = new Dictionary<string, Image>();
	}
}
