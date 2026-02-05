using System;
using System.Text.RegularExpressions;

namespace SemVer
{
	// Token: 0x02000002 RID: 2
	internal class Comparator : IEquatable<Comparator>
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Comparator(string input)
		{
			Match match = new Regex(string.Format("^{0}$", "\n            \\s*\n            ([=<>]*)                # Comparator type (can be empty)\n            \\s*\n            ([0-9a-zA-Z\\-\\+\\.\\*]+)  # Version (potentially partial version)\n            \\s*\n            "), RegexOptions.IgnorePatternWhitespace).Match(input);
			if (!match.Success)
			{
				throw new ArgumentException(string.Format("Invalid comparator string: {0}", input));
			}
			this.ComparatorType = Comparator.ParseComparatorType(match.Groups[1].Value);
			PartialVersion partialVersion = new PartialVersion(match.Groups[2].Value);
			if (partialVersion.IsFull())
			{
				this.Version = partialVersion.ToZeroVersion();
				return;
			}
			Comparator.Operator comparatorType = this.ComparatorType;
			if (comparatorType != Comparator.Operator.LessThanOrEqual)
			{
				if (comparatorType != Comparator.Operator.GreaterThan)
				{
					this.Version = partialVersion.ToZeroVersion();
					return;
				}
				this.ComparatorType = Comparator.Operator.GreaterThanOrEqual;
				if (partialVersion.Major == null)
				{
					this.ComparatorType = Comparator.Operator.LessThan;
					this.Version = new Version(0, 0, 0, null, null);
					return;
				}
				if (partialVersion.Minor == null)
				{
					this.Version = new Version(partialVersion.Major.Value + 1, 0, 0, null, null);
					return;
				}
				this.Version = new Version(partialVersion.Major.Value, partialVersion.Minor.Value + 1, 0, null, null);
				return;
			}
			else
			{
				this.ComparatorType = Comparator.Operator.LessThan;
				if (partialVersion.Major == null)
				{
					this.ComparatorType = Comparator.Operator.GreaterThanOrEqual;
					this.Version = new Version(0, 0, 0, null, null);
					return;
				}
				if (partialVersion.Minor == null)
				{
					this.Version = new Version(partialVersion.Major.Value + 1, 0, 0, null, null);
					return;
				}
				this.Version = new Version(partialVersion.Major.Value, partialVersion.Minor.Value + 1, 0, null, null);
				return;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000221F File Offset: 0x0000041F
		public Comparator(Comparator.Operator comparatorType, Version comparatorVersion)
		{
			if (comparatorVersion == null)
			{
				throw new NullReferenceException("Null comparator version");
			}
			this.ComparatorType = comparatorType;
			this.Version = comparatorVersion;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000224C File Offset: 0x0000044C
		public static Tuple<int, Comparator> TryParse(string input)
		{
			Match match = new Regex(string.Format("^{0}", "\n            \\s*\n            ([=<>]*)                # Comparator type (can be empty)\n            \\s*\n            ([0-9a-zA-Z\\-\\+\\.\\*]+)  # Version (potentially partial version)\n            \\s*\n            "), RegexOptions.IgnorePatternWhitespace).Match(input);
			if (!match.Success)
			{
				return null;
			}
			return Tuple.Create<int, Comparator>(match.Length, new Comparator(match.Value));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002298 File Offset: 0x00000498
		private static Comparator.Operator ParseComparatorType(string input)
		{
			if (input != null)
			{
				if ((input != null && input.Length == 0) || input == "=")
				{
					return Comparator.Operator.Equal;
				}
				if (input == "<")
				{
					return Comparator.Operator.LessThan;
				}
				if (input == "<=")
				{
					return Comparator.Operator.LessThanOrEqual;
				}
				if (input == ">")
				{
					return Comparator.Operator.GreaterThan;
				}
				if (input == ">=")
				{
					return Comparator.Operator.GreaterThanOrEqual;
				}
			}
			throw new ArgumentException(string.Format("Invalid comparator type: {0}", input));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002310 File Offset: 0x00000510
		public bool IsSatisfied(Version version)
		{
			switch (this.ComparatorType)
			{
			case Comparator.Operator.Equal:
				return version == this.Version;
			case Comparator.Operator.LessThan:
				return version < this.Version;
			case Comparator.Operator.LessThanOrEqual:
				return version <= this.Version;
			case Comparator.Operator.GreaterThan:
				return version > this.Version;
			case Comparator.Operator.GreaterThanOrEqual:
				return version >= this.Version;
			default:
				throw new InvalidOperationException("Comparator type not recognised.");
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000238C File Offset: 0x0000058C
		public bool Intersects(Comparator other)
		{
			Func<Comparator, bool> func = (Comparator c) => c.ComparatorType == Comparator.Operator.GreaterThan || c.ComparatorType == Comparator.Operator.GreaterThanOrEqual;
			Func<Comparator, bool> func2 = (Comparator c) => c.ComparatorType == Comparator.Operator.LessThan || c.ComparatorType == Comparator.Operator.LessThanOrEqual;
			Func<Comparator, bool> func3 = (Comparator c) => c.ComparatorType == Comparator.Operator.GreaterThanOrEqual || c.ComparatorType == Comparator.Operator.Equal || c.ComparatorType == Comparator.Operator.LessThanOrEqual;
			return (this.Version > other.Version && (func2(this) || func(other))) || (this.Version < other.Version && (func(this) || func2(other))) || (this.Version == other.Version && ((func3(this) && func3(other)) || (func2(this) && func2(other)) || (func(this) && func(other))));
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002494 File Offset: 0x00000694
		public override string ToString()
		{
			string text;
			switch (this.ComparatorType)
			{
			case Comparator.Operator.Equal:
				text = "=";
				break;
			case Comparator.Operator.LessThan:
				text = "<";
				break;
			case Comparator.Operator.LessThanOrEqual:
				text = "<=";
				break;
			case Comparator.Operator.GreaterThan:
				text = ">";
				break;
			case Comparator.Operator.GreaterThanOrEqual:
				text = ">=";
				break;
			default:
				throw new InvalidOperationException("Comparator type not recognised.");
			}
			return string.Format("{0}{1}", text, this.Version);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000250A File Offset: 0x0000070A
		public bool Equals(Comparator other)
		{
			return other != null && this.ComparatorType == other.ComparatorType && this.Version == other.Version;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002532 File Offset: 0x00000732
		public override bool Equals(object other)
		{
			return this.Equals(other as Comparator);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002540 File Offset: 0x00000740
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x04000001 RID: 1
		public readonly Comparator.Operator ComparatorType;

		// Token: 0x04000002 RID: 2
		public readonly Version Version;

		// Token: 0x04000003 RID: 3
		private const string pattern = "\n            \\s*\n            ([=<>]*)                # Comparator type (can be empty)\n            \\s*\n            ([0-9a-zA-Z\\-\\+\\.\\*]+)  # Version (potentially partial version)\n            \\s*\n            ";

		// Token: 0x02000009 RID: 9
		public enum Operator
		{
			// Token: 0x04000016 RID: 22
			Equal,
			// Token: 0x04000017 RID: 23
			LessThan,
			// Token: 0x04000018 RID: 24
			LessThanOrEqual,
			// Token: 0x04000019 RID: 25
			GreaterThan,
			// Token: 0x0400001A RID: 26
			GreaterThanOrEqual
		}
	}
}
