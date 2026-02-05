using System;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x0200001C RID: 28
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00008FCE File Offset: 0x000071CE
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.None;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008FD4 File Offset: 0x000071D4
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008FEA File Offset: 0x000071EA
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00009000 File Offset: 0x00007200
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009016 File Offset: 0x00007216
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

		// Token: 0x17000049 RID: 73
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

		// Token: 0x1700004A RID: 74
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

		// Token: 0x0600012B RID: 299 RVA: 0x00009098 File Offset: 0x00007298
		public override void Add(JSONNode aItem)
		{
			JSONArray jsonarray = new JSONArray();
			jsonarray.Add(aItem);
			this.Set(jsonarray);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000090BC File Offset: 0x000072BC
		public override void Add(string aKey, JSONNode aItem)
		{
			JSONObject jsonobject = new JSONObject();
			jsonobject.Add(aKey, aItem);
			this.Set(jsonobject);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000090DE File Offset: 0x000072DE
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000090E9 File Offset: 0x000072E9
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000090DE File Offset: 0x000072DE
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007FA7 File Offset: 0x000061A7
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000090F8 File Offset: 0x000072F8
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000911C File Offset: 0x0000731C
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00009138 File Offset: 0x00007338
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00009160 File Offset: 0x00007360
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000917C File Offset: 0x0000737C
		// (set) Token: 0x06000136 RID: 310 RVA: 0x000091A8 File Offset: 0x000073A8
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000091C4 File Offset: 0x000073C4
		// (set) Token: 0x06000138 RID: 312 RVA: 0x000091E0 File Offset: 0x000073E0
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000091FC File Offset: 0x000073FC
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00009218 File Offset: 0x00007418
		public override JSONObject AsObject
		{
			get
			{
				JSONObject jsonobject = new JSONObject();
				this.Set(jsonobject);
				return jsonobject;
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008FAE File Offset: 0x000071AE
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x040000B4 RID: 180
		private JSONNode m_Node;

		// Token: 0x040000B5 RID: 181
		private string m_Key;
	}
}
