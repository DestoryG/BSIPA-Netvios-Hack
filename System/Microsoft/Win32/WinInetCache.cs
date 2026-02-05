using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Microsoft.Win32
{
	// Token: 0x0200002A RID: 42
	internal class WinInetCache : RequestCache
	{
		// Token: 0x06000282 RID: 642 RVA: 0x0000F47D File Offset: 0x0000D67D
		internal WinInetCache(bool isPrivateCache, bool canWrite, bool async)
			: base(isPrivateCache, canWrite)
		{
			this.async = async;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000F48E File Offset: 0x0000D68E
		internal override Stream Retrieve(string key, out RequestCacheEntry cacheEntry)
		{
			return this.Lookup(key, out cacheEntry, true);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000F499 File Offset: 0x0000D699
		internal override bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream)
		{
			readStream = this.Lookup(key, out cacheEntry, false);
			return readStream != null;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		internal override Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			return this.GetWriteStream(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, true);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		internal override bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream)
		{
			writeStream = this.GetWriteStream(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, false);
			return writeStream != null;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		internal override void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.Remove()",
						SR.GetString("net_cache_access_denied", new object[] { "Write" })
					}));
				}
				return;
			}
			_WinInetCache.Entry entry = new _WinInetCache.Entry(key, int.MaxValue);
			if (_WinInetCache.Remove(entry) != _WinInetCache.Status.Success && entry.Error != _WinInetCache.Status.FileNotFound)
			{
				Win32Exception ex = new Win32Exception((int)entry.Error);
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_cannot_remove", new object[] { "WinInetCache.Remove()", key, ex.Message }));
				}
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex.Message }), ex);
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
				{
					"WinInetCache.Remove(), ",
					key,
					entry.Error.ToString()
				}));
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000F624 File Offset: 0x0000D824
		internal override bool TryRemove(string key)
		{
			return this.TryRemove(key, false);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000F630 File Offset: 0x0000D830
		internal bool TryRemove(string key, bool forceRemove)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.TryRemove()",
						SR.GetString("net_cache_access_denied", new object[] { "Write" })
					}));
				}
				return false;
			}
			_WinInetCache.Entry entry = new _WinInetCache.Entry(key, int.MaxValue);
			if (_WinInetCache.Remove(entry) == _WinInetCache.Status.Success || entry.Error == _WinInetCache.Status.FileNotFound)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
					{
						"WinInetCache.TryRemove()",
						key,
						entry.Error.ToString()
					}));
				}
				return true;
			}
			if (!forceRemove)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_key_remove_failed_status", new object[]
					{
						"WinInetCache.TryRemove()",
						key,
						entry.Error.ToString()
					}));
				}
				return false;
			}
			if (_WinInetCache.LookupInfo(entry) == _WinInetCache.Status.Success)
			{
				while (entry.Info.UseCount != 0)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
						{
							"WinInetCache.TryRemove()",
							key,
							entry.Error.ToString()
						}));
					}
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_usecount_file", new object[]
						{
							"WinInetCache.TryRemove()",
							entry.Info.UseCount,
							entry.Filename
						}));
					}
					if (!UnsafeNclNativeMethods.UnsafeWinInetCache.UnlockUrlCacheEntryFileW(key, 0))
					{
						break;
					}
					_WinInetCache.Status status = _WinInetCache.LookupInfo(entry);
				}
			}
			_WinInetCache.Remove(entry);
			if (entry.Error != _WinInetCache.Status.Success && _WinInetCache.LookupInfo(entry) == _WinInetCache.Status.FileNotFound)
			{
				entry.Error = _WinInetCache.Status.Success;
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
				{
					"WinInetCache.TryRemove()",
					key,
					entry.Error.ToString()
				}));
			}
			return entry.Error == _WinInetCache.Status.Success;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000F860 File Offset: 0x0000DA60
		internal override void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			this.UpdateInfo(key, expiresUtc, lastModifiedUtc, lastSynchronizedUtc, maxStale, entryMetadata, systemMetadata, true);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000F880 File Offset: 0x0000DA80
		internal override bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			return this.UpdateInfo(key, expiresUtc, lastModifiedUtc, lastSynchronizedUtc, maxStale, entryMetadata, systemMetadata, false);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		internal override void UnlockEntry(Stream stream)
		{
			WinInetCache.ReadStream readStream = stream as WinInetCache.ReadStream;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_stream", new object[]
				{
					"WinInetCache.UnlockEntry",
					(stream == null) ? "<null>" : stream.GetType().FullName
				}));
			}
			if (readStream == null)
			{
				return;
			}
			readStream.UnlockEntry();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000F900 File Offset: 0x0000DB00
		private unsafe Stream Lookup(string key, out RequestCacheEntry cacheEntry, bool isThrow)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, "WinInetCache.Retrieve", "key = " + key);
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			Stream stream = Stream.Null;
			SafeUnlockUrlCacheEntryFile safeUnlockUrlCacheEntryFile = null;
			_WinInetCache.Entry entry = new _WinInetCache.Entry(key, int.MaxValue);
			try
			{
				safeUnlockUrlCacheEntryFile = _WinInetCache.LookupFile(entry);
				if (entry.Error == _WinInetCache.Status.Success)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_filename", new object[] { "WinInetCache.Retrieve()", entry.Filename, entry.Error }));
					}
					cacheEntry = new RequestCacheEntry(entry, base.IsPrivateCache);
					if (entry.MetaInfo != null && entry.MetaInfo.Length != 0)
					{
						int num = 0;
						int length = entry.MetaInfo.Length;
						StringCollection stringCollection = new StringCollection();
						try
						{
							fixed (string text = entry.MetaInfo)
							{
								char* ptr = text;
								if (ptr != null)
								{
									ptr += RuntimeHelpers.OffsetToStringData / 2;
								}
								for (int i = 0; i < length; i++)
								{
									if (i == num && i + 2 < length && ptr[i] == '~' && (ptr[i + 1] == 'U' || ptr[i + 1] == 'u') && ptr[i + 2] == ':')
									{
										while (i < length && ptr[(IntPtr)(++i) * 2] != '\n')
										{
										}
										num = i + 1;
									}
									else if (i + 1 == length || ptr[i] == '\n')
									{
										string text2 = entry.MetaInfo.Substring(num, ((ptr[i - 1] == '\r') ? (i - 1) : (i + 1)) - num);
										if (text2.Length == 0 && cacheEntry.EntryMetadata == null)
										{
											cacheEntry.EntryMetadata = stringCollection;
											stringCollection = new StringCollection();
										}
										else if (cacheEntry.EntryMetadata != null && text2.StartsWith("~SPARSE_ENTRY:", StringComparison.Ordinal))
										{
											cacheEntry.IsPartialEntry = true;
										}
										else
										{
											stringCollection.Add(text2);
										}
										num = i + 1;
									}
								}
							}
						}
						finally
						{
							string text = null;
						}
						if (cacheEntry.EntryMetadata == null)
						{
							cacheEntry.EntryMetadata = stringCollection;
						}
						else
						{
							cacheEntry.SystemMetadata = stringCollection;
						}
					}
					stream = new WinInetCache.ReadStream(entry, safeUnlockUrlCacheEntryFile, this.async);
				}
				else
				{
					if (safeUnlockUrlCacheEntryFile != null)
					{
						safeUnlockUrlCacheEntryFile.Close();
					}
					cacheEntry = new RequestCacheEntry();
					cacheEntry.IsPrivateEntry = base.IsPrivateCache;
					if (entry.Error != _WinInetCache.Status.FileNotFound)
					{
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_lookup_failed", new object[]
							{
								"WinInetCache.Retrieve()",
								new Win32Exception((int)entry.Error).Message
							}));
						}
						if (Logging.On)
						{
							Logging.Exit(Logging.RequestCache, "WinInetCache.Retrieve()");
						}
						if (isThrow)
						{
							Win32Exception ex = new Win32Exception((int)entry.Error);
							throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex.Message }), ex);
						}
						return null;
					}
				}
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_exception", new object[]
					{
						"WinInetCache.Retrieve()",
						ex2.ToString()
					}));
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, "WinInetCache.Retrieve()");
				}
				if (safeUnlockUrlCacheEntryFile != null)
				{
					safeUnlockUrlCacheEntryFile.Close();
				}
				stream.Close();
				stream = Stream.Null;
				cacheEntry = new RequestCacheEntry();
				cacheEntry.IsPrivateEntry = base.IsPrivateCache;
				if (isThrow)
				{
					throw;
				}
				return null;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.RequestCache, "WinInetCache.Retrieve()", "Status = " + entry.Error.ToString());
			}
			return stream;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000FD04 File Offset: 0x0000DF04
		private string CombineMetaInfo(StringCollection entryMetadata, StringCollection systemMetadata)
		{
			if ((entryMetadata == null || entryMetadata.Count == 0) && (systemMetadata == null || systemMetadata.Count == 0))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(100);
			if (entryMetadata != null && entryMetadata.Count != 0)
			{
				for (int i = 0; i < entryMetadata.Count; i++)
				{
					if (entryMetadata[i] != null && entryMetadata[i].Length != 0)
					{
						stringBuilder.Append(entryMetadata[i]).Append("\r\n");
					}
				}
			}
			if (systemMetadata != null && systemMetadata.Count != 0)
			{
				stringBuilder.Append("\r\n");
				for (int i = 0; i < systemMetadata.Count; i++)
				{
					if (systemMetadata[i] != null && systemMetadata[i].Length != 0)
					{
						stringBuilder.Append(systemMetadata[i]).Append("\r\n");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000FDE0 File Offset: 0x0000DFE0
		private Stream GetWriteStream(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, bool isThrow)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, "WinInetCache.Store()", "Key = " + key);
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.Store()",
						SR.GetString("net_cache_access_denied", new object[] { "Write" })
					}));
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, "WinInetCache.Store");
				}
				if (isThrow)
				{
					throw new InvalidOperationException(SR.GetString("net_cache_access_denied", new object[] { "Write" }));
				}
				return null;
			}
			else
			{
				_WinInetCache.Entry entry = new _WinInetCache.Entry(key, int.MaxValue);
				entry.Key = key;
				entry.OptionalLength = ((contentLength < 0L) ? 0 : ((contentLength > 2147483647L) ? int.MaxValue : ((int)contentLength)));
				entry.Info.ExpireTime = _WinInetCache.FILETIME.Zero;
				if (expiresUtc != DateTime.MinValue && expiresUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry.Info.ExpireTime = new _WinInetCache.FILETIME(expiresUtc.ToFileTimeUtc());
				}
				entry.Info.LastModifiedTime = _WinInetCache.FILETIME.Zero;
				if (lastModifiedUtc != DateTime.MinValue && lastModifiedUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry.Info.LastModifiedTime = new _WinInetCache.FILETIME(lastModifiedUtc.ToFileTimeUtc());
				}
				entry.Info.EntryType = _WinInetCache.EntryType.NormalEntry;
				if (maxStale > TimeSpan.Zero)
				{
					if (maxStale >= WinInetCache.s_MaxTimeSpanForInt32)
					{
						maxStale = WinInetCache.s_MaxTimeSpanForInt32;
					}
					entry.Info.U.ExemptDelta = (int)maxStale.TotalSeconds;
					entry.Info.EntryType = _WinInetCache.EntryType.StickyEntry;
				}
				entry.MetaInfo = this.CombineMetaInfo(entryMetadata, systemMetadata);
				entry.FileExt = "cache";
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_expected_length", new object[] { entry.OptionalLength }));
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_last_modified", new object[] { entry.Info.LastModifiedTime.IsNull ? "0" : DateTime.FromFileTimeUtc(entry.Info.LastModifiedTime.ToLong()).ToString("r") }));
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_expires", new object[] { entry.Info.ExpireTime.IsNull ? "0" : DateTime.FromFileTimeUtc(entry.Info.ExpireTime.ToLong()).ToString("r") }));
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_max_stale", new object[] { (maxStale > TimeSpan.Zero) ? ((int)maxStale.TotalSeconds).ToString() : "n/a" }));
					if (Logging.IsVerbose(Logging.RequestCache))
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping_metadata"));
						if (entry.MetaInfo.Length == 0)
						{
							Logging.PrintInfo(Logging.RequestCache, "<null>");
						}
						else
						{
							if (entryMetadata != null)
							{
								foreach (string text in entryMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text.TrimEnd(RequestCache.LineSplits));
								}
							}
							Logging.PrintInfo(Logging.RequestCache, "------");
							if (systemMetadata != null)
							{
								foreach (string text2 in systemMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text2.TrimEnd(RequestCache.LineSplits));
								}
							}
						}
					}
				}
				_WinInetCache.CreateFileName(entry);
				Stream stream = Stream.Null;
				if (entry.Error == _WinInetCache.Status.Success)
				{
					try
					{
						stream = new WinInetCache.WriteStream(entry, isThrow, contentLength, this.async);
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_exception", new object[] { "WinInetCache.Store()", ex }));
							Logging.Exit(Logging.RequestCache, "WinInetCache.Store");
						}
						if (isThrow)
						{
							throw;
						}
						return null;
					}
					if (Logging.On)
					{
						Logging.Exit(Logging.RequestCache, "WinInetCache.Store", "Filename = " + entry.Filename);
					}
					return stream;
				}
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_create_failed", new object[] { new Win32Exception((int)entry.Error).Message }));
					Logging.Exit(Logging.RequestCache, "WinInetCache.Store");
				}
				if (isThrow)
				{
					Win32Exception ex2 = new Win32Exception((int)entry.Error);
					throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex2.Message }), ex2);
				}
				return null;
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00010338 File Offset: 0x0000E538
		private bool UpdateInfo(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, bool isThrow)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, "WinInetCache.Update", "Key = " + key);
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.Update()",
						SR.GetString("net_cache_access_denied", new object[] { "Write" })
					}));
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, "WinInetCache.Update()");
				}
				if (isThrow)
				{
					throw new InvalidOperationException(SR.GetString("net_cache_access_denied", new object[] { "Write" }));
				}
				return false;
			}
			else
			{
				_WinInetCache.Entry entry = new _WinInetCache.Entry(key, int.MaxValue);
				_WinInetCache.Entry_FC entry_FC = _WinInetCache.Entry_FC.None;
				if (expiresUtc != DateTime.MinValue && expiresUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry_FC |= _WinInetCache.Entry_FC.Exptime;
					entry.Info.ExpireTime = new _WinInetCache.FILETIME(expiresUtc.ToFileTimeUtc());
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_set_expires", new object[] { expiresUtc.ToString("r") }));
					}
				}
				if (lastModifiedUtc != DateTime.MinValue && lastModifiedUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry_FC |= _WinInetCache.Entry_FC.Modtime;
					entry.Info.LastModifiedTime = new _WinInetCache.FILETIME(lastModifiedUtc.ToFileTimeUtc());
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_set_last_modified", new object[] { lastModifiedUtc.ToString("r") }));
					}
				}
				if (lastSynchronizedUtc != DateTime.MinValue && lastSynchronizedUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry_FC |= _WinInetCache.Entry_FC.Synctime;
					entry.Info.LastSyncTime = new _WinInetCache.FILETIME(lastSynchronizedUtc.ToFileTimeUtc());
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_set_last_synchronized", new object[] { lastSynchronizedUtc.ToString("r") }));
					}
				}
				if (maxStale != TimeSpan.MinValue)
				{
					entry_FC |= _WinInetCache.Entry_FC.Attribute | _WinInetCache.Entry_FC.ExemptDelta;
					entry.Info.EntryType = _WinInetCache.EntryType.NormalEntry;
					if (maxStale >= TimeSpan.Zero)
					{
						if (maxStale >= WinInetCache.s_MaxTimeSpanForInt32)
						{
							maxStale = WinInetCache.s_MaxTimeSpanForInt32;
						}
						entry.Info.EntryType = _WinInetCache.EntryType.StickyEntry;
						entry.Info.U.ExemptDelta = (int)maxStale.TotalSeconds;
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_enable_max_stale", new object[] { ((int)maxStale.TotalSeconds).ToString() }));
						}
					}
					else
					{
						entry.Info.U.ExemptDelta = 0;
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_disable_max_stale"));
						}
					}
				}
				entry.MetaInfo = this.CombineMetaInfo(entryMetadata, systemMetadata);
				if (entry.MetaInfo.Length != 0)
				{
					entry_FC |= _WinInetCache.Entry_FC.Headerinfo;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping"));
						if (Logging.IsVerbose(Logging.RequestCache))
						{
							Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping"));
							if (entryMetadata != null)
							{
								foreach (string text in entryMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text.TrimEnd(RequestCache.LineSplits));
								}
							}
							Logging.PrintInfo(Logging.RequestCache, "------");
							if (systemMetadata != null)
							{
								foreach (string text2 in systemMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text2.TrimEnd(RequestCache.LineSplits));
								}
							}
						}
					}
				}
				_WinInetCache.Update(entry, entry_FC);
				if (entry.Error == _WinInetCache.Status.Success)
				{
					if (Logging.On)
					{
						Logging.Exit(Logging.RequestCache, "WinInetCache.Update()", "Status = " + entry.Error.ToString());
					}
					return true;
				}
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_update_failed", new object[]
					{
						"WinInetCache.Update()",
						entry.Key,
						new Win32Exception((int)entry.Error).Message
					}));
					Logging.Exit(Logging.RequestCache, "WinInetCache.Update()");
				}
				if (isThrow)
				{
					Win32Exception ex = new Win32Exception((int)entry.Error);
					throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex.Message }), ex);
				}
				return false;
			}
		}

		// Token: 0x04000390 RID: 912
		private const int _MaximumResponseHeadersLength = 2147483647;

		// Token: 0x04000391 RID: 913
		private bool async;

		// Token: 0x04000392 RID: 914
		internal const string c_SPARSE_ENTRY_HACK = "~SPARSE_ENTRY:";

		// Token: 0x04000393 RID: 915
		private static readonly DateTime s_MinDateTimeUtcForFileTimeUtc = DateTime.FromFileTimeUtc(0L);

		// Token: 0x04000394 RID: 916
		internal static readonly TimeSpan s_MaxTimeSpanForInt32 = TimeSpan.FromSeconds(2147483647.0);

		// Token: 0x020006DB RID: 1755
		private class ReadStream : FileStream, ICloseEx, IRequestLifetimeTracker
		{
			// Token: 0x0600401F RID: 16415 RVA: 0x0010D05C File Offset: 0x0010B25C
			[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
			internal ReadStream(_WinInetCache.Entry entry, SafeUnlockUrlCacheEntryFile handle, bool async)
				: base(entry.Filename, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete, 4096, async)
			{
				this.m_Key = entry.Key;
				this.m_Handle = handle;
				this.m_ReadTimeout = (this.m_WriteTimeout = -1);
			}

			// Token: 0x06004020 RID: 16416 RVA: 0x0010D0A1 File Offset: 0x0010B2A1
			internal void UnlockEntry()
			{
				this.m_Handle.Close();
			}

			// Token: 0x06004021 RID: 16417 RVA: 0x0010D0B0 File Offset: 0x0010B2B0
			public override int Read(byte[] buffer, int offset, int count)
			{
				SafeUnlockUrlCacheEntryFile handle = this.m_Handle;
				int num;
				lock (handle)
				{
					try
					{
						if (this.m_CallNesting != 0)
						{
							throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
						}
						if (this.m_Aborted)
						{
							throw ExceptionHelper.RequestAbortedException;
						}
						if (this.m_Event != null)
						{
							throw new ObjectDisposedException(base.GetType().FullName);
						}
						this.m_CallNesting = 1;
						num = base.Read(buffer, offset, count);
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							this.m_Event.Set();
						}
					}
				}
				return num;
			}

			// Token: 0x06004022 RID: 16418 RVA: 0x0010D164 File Offset: 0x0010B364
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				SafeUnlockUrlCacheEntryFile handle = this.m_Handle;
				IAsyncResult asyncResult;
				lock (handle)
				{
					if (this.m_CallNesting != 0)
					{
						throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
					}
					if (this.m_Aborted)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					if (this.m_Event != null)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
					this.m_CallNesting = 1;
					try
					{
						asyncResult = base.BeginRead(buffer, offset, count, callback, state);
					}
					catch
					{
						this.m_CallNesting = 0;
						throw;
					}
				}
				return asyncResult;
			}

			// Token: 0x06004023 RID: 16419 RVA: 0x0010D20C File Offset: 0x0010B40C
			public override int EndRead(IAsyncResult asyncResult)
			{
				SafeUnlockUrlCacheEntryFile handle = this.m_Handle;
				int num;
				lock (handle)
				{
					try
					{
						num = base.EndRead(asyncResult);
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							try
							{
								this.m_Event.Set();
							}
							catch
							{
							}
						}
					}
				}
				return num;
			}

			// Token: 0x06004024 RID: 16420 RVA: 0x0010D288 File Offset: 0x0010B488
			public void CloseEx(CloseExState closeState)
			{
				if ((closeState & CloseExState.Abort) != CloseExState.Normal)
				{
					this.m_Aborted = true;
				}
				try
				{
					this.Close();
				}
				catch
				{
					if ((closeState & CloseExState.Silent) == CloseExState.Normal)
					{
						throw;
					}
				}
			}

			// Token: 0x06004025 RID: 16421 RVA: 0x0010D2C4 File Offset: 0x0010B4C4
			protected override void Dispose(bool disposing)
			{
				if (Interlocked.Exchange(ref this.m_Disposed, 1) == 0)
				{
					if (!disposing)
					{
						base.Dispose(false);
						return;
					}
					if (this.m_Key != null)
					{
						try
						{
							SafeUnlockUrlCacheEntryFile handle = this.m_Handle;
							lock (handle)
							{
								if (this.m_CallNesting == 0)
								{
									base.Dispose(true);
								}
								else
								{
									this.m_Event = new ManualResetEvent(false);
								}
							}
							RequestLifetimeSetter.Report(this.m_RequestLifetimeSetter);
							if (this.m_Event != null)
							{
								using (this.m_Event)
								{
									this.m_Event.WaitOne();
									SafeUnlockUrlCacheEntryFile handle2 = this.m_Handle;
									lock (handle2)
									{
									}
								}
								base.Dispose(true);
							}
						}
						finally
						{
							if (Logging.On)
							{
								Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key", new object[] { "WinInetReadStream.Close()", this.m_Key }));
							}
							this.m_Handle.Close();
						}
					}
				}
			}

			// Token: 0x17000ED7 RID: 3799
			// (get) Token: 0x06004026 RID: 16422 RVA: 0x0010D3FC File Offset: 0x0010B5FC
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000ED8 RID: 3800
			// (get) Token: 0x06004027 RID: 16423 RVA: 0x0010D3FF File Offset: 0x0010B5FF
			// (set) Token: 0x06004028 RID: 16424 RVA: 0x0010D407 File Offset: 0x0010B607
			public override int ReadTimeout
			{
				get
				{
					return this.m_ReadTimeout;
				}
				set
				{
					this.m_ReadTimeout = value;
				}
			}

			// Token: 0x17000ED9 RID: 3801
			// (get) Token: 0x06004029 RID: 16425 RVA: 0x0010D410 File Offset: 0x0010B610
			// (set) Token: 0x0600402A RID: 16426 RVA: 0x0010D418 File Offset: 0x0010B618
			public override int WriteTimeout
			{
				get
				{
					return this.m_WriteTimeout;
				}
				set
				{
					this.m_WriteTimeout = value;
				}
			}

			// Token: 0x0600402B RID: 16427 RVA: 0x0010D421 File Offset: 0x0010B621
			void IRequestLifetimeTracker.TrackRequestLifetime(long requestStartTimestamp)
			{
				this.m_RequestLifetimeSetter = new RequestLifetimeSetter(requestStartTimestamp);
			}

			// Token: 0x04002FF7 RID: 12279
			private string m_Key;

			// Token: 0x04002FF8 RID: 12280
			private int m_ReadTimeout;

			// Token: 0x04002FF9 RID: 12281
			private int m_WriteTimeout;

			// Token: 0x04002FFA RID: 12282
			private SafeUnlockUrlCacheEntryFile m_Handle;

			// Token: 0x04002FFB RID: 12283
			private int m_Disposed;

			// Token: 0x04002FFC RID: 12284
			private int m_CallNesting;

			// Token: 0x04002FFD RID: 12285
			private ManualResetEvent m_Event;

			// Token: 0x04002FFE RID: 12286
			private bool m_Aborted;

			// Token: 0x04002FFF RID: 12287
			private RequestLifetimeSetter m_RequestLifetimeSetter;
		}

		// Token: 0x020006DC RID: 1756
		private class WriteStream : FileStream, ICloseEx
		{
			// Token: 0x0600402C RID: 16428 RVA: 0x0010D430 File Offset: 0x0010B630
			[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
			internal WriteStream(_WinInetCache.Entry entry, bool isThrow, long streamSize, bool async)
				: base(entry.Filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, async)
			{
				this.m_Entry = entry;
				this.m_IsThrow = isThrow;
				this.m_StreamSize = streamSize;
				this.m_OneWriteSucceeded = streamSize == 0L;
				this.m_ReadTimeout = (this.m_WriteTimeout = -1);
			}

			// Token: 0x17000EDA RID: 3802
			// (get) Token: 0x0600402D RID: 16429 RVA: 0x0010D483 File Offset: 0x0010B683
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000EDB RID: 3803
			// (get) Token: 0x0600402E RID: 16430 RVA: 0x0010D486 File Offset: 0x0010B686
			// (set) Token: 0x0600402F RID: 16431 RVA: 0x0010D48E File Offset: 0x0010B68E
			public override int ReadTimeout
			{
				get
				{
					return this.m_ReadTimeout;
				}
				set
				{
					this.m_ReadTimeout = value;
				}
			}

			// Token: 0x17000EDC RID: 3804
			// (get) Token: 0x06004030 RID: 16432 RVA: 0x0010D497 File Offset: 0x0010B697
			// (set) Token: 0x06004031 RID: 16433 RVA: 0x0010D49F File Offset: 0x0010B69F
			public override int WriteTimeout
			{
				get
				{
					return this.m_WriteTimeout;
				}
				set
				{
					this.m_WriteTimeout = value;
				}
			}

			// Token: 0x06004032 RID: 16434 RVA: 0x0010D4A8 File Offset: 0x0010B6A8
			public override void Write(byte[] buffer, int offset, int count)
			{
				_WinInetCache.Entry entry = this.m_Entry;
				lock (entry)
				{
					if (this.m_Aborted)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					if (this.m_Event != null)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
					this.m_CallNesting = 1;
					try
					{
						base.Write(buffer, offset, count);
						if (this.m_StreamSize > 0L)
						{
							this.m_StreamSize -= (long)count;
						}
						if (!this.m_OneWriteSucceeded && count != 0)
						{
							this.m_OneWriteSucceeded = true;
						}
					}
					catch
					{
						this.m_Aborted = true;
						throw;
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							this.m_Event.Set();
						}
					}
				}
			}

			// Token: 0x06004033 RID: 16435 RVA: 0x0010D584 File Offset: 0x0010B784
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				_WinInetCache.Entry entry = this.m_Entry;
				IAsyncResult asyncResult;
				lock (entry)
				{
					if (this.m_CallNesting != 0)
					{
						throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
					}
					if (this.m_Aborted)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					if (this.m_Event != null)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
					this.m_CallNesting = 1;
					try
					{
						if (this.m_StreamSize > 0L)
						{
							this.m_StreamSize -= (long)count;
						}
						asyncResult = base.BeginWrite(buffer, offset, count, callback, state);
					}
					catch
					{
						this.m_Aborted = true;
						this.m_CallNesting = 0;
						throw;
					}
				}
				return asyncResult;
			}

			// Token: 0x06004034 RID: 16436 RVA: 0x0010D64C File Offset: 0x0010B84C
			public override void EndWrite(IAsyncResult asyncResult)
			{
				_WinInetCache.Entry entry = this.m_Entry;
				lock (entry)
				{
					try
					{
						base.EndWrite(asyncResult);
						if (!this.m_OneWriteSucceeded)
						{
							this.m_OneWriteSucceeded = true;
						}
					}
					catch
					{
						this.m_Aborted = true;
						throw;
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							try
							{
								this.m_Event.Set();
							}
							catch
							{
							}
						}
					}
				}
			}

			// Token: 0x06004035 RID: 16437 RVA: 0x0010D6EC File Offset: 0x0010B8EC
			public void CloseEx(CloseExState closeState)
			{
				if ((closeState & CloseExState.Abort) != CloseExState.Normal)
				{
					this.m_Aborted = true;
				}
				try
				{
					this.Close();
				}
				catch
				{
					if ((closeState & CloseExState.Silent) == CloseExState.Normal)
					{
						throw;
					}
				}
			}

			// Token: 0x06004036 RID: 16438 RVA: 0x0010D728 File Offset: 0x0010B928
			protected override void Dispose(bool disposing)
			{
				if (Interlocked.Exchange(ref this.m_Disposed, 1) == 0 && this.m_Entry != null)
				{
					_WinInetCache.Entry entry = this.m_Entry;
					lock (entry)
					{
						if (this.m_CallNesting == 0)
						{
							base.Dispose(disposing);
						}
						else
						{
							this.m_Event = new ManualResetEvent(false);
						}
					}
					if (disposing && this.m_Event != null)
					{
						using (this.m_Event)
						{
							this.m_Event.WaitOne();
							_WinInetCache.Entry entry2 = this.m_Entry;
							lock (entry2)
							{
							}
						}
						base.Dispose(disposing);
					}
					TriState triState;
					if (this.m_StreamSize < 0L)
					{
						if (this.m_Aborted)
						{
							if (this.m_OneWriteSucceeded)
							{
								triState = TriState.Unspecified;
							}
							else
							{
								triState = TriState.False;
							}
						}
						else
						{
							triState = TriState.True;
						}
					}
					else if (!this.m_OneWriteSucceeded)
					{
						triState = TriState.False;
					}
					else if (this.m_StreamSize > 0L)
					{
						triState = TriState.Unspecified;
					}
					else
					{
						triState = TriState.True;
					}
					if (triState == TriState.False)
					{
						try
						{
							if (Logging.On)
							{
								Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_no_commit", new object[] { "WinInetWriteStream.Close()" }));
							}
							File.Delete(this.m_Entry.Filename);
						}
						catch (Exception ex)
						{
							if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
							{
								throw;
							}
							if (Logging.On)
							{
								Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_error_deleting_filename", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Filename
								}));
							}
						}
						finally
						{
							_WinInetCache.Status status = _WinInetCache.Remove(this.m_Entry);
							if (status != _WinInetCache.Status.Success && status != _WinInetCache.Status.FileNotFound && Logging.On)
							{
								Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_delete_failed", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Key,
									new Win32Exception((int)this.m_Entry.Error).Message
								}));
							}
							this.m_Entry = null;
						}
						return;
					}
					this.m_Entry.OriginalUrl = null;
					if (triState == TriState.Unspecified)
					{
						if (this.m_Entry.MetaInfo == null || this.m_Entry.MetaInfo.Length == 0 || (this.m_Entry.MetaInfo != "\r\n" && this.m_Entry.MetaInfo.IndexOf("\r\n\r\n", StringComparison.Ordinal) == -1))
						{
							this.m_Entry.MetaInfo = "\r\n~SPARSE_ENTRY:\r\n";
						}
						else
						{
							_WinInetCache.Entry entry3 = this.m_Entry;
							entry3.MetaInfo += "~SPARSE_ENTRY:\r\n";
						}
					}
					if (_WinInetCache.Commit(this.m_Entry) != _WinInetCache.Status.Success)
					{
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_commit_failed", new object[]
							{
								"WinInetWriteStream.Close()",
								this.m_Entry.Key,
								new Win32Exception((int)this.m_Entry.Error).Message
							}));
						}
						try
						{
							File.Delete(this.m_Entry.Filename);
						}
						catch (Exception ex2)
						{
							if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
							{
								throw;
							}
							if (Logging.On)
							{
								Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_error_deleting_filename", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Filename
								}));
							}
						}
						if (this.m_IsThrow)
						{
							Win32Exception ex3 = new Win32Exception((int)this.m_Entry.Error);
							throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex3.Message }), ex3);
						}
						return;
					}
					else
					{
						if (Logging.On)
						{
							if (this.m_StreamSize > 0L || (this.m_StreamSize < 0L && this.m_Aborted))
							{
								Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_committed_as_partial", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Key,
									(this.m_StreamSize > 0L) ? this.m_StreamSize.ToString(CultureInfo.CurrentCulture) : SR.GetString("net_log_unknown")
								}));
							}
							Logging.PrintInfo(Logging.RequestCache, "WinInetWriteStream.Close(), Key = " + this.m_Entry.Key + ", Commit Status = " + this.m_Entry.Error.ToString());
						}
						if ((this.m_Entry.Info.EntryType & _WinInetCache.EntryType.StickyEntry) == _WinInetCache.EntryType.StickyEntry)
						{
							if (_WinInetCache.Update(this.m_Entry, _WinInetCache.Entry_FC.ExemptDelta) != _WinInetCache.Status.Success)
							{
								if (Logging.On)
								{
									Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_update_failed", new object[]
									{
										"WinInetWriteStream.Close(), Key = " + this.m_Entry.Key,
										new Win32Exception((int)this.m_Entry.Error).Message
									}));
								}
								if (this.m_IsThrow)
								{
									Win32Exception ex4 = new Win32Exception((int)this.m_Entry.Error);
									throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[] { ex4.Message }), ex4);
								}
								return;
							}
							else if (Logging.On)
							{
								Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_max_stale_and_update_status", new object[]
								{
									"WinInetWriteFile.Close()",
									this.m_Entry.Info.U.ExemptDelta,
									this.m_Entry.Error.ToString()
								}));
							}
						}
						base.Dispose(disposing);
					}
				}
			}

			// Token: 0x04003000 RID: 12288
			private _WinInetCache.Entry m_Entry;

			// Token: 0x04003001 RID: 12289
			private bool m_IsThrow;

			// Token: 0x04003002 RID: 12290
			private long m_StreamSize;

			// Token: 0x04003003 RID: 12291
			private bool m_Aborted;

			// Token: 0x04003004 RID: 12292
			private int m_ReadTimeout;

			// Token: 0x04003005 RID: 12293
			private int m_WriteTimeout;

			// Token: 0x04003006 RID: 12294
			private int m_Disposed;

			// Token: 0x04003007 RID: 12295
			private int m_CallNesting;

			// Token: 0x04003008 RID: 12296
			private ManualResetEvent m_Event;

			// Token: 0x04003009 RID: 12297
			private bool m_OneWriteSucceeded;
		}
	}
}
