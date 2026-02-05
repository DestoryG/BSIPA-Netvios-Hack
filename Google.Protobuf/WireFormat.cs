using System;

namespace Google.Protobuf
{
	// Token: 0x02000026 RID: 38
	public static class WireFormat
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000A52E File Offset: 0x0000872E
		public static WireFormat.WireType GetTagWireType(uint tag)
		{
			return (WireFormat.WireType)(tag & 7U);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000A533 File Offset: 0x00008733
		public static int GetTagFieldNumber(uint tag)
		{
			return (int)tag >> 3;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A538 File Offset: 0x00008738
		public static uint MakeTag(int fieldNumber, WireFormat.WireType wireType)
		{
			return (uint)((fieldNumber << 3) | (int)wireType);
		}

		// Token: 0x0400006C RID: 108
		private const int TagTypeBits = 3;

		// Token: 0x0400006D RID: 109
		private const uint TagTypeMask = 7U;

		// Token: 0x020000AA RID: 170
		public enum WireType : uint
		{
			// Token: 0x040003BC RID: 956
			Varint,
			// Token: 0x040003BD RID: 957
			Fixed64,
			// Token: 0x040003BE RID: 958
			LengthDelimited,
			// Token: 0x040003BF RID: 959
			StartGroup,
			// Token: 0x040003C0 RID: 960
			EndGroup,
			// Token: 0x040003C1 RID: 961
			Fixed32
		}
	}
}
