using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000686 RID: 1670
	[global::__DynamicallyInvokable]
	[Serializable]
	public class Regex : ISerializable
	{
		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x000FC66F File Offset: 0x000FA86F
		// (set) Token: 0x06003D8E RID: 15758 RVA: 0x000FC677 File Offset: 0x000FA877
		[CLSCompliant(false)]
		protected IDictionary Caps
		{
			get
			{
				return this.caps;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.caps = value as Hashtable;
				if (this.caps == null)
				{
					this.caps = new Hashtable(value);
				}
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x000FC6A7 File Offset: 0x000FA8A7
		// (set) Token: 0x06003D90 RID: 15760 RVA: 0x000FC6AF File Offset: 0x000FA8AF
		[CLSCompliant(false)]
		protected IDictionary CapNames
		{
			get
			{
				return this.capnames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.capnames = value as Hashtable;
				if (this.capnames == null)
				{
					this.capnames = new Hashtable(value);
				}
			}
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x000FC6DF File Offset: 0x000FA8DF
		[global::__DynamicallyInvokable]
		protected Regex()
		{
			this.internalMatchTimeout = Regex.DefaultMatchTimeout;
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x000FC6F2 File Offset: 0x000FA8F2
		[global::__DynamicallyInvokable]
		public Regex(string pattern)
			: this(pattern, RegexOptions.None, Regex.DefaultMatchTimeout, false)
		{
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x000FC702 File Offset: 0x000FA902
		[global::__DynamicallyInvokable]
		public Regex(string pattern, RegexOptions options)
			: this(pattern, options, Regex.DefaultMatchTimeout, false)
		{
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x000FC712 File Offset: 0x000FA912
		[global::__DynamicallyInvokable]
		public Regex(string pattern, RegexOptions options, TimeSpan matchTimeout)
			: this(pattern, options, matchTimeout, false)
		{
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x000FC720 File Offset: 0x000FA920
		private Regex(string pattern, RegexOptions options, TimeSpan matchTimeout, bool useCache)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			if (options < RegexOptions.None || options >> 10 != RegexOptions.None)
			{
				throw new ArgumentOutOfRangeException("options");
			}
			if ((options & RegexOptions.ECMAScript) != RegexOptions.None && (options & ~(RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.CultureInvariant)) != RegexOptions.None)
			{
				throw new ArgumentOutOfRangeException("options");
			}
			Regex.ValidateMatchTimeout(matchTimeout);
			string text;
			if ((options & RegexOptions.CultureInvariant) != RegexOptions.None)
			{
				text = CultureInfo.InvariantCulture.ToString();
			}
			else
			{
				text = CultureInfo.CurrentCulture.ToString();
			}
			string[] array = new string[5];
			int num = 0;
			int num2 = (int)options;
			array[num] = num2.ToString(NumberFormatInfo.InvariantInfo);
			array[1] = ":";
			array[2] = text;
			array[3] = ":";
			array[4] = pattern;
			string text2 = string.Concat(array);
			CachedCodeEntry cachedCodeEntry = Regex.LookupCachedAndUpdate(text2);
			this.pattern = pattern;
			this.roptions = options;
			this.internalMatchTimeout = matchTimeout;
			if (cachedCodeEntry == null)
			{
				RegexTree regexTree = RegexParser.Parse(pattern, this.roptions);
				this.capnames = regexTree._capnames;
				this.capslist = regexTree._capslist;
				this.code = RegexWriter.Write(regexTree);
				this.caps = this.code._caps;
				this.capsize = this.code._capsize;
				this.InitializeReferences();
				if (useCache)
				{
					cachedCodeEntry = this.CacheCode(text2);
				}
			}
			else
			{
				this.caps = cachedCodeEntry._caps;
				this.capnames = cachedCodeEntry._capnames;
				this.capslist = cachedCodeEntry._capslist;
				this.capsize = cachedCodeEntry._capsize;
				this.code = cachedCodeEntry._code;
				this.factory = cachedCodeEntry._factory;
				this.runnerref = cachedCodeEntry._runnerref;
				this.replref = cachedCodeEntry._replref;
				this.refsInitialized = true;
			}
			if (this.UseOptionC() && this.factory == null)
			{
				this.factory = this.Compile(this.code, this.roptions);
				if (useCache && cachedCodeEntry != null)
				{
					cachedCodeEntry.AddCompiled(this.factory);
				}
				this.code = null;
			}
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x000FC908 File Offset: 0x000FAB08
		protected Regex(SerializationInfo info, StreamingContext context)
			: this(info.GetString("pattern"), (RegexOptions)info.GetInt32("options"))
		{
			try
			{
				long @int = info.GetInt64("matchTimeout");
				TimeSpan timeSpan = new TimeSpan(@int);
				Regex.ValidateMatchTimeout(timeSpan);
				this.internalMatchTimeout = timeSpan;
			}
			catch (SerializationException)
			{
			}
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x000FC968 File Offset: 0x000FAB68
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			si.AddValue("pattern", this.ToString());
			si.AddValue("options", this.Options);
			si.AddValue("matchTimeout", this.MatchTimeout.Ticks);
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x000FC9B5 File Offset: 0x000FABB5
		protected internal static void ValidateMatchTimeout(TimeSpan matchTimeout)
		{
			if (Regex.InfiniteMatchTimeout == matchTimeout)
			{
				return;
			}
			if (TimeSpan.Zero < matchTimeout && matchTimeout <= Regex.MaximumMatchTimeout)
			{
				return;
			}
			throw new ArgumentOutOfRangeException("matchTimeout");
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x000FC9EC File Offset: 0x000FABEC
		private static TimeSpan InitDefaultMatchTimeout()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			object data = currentDomain.GetData("REGEX_DEFAULT_MATCH_TIMEOUT");
			if (data == null)
			{
				return Regex.FallbackDefaultMatchTimeout;
			}
			if (!(data is TimeSpan))
			{
				throw new InvalidCastException(SR.GetString("IllegalDefaultRegexMatchTimeoutInAppDomain", new object[] { "REGEX_DEFAULT_MATCH_TIMEOUT" }));
			}
			TimeSpan timeSpan = (TimeSpan)data;
			try
			{
				Regex.ValidateMatchTimeout(timeSpan);
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("IllegalDefaultRegexMatchTimeoutInAppDomain", new object[] { "REGEX_DEFAULT_MATCH_TIMEOUT" }));
			}
			return timeSpan;
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x000FCA7C File Offset: 0x000FAC7C
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal RegexRunnerFactory Compile(RegexCode code, RegexOptions roptions)
		{
			return RegexCompiler.Compile(code, roptions);
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x000FCA85 File Offset: 0x000FAC85
		[global::__DynamicallyInvokable]
		public static string Escape(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return RegexParser.Escape(str);
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x000FCA9B File Offset: 0x000FAC9B
		[global::__DynamicallyInvokable]
		public static string Unescape(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return RegexParser.Unescape(str);
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06003D9D RID: 15773 RVA: 0x000FCAB1 File Offset: 0x000FACB1
		// (set) Token: 0x06003D9E RID: 15774 RVA: 0x000FCAB8 File Offset: 0x000FACB8
		[global::__DynamicallyInvokable]
		public static int CacheSize
		{
			[global::__DynamicallyInvokable]
			get
			{
				return Regex.cacheSize;
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				Regex.cacheSize = value;
				if (Regex.livecode.Count > Regex.cacheSize)
				{
					LinkedList<CachedCodeEntry> linkedList = Regex.livecode;
					lock (linkedList)
					{
						while (Regex.livecode.Count > Regex.cacheSize)
						{
							Regex.livecode.RemoveLast();
						}
					}
				}
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x000FCB34 File Offset: 0x000FAD34
		[global::__DynamicallyInvokable]
		public RegexOptions Options
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.roptions;
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x000FCB3C File Offset: 0x000FAD3C
		[global::__DynamicallyInvokable]
		public TimeSpan MatchTimeout
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.internalMatchTimeout;
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x000FCB44 File Offset: 0x000FAD44
		[global::__DynamicallyInvokable]
		public bool RightToLeft
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.UseOptionR();
			}
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x000FCB4C File Offset: 0x000FAD4C
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			return this.pattern;
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x000FCB54 File Offset: 0x000FAD54
		[global::__DynamicallyInvokable]
		public string[] GetGroupNames()
		{
			string[] array;
			if (this.capslist == null)
			{
				int num = this.capsize;
				array = new string[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = Convert.ToString(i, CultureInfo.InvariantCulture);
				}
			}
			else
			{
				array = new string[this.capslist.Length];
				Array.Copy(this.capslist, 0, array, 0, this.capslist.Length);
			}
			return array;
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x000FCBB8 File Offset: 0x000FADB8
		[global::__DynamicallyInvokable]
		public int[] GetGroupNumbers()
		{
			int[] array;
			if (this.caps == null)
			{
				int num = this.capsize;
				array = new int[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = i;
				}
			}
			else
			{
				array = new int[this.caps.Count];
				IDictionaryEnumerator enumerator = this.caps.GetEnumerator();
				while (enumerator.MoveNext())
				{
					array[(int)enumerator.Value] = (int)enumerator.Key;
				}
			}
			return array;
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x000FCC30 File Offset: 0x000FAE30
		[global::__DynamicallyInvokable]
		public string GroupNameFromNumber(int i)
		{
			if (this.capslist == null)
			{
				if (i >= 0 && i < this.capsize)
				{
					return i.ToString(CultureInfo.InvariantCulture);
				}
				return string.Empty;
			}
			else
			{
				if (this.caps != null)
				{
					object obj = this.caps[i];
					if (obj == null)
					{
						return string.Empty;
					}
					i = (int)obj;
				}
				if (i >= 0 && i < this.capslist.Length)
				{
					return this.capslist[i];
				}
				return string.Empty;
			}
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x000FCCB0 File Offset: 0x000FAEB0
		[global::__DynamicallyInvokable]
		public int GroupNumberFromName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.capnames != null)
			{
				object obj = this.capnames[name];
				if (obj == null)
				{
					return -1;
				}
				return (int)obj;
			}
			else
			{
				int num = 0;
				foreach (char c in name)
				{
					if (c > '9' || c < '0')
					{
						return -1;
					}
					num *= 10;
					num += (int)(c - '0');
				}
				if (num >= 0 && num < this.capsize)
				{
					return num;
				}
				return -1;
			}
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x000FCD31 File Offset: 0x000FAF31
		[global::__DynamicallyInvokable]
		public static bool IsMatch(string input, string pattern)
		{
			return Regex.IsMatch(input, pattern, RegexOptions.None, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x000FCD40 File Offset: 0x000FAF40
		[global::__DynamicallyInvokable]
		public static bool IsMatch(string input, string pattern, RegexOptions options)
		{
			return Regex.IsMatch(input, pattern, options, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x000FCD4F File Offset: 0x000FAF4F
		[global::__DynamicallyInvokable]
		public static bool IsMatch(string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
		{
			return new Regex(pattern, options, matchTimeout, true).IsMatch(input);
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x000FCD60 File Offset: 0x000FAF60
		[global::__DynamicallyInvokable]
		public bool IsMatch(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.IsMatch(input, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x000FCD88 File Offset: 0x000FAF88
		[global::__DynamicallyInvokable]
		public bool IsMatch(string input, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Run(true, -1, input, 0, input.Length, startat) == null;
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x000FCDAC File Offset: 0x000FAFAC
		[global::__DynamicallyInvokable]
		public static Match Match(string input, string pattern)
		{
			return Regex.Match(input, pattern, RegexOptions.None, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x000FCDBB File Offset: 0x000FAFBB
		[global::__DynamicallyInvokable]
		public static Match Match(string input, string pattern, RegexOptions options)
		{
			return Regex.Match(input, pattern, options, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x000FCDCA File Offset: 0x000FAFCA
		[global::__DynamicallyInvokable]
		public static Match Match(string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
		{
			return new Regex(pattern, options, matchTimeout, true).Match(input);
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x000FCDDB File Offset: 0x000FAFDB
		[global::__DynamicallyInvokable]
		public Match Match(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Match(input, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x000FCE03 File Offset: 0x000FB003
		[global::__DynamicallyInvokable]
		public Match Match(string input, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Run(false, -1, input, 0, input.Length, startat);
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x000FCE24 File Offset: 0x000FB024
		[global::__DynamicallyInvokable]
		public Match Match(string input, int beginning, int length)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Run(false, -1, input, beginning, length, this.UseOptionR() ? (beginning + length) : beginning);
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x000FCE4D File Offset: 0x000FB04D
		[global::__DynamicallyInvokable]
		public static MatchCollection Matches(string input, string pattern)
		{
			return Regex.Matches(input, pattern, RegexOptions.None, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x000FCE5C File Offset: 0x000FB05C
		[global::__DynamicallyInvokable]
		public static MatchCollection Matches(string input, string pattern, RegexOptions options)
		{
			return Regex.Matches(input, pattern, options, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x000FCE6B File Offset: 0x000FB06B
		[global::__DynamicallyInvokable]
		public static MatchCollection Matches(string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
		{
			return new Regex(pattern, options, matchTimeout, true).Matches(input);
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x000FCE7C File Offset: 0x000FB07C
		[global::__DynamicallyInvokable]
		public MatchCollection Matches(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Matches(input, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x000FCEA4 File Offset: 0x000FB0A4
		[global::__DynamicallyInvokable]
		public MatchCollection Matches(string input, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new MatchCollection(this, input, 0, input.Length, startat);
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x000FCEC3 File Offset: 0x000FB0C3
		[global::__DynamicallyInvokable]
		public static string Replace(string input, string pattern, string replacement)
		{
			return Regex.Replace(input, pattern, replacement, RegexOptions.None, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x000FCED3 File Offset: 0x000FB0D3
		[global::__DynamicallyInvokable]
		public static string Replace(string input, string pattern, string replacement, RegexOptions options)
		{
			return Regex.Replace(input, pattern, replacement, options, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x000FCEE3 File Offset: 0x000FB0E3
		[global::__DynamicallyInvokable]
		public static string Replace(string input, string pattern, string replacement, RegexOptions options, TimeSpan matchTimeout)
		{
			return new Regex(pattern, options, matchTimeout, true).Replace(input, replacement);
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x000FCEF6 File Offset: 0x000FB0F6
		[global::__DynamicallyInvokable]
		public string Replace(string input, string replacement)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, replacement, -1, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x000FCF20 File Offset: 0x000FB120
		[global::__DynamicallyInvokable]
		public string Replace(string input, string replacement, int count)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, replacement, count, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x000FCF4C File Offset: 0x000FB14C
		[global::__DynamicallyInvokable]
		public string Replace(string input, string replacement, int count, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			RegexReplacement regexReplacement = (RegexReplacement)this.replref.Get();
			if (regexReplacement == null || !regexReplacement.Pattern.Equals(replacement))
			{
				regexReplacement = RegexParser.ParseReplacement(replacement, this.caps, this.capsize, this.capnames, this.roptions);
				this.replref.Cache(regexReplacement);
			}
			return regexReplacement.Replace(this, input, count, startat);
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x000FCFCD File Offset: 0x000FB1CD
		[global::__DynamicallyInvokable]
		public static string Replace(string input, string pattern, MatchEvaluator evaluator)
		{
			return Regex.Replace(input, pattern, evaluator, RegexOptions.None, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x000FCFDD File Offset: 0x000FB1DD
		[global::__DynamicallyInvokable]
		public static string Replace(string input, string pattern, MatchEvaluator evaluator, RegexOptions options)
		{
			return Regex.Replace(input, pattern, evaluator, options, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x000FCFED File Offset: 0x000FB1ED
		[global::__DynamicallyInvokable]
		public static string Replace(string input, string pattern, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout)
		{
			return new Regex(pattern, options, matchTimeout, true).Replace(input, evaluator);
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x000FD000 File Offset: 0x000FB200
		[global::__DynamicallyInvokable]
		public string Replace(string input, MatchEvaluator evaluator)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, evaluator, -1, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x000FD02A File Offset: 0x000FB22A
		[global::__DynamicallyInvokable]
		public string Replace(string input, MatchEvaluator evaluator, int count)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, evaluator, count, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x000FD054 File Offset: 0x000FB254
		[global::__DynamicallyInvokable]
		public string Replace(string input, MatchEvaluator evaluator, int count, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return RegexReplacement.Replace(evaluator, this, input, count, startat);
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x000FD06F File Offset: 0x000FB26F
		[global::__DynamicallyInvokable]
		public static string[] Split(string input, string pattern)
		{
			return Regex.Split(input, pattern, RegexOptions.None, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x000FD07E File Offset: 0x000FB27E
		[global::__DynamicallyInvokable]
		public static string[] Split(string input, string pattern, RegexOptions options)
		{
			return Regex.Split(input, pattern, options, Regex.DefaultMatchTimeout);
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x000FD08D File Offset: 0x000FB28D
		[global::__DynamicallyInvokable]
		public static string[] Split(string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
		{
			return new Regex(pattern, options, matchTimeout, true).Split(input);
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x000FD09E File Offset: 0x000FB29E
		[global::__DynamicallyInvokable]
		public string[] Split(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Split(input, 0, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x000FD0C7 File Offset: 0x000FB2C7
		[global::__DynamicallyInvokable]
		public string[] Split(string input, int count)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return RegexReplacement.Split(this, input, count, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x000FD0F0 File Offset: 0x000FB2F0
		[global::__DynamicallyInvokable]
		public string[] Split(string input, int count, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return RegexReplacement.Split(this, input, count, startat);
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x000FD109 File Offset: 0x000FB309
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname)
		{
			Regex.CompileToAssemblyInternal(regexinfos, assemblyname, null, null);
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x000FD114 File Offset: 0x000FB314
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[] attributes)
		{
			Regex.CompileToAssemblyInternal(regexinfos, assemblyname, attributes, null);
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x000FD11F File Offset: 0x000FB31F
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[] attributes, string resourceFile)
		{
			Regex.CompileToAssemblyInternal(regexinfos, assemblyname, attributes, resourceFile);
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x000FD12A File Offset: 0x000FB32A
		private static void CompileToAssemblyInternal(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[] attributes, string resourceFile)
		{
			if (assemblyname == null)
			{
				throw new ArgumentNullException("assemblyname");
			}
			if (regexinfos == null)
			{
				throw new ArgumentNullException("regexinfos");
			}
			RegexCompiler.CompileToAssembly(regexinfos, assemblyname, attributes, resourceFile);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x000FD151 File Offset: 0x000FB351
		protected void InitializeReferences()
		{
			if (this.refsInitialized)
			{
				throw new NotSupportedException(SR.GetString("OnlyAllowedOnce"));
			}
			this.refsInitialized = true;
			this.runnerref = new ExclusiveReference();
			this.replref = new SharedReference();
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x000FD188 File Offset: 0x000FB388
		internal Match Run(bool quick, int prevlen, string input, int beginning, int length, int startat)
		{
			RegexRunner regexRunner = null;
			if (startat < 0 || startat > input.Length)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("BeginIndexNotNegative"));
			}
			if (length < 0 || length > input.Length)
			{
				throw new ArgumentOutOfRangeException("length", SR.GetString("LengthNotNegative"));
			}
			regexRunner = (RegexRunner)this.runnerref.Get();
			if (regexRunner == null)
			{
				if (this.factory != null)
				{
					regexRunner = this.factory.CreateInstance();
				}
				else
				{
					regexRunner = new RegexInterpreter(this.code, this.UseOptionInvariant() ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
				}
			}
			Match match;
			try
			{
				match = regexRunner.Scan(this, input, beginning, beginning + length, startat, prevlen, quick, this.internalMatchTimeout);
			}
			finally
			{
				this.runnerref.Release(regexRunner);
			}
			return match;
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x000FD264 File Offset: 0x000FB464
		private static CachedCodeEntry LookupCachedAndUpdate(string key)
		{
			LinkedList<CachedCodeEntry> linkedList = Regex.livecode;
			lock (linkedList)
			{
				for (LinkedListNode<CachedCodeEntry> linkedListNode = Regex.livecode.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					if (linkedListNode.Value._key == key)
					{
						Regex.livecode.Remove(linkedListNode);
						Regex.livecode.AddFirst(linkedListNode);
						return linkedListNode.Value;
					}
				}
			}
			return null;
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x000FD2EC File Offset: 0x000FB4EC
		private CachedCodeEntry CacheCode(string key)
		{
			CachedCodeEntry cachedCodeEntry = null;
			LinkedList<CachedCodeEntry> linkedList = Regex.livecode;
			lock (linkedList)
			{
				for (LinkedListNode<CachedCodeEntry> linkedListNode = Regex.livecode.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					if (linkedListNode.Value._key == key)
					{
						Regex.livecode.Remove(linkedListNode);
						Regex.livecode.AddFirst(linkedListNode);
						return linkedListNode.Value;
					}
				}
				if (Regex.cacheSize != 0)
				{
					cachedCodeEntry = new CachedCodeEntry(key, this.capnames, this.capslist, this.code, this.caps, this.capsize, this.runnerref, this.replref);
					Regex.livecode.AddFirst(cachedCodeEntry);
					if (Regex.livecode.Count > Regex.cacheSize)
					{
						Regex.livecode.RemoveLast();
					}
				}
			}
			return cachedCodeEntry;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x000FD3D4 File Offset: 0x000FB5D4
		protected bool UseOptionC()
		{
			return (this.roptions & RegexOptions.Compiled) > RegexOptions.None;
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x000FD3E1 File Offset: 0x000FB5E1
		protected bool UseOptionR()
		{
			return (this.roptions & RegexOptions.RightToLeft) > RegexOptions.None;
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x000FD3EF File Offset: 0x000FB5EF
		internal bool UseOptionInvariant()
		{
			return (this.roptions & RegexOptions.CultureInvariant) > RegexOptions.None;
		}

		// Token: 0x04002CC7 RID: 11463
		protected internal string pattern;

		// Token: 0x04002CC8 RID: 11464
		protected internal RegexRunnerFactory factory;

		// Token: 0x04002CC9 RID: 11465
		protected internal RegexOptions roptions;

		// Token: 0x04002CCA RID: 11466
		[NonSerialized]
		private static readonly TimeSpan MaximumMatchTimeout = TimeSpan.FromMilliseconds(2147483646.0);

		// Token: 0x04002CCB RID: 11467
		[global::__DynamicallyInvokable]
		[NonSerialized]
		public static readonly TimeSpan InfiniteMatchTimeout = Timeout.InfiniteTimeSpan;

		// Token: 0x04002CCC RID: 11468
		[OptionalField(VersionAdded = 2)]
		protected internal TimeSpan internalMatchTimeout;

		// Token: 0x04002CCD RID: 11469
		private const string DefaultMatchTimeout_ConfigKeyName = "REGEX_DEFAULT_MATCH_TIMEOUT";

		// Token: 0x04002CCE RID: 11470
		[NonSerialized]
		internal static readonly TimeSpan FallbackDefaultMatchTimeout = Regex.InfiniteMatchTimeout;

		// Token: 0x04002CCF RID: 11471
		[NonSerialized]
		internal static readonly TimeSpan DefaultMatchTimeout = Regex.InitDefaultMatchTimeout();

		// Token: 0x04002CD0 RID: 11472
		protected internal Hashtable caps;

		// Token: 0x04002CD1 RID: 11473
		protected internal Hashtable capnames;

		// Token: 0x04002CD2 RID: 11474
		protected internal string[] capslist;

		// Token: 0x04002CD3 RID: 11475
		protected internal int capsize;

		// Token: 0x04002CD4 RID: 11476
		internal ExclusiveReference runnerref;

		// Token: 0x04002CD5 RID: 11477
		internal SharedReference replref;

		// Token: 0x04002CD6 RID: 11478
		internal RegexCode code;

		// Token: 0x04002CD7 RID: 11479
		internal bool refsInitialized;

		// Token: 0x04002CD8 RID: 11480
		internal static LinkedList<CachedCodeEntry> livecode = new LinkedList<CachedCodeEntry>();

		// Token: 0x04002CD9 RID: 11481
		internal static int cacheSize = 15;

		// Token: 0x04002CDA RID: 11482
		internal const int MaxOptionShift = 10;
	}
}
