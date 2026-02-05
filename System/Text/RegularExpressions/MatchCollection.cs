using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200069E RID: 1694
	[global::__DynamicallyInvokable]
	[Serializable]
	public class MatchCollection : ICollection, IEnumerable
	{
		// Token: 0x06003F15 RID: 16149 RVA: 0x001075F0 File Offset: 0x001057F0
		internal MatchCollection(Regex regex, string input, int beginning, int length, int startat)
		{
			if (startat < 0 || startat > input.Length)
			{
				throw new ArgumentOutOfRangeException("startat", SR.GetString("BeginIndexNotNegative"));
			}
			this._regex = regex;
			this._input = input;
			this._beginning = beginning;
			this._length = length;
			this._startat = startat;
			this._prevlen = -1;
			this._matches = new ArrayList();
			this._done = false;
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x00107668 File Offset: 0x00105868
		internal Match GetMatch(int i)
		{
			if (i < 0)
			{
				return null;
			}
			if (this._matches.Count > i)
			{
				return (Match)this._matches[i];
			}
			if (this._done)
			{
				return null;
			}
			for (;;)
			{
				Match match = this._regex.Run(false, this._prevlen, this._input, this._beginning, this._length, this._startat);
				if (!match.Success)
				{
					break;
				}
				this._matches.Add(match);
				this._prevlen = match._length;
				this._startat = match._textpos;
				if (this._matches.Count > i)
				{
					return match;
				}
			}
			this._done = true;
			return null;
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06003F17 RID: 16151 RVA: 0x00107715 File Offset: 0x00105915
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._done)
				{
					return this._matches.Count;
				}
				this.GetMatch(MatchCollection.infinite);
				return this._matches.Count;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06003F18 RID: 16152 RVA: 0x00107742 File Offset: 0x00105942
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x00107745 File Offset: 0x00105945
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06003F1A RID: 16154 RVA: 0x00107748 File Offset: 0x00105948
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		[global::__DynamicallyInvokable]
		public virtual Match this[int i]
		{
			[global::__DynamicallyInvokable]
			get
			{
				Match match = this.GetMatch(i);
				if (match == null)
				{
					throw new ArgumentOutOfRangeException("i");
				}
				return match;
			}
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x00107770 File Offset: 0x00105970
		public void CopyTo(Array array, int arrayIndex)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			int count = this.Count;
			try
			{
				this._matches.CopyTo(array, arrayIndex);
			}
			catch (ArrayTypeMismatchException ex)
			{
				throw new ArgumentException(SR.GetString("Arg_InvalidArrayType"), ex);
			}
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x001077D4 File Offset: 0x001059D4
		[global::__DynamicallyInvokable]
		public IEnumerator GetEnumerator()
		{
			return new MatchEnumerator(this);
		}

		// Token: 0x04002DF1 RID: 11761
		internal Regex _regex;

		// Token: 0x04002DF2 RID: 11762
		internal ArrayList _matches;

		// Token: 0x04002DF3 RID: 11763
		internal bool _done;

		// Token: 0x04002DF4 RID: 11764
		internal string _input;

		// Token: 0x04002DF5 RID: 11765
		internal int _beginning;

		// Token: 0x04002DF6 RID: 11766
		internal int _length;

		// Token: 0x04002DF7 RID: 11767
		internal int _startat;

		// Token: 0x04002DF8 RID: 11768
		internal int _prevlen;

		// Token: 0x04002DF9 RID: 11769
		private static int infinite = int.MaxValue;
	}
}
