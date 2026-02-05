using System;
using System.Collections.Generic;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x0200007D RID: 125
	public class JSONArray : JSONNode
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00024F93 File Offset: 0x00023193
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x00024F9B File Offset: 0x0002319B
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

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00024FA4 File Offset: 0x000231A4
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Array;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00024FA4 File Offset: 0x000231A4
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00024FA7 File Offset: 0x000231A7
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_List.GetEnumerator());
		}

		// Token: 0x17000259 RID: 601
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

		// Token: 0x1700025A RID: 602
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

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00025044 File Offset: 0x00023244
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00025026 File Offset: 0x00023226
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			this.m_List.Add(aItem);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00025051 File Offset: 0x00023251
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

		// Token: 0x060008CF RID: 2255 RVA: 0x0002507F File Offset: 0x0002327F
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00025090 File Offset: 0x00023290
		public override JSONNode Clone()
		{
			JSONArray jsonarray = new JSONArray();
			jsonarray.m_List.Capacity = this.m_List.Capacity;
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (jsonnode != null)
				{
					jsonarray.Add(jsonnode.Clone());
				}
				else
				{
					jsonarray.Add(null);
				}
			}
			return jsonarray;
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00025118 File Offset: 0x00023318
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

		// Token: 0x060008D2 RID: 2258 RVA: 0x00025128 File Offset: 0x00023328
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

		// Token: 0x04000460 RID: 1120
		private List<JSONNode> m_List = new List<JSONNode>();

		// Token: 0x04000461 RID: 1121
		private bool inline;
	}
}
