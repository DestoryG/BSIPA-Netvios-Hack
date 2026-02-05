using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D6 RID: 214
	internal class IDATChunk : Chunk
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x00014631 File Offset: 0x00012831
		public IDATChunk(byte[] bytes)
			: base(bytes)
		{
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001463A File Offset: 0x0001283A
		public IDATChunk(MemoryStream ms)
			: base(ms)
		{
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00014643 File Offset: 0x00012843
		public IDATChunk(Chunk chunk)
			: base(chunk)
		{
		}
	}
}
