using System;

namespace System.Net
{
	// Token: 0x020000EB RID: 235
	public static class WebRequestMethods
	{
		// Token: 0x020006F8 RID: 1784
		public static class Ftp
		{
			// Token: 0x0400309D RID: 12445
			public const string DownloadFile = "RETR";

			// Token: 0x0400309E RID: 12446
			public const string ListDirectory = "NLST";

			// Token: 0x0400309F RID: 12447
			public const string UploadFile = "STOR";

			// Token: 0x040030A0 RID: 12448
			public const string DeleteFile = "DELE";

			// Token: 0x040030A1 RID: 12449
			public const string AppendFile = "APPE";

			// Token: 0x040030A2 RID: 12450
			public const string GetFileSize = "SIZE";

			// Token: 0x040030A3 RID: 12451
			public const string UploadFileWithUniqueName = "STOU";

			// Token: 0x040030A4 RID: 12452
			public const string MakeDirectory = "MKD";

			// Token: 0x040030A5 RID: 12453
			public const string RemoveDirectory = "RMD";

			// Token: 0x040030A6 RID: 12454
			public const string ListDirectoryDetails = "LIST";

			// Token: 0x040030A7 RID: 12455
			public const string GetDateTimestamp = "MDTM";

			// Token: 0x040030A8 RID: 12456
			public const string PrintWorkingDirectory = "PWD";

			// Token: 0x040030A9 RID: 12457
			public const string Rename = "RENAME";
		}

		// Token: 0x020006F9 RID: 1785
		public static class Http
		{
			// Token: 0x040030AA RID: 12458
			public const string Get = "GET";

			// Token: 0x040030AB RID: 12459
			public const string Connect = "CONNECT";

			// Token: 0x040030AC RID: 12460
			public const string Head = "HEAD";

			// Token: 0x040030AD RID: 12461
			public const string Put = "PUT";

			// Token: 0x040030AE RID: 12462
			public const string Post = "POST";

			// Token: 0x040030AF RID: 12463
			public const string MkCol = "MKCOL";
		}

		// Token: 0x020006FA RID: 1786
		public static class File
		{
			// Token: 0x040030B0 RID: 12464
			public const string DownloadFile = "GET";

			// Token: 0x040030B1 RID: 12465
			public const string UploadFile = "PUT";
		}
	}
}
