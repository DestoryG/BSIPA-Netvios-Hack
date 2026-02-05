using System;
using System.Reflection;

namespace IniParser.Exceptions
{
	// Token: 0x02000005 RID: 5
	public class ParsingException : Exception
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000238C File Offset: 0x0000058C
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002394 File Offset: 0x00000594
		public Version LibVersion { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000239D File Offset: 0x0000059D
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000023A5 File Offset: 0x000005A5
		public int LineNumber { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000023AE File Offset: 0x000005AE
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000023B6 File Offset: 0x000005B6
		public string LineValue { get; private set; }

		// Token: 0x0600001C RID: 28 RVA: 0x000023BF File Offset: 0x000005BF
		public ParsingException(string msg)
			: this(msg, 0, string.Empty, null)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023D1 File Offset: 0x000005D1
		public ParsingException(string msg, Exception innerException)
			: this(msg, 0, string.Empty, innerException)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023E3 File Offset: 0x000005E3
		public ParsingException(string msg, int lineNumber, string lineValue)
			: this(msg, lineNumber, lineValue, null)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023F4 File Offset: 0x000005F4
		public ParsingException(string msg, int lineNumber, string lineValue, Exception innerException)
			: base(string.Format("{0} while parsing line number {1} with value '{2}' - IniParser version: {3}", new object[]
			{
				msg,
				lineNumber,
				lineValue,
				Assembly.GetExecutingAssembly().GetName().Version
			}), innerException)
		{
			this.LibVersion = Assembly.GetExecutingAssembly().GetName().Version;
			this.LineNumber = lineNumber;
			this.LineValue = lineValue;
		}
	}
}
