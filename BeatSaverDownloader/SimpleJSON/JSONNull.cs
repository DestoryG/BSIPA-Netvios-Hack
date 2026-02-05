using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200000A RID: 10
	public class JSONNull : JSONNode
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000301F File Offset: 0x0000121F
		public static JSONNull CreateOrGet()
		{
			if (JSONNull.reuseSameInstance)
			{
				return JSONNull.m_StaticInstance;
			}
			return new JSONNull();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003033 File Offset: 0x00001233
		private JSONNull()
		{
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000303B File Offset: 0x0000123B
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.NullValue;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002830 File Offset: 0x00000A30
		public override bool IsNull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003040 File Offset: 0x00001240
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003056 File Offset: 0x00001256
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002053 File Offset: 0x00000253
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
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000205C File Offset: 0x0000025C
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00002053 File Offset: 0x00000253
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

		// Token: 0x0600008A RID: 138 RVA: 0x0000305D File Offset: 0x0000125D
		public override bool Equals(object obj)
		{
			return this == obj || obj is JSONNull;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000205C File Offset: 0x0000025C
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000306E File Offset: 0x0000126E
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
