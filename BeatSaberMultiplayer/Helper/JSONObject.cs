using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x0200007E RID: 126
	public class JSONObject : JSONNode
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x000251D3 File Offset: 0x000233D3
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x000251DB File Offset: 0x000233DB
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

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x000251E4 File Offset: 0x000233E4
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Object;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00024FA4 File Offset: 0x000231A4
		public override bool IsObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x000251E7 File Offset: 0x000233E7
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_Dict.GetEnumerator());
		}

		// Token: 0x17000260 RID: 608
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
					return;
				}
				this.m_Dict.Add(aKey, value);
			}
		}

		// Token: 0x17000261 RID: 609
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000252E2 File Offset: 0x000234E2
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x000252F0 File Offset: 0x000234F0
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			if (aKey == null)
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
				return;
			}
			if (this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict[aKey] = aItem;
				return;
			}
			this.m_Dict.Add(aKey, aItem);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00025359 File Offset: 0x00023559
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode jsonnode = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return jsonnode;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00025384 File Offset: 0x00023584
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x000253CC File Offset: 0x000235CC
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode jsonnode;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.Where((KeyValuePair<string, JSONNode> k) => k.Value == aNode).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				jsonnode = aNode;
			}
			catch
			{
				jsonnode = null;
			}
			return jsonnode;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00025438 File Offset: 0x00023638
		public override JSONNode Clone()
		{
			JSONObject jsonobject = new JSONObject();
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				jsonobject.Add(keyValuePair.Key, keyValuePair.Value.Clone());
			}
			return jsonobject;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x000254A4 File Offset: 0x000236A4
		public override bool HasKey(string aKey)
		{
			return this.m_Dict.ContainsKey(aKey);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000254B4 File Offset: 0x000236B4
		public override JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
		{
			JSONNode jsonnode;
			if (this.m_Dict.TryGetValue(aKey, out jsonnode))
			{
				return jsonnode;
			}
			return aDefault;
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x000254D4 File Offset: 0x000236D4
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
				{
					yield return keyValuePair.Value;
				}
				Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x000254E4 File Offset: 0x000236E4
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('{');
			bool flag = true;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (!flag)
				{
					aSB.Append(',');
				}
				flag = false;
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				aSB.Append('"').Append(JSONNode.Escape(keyValuePair.Key)).Append('"');
				if (aMode == JSONTextMode.Compact)
				{
					aSB.Append(':');
				}
				else
				{
					aSB.Append(" : ");
				}
				keyValuePair.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append('}');
		}

		// Token: 0x04000462 RID: 1122
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		// Token: 0x04000463 RID: 1123
		private bool inline;
	}
}
