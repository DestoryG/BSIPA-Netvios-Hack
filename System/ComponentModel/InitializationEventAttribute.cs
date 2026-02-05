using System;

namespace System.ComponentModel
{
	// Token: 0x02000568 RID: 1384
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class InitializationEventAttribute : Attribute
	{
		// Token: 0x060033A8 RID: 13224 RVA: 0x000E3C84 File Offset: 0x000E1E84
		public InitializationEventAttribute(string eventName)
		{
			this.eventName = eventName;
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x060033A9 RID: 13225 RVA: 0x000E3C93 File Offset: 0x000E1E93
		public string EventName
		{
			get
			{
				return this.eventName;
			}
		}

		// Token: 0x040029B6 RID: 10678
		private string eventName;
	}
}
