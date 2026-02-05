using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000007 RID: 7
	public class JSONArray : JSONNode
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002897 File Offset: 0x00000A97
		// (set) Token: 0x06000043 RID: 67 RVA: 0x0000289F File Offset: 0x00000A9F
		public override bool Inline
		{
			get
			{
				return this.inline;
			}
			set
			{
				this.inline = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Array;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000028AB File Offset: 0x00000AAB
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_List.GetEnumerator());
		}

		// Token: 0x1700001C RID: 28
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		// Token: 0x1700001D RID: 29
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				this.m_List.Add(value);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002948 File Offset: 0x00000B48
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000292A File Offset: 0x00000B2A
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			this.m_List.Add(aItem);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002955 File Offset: 0x00000B55
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode jsonnode = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return jsonnode;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002983 File Offset: 0x00000B83
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002993 File Offset: 0x00000B93
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000029A4 File Offset: 0x00000BA4
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('[');
			int count = this.m_List.Count;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
				{
					aSB.Append(',');
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				this.m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append(']');
		}

		// Token: 0x0400000F RID: 15
		private List<JSONNode> m_List = new List<JSONNode>();

		// Token: 0x04000010 RID: 16
		private bool inline;
	}
}
