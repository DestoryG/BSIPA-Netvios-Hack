using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IPA.Logging;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x0200000C RID: 12
	public static class Utilities
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003C78 File Offset: 0x00001E78
		public static Sprite EditIcon
		{
			get
			{
				if (!Utilities._editIcon)
				{
					Utilities._editIcon = Resources.FindObjectsOfTypeAll<Image>().First(delegate(Image x)
					{
						Sprite sprite = x.sprite;
						return ((sprite != null) ? sprite.name : null) == "EditIcon";
					}).sprite;
				}
				return Utilities._editIcon;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003CCC File Offset: 0x00001ECC
		public static string GetResourceContent(Assembly assembly, string resource)
		{
			string text;
			using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resource))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					text = streamReader.ReadToEnd();
				}
			}
			return text;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003D24 File Offset: 0x00001F24
		public static List<T> GetListOfType<T>(params object[] constructorArgs)
		{
			List<T> list = new List<T>();
			foreach (Type type in from myType in Assembly.GetAssembly(typeof(T)).GetTypes()
				where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))
				select myType)
			{
				list.Add((T)((object)Activator.CreateInstance(type, constructorArgs)));
			}
			return list;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public static T GetCopyOf<T>(this Component comp, T other) where T : Component
		{
			Type type = comp.GetType();
			if (type != other.GetType())
			{
				return default(T);
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags))
			{
				if (propertyInfo.CanWrite && propertyInfo.Name != "name")
				{
					try
					{
						propertyInfo.SetValue(comp, propertyInfo.GetValue(other, null), null);
					}
					catch
					{
					}
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields(bindingFlags))
			{
				fieldInfo.SetValue(comp, fieldInfo.GetValue(other));
			}
			return comp as T;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003E98 File Offset: 0x00002098
		public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
		{
			return go.AddComponent<T>().GetCopyOf(toAdd);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003EAC File Offset: 0x000020AC
		public static string EscapeXml(string source)
		{
			return source.Replace("\"", "&quot;").Replace("\"", "&quot;").Replace("&", "&amp;")
				.Replace("'", "&apos;")
				.Replace("<", "&lt;")
				.Replace(">", "&gt;");
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003F14 File Offset: 0x00002114
		public static Texture2D FindTextureInAssembly(string path)
		{
			try
			{
				Assembly assembly;
				string text;
				Utilities.AssemblyFromPath(path, out assembly, out text);
				if (assembly.GetManifestResourceNames().Contains(text))
				{
					return Utilities.LoadTextureRaw(Utilities.GetResource(assembly, text));
				}
			}
			catch (Exception ex)
			{
				Logger log = Logger.log;
				if (log != null)
				{
					string text2 = "Unable to find texture in assembly! (You must prefix path with 'assembly name:' if the assembly and root namespace don't have the same name) Exception: ";
					Exception ex2 = ex;
					log.Error(text2 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			return null;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003F88 File Offset: 0x00002188
		public static Sprite FindSpriteInAssembly(string path)
		{
			try
			{
				Assembly assembly;
				string text;
				Utilities.AssemblyFromPath(path, out assembly, out text);
				if (assembly.GetManifestResourceNames().Contains(text))
				{
					return Utilities.LoadSpriteRaw(Utilities.GetResource(assembly, text), 100f);
				}
			}
			catch (Exception ex)
			{
				Logger log = Logger.log;
				if (log != null)
				{
					string text2 = "Unable to find sprite in assembly! (You must prefix path with 'assembly name:' if the assembly and root namespace don't have the same name) Exception: ";
					Exception ex2 = ex;
					log.Error(text2 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			return null;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004000 File Offset: 0x00002200
		public static void AssemblyFromPath(string inputPath, out Assembly assembly, out string path)
		{
			string[] array = inputPath.Split(new char[] { ':' });
			int num = array.Length;
			if (num == 1)
			{
				path = array[0];
				assembly = Assembly.Load(path.Substring(0, path.IndexOf('.')));
				return;
			}
			if (num != 2)
			{
				throw new Exception("Could not process resource path " + inputPath);
			}
			path = array[1];
			assembly = Assembly.Load(array[0]);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000406C File Offset: 0x0000226C
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

		// Token: 0x0600005D RID: 93 RVA: 0x00004096 File Offset: 0x00002296
		public static Sprite LoadSpriteRaw(byte[] image, float PixelsPerUnit = 100f)
		{
			return Utilities.LoadSpriteFromTexture(Utilities.LoadTextureRaw(image), PixelsPerUnit);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000040A4 File Offset: 0x000022A4
		public static Sprite LoadSpriteFromTexture(Texture2D SpriteTexture, float PixelsPerUnit = 100f)
		{
			if (SpriteTexture)
			{
				return Sprite.Create(SpriteTexture, new Rect(0f, 0f, (float)SpriteTexture.width, (float)SpriteTexture.height), new Vector2(0f, 0f), PixelsPerUnit);
			}
			return null;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000040E4 File Offset: 0x000022E4
		public static byte[] GetResource(Assembly asm, string ResourceName)
		{
			Stream manifestResourceStream = asm.GetManifestResourceStream(ResourceName);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			return array;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004117 File Offset: 0x00002317
		public static IEnumerable<T> SingleEnumerable<T>(this T item)
		{
			return Enumerable.Empty<T>().Append(item);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004124 File Offset: 0x00002324
		public static IEnumerable<T?> AsNullable<T>(this IEnumerable<T> seq) where T : struct
		{
			return seq.Select((T v) => new T?(v));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000414B File Offset: 0x0000234B
		public static T? AsNullable<T>(this T item) where T : struct
		{
			return new T?(item);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004154 File Offset: 0x00002354
		public static void GetData(string location, Action<byte[]> callback)
		{
			try
			{
				if (location.StartsWith("http://") || location.StartsWith("https://"))
				{
					PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(Utilities.GetWebDataCoroutine(location, callback));
				}
				else if (File.Exists(location))
				{
					if (callback != null)
					{
						callback(File.ReadAllBytes(location));
					}
				}
				else
				{
					Assembly assembly;
					string text;
					Utilities.AssemblyFromPath(location, out assembly, out text);
					if (callback != null)
					{
						callback(Utilities.GetResource(assembly, text));
					}
				}
			}
			catch
			{
				Logger.log.Error("Error getting data from '" + location + "' either invalid path or file does not exist");
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000041F4 File Offset: 0x000023F4
		private static IEnumerator GetWebDataCoroutine(string url, Action<byte[]> callback)
		{
			UnityWebRequest www = UnityWebRequest.Get(url);
			yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError)
			{
				Logger.log.Debug("Error getting data from " + url + ", Message:" + www.error);
			}
			else if (callback != null)
			{
				callback(www.downloadHandler.data);
			}
			yield break;
		}

		// Token: 0x0400001C RID: 28
		private static Sprite _editIcon;

		// Token: 0x020000EC RID: 236
		public static class ImageResources
		{
			// Token: 0x17000132 RID: 306
			// (get) Token: 0x06000529 RID: 1321 RVA: 0x000156C4 File Offset: 0x000138C4
			public static Material NoGlowMat
			{
				get
				{
					if (Utilities.ImageResources.noGlowMat == null)
					{
						Utilities.ImageResources.noGlowMat = new Material((from m in Resources.FindObjectsOfTypeAll<Material>()
							where m.name == "UINoGlow"
							select m).First<Material>());
						Utilities.ImageResources.noGlowMat.name = "UINoGlowCustom";
					}
					return Utilities.ImageResources.noGlowMat;
				}
			}

			// Token: 0x17000133 RID: 307
			// (get) Token: 0x0600052A RID: 1322 RVA: 0x0001572C File Offset: 0x0001392C
			public static Sprite BlankSprite
			{
				get
				{
					if (!Utilities.ImageResources._blankSprite)
					{
						Utilities.ImageResources._blankSprite = Sprite.Create(Texture2D.blackTexture, default(Rect), Vector2.zero);
					}
					return Utilities.ImageResources._blankSprite;
				}
			}

			// Token: 0x17000134 RID: 308
			// (get) Token: 0x0600052B RID: 1323 RVA: 0x00015768 File Offset: 0x00013968
			public static Sprite WhitePixel
			{
				get
				{
					if (!Utilities.ImageResources._whitePixel)
					{
						Utilities.ImageResources._whitePixel = Resources.FindObjectsOfTypeAll<Image>().First(delegate(Image i)
						{
							Sprite sprite = i.sprite;
							return ((sprite != null) ? sprite.name : null) == "WhitePixel";
						}).sprite;
					}
					return Utilities.ImageResources._whitePixel;
				}
			}

			// Token: 0x040001C6 RID: 454
			private static Material noGlowMat;

			// Token: 0x040001C7 RID: 455
			private static Sprite _blankSprite;

			// Token: 0x040001C8 RID: 456
			private static Sprite _whitePixel;
		}
	}
}
