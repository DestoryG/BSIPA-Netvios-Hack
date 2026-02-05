using System;
using System.Reflection;

namespace BeatSaberMarkupLanguage.ViewControllers
{
	// Token: 0x0200000E RID: 14
	public abstract class BSMLResourceViewController : BSMLViewController
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000074 RID: 116
		public abstract string ResourceName { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000045B8 File Offset: 0x000027B8
		public override string Content
		{
			get
			{
				return Utilities.GetResourceContent(Assembly.GetAssembly(base.GetType()), this.ResourceName);
			}
		}
	}
}
