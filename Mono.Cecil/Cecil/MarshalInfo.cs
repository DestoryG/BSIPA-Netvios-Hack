using System;

namespace Mono.Cecil
{
	// Token: 0x02000079 RID: 121
	public class MarshalInfo
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0001310F File Offset: 0x0001130F
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00013117 File Offset: 0x00011317
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

		// Token: 0x060004CA RID: 1226 RVA: 0x00013120 File Offset: 0x00011320
		public MarshalInfo(NativeType native)
		{
			this.native = native;
		}

		// Token: 0x040000EC RID: 236
		internal NativeType native;
	}
}
