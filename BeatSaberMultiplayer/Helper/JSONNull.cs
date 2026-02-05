using System;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000082 RID: 130
	public class JSONNull : JSONNode
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x000258FC File Offset: 0x00023AFC
		public static JSONNull CreateOrGet()
		{
			if (JSONNull.reuseSameInstance)
			{
				return JSONNull.m_StaticInstance;
			}
			return new JSONNull();
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00025910 File Offset: 0x00023B10
		private JSONNull()
		{
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x00025918 File Offset: 0x00023B18
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.NullValue;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00024FA4 File Offset: 0x000231A4
		public override bool IsNull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002591C File Offset: 0x00023B1C
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00025932 File Offset: 0x00023B32
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x000196A0 File Offset: 0x000178A0
		public override string Value
		{
			get
			{
				return "null";
			}
			set
			{
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x000246C1 File Offset: 0x000228C1
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x000196A0 File Offset: 0x000178A0
		public override bool AsBool
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00025939 File Offset: 0x00023B39
		public override JSONNode Clone()
		{
			return JSONNull.CreateOrGet();
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00025940 File Offset: 0x00023B40
		public override bool Equals(object obj)
		{
			return this == obj || obj is JSONNull;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x000246C1 File Offset: 0x000228C1
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00025951 File Offset: 0x00023B51
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000467 RID: 1127
		private static JSONNull m_StaticInstance = new JSONNull();

		// Token: 0x04000468 RID: 1128
		public static bool reuseSameInstance = true;
	}
}
