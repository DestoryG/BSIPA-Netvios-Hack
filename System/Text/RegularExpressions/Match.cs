using System;
using System.Security.Permissions;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200069C RID: 1692
	[global::__DynamicallyInvokable]
	[Serializable]
	public class Match : Group
	{
		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06003F02 RID: 16130 RVA: 0x0010708D File Offset: 0x0010528D
		[global::__DynamicallyInvokable]
		public static Match Empty
		{
			[global::__DynamicallyInvokable]
			get
			{
				return Match._empty;
			}
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00107094 File Offset: 0x00105294
		internal Match(Regex regex, int capcount, string text, int begpos, int len, int startpos)
			: base(text, new int[2], 0, "0")
		{
			this._regex = regex;
			this._matchcount = new int[capcount];
			this._matches = new int[capcount][];
			this._matches[0] = this._caps;
			this._textbeg = begpos;
			this._textend = begpos + len;
			this._textstart = startpos;
			this._balancing = false;
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x00107104 File Offset: 0x00105304
		internal virtual void Reset(Regex regex, string text, int textbeg, int textend, int textstart)
		{
			this._regex = regex;
			this._text = text;
			this._textbeg = textbeg;
			this._textend = textend;
			this._textstart = textstart;
			for (int i = 0; i < this._matchcount.Length; i++)
			{
				this._matchcount[i] = 0;
			}
			this._balancing = false;
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06003F05 RID: 16133 RVA: 0x00107159 File Offset: 0x00105359
		[global::__DynamicallyInvokable]
		public virtual GroupCollection Groups
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._groupcoll == null)
				{
					this._groupcoll = new GroupCollection(this, null);
				}
				return this._groupcoll;
			}
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00107176 File Offset: 0x00105376
		[global::__DynamicallyInvokable]
		public Match NextMatch()
		{
			if (this._regex == null)
			{
				return this;
			}
			return this._regex.Run(false, this._length, this._text, this._textbeg, this._textend - this._textbeg, this._textpos);
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x001071B4 File Offset: 0x001053B4
		[global::__DynamicallyInvokable]
		public virtual string Result(string replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			if (this._regex == null)
			{
				throw new NotSupportedException(SR.GetString("NoResultOnFailed"));
			}
			RegexReplacement regexReplacement = (RegexReplacement)this._regex.replref.Get();
			if (regexReplacement == null || !regexReplacement.Pattern.Equals(replacement))
			{
				regexReplacement = RegexParser.ParseReplacement(replacement, this._regex.caps, this._regex.capsize, this._regex.capnames, this._regex.roptions);
				this._regex.replref.Cache(regexReplacement);
			}
			return regexReplacement.Replacement(this);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0010725C File Offset: 0x0010545C
		internal virtual string GroupToStringImpl(int groupnum)
		{
			int num = this._matchcount[groupnum];
			if (num == 0)
			{
				return string.Empty;
			}
			int[] array = this._matches[groupnum];
			return this._text.Substring(array[(num - 1) * 2], array[num * 2 - 1]);
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0010729D File Offset: 0x0010549D
		internal string LastGroupToStringImpl()
		{
			return this.GroupToStringImpl(this._matchcount.Length - 1);
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x001072B0 File Offset: 0x001054B0
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Match Synchronized(Match inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			int num = inner._matchcount.Length;
			for (int i = 0; i < num; i++)
			{
				Group group = inner.Groups[i];
				Group.Synchronized(group);
			}
			return inner;
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x001072F8 File Offset: 0x001054F8
		internal virtual void AddMatch(int cap, int start, int len)
		{
			if (this._matches[cap] == null)
			{
				this._matches[cap] = new int[2];
			}
			int num = this._matchcount[cap];
			if (num * 2 + 2 > this._matches[cap].Length)
			{
				int[] array = this._matches[cap];
				int[] array2 = new int[num * 8];
				for (int i = 0; i < num * 2; i++)
				{
					array2[i] = array[i];
				}
				this._matches[cap] = array2;
			}
			this._matches[cap][num * 2] = start;
			this._matches[cap][num * 2 + 1] = len;
			this._matchcount[cap] = num + 1;
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x00107390 File Offset: 0x00105590
		internal virtual void BalanceMatch(int cap)
		{
			this._balancing = true;
			int num = this._matchcount[cap];
			int num2 = num * 2 - 2;
			if (this._matches[cap][num2] < 0)
			{
				num2 = -3 - this._matches[cap][num2];
			}
			num2 -= 2;
			if (num2 >= 0 && this._matches[cap][num2] < 0)
			{
				this.AddMatch(cap, this._matches[cap][num2], this._matches[cap][num2 + 1]);
				return;
			}
			this.AddMatch(cap, -3 - num2, -4 - num2);
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x00107410 File Offset: 0x00105610
		internal virtual void RemoveMatch(int cap)
		{
			this._matchcount[cap]--;
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x00107423 File Offset: 0x00105623
		internal virtual bool IsMatched(int cap)
		{
			return cap < this._matchcount.Length && this._matchcount[cap] > 0 && this._matches[cap][this._matchcount[cap] * 2 - 1] != -2;
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x0010745C File Offset: 0x0010565C
		internal virtual int MatchIndex(int cap)
		{
			int num = this._matches[cap][this._matchcount[cap] * 2 - 2];
			if (num >= 0)
			{
				return num;
			}
			return this._matches[cap][-3 - num];
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x00107494 File Offset: 0x00105694
		internal virtual int MatchLength(int cap)
		{
			int num = this._matches[cap][this._matchcount[cap] * 2 - 1];
			if (num >= 0)
			{
				return num;
			}
			return this._matches[cap][-3 - num];
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x001074CC File Offset: 0x001056CC
		internal virtual void Tidy(int textpos)
		{
			int[] array = this._matches[0];
			this._index = array[0];
			this._length = array[1];
			this._textpos = textpos;
			this._capcount = this._matchcount[0];
			if (this._balancing)
			{
				for (int i = 0; i < this._matchcount.Length; i++)
				{
					int num = this._matchcount[i] * 2;
					int[] array2 = this._matches[i];
					int j = 0;
					while (j < num && array2[j] >= 0)
					{
						j++;
					}
					int num2 = j;
					while (j < num)
					{
						if (array2[j] < 0)
						{
							num2--;
						}
						else
						{
							if (j != num2)
							{
								array2[num2] = array2[j];
							}
							num2++;
						}
						j++;
					}
					this._matchcount[i] = num2 / 2;
				}
				this._balancing = false;
			}
		}

		// Token: 0x04002DE6 RID: 11750
		internal static Match _empty = new Match(null, 1, string.Empty, 0, 0, 0);

		// Token: 0x04002DE7 RID: 11751
		internal GroupCollection _groupcoll;

		// Token: 0x04002DE8 RID: 11752
		internal Regex _regex;

		// Token: 0x04002DE9 RID: 11753
		internal int _textbeg;

		// Token: 0x04002DEA RID: 11754
		internal int _textpos;

		// Token: 0x04002DEB RID: 11755
		internal int _textend;

		// Token: 0x04002DEC RID: 11756
		internal int _textstart;

		// Token: 0x04002DED RID: 11757
		internal int[][] _matches;

		// Token: 0x04002DEE RID: 11758
		internal int[] _matchcount;

		// Token: 0x04002DEF RID: 11759
		internal bool _balancing;
	}
}
