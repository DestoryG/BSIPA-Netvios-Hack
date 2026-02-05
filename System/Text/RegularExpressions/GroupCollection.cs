using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000699 RID: 1689
	[global::__DynamicallyInvokable]
	[Serializable]
	public class GroupCollection : ICollection, IEnumerable
	{
		// Token: 0x06003EC8 RID: 16072 RVA: 0x001055DD File Offset: 0x001037DD
		internal GroupCollection(Match match, Hashtable caps)
		{
			this._match = match;
			this._captureMap = caps;
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x001055F3 File Offset: 0x001037F3
		public object SyncRoot
		{
			get
			{
				return this._match;
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06003ECA RID: 16074 RVA: 0x001055FB File Offset: 0x001037FB
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x001055FE File Offset: 0x001037FE
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06003ECC RID: 16076 RVA: 0x00105601 File Offset: 0x00103801
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._match._matchcount.Length;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		[global::__DynamicallyInvokable]
		public Group this[int groupnum]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetGroup(groupnum);
			}
		}

		// Token: 0x17000EC7 RID: 3783
		[global::__DynamicallyInvokable]
		public Group this[string groupname]
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._match._regex == null)
				{
					return Group._emptygroup;
				}
				return this.GetGroup(this._match._regex.GroupNumberFromName(groupname));
			}
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x00105648 File Offset: 0x00103848
		internal Group GetGroup(int groupnum)
		{
			if (this._captureMap != null)
			{
				object obj = this._captureMap[groupnum];
				if (obj == null)
				{
					return Group._emptygroup;
				}
				return this.GetGroupImpl((int)obj);
			}
			else
			{
				if (groupnum >= this._match._matchcount.Length || groupnum < 0)
				{
					return Group._emptygroup;
				}
				return this.GetGroupImpl(groupnum);
			}
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x001056A8 File Offset: 0x001038A8
		internal Group GetGroupImpl(int groupnum)
		{
			if (groupnum == 0)
			{
				return this._match;
			}
			if (this._groups == null)
			{
				this._groups = new Group[this._match._matchcount.Length - 1];
				for (int i = 0; i < this._groups.Length; i++)
				{
					string text = this._match._regex.GroupNameFromNumber(i + 1);
					this._groups[i] = new Group(this._match._text, this._match._matches[i + 1], this._match._matchcount[i + 1], text);
				}
			}
			return this._groups[groupnum - 1];
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0010574C File Offset: 0x0010394C
		public void CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = arrayIndex;
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], num);
				num++;
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0010578C File Offset: 0x0010398C
		[global::__DynamicallyInvokable]
		public IEnumerator GetEnumerator()
		{
			return new GroupEnumerator(this);
		}

		// Token: 0x04002DD4 RID: 11732
		internal Match _match;

		// Token: 0x04002DD5 RID: 11733
		internal Hashtable _captureMap;

		// Token: 0x04002DD6 RID: 11734
		internal Group[] _groups;
	}
}
