using System;

namespace Mono.Cecil
{
	// Token: 0x0200007D RID: 125
	public sealed class FixedArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0001320A File Offset: 0x0001140A
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00013212 File Offset: 0x00011412
		public NativeType ElementType
		{
			get
			{
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001321B File Offset: 0x0001141B
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00013223 File Offset: 0x00011423
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

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001322C File Offset: 0x0001142C
		public FixedArrayMarshalInfo()
			: base(NativeType.FixedArray)
		{
			this.element_type = NativeType.None;
		}

		// Token: 0x040000F6 RID: 246
		internal NativeType element_type;

		// Token: 0x040000F7 RID: 247
		internal int size;
	}
}
