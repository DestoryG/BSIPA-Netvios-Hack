using System;
using System.Collections.Generic;
using System.Linq;

namespace SemVer
{
	// Token: 0x02000003 RID: 3
	internal class ComparatorSet : IEquatable<ComparatorSet>
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002550 File Offset: 0x00000750
		public ComparatorSet(string spec)
		{
			this._comparators = new List<Comparator>();
			spec = spec.Trim();
			if (spec == "")
			{
				spec = "*";
			}
			int i = 0;
			int length = spec.Length;
			while (i < length)
			{
				int num = i;
				Func<string, Tuple<int, Comparator[]>>[] array = new Func<string, Tuple<int, Comparator[]>>[]
				{
					new Func<string, Tuple<int, Comparator[]>>(Desugarer.HyphenRange),
					new Func<string, Tuple<int, Comparator[]>>(Desugarer.TildeRange),
					new Func<string, Tuple<int, Comparator[]>>(Desugarer.CaretRange),
					new Func<string, Tuple<int, Comparator[]>>(Desugarer.StarRange)
				};
				for (int j = 0; j < array.Length; j++)
				{
					Tuple<int, Comparator[]> tuple = array[j](spec.Substring(i));
					if (tuple != null)
					{
						i += tuple.Item1;
						this._comparators.AddRange(tuple.Item2);
					}
				}
				Tuple<int, Comparator> tuple2 = Comparator.TryParse(spec.Substring(i));
				if (tuple2 != null)
				{
					i += tuple2.Item1;
					this._comparators.Add(tuple2.Item2);
				}
				if (i == num)
				{
					throw new ArgumentException(string.Format("Invalid range specification: \"{0}\"", spec));
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000266A File Offset: 0x0000086A
		private ComparatorSet(IEnumerable<Comparator> comparators)
		{
			this._comparators = comparators.ToList<Comparator>();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002680 File Offset: 0x00000880
		public bool IsSatisfied(Version version)
		{
			bool flag = this._comparators.All((Comparator c) => c.IsSatisfied(version));
			if (version.PreRelease != null)
			{
				return flag && this._comparators.Any((Comparator c) => c.Version.PreRelease != null && c.Version.BaseVersion() == version.BaseVersion());
			}
			return flag;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026E0 File Offset: 0x000008E0
		public ComparatorSet Intersect(ComparatorSet other)
		{
			Func<Comparator, bool> func = (Comparator c) => c.ComparatorType == Comparator.Operator.GreaterThan || c.ComparatorType == Comparator.Operator.GreaterThanOrEqual;
			Func<Comparator, bool> func2 = (Comparator c) => c.ComparatorType == Comparator.Operator.LessThan || c.ComparatorType == Comparator.Operator.LessThanOrEqual;
			Comparator comparator = (from c in this._comparators.Concat(other._comparators).Where(func)
				orderby c.Version descending
				select c).FirstOrDefault<Comparator>();
			Comparator comparator2 = (from c in this._comparators.Concat(other._comparators).Where(func2)
				orderby c.Version
				select c).FirstOrDefault<Comparator>();
			if (comparator != null && comparator2 != null && !comparator.Intersects(comparator2))
			{
				return null;
			}
			List<Version> equalityVersions = (from c in this._comparators.Concat(other._comparators)
				where c.ComparatorType == Comparator.Operator.Equal
				select c.Version).ToList<Version>();
			if (equalityVersions.Count > 1 && equalityVersions.Any((Version v) => v != equalityVersions[0]))
			{
				return null;
			}
			if (equalityVersions.Count > 0)
			{
				if (comparator != null && !comparator.IsSatisfied(equalityVersions[0]))
				{
					return null;
				}
				if (comparator2 != null && !comparator2.IsSatisfied(equalityVersions[0]))
				{
					return null;
				}
				return new ComparatorSet(new List<Comparator>
				{
					new Comparator(Comparator.Operator.Equal, equalityVersions[0])
				});
			}
			else
			{
				List<Comparator> list = new List<Comparator>();
				if (comparator != null)
				{
					list.Add(comparator);
				}
				if (comparator2 != null)
				{
					list.Add(comparator2);
				}
				if (list.Count <= 0)
				{
					return null;
				}
				return new ComparatorSet(list);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028ED File Offset: 0x00000AED
		public bool Equals(ComparatorSet other)
		{
			return other != null && new HashSet<Comparator>(this._comparators).SetEquals(other._comparators);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000290A File Offset: 0x00000B0A
		public override bool Equals(object other)
		{
			return this.Equals(other as ComparatorSet);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002918 File Offset: 0x00000B18
		public override string ToString()
		{
			return string.Join(" ", this._comparators.Select((Comparator c) => c.ToString()).ToArray<string>());
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002953 File Offset: 0x00000B53
		public override int GetHashCode()
		{
			return this._comparators.Aggregate(0, (int accum, Comparator next) => accum ^ next.GetHashCode());
		}

		// Token: 0x04000004 RID: 4
		private readonly List<Comparator> _comparators;
	}
}
