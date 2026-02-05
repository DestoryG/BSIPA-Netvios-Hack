using System;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x0200001A RID: 26
	public class JSONBool : JSONNode
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00008EB1 File Offset: 0x000070B1
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Boolean;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00008770 File Offset: 0x00006970
		public override bool IsBoolean
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008EB4 File Offset: 0x000070B4
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00008ECA File Offset: 0x000070CA
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00008ED8 File Offset: 0x000070D8
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00008EF6 File Offset: 0x000070F6
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00008EFE File Offset: 0x000070FE
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

		// Token: 0x06000110 RID: 272 RVA: 0x00008F07 File Offset: 0x00007107
		public JSONBool(bool aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008DCA File Offset: 0x00006FCA
		public JSONBool(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00008F16 File Offset: 0x00007116
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data ? "true" : "false");
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00008F33 File Offset: 0x00007133
		public override bool Equals(object obj)
		{
			return obj != null && obj is bool && this.m_Data == (bool)obj;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008F52 File Offset: 0x00007152
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x040000B1 RID: 177
		private bool m_Data;
	}
}
