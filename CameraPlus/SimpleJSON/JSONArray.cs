using System;
using System.Collections.Generic;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x02000016 RID: 22
	public class JSONArray : JSONNode
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000875F File Offset: 0x0000695F
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00008767 File Offset: 0x00006967
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00008770 File Offset: 0x00006970
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Array;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00008770 File Offset: 0x00006970
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00008773 File Offset: 0x00006973
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_List.GetEnumerator());
		}

		// Token: 0x1700002E RID: 46
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

		// Token: 0x1700002F RID: 47
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00008810 File Offset: 0x00006A10
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000087F2 File Offset: 0x000069F2
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			this.m_List.Add(aItem);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000881D File Offset: 0x00006A1D
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

		// Token: 0x060000DE RID: 222 RVA: 0x0000884B File Offset: 0x00006A4B
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000885B File Offset: 0x00006A5B
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

		// Token: 0x060000E0 RID: 224 RVA: 0x0000886C File Offset: 0x00006A6C
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

		// Token: 0x040000AB RID: 171
		private List<JSONNode> m_List = new List<JSONNode>();

		// Token: 0x040000AC RID: 172
		private bool inline;
	}
}
