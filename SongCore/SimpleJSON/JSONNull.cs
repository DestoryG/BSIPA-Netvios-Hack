using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200000C RID: 12
	public class JSONNull : JSONNode
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00003097 File Offset: 0x00001297
		public static JSONNull CreateOrGet()
		{
			if (JSONNull.reuseSameInstance)
			{
				return JSONNull.m_StaticInstance;
			}
			return new JSONNull();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000030AB File Offset: 0x000012AB
		private JSONNull()
		{
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000030B3 File Offset: 0x000012B3
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.NullValue;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override bool IsNull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000030B8 File Offset: 0x000012B8
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000030CE File Offset: 0x000012CE
		// (set) Token: 0x0600008B RID: 139 RVA: 0x000020D3 File Offset: 0x000002D3
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000020DC File Offset: 0x000002DC
		// (set) Token: 0x0600008D RID: 141 RVA: 0x000020D3 File Offset: 0x000002D3
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

		// Token: 0x0600008E RID: 142 RVA: 0x000030D5 File Offset: 0x000012D5
		public override bool Equals(object obj)
		{
			return this == obj || obj is JSONNull;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000020DC File Offset: 0x000002DC
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000030E6 File Offset: 0x000012E6
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000016 RID: 22
		private static JSONNull m_StaticInstance = new JSONNull();

		// Token: 0x04000017 RID: 23
		public static bool reuseSameInstance = true;
	}
}
