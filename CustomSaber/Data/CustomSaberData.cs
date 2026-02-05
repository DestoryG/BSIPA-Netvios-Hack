using System;
using System.IO;
using CustomSaber.Utilities;
using UnityEngine;

namespace CustomSaber.Data
{
	// Token: 0x0200001A RID: 26
	public class CustomSaberData
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005350 File Offset: 0x00003550
		public string FileName { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005358 File Offset: 0x00003558
		public AssetBundle AssetBundle { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005360 File Offset: 0x00003560
		public SaberDescriptor Descriptor { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00005368 File Offset: 0x00003568
		public GameObject Sabers { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005370 File Offset: 0x00003570
		public string ErrorMessage { get; } = string.Empty;

		// Token: 0x060000C4 RID: 196 RVA: 0x00005378 File Offset: 0x00003578
		public CustomSaberData(string fileName)
		{
			this.FileName = fileName;
			bool flag = fileName != "DefaultSabers";
			if (flag)
			{
				try
				{
					this.AssetBundle = AssetBundle.LoadFromFile(Path.Combine(Plugin.PluginAssetPath, fileName));
					this.Sabers = this.AssetBundle.LoadAsset<GameObject>("_CustomSaber");
					this.Descriptor = this.Sabers.GetComponent<SaberDescriptor>();
					this.Descriptor.CoverImage = this.Descriptor.CoverImage ?? Utils.GetDefaultCoverImage();
				}
				catch
				{
					Logger.log.Warn("Something went wrong getting the AssetBundle for '" + fileName + "'!");
					this.Descriptor = new SaberDescriptor
					{
						SaberName = "Invalid Saber (Delete it)",
						AuthorName = fileName,
						CoverImage = Utils.GetErrorCoverImage()
					};
					this.ErrorMessage = "File: '" + fileName + "'\n\nThis file failed to load.\n\nThis may have been caused by having duplicated files, another saber with the same name already exists or that the custom saber is simply just broken.\n\nThe best thing is probably just to delete it!";
					this.FileName = "DefaultSabers";
				}
			}
			else
			{
				this.Descriptor = new SaberDescriptor
				{
					SaberName = "Default",
					AuthorName = "Beat Saber",
					Description = "This is the default sabers. (No preview available)",
					CoverImage = Utils.GetRandomCoverImage()
				};
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000054C8 File Offset: 0x000036C8
		public CustomSaberData(GameObject leftSaber, GameObject rightSaber)
		{
			this.FileName = "DefaultSabers";
			this.Descriptor = new SaberDescriptor
			{
				SaberName = "Default",
				AuthorName = "Beat Games",
				Description = "This is the default sabers.",
				CoverImage = Utils.GetRandomCoverImage()
			};
			GameObject gameObject = new GameObject();
			bool flag = gameObject;
			if (flag)
			{
				leftSaber.transform.SetParent(gameObject.transform);
				rightSaber.transform.SetParent(gameObject.transform);
			}
			this.Sabers = gameObject;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005568 File Offset: 0x00003768
		public void Destroy()
		{
			bool flag = this.AssetBundle != null;
			if (flag)
			{
				this.AssetBundle.Unload(true);
			}
			else
			{
				Object.Destroy(this.Descriptor);
			}
		}
	}
}
