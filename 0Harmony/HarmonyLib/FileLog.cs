using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x0200009F RID: 159
	public static class FileLog
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000F490 File Offset: 0x0000D690
		private static string IndentString()
		{
			return new string(FileLog.indentChar, FileLog.indentLevel);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000F4A1 File Offset: 0x0000D6A1
		public static void ChangeIndent(int delta)
		{
			FileLog.indentLevel = Math.Max(0, FileLog.indentLevel + delta);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		public static void LogBuffered(string str)
		{
			string text = FileLog.logPath;
			lock (text)
			{
				FileLog.buffer.Add(FileLog.IndentString() + str);
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000F508 File Offset: 0x0000D708
		public static void LogBuffered(List<string> strings)
		{
			string text = FileLog.logPath;
			lock (text)
			{
				FileLog.buffer.AddRange(strings);
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000F54C File Offset: 0x0000D74C
		public static List<string> GetBuffer(bool clear)
		{
			string text = FileLog.logPath;
			List<string> list2;
			lock (text)
			{
				List<string> list = FileLog.buffer;
				if (clear)
				{
					FileLog.buffer = new List<string>();
				}
				list2 = list;
			}
			return list2;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000F59C File Offset: 0x0000D79C
		public static void SetBuffer(List<string> buffer)
		{
			string text = FileLog.logPath;
			lock (text)
			{
				FileLog.buffer = buffer;
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000F5DC File Offset: 0x0000D7DC
		public static void FlushBuffer()
		{
			string text = FileLog.logPath;
			lock (text)
			{
				if (FileLog.buffer.Count > 0)
				{
					using (StreamWriter streamWriter = File.AppendText(FileLog.logPath))
					{
						foreach (string text2 in FileLog.buffer)
						{
							streamWriter.WriteLine(text2);
						}
					}
					FileLog.buffer.Clear();
				}
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000F690 File Offset: 0x0000D890
		public static void Log(string str)
		{
			string text = FileLog.logPath;
			lock (text)
			{
				using (StreamWriter streamWriter = File.AppendText(FileLog.logPath))
				{
					streamWriter.WriteLine(FileLog.IndentString() + str);
				}
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000F6FC File Offset: 0x0000D8FC
		public static void Reset()
		{
			string text = FileLog.logPath;
			lock (text)
			{
				File.Delete(string.Format("{0}{1}harmony.log.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.DirectorySeparatorChar));
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000F754 File Offset: 0x0000D954
		public unsafe static void LogBytes(long ptr, int len)
		{
			string text = FileLog.logPath;
			lock (text)
			{
				byte* ptr2 = ptr;
				string text2 = "";
				for (int i = 1; i <= len; i++)
				{
					if (text2.Length == 0)
					{
						text2 = "#  ";
					}
					text2 = text2 + ptr2->ToString("X2") + " ";
					if (i > 1 || len == 1)
					{
						if (i % 8 == 0 || i == len)
						{
							FileLog.Log(text2);
							text2 = "";
						}
						else if (i % 4 == 0)
						{
							text2 += " ";
						}
					}
					ptr2++;
				}
				byte[] array = new byte[len];
				Marshal.Copy((IntPtr)ptr, array, 0, len);
				byte[] array2 = MD5.Create().ComputeHash(array);
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < array2.Length; j++)
				{
					stringBuilder.Append(array2[j].ToString("X2"));
				}
				FileLog.Log(string.Format("HASH: {0}", stringBuilder));
			}
		}

		// Token: 0x040001C3 RID: 451
		public static string logPath = string.Format("{0}{1}harmony.log.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.DirectorySeparatorChar);

		// Token: 0x040001C4 RID: 452
		public static char indentChar = '\t';

		// Token: 0x040001C5 RID: 453
		public static int indentLevel = 0;

		// Token: 0x040001C6 RID: 454
		private static List<string> buffer = new List<string>();
	}
}
