using System;

namespace Ionic.Zip
{
	// Token: 0x0200002E RID: 46
	public class ZipProgressEventArgs : EventArgs
	{
		// Token: 0x06000158 RID: 344 RVA: 0x000068AF File Offset: 0x00004AAF
		internal ZipProgressEventArgs()
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000068B7 File Offset: 0x00004AB7
		internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
		{
			this._archiveName = archiveName;
			this._flavor = flavor;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000068CD File Offset: 0x00004ACD
		// (set) Token: 0x0600015B RID: 347 RVA: 0x000068D5 File Offset: 0x00004AD5
		public int EntriesTotal
		{
			get
			{
				return this._entriesTotal;
			}
			set
			{
				this._entriesTotal = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000068DE File Offset: 0x00004ADE
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000068E6 File Offset: 0x00004AE6
		public ZipEntry CurrentEntry
		{
			get
			{
				return this._latestEntry;
			}
			set
			{
				this._latestEntry = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000068EF File Offset: 0x00004AEF
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000068F7 File Offset: 0x00004AF7
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = this._cancel || value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000690B File Offset: 0x00004B0B
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00006913 File Offset: 0x00004B13
		public ZipProgressEventType EventType
		{
			get
			{
				return this._flavor;
			}
			set
			{
				this._flavor = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000691C File Offset: 0x00004B1C
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00006924 File Offset: 0x00004B24
		public string ArchiveName
		{
			get
			{
				return this._archiveName;
			}
			set
			{
				this._archiveName = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000692D File Offset: 0x00004B2D
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00006935 File Offset: 0x00004B35
		public long BytesTransferred
		{
			get
			{
				return this._bytesTransferred;
			}
			set
			{
				this._bytesTransferred = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000693E File Offset: 0x00004B3E
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00006946 File Offset: 0x00004B46
		public long TotalBytesToTransfer
		{
			get
			{
				return this._totalBytesToTransfer;
			}
			set
			{
				this._totalBytesToTransfer = value;
			}
		}

		// Token: 0x040000DB RID: 219
		private int _entriesTotal;

		// Token: 0x040000DC RID: 220
		private bool _cancel;

		// Token: 0x040000DD RID: 221
		private ZipEntry _latestEntry;

		// Token: 0x040000DE RID: 222
		private ZipProgressEventType _flavor;

		// Token: 0x040000DF RID: 223
		private string _archiveName;

		// Token: 0x040000E0 RID: 224
		private long _bytesTransferred;

		// Token: 0x040000E1 RID: 225
		private long _totalBytesToTransfer;
	}
}
