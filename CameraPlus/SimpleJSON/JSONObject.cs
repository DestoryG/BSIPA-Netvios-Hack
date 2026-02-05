using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x02000017 RID: 23
	public class JSONObject : JSONNode
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00008917 File Offset: 0x00006B17
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x0000891F File Offset: 0x00006B1F
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00008928 File Offset: 0x00006B28
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Object;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00008770 File Offset: 0x00006970
		public override bool IsObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000892B File Offset: 0x00006B2B
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_Dict.GetEnumerator());
		}

		// Token: 0x17000035 RID: 53
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

		// Token: 0x17000036 RID: 54
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00008A26 File Offset: 0x00006C26
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00008A34 File Offset: 0x00006C34
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

		// Token: 0x060000ED RID: 237 RVA: 0x00008AA2 File Offset: 0x00006CA2
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

		// Token: 0x060000EE RID: 238 RVA: 0x00008AD0 File Offset: 0x00006CD0
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

		// Token: 0x060000EF RID: 239 RVA: 0x00008B18 File Offset: 0x00006D18
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00008B84 File Offset: 0x00006D84
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

		// Token: 0x060000F1 RID: 241 RVA: 0x00008B94 File Offset: 0x00006D94
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

		// Token: 0x040000AD RID: 173
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		// Token: 0x040000AE RID: 174
		private bool inline;
	}
}
