using System;
using System.IO;
using System.Text;
using Ionic.Crc;

namespace Ionic.Zip
{
	// Token: 0x02000005 RID: 5
	public class ZipInputStream : Stream
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00002B83 File Offset: 0x00000D83
		public ZipInputStream(Stream stream)
			: this(stream, false)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002B90 File Offset: 0x00000D90
		public ZipInputStream(string fileName)
		{
			Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this._Init(stream, false, fileName);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002BB6 File Offset: 0x00000DB6
		public ZipInputStream(Stream stream, bool leaveOpen)
		{
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002BC8 File Offset: 0x00000DC8
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._inputStream = stream;
			if (!this._inputStream.CanRead)
			{
				throw new ZipException("The stream must be readable.");
			}
			this._container = new ZipContainer(this);
			this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this._findRequired = true;
			this._name = name ?? "(stream)";
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002C2E File Offset: 0x00000E2E
		public override string ToString()
		{
			return string.Format("ZipInputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002C4B File Offset: 0x00000E4B
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002C53 File Offset: 0x00000E53
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				return this._provisionalAlternateEncoding;
			}
			set
			{
				this._provisionalAlternateEncoding = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002C5C File Offset: 0x00000E5C
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002C64 File Offset: 0x00000E64
		public int CodecBufferSize { get; set; }

		// Token: 0x1700002C RID: 44
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002C6D File Offset: 0x00000E6D
		public string Password
		{
			set
			{
				if (this._closed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._Password = value;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002C90 File Offset: 0x00000E90
		private void SetupStream()
		{
			this._crcStream = this._currentEntry.InternalOpenReader(this._Password);
			this._LeftToRead = this._crcStream.Length;
			this._needSetup = false;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002CC1 File Offset: 0x00000EC1
		internal Stream ReadStream
		{
			get
			{
				return this._inputStream;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002CCC File Offset: 0x00000ECC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._closed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (this._needSetup)
			{
				this.SetupStream();
			}
			if (this._LeftToRead == 0L)
			{
				return 0;
			}
			int num = ((this._LeftToRead > (long)count) ? count : ((int)this._LeftToRead));
			int num2 = this._crcStream.Read(buffer, offset, num);
			this._LeftToRead -= (long)num2;
			if (this._LeftToRead == 0L)
			{
				int crc = this._crcStream.Crc;
				this._currentEntry.VerifyCrcAfterExtract(crc);
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			return num2;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D78 File Offset: 0x00000F78
		public ZipEntry GetNextEntry()
		{
			if (this._findRequired)
			{
				long num = SharedUtilities.FindSignature(this._inputStream, 67324752);
				if (num == -1L)
				{
					return null;
				}
				this._inputStream.Seek(-4L, SeekOrigin.Current);
			}
			else if (this._firstEntry)
			{
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			this._currentEntry = ZipEntry.ReadEntry(this._container, !this._firstEntry);
			this._endOfEntry = this._inputStream.Position;
			this._firstEntry = true;
			this._needSetup = true;
			this._findRequired = false;
			return this._currentEntry;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E18 File Offset: 0x00001018
		protected override void Dispose(bool disposing)
		{
			if (this._closed)
			{
				return;
			}
			if (disposing)
			{
				if (this._exceptionPending)
				{
					return;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					this._inputStream.Dispose();
				}
			}
			this._closed = true;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002E49 File Offset: 0x00001049
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002E4C File Offset: 0x0000104C
		public override bool CanSeek
		{
			get
			{
				return this._inputStream.CanSeek;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002E59 File Offset: 0x00001059
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002E5C File Offset: 0x0000105C
		public override long Length
		{
			get
			{
				return this._inputStream.Length;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002E69 File Offset: 0x00001069
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002E76 File Offset: 0x00001076
		public override long Position
		{
			get
			{
				return this._inputStream.Position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002E81 File Offset: 0x00001081
		public override void Flush()
		{
			throw new NotSupportedException("Flush");
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002E8D File Offset: 0x0000108D
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Write");
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002E9C File Offset: 0x0000109C
		public override long Seek(long offset, SeekOrigin origin)
		{
			this._findRequired = true;
			return this._inputStream.Seek(offset, origin);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002EBF File Offset: 0x000010BF
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400002A RID: 42
		private Stream _inputStream;

		// Token: 0x0400002B RID: 43
		private Encoding _provisionalAlternateEncoding;

		// Token: 0x0400002C RID: 44
		private ZipEntry _currentEntry;

		// Token: 0x0400002D RID: 45
		private bool _firstEntry;

		// Token: 0x0400002E RID: 46
		private bool _needSetup;

		// Token: 0x0400002F RID: 47
		private ZipContainer _container;

		// Token: 0x04000030 RID: 48
		private CrcCalculatorStream _crcStream;

		// Token: 0x04000031 RID: 49
		private long _LeftToRead;

		// Token: 0x04000032 RID: 50
		internal string _Password;

		// Token: 0x04000033 RID: 51
		private long _endOfEntry;

		// Token: 0x04000034 RID: 52
		private string _name;

		// Token: 0x04000035 RID: 53
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x04000036 RID: 54
		private bool _closed;

		// Token: 0x04000037 RID: 55
		private bool _findRequired;

		// Token: 0x04000038 RID: 56
		private bool _exceptionPending;
	}
}
