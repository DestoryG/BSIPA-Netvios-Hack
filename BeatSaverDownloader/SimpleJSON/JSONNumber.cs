using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000008 RID: 8
	public class JSONNumber : JSONNode
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002E23 File Offset: 0x00001023
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Number;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002830 File Offset: 0x00000A30
		public override bool IsNumber
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002E28 File Offset: 0x00001028
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002E3E File Offset: 0x0000103E
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002E4C File Offset: 0x0000104C
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
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002E6A File Offset: 0x0000106A
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002E72 File Offset: 0x00001072
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

		// Token: 0x0600006F RID: 111 RVA: 0x00002E7B File Offset: 0x0000107B
		public JSONNumber(double aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002E8A File Offset: 0x0000108A
		public JSONNumber(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002E99 File Offset: 0x00001099
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002EA8 File Offset: 0x000010A8
		private static bool IsNumeric(object value)
		{
			return value is int || value is uint || value is float || value is double || value is decimal || value is long || value is ulong || value is short || value is ushort || value is sbyte || value is byte;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002F10 File Offset: 0x00001110
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

		// Token: 0x06000074 RID: 116 RVA: 0x00002F64 File Offset: 0x00001164
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000014 RID: 20
		private double m_Data;
	}
}
