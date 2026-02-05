using System;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x0200001B RID: 27
	public class JSONNull : JSONNode
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00008F5F File Offset: 0x0000715F
		public static JSONNull CreateOrGet()
		{
			if (JSONNull.reuseSameInstance)
			{
				return JSONNull.m_StaticInstance;
			}
			return new JSONNull();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00008F73 File Offset: 0x00007173
		private JSONNull()
		{
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00008F7B File Offset: 0x0000717B
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.NullValue;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00008770 File Offset: 0x00006970
		public override bool IsNull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008F80 File Offset: 0x00007180
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00008F96 File Offset: 0x00007196
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00003561 File Offset: 0x00001761
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00007FA7 File Offset: 0x000061A7
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00003561 File Offset: 0x00001761
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

		// Token: 0x0600011E RID: 286 RVA: 0x00008F9D File Offset: 0x0000719D
		public override bool Equals(object obj)
		{
			return this == obj || obj is JSONNull;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007FA7 File Offset: 0x000061A7
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008FAE File Offset: 0x000071AE
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x040000B2 RID: 178
		private static JSONNull m_StaticInstance = new JSONNull();

		// Token: 0x040000B3 RID: 179
		public static bool reuseSameInstance = true;
	}
}
