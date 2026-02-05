using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace DynamicOpenVR
{
	// Token: 0x020000CC RID: 204
	public static class SteamUtilities
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00005CA8 File Offset: 0x00003EA8
		public static string GetSteamHomeDirectory()
		{
			string text = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", string.Empty).ToString();
			if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
			{
				string text2;
				if (SteamUtilities.TryGetExactPath(text, out text2))
				{
					return text2;
				}
				return text;
			}
			else
			{
				Process process = Process.GetProcessesByName("Steam").FirstOrDefault<Process>();
				if (process == null)
				{
					throw new Exception("Steam process could not be found.");
				}
				StringBuilder stringBuilder = new StringBuilder(2048);
				int num = stringBuilder.Capacity + 1;
				if (NativeMethods.QueryFullProcessImageName(process.Handle, 0, stringBuilder, ref num) == 0)
				{
					throw new Exception("QueryFullProcessImageName returned 0");
				}
				string text3 = stringBuilder.ToString();
				if (string.IsNullOrEmpty(text3))
				{
					throw new Exception("Steam path could not be found.");
				}
				return Path.GetDirectoryName(text3);
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00005D5C File Offset: 0x00003F5C
		private static bool TryGetExactPath(string path, out string exactPath)
		{
			bool flag = false;
			exactPath = null;
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (File.Exists(path) || directoryInfo.Exists)
			{
				List<string> list = new List<string>();
				for (DirectoryInfo directoryInfo2 = directoryInfo.Parent; directoryInfo2 != null; directoryInfo2 = directoryInfo.Parent)
				{
					FileSystemInfo fileSystemInfo = directoryInfo2.EnumerateFileSystemInfos(directoryInfo.Name).First<FileSystemInfo>();
					list.Add(fileSystemInfo.Name);
					directoryInfo = directoryInfo2;
				}
				string text = directoryInfo.FullName;
				if (text.Contains(':'))
				{
					text = text.ToUpper();
				}
				else
				{
					string[] array = text.Split(new char[] { '\\' });
					text = string.Join("\\", array.Select((string part) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(part)));
				}
				list.Add(text);
				list.Reverse();
				exactPath = Path.Combine(list.ToArray());
				flag = true;
			}
			return flag;
		}
	}
}
