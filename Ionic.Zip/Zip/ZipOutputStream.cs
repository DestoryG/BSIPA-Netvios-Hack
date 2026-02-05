using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Crc;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x02000003 RID: 3
	public class ZipOutputStream : Stream
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public ZipOutputStream(Stream stream)
			: this(stream, false)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020DC File Offset: 0x000002DC
		public ZipOutputStream(string fileName)
		{
			this._alternateEncoding = Encoding.GetEncoding("IBM437");
			this._maxBufferPairs = 16;
			base..ctor();
			Stream stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			this._Init(stream, false, fileName);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000211A File Offset: 0x0000031A
		public ZipOutputStream(Stream stream, bool leaveOpen)
		{
			this._alternateEncoding = Encoding.GetEncoding("IBM437");
			this._maxBufferPairs = 16;
			base..ctor();
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002144 File Offset: 0x00000344
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._outputStream = (stream.CanRead ? stream : new CountingStream(stream));
			this.CompressionLevel = CompressionLevel.Default;
			this.CompressionMethod = CompressionMethod.Deflate;
			this._encryption = EncryptionAlgorithm.None;
			this._entriesWritten = new Dictionary<string, ZipEntry>(StringComparer.Ordinal);
			this._zip64 = Zip64Option.Default;
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this.Strategy = CompressionStrategy.Default;
			this._name = name ?? "(stream)";
			this.ParallelDeflateThreshold = -1L;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021BA File Offset: 0x000003BA
		public override string ToString()
		{
			return string.Format("ZipOutputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x17000001 RID: 1
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000021D8 File Offset: 0x000003D8
		public string Password
		{
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._password = value;
				if (this._password == null)
				{
					this._encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this._encryption == EncryptionAlgorithm.None)
				{
					this._encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002225 File Offset: 0x00000425
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000222D File Offset: 0x0000042D
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._encryption;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._encryption = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002266 File Offset: 0x00000466
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000226E File Offset: 0x0000046E
		public int CodecBufferSize { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002277 File Offset: 0x00000477
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000227F File Offset: 0x0000047F
		public CompressionStrategy Strategy { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002288 File Offset: 0x00000488
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002290 File Offset: 0x00000490
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._timestamp = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022B3 File Offset: 0x000004B3
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000022BB File Offset: 0x000004BB
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022C4 File Offset: 0x000004C4
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000022CC File Offset: 0x000004CC
		public CompressionMethod CompressionMethod { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000022D5 File Offset: 0x000004D5
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000022DD File Offset: 0x000004DD
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._comment = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002300 File Offset: 0x00000500
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002308 File Offset: 0x00000508
		public Zip64Option EnableZip64
		{
			get
			{
				return this._zip64;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._zip64 = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000232B File Offset: 0x0000052B
		public bool OutputUsedZip64
		{
			get
			{
				return this._anyEntriesUsedZip64 || this._directoryNeededZip64;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000233D File Offset: 0x0000053D
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002348 File Offset: 0x00000548
		public bool IgnoreCase
		{
			get
			{
				return !this._DontIgnoreCase;
			}
			set
			{
				this._DontIgnoreCase = !value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002354 File Offset: 0x00000554
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000236E File Offset: 0x0000056E
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete. It will be removed in a future version of the library. Use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.UTF8 && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.UTF8;
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipOutputStream.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002398 File Offset: 0x00000598
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000023AB File Offset: 0x000005AB
		[Obsolete("use AlternateEncoding and AlternateEncodingUsage instead.")]
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000023BB File Offset: 0x000005BB
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000023C3 File Offset: 0x000005C3
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000023CC File Offset: 0x000005CC
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000023D4 File Offset: 0x000005D4
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000023DD File Offset: 0x000005DD
		public static Encoding DefaultEncoding
		{
			get
			{
				return Encoding.GetEncoding("IBM437");
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002410 File Offset: 0x00000610
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000023E9 File Offset: 0x000005E9
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
					throw new ArgumentOutOfRangeException("value must be greater than 64k, or 0, or -1");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002418 File Offset: 0x00000618
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002420 File Offset: 0x00000620
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

		// Token: 0x06000027 RID: 39 RVA: 0x0000243D File Offset: 0x0000063D
		private void InsureUniqueEntry(ZipEntry ze1)
		{
			if (this._entriesWritten.ContainsKey(ze1.FileName))
			{
				this._exceptionPending = true;
				throw new ArgumentException(string.Format("The entry '{0}' already exists in the zip archive.", ze1.FileName));
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000246F File Offset: 0x0000066F
		internal Stream OutputStream
		{
			get
			{
				return this._outputStream;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002477 File Offset: 0x00000677
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000247F File Offset: 0x0000067F
		public bool ContainsEntry(string name)
		{
			return this._entriesWritten.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002494 File Offset: 0x00000694
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (buffer == null)
			{
				this._exceptionPending = true;
				throw new ArgumentNullException("buffer");
			}
			if (this._currentEntry == null)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You must call PutNextEntry() before calling Write().");
			}
			if (this._currentEntry.IsDirectory)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You cannot Write() data for an entry that is a directory.");
			}
			if (this._needToWriteEntryHeader)
			{
				this._InitiateCurrentEntry(false);
			}
			if (count != 0)
			{
				this._entryOutputStream.Write(buffer, offset, count);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000252C File Offset: 0x0000072C
		public ZipEntry PutNextEntry(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			this._FinishCurrentEntry();
			this._currentEntry = ZipEntry.CreateForZipOutputStream(entryName);
			this._currentEntry._container = new ZipContainer(this);
			ZipEntry currentEntry = this._currentEntry;
			currentEntry._BitField |= 8;
			this._currentEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			this._currentEntry.CompressionLevel = this.CompressionLevel;
			this._currentEntry.CompressionMethod = this.CompressionMethod;
			this._currentEntry.Password = this._password;
			this._currentEntry.Encryption = this.Encryption;
			this._currentEntry.AlternateEncoding = this.AlternateEncoding;
			this._currentEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			if (entryName.EndsWith("/"))
			{
				this._currentEntry.MarkAsDirectory();
			}
			this._currentEntry.EmitTimesInWindowsFormatWhenSaving = (this._timestamp & ZipEntryTimestamp.Windows) != ZipEntryTimestamp.None;
			this._currentEntry.EmitTimesInUnixFormatWhenSaving = (this._timestamp & ZipEntryTimestamp.Unix) != ZipEntryTimestamp.None;
			this.InsureUniqueEntry(this._currentEntry);
			this._needToWriteEntryHeader = true;
			return this._currentEntry;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002680 File Offset: 0x00000880
		private void _InitiateCurrentEntry(bool finishing)
		{
			this._entriesWritten.Add(this._currentEntry.FileName, this._currentEntry);
			this._entryCount++;
			if (this._entryCount > 65534 && this._zip64 == Zip64Option.Default)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Too many entries. Consider setting ZipOutputStream.EnableZip64.");
			}
			this._currentEntry.WriteHeader(this._outputStream, finishing ? 99 : 0);
			this._currentEntry.StoreRelativeOffset();
			if (!this._currentEntry.IsDirectory)
			{
				this._currentEntry.WriteSecurityMetadata(this._outputStream);
				this._currentEntry.PrepOutputStream(this._outputStream, finishing ? 0L : (-1L), out this._outputCounter, out this._encryptor, out this._deflater, out this._entryOutputStream);
			}
			this._needToWriteEntryHeader = false;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002758 File Offset: 0x00000958
		private void _FinishCurrentEntry()
		{
			if (this._currentEntry != null)
			{
				if (this._needToWriteEntryHeader)
				{
					this._InitiateCurrentEntry(true);
				}
				this._currentEntry.FinishOutputStream(this._outputStream, this._outputCounter, this._encryptor, this._deflater, this._entryOutputStream);
				this._currentEntry.PostProcessOutput(this._outputStream);
				if (this._currentEntry.OutputUsedZip64 != null)
				{
					this._anyEntriesUsedZip64 |= this._currentEntry.OutputUsedZip64.Value;
				}
				this._outputCounter = null;
				this._encryptor = (this._deflater = null);
				this._entryOutputStream = null;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000280C File Offset: 0x00000A0C
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing && !this._exceptionPending)
			{
				this._FinishCurrentEntry();
				this._directoryNeededZip64 = ZipOutput.WriteCentralDirectoryStructure(this._outputStream, this._entriesWritten.Values, 1U, this._zip64, this.Comment, new ZipContainer(this));
				CountingStream countingStream = this._outputStream as CountingStream;
				Stream stream;
				if (countingStream != null)
				{
					stream = countingStream.WrappedStream;
					countingStream.Dispose();
				}
				else
				{
					stream = this._outputStream;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					stream.Dispose();
				}
				this._outputStream = null;
			}
			this._disposed = true;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000028A5 File Offset: 0x00000AA5
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000028AB File Offset: 0x00000AAB
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000028AE File Offset: 0x00000AAE
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000028B5 File Offset: 0x00000AB5
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000028C2 File Offset: 0x00000AC2
		public override long Position
		{
			get
			{
				return this._outputStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000028C9 File Offset: 0x00000AC9
		public override void Flush()
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000028CB File Offset: 0x00000ACB
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Read");
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000028D7 File Offset: 0x00000AD7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek");
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000028E3 File Offset: 0x00000AE3
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000009 RID: 9
		private EncryptionAlgorithm _encryption;

		// Token: 0x0400000A RID: 10
		private ZipEntryTimestamp _timestamp;

		// Token: 0x0400000B RID: 11
		internal string _password;

		// Token: 0x0400000C RID: 12
		private string _comment;

		// Token: 0x0400000D RID: 13
		private Stream _outputStream;

		// Token: 0x0400000E RID: 14
		private ZipEntry _currentEntry;

		// Token: 0x0400000F RID: 15
		internal Zip64Option _zip64;

		// Token: 0x04000010 RID: 16
		private Dictionary<string, ZipEntry> _entriesWritten;

		// Token: 0x04000011 RID: 17
		private int _entryCount;

		// Token: 0x04000012 RID: 18
		private ZipOption _alternateEncodingUsage;

		// Token: 0x04000013 RID: 19
		private Encoding _alternateEncoding;

		// Token: 0x04000014 RID: 20
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x04000015 RID: 21
		private bool _disposed;

		// Token: 0x04000016 RID: 22
		private bool _exceptionPending;

		// Token: 0x04000017 RID: 23
		private bool _anyEntriesUsedZip64;

		// Token: 0x04000018 RID: 24
		private bool _directoryNeededZip64;

		// Token: 0x04000019 RID: 25
		private CountingStream _outputCounter;

		// Token: 0x0400001A RID: 26
		private Stream _encryptor;

		// Token: 0x0400001B RID: 27
		private Stream _deflater;

		// Token: 0x0400001C RID: 28
		private CrcCalculatorStream _entryOutputStream;

		// Token: 0x0400001D RID: 29
		private bool _needToWriteEntryHeader;

		// Token: 0x0400001E RID: 30
		private string _name;

		// Token: 0x0400001F RID: 31
		private bool _DontIgnoreCase;

		// Token: 0x04000020 RID: 32
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x04000021 RID: 33
		private long _ParallelDeflateThreshold;

		// Token: 0x04000022 RID: 34
		private int _maxBufferPairs;
	}
}
