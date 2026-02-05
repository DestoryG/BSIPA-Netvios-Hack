using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200068D RID: 1677
	[global::__DynamicallyInvokable]
	[Serializable]
	public class CaptureCollection : ICollection, IEnumerable
	{
		// Token: 0x06003DEE RID: 15854 RVA: 0x000FDB91 File Offset: 0x000FBD91
		internal CaptureCollection(Group group)
		{
			this._group = group;
			this._capcount = this._group._capcount;
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06003DEF RID: 15855 RVA: 0x000FDBB1 File Offset: 0x000FBDB1
		public object SyncRoot
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x000FDBB9 File Offset: 0x000FBDB9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x000FDBBC File Offset: 0x000FBDBC
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x000FDBBF File Offset: 0x000FBDBF
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._capcount;
			}
		}

		// Token: 0x17000EB1 RID: 3761
		[global::__DynamicallyInvokable]
		public Capture this[int i]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetCapture(i);
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x000FDBD0 File Offset: 0x000FBDD0
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

		// Token: 0x06003DF5 RID: 15861 RVA: 0x000FDC10 File Offset: 0x000FBE10
		[global::__DynamicallyInvokable]
		public IEnumerator GetEnumerator()
		{
			return new CaptureEnumerator(this);
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x000FDC18 File Offset: 0x000FBE18
		internal Capture GetCapture(int i)
		{
			if (i == this._capcount - 1 && i >= 0)
			{
				return this._group;
			}
			if (i >= this._capcount || i < 0)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			if (this._captures == null)
			{
				this._captures = new Capture[this._capcount];
				for (int j = 0; j < this._capcount - 1; j++)
				{
					this._captures[j] = new Capture(this._group._text, this._group._caps[j * 2], this._group._caps[j * 2 + 1]);
				}
			}
			return this._captures[i];
		}

		// Token: 0x04002CF6 RID: 11510
		internal Group _group;

		// Token: 0x04002CF7 RID: 11511
		internal int _capcount;

		// Token: 0x04002CF8 RID: 11512
		internal Capture[] _captures;
	}
}
