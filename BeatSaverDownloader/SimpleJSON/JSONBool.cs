using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000009 RID: 9
	public class JSONBool : JSONNode
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002F71 File Offset: 0x00001171
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Boolean;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002830 File Offset: 0x00000A30
		public override bool IsBoolean
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002F74 File Offset: 0x00001174
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002F8A File Offset: 0x0000118A
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002F98 File Offset: 0x00001198
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
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002FB6 File Offset: 0x000011B6
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002FBE File Offset: 0x000011BE
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

		// Token: 0x0600007C RID: 124 RVA: 0x00002FC7 File Offset: 0x000011C7
		public JSONBool(bool aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002E8A File Offset: 0x0000108A
		public JSONBool(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002FD6 File Offset: 0x000011D6
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data ? "true" : "false");
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002FF3 File Offset: 0x000011F3
		public override bool Equals(object obj)
		{
			return obj != null && obj is bool && this.m_Data == (bool)obj;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003012 File Offset: 0x00001212
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000015 RID: 21
		private bool m_Data;
	}
}
