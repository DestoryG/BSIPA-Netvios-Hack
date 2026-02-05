using System;

namespace Ionic.Zip
{
	// Token: 0x0200002F RID: 47
	public class ReadProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000168 RID: 360 RVA: 0x0000694F File Offset: 0x00004B4F
		internal ReadProgressEventArgs()
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006957 File Offset: 0x00004B57
		private ReadProgressEventArgs(string archiveName, ZipProgressEventType flavor)
			: base(archiveName, flavor)
		{
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006964 File Offset: 0x00004B64
		internal static ReadProgressEventArgs Before(string archiveName, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_BeforeReadEntry)
			{
				EntriesTotal = entriesTotal
			};
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006984 File Offset: 0x00004B84
		internal static ReadProgressEventArgs After(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_AfterReadEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000069A8 File Offset: 0x00004BA8
		internal static ReadProgressEventArgs Started(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Started);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000069C0 File Offset: 0x00004BC0
		internal static ReadProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_ArchiveBytesRead)
			{
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000069EC File Offset: 0x00004BEC
		internal static ReadProgressEventArgs Completed(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Completed);
		}
	}
}
