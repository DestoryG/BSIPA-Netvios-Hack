using System;

namespace Ionic.Zip
{
	// Token: 0x02000030 RID: 48
	public class AddProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x0600016F RID: 367 RVA: 0x00006A02 File Offset: 0x00004C02
		internal AddProgressEventArgs()
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006A0A File Offset: 0x00004C0A
		private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor)
			: base(archiveName, flavor)
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006A14 File Offset: 0x00004C14
		internal static AddProgressEventArgs AfterEntry(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006A38 File Offset: 0x00004C38
		internal static AddProgressEventArgs Started(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006A50 File Offset: 0x00004C50
		internal static AddProgressEventArgs Completed(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
		}
	}
}
