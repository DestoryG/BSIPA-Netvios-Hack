using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using IPA.Logging;
using SongCore.OverrideClasses;
using SongCore.UI;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore.Data
{
	// Token: 0x02000030 RID: 48
	public class SeperateSongFolder
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x000084FD File Offset: 0x000066FD
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00008505 File Offset: 0x00006705
		public SongFolderEntry SongFolderEntry { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000850E File Offset: 0x0000670E
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00008516 File Offset: 0x00006716
		public SongCoreCustomLevelCollection LevelCollection { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000851F File Offset: 0x0000671F
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00008527 File Offset: 0x00006727
		public SongCoreCustomBeatmapLevelPack LevelPack { get; private set; }

		// Token: 0x060001AC RID: 428 RVA: 0x00008530 File Offset: 0x00006730
		public SeperateSongFolder(SongFolderEntry folderEntry)
		{
			this.SongFolderEntry = folderEntry;
			if (folderEntry.Pack == FolderLevelPack.NewPack)
			{
				this.LevelCollection = new SongCoreCustomLevelCollection(this.Levels.Values.ToArray<CustomPreviewBeatmapLevel>());
				Sprite sprite = BasicUI.FolderIcon;
				if (!string.IsNullOrEmpty(folderEntry.ImagePath))
				{
					try
					{
						Sprite sprite2 = Utils.LoadSpriteFromFile(folderEntry.ImagePath, 100f);
						if (sprite2 != null)
						{
							sprite = sprite2;
						}
					}
					catch
					{
						Logging.Log("Failed to Load Image For Seperate Folder \"" + folderEntry.Name + "\"");
					}
				}
				this.LevelPack = new SongCoreCustomBeatmapLevelPack("custom_levelpack_" + folderEntry.Name, folderEntry.Name, sprite, this.LevelCollection, "");
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008608 File Offset: 0x00006808
		public SeperateSongFolder(SongFolderEntry folderEntry, Sprite Image)
		{
			this.SongFolderEntry = folderEntry;
			if (folderEntry.Pack == FolderLevelPack.NewPack)
			{
				this.LevelCollection = new SongCoreCustomLevelCollection(this.Levels.Values.ToArray<CustomPreviewBeatmapLevel>());
				this.LevelPack = new SongCoreCustomBeatmapLevelPack("custom_levelpack_" + folderEntry.Name, folderEntry.Name, Image, this.LevelCollection, "");
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008680 File Offset: 0x00006880
		public static List<SeperateSongFolder> ReadSeperateFoldersFromFile(string filePath)
		{
			List<SeperateSongFolder> list = new List<SeperateSongFolder>();
			try
			{
				string text = Environment.CurrentDirectory;
				text = text.Replace('\\', '/');
				foreach (XElement xelement in XDocument.Load(filePath).Root.Elements())
				{
					string value = xelement.Element("Name").Value;
					if (!(value == "Example"))
					{
						string text2 = Path.Combine(text, xelement.Element("Path").Value);
						int num = int.Parse(xelement.Element("Pack").Value);
						string text3 = "";
						XElement xelement2 = xelement.Element("ImagePath");
						if (xelement2 != null)
						{
							text3 = Path.Combine(text, xelement2.Value);
						}
						bool flag = false;
						XElement xelement3 = xelement.Element("WIP");
						if (xelement3 != null)
						{
							flag = bool.Parse(xelement3.Value);
						}
						SongFolderEntry songFolderEntry = new SongFolderEntry(value, text2, (FolderLevelPack)num, text3, flag);
						list.Add(new SeperateSongFolder(songFolderEntry));
					}
				}
			}
			catch
			{
				Logging.Log("Error Reading folders.xml! Make sure the file is properly formatted.", Logger.Level.Warning);
			}
			return list;
		}

		// Token: 0x040000A3 RID: 163
		public Dictionary<string, CustomPreviewBeatmapLevel> Levels = new Dictionary<string, CustomPreviewBeatmapLevel>();
	}
}
