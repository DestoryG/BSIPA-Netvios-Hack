using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000011 RID: 17
	internal class SizeCriterion : SelectionCriterion
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00003674 File Offset: 0x00001874
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("size ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ")
				.Append(this.Size.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000036C8 File Offset: 0x000018C8
		internal override bool Evaluate(string filename)
		{
			FileInfo fileInfo = new FileInfo(filename);
			return this._Evaluate(fileInfo.Length);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000036E8 File Offset: 0x000018E8
		private bool _Evaluate(long Length)
		{
			bool flag;
			switch (this.Operator)
			{
			case ComparisonOperator.GreaterThan:
				flag = Length > this.Size;
				break;
			case ComparisonOperator.GreaterThanOrEqualTo:
				flag = Length >= this.Size;
				break;
			case ComparisonOperator.LesserThan:
				flag = Length < this.Size;
				break;
			case ComparisonOperator.LesserThanOrEqualTo:
				flag = Length <= this.Size;
				break;
			case ComparisonOperator.EqualTo:
				flag = Length == this.Size;
				break;
			case ComparisonOperator.NotEqualTo:
				flag = Length != this.Size;
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return flag;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000377B File Offset: 0x0000197B
		internal override bool Evaluate(ZipEntry entry)
		{
			return this._Evaluate(entry.UncompressedSize);
		}

		// Token: 0x0400006C RID: 108
		internal ComparisonOperator Operator;

		// Token: 0x0400006D RID: 109
		internal long Size;
	}
}
