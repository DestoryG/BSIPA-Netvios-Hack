using System;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000016 RID: 22
	internal class CompoundCriterion : SelectionCriterion
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00003FB7 File Offset: 0x000021B7
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00003FBF File Offset: 0x000021BF
		internal SelectionCriterion Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
				if (value == null)
				{
					this.Conjunction = LogicalConjunction.NONE;
					return;
				}
				if (this.Conjunction == LogicalConjunction.NONE)
				{
					this.Conjunction = LogicalConjunction.AND;
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003FE4 File Offset: 0x000021E4
		internal override bool Evaluate(string filename)
		{
			bool flag = this.Left.Evaluate(filename);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(filename);
				break;
			default:
				throw new ArgumentException("Conjunction");
			}
			return flag;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000405C File Offset: 0x0000225C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(").Append((this.Left != null) ? this.Left.ToString() : "null").Append(" ")
				.Append(this.Conjunction.ToString())
				.Append(" ")
				.Append((this.Right != null) ? this.Right.ToString() : "null")
				.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000040F4 File Offset: 0x000022F4
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = this.Left.Evaluate(entry);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(entry);
				break;
			}
			return flag;
		}

		// Token: 0x04000079 RID: 121
		internal LogicalConjunction Conjunction;

		// Token: 0x0400007A RID: 122
		internal SelectionCriterion Left;

		// Token: 0x0400007B RID: 123
		private SelectionCriterion _Right;
	}
}
