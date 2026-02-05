using System;
using System.IO;
using System.Text;
using IniParser.Exceptions;
using IniParser.Model;
using IniParser.Parser;

namespace IniParser
{
	// Token: 0x02000002 RID: 2
	public class FileIniDataParser : StreamIniDataParser
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public FileIniDataParser()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205A File Offset: 0x0000025A
		public FileIniDataParser(IniDataParser parser)
			: base(parser)
		{
			base.Parser = parser;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		[Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
		public IniData LoadFile(string filePath)
		{
			return this.ReadFile(filePath);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
		[Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
		public IniData LoadFile(string filePath, Encoding fileEncoding)
		{
			return this.ReadFile(filePath, fileEncoding);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020A8 File Offset: 0x000002A8
		public IniData ReadFile(string filePath)
		{
			return this.ReadFile(filePath, Encoding.ASCII);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
		public IniData ReadFile(string filePath, Encoding fileEncoding)
		{
			bool flag = filePath == string.Empty;
			if (flag)
			{
				throw new ArgumentException("Bad filename.");
			}
			IniData iniData;
			try
			{
				using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					using (StreamReader streamReader = new StreamReader(fileStream, fileEncoding))
					{
						iniData = base.ReadData(streamReader);
					}
				}
			}
			catch (IOException ex)
			{
				throw new ParsingException(string.Format("Could not parse file {0}", filePath), ex);
			}
			return iniData;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002168 File Offset: 0x00000368
		[Obsolete("Please use WriteFile method instead of this one as is more semantically accurate")]
		public void SaveFile(string filePath, IniData parsedData)
		{
			this.WriteFile(filePath, parsedData, Encoding.UTF8);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000217C File Offset: 0x0000037C
		public void WriteFile(string filePath, IniData parsedData, Encoding fileEncoding = null)
		{
			bool flag = fileEncoding == null;
			if (flag)
			{
				fileEncoding = Encoding.UTF8;
			}
			bool flag2 = string.IsNullOrEmpty(filePath);
			if (flag2)
			{
				throw new ArgumentException("Bad filename.");
			}
			bool flag3 = parsedData == null;
			if (flag3)
			{
				throw new ArgumentNullException("parsedData");
			}
			using (FileStream fileStream = File.Open(filePath, FileMode.Create, FileAccess.Write))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream, fileEncoding))
				{
					base.WriteData(streamWriter, parsedData);
				}
			}
		}
	}
}
