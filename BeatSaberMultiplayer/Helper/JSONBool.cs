using System;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000081 RID: 129
	public class JSONBool : JSONNode
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00025841 File Offset: 0x00023A41
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Boolean;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00024FA4 File Offset: 0x000231A4
		public override bool IsBoolean
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00025844 File Offset: 0x00023A44
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0002585A File Offset: 0x00023A5A
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x00025868 File Offset: 0x00023A68
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

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00025886 File Offset: 0x00023A86
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x0002588E File Offset: 0x00023A8E
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

		// Token: 0x06000909 RID: 2313 RVA: 0x00025897 File Offset: 0x00023A97
		public JSONBool(bool aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002574B File Offset: 0x0002394B
		public JSONBool(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x000258A6 File Offset: 0x00023AA6
		public override JSONNode Clone()
		{
			return new JSONBool(this.m_Data);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x000258B3 File Offset: 0x00023AB3
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data ? "true" : "false");
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000258D0 File Offset: 0x00023AD0
		public override bool Equals(object obj)
		{
			return obj != null && obj is bool && this.m_Data == (bool)obj;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x000258EF File Offset: 0x00023AEF
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000466 RID: 1126
		private bool m_Data;
	}
}
