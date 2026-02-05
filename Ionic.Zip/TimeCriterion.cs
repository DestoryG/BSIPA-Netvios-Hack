using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000012 RID: 18
	internal class TimeCriterion : SelectionCriterion
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00003794 File Offset: 0x00001994
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Which.ToString()).Append(" ").Append(EnumUtil.GetDescription(this.Operator))
				.Append(" ")
				.Append(this.Time.ToString("yyyy-MM-dd-HH:mm:ss"));
			return stringBuilder.ToString();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003804 File Offset: 0x00001A04
		internal override bool Evaluate(string filename)
		{
			DateTime dateTime;
			switch (this.Which)
			{
			case WhichTime.atime:
				dateTime = File.GetLastAccessTime(filename).ToUniversalTime();
				break;
			case WhichTime.mtime:
				dateTime = File.GetLastWriteTime(filename).ToUniversalTime();
				break;
			case WhichTime.ctime:
				dateTime = File.GetCreationTime(filename).ToUniversalTime();
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return this._Evaluate(dateTime);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003874 File Offset: 0x00001A74
		private bool _Evaluate(DateTime x)
		{
			bool flag;
			switch (this.Operator)
			{
			case ComparisonOperator.GreaterThan:
				flag = x > this.Time;
				break;
			case ComparisonOperator.GreaterThanOrEqualTo:
				flag = x >= this.Time;
				break;
			case ComparisonOperator.LesserThan:
				flag = x < this.Time;
				break;
			case ComparisonOperator.LesserThanOrEqualTo:
				flag = x <= this.Time;
				break;
			case ComparisonOperator.EqualTo:
				flag = x == this.Time;
				break;
			case ComparisonOperator.NotEqualTo:
				flag = x != this.Time;
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return flag;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003910 File Offset: 0x00001B10
		internal override bool Evaluate(ZipEntry entry)
		{
			DateTime dateTime;
			switch (this.Which)
			{
			case WhichTime.atime:
				dateTime = entry.AccessedTime;
				break;
			case WhichTime.mtime:
				dateTime = entry.ModifiedTime;
				break;
			case WhichTime.ctime:
				dateTime = entry.CreationTime;
				break;
			default:
				throw new ArgumentException("??time");
			}
			return this._Evaluate(dateTime);
		}

		// Token: 0x0400006E RID: 110
		internal ComparisonOperator Operator;

		// Token: 0x0400006F RID: 111
		internal WhichTime Which;

		// Token: 0x04000070 RID: 112
		internal DateTime Time;
	}
}
