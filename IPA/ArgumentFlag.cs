using System;
using System.Collections.Generic;

namespace IPA
{
	// Token: 0x02000003 RID: 3
	public class ArgumentFlag
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002500 File Offset: 0x00000700
		public ArgumentFlag(params string[] flags)
		{
			foreach (string text in flags)
			{
				this.AddPart(text);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002550 File Offset: 0x00000750
		private void AddPart(string flagPart)
		{
			if (flagPart.StartsWith("--"))
			{
				this.LongFlags.Add(flagPart.Substring(2));
				return;
			}
			if (flagPart.StartsWith("-"))
			{
				this.ShortFlags.Add(flagPart[1]);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000259C File Offset: 0x0000079C
		public bool Exists
		{
			get
			{
				return this.exists_;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000025A4 File Offset: 0x000007A4
		public string Value
		{
			get
			{
				return this.value_;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000025AC File Offset: 0x000007AC
		public bool HasValue
		{
			get
			{
				return this.Exists && this.Value != null;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000025C1 File Offset: 0x000007C1
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000025C9 File Offset: 0x000007C9
		public string DocString { get; set; } = "";

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000025D2 File Offset: 0x000007D2
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000025DA File Offset: 0x000007DA
		public string ValueString { get; set; }

		// Token: 0x06000015 RID: 21 RVA: 0x000025E3 File Offset: 0x000007E3
		public static implicit operator bool(ArgumentFlag f)
		{
			return f.Exists;
		}

		// Token: 0x04000007 RID: 7
		internal readonly List<char> ShortFlags = new List<char>();

		// Token: 0x04000008 RID: 8
		internal readonly List<string> LongFlags = new List<string>();

		// Token: 0x04000009 RID: 9
		internal string value_;

		// Token: 0x0400000A RID: 10
		internal bool exists_;
	}
}
