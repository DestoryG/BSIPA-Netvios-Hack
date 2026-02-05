using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000005 RID: 5
	public class JSONArray : JSONNode
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000281F File Offset: 0x00000A1F
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002827 File Offset: 0x00000A27
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
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002830 File Offset: 0x00000A30
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Array;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002830 File Offset: 0x00000A30
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002833 File Offset: 0x00000A33
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
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000028D0 File Offset: 0x00000AD0
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000028B2 File Offset: 0x00000AB2
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			this.m_List.Add(aItem);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000028DD File Offset: 0x00000ADD
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

		// Token: 0x0600004A RID: 74 RVA: 0x0000290B File Offset: 0x00000B0B
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000291B File Offset: 0x00000B1B
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

		// Token: 0x0600004C RID: 76 RVA: 0x0000292C File Offset: 0x00000B2C
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
