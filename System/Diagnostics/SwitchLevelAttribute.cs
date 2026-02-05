using System;

namespace System.Diagnostics
{
	// Token: 0x020004AA RID: 1194
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SwitchLevelAttribute : Attribute
	{
		// Token: 0x06002C33 RID: 11315 RVA: 0x000C7539 File Offset: 0x000C5739
		public SwitchLevelAttribute(Type switchLevelType)
		{
			this.SwitchLevelType = switchLevelType;
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06002C34 RID: 11316 RVA: 0x000C7548 File Offset: 0x000C5748
		// (set) Token: 0x06002C35 RID: 11317 RVA: 0x000C7550 File Offset: 0x000C5750
		public Type SwitchLevelType
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.type = value;
			}
		}

		// Token: 0x040026B7 RID: 9911
		private Type type;
	}
}
