using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000081 RID: 129
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class EnumMemberAttribute : Attribute
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0002A300 File Offset: 0x00028500
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0002A308 File Offset: 0x00028508
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				this.isValueSetExplicitly = true;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0002A318 File Offset: 0x00028518
		public bool IsValueSetExplicitly
		{
			get
			{
				return this.isValueSetExplicitly;
			}
		}

		// Token: 0x04000396 RID: 918
		private string value;

		// Token: 0x04000397 RID: 919
		private bool isValueSetExplicitly;
	}
}
