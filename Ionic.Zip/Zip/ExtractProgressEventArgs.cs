using System;

namespace Ionic.Zip
{
	// Token: 0x02000032 RID: 50
	public class ExtractProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00006B0F File Offset: 0x00004D0F
		internal ExtractProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesExtracted, ZipEntry entry, string extractLocation)
			: base(archiveName, before ? ZipProgressEventType.Extracting_BeforeExtractEntry : ZipProgressEventType.Extracting_AfterExtractEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesExtracted = entriesExtracted;
			this._target = extractLocation;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006B40 File Offset: 0x00004D40
		internal ExtractProgressEventArgs(string archiveName, ZipProgressEventType flavor)
			: base(archiveName, flavor)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006B4A File Offset: 0x00004D4A
		internal ExtractProgressEventArgs()
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006B54 File Offset: 0x00004D54
		internal static ExtractProgressEventArgs BeforeExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_BeforeExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006B88 File Offset: 0x00004D88
		internal static ExtractProgressEventArgs ExtractExisting(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006BBC File Offset: 0x00004DBC
		internal static ExtractProgressEventArgs AfterExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_AfterExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006BF0 File Offset: 0x00004DF0
		internal static ExtractProgressEventArgs ExtractAllStarted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_BeforeExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006C10 File Offset: 0x00004E10
		internal static ExtractProgressEventArgs ExtractAllCompleted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_AfterExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006C30 File Offset: 0x00004E30
		internal static ExtractProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesWritten, long totalBytes)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_EntryBytesWritten)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesWritten,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00006C63 File Offset: 0x00004E63
		public int EntriesExtracted
		{
			get
			{
				return this._entriesExtracted;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006C6B File Offset: 0x00004E6B
		public string ExtractLocation
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x040000E3 RID: 227
		private int _entriesExtracted;

		// Token: 0x040000E4 RID: 228
		private string _target;
	}
}
