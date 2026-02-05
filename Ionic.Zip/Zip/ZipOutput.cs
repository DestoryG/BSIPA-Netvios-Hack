using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
	// Token: 0x0200003F RID: 63
	internal static class ZipOutput
	{
		// Token: 0x06000316 RID: 790 RVA: 0x000113E8 File Offset: 0x0000F5E8
		public static bool WriteCentralDirectoryStructure(Stream s, ICollection<ZipEntry> entries, uint numSegments, Zip64Option zip64, string comment, ZipContainer container)
		{
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = true;
			}
			long num = 0L;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				foreach (ZipEntry zipEntry in entries)
				{
					if (zipEntry.IncludedInMostRecentSave)
					{
						zipEntry.WriteCentralDirectoryEntry(memoryStream);
					}
				}
				byte[] array = memoryStream.ToArray();
				s.Write(array, 0, array.Length);
				num = (long)array.Length;
			}
			CountingStream countingStream = s as CountingStream;
			long num2 = ((countingStream != null) ? countingStream.ComputedPosition : s.Position);
			long num3 = num2 - num;
			uint num4 = ((zipSegmentedStream != null) ? zipSegmentedStream.CurrentSegment : 0U);
			long num5 = num2 - num3;
			int num6 = ZipOutput.CountEntries(entries);
			bool flag = zip64 == Zip64Option.Always || num6 >= 65535 || num5 > (long)((ulong)(-1)) || num3 > (long)((ulong)(-1));
			byte[] array3;
			if (flag)
			{
				if (zip64 == Zip64Option.Default)
				{
					StackFrame stackFrame = new StackFrame(1);
					if (stackFrame.GetMethod().DeclaringType == typeof(ZipFile))
					{
						throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipFile.UseZip64WhenSaving property.");
					}
					throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipOutputStream.EnableZip64 property.");
				}
				else
				{
					byte[] array2 = ZipOutput.GenZip64EndOfCentralDirectory(num3, num2, num6, numSegments);
					array3 = ZipOutput.GenCentralDirectoryFooter(num3, num2, zip64, num6, comment, container);
					if (num4 != 0U)
					{
						uint num7 = zipSegmentedStream.ComputeSegment(array2.Length + array3.Length);
						int num8 = 16;
						Array.Copy(BitConverter.GetBytes(num7), 0, array2, num8, 4);
						num8 += 4;
						Array.Copy(BitConverter.GetBytes(num7), 0, array2, num8, 4);
						num8 = 60;
						Array.Copy(BitConverter.GetBytes(num7), 0, array2, num8, 4);
						num8 += 4;
						num8 += 8;
						Array.Copy(BitConverter.GetBytes(num7), 0, array2, num8, 4);
					}
					s.Write(array2, 0, array2.Length);
				}
			}
			else
			{
				array3 = ZipOutput.GenCentralDirectoryFooter(num3, num2, zip64, num6, comment, container);
			}
			if (num4 != 0U)
			{
				ushort num9 = (ushort)zipSegmentedStream.ComputeSegment(array3.Length);
				int num10 = 4;
				Array.Copy(BitConverter.GetBytes(num9), 0, array3, num10, 2);
				num10 += 2;
				Array.Copy(BitConverter.GetBytes(num9), 0, array3, num10, 2);
				num10 += 2;
			}
			s.Write(array3, 0, array3.Length);
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = false;
			}
			return flag;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00011640 File Offset: 0x0000F840
		private static Encoding GetEncoding(ZipContainer container, string t)
		{
			switch (container.AlternateEncodingUsage)
			{
			case ZipOption.Default:
				return container.DefaultEncoding;
			case ZipOption.Always:
				return container.AlternateEncoding;
			}
			Encoding defaultEncoding = container.DefaultEncoding;
			if (t == null)
			{
				return defaultEncoding;
			}
			byte[] bytes = defaultEncoding.GetBytes(t);
			string @string = defaultEncoding.GetString(bytes, 0, bytes.Length);
			if (@string.Equals(t))
			{
				return defaultEncoding;
			}
			return container.AlternateEncoding;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000116A8 File Offset: 0x0000F8A8
		private static byte[] GenCentralDirectoryFooter(long StartOfCentralDirectory, long EndOfCentralDirectory, Zip64Option zip64, int entryCount, string comment, ZipContainer container)
		{
			Encoding encoding = ZipOutput.GetEncoding(container, comment);
			int num = 22;
			byte[] array = null;
			short num2 = 0;
			if (comment != null && comment.Length != 0)
			{
				array = encoding.GetBytes(comment);
				num2 = (short)array.Length;
			}
			num += (int)num2;
			byte[] array2 = new byte[num];
			int num3 = 0;
			byte[] bytes = BitConverter.GetBytes(101010256U);
			Array.Copy(bytes, 0, array2, num3, 4);
			num3 += 4;
			array2[num3++] = 0;
			array2[num3++] = 0;
			array2[num3++] = 0;
			array2[num3++] = 0;
			if (entryCount >= 65535 || zip64 == Zip64Option.Always)
			{
				for (int i = 0; i < 4; i++)
				{
					array2[num3++] = byte.MaxValue;
				}
			}
			else
			{
				array2[num3++] = (byte)(entryCount & 255);
				array2[num3++] = (byte)((entryCount & 65280) >> 8);
				array2[num3++] = (byte)(entryCount & 255);
				array2[num3++] = (byte)((entryCount & 65280) >> 8);
			}
			long num4 = EndOfCentralDirectory - StartOfCentralDirectory;
			if (num4 >= (long)((ulong)(-1)) || StartOfCentralDirectory >= (long)((ulong)(-1)))
			{
				for (int i = 0; i < 8; i++)
				{
					array2[num3++] = byte.MaxValue;
				}
			}
			else
			{
				array2[num3++] = (byte)(num4 & 255L);
				array2[num3++] = (byte)((num4 & 65280L) >> 8);
				array2[num3++] = (byte)((num4 & 16711680L) >> 16);
				array2[num3++] = (byte)((num4 & (long)((ulong)(-16777216))) >> 24);
				array2[num3++] = (byte)(StartOfCentralDirectory & 255L);
				array2[num3++] = (byte)((StartOfCentralDirectory & 65280L) >> 8);
				array2[num3++] = (byte)((StartOfCentralDirectory & 16711680L) >> 16);
				array2[num3++] = (byte)((StartOfCentralDirectory & (long)((ulong)(-16777216))) >> 24);
			}
			if (comment == null || comment.Length == 0)
			{
				array2[num3++] = 0;
				array2[num3++] = 0;
			}
			else
			{
				if ((int)num2 + num3 + 2 > array2.Length)
				{
					num2 = (short)(array2.Length - num3 - 2);
				}
				array2[num3++] = (byte)(num2 & 255);
				array2[num3++] = (byte)(((int)num2 & 65280) >> 8);
				if (num2 != 0)
				{
					int i = 0;
					while (i < (int)num2 && num3 + i < array2.Length)
					{
						array2[num3 + i] = array[i];
						i++;
					}
					num3 += i;
				}
			}
			return array2;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011924 File Offset: 0x0000FB24
		private static byte[] GenZip64EndOfCentralDirectory(long StartOfCentralDirectory, long EndOfCentralDirectory, int entryCount, uint numSegments)
		{
			byte[] array = new byte[76];
			int num = 0;
			byte[] array2 = BitConverter.GetBytes(101075792U);
			Array.Copy(array2, 0, array, num, 4);
			num += 4;
			long num2 = 44L;
			Array.Copy(BitConverter.GetBytes(num2), 0, array, num, 8);
			num += 8;
			array[num++] = 45;
			array[num++] = 0;
			array[num++] = 45;
			array[num++] = 0;
			for (int i = 0; i < 8; i++)
			{
				array[num++] = 0;
			}
			long num3 = (long)entryCount;
			Array.Copy(BitConverter.GetBytes(num3), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(num3), 0, array, num, 8);
			num += 8;
			long num4 = EndOfCentralDirectory - StartOfCentralDirectory;
			Array.Copy(BitConverter.GetBytes(num4), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(StartOfCentralDirectory), 0, array, num, 8);
			num += 8;
			array2 = BitConverter.GetBytes(117853008U);
			Array.Copy(array2, 0, array, num, 4);
			num += 4;
			uint num5 = ((numSegments == 0U) ? 0U : (numSegments - 1U));
			Array.Copy(BitConverter.GetBytes(num5), 0, array, num, 4);
			num += 4;
			Array.Copy(BitConverter.GetBytes(EndOfCentralDirectory), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(numSegments), 0, array, num, 4);
			num += 4;
			return array;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00011A5C File Offset: 0x0000FC5C
		private static int CountEntries(ICollection<ZipEntry> _entries)
		{
			int num = 0;
			foreach (ZipEntry zipEntry in _entries)
			{
				if (zipEntry.IncludedInMostRecentSave)
				{
					num++;
				}
			}
			return num;
		}
	}
}
