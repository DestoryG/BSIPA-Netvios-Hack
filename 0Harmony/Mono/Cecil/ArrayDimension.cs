using System;

namespace Mono.Cecil
{
	// Token: 0x020000B9 RID: 185
	internal struct ArrayDimension
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00012F47 File Offset: 0x00011147
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00012F4F File Offset: 0x0001114F
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00012F58 File Offset: 0x00011158
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x00012F60 File Offset: 0x00011160
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00012F69 File Offset: 0x00011169
		public bool IsSized
		{
			get
			{
				return this.lower_bound != null || this.upper_bound != null;
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00012F85 File Offset: 0x00011185
		public ArrayDimension(int? lowerBound, int? upperBound)
		{
			this.lower_bound = lowerBound;
			this.upper_bound = upperBound;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00012F98 File Offset: 0x00011198
		public override string ToString()
		{
			if (this.IsSized)
			{
				int? num = this.lower_bound;
				string text = num.ToString();
				string text2 = "...";
				num = this.upper_bound;
				return text + text2 + num.ToString();
			}
			return string.Empty;
		}

		// Token: 0x04000219 RID: 537
		private int? lower_bound;

		// Token: 0x0400021A RID: 538
		private int? upper_bound;
	}
}
