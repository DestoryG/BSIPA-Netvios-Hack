using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Ionic.Zip
{
	// Token: 0x02000024 RID: 36
	internal static class SharedUtilities
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00005B44 File Offset: 0x00003D44
		public static long GetFileLength(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException(fileName);
			}
			long num = 0L;
			FileShare fileShare = FileShare.ReadWrite;
			fileShare |= FileShare.Delete;
			using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, fileShare))
			{
				num = fileStream.Length;
			}
			return num;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005B98 File Offset: 0x00003D98
		[Conditional("NETCF")]
		public static void Workaround_Ladybug318918(Stream s)
		{
			s.Flush();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005BA0 File Offset: 0x00003DA0
		private static string SimplifyFwdSlashPath(string path)
		{
			if (path.StartsWith("./"))
			{
				path = path.Substring(2);
			}
			path = path.Replace("/./", "/");
			path = SharedUtilities.doubleDotRegex1.Replace(path, "$1$3");
			return path;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005BE0 File Offset: 0x00003DE0
		public static string NormalizePathForUseInZipFile(string pathName)
		{
			if (string.IsNullOrEmpty(pathName))
			{
				return pathName;
			}
			if (pathName.Length >= 2 && pathName[1] == ':' && pathName[2] == '\\')
			{
				pathName = pathName.Substring(3);
			}
			pathName = pathName.Replace('\\', '/');
			while (pathName.StartsWith("/"))
			{
				pathName = pathName.Substring(1);
			}
			return SharedUtilities.SimplifyFwdSlashPath(pathName);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005C4C File Offset: 0x00003E4C
		internal static byte[] StringToByteArray(string value, Encoding encoding)
		{
			return encoding.GetBytes(value);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005C62 File Offset: 0x00003E62
		internal static byte[] StringToByteArray(string value)
		{
			return SharedUtilities.StringToByteArray(value, SharedUtilities.ibm437);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005C6F File Offset: 0x00003E6F
		internal static string Utf8StringFromBuffer(byte[] buf)
		{
			return SharedUtilities.StringFromBuffer(buf, SharedUtilities.utf8);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005C7C File Offset: 0x00003E7C
		internal static string StringFromBuffer(byte[] buf, Encoding encoding)
		{
			return encoding.GetString(buf, 0, buf.Length);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005C98 File Offset: 0x00003E98
		internal static int ReadSignature(Stream s)
		{
			int num = 0;
			try
			{
				num = SharedUtilities._ReadFourBytes(s, "n/a");
			}
			catch (BadReadException)
			{
			}
			return num;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005CCC File Offset: 0x00003ECC
		internal static int ReadEntrySignature(Stream s)
		{
			int num = 0;
			try
			{
				num = SharedUtilities._ReadFourBytes(s, "n/a");
				if (num == 134695760)
				{
					s.Seek(12L, SeekOrigin.Current);
					num = SharedUtilities._ReadFourBytes(s, "n/a");
					if (num != 67324752)
					{
						s.Seek(8L, SeekOrigin.Current);
						num = SharedUtilities._ReadFourBytes(s, "n/a");
						if (num != 67324752)
						{
							s.Seek(-24L, SeekOrigin.Current);
							num = SharedUtilities._ReadFourBytes(s, "n/a");
						}
					}
				}
			}
			catch (BadReadException)
			{
			}
			return num;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005D5C File Offset: 0x00003F5C
		internal static int ReadInt(Stream s)
		{
			return SharedUtilities._ReadFourBytes(s, "Could not read block - no data!  (position 0x{0:X8})");
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005D6C File Offset: 0x00003F6C
		private static int _ReadFourBytes(Stream s, string message)
		{
			byte[] array = new byte[4];
			int num = s.Read(array, 0, array.Length);
			if (num != array.Length)
			{
				throw new BadReadException(string.Format(message, s.Position));
			}
			return (((int)array[3] * 256 + (int)array[2]) * 256 + (int)array[1]) * 256 + (int)array[0];
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005DD0 File Offset: 0x00003FD0
		internal static long FindSignature(Stream stream, int SignatureToFind)
		{
			long position = stream.Position;
			int num = 65536;
			byte[] array = new byte[]
			{
				(byte)(SignatureToFind >> 24),
				(byte)((SignatureToFind & 16711680) >> 16),
				(byte)((SignatureToFind & 65280) >> 8),
				(byte)(SignatureToFind & 255)
			};
			byte[] array2 = new byte[num];
			bool flag = false;
			do
			{
				int num2 = stream.Read(array2, 0, array2.Length);
				if (num2 == 0)
				{
					break;
				}
				for (int i = 0; i < num2; i++)
				{
					if (array2[i] == array[3])
					{
						long position2 = stream.Position;
						stream.Seek((long)(i - num2), SeekOrigin.Current);
						int num3 = SharedUtilities.ReadSignature(stream);
						flag = num3 == SignatureToFind;
						if (flag)
						{
							break;
						}
						stream.Seek(position2, SeekOrigin.Begin);
					}
				}
			}
			while (!flag);
			if (!flag)
			{
				stream.Seek(position, SeekOrigin.Begin);
				return -1L;
			}
			return stream.Position - position - 4L;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005EB0 File Offset: 0x000040B0
		internal static DateTime AdjustTime_Reverse(DateTime time)
		{
			if (time.Kind == DateTimeKind.Utc)
			{
				return time;
			}
			DateTime dateTime = time;
			if (DateTime.Now.IsDaylightSavingTime() && !time.IsDaylightSavingTime())
			{
				dateTime = time - new TimeSpan(1, 0, 0);
			}
			else if (!DateTime.Now.IsDaylightSavingTime() && time.IsDaylightSavingTime())
			{
				dateTime = time + new TimeSpan(1, 0, 0);
			}
			return dateTime;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005F1C File Offset: 0x0000411C
		internal static DateTime PackedToDateTime(int packedDateTime)
		{
			if (packedDateTime == 65535 || packedDateTime == 0)
			{
				return new DateTime(1995, 1, 1, 0, 0, 0, 0);
			}
			short num = (short)(packedDateTime & 65535);
			short num2 = (short)(((long)packedDateTime & (long)((ulong)(-65536))) >> 16);
			int i = 1980 + (((int)num2 & 65024) >> 9);
			int j = (num2 & 480) >> 5;
			int k = (int)(num2 & 31);
			int num3 = ((int)num & 63488) >> 11;
			int l = (num & 2016) >> 5;
			int m = (int)((num & 31) * 2);
			if (m >= 60)
			{
				l++;
				m = 0;
			}
			if (l >= 60)
			{
				num3++;
				l = 0;
			}
			if (num3 >= 24)
			{
				k++;
				num3 = 0;
			}
			DateTime dateTime = DateTime.Now;
			bool flag = false;
			try
			{
				dateTime = new DateTime(i, j, k, num3, l, m, 0);
				flag = true;
			}
			catch (ArgumentOutOfRangeException)
			{
				if (i == 1980)
				{
					if (j != 0)
					{
						if (k != 0)
						{
							goto IL_0111;
						}
					}
					try
					{
						dateTime = new DateTime(1980, 1, 1, num3, l, m, 0);
						flag = true;
						goto IL_01AD;
					}
					catch (ArgumentOutOfRangeException)
					{
						try
						{
							dateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
							flag = true;
						}
						catch (ArgumentOutOfRangeException)
						{
						}
						goto IL_01AD;
					}
				}
				try
				{
					IL_0111:
					while (i < 1980)
					{
						i++;
					}
					while (i > 2030)
					{
						i--;
					}
					while (j < 1)
					{
						j++;
					}
					while (j > 12)
					{
						j--;
					}
					while (k < 1)
					{
						k++;
					}
					while (k > 28)
					{
						k--;
					}
					while (l < 0)
					{
						l++;
					}
					while (l > 59)
					{
						l--;
					}
					while (m < 0)
					{
						m++;
					}
					while (m > 59)
					{
						m--;
					}
					dateTime = new DateTime(i, j, k, num3, l, m, 0);
					flag = true;
				}
				catch (ArgumentOutOfRangeException)
				{
				}
				IL_01AD:;
			}
			if (!flag)
			{
				string text = string.Format("y({0}) m({1}) d({2}) h({3}) m({4}) s({5})", new object[] { i, j, k, num3, l, m });
				throw new ZipException(string.Format("Bad date/time format in the zip file. ({0})", text));
			}
			dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
			return dateTime;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006184 File Offset: 0x00004384
		internal static int DateTimeToPacked(DateTime time)
		{
			time = time.ToLocalTime();
			ushort num = (ushort)((time.Day & 31) | ((time.Month << 5) & 480) | ((time.Year - 1980 << 9) & 65024));
			ushort num2 = (ushort)(((time.Second / 2) & 31) | ((time.Minute << 5) & 2016) | ((time.Hour << 11) & 63488));
			return ((int)num << 16) | (int)num2;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006204 File Offset: 0x00004404
		public static void CreateAndOpenUniqueTempFile(string dir, out Stream fs, out string filename)
		{
			for (int i = 0; i < 3; i++)
			{
				try
				{
					filename = Path.Combine(dir, SharedUtilities.InternalGetTempFileName());
					fs = new FileStream(filename, FileMode.CreateNew);
					return;
				}
				catch (IOException)
				{
					if (i == 2)
					{
						throw;
					}
				}
			}
			throw new IOException();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006258 File Offset: 0x00004458
		public static string InternalGetTempFileName()
		{
			return "DotNetZip-" + Path.GetRandomFileName().Substring(0, 8) + ".tmp";
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006278 File Offset: 0x00004478
		internal static int ReadWithRetry(Stream s, byte[] buffer, int offset, int count, string FileName)
		{
			int num = 0;
			bool flag = false;
			int num2 = 0;
			do
			{
				try
				{
					num = s.Read(buffer, offset, count);
					flag = true;
				}
				catch (IOException ex)
				{
					SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
					if (!securityPermission.IsUnrestricted())
					{
						throw;
					}
					uint num3 = SharedUtilities._HRForException(ex);
					if (num3 != 2147942433U)
					{
						throw new IOException(string.Format("Cannot read file {0}", FileName), ex);
					}
					num2++;
					if (num2 > 10)
					{
						throw new IOException(string.Format("Cannot read file {0}, at offset 0x{1:X8} after 10 retries", FileName, offset), ex);
					}
					Thread.Sleep(250 + num2 * 550);
				}
			}
			while (!flag);
			return num;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006320 File Offset: 0x00004520
		private static uint _HRForException(Exception ex1)
		{
			return (uint)Marshal.GetHRForException(ex1);
		}

		// Token: 0x040000B3 RID: 179
		private static Regex doubleDotRegex1 = new Regex("^(.*/)?([^/\\\\.]+/\\\\.\\\\./)(.+)$");

		// Token: 0x040000B4 RID: 180
		private static Encoding ibm437 = Encoding.GetEncoding("IBM437");

		// Token: 0x040000B5 RID: 181
		private static Encoding utf8 = Encoding.GetEncoding("UTF-8");
	}
}
