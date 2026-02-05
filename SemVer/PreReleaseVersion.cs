using System;
using System.Collections.Generic;
using System.Linq;

namespace SemVer
{
	// Token: 0x02000006 RID: 6
	internal static class PreReleaseVersion
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000030A8 File Offset: 0x000012A8
		public static int Compare(string a, string b)
		{
			if (a == null && b == null)
			{
				return 0;
			}
			if (a == null)
			{
				return 1;
			}
			if (b == null)
			{
				return -1;
			}
			foreach (int num in PreReleaseVersion.IdentifierComparisons(PreReleaseVersion.Identifiers(a), PreReleaseVersion.Identifiers(b)))
			{
				if (num != 0)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003118 File Offset: 0x00001318
		public static string Clean(string input)
		{
			IEnumerable<string> enumerable = from i in PreReleaseVersion.Identifiers(input)
				select i.Clean();
			return string.Join(".", enumerable.ToArray<string>());
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003160 File Offset: 0x00001360
		private static IEnumerable<PreReleaseVersion.Identifier> Identifiers(string input)
		{
			foreach (string text in input.Split(new char[] { '.' }))
			{
				yield return new PreReleaseVersion.Identifier(text);
			}
			string[] array = null;
			yield break;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003170 File Offset: 0x00001370
		private static IEnumerable<int> IdentifierComparisons(IEnumerable<PreReleaseVersion.Identifier> aIdentifiers, IEnumerable<PreReleaseVersion.Identifier> bIdentifiers)
		{
			foreach (Tuple<PreReleaseVersion.Identifier, PreReleaseVersion.Identifier> tuple in PreReleaseVersion.ZipIdentifiers(aIdentifiers, bIdentifiers))
			{
				PreReleaseVersion.Identifier item = tuple.Item1;
				PreReleaseVersion.Identifier item2 = tuple.Item2;
				if (item == item2)
				{
					yield return 0;
				}
				else if (item == null)
				{
					yield return -1;
				}
				else if (item2 == null)
				{
					yield return 1;
				}
				else if (item.IsNumeric && item2.IsNumeric)
				{
					yield return item.IntValue.CompareTo(item2.IntValue);
				}
				else if (!item.IsNumeric && !item2.IsNumeric)
				{
					yield return string.CompareOrdinal(item.Value, item2.Value);
				}
				else if (item.IsNumeric && !item2.IsNumeric)
				{
					yield return -1;
				}
				else
				{
					yield return 1;
				}
			}
			IEnumerator<Tuple<PreReleaseVersion.Identifier, PreReleaseVersion.Identifier>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003187 File Offset: 0x00001387
		private static IEnumerable<Tuple<PreReleaseVersion.Identifier, PreReleaseVersion.Identifier>> ZipIdentifiers(IEnumerable<PreReleaseVersion.Identifier> first, IEnumerable<PreReleaseVersion.Identifier> second)
		{
			using (IEnumerator<PreReleaseVersion.Identifier> ie = first.GetEnumerator())
			{
				using (IEnumerator<PreReleaseVersion.Identifier> ie2 = second.GetEnumerator())
				{
					while (ie.MoveNext())
					{
						if (ie2.MoveNext())
						{
							yield return Tuple.Create<PreReleaseVersion.Identifier, PreReleaseVersion.Identifier>(ie.Current, ie2.Current);
						}
						else
						{
							yield return Tuple.Create<PreReleaseVersion.Identifier, PreReleaseVersion.Identifier>(ie.Current, null);
						}
					}
					while (ie2.MoveNext())
					{
						PreReleaseVersion.Identifier identifier = ie2.Current;
						yield return Tuple.Create<PreReleaseVersion.Identifier, PreReleaseVersion.Identifier>(null, identifier);
					}
				}
				IEnumerator<PreReleaseVersion.Identifier> ie2 = null;
			}
			IEnumerator<PreReleaseVersion.Identifier> ie = null;
			yield break;
			yield break;
		}

		// Token: 0x0200000E RID: 14
		private class Identifier
		{
			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000067 RID: 103 RVA: 0x000039BB File Offset: 0x00001BBB
			// (set) Token: 0x06000068 RID: 104 RVA: 0x000039C3 File Offset: 0x00001BC3
			public bool IsNumeric { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000069 RID: 105 RVA: 0x000039CC File Offset: 0x00001BCC
			// (set) Token: 0x0600006A RID: 106 RVA: 0x000039D4 File Offset: 0x00001BD4
			public int IntValue { get; set; }

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x0600006B RID: 107 RVA: 0x000039DD File Offset: 0x00001BDD
			// (set) Token: 0x0600006C RID: 108 RVA: 0x000039E5 File Offset: 0x00001BE5
			public string Value { get; set; }

			// Token: 0x0600006D RID: 109 RVA: 0x000039EE File Offset: 0x00001BEE
			public Identifier(string input)
			{
				this.Value = input;
				this.SetNumeric();
			}

			// Token: 0x0600006E RID: 110 RVA: 0x00003A04 File Offset: 0x00001C04
			public string Clean()
			{
				if (!this.IsNumeric)
				{
					return this.Value;
				}
				return this.IntValue.ToString();
			}

			// Token: 0x0600006F RID: 111 RVA: 0x00003A30 File Offset: 0x00001C30
			private void SetNumeric()
			{
				int num;
				bool flag = int.TryParse(this.Value, out num);
				this.IsNumeric = flag && num >= 0;
				this.IntValue = num;
			}
		}
	}
}
