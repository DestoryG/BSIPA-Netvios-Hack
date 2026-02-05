using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000006 RID: 6
	public class JSONObject : JSONNode
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000029D7 File Offset: 0x00000BD7
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000029DF File Offset: 0x00000BDF
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
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000029E8 File Offset: 0x00000BE8
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Object;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002830 File Offset: 0x00000A30
		public override bool IsObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000029EB File Offset: 0x00000BEB
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
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002AE6 File Offset: 0x00000CE6
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002AF4 File Offset: 0x00000CF4
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

		// Token: 0x06000059 RID: 89 RVA: 0x00002B62 File Offset: 0x00000D62
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

		// Token: 0x0600005A RID: 90 RVA: 0x00002B90 File Offset: 0x00000D90
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

		// Token: 0x0600005B RID: 91 RVA: 0x00002BD8 File Offset: 0x00000DD8
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
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002C44 File Offset: 0x00000E44
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

		// Token: 0x0600005D RID: 93 RVA: 0x00002C54 File Offset: 0x00000E54
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
