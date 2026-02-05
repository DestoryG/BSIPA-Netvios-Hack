using System;

namespace Mono.Cecil
{
	// Token: 0x02000130 RID: 304
	internal class MarshalInfo
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0002193B File Offset: 0x0001FB3B
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x00021943 File Offset: 0x0001FB43
		public NativeType NativeType
		{
			get
			{
				return this.native;
			}
			set
			{
				this.native = value;
			}
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002194C File Offset: 0x0001FB4C
		public MarshalInfo(NativeType native)
		{
			this.native = native;
		}

		// Token: 0x04000306 RID: 774
		internal NativeType native;
	}
}
