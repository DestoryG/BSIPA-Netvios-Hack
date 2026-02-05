using System;
using System.Globalization;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000080 RID: 128
	public class JSONNumber : JSONNode
	{
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x000256C3 File Offset: 0x000238C3
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Number;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00024FA4 File Offset: 0x000231A4
		public override bool IsNumber
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000256C8 File Offset: 0x000238C8
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x000256DE File Offset: 0x000238DE
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x000256F0 File Offset: 0x000238F0
		public override string Value
		{
			get
			{
				return this.m_Data.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				double num;
				if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
				{
					this.m_Data = num;
				}
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00025718 File Offset: 0x00023918
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00025720 File Offset: 0x00023920
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

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00025729 File Offset: 0x00023929
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x00025732 File Offset: 0x00023932
		public override long AsLong
		{
			get
			{
				return (long)this.m_Data;
			}
			set
			{
				this.m_Data = (double)value;
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002573C File Offset: 0x0002393C
		public JSONNumber(double aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002574B File Offset: 0x0002394B
		public JSONNumber(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002575A File Offset: 0x0002395A
		public override JSONNode Clone()
		{
			return new JSONNumber(this.m_Data);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00025767 File Offset: 0x00023967
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.Value);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00025778 File Offset: 0x00023978
		private static bool IsNumeric(object value)
		{
			return value is int || value is uint || value is float || value is double || value is decimal || value is long || value is ulong || value is short || value is ushort || value is sbyte || value is byte;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x000257E0 File Offset: 0x000239E0
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

		// Token: 0x06000901 RID: 2305 RVA: 0x00025834 File Offset: 0x00023A34
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000465 RID: 1125
		private double m_Data;
	}
}
