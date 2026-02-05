using System;

namespace HarmonyLib
{
	// Token: 0x0200005A RID: 90
	public class ExceptionBlock
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00009AE8 File Offset: 0x00007CE8
		public ExceptionBlock(ExceptionBlockType blockType, Type catchType = null)
		{
			this.blockType = blockType;
			this.catchType = catchType ?? typeof(object);
		}

		// Token: 0x040000FB RID: 251
		public ExceptionBlockType blockType;

		// Token: 0x040000FC RID: 252
		public Type catchType;
	}
}
