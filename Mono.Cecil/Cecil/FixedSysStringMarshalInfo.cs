using System;

namespace Mono.Cecil
{
	// Token: 0x0200007E RID: 126
	public sealed class FixedSysStringMarshalInfo : MarshalInfo
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001323E File Offset: 0x0001143E
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00013246 File Offset: 0x00011446
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

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001324F File Offset: 0x0001144F
		public FixedSysStringMarshalInfo()
			: base(NativeType.FixedSysString)
		{
			this.size = -1;
		}

		// Token: 0x040000F8 RID: 248
		internal int size;
	}
}
