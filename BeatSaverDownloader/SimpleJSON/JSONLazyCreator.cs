using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200000B RID: 11
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000308E File Offset: 0x0000128E
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.None;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003094 File Offset: 0x00001294
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000030AA File Offset: 0x000012AA
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000030C0 File Offset: 0x000012C0
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000030D6 File Offset: 0x000012D6
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x17000037 RID: 55
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				JSONArray jsonarray = new JSONArray();
				jsonarray.Add(value);
				this.Set(jsonarray);
			}
		}

		// Token: 0x17000038 RID: 56
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				JSONObject jsonobject = new JSONObject();
				jsonobject.Add(aKey, value);
				this.Set(jsonobject);
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003158 File Offset: 0x00001358
		public override void Add(JSONNode aItem)
		{
			JSONArray jsonarray = new JSONArray();
			jsonarray.Add(aItem);
			this.Set(jsonarray);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000317C File Offset: 0x0000137C
		public override void Add(string aKey, JSONNode aItem)
		{
			JSONObject jsonobject = new JSONObject();
			jsonobject.Add(aKey, aItem);
			this.Set(jsonobject);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000319E File Offset: 0x0000139E
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000031A9 File Offset: 0x000013A9
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000319E File Offset: 0x0000139E
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000205C File Offset: 0x0000025C
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000031B8 File Offset: 0x000013B8
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000031DC File Offset: 0x000013DC
		public override int AsInt
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber((double)value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000031F8 File Offset: 0x000013F8
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003220 File Offset: 0x00001420
		public override float AsFloat
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0f;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber((double)value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000323C File Offset: 0x0000143C
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003268 File Offset: 0x00001468
		public override double AsDouble
		{
			get
			{
				JSONNumber jsonnumber = new JSONNumber(0.0);
				this.Set(jsonnumber);
				return 0.0;
			}
			set
			{
				JSONNumber jsonnumber = new JSONNumber(value);
				this.Set(jsonnumber);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003284 File Offset: 0x00001484
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x000032A0 File Offset: 0x000014A0
		public override bool AsBool
		{
			get
			{
				JSONBool jsonbool = new JSONBool(false);
				this.Set(jsonbool);
				return false;
			}
			set
			{
				JSONBool jsonbool = new JSONBool(value);
				this.Set(jsonbool);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000032BC File Offset: 0x000014BC
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000032D8 File Offset: 0x000014D8
		public override JSONObject AsObject
		{
			get
			{
				JSONObject jsonobject = new JSONObject();
				this.Set(jsonobject);
				return jsonobject;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000306E File Offset: 0x0000126E
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000018 RID: 24
		private JSONNode m_Node;

		// Token: 0x04000019 RID: 25
		private string m_Key;
	}
}
