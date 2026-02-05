using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SemVer
{
	// Token: 0x02000008 RID: 8
	public class Version : IComparable<Version>, IComparable, IEquatable<Version>
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000348D File Offset: 0x0000168D
		public int Major
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003495 File Offset: 0x00001695
		public int Minor
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000349D File Offset: 0x0000169D
		public int Patch
		{
			get
			{
				return this._patch;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000034A5 File Offset: 0x000016A5
		public string PreRelease
		{
			get
			{
				return this._preRelease;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000034AD File Offset: 0x000016AD
		public string Build
		{
			get
			{
				return this._build;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000034B8 File Offset: 0x000016B8
		public Version(string input, bool loose = false)
		{
			this._inputString = input;
			Match match = (loose ? Version.looseRegex : Version.strictRegex).Match(input);
			if (!match.Success)
			{
				throw new ArgumentException(string.Format("Invalid version string: {0}", input));
			}
			this._major = int.Parse(match.Groups[1].Value);
			this._minor = int.Parse(match.Groups[2].Value);
			this._patch = int.Parse(match.Groups[3].Value);
			if (match.Groups[4].Success)
			{
				string value = match.Groups[5].Value;
				string text = PreReleaseVersion.Clean(value);
				if (!loose && value != text)
				{
					throw new ArgumentException(string.Format("Invalid pre-release version: {0}", value));
				}
				this._preRelease = text;
			}
			if (match.Groups[6].Success)
			{
				this._build = match.Groups[7].Value;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000035CF File Offset: 0x000017CF
		public Version(int major, int minor, int patch, string preRelease = null, string build = null)
		{
			this._major = major;
			this._minor = minor;
			this._patch = patch;
			this._preRelease = preRelease;
			this._build = build;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000035FC File Offset: 0x000017FC
		public Version BaseVersion()
		{
			return new Version(this.Major, this.Minor, this.Patch, null, null);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003617 File Offset: 0x00001817
		public override string ToString()
		{
			return this._inputString ?? this.Clean();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000362C File Offset: 0x0000182C
		public string Clean()
		{
			string text = ((this.PreRelease == null) ? "" : string.Format("-{0}", PreReleaseVersion.Clean(this.PreRelease)));
			string text2 = ((this.Build == null) ? "" : string.Format("+{0}", this.Build));
			return string.Format("{0}.{1}.{2}{3}{4}", new object[] { this.Major, this.Minor, this.Patch, text, text2 });
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000036C0 File Offset: 0x000018C0
		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.Major.GetHashCode();
			num = num * 23 + this.Minor.GetHashCode();
			num = num * 23 + this.Patch.GetHashCode();
			if (this.PreRelease != null)
			{
				num = num * 23 + this.PreRelease.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003726 File Offset: 0x00001926
		public bool Equals(Version other)
		{
			return other != null && this.CompareTo(other) == 0;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003738 File Offset: 0x00001938
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			Version version = obj as Version;
			if (version == null)
			{
				throw new ArgumentException("Object is not a Version");
			}
			return this.CompareTo(version);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003768 File Offset: 0x00001968
		public int CompareTo(Version other)
		{
			if (other == null)
			{
				return 1;
			}
			foreach (int num in this.PartComparisons(other))
			{
				if (num != 0)
				{
					return num;
				}
			}
			return PreReleaseVersion.Compare(this.PreRelease, other.PreRelease);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000037D0 File Offset: 0x000019D0
		private IEnumerable<int> PartComparisons(Version other)
		{
			yield return this.Major.CompareTo(other.Major);
			yield return this.Minor.CompareTo(other.Minor);
			yield return this.Patch.CompareTo(other.Patch);
			yield break;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000037E7 File Offset: 0x000019E7
		public override bool Equals(object other)
		{
			return this.Equals(other as Version);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000037F5 File Offset: 0x000019F5
		public static bool operator ==(Version a, Version b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003806 File Offset: 0x00001A06
		public static bool operator !=(Version a, Version b)
		{
			return !(a == b);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003812 File Offset: 0x00001A12
		public static bool operator >(Version a, Version b)
		{
			return a != null && a.CompareTo(b) > 0;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003823 File Offset: 0x00001A23
		public static bool operator >=(Version a, Version b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.CompareTo(b) >= 0;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000383C File Offset: 0x00001A3C
		public static bool operator <(Version a, Version b)
		{
			if (a == null)
			{
				return b != null;
			}
			return a.CompareTo(b) < 0;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003852 File Offset: 0x00001A52
		public static bool operator <=(Version a, Version b)
		{
			return a == null || a.CompareTo(b) <= 0;
		}

		// Token: 0x0400000D RID: 13
		private readonly string _inputString;

		// Token: 0x0400000E RID: 14
		private readonly int _major;

		// Token: 0x0400000F RID: 15
		private readonly int _minor;

		// Token: 0x04000010 RID: 16
		private readonly int _patch;

		// Token: 0x04000011 RID: 17
		private readonly string _preRelease;

		// Token: 0x04000012 RID: 18
		private readonly string _build;

		// Token: 0x04000013 RID: 19
		private static Regex strictRegex = new Regex("^\n            \\s*v?\n            ([0-9]|[1-9][0-9]+)       # major version\n            \\.\n            ([0-9]|[1-9][0-9]+)       # minor version\n            \\.\n            ([0-9]|[1-9][0-9]+)       # patch version\n            (\\-([0-9A-Za-z\\-\\.]+))?   # pre-release version\n            (\\+([0-9A-Za-z\\-\\.]+))?   # build metadata\n            \\s*\n            $", RegexOptions.IgnorePatternWhitespace);

		// Token: 0x04000014 RID: 20
		private static Regex looseRegex = new Regex("^\n            [v=\\s]*\n            (\\d+)                     # major version\n            \\.\n            (\\d+)                     # minor version\n            \\.\n            (\\d+)                     # patch version\n            (\\-?([0-9A-Za-z\\-\\.]+))?  # pre-release version\n            (\\+([0-9A-Za-z\\-\\.]+))?   # build metadata\n            \\s*\n            $", RegexOptions.IgnorePatternWhitespace);
	}
}
