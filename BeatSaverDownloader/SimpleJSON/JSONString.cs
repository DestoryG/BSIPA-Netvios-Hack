using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000007 RID: 7
	public class JSONString : JSONNode
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002D67 File Offset: 0x00000F67
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.String;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002830 File Offset: 0x00000A30
		public override bool IsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002D6C File Offset: 0x00000F6C
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002D82 File Offset: 0x00000F82
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002D8A File Offset: 0x00000F8A
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

		// Token: 0x06000064 RID: 100 RVA: 0x00002D93 File Offset: 0x00000F93
		public JSONString(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002DA2 File Offset: 0x00000FA2
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002DC4 File Offset: 0x00000FC4
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

		// Token: 0x06000067 RID: 103 RVA: 0x00002E16 File Offset: 0x00001016
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000013 RID: 19
		private string m_Data;
	}
}
