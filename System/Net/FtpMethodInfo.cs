using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020000EE RID: 238
	internal class FtpMethodInfo
	{
		// Token: 0x0600080C RID: 2060 RVA: 0x0002C490 File Offset: 0x0002A690
		internal FtpMethodInfo(string method, FtpOperation operation, FtpMethodFlags flags, string httpCommand)
		{
			this.Method = method;
			this.Operation = operation;
			this.Flags = flags;
			this.HttpCommand = httpCommand;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002C4B5 File Offset: 0x0002A6B5
		internal bool HasFlag(FtpMethodFlags flags)
		{
			return (this.Flags & flags) > FtpMethodFlags.None;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x0002C4C2 File Offset: 0x0002A6C2
		internal bool IsCommandOnly
		{
			get
			{
				return (this.Flags & (FtpMethodFlags.IsDownload | FtpMethodFlags.IsUpload)) == FtpMethodFlags.None;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0002C4CF File Offset: 0x0002A6CF
		internal bool IsUpload
		{
			get
			{
				return (this.Flags & FtpMethodFlags.IsUpload) > FtpMethodFlags.None;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0002C4DC File Offset: 0x0002A6DC
		internal bool IsDownload
		{
			get
			{
				return (this.Flags & FtpMethodFlags.IsDownload) > FtpMethodFlags.None;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0002C4E9 File Offset: 0x0002A6E9
		internal bool HasHttpCommand
		{
			get
			{
				return (this.Flags & FtpMethodFlags.HasHttpCommand) > FtpMethodFlags.None;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0002C4FA File Offset: 0x0002A6FA
		internal bool ShouldParseForResponseUri
		{
			get
			{
				return (this.Flags & FtpMethodFlags.ShouldParseForResponseUri) > FtpMethodFlags.None;
			}
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002C508 File Offset: 0x0002A708
		internal static FtpMethodInfo GetMethodInfo(string method)
		{
			method = method.ToUpper(CultureInfo.InvariantCulture);
			foreach (FtpMethodInfo ftpMethodInfo in FtpMethodInfo.KnownMethodInfo)
			{
				if (method == ftpMethodInfo.Method)
				{
					return ftpMethodInfo;
				}
			}
			throw new ArgumentException(SR.GetString("net_ftp_unsupported_method"), "method");
		}

		// Token: 0x04000D9B RID: 3483
		internal string Method;

		// Token: 0x04000D9C RID: 3484
		internal FtpOperation Operation;

		// Token: 0x04000D9D RID: 3485
		internal FtpMethodFlags Flags;

		// Token: 0x04000D9E RID: 3486
		internal string HttpCommand;

		// Token: 0x04000D9F RID: 3487
		private static readonly FtpMethodInfo[] KnownMethodInfo = new FtpMethodInfo[]
		{
			new FtpMethodInfo("RETR", FtpOperation.DownloadFile, FtpMethodFlags.IsDownload | FtpMethodFlags.TakesParameter | FtpMethodFlags.HasHttpCommand, "GET"),
			new FtpMethodInfo("NLST", FtpOperation.ListDirectory, FtpMethodFlags.IsDownload | FtpMethodFlags.MayTakeParameter | FtpMethodFlags.HasHttpCommand | FtpMethodFlags.MustChangeWorkingDirectoryToPath, "GET"),
			new FtpMethodInfo("LIST", FtpOperation.ListDirectoryDetails, FtpMethodFlags.IsDownload | FtpMethodFlags.MayTakeParameter | FtpMethodFlags.HasHttpCommand | FtpMethodFlags.MustChangeWorkingDirectoryToPath, "GET"),
			new FtpMethodInfo("STOR", FtpOperation.UploadFile, FtpMethodFlags.IsUpload | FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("STOU", FtpOperation.UploadFileUnique, FtpMethodFlags.IsUpload | FtpMethodFlags.DoesNotTakeParameter | FtpMethodFlags.ShouldParseForResponseUri | FtpMethodFlags.MustChangeWorkingDirectoryToPath, null),
			new FtpMethodInfo("APPE", FtpOperation.AppendFile, FtpMethodFlags.IsUpload | FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("DELE", FtpOperation.DeleteFile, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("MDTM", FtpOperation.GetDateTimestamp, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("SIZE", FtpOperation.GetFileSize, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("RENAME", FtpOperation.Rename, FtpMethodFlags.TakesParameter, null),
			new FtpMethodInfo("MKD", FtpOperation.MakeDirectory, FtpMethodFlags.TakesParameter | FtpMethodFlags.ParameterIsDirectory, null),
			new FtpMethodInfo("RMD", FtpOperation.RemoveDirectory, FtpMethodFlags.TakesParameter | FtpMethodFlags.ParameterIsDirectory, null),
			new FtpMethodInfo("PWD", FtpOperation.PrintWorkingDirectory, FtpMethodFlags.DoesNotTakeParameter, null)
		};
	}
}
