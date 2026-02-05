using System;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x02000018 RID: 24
	public class JSONString : JSONNode
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00008CA7 File Offset: 0x00006EA7
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.String;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00008770 File Offset: 0x00006970
		public override bool IsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00008CAC File Offset: 0x00006EAC
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00008CC2 File Offset: 0x00006EC2
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00008CCA File Offset: 0x00006ECA
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

		// Token: 0x060000F8 RID: 248 RVA: 0x00008CD3 File Offset: 0x00006ED3
		public JSONString(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00008CE2 File Offset: 0x00006EE2
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00008D04 File Offset: 0x00006F04
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

		// Token: 0x060000FB RID: 251 RVA: 0x00008D56 File Offset: 0x00006F56
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x040000AF RID: 175
		private string m_Data;
	}
}
