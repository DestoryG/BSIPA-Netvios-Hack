using System;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x0200007F RID: 127
	public class JSONString : JSONNode
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x000255F7 File Offset: 0x000237F7
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.String;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00024FA4 File Offset: 0x000231A4
		public override bool IsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x000255FC File Offset: 0x000237FC
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x00025612 File Offset: 0x00023812
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x0002561A File Offset: 0x0002381A
		public override string Value
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

		// Token: 0x060008ED RID: 2285 RVA: 0x00025623 File Offset: 0x00023823
		public JSONString(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00025632 File Offset: 0x00023832
		public override JSONNode Clone()
		{
			return new JSONString(this.m_Data);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002563F File Offset: 0x0002383F
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00025664 File Offset: 0x00023864
		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				return true;
			}
			string text = obj as string;
			if (text != null)
			{
				return this.m_Data == text;
			}
			JSONString jsonstring = obj as JSONString;
			return jsonstring != null && this.m_Data == jsonstring.m_Data;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000256B6 File Offset: 0x000238B6
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000464 RID: 1124
		private string m_Data;
	}
}
