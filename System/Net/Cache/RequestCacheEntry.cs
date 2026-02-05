using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using Microsoft.Win32;

namespace System.Net.Cache
{
	// Token: 0x0200030E RID: 782
	internal class RequestCacheEntry
	{
		// Token: 0x06001BE9 RID: 7145 RVA: 0x000853B8 File Offset: 0x000835B8
		internal RequestCacheEntry()
		{
			this.m_ExpiresUtc = (this.m_LastAccessedUtc = (this.m_LastModifiedUtc = (this.m_LastSynchronizedUtc = DateTime.MinValue)));
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000853F4 File Offset: 0x000835F4
		internal RequestCacheEntry(_WinInetCache.Entry entry, bool isPrivateEntry)
		{
			this.m_IsPrivateEntry = isPrivateEntry;
			this.m_StreamSize = ((long)entry.Info.SizeHigh << 32) | (long)entry.Info.SizeLow;
			this.m_ExpiresUtc = (entry.Info.ExpireTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.ExpireTime.ToLong()));
			this.m_HitCount = entry.Info.HitRate;
			this.m_LastAccessedUtc = (entry.Info.LastAccessTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastAccessTime.ToLong()));
			this.m_LastModifiedUtc = (entry.Info.LastModifiedTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastModifiedTime.ToLong()));
			this.m_LastSynchronizedUtc = (entry.Info.LastSyncTime.IsNull ? DateTime.MinValue : DateTime.FromFileTimeUtc(entry.Info.LastSyncTime.ToLong()));
			this.m_MaxStale = TimeSpan.FromSeconds((double)entry.Info.U.ExemptDelta);
			if (this.m_MaxStale == WinInetCache.s_MaxTimeSpanForInt32)
			{
				this.m_MaxStale = TimeSpan.MaxValue;
			}
			this.m_UsageCount = entry.Info.UseCount;
			this.m_IsPartialEntry = (entry.Info.EntryType & _WinInetCache.EntryType.Sparse) > (_WinInetCache.EntryType)0;
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x00085575 File Offset: 0x00083775
		// (set) Token: 0x06001BEC RID: 7148 RVA: 0x0008557D File Offset: 0x0008377D
		internal bool IsPrivateEntry
		{
			get
			{
				return this.m_IsPrivateEntry;
			}
			set
			{
				this.m_IsPrivateEntry = value;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x00085586 File Offset: 0x00083786
		// (set) Token: 0x06001BEE RID: 7150 RVA: 0x0008558E File Offset: 0x0008378E
		internal long StreamSize
		{
			get
			{
				return this.m_StreamSize;
			}
			set
			{
				this.m_StreamSize = value;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x00085597 File Offset: 0x00083797
		// (set) Token: 0x06001BF0 RID: 7152 RVA: 0x0008559F File Offset: 0x0008379F
		internal DateTime ExpiresUtc
		{
			get
			{
				return this.m_ExpiresUtc;
			}
			set
			{
				this.m_ExpiresUtc = value;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x000855A8 File Offset: 0x000837A8
		// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x000855B0 File Offset: 0x000837B0
		internal DateTime LastAccessedUtc
		{
			get
			{
				return this.m_LastAccessedUtc;
			}
			set
			{
				this.m_LastAccessedUtc = value;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x000855B9 File Offset: 0x000837B9
		// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x000855C1 File Offset: 0x000837C1
		internal DateTime LastModifiedUtc
		{
			get
			{
				return this.m_LastModifiedUtc;
			}
			set
			{
				this.m_LastModifiedUtc = value;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x000855CA File Offset: 0x000837CA
		// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x000855D2 File Offset: 0x000837D2
		internal DateTime LastSynchronizedUtc
		{
			get
			{
				return this.m_LastSynchronizedUtc;
			}
			set
			{
				this.m_LastSynchronizedUtc = value;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x000855DB File Offset: 0x000837DB
		// (set) Token: 0x06001BF8 RID: 7160 RVA: 0x000855E3 File Offset: 0x000837E3
		internal TimeSpan MaxStale
		{
			get
			{
				return this.m_MaxStale;
			}
			set
			{
				this.m_MaxStale = value;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x000855EC File Offset: 0x000837EC
		// (set) Token: 0x06001BFA RID: 7162 RVA: 0x000855F4 File Offset: 0x000837F4
		internal int HitCount
		{
			get
			{
				return this.m_HitCount;
			}
			set
			{
				this.m_HitCount = value;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x000855FD File Offset: 0x000837FD
		// (set) Token: 0x06001BFC RID: 7164 RVA: 0x00085605 File Offset: 0x00083805
		internal int UsageCount
		{
			get
			{
				return this.m_UsageCount;
			}
			set
			{
				this.m_UsageCount = value;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x0008560E File Offset: 0x0008380E
		// (set) Token: 0x06001BFE RID: 7166 RVA: 0x00085616 File Offset: 0x00083816
		internal bool IsPartialEntry
		{
			get
			{
				return this.m_IsPartialEntry;
			}
			set
			{
				this.m_IsPartialEntry = value;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x0008561F File Offset: 0x0008381F
		// (set) Token: 0x06001C00 RID: 7168 RVA: 0x00085627 File Offset: 0x00083827
		internal StringCollection EntryMetadata
		{
			get
			{
				return this.m_EntryMetadata;
			}
			set
			{
				this.m_EntryMetadata = value;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x00085630 File Offset: 0x00083830
		// (set) Token: 0x06001C02 RID: 7170 RVA: 0x00085638 File Offset: 0x00083838
		internal StringCollection SystemMetadata
		{
			get
			{
				return this.m_SystemMetadata;
			}
			set
			{
				this.m_SystemMetadata = value;
			}
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x00085644 File Offset: 0x00083844
		internal virtual string ToString(bool verbose)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append("\r\nIsPrivateEntry   = ").Append(this.IsPrivateEntry);
			stringBuilder.Append("\r\nIsPartialEntry   = ").Append(this.IsPartialEntry);
			stringBuilder.Append("\r\nStreamSize       = ").Append(this.StreamSize);
			stringBuilder.Append("\r\nExpires          = ").Append((this.ExpiresUtc == DateTime.MinValue) ? "" : this.ExpiresUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastAccessed     = ").Append((this.LastAccessedUtc == DateTime.MinValue) ? "" : this.LastAccessedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastModified     = ").Append((this.LastModifiedUtc == DateTime.MinValue) ? "" : this.LastModifiedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nLastSynchronized = ").Append((this.LastSynchronizedUtc == DateTime.MinValue) ? "" : this.LastSynchronizedUtc.ToString("r", CultureInfo.CurrentCulture));
			stringBuilder.Append("\r\nMaxStale(sec)    = ").Append((this.MaxStale == TimeSpan.MinValue) ? "" : ((int)this.MaxStale.TotalSeconds).ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nHitCount         = ").Append(this.HitCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\nUsageCount       = ").Append(this.UsageCount.ToString(NumberFormatInfo.CurrentInfo));
			stringBuilder.Append("\r\n");
			if (verbose)
			{
				stringBuilder.Append("EntryMetadata:\r\n");
				if (this.m_EntryMetadata != null)
				{
					foreach (string text in this.m_EntryMetadata)
					{
						stringBuilder.Append(text).Append("\r\n");
					}
				}
				stringBuilder.Append("---\r\nSystemMetadata:\r\n");
				if (this.m_SystemMetadata != null)
				{
					foreach (string text2 in this.m_SystemMetadata)
					{
						stringBuilder.Append(text2).Append("\r\n");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001B35 RID: 6965
		private bool m_IsPrivateEntry;

		// Token: 0x04001B36 RID: 6966
		private long m_StreamSize;

		// Token: 0x04001B37 RID: 6967
		private DateTime m_ExpiresUtc;

		// Token: 0x04001B38 RID: 6968
		private int m_HitCount;

		// Token: 0x04001B39 RID: 6969
		private DateTime m_LastAccessedUtc;

		// Token: 0x04001B3A RID: 6970
		private DateTime m_LastModifiedUtc;

		// Token: 0x04001B3B RID: 6971
		private DateTime m_LastSynchronizedUtc;

		// Token: 0x04001B3C RID: 6972
		private TimeSpan m_MaxStale;

		// Token: 0x04001B3D RID: 6973
		private int m_UsageCount;

		// Token: 0x04001B3E RID: 6974
		private bool m_IsPartialEntry;

		// Token: 0x04001B3F RID: 6975
		private StringCollection m_EntryMetadata;

		// Token: 0x04001B40 RID: 6976
		private StringCollection m_SystemMetadata;
	}
}
