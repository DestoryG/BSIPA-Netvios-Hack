using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zlib;

namespace IPA.Logging.Printers
{
	/// <summary>
	/// A <see cref="T:IPA.Logging.LogPrinter" /> abstract class that provides the utilities to write to a GZip file.
	/// </summary>
	// Token: 0x0200003B RID: 59
	public abstract class GZFilePrinter : LogPrinter, IDisposable
	{
		// Token: 0x0600016F RID: 367
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

		/// <summary>
		/// Gets the <see cref="T:System.IO.FileInfo" /> for the file to write to.
		/// </summary>
		/// <returns>the file to write to</returns>
		// Token: 0x06000170 RID: 368
		protected abstract FileInfo GetFileInfo();

		// Token: 0x06000171 RID: 369 RVA: 0x000060E0 File Offset: 0x000042E0
		private void InitLog()
		{
			try
			{
				if (this.fileInfo == null)
				{
					this.fileInfo = this.GetFileInfo();
					string ext = this.fileInfo.Extension;
					string directoryName = this.fileInfo.DirectoryName;
					if (directoryName == null)
					{
						throw new InvalidOperationException();
					}
					FileInfo symlink = new FileInfo(Path.Combine(directoryName, string.Format("_latest{0}", ext)));
					if (symlink.Exists)
					{
						symlink.Delete();
					}
					foreach (FileInfo file in this.fileInfo.Directory.EnumerateFiles("*.log", SearchOption.TopDirectoryOnly))
					{
						if (!file.Equals(this.fileInfo) && !(file.Extension == ".gz"))
						{
							GZFilePrinter.CompressOldLog(file);
						}
					}
					this.fileInfo.Create().Close();
					try
					{
						if (!GZFilePrinter.CreateHardLink(symlink.FullName, this.fileInfo.FullName, IntPtr.Zero))
						{
							int error = Marshal.GetLastWin32Error();
							Logger.log.Error(string.Format("Hardlink creation failed ({0})", error));
						}
					}
					catch (Exception e)
					{
						Logger.log.Error("Error creating latest hardlink!");
						Logger.log.Error(e);
					}
				}
			}
			catch (Exception e2)
			{
				Logger.log.Error("Error initializing log!");
				Logger.log.Error(e2);
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006288 File Offset: 0x00004488
		private static async void CompressOldLog(FileInfo file)
		{
			Logger.log.Debug(string.Format("Compressing log file {0}", file));
			FileInfo newFile = new FileInfo(file.FullName + ".gz");
			using (FileStream istream = file.OpenRead())
			{
				using (FileStream ostream = newFile.Create())
				{
					using (GZipStream gz = new GZipStream(ostream, CompressionMode.Compress, CompressionLevel.BestCompression, false))
					{
						await istream.CopyToAsync(gz);
					}
					GZipStream gz = null;
				}
				FileStream ostream = null;
			}
			FileStream istream = null;
			file.Delete();
		}

		/// <summary>
		/// Called at the start of any print session.
		/// </summary>
		// Token: 0x06000173 RID: 371 RVA: 0x000062BF File Offset: 0x000044BF
		public sealed override void StartPrint()
		{
			this.InitLog();
			this.fstream = this.fileInfo.Open(FileMode.Append, FileAccess.Write);
			this.FileWriter = new StreamWriter(this.fstream, new UTF8Encoding(false));
		}

		/// <summary>
		/// Called at the end of any print session.
		/// </summary>
		// Token: 0x06000174 RID: 372 RVA: 0x000062F1 File Offset: 0x000044F1
		public sealed override void EndPrint()
		{
			this.FileWriter.Flush();
			this.fstream.Flush();
			this.FileWriter.Dispose();
			this.fstream.Dispose();
			this.FileWriter = null;
			this.fstream = null;
		}

		/// <inheritdoc />
		// Token: 0x06000175 RID: 373 RVA: 0x0000632D File Offset: 0x0000452D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the file printer.
		/// </summary>
		/// <param name="disposing">does nothing</param>
		// Token: 0x06000176 RID: 374 RVA: 0x0000633C File Offset: 0x0000453C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.FileWriter.Flush();
				this.fstream.Flush();
				this.FileWriter.Close();
				this.fstream.Close();
				this.FileWriter.Dispose();
				this.fstream.Dispose();
			}
		}

		// Token: 0x04000085 RID: 133
		private const RegexOptions reOptions = RegexOptions.Compiled;

		// Token: 0x04000086 RID: 134
		internal static Regex removeControlCodes = new Regex("\u001b\\[\\d+m", RegexOptions.Compiled);

		// Token: 0x04000087 RID: 135
		private FileInfo fileInfo;

		/// <summary>
		/// The <see cref="T:System.IO.StreamWriter" /> that writes to the GZip file.
		/// </summary>
		/// <value>the writer to the underlying filestream</value>
		// Token: 0x04000088 RID: 136
		protected StreamWriter FileWriter;

		// Token: 0x04000089 RID: 137
		private FileStream fstream;

		// Token: 0x0400008A RID: 138
		private const string latestFormat = "_latest{0}";
	}
}
