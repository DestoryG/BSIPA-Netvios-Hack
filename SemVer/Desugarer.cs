using System;
using System.Text.RegularExpressions;

namespace SemVer
{
	// Token: 0x02000004 RID: 4
	internal static class Desugarer
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002980 File Offset: 0x00000B80
		public static Tuple<int, Comparator[]> TildeRange(string spec)
		{
			Match match = new Regex(string.Format("^\\s*~\\s*({0}+)\\s*", "[0-9a-zA-Z\\-\\+\\.\\*]")).Match(spec);
			if (!match.Success)
			{
				return null;
			}
			PartialVersion partialVersion = new PartialVersion(match.Groups[1].Value);
			Version version;
			Version version2;
			if (partialVersion.Minor != null)
			{
				version = partialVersion.ToZeroVersion();
				version2 = new Version(partialVersion.Major.Value, partialVersion.Minor.Value + 1, 0, null, null);
			}
			else
			{
				version = partialVersion.ToZeroVersion();
				version2 = new Version(partialVersion.Major.Value + 1, 0, 0, null, null);
			}
			return Tuple.Create<int, Comparator[]>(match.Length, Desugarer.minMaxComparators(version, version2, Comparator.Operator.LessThan));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002A48 File Offset: 0x00000C48
		public static Tuple<int, Comparator[]> CaretRange(string spec)
		{
			Match match = new Regex(string.Format("^\\s*\\^\\s*({0}+)\\s*", "[0-9a-zA-Z\\-\\+\\.\\*]")).Match(spec);
			if (!match.Success)
			{
				return null;
			}
			PartialVersion partialVersion = new PartialVersion(match.Groups[1].Value);
			Version version;
			Version version2;
			if (partialVersion.Major.Value > 0)
			{
				version = partialVersion.ToZeroVersion();
				version2 = new Version(partialVersion.Major.Value + 1, 0, 0, null, null);
			}
			else if (partialVersion.Minor == null)
			{
				version = partialVersion.ToZeroVersion();
				version2 = new Version(partialVersion.Major.Value + 1, 0, 0, null, null);
			}
			else if (partialVersion.Patch == null)
			{
				version = partialVersion.ToZeroVersion();
				version2 = new Version(0, partialVersion.Minor.Value + 1, 0, null, null);
			}
			else
			{
				int? minor = partialVersion.Minor;
				int num = 0;
				if ((minor.GetValueOrDefault() > num) & (minor != null))
				{
					version = partialVersion.ToZeroVersion();
					version2 = new Version(0, partialVersion.Minor.Value + 1, 0, null, null);
				}
				else
				{
					version = partialVersion.ToZeroVersion();
					version2 = new Version(0, 0, partialVersion.Patch.Value + 1, null, null);
				}
			}
			return Tuple.Create<int, Comparator[]>(match.Length, Desugarer.minMaxComparators(version, version2, Comparator.Operator.LessThan));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public static Tuple<int, Comparator[]> HyphenRange(string spec)
		{
			Match match = new Regex(string.Format("^\\s*({0}+)\\s+\\-\\s+({0}+)\\s*", "[0-9a-zA-Z\\-\\+\\.\\*]")).Match(spec);
			if (!match.Success)
			{
				return null;
			}
			PartialVersion partialVersion = null;
			PartialVersion partialVersion2 = null;
			try
			{
				partialVersion = new PartialVersion(match.Groups[1].Value);
				partialVersion2 = new PartialVersion(match.Groups[2].Value);
			}
			catch (ArgumentException)
			{
				return null;
			}
			Version version = partialVersion.ToZeroVersion();
			Comparator.Operator @operator = (partialVersion2.IsFull() ? Comparator.Operator.LessThanOrEqual : Comparator.Operator.LessThan);
			Version version2 = null;
			if (partialVersion2.Major != null)
			{
				if (partialVersion2.Minor == null)
				{
					version2 = new Version(partialVersion2.Major.Value + 1, 0, 0, null, null);
				}
				else if (partialVersion2.Patch == null)
				{
					version2 = new Version(partialVersion2.Major.Value, partialVersion2.Minor.Value + 1, 0, null, null);
				}
				else
				{
					version2 = partialVersion2.ToZeroVersion();
				}
			}
			return Tuple.Create<int, Comparator[]>(match.Length, Desugarer.minMaxComparators(version, version2, @operator));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002CE8 File Offset: 0x00000EE8
		public static Tuple<int, Comparator[]> StarRange(string spec)
		{
			Match match = new Regex(string.Format("^\\s*=?\\s*({0}+)\\s*", "[0-9a-zA-Z\\-\\+\\.\\*]")).Match(spec);
			if (!match.Success)
			{
				return null;
			}
			PartialVersion partialVersion = null;
			try
			{
				partialVersion = new PartialVersion(match.Groups[1].Value);
			}
			catch (ArgumentException)
			{
				return null;
			}
			if (partialVersion.IsFull())
			{
				return null;
			}
			Version version = null;
			Version version2;
			if (partialVersion.Major == null)
			{
				version2 = partialVersion.ToZeroVersion();
			}
			else if (partialVersion.Minor == null)
			{
				version2 = partialVersion.ToZeroVersion();
				version = new Version(partialVersion.Major.Value + 1, 0, 0, null, null);
			}
			else
			{
				version2 = partialVersion.ToZeroVersion();
				version = new Version(partialVersion.Major.Value, partialVersion.Minor.Value + 1, 0, null, null);
			}
			return Tuple.Create<int, Comparator[]>(match.Length, Desugarer.minMaxComparators(version2, version, Comparator.Operator.LessThan));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002DF4 File Offset: 0x00000FF4
		private static Comparator[] minMaxComparators(Version minVersion, Version maxVersion, Comparator.Operator maxOperator = Comparator.Operator.LessThan)
		{
			Comparator comparator = new Comparator(Comparator.Operator.GreaterThanOrEqual, minVersion);
			if (maxVersion == null)
			{
				return new Comparator[] { comparator };
			}
			Comparator comparator2 = new Comparator(maxOperator, maxVersion);
			return new Comparator[] { comparator, comparator2 };
		}

		// Token: 0x04000005 RID: 5
		private const string versionChars = "[0-9a-zA-Z\\-\\+\\.\\*]";
	}
}
