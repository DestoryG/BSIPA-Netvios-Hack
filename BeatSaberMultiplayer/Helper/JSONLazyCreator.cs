using System;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000083 RID: 131
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x00025971 File Offset: 0x00023B71
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.None;
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00025974 File Offset: 0x00023B74
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0002598A File Offset: 0x00023B8A
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x000259A0 File Offset: 0x00023BA0
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000259B6 File Offset: 0x00023BB6
		private T Set<T>(T aVal) where T : JSONNode
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
			return aVal;
		}

		// Token: 0x17000275 RID: 629
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set<JSONArray>(new JSONArray()).Add(value);
			}
		}

		// Token: 0x17000276 RID: 630
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set<JSONObject>(new JSONObject()).Add(aKey, value);
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00025A22 File Offset: 0x00023C22
		public override void Add(JSONNode aItem)
		{
			this.Set<JSONArray>(new JSONArray()).Add(aItem);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00025A0E File Offset: 0x00023C0E
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set<JSONObject>(new JSONObject()).Add(aKey, aItem);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00025A35 File Offset: 0x00023C35
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00025A40 File Offset: 0x00023C40
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00025A35 File Offset: 0x00023C35
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000246C1 File Offset: 0x000228C1
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x00025A4C File Offset: 0x00023C4C
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x00025A64 File Offset: 0x00023C64
		public override int AsInt
		{
			get
			{
				this.Set<JSONNumber>(new JSONNumber(0.0));
				return 0;
			}
			set
			{
				this.Set<JSONNumber>(new JSONNumber((double)value));
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x00025A74 File Offset: 0x00023C74
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x00025A64 File Offset: 0x00023C64
		public override float AsFloat
		{
			get
			{
				this.Set<JSONNumber>(new JSONNumber(0.0));
				return 0f;
			}
			set
			{
				this.Set<JSONNumber>(new JSONNumber((double)value));
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x00025A90 File Offset: 0x00023C90
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x00025AB0 File Offset: 0x00023CB0
		public override double AsDouble
		{
			get
			{
				this.Set<JSONNumber>(new JSONNumber(0.0));
				return 0.0;
			}
			set
			{
				this.Set<JSONNumber>(new JSONNumber(value));
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x00025ABF File Offset: 0x00023CBF
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x00025AF2 File Offset: 0x00023CF2
		public override long AsLong
		{
			get
			{
				if (JSONNode.longAsString)
				{
					this.Set<JSONString>(new JSONString("0"));
				}
				else
				{
					this.Set<JSONNumber>(new JSONNumber(0.0));
				}
				return 0L;
			}
			set
			{
				if (JSONNode.longAsString)
				{
					this.Set<JSONString>(new JSONString(value.ToString()));
					return;
				}
				this.Set<JSONNumber>(new JSONNumber((double)value));
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x00025B1D File Offset: 0x00023D1D
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x00025B2D File Offset: 0x00023D2D
		public override bool AsBool
		{
			get
			{
				this.Set<JSONBool>(new JSONBool(false));
				return false;
			}
			set
			{
				this.Set<JSONBool>(new JSONBool(value));
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00025B3C File Offset: 0x00023D3C
		public override JSONArray AsArray
		{
			get
			{
				return this.Set<JSONArray>(new JSONArray());
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00025B49 File Offset: 0x00023D49
		public override JSONObject AsObject
		{
			get
			{
				return this.Set<JSONObject>(new JSONObject());
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00025951 File Offset: 0x00023B51
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000469 RID: 1129
		private JSONNode m_Node;

		// Token: 0x0400046A RID: 1130
		private string m_Key;
	}
}
