using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000009 RID: 9
	public static class ParameterReferenceRocks
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00003891 File Offset: 0x00001A91
		public static int GetSequence(this ParameterReference self)
		{
			return self.Index + 1;
		}
	}
}
