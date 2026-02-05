using System;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000C8 RID: 200
	internal class Helper
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x00013124 File Offset: 0x00011324
		internal static byte[] ConvertEndian(byte[] i)
		{
			if (i.Length % 2 != 0)
			{
				throw new Exception("byte array length must multiply of 2");
			}
			Array.Reverse(i);
			return i;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001313F File Offset: 0x0001133F
		internal static int ConvertEndian(int i)
		{
			return BitConverter.ToInt32(Helper.ConvertEndian(BitConverter.GetBytes(i)), 0);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00013152 File Offset: 0x00011352
		internal static uint ConvertEndian(uint i)
		{
			return BitConverter.ToUInt32(Helper.ConvertEndian(BitConverter.GetBytes(i)), 0);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00013165 File Offset: 0x00011365
		internal static short ConvertEndian(short i)
		{
			return BitConverter.ToInt16(Helper.ConvertEndian(BitConverter.GetBytes(i)), 0);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00013178 File Offset: 0x00011378
		internal static ushort ConvertEndian(ushort i)
		{
			return BitConverter.ToUInt16(Helper.ConvertEndian(BitConverter.GetBytes(i)), 0);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001318C File Offset: 0x0001138C
		public static bool IsBytesEqual(byte[] byte1, byte[] byte2)
		{
			if (byte1.Length != byte2.Length)
			{
				return false;
			}
			for (int i = 0; i < byte1.Length; i++)
			{
				if (byte1[i] != byte2[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
