using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02000066 RID: 102
	internal class SharedUtils
	{
		// Token: 0x06000480 RID: 1152 RVA: 0x0002018A File Offset: 0x0001E38A
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00020194 File Offset: 0x0001E394
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000201D6 File Offset: 0x0001E3D6
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000201E3 File Offset: 0x0001E3E3
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
