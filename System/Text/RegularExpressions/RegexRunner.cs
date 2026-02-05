using System;
using System.ComponentModel;

namespace System.Text.RegularExpressions
{
	// Token: 0x020006A5 RID: 1701
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class RegexRunner
	{
		// Token: 0x06003F99 RID: 16281 RVA: 0x0010B50C File Offset: 0x0010970C
		protected internal RegexRunner()
		{
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0010B514 File Offset: 0x00109714
		protected internal Match Scan(Regex regex, string text, int textbeg, int textend, int textstart, int prevlen, bool quick)
		{
			return this.Scan(regex, text, textbeg, textend, textstart, prevlen, quick, regex.MatchTimeout);
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0010B538 File Offset: 0x00109738
		protected internal Match Scan(Regex regex, string text, int textbeg, int textend, int textstart, int prevlen, bool quick, TimeSpan timeout)
		{
			bool flag = false;
			Regex.ValidateMatchTimeout(timeout);
			this.ignoreTimeout = Regex.InfiniteMatchTimeout == timeout;
			this.timeout = (this.ignoreTimeout ? ((int)Regex.InfiniteMatchTimeout.TotalMilliseconds) : ((int)(timeout.TotalMilliseconds + 0.5)));
			this.runregex = regex;
			this.runtext = text;
			this.runtextbeg = textbeg;
			this.runtextend = textend;
			this.runtextstart = textstart;
			int num = (this.runregex.RightToLeft ? (-1) : 1);
			int num2 = (this.runregex.RightToLeft ? this.runtextbeg : this.runtextend);
			this.runtextpos = textstart;
			if (prevlen == 0)
			{
				if (this.runtextpos == num2)
				{
					return Match.Empty;
				}
				this.runtextpos += num;
			}
			this.StartTimeoutWatch();
			for (;;)
			{
				if (this.FindFirstChar())
				{
					this.CheckTimeout();
					if (!flag)
					{
						this.InitMatch();
						flag = true;
					}
					this.Go();
					if (this.runmatch._matchcount[0] > 0)
					{
						break;
					}
					this.runtrackpos = this.runtrack.Length;
					this.runstackpos = this.runstack.Length;
					this.runcrawlpos = this.runcrawl.Length;
				}
				if (this.runtextpos == num2)
				{
					goto Block_9;
				}
				this.runtextpos += num;
			}
			return this.TidyMatch(quick);
			Block_9:
			this.TidyMatch(true);
			return Match.Empty;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0010B69D File Offset: 0x0010989D
		private void StartTimeoutWatch()
		{
			if (this.ignoreTimeout)
			{
				return;
			}
			this.timeoutChecksToSkip = 1000;
			this.timeoutOccursAt = Environment.TickCount + this.timeout;
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0010B6C8 File Offset: 0x001098C8
		protected void CheckTimeout()
		{
			if (this.ignoreTimeout)
			{
				return;
			}
			int num = this.timeoutChecksToSkip - 1;
			this.timeoutChecksToSkip = num;
			if (num != 0)
			{
				return;
			}
			this.timeoutChecksToSkip = 1000;
			this.DoCheckTimeout();
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x0010B704 File Offset: 0x00109904
		private void DoCheckTimeout()
		{
			int tickCount = Environment.TickCount;
			if (tickCount < this.timeoutOccursAt)
			{
				return;
			}
			if (0 > this.timeoutOccursAt && 0 < tickCount)
			{
				return;
			}
			throw new RegexMatchTimeoutException(this.runtext, this.runregex.pattern, TimeSpan.FromMilliseconds((double)this.timeout));
		}

		// Token: 0x06003F9F RID: 16287
		protected abstract void Go();

		// Token: 0x06003FA0 RID: 16288
		protected abstract bool FindFirstChar();

		// Token: 0x06003FA1 RID: 16289
		protected abstract void InitTrackCount();

		// Token: 0x06003FA2 RID: 16290 RVA: 0x0010B754 File Offset: 0x00109954
		private void InitMatch()
		{
			if (this.runmatch == null)
			{
				if (this.runregex.caps != null)
				{
					this.runmatch = new MatchSparse(this.runregex, this.runregex.caps, this.runregex.capsize, this.runtext, this.runtextbeg, this.runtextend - this.runtextbeg, this.runtextstart);
				}
				else
				{
					this.runmatch = new Match(this.runregex, this.runregex.capsize, this.runtext, this.runtextbeg, this.runtextend - this.runtextbeg, this.runtextstart);
				}
			}
			else
			{
				this.runmatch.Reset(this.runregex, this.runtext, this.runtextbeg, this.runtextend, this.runtextstart);
			}
			if (this.runcrawl != null)
			{
				this.runtrackpos = this.runtrack.Length;
				this.runstackpos = this.runstack.Length;
				this.runcrawlpos = this.runcrawl.Length;
				return;
			}
			this.InitTrackCount();
			int num = this.runtrackcount * 8;
			int num2 = this.runtrackcount * 8;
			if (num < 32)
			{
				num = 32;
			}
			if (num2 < 16)
			{
				num2 = 16;
			}
			this.runtrack = new int[num];
			this.runtrackpos = num;
			this.runstack = new int[num2];
			this.runstackpos = num2;
			this.runcrawl = new int[32];
			this.runcrawlpos = 32;
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x0010B8C0 File Offset: 0x00109AC0
		private Match TidyMatch(bool quick)
		{
			if (!quick)
			{
				Match match = this.runmatch;
				this.runmatch = null;
				match.Tidy(this.runtextpos);
				return match;
			}
			return null;
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0010B8ED File Offset: 0x00109AED
		protected void EnsureStorage()
		{
			if (this.runstackpos < this.runtrackcount * 4)
			{
				this.DoubleStack();
			}
			if (this.runtrackpos < this.runtrackcount * 4)
			{
				this.DoubleTrack();
			}
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0010B91B File Offset: 0x00109B1B
		protected bool IsBoundary(int index, int startpos, int endpos)
		{
			return (index > startpos && RegexCharClass.IsWordChar(this.runtext[index - 1])) != (index < endpos && RegexCharClass.IsWordChar(this.runtext[index]));
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0010B954 File Offset: 0x00109B54
		protected bool IsECMABoundary(int index, int startpos, int endpos)
		{
			return (index > startpos && RegexCharClass.IsECMAWordChar(this.runtext[index - 1])) != (index < endpos && RegexCharClass.IsECMAWordChar(this.runtext[index]));
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0010B990 File Offset: 0x00109B90
		protected static bool CharInSet(char ch, string set, string category)
		{
			string text = RegexCharClass.ConvertOldStringsToClass(set, category);
			return RegexCharClass.CharInClass(ch, text);
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0010B9AC File Offset: 0x00109BAC
		protected static bool CharInClass(char ch, string charClass)
		{
			return RegexCharClass.CharInClass(ch, charClass);
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0010B9B8 File Offset: 0x00109BB8
		protected void DoubleTrack()
		{
			int[] array = new int[this.runtrack.Length * 2];
			Array.Copy(this.runtrack, 0, array, this.runtrack.Length, this.runtrack.Length);
			this.runtrackpos += this.runtrack.Length;
			this.runtrack = array;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x0010BA10 File Offset: 0x00109C10
		protected void DoubleStack()
		{
			int[] array = new int[this.runstack.Length * 2];
			Array.Copy(this.runstack, 0, array, this.runstack.Length, this.runstack.Length);
			this.runstackpos += this.runstack.Length;
			this.runstack = array;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0010BA68 File Offset: 0x00109C68
		protected void DoubleCrawl()
		{
			int[] array = new int[this.runcrawl.Length * 2];
			Array.Copy(this.runcrawl, 0, array, this.runcrawl.Length, this.runcrawl.Length);
			this.runcrawlpos += this.runcrawl.Length;
			this.runcrawl = array;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0010BAC0 File Offset: 0x00109CC0
		protected void Crawl(int i)
		{
			if (this.runcrawlpos == 0)
			{
				this.DoubleCrawl();
			}
			int[] array = this.runcrawl;
			int num = this.runcrawlpos - 1;
			this.runcrawlpos = num;
			array[num] = i;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0010BAF4 File Offset: 0x00109CF4
		protected int Popcrawl()
		{
			int[] array = this.runcrawl;
			int num = this.runcrawlpos;
			this.runcrawlpos = num + 1;
			return array[num];
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0010BB19 File Offset: 0x00109D19
		protected int Crawlpos()
		{
			return this.runcrawl.Length - this.runcrawlpos;
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0010BB2C File Offset: 0x00109D2C
		protected void Capture(int capnum, int start, int end)
		{
			if (end < start)
			{
				int num = end;
				end = start;
				start = num;
			}
			this.Crawl(capnum);
			this.runmatch.AddMatch(capnum, start, end - start);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0010BB5C File Offset: 0x00109D5C
		protected void TransferCapture(int capnum, int uncapnum, int start, int end)
		{
			if (end < start)
			{
				int num = end;
				end = start;
				start = num;
			}
			int num2 = this.MatchIndex(uncapnum);
			int num3 = num2 + this.MatchLength(uncapnum);
			if (start >= num3)
			{
				end = start;
				start = num3;
			}
			else if (end <= num2)
			{
				start = num2;
			}
			else
			{
				if (end > num3)
				{
					end = num3;
				}
				if (num2 > start)
				{
					start = num2;
				}
			}
			this.Crawl(uncapnum);
			this.runmatch.BalanceMatch(uncapnum);
			if (capnum != -1)
			{
				this.Crawl(capnum);
				this.runmatch.AddMatch(capnum, start, end - start);
			}
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x0010BBE0 File Offset: 0x00109DE0
		protected void Uncapture()
		{
			int num = this.Popcrawl();
			this.runmatch.RemoveMatch(num);
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0010BC00 File Offset: 0x00109E00
		protected bool IsMatched(int cap)
		{
			return this.runmatch.IsMatched(cap);
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x0010BC0E File Offset: 0x00109E0E
		protected int MatchIndex(int cap)
		{
			return this.runmatch.MatchIndex(cap);
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0010BC1C File Offset: 0x00109E1C
		protected int MatchLength(int cap)
		{
			return this.runmatch.MatchLength(cap);
		}

		// Token: 0x04002E59 RID: 11865
		protected internal int runtextbeg;

		// Token: 0x04002E5A RID: 11866
		protected internal int runtextend;

		// Token: 0x04002E5B RID: 11867
		protected internal int runtextstart;

		// Token: 0x04002E5C RID: 11868
		protected internal string runtext;

		// Token: 0x04002E5D RID: 11869
		protected internal int runtextpos;

		// Token: 0x04002E5E RID: 11870
		protected internal int[] runtrack;

		// Token: 0x04002E5F RID: 11871
		protected internal int runtrackpos;

		// Token: 0x04002E60 RID: 11872
		protected internal int[] runstack;

		// Token: 0x04002E61 RID: 11873
		protected internal int runstackpos;

		// Token: 0x04002E62 RID: 11874
		protected internal int[] runcrawl;

		// Token: 0x04002E63 RID: 11875
		protected internal int runcrawlpos;

		// Token: 0x04002E64 RID: 11876
		protected internal int runtrackcount;

		// Token: 0x04002E65 RID: 11877
		protected internal Match runmatch;

		// Token: 0x04002E66 RID: 11878
		protected internal Regex runregex;

		// Token: 0x04002E67 RID: 11879
		private int timeout;

		// Token: 0x04002E68 RID: 11880
		private bool ignoreTimeout;

		// Token: 0x04002E69 RID: 11881
		private int timeoutOccursAt;

		// Token: 0x04002E6A RID: 11882
		private const int TimeoutCheckFrequency = 1000;

		// Token: 0x04002E6B RID: 11883
		private int timeoutChecksToSkip;
	}
}
