using System;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000036 RID: 54
	public class ComponentHandler : Attribute
	{
		// Token: 0x06000127 RID: 295 RVA: 0x000081A8 File Offset: 0x000063A8
		public ComponentHandler(Type type)
		{
			this.type = type;
		}

		// Token: 0x04000035 RID: 53
		public Type type;
	}
}
