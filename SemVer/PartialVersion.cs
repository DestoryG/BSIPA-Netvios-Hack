using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SemVer
{
	// Token: 0x02000005 RID: 5
	internal class PartialVersion
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002E33 File Offset: 0x00001033
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002E3B File Offset: 0x0000103B
		public int? Major { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002E44 File Offset: 0x00001044
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002E4C File Offset: 0x0000104C
		public int? Minor { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002E55 File Offset: 0x00001055
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002E5D File Offset: 0x0000105D
		public int? Patch { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002E66 File Offset: 0x00001066
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002E6E File Offset: 0x0000106E
		public string PreRelease { get; set; }

		// Token: 0x06000020 RID: 32 RVA: 0x00002E78 File Offset: 0x00001078
		public PartialVersion(string input)
		{
			string[] array = new string[] { "X", "x", "*" };
			if (input.Trim() == "")
			{
				return;
			}
			Match match = PartialVersion.regex.Match(input);
			if (!match.Success)
			{
				throw new ArgumentException(string.Format("Invalid version string: \"{0}\"", input));
			}
			if (array.Contains(match.Groups[1].Value))
			{
				this.Major = null;
			}
			else
			{
				this.Major = new int?(int.Parse(match.Groups[1].Value));
			}
			if (match.Groups[2].Success)
			{
				if (array.Contains(match.Groups[3].Value))
				{
					this.Minor = null;
				}
				else
				{
					this.Minor = new int?(int.Parse(match.Groups[3].Value));
				}
			}
			if (match.Groups[4].Success)
			{
				if (array.Contains(match.Groups[5].Value))
				{
					this.Patch = null;
				}
				else
				{
					this.Patch = new int?(int.Parse(match.Groups[5].Value));
				}
			}
			if (match.Groups[6].Success)
			{
				this.PreRelease = match.Groups[7].Value;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003014 File Offset: 0x00001214
		public Version ToZeroVersion()
		{
			return new Version(this.Major.GetValueOrDefault(), this.Minor.GetValueOrDefault(), this.Patch.GetValueOrDefault(), this.PreRelease, null);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003058 File Offset: 0x00001258
		public bool IsFull()
		{
			return this.Major != null && this.Minor != null && this.Patch != null;
		}

		// Token: 0x0400000A RID: 10
		private static Regex regex = new Regex("^\n                [v=\\s]*\n                (\\d+|[Xx\\*])                      # major version\n                (\n                    \\.\n                    (\\d+|[Xx\\*])                  # minor version\n                    (\n                        \\.\n                        (\\d+|[Xx\\*])              # patch version\n                        (\\-?([0-9A-Za-z\\-\\.]+))?  # pre-release version\n                        (\\+([0-9A-Za-z\\-\\.]+))?   # build version (ignored)\n                    )?\n                )?\n                $", RegexOptions.IgnorePatternWhitespace);
	}
}
