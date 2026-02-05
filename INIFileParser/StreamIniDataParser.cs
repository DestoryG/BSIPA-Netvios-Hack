using System;
using System.IO;
using IniParser.Model;
using IniParser.Model.Formatting;
using IniParser.Parser;

namespace IniParser
{
	// Token: 0x02000003 RID: 3
	public class StreamIniDataParser
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000221C File Offset: 0x0000041C
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002224 File Offset: 0x00000424
		public IniDataParser Parser { get; protected set; }

		// Token: 0x0600000B RID: 11 RVA: 0x0000222D File Offset: 0x0000042D
		public StreamIniDataParser()
			: this(new IniDataParser())
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000223C File Offset: 0x0000043C
		public StreamIniDataParser(IniDataParser parser)
		{
			this.Parser = parser;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002250 File Offset: 0x00000450
		public IniData ReadData(StreamReader reader)
		{
			bool flag = reader == null;
			if (flag)
			{
				throw new ArgumentNullException("reader");
			}
			return this.Parser.Parse(reader.ReadToEnd());
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002288 File Offset: 0x00000488
		public void WriteData(StreamWriter writer, IniData iniData)
		{
			bool flag = iniData == null;
			if (flag)
			{
				throw new ArgumentNullException("iniData");
			}
			bool flag2 = writer == null;
			if (flag2)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(iniData.ToString());
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022CC File Offset: 0x000004CC
		public void WriteData(StreamWriter writer, IniData iniData, IIniDataFormatter formatter)
		{
			bool flag = formatter == null;
			if (flag)
			{
				throw new ArgumentNullException("formatter");
			}
			bool flag2 = iniData == null;
			if (flag2)
			{
				throw new ArgumentNullException("iniData");
			}
			bool flag3 = writer == null;
			if (flag3)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(iniData.ToString(formatter));
		}
	}
}
