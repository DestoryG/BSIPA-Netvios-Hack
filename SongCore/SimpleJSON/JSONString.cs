using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000009 RID: 9
	public class JSONString : JSONNode
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002DDF File Offset: 0x00000FDF
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.String;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override bool IsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002DE4 File Offset: 0x00000FE4
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002DFA File Offset: 0x00000FFA
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002E02 File Offset: 0x00001002
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

		// Token: 0x06000068 RID: 104 RVA: 0x00002E0B File Offset: 0x0000100B
		public JSONString(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002E1A File Offset: 0x0000101A
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002E3C File Offset: 0x0000103C
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

		// Token: 0x0600006B RID: 107 RVA: 0x00002E8E File Offset: 0x0000108E
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000013 RID: 19
		private string m_Data;
	}
}
