using System;

namespace Ionic.Zip
{
	// Token: 0x02000033 RID: 51
	public class ZipErrorEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00006C73 File Offset: 0x00004E73
		private ZipErrorEventArgs()
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006C7C File Offset: 0x00004E7C
		internal static ZipErrorEventArgs Saving(string archiveName, ZipEntry entry, Exception exception)
		{
			return new ZipErrorEventArgs
			{
				EventType = ZipProgressEventType.Error_Saving,
				ArchiveName = archiveName,
				CurrentEntry = entry,
				_exc = exception
			};
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00006CAF File Offset: 0x00004EAF
		public Exception Exception
		{
			get
			{
				return this._exc;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006CB7 File Offset: 0x00004EB7
		public string FileName
		{
			get
			{
				return base.CurrentEntry.LocalFileName;
			}
		}

		// Token: 0x040000E5 RID: 229
		private Exception _exc;
	}
}
