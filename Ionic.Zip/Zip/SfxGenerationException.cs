using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000022 RID: 34
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00008")]
	[Serializable]
	public class SfxGenerationException : ZipException
	{
		// Token: 0x06000107 RID: 263 RVA: 0x00005B02 File Offset: 0x00003D02
		public SfxGenerationException()
		{
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005B0A File Offset: 0x00003D0A
		public SfxGenerationException(string message)
			: base(message)
		{
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005B13 File Offset: 0x00003D13
		protected SfxGenerationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
