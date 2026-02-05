using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D9 RID: 217
	internal class OtherChunk : Chunk
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x00014631 File Offset: 0x00012831
		public OtherChunk(byte[] bytes)
			: base(bytes)
		{
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001463A File Offset: 0x0001283A
		public OtherChunk(MemoryStream ms)
			: base(ms)
		{
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00014643 File Offset: 0x00012843
		public OtherChunk(Chunk chunk)
			: base(chunk)
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000263A File Offset: 0x0000083A
		protected override void ParseData(MemoryStream ms)
		{
		}
	}
}
