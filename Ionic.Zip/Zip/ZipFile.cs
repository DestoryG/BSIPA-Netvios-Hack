using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Ionic.Zlib;
using Microsoft.CSharp;

namespace Ionic.Zip
{
	// Token: 0x02000039 RID: 57
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00005")]
	public class ZipFile : IEnumerable<ZipEntry>, IEnumerable, IDisposable
	{
		// Token: 0x06000232 RID: 562 RVA: 0x0000CC9F File Offset: 0x0000AE9F
		public ZipEntry AddItem(string fileOrDirectoryName)
		{
			return this.AddItem(fileOrDirectoryName, null);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000CCA9 File Offset: 0x0000AEA9
		public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
		{
			if (File.Exists(fileOrDirectoryName))
			{
				return this.AddFile(fileOrDirectoryName, directoryPathInArchive);
			}
			if (Directory.Exists(fileOrDirectoryName))
			{
				return this.AddDirectory(fileOrDirectoryName, directoryPathInArchive);
			}
			throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", fileOrDirectoryName));
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000CCDD File Offset: 0x0000AEDD
		public ZipEntry AddFile(string fileName)
		{
			return this.AddFile(fileName, null);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		public ZipEntry AddFile(string fileName, string directoryPathInArchive)
		{
			string text = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			ZipEntry zipEntry = ZipEntry.CreateFromFile(fileName, text);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000CD28 File Offset: 0x0000AF28
		public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (ZipEntry zipEntry in entriesToRemove)
			{
				this.RemoveEntry(zipEntry);
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000CD80 File Offset: 0x0000AF80
		public void RemoveEntries(ICollection<string> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (string text in entriesToRemove)
			{
				this.RemoveEntry(text);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
		public void AddFiles(IEnumerable<string> fileNames)
		{
			this.AddFiles(fileNames, null);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000CDE2 File Offset: 0x0000AFE2
		public void UpdateFiles(IEnumerable<string> fileNames)
		{
			this.UpdateFiles(fileNames, null);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000CDEC File Offset: 0x0000AFEC
		public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			this.AddFiles(fileNames, false, directoryPathInArchive);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		public void AddFiles(IEnumerable<string> fileNames, bool preserveDirHierarchy, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this._addOperationCanceled = false;
			this.OnAddStarted();
			if (preserveDirHierarchy)
			{
				using (IEnumerator<string> enumerator = fileNames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (this._addOperationCanceled)
						{
							break;
						}
						if (directoryPathInArchive != null)
						{
							string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(text)));
							this.AddFile(text, fullPath);
						}
						else
						{
							this.AddFile(text, null);
						}
					}
					goto IL_00AD;
				}
			}
			foreach (string text2 in fileNames)
			{
				if (this._addOperationCanceled)
				{
					break;
				}
				this.AddFile(text2, directoryPathInArchive);
			}
			IL_00AD:
			if (!this._addOperationCanceled)
			{
				this.OnAddCompleted();
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000CEDC File Offset: 0x0000B0DC
		public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this.OnAddStarted();
			foreach (string text in fileNames)
			{
				this.UpdateFile(text, directoryPathInArchive);
			}
			this.OnAddCompleted();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000CF40 File Offset: 0x0000B140
		public ZipEntry UpdateFile(string fileName)
		{
			return this.UpdateFile(fileName, null);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000CF4C File Offset: 0x0000B14C
		public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
		{
			string text = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			if (this[text] != null)
			{
				this.RemoveEntry(text);
			}
			return this.AddFile(fileName, directoryPathInArchive);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000CF79 File Offset: 0x0000B179
		public ZipEntry UpdateDirectory(string directoryName)
		{
			return this.UpdateDirectory(directoryName, null);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000CF83 File Offset: 0x0000B183
		public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000CF8E File Offset: 0x0000B18E
		public void UpdateItem(string itemName)
		{
			this.UpdateItem(itemName, null);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000CF98 File Offset: 0x0000B198
		public void UpdateItem(string itemName, string directoryPathInArchive)
		{
			if (File.Exists(itemName))
			{
				this.UpdateFile(itemName, directoryPathInArchive);
				return;
			}
			if (Directory.Exists(itemName))
			{
				this.UpdateDirectory(itemName, directoryPathInArchive);
				return;
			}
			throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", itemName));
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000CFCE File Offset: 0x0000B1CE
		public ZipEntry AddEntry(string entryName, string content)
		{
			return this.AddEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
		public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, encoding);
			streamWriter.Write(content);
			streamWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return this.AddEntry(entryName, memoryStream);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D01C File Offset: 0x0000B21C
		public ZipEntry AddEntry(string entryName, Stream stream)
		{
			ZipEntry zipEntry = ZipEntry.CreateForStream(entryName, stream);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000D068 File Offset: 0x0000B268
		public ZipEntry AddEntry(string entryName, WriteDelegate writer)
		{
			ZipEntry zipEntry = ZipEntry.CreateForWriter(entryName, writer);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
		public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			ZipEntry zipEntry = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000D0EC File Offset: 0x0000B2EC
		private ZipEntry _InternalAddEntry(ZipEntry ze)
		{
			ze._container = new ZipContainer(this);
			ze.CompressionMethod = this.CompressionMethod;
			ze.CompressionLevel = this.CompressionLevel;
			ze.ExtractExistingFile = this.ExtractExistingFile;
			ze.ZipErrorAction = this.ZipErrorAction;
			ze.SetCompression = this.SetCompression;
			ze.AlternateEncoding = this.AlternateEncoding;
			ze.AlternateEncodingUsage = this.AlternateEncodingUsage;
			ze.Password = this._Password;
			ze.Encryption = this.Encryption;
			ze.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			ze.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			this.InternalAddEntry(ze.FileName, ze);
			this.AfterAddEntry(ze);
			return ze;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000D19E File Offset: 0x0000B39E
		public ZipEntry UpdateEntry(string entryName, string content)
		{
			return this.UpdateEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000D1AD File Offset: 0x0000B3AD
		public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, content, encoding);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000D1BF File Offset: 0x0000B3BF
		public ZipEntry UpdateEntry(string entryName, WriteDelegate writer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, writer);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		public ZipEntry UpdateEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, opener, closer);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public ZipEntry UpdateEntry(string entryName, Stream stream)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000D1F4 File Offset: 0x0000B3F4
		private void RemoveEntryForUpdate(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			string text = null;
			if (entryName.IndexOf('\\') != -1)
			{
				text = Path.GetDirectoryName(entryName);
				entryName = Path.GetFileName(entryName);
			}
			string text2 = ZipEntry.NameInArchive(entryName, text);
			if (this[text2] != null)
			{
				this.RemoveEntry(text2);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000D248 File Offset: 0x0000B448
		public ZipEntry AddEntry(string entryName, byte[] byteContent)
		{
			if (byteContent == null)
			{
				throw new ArgumentException("bad argument", "byteContent");
			}
			MemoryStream memoryStream = new MemoryStream(byteContent);
			return this.AddEntry(entryName, memoryStream);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D277 File Offset: 0x0000B477
		public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, byteContent);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D288 File Offset: 0x0000B488
		public ZipEntry AddDirectory(string directoryName)
		{
			return this.AddDirectory(directoryName, null);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D292 File Offset: 0x0000B492
		public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public ZipEntry AddDirectoryByName(string directoryNameInArchive)
		{
			ZipEntry zipEntry = ZipEntry.CreateFromNothing(directoryNameInArchive);
			zipEntry._container = new ZipContainer(this);
			zipEntry.MarkAsDirectory();
			zipEntry.AlternateEncoding = this.AlternateEncoding;
			zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			zipEntry._Source = ZipEntrySource.Stream;
			this.InternalAddEntry(zipEntry.FileName, zipEntry);
			this.AfterAddEntry(zipEntry);
			return zipEntry;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000D327 File Offset: 0x0000B527
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
		{
			if (rootDirectoryPathInArchive == null)
			{
				rootDirectoryPathInArchive = "";
			}
			return this.AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, true, 0);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000D33E File Offset: 0x0000B53E
		internal void InternalAddEntry(string name, ZipEntry entry)
		{
			this._entries.Add(name, entry);
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000D35C File Offset: 0x0000B55C
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, bool recurse, int level)
		{
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("{0} {1}...", (action == AddOrUpdateAction.AddOnly) ? "adding" : "Adding or updating", directoryName);
			}
			if (level == 0)
			{
				this._addOperationCanceled = false;
				this.OnAddStarted();
			}
			if (this._addOperationCanceled)
			{
				return null;
			}
			string text = rootDirectoryPathInArchive;
			ZipEntry zipEntry = null;
			if (level > 0)
			{
				int num = directoryName.Length;
				for (int i = level; i > 0; i--)
				{
					num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
				}
				text = directoryName.Substring(num + 1);
				text = Path.Combine(rootDirectoryPathInArchive, text);
			}
			if (level > 0 || rootDirectoryPathInArchive != "")
			{
				zipEntry = ZipEntry.CreateFromFile(directoryName, text);
				zipEntry._container = new ZipContainer(this);
				zipEntry.AlternateEncoding = this.AlternateEncoding;
				zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
				zipEntry.MarkAsDirectory();
				zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
				zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
				if (!this._entries.ContainsKey(zipEntry.FileName))
				{
					this.InternalAddEntry(zipEntry.FileName, zipEntry);
					this.AfterAddEntry(zipEntry);
				}
				text = zipEntry.FileName;
			}
			if (!this._addOperationCanceled)
			{
				string[] files = Directory.GetFiles(directoryName);
				if (recurse)
				{
					foreach (string text2 in files)
					{
						if (this._addOperationCanceled)
						{
							break;
						}
						if (action == AddOrUpdateAction.AddOnly)
						{
							this.AddFile(text2, text);
						}
						else
						{
							this.UpdateFile(text2, text);
						}
					}
					if (!this._addOperationCanceled)
					{
						string[] directories = Directory.GetDirectories(directoryName);
						foreach (string text3 in directories)
						{
							FileAttributes attributes = File.GetAttributes(text3);
							if (this.AddDirectoryWillTraverseReparsePoints || (attributes & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								this.AddOrUpdateDirectoryImpl(text3, rootDirectoryPathInArchive, action, recurse, level + 1);
							}
						}
					}
				}
			}
			if (level == 0)
			{
				this.OnAddCompleted();
			}
			return zipEntry;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D53A File Offset: 0x0000B73A
		public static bool CheckZip(string zipFileName)
		{
			return ZipFile.CheckZip(zipFileName, false, null);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D544 File Offset: 0x0000B744
		public static bool CheckZip(string zipFileName, bool fixIfNecessary, TextWriter writer)
		{
			ZipFile zipFile = null;
			ZipFile zipFile2 = null;
			bool flag = true;
			try
			{
				zipFile = new ZipFile();
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile2 = ZipFile.Read(zipFileName);
				foreach (ZipEntry zipEntry in zipFile)
				{
					foreach (ZipEntry zipEntry2 in zipFile2)
					{
						if (zipEntry.FileName == zipEntry2.FileName)
						{
							if (zipEntry._RelativeOffsetOfLocalHeader != zipEntry2._RelativeOffsetOfLocalHeader)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in RelativeOffsetOfLocalHeader  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._RelativeOffsetOfLocalHeader, zipEntry2._RelativeOffsetOfLocalHeader);
								}
							}
							if (zipEntry._CompressedSize != zipEntry2._CompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._CompressedSize, zipEntry2._CompressedSize);
								}
							}
							if (zipEntry._UncompressedSize != zipEntry2._UncompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in UncompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._UncompressedSize, zipEntry2._UncompressedSize);
								}
							}
							if (zipEntry.CompressionMethod != zipEntry2.CompressionMethod)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressionMethod  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.CompressionMethod, zipEntry2.CompressionMethod);
								}
							}
							if (zipEntry.Crc == zipEntry2.Crc)
							{
								break;
							}
							flag = false;
							if (writer != null)
							{
								writer.WriteLine("{0}: mismatch in Crc32  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.Crc, zipEntry2.Crc);
								break;
							}
							break;
						}
					}
				}
				zipFile2.Dispose();
				zipFile2 = null;
				if (!flag && fixIfNecessary)
				{
					string text = Path.GetFileNameWithoutExtension(zipFileName);
					text = string.Format("{0}_fixed.zip", text);
					zipFile.Save(text);
				}
			}
			finally
			{
				if (zipFile != null)
				{
					zipFile.Dispose();
				}
				if (zipFile2 != null)
				{
					zipFile2.Dispose();
				}
			}
			return flag;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		public static void FixZipDirectory(string zipFileName)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile.Save(zipFileName);
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D7E8 File Offset: 0x0000B9E8
		public static bool CheckZipPassword(string zipFileName, string password)
		{
			bool flag = false;
			try
			{
				using (ZipFile zipFile = ZipFile.Read(zipFileName))
				{
					foreach (ZipEntry zipEntry in zipFile)
					{
						if (!zipEntry.IsDirectory && zipEntry.UsesEncryption)
						{
							zipEntry.ExtractWithPassword(Stream.Null, password);
						}
					}
				}
				flag = true;
			}
			catch (BadPasswordException)
			{
			}
			return flag;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000D87C File Offset: 0x0000BA7C
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipFile: {0}\n", this.Name));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("          Comment: {0}\n", this._Comment));
				}
				if (this._versionMadeBy != 0)
				{
					stringBuilder.Append(string.Format("  version made by: 0x{0:X4}\n", this._versionMadeBy));
				}
				if (this._versionNeededToExtract != 0)
				{
					stringBuilder.Append(string.Format("needed to extract: 0x{0:X4}\n", this._versionNeededToExtract));
				}
				stringBuilder.Append(string.Format("       uses ZIP64: {0}\n", this.InputUsesZip64));
				stringBuilder.Append(string.Format("     disk with CD: {0}\n", this._diskNumberWithCd));
				if (this._OffsetOfCentralDirectory == 4294967295U)
				{
					stringBuilder.Append(string.Format("      CD64 offset: 0x{0:X16}\n", this._OffsetOfCentralDirectory64));
				}
				else
				{
					stringBuilder.Append(string.Format("        CD offset: 0x{0:X8}\n", this._OffsetOfCentralDirectory));
				}
				stringBuilder.Append("\n");
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					stringBuilder.Append(zipEntry.Info);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000D9F0 File Offset: 0x0000BBF0
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000D9F8 File Offset: 0x0000BBF8
		public bool FullScan { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000DA01 File Offset: 0x0000BC01
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000DA09 File Offset: 0x0000BC09
		public bool SortEntriesBeforeSaving { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000DA12 File Offset: 0x0000BC12
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000DA1A File Offset: 0x0000BC1A
		public bool AddDirectoryWillTraverseReparsePoints { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000DA23 File Offset: 0x0000BC23
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000DA2B File Offset: 0x0000BC2B
		public int BufferSize
		{
			get
			{
				return this._BufferSize;
			}
			set
			{
				this._BufferSize = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000DA34 File Offset: 0x0000BC34
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		public int CodecBufferSize { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000DA45 File Offset: 0x0000BC45
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000DA4D File Offset: 0x0000BC4D
		public bool FlattenFoldersOnExtract { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000DA56 File Offset: 0x0000BC56
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000DA5E File Offset: 0x0000BC5E
		public CompressionStrategy Strategy
		{
			get
			{
				return this._Strategy;
			}
			set
			{
				this._Strategy = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000DA67 File Offset: 0x0000BC67
		// (set) Token: 0x0600026B RID: 619 RVA: 0x0000DA6F File Offset: 0x0000BC6F
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000DA78 File Offset: 0x0000BC78
		// (set) Token: 0x0600026D RID: 621 RVA: 0x0000DA80 File Offset: 0x0000BC80
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000DA89 File Offset: 0x0000BC89
		// (set) Token: 0x0600026F RID: 623 RVA: 0x0000DA91 File Offset: 0x0000BC91
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this._compressionMethod;
			}
			set
			{
				this._compressionMethod = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000DA9A File Offset: 0x0000BC9A
		// (set) Token: 0x06000271 RID: 625 RVA: 0x0000DAA2 File Offset: 0x0000BCA2
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._contentsChanged = true;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000DAB2 File Offset: 0x0000BCB2
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000DABA File Offset: 0x0000BCBA
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000DAC3 File Offset: 0x0000BCC3
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0000DACB File Offset: 0x0000BCCB
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000DAD4 File Offset: 0x0000BCD4
		internal bool Verbose
		{
			get
			{
				return this._StatusMessageTextWriter != null;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000DAE2 File Offset: 0x0000BCE2
		public bool ContainsEntry(string name)
		{
			return this._entries.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000DAF5 File Offset: 0x0000BCF5
		// (set) Token: 0x06000279 RID: 633 RVA: 0x0000DAFD File Offset: 0x0000BCFD
		public bool CaseSensitiveRetrieval
		{
			get
			{
				return this._CaseSensitiveRetrieval;
			}
			set
			{
				if (value != this._CaseSensitiveRetrieval)
				{
					this._CaseSensitiveRetrieval = value;
					this._initEntriesDictionary();
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000DB15 File Offset: 0x0000BD15
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0000DB34 File Offset: 0x0000BD34
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.GetEncoding("UTF-8") && this._alternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.GetEncoding("UTF-8");
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipFile.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000DB63 File Offset: 0x0000BD63
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000DB6B File Offset: 0x0000BD6B
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				return this._zip64;
			}
			set
			{
				this._zip64 = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000DB74 File Offset: 0x0000BD74
		public bool? RequiresZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				if (!this._hasBeenSaved || this._contentsChanged)
				{
					return null;
				}
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					if (zipEntry.RequiresZip64.Value)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000DC24 File Offset: 0x0000BE24
		public bool? InputUsesZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				foreach (ZipEntry zipEntry in this)
				{
					if (zipEntry.Source != ZipEntrySource.ZipFile)
					{
						return null;
					}
					if (zipEntry._InputUsesZip64)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000DCAC File Offset: 0x0000BEAC
		// (set) Token: 0x06000282 RID: 642 RVA: 0x0000DCBF File Offset: 0x0000BEBF
		[Obsolete("use AlternateEncoding instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000DCCF File Offset: 0x0000BECF
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000DCD7 File Offset: 0x0000BED7
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000DCE8 File Offset: 0x0000BEE8
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000DCF1 File Offset: 0x0000BEF1
		public static Encoding DefaultEncoding
		{
			get
			{
				return ZipFile._defaultEncoding;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000DCF8 File Offset: 0x0000BEF8
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000DD00 File Offset: 0x0000BF00
		public TextWriter StatusMessageTextWriter
		{
			get
			{
				return this._StatusMessageTextWriter;
			}
			set
			{
				this._StatusMessageTextWriter = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000DD09 File Offset: 0x0000BF09
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000DD11 File Offset: 0x0000BF11
		public string TempFileFolder
		{
			get
			{
				return this._TempFileFolder;
			}
			set
			{
				this._TempFileFolder = value;
				if (value == null)
				{
					return;
				}
				if (!Directory.Exists(value))
				{
					throw new FileNotFoundException(string.Format("That directory ({0}) does not exist.", value));
				}
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000DD5F File Offset: 0x0000BF5F
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000DD37 File Offset: 0x0000BF37
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password == null)
				{
					this.Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this.Encryption == EncryptionAlgorithm.None)
				{
					this.Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000DD67 File Offset: 0x0000BF67
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000DD6F File Offset: 0x0000BF6F
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000DD78 File Offset: 0x0000BF78
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000DD8F File Offset: 0x0000BF8F
		public ZipErrorAction ZipErrorAction
		{
			get
			{
				if (this.ZipError != null)
				{
					this._zipErrorAction = ZipErrorAction.InvokeErrorEvent;
				}
				return this._zipErrorAction;
			}
			set
			{
				this._zipErrorAction = value;
				if (this._zipErrorAction != ZipErrorAction.InvokeErrorEvent && this.ZipError != null)
				{
					this.ZipError = null;
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000DDD0 File Offset: 0x0000BFD0
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		public SetCompressionCallback SetCompression { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000DDE1 File Offset: 0x0000BFE1
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000DDE9 File Offset: 0x0000BFE9
		public int MaxOutputSegmentSize
		{
			get
			{
				return this._maxOutputSegmentSize;
			}
			set
			{
				if (value < 65536 && value != 0)
				{
					throw new ZipException("The minimum acceptable segment size is 65536.");
				}
				this._maxOutputSegmentSize = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000DE08 File Offset: 0x0000C008
		public int NumberOfSegmentsForMostRecentSave
		{
			get
			{
				return (int)(this._numberOfSegmentsForMostRecentSave + 1U);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000DE39 File Offset: 0x0000C039
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000DE12 File Offset: 0x0000C012
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateThreshold should be -1, 0, or > 65536");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000DE41 File Offset: 0x0000C041
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000DE49 File Offset: 0x0000C049
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000DE66 File Offset: 0x0000C066
		public override string ToString()
		{
			return string.Format("ZipFile::{0}", this.Name);
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000DE78 File Offset: 0x0000C078
		public static Version LibraryVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000DE89 File Offset: 0x0000C089
		internal void NotifyEntryChanged()
		{
			this._contentsChanged = true;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000DE92 File Offset: 0x0000C092
		internal Stream StreamForDiskNumber(uint diskNumber)
		{
			if (diskNumber + 1U == this._diskNumberWithCd || (diskNumber == 0U && this._diskNumberWithCd == 0U))
			{
				return this.ReadStream;
			}
			return ZipSegmentedStream.ForReading(this._readName ?? this._name, diskNumber, this._diskNumberWithCd);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000DED0 File Offset: 0x0000C0D0
		internal void Reset(bool whileSaving)
		{
			if (this._JustSaved)
			{
				using (ZipFile zipFile = new ZipFile())
				{
					zipFile._readName = (zipFile._name = (whileSaving ? (this._readName ?? this._name) : this._name));
					zipFile.AlternateEncoding = this.AlternateEncoding;
					zipFile.AlternateEncodingUsage = this.AlternateEncodingUsage;
					ZipFile.ReadIntoInstance(zipFile);
					foreach (ZipEntry zipEntry in zipFile)
					{
						foreach (ZipEntry zipEntry2 in this)
						{
							if (zipEntry.FileName == zipEntry2.FileName)
							{
								zipEntry2.CopyMetaData(zipEntry);
								break;
							}
						}
					}
				}
				this._JustSaved = false;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000DFE0 File Offset: 0x0000C1E0
		public ZipFile(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception ex)
			{
				throw new ZipException(string.Format("Could not read {0} as a zip file", fileName), ex);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000E074 File Offset: 0x0000C274
		public ZipFile(string fileName, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, null);
			}
			catch (Exception ex)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), ex);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000E118 File Offset: 0x0000C318
		public ZipFile()
		{
			this._InitInstance(null, null);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000E188 File Offset: 0x0000C388
		public ZipFile(Encoding encoding)
		{
			this.AlternateEncoding = encoding;
			this.AlternateEncodingUsage = ZipOption.Always;
			this._InitInstance(null, null);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000E208 File Offset: 0x0000C408
		public ZipFile(string fileName, TextWriter statusMessageWriter)
		{
			try
			{
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception ex)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), ex);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000E29C File Offset: 0x0000C49C
		public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception ex)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), ex);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000E340 File Offset: 0x0000C540
		public void Initialize(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception ex)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), ex);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000E37C File Offset: 0x0000C57C
		private void _initEntriesDictionary()
		{
			StringComparer stringComparer = (this.CaseSensitiveRetrieval ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
			this._entries = ((this._entries == null) ? new Dictionary<string, ZipEntry>(stringComparer) : new Dictionary<string, ZipEntry>(this._entries, stringComparer));
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
		{
			this._name = zipFileName;
			this._StatusMessageTextWriter = statusMessageWriter;
			this._contentsChanged = true;
			this.AddDirectoryWillTraverseReparsePoints = true;
			this.CompressionLevel = CompressionLevel.Default;
			this.ParallelDeflateThreshold = 524288L;
			this._initEntriesDictionary();
			if (File.Exists(this._name))
			{
				if (this.FullScan)
				{
					ZipFile.ReadIntoInstance_Orig(this);
				}
				else
				{
					ZipFile.ReadIntoInstance(this);
				}
				this._fileAlreadyExists = true;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000E42C File Offset: 0x0000C62C
		private List<ZipEntry> ZipEntriesAsList
		{
			get
			{
				if (this._zipEntriesAsList == null)
				{
					this._zipEntriesAsList = new List<ZipEntry>(this._entries.Values);
				}
				return this._zipEntriesAsList;
			}
		}

		// Token: 0x170000C4 RID: 196
		public ZipEntry this[int ix]
		{
			get
			{
				return this.ZipEntriesAsList[ix];
			}
		}

		// Token: 0x170000C5 RID: 197
		public ZipEntry this[string fileName]
		{
			get
			{
				string text = SharedUtilities.NormalizePathForUseInZipFile(fileName);
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				text = text.Replace("/", "\\");
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				return null;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		public ICollection<string> EntryFileNames
		{
			get
			{
				return this._entries.Keys;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000E4C9 File Offset: 0x0000C6C9
		public ICollection<ZipEntry> Entries
		{
			get
			{
				return this._entries.Values;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000E4F8 File Offset: 0x0000C6F8
		public ICollection<ZipEntry> EntriesSorted
		{
			get
			{
				List<ZipEntry> list = new List<ZipEntry>();
				foreach (ZipEntry zipEntry in this.Entries)
				{
					list.Add(zipEntry);
				}
				StringComparison sc = (this.CaseSensitiveRetrieval ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
				list.Sort((ZipEntry x, ZipEntry y) => string.Compare(x.FileName, y.FileName, sc));
				return list.AsReadOnly();
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000E57C File Offset: 0x0000C77C
		public int Count
		{
			get
			{
				return this._entries.Count;
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E589 File Offset: 0x0000C789
		public void RemoveEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this._entries.Remove(SharedUtilities.NormalizePathForUseInZipFile(entry.FileName));
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
		public void RemoveEntry(string fileName)
		{
			string text = ZipEntry.NameInArchive(fileName, null);
			ZipEntry zipEntry = this[text];
			if (zipEntry == null)
			{
				throw new ArgumentException("The entry you specified was not found in the zip archive.");
			}
			this.RemoveEntry(zipEntry);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E5F2 File Offset: 0x0000C7F2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E604 File Offset: 0x0000C804
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (!this._disposed)
			{
				if (disposeManagedResources)
				{
					if (this._ReadStreamIsOurs && this._readstream != null)
					{
						this._readstream.Dispose();
						this._readstream = null;
					}
					if (this._temporaryFileName != null && this._name != null && this._writestream != null)
					{
						this._writestream.Dispose();
						this._writestream = null;
					}
					if (this.ParallelDeflater != null)
					{
						this.ParallelDeflater.Dispose();
						this.ParallelDeflater = null;
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000E68C File Offset: 0x0000C88C
		internal Stream ReadStream
		{
			get
			{
				if (this._readstream == null && (this._readName != null || this._name != null))
				{
					this._readstream = File.Open(this._readName ?? this._name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					this._ReadStreamIsOurs = true;
				}
				return this._readstream;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000E759 File Offset: 0x0000C959
		private Stream WriteStream
		{
			get
			{
				if (this._writestream != null)
				{
					return this._writestream;
				}
				if (this._name == null)
				{
					return this._writestream;
				}
				if (this._maxOutputSegmentSize != 0)
				{
					this._writestream = ZipSegmentedStream.ForWriting(this._name, this._maxOutputSegmentSize);
					return this._writestream;
				}
				SharedUtilities.CreateAndOpenUniqueTempFile(this.TempFileFolder ?? Path.GetDirectoryName(this._name), out this._writestream, out this._temporaryFileName);
				return this._writestream;
			}
			set
			{
				if (value != null)
				{
					throw new ZipException("Cannot set the stream to a non-null value.");
				}
				this._writestream = null;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000E770 File Offset: 0x0000C970
		private string ArchiveNameForEvent
		{
			get
			{
				if (this._name == null)
				{
					return "(stream)";
				}
				return this._name;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002BA RID: 698 RVA: 0x0000E788 File Offset: 0x0000C988
		// (remove) Token: 0x060002BB RID: 699 RVA: 0x0000E7C0 File Offset: 0x0000C9C0
		public event EventHandler<SaveProgressEventArgs> SaveProgress;

		// Token: 0x060002BC RID: 700 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E83C File Offset: 0x0000CA3C
		private void OnSaveEntry(int current, ZipEntry entry, bool before)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, entry);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E884 File Offset: 0x0000CA84
		private void OnSaveEvent(ZipProgressEventType eventFlavor)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, eventFlavor);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
		private void OnSaveStarted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Started(this.ArchiveNameForEvent);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E8FC File Offset: 0x0000CAFC
		private void OnSaveCompleted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Completed(this.ArchiveNameForEvent);
				saveProgress(this, saveProgressEventArgs);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002C1 RID: 705 RVA: 0x0000E928 File Offset: 0x0000CB28
		// (remove) Token: 0x060002C2 RID: 706 RVA: 0x0000E960 File Offset: 0x0000CB60
		public event EventHandler<ReadProgressEventArgs> ReadProgress;

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E998 File Offset: 0x0000CB98
		private void OnReadStarted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = ReadProgressEventArgs.Started(this.ArchiveNameForEvent);
				readProgress(this, readProgressEventArgs);
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		private void OnReadCompleted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = ReadProgressEventArgs.Completed(this.ArchiveNameForEvent);
				readProgress(this, readProgressEventArgs);
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		internal void OnReadBytes(ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = ReadProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, this.ReadStream.Position, this.LengthOfReadStream);
				readProgress(this, readProgressEventArgs);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000EA30 File Offset: 0x0000CC30
		internal void OnReadEntry(bool before, ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = (before ? ReadProgressEventArgs.Before(this.ArchiveNameForEvent, this._entries.Count) : ReadProgressEventArgs.After(this.ArchiveNameForEvent, entry, this._entries.Count));
				readProgress(this, readProgressEventArgs);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000EA82 File Offset: 0x0000CC82
		private long LengthOfReadStream
		{
			get
			{
				if (this._lengthOfReadStream == -99L)
				{
					this._lengthOfReadStream = (this._ReadStreamIsOurs ? SharedUtilities.GetFileLength(this._name) : (-1L));
				}
				return this._lengthOfReadStream;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002C8 RID: 712 RVA: 0x0000EAB4 File Offset: 0x0000CCB4
		// (remove) Token: 0x060002C9 RID: 713 RVA: 0x0000EAEC File Offset: 0x0000CCEC
		public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

		// Token: 0x060002CA RID: 714 RVA: 0x0000EB24 File Offset: 0x0000CD24
		private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = new ExtractProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, currentEntry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000EB70 File Offset: 0x0000CD70
		internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = (before ? ExtractProgressEventArgs.BeforeExtractEntry(this.ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.AfterExtractEntry(this.ArchiveNameForEvent, entry, path));
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EC08 File Offset: 0x0000CE08
		internal bool OnExtractExisting(ZipEntry entry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractExisting(this.ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EC4C File Offset: 0x0000CE4C
		private void OnExtractAllCompleted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractAllCompleted(this.ArchiveNameForEvent, path);
				extractProgress(this, extractProgressEventArgs);
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EC78 File Offset: 0x0000CE78
		private void OnExtractAllStarted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractAllStarted(this.ArchiveNameForEvent, path);
				extractProgress(this, extractProgressEventArgs);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002D0 RID: 720 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		// (remove) Token: 0x060002D1 RID: 721 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public event EventHandler<AddProgressEventArgs> AddProgress;

		// Token: 0x060002D2 RID: 722 RVA: 0x0000ED14 File Offset: 0x0000CF14
		private void OnAddStarted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Started(this.ArchiveNameForEvent);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000ED50 File Offset: 0x0000CF50
		private void OnAddCompleted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Completed(this.ArchiveNameForEvent);
				addProgress(this, addProgressEventArgs);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000ED7C File Offset: 0x0000CF7C
		internal void AfterAddEntry(ZipEntry entry)
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.AfterEntry(this.ArchiveNameForEvent, entry, this._entries.Count);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060002D5 RID: 725 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		// (remove) Token: 0x060002D6 RID: 726 RVA: 0x0000EDFC File Offset: 0x0000CFFC
		public event EventHandler<ZipErrorEventArgs> ZipError;

		// Token: 0x060002D7 RID: 727 RVA: 0x0000EE34 File Offset: 0x0000D034
		internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
		{
			if (this.ZipError != null)
			{
				lock (this.LOCK)
				{
					ZipErrorEventArgs zipErrorEventArgs = ZipErrorEventArgs.Saving(this.Name, entry, exc);
					this.ZipError(this, zipErrorEventArgs);
					if (zipErrorEventArgs.Cancel)
					{
						this._saveOperationCanceled = true;
					}
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		public void ExtractAll(string path)
		{
			this._InternalExtractAll(path, true);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000EEAA File Offset: 0x0000D0AA
		public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this._InternalExtractAll(path, true);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000EEBC File Offset: 0x0000D0BC
		private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
		{
			bool flag = this.Verbose;
			this._inExtractAll = true;
			try
			{
				this.OnExtractAllStarted(path);
				int num = 0;
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					if (flag)
					{
						this.StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", new object[] { "Name", "Modified", "Size", "Ratio", "Packed" });
						this.StatusMessageTextWriter.WriteLine(new string('-', 72));
						flag = false;
					}
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", new object[]
						{
							zipEntry.FileName,
							zipEntry.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
							zipEntry.UncompressedSize,
							zipEntry.CompressionRatio,
							zipEntry.CompressedSize
						});
						if (!string.IsNullOrEmpty(zipEntry.Comment))
						{
							this.StatusMessageTextWriter.WriteLine("  Comment: {0}", zipEntry.Comment);
						}
					}
					zipEntry.Password = this._Password;
					this.OnExtractEntry(num, true, zipEntry, path);
					if (overrideExtractExistingProperty)
					{
						zipEntry.ExtractExistingFile = this.ExtractExistingFile;
					}
					zipEntry.Extract(path);
					num++;
					this.OnExtractEntry(num, false, zipEntry, path);
					if (this._extractOperationCanceled)
					{
						break;
					}
				}
				if (!this._extractOperationCanceled)
				{
					foreach (ZipEntry zipEntry2 in this._entries.Values)
					{
						if (zipEntry2.IsDirectory || zipEntry2.FileName.EndsWith("/"))
						{
							string text = (zipEntry2.FileName.StartsWith("/") ? Path.Combine(path, zipEntry2.FileName.Substring(1)) : Path.Combine(path, zipEntry2.FileName));
							zipEntry2._SetTimes(text, false);
						}
					}
					this.OnExtractAllCompleted(path);
				}
			}
			finally
			{
				this._inExtractAll = false;
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F148 File Offset: 0x0000D348
		public static ZipFile Read(string fileName)
		{
			return ZipFile.Read(fileName, null, null, null);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F153 File Offset: 0x0000D353
		public static ZipFile Read(string fileName, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(fileName, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F17C File Offset: 0x0000D37C
		private static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			ZipFile zipFile = new ZipFile();
			zipFile.AlternateEncoding = encoding ?? ZipFile.DefaultEncoding;
			zipFile.AlternateEncodingUsage = ZipOption.Always;
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._name = fileName;
			if (readProgress != null)
			{
				zipFile.ReadProgress = readProgress;
			}
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from {0}...", fileName);
			}
			ZipFile.ReadIntoInstance(zipFile);
			zipFile._fileAlreadyExists = true;
			return zipFile;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F1E5 File Offset: 0x0000D3E5
		public static ZipFile Read(Stream zipStream)
		{
			return ZipFile.Read(zipStream, null, null, null);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F1F0 File Offset: 0x0000D3F0
		public static ZipFile Read(Stream zipStream, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(zipStream, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F218 File Offset: 0x0000D418
		private static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			if (zipStream == null)
			{
				throw new ArgumentNullException("zipStream");
			}
			ZipFile zipFile = new ZipFile();
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._alternateEncoding = encoding ?? ZipFile.DefaultEncoding;
			zipFile._alternateEncodingUsage = ZipOption.Always;
			if (readProgress != null)
			{
				zipFile.ReadProgress += readProgress;
			}
			zipFile._readstream = ((zipStream.Position == 0L) ? zipStream : new OffsetStream(zipStream));
			zipFile._ReadStreamIsOurs = false;
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from stream...");
			}
			ZipFile.ReadIntoInstance(zipFile);
			return zipFile;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F2A0 File Offset: 0x0000D4A0
		private static void ReadIntoInstance(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			try
			{
				zf._readName = zf._name;
				if (!readStream.CanSeek)
				{
					ZipFile.ReadIntoInstance_Orig(zf);
					return;
				}
				zf.OnReadStarted();
				uint num = ZipFile.ReadFirstFourBytes(readStream);
				if (num == 101010256U)
				{
					return;
				}
				int num2 = 0;
				bool flag = false;
				long num3 = readStream.Length - 64L;
				long num4 = Math.Max(readStream.Length - 16384L, 10L);
				do
				{
					if (num3 < 0L)
					{
						num3 = 0L;
					}
					readStream.Seek(num3, SeekOrigin.Begin);
					long num5 = SharedUtilities.FindSignature(readStream, 101010256);
					if (num5 != -1L)
					{
						flag = true;
					}
					else
					{
						if (num3 == 0L)
						{
							break;
						}
						num2++;
						num3 -= (long)(32 * (num2 + 1) * num2);
					}
				}
				while (!flag && num3 > num4);
				if (flag)
				{
					zf._locEndOfCDS = readStream.Position - 4L;
					byte[] array = new byte[16];
					readStream.Read(array, 0, array.Length);
					zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
					if (zf._diskNumberWithCd == 65535U)
					{
						throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
					}
					zf._diskNumberWithCd += 1U;
					int num6 = 12;
					uint num7 = BitConverter.ToUInt32(array, num6);
					if (num7 == 4294967295U)
					{
						ZipFile.Zip64SeekToCentralDirectory(zf);
					}
					else
					{
						zf._OffsetOfCentralDirectory = num7;
						readStream.Seek((long)((ulong)num7), SeekOrigin.Begin);
					}
					ZipFile.ReadCentralDirectory(zf);
				}
				else
				{
					readStream.Seek(0L, SeekOrigin.Begin);
					ZipFile.ReadIntoInstance_Orig(zf);
				}
			}
			catch (Exception ex)
			{
				if (zf._ReadStreamIsOurs && zf._readstream != null)
				{
					zf._readstream.Dispose();
					zf._readstream = null;
				}
				throw new ZipException("Cannot read that as a ZipFile", ex);
			}
			zf._contentsChanged = false;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F460 File Offset: 0x0000D660
		private static void Zip64SeekToCentralDirectory(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			byte[] array = new byte[16];
			readStream.Seek(-40L, SeekOrigin.Current);
			readStream.Read(array, 0, 16);
			long num = BitConverter.ToInt64(array, 8);
			zf._OffsetOfCentralDirectory = uint.MaxValue;
			zf._OffsetOfCentralDirectory64 = num;
			readStream.Seek(num, SeekOrigin.Begin);
			uint num2 = (uint)SharedUtilities.ReadInt(readStream);
			if (num2 != 101075792U)
			{
				throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) looking for ZIP64 EoCD Record at position 0x{1:X8}", num2, readStream.Position));
			}
			readStream.Read(array, 0, 8);
			long num3 = BitConverter.ToInt64(array, 0);
			array = new byte[num3];
			readStream.Read(array, 0, array.Length);
			num = BitConverter.ToInt64(array, 36);
			readStream.Seek(num, SeekOrigin.Begin);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F51C File Offset: 0x0000D71C
		private static uint ReadFirstFourBytes(Stream s)
		{
			return (uint)SharedUtilities.ReadInt(s);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F534 File Offset: 0x0000D734
		private static void ReadCentralDirectory(ZipFile zf)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
			{
				zipEntry.ResetDirEntry();
				zf.OnReadEntry(true, null);
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("entry {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				if (zipEntry._InputUsesZip64)
				{
					flag = true;
				}
				dictionary.Add(zipEntry.FileName, null);
			}
			if (flag)
			{
				zf.UseZip64WhenSaving = Zip64Option.Always;
			}
			if (zf._locEndOfCDS > 0L)
			{
				zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
			}
			ZipFile.ReadCentralDirectoryFooter(zf);
			if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
			{
				zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
			}
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", zf._entries.Count);
			}
			zf.OnReadCompleted();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F634 File Offset: 0x0000D834
		private static void ReadIntoInstance_Orig(ZipFile zf)
		{
			zf.OnReadStarted();
			zf._entries = new Dictionary<string, ZipEntry>();
			if (zf.Verbose)
			{
				if (zf.Name == null)
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
				}
				else
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
				}
			}
			bool flag = true;
			ZipContainer zipContainer = new ZipContainer(zf);
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadEntry(zipContainer, flag)) != null)
			{
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("  {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				flag = false;
			}
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				ZipEntry zipEntry2;
				while ((zipEntry2 = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
				{
					ZipEntry zipEntry3 = zf._entries[zipEntry2.FileName];
					if (zipEntry3 != null)
					{
						zipEntry3._Comment = zipEntry2.Comment;
						if (zipEntry2.IsDirectory)
						{
							zipEntry3.MarkAsDirectory();
						}
					}
					dictionary.Add(zipEntry2.FileName, null);
				}
				if (zf._locEndOfCDS > 0L)
				{
					zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
				}
				ZipFile.ReadCentralDirectoryFooter(zf);
				if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
				{
					zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
				}
			}
			catch (ZipException)
			{
			}
			catch (IOException)
			{
			}
			zf.OnReadCompleted();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F7A0 File Offset: 0x0000D9A0
		private static void ReadCentralDirectoryFooter(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			int num = SharedUtilities.ReadSignature(readStream);
			int num2 = 0;
			byte[] array;
			if ((long)num == 101075792L)
			{
				array = new byte[52];
				readStream.Read(array, 0, array.Length);
				long num3 = BitConverter.ToInt64(array, 0);
				if (num3 < 44L)
				{
					throw new ZipException("Bad size in the ZIP64 Central Directory.");
				}
				zf._versionMadeBy = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._versionNeededToExtract = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._diskNumberWithCd = BitConverter.ToUInt32(array, num2);
				num2 += 2;
				array = new byte[num3 - 44L];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
				if ((long)num != 117853008L)
				{
					throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
				}
				array = new byte[16];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
			}
			if ((long)num != 101010256L)
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				throw new BadReadException(string.Format("Bad signature ({0:X8}) at position 0x{1:X8}", num, readStream.Position));
			}
			array = new byte[16];
			zf.ReadStream.Read(array, 0, array.Length);
			if (zf._diskNumberWithCd == 0U)
			{
				zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
			}
			ZipFile.ReadZipFileComment(zf);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F8E8 File Offset: 0x0000DAE8
		private static void ReadZipFileComment(ZipFile zf)
		{
			byte[] array = new byte[2];
			zf.ReadStream.Read(array, 0, array.Length);
			short num = (short)((int)array[0] + (int)array[1] * 256);
			if (num > 0)
			{
				array = new byte[(int)num];
				zf.ReadStream.Read(array, 0, array.Length);
				string @string = zf.AlternateEncoding.GetString(array, 0, array.Length);
				zf.Comment = @string;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F950 File Offset: 0x0000DB50
		public static bool IsZipFile(string fileName)
		{
			return ZipFile.IsZipFile(fileName, false);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F95C File Offset: 0x0000DB5C
		public static bool IsZipFile(string fileName, bool testExtract)
		{
			bool flag = false;
			try
			{
				if (!File.Exists(fileName))
				{
					return false;
				}
				using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					flag = ZipFile.IsZipFile(fileStream, testExtract);
				}
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return flag;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
		public static bool IsZipFile(Stream stream, bool testExtract)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool flag = false;
			try
			{
				if (!stream.CanRead)
				{
					return false;
				}
				Stream @null = Stream.Null;
				using (ZipFile zipFile = ZipFile.Read(stream, null, null, null))
				{
					if (testExtract)
					{
						foreach (ZipEntry zipEntry in zipFile)
						{
							if (!zipEntry.IsDirectory)
							{
								zipEntry.Extract(@null);
							}
						}
					}
				}
				flag = true;
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return flag;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000FA8C File Offset: 0x0000DC8C
		private void DeleteFileWithRetry(string filename)
		{
			bool flag = false;
			int num = 3;
			int num2 = 0;
			while (num2 < num && !flag)
			{
				try
				{
					File.Delete(filename);
					flag = true;
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("************************************************** Retry delete.");
					Thread.Sleep(200 + num2 * 200);
				}
				num2++;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		public void Save()
		{
			try
			{
				bool flag = false;
				this._saveOperationCanceled = false;
				this._numberOfSegmentsForMostRecentSave = 0U;
				this.OnSaveStarted();
				if (this.WriteStream == null)
				{
					throw new BadStateException("You haven't specified where to save the zip.");
				}
				if (this._name != null && this._name.EndsWith(".exe") && !this._SavingSfx)
				{
					throw new BadStateException("You specified an EXE for a plain zip file.");
				}
				if (!this._contentsChanged)
				{
					this.OnSaveCompleted();
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("No save is necessary....");
					}
				}
				else
				{
					this.Reset(true);
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("saving....");
					}
					if (this._entries.Count >= 65535 && this._zip64 == Zip64Option.Default)
					{
						throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
					}
					int num = 0;
					ICollection<ZipEntry> collection = (this.SortEntriesBeforeSaving ? this.EntriesSorted : this.Entries);
					foreach (ZipEntry zipEntry in collection)
					{
						this.OnSaveEntry(num, zipEntry, true);
						zipEntry.Write(this.WriteStream);
						if (this._saveOperationCanceled)
						{
							break;
						}
						num++;
						this.OnSaveEntry(num, zipEntry, false);
						if (this._saveOperationCanceled)
						{
							break;
						}
						if (zipEntry.IncludedInMostRecentSave)
						{
							flag |= zipEntry.OutputUsedZip64.Value;
						}
					}
					if (!this._saveOperationCanceled)
					{
						ZipSegmentedStream zipSegmentedStream = this.WriteStream as ZipSegmentedStream;
						this._numberOfSegmentsForMostRecentSave = ((zipSegmentedStream != null) ? zipSegmentedStream.CurrentSegment : 1U);
						bool flag2 = ZipOutput.WriteCentralDirectoryStructure(this.WriteStream, collection, this._numberOfSegmentsForMostRecentSave, this._zip64, this.Comment, new ZipContainer(this));
						this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
						this._hasBeenSaved = true;
						this._contentsChanged = false;
						flag = flag || flag2;
						this._OutputUsesZip64 = new bool?(flag);
						if (this._name != null && (this._temporaryFileName != null || zipSegmentedStream != null))
						{
							this.WriteStream.Dispose();
							if (this._saveOperationCanceled)
							{
								return;
							}
							if (this._fileAlreadyExists && this._readstream != null)
							{
								this._readstream.Close();
								this._readstream = null;
								foreach (ZipEntry zipEntry2 in collection)
								{
									ZipSegmentedStream zipSegmentedStream2 = zipEntry2._archiveStream as ZipSegmentedStream;
									if (zipSegmentedStream2 != null)
									{
										zipSegmentedStream2.Dispose();
									}
									zipEntry2._archiveStream = null;
								}
							}
							string text = null;
							if (File.Exists(this._name))
							{
								text = this._name + "." + Path.GetRandomFileName();
								if (File.Exists(text))
								{
									this.DeleteFileWithRetry(text);
								}
								File.Move(this._name, text);
							}
							this.OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
							File.Move((zipSegmentedStream != null) ? zipSegmentedStream.CurrentTempName : this._temporaryFileName, this._name);
							this.OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
							if (text != null)
							{
								try
								{
									if (File.Exists(text))
									{
										File.Delete(text);
									}
								}
								catch
								{
								}
							}
							this._fileAlreadyExists = true;
						}
						ZipFile.NotifyEntriesSaveComplete(collection);
						this.OnSaveCompleted();
						this._JustSaved = true;
					}
				}
			}
			finally
			{
				this.CleanupAfterSaveOperation();
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000FE78 File Offset: 0x0000E078
		private static void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
		{
			foreach (ZipEntry zipEntry in c)
			{
				zipEntry.NotifySaveComplete();
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		private void RemoveTempFile()
		{
			try
			{
				if (File.Exists(this._temporaryFileName))
				{
					File.Delete(this._temporaryFileName);
				}
			}
			catch (IOException ex)
			{
				if (this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", ex.Message);
				}
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000FF18 File Offset: 0x0000E118
		private void CleanupAfterSaveOperation()
		{
			if (this._name != null)
			{
				if (this._writestream != null)
				{
					try
					{
						this._writestream.Dispose();
					}
					catch (IOException)
					{
					}
				}
				this._writestream = null;
				if (this._temporaryFileName != null)
				{
					this.RemoveTempFile();
					this._temporaryFileName = null;
				}
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FF74 File Offset: 0x0000E174
		public void Save(string fileName)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			else
			{
				this._readName = this._name;
			}
			this._name = fileName;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "fileName"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this.Save();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FFEC File Offset: 0x0000E1EC
		public void Save(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (!outputStream.CanWrite)
			{
				throw new ArgumentException("Must be a writable stream.", "outputStream");
			}
			this._name = null;
			this._writestream = new CountingStream(outputStream);
			this._contentsChanged = true;
			this._fileAlreadyExists = false;
			this.Save();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00010046 File Offset: 0x0000E246
		public void AddSelectedFiles(string selectionCriteria)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, false);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00010056 File Offset: 0x0000E256
		public void AddSelectedFiles(string selectionCriteria, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, recurseDirectories);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00010066 File Offset: 0x0000E266
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, false);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00010072 File Offset: 0x0000E272
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, recurseDirectories);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0001007E File Offset: 0x0000E27E
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, false);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0001008A File Offset: 0x0000E28A
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, false);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010098 File Offset: 0x0000E298
		public void UpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, true);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000100A6 File Offset: 0x0000E2A6
		private string EnsureendInSlash(string s)
		{
			if (s.EndsWith("\\"))
			{
				return s;
			}
			return s + "\\";
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000100C4 File Offset: 0x0000E2C4
		private void _AddOrUpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories, bool wantUpdate)
		{
			if (directoryOnDisk == null && Directory.Exists(selectionCriteria))
			{
				directoryOnDisk = selectionCriteria;
				selectionCriteria = "*.*";
			}
			else if (string.IsNullOrEmpty(directoryOnDisk))
			{
				directoryOnDisk = ".";
			}
			while (directoryOnDisk.EndsWith("\\"))
			{
				directoryOnDisk = directoryOnDisk.Substring(0, directoryOnDisk.Length - 1);
			}
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding selection '{0}' from dir '{1}'...", selectionCriteria, directoryOnDisk);
			}
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			ReadOnlyCollection<string> readOnlyCollection = fileSelector.SelectFiles(directoryOnDisk, recurseDirectories);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("found {0} files...", readOnlyCollection.Count);
			}
			this.OnAddStarted();
			AddOrUpdateAction addOrUpdateAction = (wantUpdate ? AddOrUpdateAction.AddOrUpdate : AddOrUpdateAction.AddOnly);
			foreach (string text in readOnlyCollection)
			{
				string text2 = ((directoryPathInArchive == null) ? null : ZipFile.ReplaceLeadingDirectory(Path.GetDirectoryName(text), directoryOnDisk, directoryPathInArchive));
				if (File.Exists(text))
				{
					if (wantUpdate)
					{
						this.UpdateFile(text, text2);
					}
					else
					{
						this.AddFile(text, text2);
					}
				}
				else
				{
					this.AddOrUpdateDirectoryImpl(text, text2, addOrUpdateAction, false, 0);
				}
			}
			this.OnAddCompleted();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00010200 File Offset: 0x0000E400
		private static string ReplaceLeadingDirectory(string original, string pattern, string replacement)
		{
			string text = original.ToUpper();
			string text2 = pattern.ToUpper();
			int num = text.IndexOf(text2);
			if (num != 0)
			{
				return original;
			}
			return replacement + original.Substring(text2.Length);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001023C File Offset: 0x0000E43C
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00010260 File Offset: 0x0000E460
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria, string directoryPathInArchive)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this, directoryPathInArchive);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00010284 File Offset: 0x0000E484
		public int RemoveSelectedEntries(string selectionCriteria)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x000102A8 File Offset: 0x0000E4A8
		public int RemoveSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria, directoryPathInArchive);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000102CC File Offset: 0x0000E4CC
		public void ExtractSelectedEntries(string selectionCriteria)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00010328 File Offset: 0x0000E528
		public void ExtractSelectedEntries(string selectionCriteria, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractExistingFile);
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010384 File Offset: 0x0000E584
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000103E0 File Offset: 0x0000E5E0
		public void ExtractSelectedEntries(string selectionCriteria, string directoryInArchive, string extractDirectory)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0001043C File Offset: 0x0000E63C
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive, string extractDirectory, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory, extractExistingFile);
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0001049C File Offset: 0x0000E69C
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorFlavor flavor)
		{
			this.SaveSelfExtractor(exeToGenerate, new SelfExtractorSaveOptions
			{
				Flavor = flavor
			});
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000104C0 File Offset: 0x0000E6C0
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			this._SavingSfx = true;
			this._name = exeToGenerate;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "exeToGenerate"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this._SaveSfxStub(exeToGenerate, options);
			this.Save();
			this._SavingSfx = false;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010540 File Offset: 0x0000E740
		private static void ExtractResourceToFile(Assembly a, string resourceName, string filename)
		{
			byte[] array = new byte[1024];
			using (Stream manifestResourceStream = a.GetManifestResourceStream(resourceName))
			{
				if (manifestResourceStream == null)
				{
					throw new ZipException(string.Format("missing resource '{0}'", resourceName));
				}
				using (FileStream fileStream = File.OpenWrite(filename))
				{
					int num;
					do
					{
						num = manifestResourceStream.Read(array, 0, array.Length);
						fileStream.Write(array, 0, num);
					}
					while (num > 0);
				}
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000105CC File Offset: 0x0000E7CC
		private void _SaveSfxStub(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			try
			{
				if (File.Exists(exeToGenerate) && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("The existing file ({0}) will be overwritten.", exeToGenerate);
				}
				if (!exeToGenerate.EndsWith(".exe") && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("Warning: The generated self-extracting file will not have an .exe extension.");
				}
				text3 = this.TempFileFolder ?? Path.GetDirectoryName(exeToGenerate);
				text = ZipFile.GenerateTempPathname(text3, "exe");
				Assembly assembly = typeof(ZipFile).Assembly;
				using (CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider())
				{
					ZipFile.ExtractorSettings extractorSettings = null;
					foreach (ZipFile.ExtractorSettings extractorSettings2 in ZipFile.SettingsList)
					{
						if (extractorSettings2.Flavor == options.Flavor)
						{
							extractorSettings = extractorSettings2;
							break;
						}
					}
					if (extractorSettings == null)
					{
						throw new BadStateException(string.Format("While saving a Self-Extracting Zip, Cannot find that flavor ({0})?", options.Flavor));
					}
					CompilerParameters compilerParameters = new CompilerParameters();
					compilerParameters.ReferencedAssemblies.Add(assembly.Location);
					if (extractorSettings.ReferencedAssemblies != null)
					{
						foreach (string text4 in extractorSettings.ReferencedAssemblies)
						{
							compilerParameters.ReferencedAssemblies.Add(text4);
						}
					}
					compilerParameters.GenerateInMemory = false;
					compilerParameters.GenerateExecutable = true;
					compilerParameters.IncludeDebugInformation = false;
					compilerParameters.CompilerOptions = "";
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
					StringBuilder stringBuilder = new StringBuilder();
					string text5 = ZipFile.GenerateTempPathname(text3, "cs");
					using (ZipFile zipFile = ZipFile.Read(executingAssembly.GetManifestResourceStream("Ionic.Zip.Resources.ZippedResources.zip")))
					{
						text2 = ZipFile.GenerateTempPathname(text3, "tmp");
						if (string.IsNullOrEmpty(options.IconFile))
						{
							Directory.CreateDirectory(text2);
							ZipEntry zipEntry = zipFile["zippedFile.ico"];
							if ((zipEntry.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
							{
								zipEntry.Attributes ^= FileAttributes.ReadOnly;
							}
							zipEntry.Extract(text2);
							string text6 = Path.Combine(text2, "zippedFile.ico");
							CompilerParameters compilerParameters2 = compilerParameters;
							compilerParameters2.CompilerOptions += string.Format("/win32icon:\"{0}\"", text6);
						}
						else
						{
							CompilerParameters compilerParameters3 = compilerParameters;
							compilerParameters3.CompilerOptions += string.Format("/win32icon:\"{0}\"", options.IconFile);
						}
						compilerParameters.OutputAssembly = text;
						if (options.Flavor == SelfExtractorFlavor.WinFormsApplication)
						{
							CompilerParameters compilerParameters4 = compilerParameters;
							compilerParameters4.CompilerOptions += " /target:winexe";
						}
						if (!string.IsNullOrEmpty(options.AdditionalCompilerSwitches))
						{
							CompilerParameters compilerParameters5 = compilerParameters;
							compilerParameters5.CompilerOptions = compilerParameters5.CompilerOptions + " " + options.AdditionalCompilerSwitches;
						}
						if (string.IsNullOrEmpty(compilerParameters.CompilerOptions))
						{
							compilerParameters.CompilerOptions = null;
						}
						if (extractorSettings.CopyThroughResources != null && extractorSettings.CopyThroughResources.Count != 0)
						{
							if (!Directory.Exists(text2))
							{
								Directory.CreateDirectory(text2);
							}
							foreach (string text7 in extractorSettings.CopyThroughResources)
							{
								string text8 = Path.Combine(text2, text7);
								ZipFile.ExtractResourceToFile(executingAssembly, text7, text8);
								compilerParameters.EmbeddedResources.Add(text8);
							}
						}
						compilerParameters.EmbeddedResources.Add(assembly.Location);
						stringBuilder.Append("// " + Path.GetFileName(text5) + "\n").Append("// --------------------------------------------\n//\n").Append("// This SFX source file was generated by DotNetZip ")
							.Append(ZipFile.LibraryVersion.ToString())
							.Append("\n//         at ")
							.Append(DateTime.Now.ToString("yyyy MMMM dd  HH:mm:ss"))
							.Append("\n//\n// --------------------------------------------\n\n\n");
						if (!string.IsNullOrEmpty(options.Description))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"" + options.Description.Replace("\"", "") + "\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"DotNetZip SFX Archive\")]\n");
						}
						if (!string.IsNullOrEmpty(options.ProductVersion))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyInformationalVersion(\"" + options.ProductVersion.Replace("\"", "") + "\")]\n");
						}
						string text9 = (string.IsNullOrEmpty(options.Copyright) ? "Extractor: Copyright © Dino Chiesa 2008-2011" : options.Copyright.Replace("\"", ""));
						if (!string.IsNullOrEmpty(options.ProductName))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"").Append(options.ProductName.Replace("\"", "")).Append("\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"DotNetZip\")]\n");
						}
						stringBuilder.Append("[assembly: System.Reflection.AssemblyCopyright(\"" + text9 + "\")]\n").Append(string.Format("[assembly: System.Reflection.AssemblyVersion(\"{0}\")]\n", ZipFile.LibraryVersion.ToString()));
						if (options.FileVersion != null)
						{
							stringBuilder.Append(string.Format("[assembly: System.Reflection.AssemblyFileVersion(\"{0}\")]\n", options.FileVersion.ToString()));
						}
						stringBuilder.Append("\n\n\n");
						string text10 = options.DefaultExtractDirectory;
						if (text10 != null)
						{
							text10 = text10.Replace("\"", "").Replace("\\", "\\\\");
						}
						string text11 = options.PostExtractCommandLine;
						if (text11 != null)
						{
							text11 = text11.Replace("\\", "\\\\");
							text11 = text11.Replace("\"", "\\\"");
						}
						foreach (string text12 in extractorSettings.ResourcesToCompile)
						{
							using (Stream stream = zipFile[text12].OpenReader())
							{
								if (stream == null)
								{
									throw new ZipException(string.Format("missing resource '{0}'", text12));
								}
								using (StreamReader streamReader = new StreamReader(stream))
								{
									while (streamReader.Peek() >= 0)
									{
										string text13 = streamReader.ReadLine();
										if (text10 != null)
										{
											text13 = text13.Replace("@@EXTRACTLOCATION", text10);
										}
										text13 = text13.Replace("@@REMOVE_AFTER_EXECUTE", options.RemoveUnpackedFilesAfterExecute.ToString());
										text13 = text13.Replace("@@QUIET", options.Quiet.ToString());
										if (!string.IsNullOrEmpty(options.SfxExeWindowTitle))
										{
											text13 = text13.Replace("@@SFX_EXE_WINDOW_TITLE", options.SfxExeWindowTitle);
										}
										text13 = text13.Replace("@@EXTRACT_EXISTING_FILE", ((int)options.ExtractExistingFile).ToString());
										if (text11 != null)
										{
											text13 = text13.Replace("@@POST_UNPACK_CMD_LINE", text11);
										}
										stringBuilder.Append(text13).Append("\n");
									}
								}
								stringBuilder.Append("\n\n");
							}
						}
					}
					string text14 = stringBuilder.ToString();
					CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, new string[] { text14 });
					if (compilerResults == null)
					{
						throw new SfxGenerationException("Cannot compile the extraction logic!");
					}
					if (this.Verbose)
					{
						foreach (string text15 in compilerResults.Output)
						{
							this.StatusMessageTextWriter.WriteLine(text15);
						}
					}
					if (compilerResults.Errors.Count != 0)
					{
						using (TextWriter textWriter = new StreamWriter(text5))
						{
							textWriter.Write(text14);
							textWriter.Write("\n\n\n// ------------------------------------------------------------------\n");
							textWriter.Write("// Errors during compilation: \n//\n");
							string fileName = Path.GetFileName(text5);
							foreach (object obj in compilerResults.Errors)
							{
								CompilerError compilerError = (CompilerError)obj;
								textWriter.Write(string.Format("//   {0}({1},{2}): {3} {4}: {5}\n//\n", new object[]
								{
									fileName,
									compilerError.Line,
									compilerError.Column,
									compilerError.IsWarning ? "Warning" : "error",
									compilerError.ErrorNumber,
									compilerError.ErrorText
								}));
							}
						}
						throw new SfxGenerationException(string.Format("Errors compiling the extraction logic!  {0}", text5));
					}
					this.OnSaveEvent(ZipProgressEventType.Saving_AfterCompileSelfExtractor);
					using (Stream stream2 = File.OpenRead(text))
					{
						byte[] array = new byte[4000];
						int num = 1;
						while (num != 0)
						{
							num = stream2.Read(array, 0, array.Length);
							if (num != 0)
							{
								this.WriteStream.Write(array, 0, num);
							}
						}
					}
				}
				this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
			}
			finally
			{
				try
				{
					if (Directory.Exists(text2))
					{
						try
						{
							Directory.Delete(text2, true);
						}
						catch (IOException ex)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", ex);
						}
					}
					if (File.Exists(text))
					{
						try
						{
							File.Delete(text);
						}
						catch (IOException ex2)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", ex2);
						}
					}
				}
				catch (IOException)
				{
				}
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00011064 File Offset: 0x0000F264
		internal static string GenerateTempPathname(string dir, string extension)
		{
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			string text3;
			do
			{
				string text = Guid.NewGuid().ToString();
				string text2 = string.Format("{0}-{1}-{2}.{3}", new object[]
				{
					name,
					DateTime.Now.ToString("yyyyMMMdd-HHmmss"),
					text,
					extension
				});
				text3 = Path.Combine(dir, text2);
			}
			while (File.Exists(text3) || Directory.Exists(text3));
			return text3;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00011224 File Offset: 0x0000F424
		public IEnumerator<ZipEntry> GetEnumerator()
		{
			foreach (ZipEntry e in this._entries.Values)
			{
				yield return e;
			}
			yield break;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00011240 File Offset: 0x0000F440
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00011248 File Offset: 0x0000F448
		[DispId(-4)]
		public IEnumerator GetNewEnum()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000145 RID: 325
		private TextWriter _StatusMessageTextWriter;

		// Token: 0x04000146 RID: 326
		private bool _CaseSensitiveRetrieval;

		// Token: 0x04000147 RID: 327
		private Stream _readstream;

		// Token: 0x04000148 RID: 328
		private Stream _writestream;

		// Token: 0x04000149 RID: 329
		private ushort _versionMadeBy;

		// Token: 0x0400014A RID: 330
		private ushort _versionNeededToExtract;

		// Token: 0x0400014B RID: 331
		private uint _diskNumberWithCd;

		// Token: 0x0400014C RID: 332
		private int _maxOutputSegmentSize;

		// Token: 0x0400014D RID: 333
		private uint _numberOfSegmentsForMostRecentSave;

		// Token: 0x0400014E RID: 334
		private ZipErrorAction _zipErrorAction;

		// Token: 0x0400014F RID: 335
		private bool _disposed;

		// Token: 0x04000150 RID: 336
		private Dictionary<string, ZipEntry> _entries;

		// Token: 0x04000151 RID: 337
		private List<ZipEntry> _zipEntriesAsList;

		// Token: 0x04000152 RID: 338
		private string _name;

		// Token: 0x04000153 RID: 339
		private string _readName;

		// Token: 0x04000154 RID: 340
		private string _Comment;

		// Token: 0x04000155 RID: 341
		internal string _Password;

		// Token: 0x04000156 RID: 342
		private bool _emitNtfsTimes = true;

		// Token: 0x04000157 RID: 343
		private bool _emitUnixTimes;

		// Token: 0x04000158 RID: 344
		private CompressionStrategy _Strategy;

		// Token: 0x04000159 RID: 345
		private CompressionMethod _compressionMethod = CompressionMethod.Deflate;

		// Token: 0x0400015A RID: 346
		private bool _fileAlreadyExists;

		// Token: 0x0400015B RID: 347
		private string _temporaryFileName;

		// Token: 0x0400015C RID: 348
		private bool _contentsChanged;

		// Token: 0x0400015D RID: 349
		private bool _hasBeenSaved;

		// Token: 0x0400015E RID: 350
		private string _TempFileFolder;

		// Token: 0x0400015F RID: 351
		private bool _ReadStreamIsOurs = true;

		// Token: 0x04000160 RID: 352
		private object LOCK = new object();

		// Token: 0x04000161 RID: 353
		private bool _saveOperationCanceled;

		// Token: 0x04000162 RID: 354
		private bool _extractOperationCanceled;

		// Token: 0x04000163 RID: 355
		private bool _addOperationCanceled;

		// Token: 0x04000164 RID: 356
		private EncryptionAlgorithm _Encryption;

		// Token: 0x04000165 RID: 357
		private bool _JustSaved;

		// Token: 0x04000166 RID: 358
		private long _locEndOfCDS = -1L;

		// Token: 0x04000167 RID: 359
		private uint _OffsetOfCentralDirectory;

		// Token: 0x04000168 RID: 360
		private long _OffsetOfCentralDirectory64;

		// Token: 0x04000169 RID: 361
		private bool? _OutputUsesZip64;

		// Token: 0x0400016A RID: 362
		internal bool _inExtractAll;

		// Token: 0x0400016B RID: 363
		private Encoding _alternateEncoding = Encoding.GetEncoding("IBM437");

		// Token: 0x0400016C RID: 364
		private ZipOption _alternateEncodingUsage;

		// Token: 0x0400016D RID: 365
		private static Encoding _defaultEncoding = Encoding.GetEncoding("IBM437");

		// Token: 0x0400016E RID: 366
		private int _BufferSize = ZipFile.BufferSizeDefault;

		// Token: 0x0400016F RID: 367
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x04000170 RID: 368
		private long _ParallelDeflateThreshold;

		// Token: 0x04000171 RID: 369
		private int _maxBufferPairs = 16;

		// Token: 0x04000172 RID: 370
		internal Zip64Option _zip64;

		// Token: 0x04000173 RID: 371
		private bool _SavingSfx;

		// Token: 0x04000174 RID: 372
		public static readonly int BufferSizeDefault = 32768;

		// Token: 0x04000177 RID: 375
		private long _lengthOfReadStream = -99L;

		// Token: 0x0400017B RID: 379
		private static ZipFile.ExtractorSettings[] SettingsList = new ZipFile.ExtractorSettings[]
		{
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.WinFormsApplication,
				ReferencedAssemblies = new List<string> { "System.dll", "System.Windows.Forms.dll", "System.Drawing.dll" },
				CopyThroughResources = new List<string> { "Ionic.Zip.WinFormsSelfExtractorStub.resources", "Ionic.Zip.Forms.PasswordDialog.resources", "Ionic.Zip.Forms.ZipContentsDialog.resources" },
				ResourcesToCompile = new List<string> { "WinFormsSelfExtractorStub.cs", "WinFormsSelfExtractorStub.Designer.cs", "PasswordDialog.cs", "PasswordDialog.Designer.cs", "ZipContentsDialog.cs", "ZipContentsDialog.Designer.cs", "FolderBrowserDialogEx.cs" }
			},
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.ConsoleApplication,
				ReferencedAssemblies = new List<string> { "System.dll" },
				CopyThroughResources = null,
				ResourcesToCompile = new List<string> { "CommandLineSelfExtractorStub.cs" }
			}
		};

		// Token: 0x0200003A RID: 58
		private class ExtractorSettings
		{
			// Token: 0x04000184 RID: 388
			public SelfExtractorFlavor Flavor;

			// Token: 0x04000185 RID: 389
			public List<string> ReferencedAssemblies;

			// Token: 0x04000186 RID: 390
			public List<string> CopyThroughResources;

			// Token: 0x04000187 RID: 391
			public List<string> ResourcesToCompile;
		}
	}
}
