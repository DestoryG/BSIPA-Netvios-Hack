using System;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x02000019 RID: 25
	public class JSONNumber : JSONNode
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00008D63 File Offset: 0x00006F63
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Number;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00008770 File Offset: 0x00006970
		public override bool IsNumber
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008D68 File Offset: 0x00006F68
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00008D7E File Offset: 0x00006F7E
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00008D8C File Offset: 0x00006F8C
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00008DAA File Offset: 0x00006FAA
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00008DB2 File Offset: 0x00006FB2
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

		// Token: 0x06000103 RID: 259 RVA: 0x00008DBB File Offset: 0x00006FBB
		public JSONNumber(double aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008DCA File Offset: 0x00006FCA
		public JSONNumber(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008DD9 File Offset: 0x00006FD9
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008DE8 File Offset: 0x00006FE8
		private static bool IsNumeric(object value)
		{
			return value is int || value is uint || value is float || value is double || value is decimal || value is long || value is ulong || value is short || value is ushort || value is sbyte || value is byte;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00008E50 File Offset: 0x00007050
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

		// Token: 0x06000108 RID: 264 RVA: 0x00008EA4 File Offset: 0x000070A4
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x040000B0 RID: 176
		private double m_Data;
	}
}
