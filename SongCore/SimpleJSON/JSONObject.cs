using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000008 RID: 8
	public class JSONObject : JSONNode
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002A4F File Offset: 0x00000C4F
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002A57 File Offset: 0x00000C57
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002A60 File Offset: 0x00000C60
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Object;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override bool IsObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A63 File Offset: 0x00000C63
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_Dict.GetEnumerator());
		}

		// Token: 0x17000023 RID: 35
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

		// Token: 0x17000024 RID: 36
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002B5E File Offset: 0x00000D5E
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002B6C File Offset: 0x00000D6C
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			if (string.IsNullOrEmpty(aKey))
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

		// Token: 0x0600005D RID: 93 RVA: 0x00002BDA File Offset: 0x00000DDA
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

		// Token: 0x0600005E RID: 94 RVA: 0x00002C08 File Offset: 0x00000E08
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

		// Token: 0x0600005F RID: 95 RVA: 0x00002C50 File Offset: 0x00000E50
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002CBC File Offset: 0x00000EBC
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

		// Token: 0x06000061 RID: 97 RVA: 0x00002CCC File Offset: 0x00000ECC
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

		// Token: 0x04000011 RID: 17
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		// Token: 0x04000012 RID: 18
		private bool inline;
	}
}
