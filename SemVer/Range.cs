using System;
using System.Collections.Generic;
using System.Linq;

namespace SemVer
{
	// Token: 0x02000007 RID: 7
	public class Range : IEquatable<Range>
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000031A0 File Offset: 0x000013A0
		public Range(string rangeSpec, bool loose = false)
		{
			this._rangeSpec = rangeSpec;
			string[] array = rangeSpec.Split(new string[] { "||" }, StringSplitOptions.None);
			this._comparatorSets = array.Select((string s) => new ComparatorSet(s)).ToArray<ComparatorSet>();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003200 File Offset: 0x00001400
		private Range(IEnumerable<ComparatorSet> comparatorSets)
		{
			this._comparatorSets = comparatorSets.ToArray<ComparatorSet>();
			this._rangeSpec = string.Join(" || ", this._comparatorSets.Select((ComparatorSet cs) => cs.ToString()).ToArray<string>());
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003260 File Offset: 0x00001460
		public bool IsSatisfied(Version version)
		{
			return this._comparatorSets.Any((ComparatorSet s) => s.IsSatisfied(version));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003294 File Offset: 0x00001494
		public bool IsSatisfied(string versionString, bool loose = false)
		{
			bool flag;
			try
			{
				Version version = new Version(versionString, loose);
				flag = this.IsSatisfied(version);
			}
			catch (ArgumentException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000032CC File Offset: 0x000014CC
		public IEnumerable<Version> Satisfying(IEnumerable<Version> versions)
		{
			return versions.Where(new Func<Version, bool>(this.IsSatisfied));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000032E0 File Offset: 0x000014E0
		public IEnumerable<string> Satisfying(IEnumerable<string> versions, bool loose = false)
		{
			return versions.Where((string v) => this.IsSatisfied(v, loose));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003313 File Offset: 0x00001513
		public Version MaxSatisfying(IEnumerable<Version> versions)
		{
			return this.Satisfying(versions).Max<Version>();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003324 File Offset: 0x00001524
		public string MaxSatisfying(IEnumerable<string> versionStrings, bool loose = false)
		{
			IEnumerable<Version> enumerable = this.ValidVersions(versionStrings, loose);
			Version version = this.MaxSatisfying(enumerable);
			if (!(version == null))
			{
				return version.ToString();
			}
			return null;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003354 File Offset: 0x00001554
		public Range Intersect(Range other)
		{
			List<ComparatorSet> list = (from cs in this._comparatorSets.SelectMany((ComparatorSet thisCs) => other._comparatorSets.Select(new Func<ComparatorSet, ComparatorSet>(thisCs.Intersect)))
				where cs != null
				select cs).ToList<ComparatorSet>();
			if (list.Count == 0)
			{
				return new Range("<0.0.0", false);
			}
			return new Range(list);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000033C9 File Offset: 0x000015C9
		public override string ToString()
		{
			return this._rangeSpec;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000033D1 File Offset: 0x000015D1
		public bool Equals(Range other)
		{
			return other != null && new HashSet<ComparatorSet>(this._comparatorSets).SetEquals(other._comparatorSets);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000033EE File Offset: 0x000015EE
		public override bool Equals(object other)
		{
			return this.Equals(other as Range);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000033FC File Offset: 0x000015FC
		public static bool operator ==(Range a, Range b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000340D File Offset: 0x0000160D
		public static bool operator !=(Range a, Range b)
		{
			return !(a == b);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003419 File Offset: 0x00001619
		public override int GetHashCode()
		{
			return this._comparatorSets.Aggregate(0, (int accum, ComparatorSet next) => accum ^ next.GetHashCode());
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003446 File Offset: 0x00001646
		public static bool IsSatisfied(string rangeSpec, string versionString, bool loose = false)
		{
			return new Range(rangeSpec, false).IsSatisfied(versionString, false);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003456 File Offset: 0x00001656
		public static IEnumerable<string> Satisfying(string rangeSpec, IEnumerable<string> versions, bool loose = false)
		{
			return new Range(rangeSpec, false).Satisfying(versions, false);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003466 File Offset: 0x00001666
		public static string MaxSatisfying(string rangeSpec, IEnumerable<string> versionStrings, bool loose = false)
		{
			return new Range(rangeSpec, false).MaxSatisfying(versionStrings, false);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003476 File Offset: 0x00001676
		private IEnumerable<Version> ValidVersions(IEnumerable<string> versionStrings, bool loose)
		{
			foreach (string text in versionStrings)
			{
				Version version = null;
				try
				{
					version = new Version(text, loose);
				}
				catch (ArgumentException)
				{
				}
				if (version != null)
				{
					yield return version;
				}
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400000B RID: 11
		private readonly ComparatorSet[] _comparatorSets;

		// Token: 0x0400000C RID: 12
		private readonly string _rangeSpec;
	}
}
