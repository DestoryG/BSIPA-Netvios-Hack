using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200000A RID: 10
	public class JSONNumber : JSONNode
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002E9B File Offset: 0x0000109B
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Number;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override bool IsNumber
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002EA0 File Offset: 0x000010A0
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002EB6 File Offset: 0x000010B6
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002EC4 File Offset: 0x000010C4
		public override string Value
		{
			get
			{
				return this.m_Data.ToString();
			}
			set
			{
				double num;
				if (double.TryParse(value, out num))
				{
					this.m_Data = num;
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002EE2 File Offset: 0x000010E2
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002EEA File Offset: 0x000010EA
		public override double AsDouble
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002EF3 File Offset: 0x000010F3
		public JSONNumber(double aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002F02 File Offset: 0x00001102
		public JSONNumber(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002F11 File Offset: 0x00001111
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002F20 File Offset: 0x00001120
		private static bool IsNumeric(object value)
		{
			return value is int || value is uint || value is float || value is double || value is decimal || value is long || value is ulong || value is short || value is ushort || value is sbyte || value is byte;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002F88 File Offset: 0x00001188
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (base.Equals(obj))
			{
				return true;
			}
			JSONNumber jsonnumber = obj as JSONNumber;
			if (jsonnumber != null)
			{
				return this.m_Data == jsonnumber.m_Data;
			}
			return JSONNumber.IsNumeric(obj) && Convert.ToDouble(obj) == this.m_Data;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002FDC File Offset: 0x000011DC
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000014 RID: 20
		private double m_Data;
	}
}
