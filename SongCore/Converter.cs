using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using IPA.Logging;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore
{
	// Token: 0x02000011 RID: 17
	public class Converter
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00003744 File Offset: 0x00001944
		public static void PrepareExistingLibrary()
		{
			Logging.Log("Attempting to Convert Existing Library");
			if (!Directory.Exists(Converter.oldFolderPath))
			{
				Logging.Log("No Existing Library to Convert", Logger.Level.Notice);
				return;
			}
			Utils.GrantAccess(Converter.oldFolderPath);
			Loader.Instance._progressBar.ShowMessage("Converting Existing Song Library");
			List<string> list = Directory.GetDirectories(Converter.oldFolderPath).ToList<string>();
			float num = 0f;
			foreach (string text in list)
			{
				num += 1f;
				if (Directory.Exists(text))
				{
					string[] files = Directory.GetFiles(text, "info.json", SearchOption.AllDirectories);
					for (int i = 0; i < files.Length; i++)
					{
						string directoryName = Path.GetDirectoryName(files[i].Replace('\\', '/'));
						if (Directory.Exists(directoryName) && Directory.GetFiles(directoryName, "info.dat").Count<string>() <= 0)
						{
							string text2 = directoryName;
							DirectoryInfo parent = Directory.GetParent(directoryName);
							if (parent.Name != "CustomSongs")
							{
								try
								{
									text2 = string.Concat(new string[]
									{
										Converter.oldFolderPath,
										"/",
										parent.Name,
										" ",
										new DirectoryInfo(directoryName).Name
									});
									if (Directory.Exists(text2))
									{
										int num2 = 1;
										while (Directory.Exists(text2 + string.Format(" ({0})", num2)))
										{
											num2++;
										}
										text2 += string.Format(" ({0})", num2);
									}
									Directory.Move(directoryName, text2);
									if (Utils.IsDirectoryEmpty(parent.FullName))
									{
										Directory.Delete(parent.FullName);
									}
								}
								catch (Exception ex)
								{
									Logging.Log(string.Format("Error attempting to correct Subfolder {0}: \n {1}", directoryName, ex), Logger.Level.Error);
								}
							}
							Converter.ToConvert.Push(text2);
						}
					}
				}
			}
			if (File.Exists(Converter.oldFolderPath + "/../songe-converter.exe"))
			{
				Loader.Instance.StartCoroutine(Converter.ConvertSongs());
				return;
			}
			Logging.Log("Missing Songe converter, not converting", Logger.Level.Notice);
			Loader.Instance.RefreshSongs(true);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000039B0 File Offset: 0x00001BB0
		internal static IEnumerator ConvertSongs()
		{
			int totalSongs = Converter.ToConvert.Count;
			Loader.Instance._progressBar.ShowMessage(string.Format("Converting {0} Existing Songs. Please Wait...", totalSongs));
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo
			{
				WindowStyle = ProcessWindowStyle.Normal,
				FileName = "cmd.exe",
				Arguments = "/C songe-converter.exe -k -a \"" + Converter.oldFolderPath + "\""
			};
			process.EnableRaisingEvents = true;
			process.Exited += Converter.Process_Exited;
			process.Start();
			yield return new WaitUntil(() => Converter.doneConverting);
			Logging.Log(string.Format("Converted {0} songs.", totalSongs));
			Converter.FinishConversion();
			yield break;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000039B8 File Offset: 0x00001BB8
		private static void Process_Exited(object sender, EventArgs e)
		{
			Converter.doneConverting = true;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000039C0 File Offset: 0x00001BC0
		internal static void FinishConversion()
		{
			if (Directory.Exists(Converter.oldFolderPath))
			{
				Logging.Log("Moving CustomSongs folder to new Location");
				string text = CustomLevelPathHelper.customLevelsDirectoryPath.Replace("CustomLevels", Plugin.costomLevelsDirName);
				if (Directory.Exists(text))
				{
					Utils.GrantAccess(text);
					Directory.Move(text, text + DateTime.Now.ToFileTime().ToString());
				}
				Utils.GrantAccess(Converter.oldFolderPath);
				Directory.Move(Converter.oldFolderPath, text);
			}
			Logging.Log("Conversion Finished. Loading songs");
			Loader.Instance.RefreshSongs(true);
		}

		// Token: 0x04000025 RID: 37
		internal static int ConcurrentProcesses = 4;

		// Token: 0x04000026 RID: 38
		internal static int ActiveProcesses = 0;

		// Token: 0x04000027 RID: 39
		internal static int ConvertedCount = 0;

		// Token: 0x04000028 RID: 40
		internal static bool doneConverting = false;

		// Token: 0x04000029 RID: 41
		public static Stack<string> ToConvert = new Stack<string>();

		// Token: 0x0400002A RID: 42
		public static string oldFolderPath = Environment.CurrentDirectory + "/CustomSongs";
	}
}
