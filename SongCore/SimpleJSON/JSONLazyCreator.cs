using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x0200000D RID: 13
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003106 File Offset: 0x00001306
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.None;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000310C File Offset: 0x0000130C
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003122 File Offset: 0x00001322
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003138 File Offset: 0x00001338
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000314E File Offset: 0x0000134E
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

		// Token: 0x0600009B RID: 155 RVA: 0x000031D0 File Offset: 0x000013D0
		public override void Add(JSONNode aItem)
		{
			JSONArray jsonarray = new JSONArray();
			jsonarray.Add(aItem);
			this.Set(jsonarray);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000031F4 File Offset: 0x000013F4
		public override void Add(string aKey, JSONNode aItem)
		{
			JSONObject jsonobject = new JSONObject();
			jsonobject.Add(aKey, aItem);
			this.Set(jsonobject);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003216 File Offset: 0x00001416
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003221 File Offset: 0x00001421
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003216 File Offset: 0x00001416
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000020DC File Offset: 0x000002DC
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003230 File Offset: 0x00001430
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003254 File Offset: 0x00001454
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
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003270 File Offset: 0x00001470
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003298 File Offset: 0x00001498
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
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000032B4 File Offset: 0x000014B4
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000032E0 File Offset: 0x000014E0
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
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000032FC File Offset: 0x000014FC
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003318 File Offset: 0x00001518
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
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003334 File Offset: 0x00001534
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
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003350 File Offset: 0x00001550
		public override JSONObject AsObject
		{
			get
			{
				JSONObject jsonobject = new JSONObject();
				this.Set(jsonobject);
				return jsonobject;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000030E6 File Offset: 0x000012E6
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
