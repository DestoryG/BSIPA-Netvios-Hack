using System;

namespace Ionic.Zip
{
	// Token: 0x0200001D RID: 29
	internal static class ZipConstants
	{
		// Token: 0x040000A6 RID: 166
		public const uint PackedToRemovableMedia = 808471376U;

		// Token: 0x040000A7 RID: 167
		public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792U;

		// Token: 0x040000A8 RID: 168
		public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008U;

		// Token: 0x040000A9 RID: 169
		public const uint EndOfCentralDirectorySignature = 101010256U;

		// Token: 0x040000AA RID: 170
		public const int ZipEntrySignature = 67324752;

		// Token: 0x040000AB RID: 171
		public const int ZipEntryDataDescriptorSignature = 134695760;

		// Token: 0x040000AC RID: 172
		public const int SplitArchiveSignature = 134695760;

		// Token: 0x040000AD RID: 173
		public const int ZipDirEntrySignature = 33639248;

		// Token: 0x040000AE RID: 174
		public const int AesKeySize = 192;

		// Token: 0x040000AF RID: 175
		public const int AesBlockSize = 128;

		// Token: 0x040000B0 RID: 176
		public const ushort AesAlgId128 = 26126;

		// Token: 0x040000B1 RID: 177
		public const ushort AesAlgId192 = 26127;

		// Token: 0x040000B2 RID: 178
		public const ushort AesAlgId256 = 26128;
	}
}
