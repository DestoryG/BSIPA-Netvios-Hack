using System;

namespace Mono.Cecil
{
	// Token: 0x02000135 RID: 309
	internal sealed class FixedSysStringMarshalInfo : MarshalInfo
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x00021A6A File Offset: 0x0001FC6A
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x00021A72 File Offset: 0x0001FC72
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00021A7B File Offset: 0x0001FC7B
		public FixedSysStringMarshalInfo()
			: base(NativeType.FixedSysString)
		{
			this.size = -1;
		}

		// Token: 0x04000312 RID: 786
		internal int size;
	}
}
