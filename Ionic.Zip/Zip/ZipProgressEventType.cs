using System;

namespace Ionic.Zip
{
	// Token: 0x0200002D RID: 45
	public enum ZipProgressEventType
	{
		// Token: 0x040000C3 RID: 195
		Adding_Started,
		// Token: 0x040000C4 RID: 196
		Adding_AfterAddEntry,
		// Token: 0x040000C5 RID: 197
		Adding_Completed,
		// Token: 0x040000C6 RID: 198
		Reading_Started,
		// Token: 0x040000C7 RID: 199
		Reading_BeforeReadEntry,
		// Token: 0x040000C8 RID: 200
		Reading_AfterReadEntry,
		// Token: 0x040000C9 RID: 201
		Reading_Completed,
		// Token: 0x040000CA RID: 202
		Reading_ArchiveBytesRead,
		// Token: 0x040000CB RID: 203
		Saving_Started,
		// Token: 0x040000CC RID: 204
		Saving_BeforeWriteEntry,
		// Token: 0x040000CD RID: 205
		Saving_AfterWriteEntry,
		// Token: 0x040000CE RID: 206
		Saving_Completed,
		// Token: 0x040000CF RID: 207
		Saving_AfterSaveTempArchive,
		// Token: 0x040000D0 RID: 208
		Saving_BeforeRenameTempArchive,
		// Token: 0x040000D1 RID: 209
		Saving_AfterRenameTempArchive,
		// Token: 0x040000D2 RID: 210
		Saving_AfterCompileSelfExtractor,
		// Token: 0x040000D3 RID: 211
		Saving_EntryBytesRead,
		// Token: 0x040000D4 RID: 212
		Extracting_BeforeExtractEntry,
		// Token: 0x040000D5 RID: 213
		Extracting_AfterExtractEntry,
		// Token: 0x040000D6 RID: 214
		Extracting_ExtractEntryWouldOverwrite,
		// Token: 0x040000D7 RID: 215
		Extracting_EntryBytesWritten,
		// Token: 0x040000D8 RID: 216
		Extracting_BeforeExtractAll,
		// Token: 0x040000D9 RID: 217
		Extracting_AfterExtractAll,
		// Token: 0x040000DA RID: 218
		Error_Saving
	}
}
