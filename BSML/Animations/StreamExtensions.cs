using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations
{
	// Token: 0x020000CA RID: 202
	internal static class StreamExtensions
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x000131BC File Offset: 0x000113BC
		public static byte[] PeekBytes(this Stream ms, int position, int count)
		{
			long position2 = ms.Position;
			ms.Position = (long)position;
			byte[] array = ms.ReadBytes(count);
			ms.Position = position2;
			return array;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000131E6 File Offset: 0x000113E6
		public static char PeekChar(this Stream ms)
		{
			return ms.PeekChar((int)ms.Position);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000131F5 File Offset: 0x000113F5
		public static char PeekChar(this Stream ms, int position)
		{
			return BitConverter.ToChar(ms.PeekBytes(position, 2), 0);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00013205 File Offset: 0x00011405
		public static short PeekInt16(this Stream ms)
		{
			return ms.PeekInt16((int)ms.Position);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00013214 File Offset: 0x00011414
		public static short PeekInt16(this Stream ms, int position)
		{
			return BitConverter.ToInt16(ms.PeekBytes(position, 2), 0);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00013224 File Offset: 0x00011424
		public static int PeekInt32(this Stream ms)
		{
			return ms.PeekInt32((int)ms.Position);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00013233 File Offset: 0x00011433
		public static int PeekInt32(this Stream ms, int position)
		{
			return BitConverter.ToInt32(ms.PeekBytes(position, 4), 0);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00013243 File Offset: 0x00011443
		public static long PeekInt64(this Stream ms)
		{
			return ms.PeekInt64((int)ms.Position);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00013252 File Offset: 0x00011452
		public static long PeekInt64(this Stream ms, int position)
		{
			return BitConverter.ToInt64(ms.PeekBytes(position, 8), 0);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00013262 File Offset: 0x00011462
		public static ushort PeekUInt16(this Stream ms)
		{
			return ms.PeekUInt16((int)ms.Position);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00013271 File Offset: 0x00011471
		public static ushort PeekUInt16(this Stream ms, int position)
		{
			return BitConverter.ToUInt16(ms.PeekBytes(position, 2), 0);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00013281 File Offset: 0x00011481
		public static uint PeekUInt32(this Stream ms)
		{
			return ms.PeekUInt32((int)ms.Position);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00013290 File Offset: 0x00011490
		public static uint PeekUInt32(this Stream ms, int position)
		{
			return BitConverter.ToUInt32(ms.PeekBytes(position, 4), 0);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000132A0 File Offset: 0x000114A0
		public static ulong PeekUInt64(this Stream ms)
		{
			return ms.PeekUInt64((int)ms.Position);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000132AF File Offset: 0x000114AF
		public static ulong PeekUInt64(this Stream ms, int position)
		{
			return BitConverter.ToUInt64(ms.PeekBytes(position, 8), 0);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000132C0 File Offset: 0x000114C0
		public static byte[] ReadBytes(this Stream ms, int count)
		{
			byte[] array = new byte[count];
			if (ms.Read(array, 0, count) != count)
			{
				throw new Exception("End reached.");
			}
			return array;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000132EC File Offset: 0x000114EC
		public static char ReadChar(this Stream ms)
		{
			return BitConverter.ToChar(ms.ReadBytes(2), 0);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000132FB File Offset: 0x000114FB
		public static short ReadInt16(this Stream ms)
		{
			return BitConverter.ToInt16(ms.ReadBytes(2), 0);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001330A File Offset: 0x0001150A
		public static int ReadInt32(this Stream ms)
		{
			return BitConverter.ToInt32(ms.ReadBytes(4), 0);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00013319 File Offset: 0x00011519
		public static long ReadInt64(this Stream ms)
		{
			return BitConverter.ToInt64(ms.ReadBytes(8), 0);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00013328 File Offset: 0x00011528
		public static ushort ReadUInt16(this Stream ms)
		{
			return BitConverter.ToUInt16(ms.ReadBytes(2), 0);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00013337 File Offset: 0x00011537
		public static uint ReadUInt32(this Stream ms)
		{
			return BitConverter.ToUInt32(ms.ReadBytes(4), 0);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00013346 File Offset: 0x00011546
		public static ulong ReadUInt64(this Stream ms)
		{
			return BitConverter.ToUInt64(ms.ReadBytes(8), 0);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00013358 File Offset: 0x00011558
		public static void WriteByte(this Stream ms, int position, byte value)
		{
			long position2 = ms.Position;
			ms.Position = (long)position;
			ms.WriteByte(value);
			ms.Position = position2;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00013382 File Offset: 0x00011582
		public static void WriteBytes(this Stream ms, byte[] value)
		{
			ms.Write(value, 0, value.Length);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00013390 File Offset: 0x00011590
		public static void WriteBytes(this Stream ms, int position, byte[] value)
		{
			long position2 = ms.Position;
			ms.Position = (long)position;
			ms.Write(value, 0, value.Length);
			ms.Position = position2;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000133BE File Offset: 0x000115BE
		public static void WriteInt16(this Stream ms, short value)
		{
			ms.Write(BitConverter.GetBytes(value), 0, 2);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000133CE File Offset: 0x000115CE
		public static void WriteInt16(this Stream ms, int position, short value)
		{
			ms.WriteBytes(position, BitConverter.GetBytes(value));
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000133DD File Offset: 0x000115DD
		public static void WriteInt32(this Stream ms, int value)
		{
			ms.Write(BitConverter.GetBytes(value), 0, 4);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000133ED File Offset: 0x000115ED
		public static void WriteInt32(this Stream ms, int position, int value)
		{
			ms.WriteBytes(position, BitConverter.GetBytes(value));
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000133FC File Offset: 0x000115FC
		public static void WriteInt64(this Stream ms, long value)
		{
			ms.Write(BitConverter.GetBytes(value), 0, 8);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001340C File Offset: 0x0001160C
		public static void WriteInt64(this Stream ms, int position, long value)
		{
			ms.WriteBytes(position, BitConverter.GetBytes(value));
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001341B File Offset: 0x0001161B
		public static void WriteUInt16(this Stream ms, ushort value)
		{
			ms.Write(BitConverter.GetBytes(value), 0, 2);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001342B File Offset: 0x0001162B
		public static void WriteUInt16(this Stream ms, int position, ushort value)
		{
			ms.WriteBytes(position, BitConverter.GetBytes(value));
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001343A File Offset: 0x0001163A
		public static void WriteUInt32(this Stream ms, uint value)
		{
			ms.Write(BitConverter.GetBytes(value), 0, 4);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001344A File Offset: 0x0001164A
		public static void WriteUInt32(this Stream ms, int position, uint value)
		{
			ms.WriteBytes(position, BitConverter.GetBytes(value));
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00013459 File Offset: 0x00011659
		public static void WriteUInt64(this Stream ms, ulong value)
		{
			ms.Write(BitConverter.GetBytes(value), 0, 8);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00013469 File Offset: 0x00011669
		public static void WriteUInt64(this Stream ms, int position, ulong value)
		{
			ms.WriteBytes(position, BitConverter.GetBytes(value));
		}
	}
}
