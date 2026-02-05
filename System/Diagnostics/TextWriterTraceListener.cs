using System;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020004AC RID: 1196
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class TextWriterTraceListener : TraceListener
	{
		// Token: 0x06002C40 RID: 11328 RVA: 0x000C7728 File Offset: 0x000C5928
		public TextWriterTraceListener()
		{
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000C7730 File Offset: 0x000C5930
		public TextWriterTraceListener(Stream stream)
			: this(stream, string.Empty)
		{
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000C773E File Offset: 0x000C593E
		public TextWriterTraceListener(Stream stream, string name)
			: base(name)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.writer = new StreamWriter(stream);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x000C7761 File Offset: 0x000C5961
		public TextWriterTraceListener(TextWriter writer)
			: this(writer, string.Empty)
		{
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x000C776F File Offset: 0x000C596F
		public TextWriterTraceListener(TextWriter writer, string name)
			: base(name)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000C778D File Offset: 0x000C598D
		public TextWriterTraceListener(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000C779C File Offset: 0x000C599C
		public TextWriterTraceListener(string fileName, string name)
			: base(name)
		{
			this.fileName = fileName;
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06002C47 RID: 11335 RVA: 0x000C77AC File Offset: 0x000C59AC
		// (set) Token: 0x06002C48 RID: 11336 RVA: 0x000C77BB File Offset: 0x000C59BB
		public TextWriter Writer
		{
			get
			{
				this.EnsureWriter();
				return this.writer;
			}
			set
			{
				this.writer = value;
			}
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000C77C4 File Offset: 0x000C59C4
		public override void Close()
		{
			if (this.writer != null)
			{
				try
				{
					this.writer.Close();
				}
				catch (ObjectDisposedException)
				{
				}
			}
			this.writer = null;
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000C7800 File Offset: 0x000C5A00
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.Close();
				}
				else
				{
					if (this.writer != null)
					{
						try
						{
							this.writer.Close();
						}
						catch (ObjectDisposedException)
						{
						}
					}
					this.writer = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000C7860 File Offset: 0x000C5A60
		public override void Flush()
		{
			if (!this.EnsureWriter())
			{
				return;
			}
			try
			{
				this.writer.Flush();
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x000C7898 File Offset: 0x000C5A98
		public override void Write(string message)
		{
			if (!this.EnsureWriter())
			{
				return;
			}
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			try
			{
				this.writer.Write(message);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000C78E0 File Offset: 0x000C5AE0
		public override void WriteLine(string message)
		{
			if (!this.EnsureWriter())
			{
				return;
			}
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			try
			{
				this.writer.WriteLine(message);
				base.NeedIndent = true;
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000C7930 File Offset: 0x000C5B30
		private static Encoding GetEncodingWithFallback(Encoding encoding)
		{
			Encoding encoding2 = (Encoding)encoding.Clone();
			encoding2.EncoderFallback = EncoderFallback.ReplacementFallback;
			encoding2.DecoderFallback = DecoderFallback.ReplacementFallback;
			return encoding2;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000C7960 File Offset: 0x000C5B60
		internal bool EnsureWriter()
		{
			bool flag = true;
			if (this.writer == null)
			{
				flag = false;
				if (this.fileName == null)
				{
					return flag;
				}
				Encoding encodingWithFallback = TextWriterTraceListener.GetEncodingWithFallback(new UTF8Encoding(false));
				string text = Path.GetFullPath(this.fileName);
				string directoryName = Path.GetDirectoryName(text);
				string text2 = Path.GetFileName(text);
				for (int i = 0; i < 2; i++)
				{
					try
					{
						this.writer = new StreamWriter(text, true, encodingWithFallback, 4096);
						flag = true;
						break;
					}
					catch (IOException)
					{
						text2 = Guid.NewGuid().ToString() + text2;
						text = Path.Combine(directoryName, text2);
					}
					catch (UnauthorizedAccessException)
					{
						break;
					}
					catch (Exception)
					{
						break;
					}
				}
				if (!flag)
				{
					this.fileName = null;
				}
			}
			return flag;
		}

		// Token: 0x040026BF RID: 9919
		internal TextWriter writer;

		// Token: 0x040026C0 RID: 9920
		private string fileName;
	}
}
