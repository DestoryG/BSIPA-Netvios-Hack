using System;
using System.IO;

namespace BeatSaberMarkupLanguage.Animations.APNG.Chunks
{
	// Token: 0x020000D7 RID: 215
	internal class IENDChunk : Chunk
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x00014631 File Offset: 0x00012831
		public IENDChunk(byte[] bytes)
			: base(bytes)
		{
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001463A File Offset: 0x0001283A
		public IENDChunk(MemoryStream ms)
			: base(ms)
		{
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00014643 File Offset: 0x00012843
		public IENDChunk(Chunk chunk)
			: base(chunk)
		{
		}
	}
}
