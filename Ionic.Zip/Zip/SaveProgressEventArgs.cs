using System;

namespace Ionic.Zip
{
	// Token: 0x02000031 RID: 49
	public class SaveProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000174 RID: 372 RVA: 0x00006A66 File Offset: 0x00004C66
		internal SaveProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesSaved, ZipEntry entry)
			: base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesSaved = entriesSaved;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006A8F File Offset: 0x00004C8F
		internal SaveProgressEventArgs()
		{
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006A97 File Offset: 0x00004C97
		internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor)
			: base(archiveName, flavor)
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006AA4 File Offset: 0x00004CA4
		internal static SaveProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006AD8 File Offset: 0x00004CD8
		internal static SaveProgressEventArgs Started(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006AF0 File Offset: 0x00004CF0
		internal static SaveProgressEventArgs Completed(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00006B07 File Offset: 0x00004D07
		public int EntriesSaved
		{
			get
			{
				return this._entriesSaved;
			}
		}

		// Token: 0x040000E2 RID: 226
		private int _entriesSaved;
	}
}
