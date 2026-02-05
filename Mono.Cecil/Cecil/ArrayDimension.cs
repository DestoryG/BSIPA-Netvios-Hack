using System;

namespace Mono.Cecil
{
	// Token: 0x0200000D RID: 13
	public struct ArrayDimension
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004BB0 File Offset: 0x00002DB0
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public int? LowerBound
		{
			get
			{
				return this.lower_bound;
			}
			set
			{
				this.lower_bound = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004BC1 File Offset: 0x00002DC1
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00004BC9 File Offset: 0x00002DC9
		public int? UpperBound
		{
			get
			{
				return this.upper_bound;
			}
			set
			{
				this.upper_bound = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004BD2 File Offset: 0x00002DD2
		public bool IsSized
		{
			get
			{
				return this.lower_bound != null || this.upper_bound != null;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004BEE File Offset: 0x00002DEE
		public ArrayDimension(int? lowerBound, int? upperBound)
		{
			this.lower_bound = lowerBound;
			this.upper_bound = upperBound;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004BFE File Offset: 0x00002DFE
		public override string ToString()
		{
			if (this.IsSized)
			{
				return this.lower_bound + "..." + this.upper_bound;
			}
			return string.Empty;
		}

		// Token: 0x04000018 RID: 24
		private int? lower_bound;

		// Token: 0x04000019 RID: 25
		private int? upper_bound;
	}
}
