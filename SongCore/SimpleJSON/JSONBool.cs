using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200000B RID: 11
	public class JSONBool : JSONNode
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002FE9 File Offset: 0x000011E9
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Boolean;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override bool IsBoolean
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002FEC File Offset: 0x000011EC
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003002 File Offset: 0x00001202
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003010 File Offset: 0x00001210
		public override string Value
		{
			get
			{
				return this.m_Data.ToString();
			}
			set
			{
				bool flag;
				if (bool.TryParse(value, out flag))
				{
					this.m_Data = flag;
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007E RID: 126 RVA: 0x0000302E File Offset: 0x0000122E
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00003036 File Offset: 0x00001236
		public override bool AsBool
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

		// Token: 0x06000080 RID: 128 RVA: 0x0000303F File Offset: 0x0000123F
		public JSONBool(bool aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002F02 File Offset: 0x00001102
		public JSONBool(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000304E File Offset: 0x0000124E
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data ? "true" : "false");
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000306B File Offset: 0x0000126B
		public override bool Equals(object obj)
		{
			return obj != null && obj is bool && this.m_Data == (bool)obj;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000308A File Offset: 0x0000128A
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000015 RID: 21
		private bool m_Data;
	}
}
