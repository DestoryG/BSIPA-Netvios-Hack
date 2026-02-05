using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000014 RID: 20
	internal class TypeCriterion : SelectionCriterion
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003AEE File Offset: 0x00001CEE
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003AFB File Offset: 0x00001CFB
		internal string AttributeString
		{
			get
			{
				return this.ObjectType.ToString();
			}
			set
			{
				if (value.Length != 1 || (value[0] != 'D' && value[0] != 'F'))
				{
					throw new ArgumentException("Specify a single character: either D or F");
				}
				this.ObjectType = value[0];
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003B34 File Offset: 0x00001D34
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("type ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ")
				.Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003B84 File Offset: 0x00001D84
		internal override bool Evaluate(string filename)
		{
			bool flag = ((this.ObjectType == 'D') ? Directory.Exists(filename) : File.Exists(filename));
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003BBC File Offset: 0x00001DBC
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = ((this.ObjectType == 'D') ? entry.IsDirectory : (!entry.IsDirectory));
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x04000075 RID: 117
		private char ObjectType;

		// Token: 0x04000076 RID: 118
		internal ComparisonOperator Operator;
	}
}
