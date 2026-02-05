using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000006 RID: 6
	internal class ZipSegmentedStream : Stream
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00002EC6 File Offset: 0x000010C6
		private ZipSegmentedStream()
		{
			this._exceptionPending = false;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002ED8 File Offset: 0x000010D8
		public static ZipSegmentedStream ForReading(string name, uint initialDiskNumber, uint maxDiskNumber)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.ReadOnly,
				CurrentSegment = initialDiskNumber,
				_maxDiskNumber = maxDiskNumber,
				_baseName = name
			};
			zipSegmentedStream._SetReadStream();
			return zipSegmentedStream;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002F10 File Offset: 0x00001110
		public static ZipSegmentedStream ForWriting(string name, int maxSegmentSize)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.Write,
				CurrentSegment = 0U,
				_baseName = name,
				_maxSegmentSize = maxSegmentSize,
				_baseDir = Path.GetDirectoryName(name)
			};
			if (zipSegmentedStream._baseDir == "")
			{
				zipSegmentedStream._baseDir = ".";
			}
			zipSegmentedStream._SetWriteStream(0U);
			return zipSegmentedStream;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002F74 File Offset: 0x00001174
		public static Stream ForUpdate(string name, uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			string text = string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name)), diskNumber + 1U);
			return File.Open(text, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002FBE File Offset: 0x000011BE
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002FC6 File Offset: 0x000011C6
		public bool ContiguousWrite { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002FCF File Offset: 0x000011CF
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002FD7 File Offset: 0x000011D7
		public uint CurrentSegment
		{
			get
			{
				return this._currentDiskNumber;
			}
			private set
			{
				this._currentDiskNumber = value;
				this._currentName = null;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002FE7 File Offset: 0x000011E7
		public string CurrentName
		{
			get
			{
				if (this._currentName == null)
				{
					this._currentName = this._NameForSegment(this.CurrentSegment);
				}
				return this._currentName;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003009 File Offset: 0x00001209
		public string CurrentTempName
		{
			get
			{
				return this._currentTempName;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003014 File Offset: 0x00001214
		private string _NameForSegment(uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				this._exceptionPending = true;
				throw new OverflowException("The number of zip segments would exceed 99.");
			}
			return string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(this._baseName), Path.GetFileNameWithoutExtension(this._baseName)), diskNumber + 1U);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003065 File Offset: 0x00001265
		public uint ComputeSegment(int length)
		{
			if (this._innerStream.Position + (long)length > (long)this._maxSegmentSize)
			{
				return this.CurrentSegment + 1U;
			}
			return this.CurrentSegment;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003090 File Offset: 0x00001290
		public override string ToString()
		{
			return string.Format("{0}[{1}][{2}], pos=0x{3:X})", new object[]
			{
				"ZipSegmentedStream",
				this.CurrentName,
				this.rwMode.ToString(),
				this.Position
			});
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000030E4 File Offset: 0x000012E4
		private void _SetReadStream()
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
			}
			if (this.CurrentSegment + 1U == this._maxDiskNumber)
			{
				this._currentName = this._baseName;
			}
			this._innerStream = File.OpenRead(this.CurrentName);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003134 File Offset: 0x00001334
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.ReadOnly)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Read.");
			}
			int num = this._innerStream.Read(buffer, offset, count);
			int num2 = num;
			while (num2 != count)
			{
				if (this._innerStream.Position != this._innerStream.Length)
				{
					this._exceptionPending = true;
					throw new ZipException(string.Format("Read error in file {0}", this.CurrentName));
				}
				if (this.CurrentSegment + 1U == this._maxDiskNumber)
				{
					return num;
				}
				this.CurrentSegment += 1U;
				this._SetReadStream();
				offset += num2;
				count -= num2;
				num2 = this._innerStream.Read(buffer, offset, count);
				num += num2;
			}
			return num;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000031EC File Offset: 0x000013EC
		private void _SetWriteStream(uint increment)
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this.CurrentName))
				{
					File.Delete(this.CurrentName);
				}
				File.Move(this._currentTempName, this.CurrentName);
			}
			if (increment > 0U)
			{
				this.CurrentSegment += increment;
			}
			SharedUtilities.CreateAndOpenUniqueTempFile(this._baseDir, out this._innerStream, out this._currentTempName);
			if (this.CurrentSegment == 0U)
			{
				this._innerStream.Write(BitConverter.GetBytes(134695760), 0, 4);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003280 File Offset: 0x00001480
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Write.");
			}
			if (this.ContiguousWrite)
			{
				if (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					this._SetWriteStream(1U);
				}
			}
			else
			{
				while (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					int num = this._maxSegmentSize - (int)this._innerStream.Position;
					this._innerStream.Write(buffer, offset, num);
					this._SetWriteStream(1U);
					count -= num;
					offset += num;
				}
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003328 File Offset: 0x00001528
		public long TruncateBackward(uint diskNumber, long offset)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new ZipException("bad state.");
			}
			if (diskNumber == this.CurrentSegment)
			{
				return this._innerStream.Seek(offset, SeekOrigin.Begin);
			}
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this._currentTempName))
				{
					File.Delete(this._currentTempName);
				}
			}
			for (uint num = this.CurrentSegment - 1U; num > diskNumber; num -= 1U)
			{
				string text = this._NameForSegment(num);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			this.CurrentSegment = diskNumber;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					this._currentTempName = SharedUtilities.InternalGetTempFileName();
					File.Move(this.CurrentName, this._currentTempName);
					break;
				}
				catch (IOException)
				{
					if (i == 2)
					{
						throw;
					}
				}
			}
			this._innerStream = new FileStream(this._currentTempName, FileMode.Open);
			return this._innerStream.Seek(offset, SeekOrigin.Begin);
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000343C File Offset: 0x0000163C
		public override bool CanRead
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.ReadOnly && this._innerStream != null && this._innerStream.CanRead;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000345C File Offset: 0x0000165C
		public override bool CanSeek
		{
			get
			{
				return this._innerStream != null && this._innerStream.CanSeek;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003473 File Offset: 0x00001673
		public override bool CanWrite
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.Write && this._innerStream != null && this._innerStream.CanWrite;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003493 File Offset: 0x00001693
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000034A0 File Offset: 0x000016A0
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000034AD File Offset: 0x000016AD
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000034BA File Offset: 0x000016BA
		public override long Position
		{
			get
			{
				return this._innerStream.Position;
			}
			set
			{
				this._innerStream.Position = value;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000034C8 File Offset: 0x000016C8
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(offset, origin);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000034E4 File Offset: 0x000016E4
		public override void SetLength(long value)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException();
			}
			this._innerStream.SetLength(value);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003508 File Offset: 0x00001708
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._innerStream != null)
				{
					this._innerStream.Dispose();
					if (this.rwMode == ZipSegmentedStream.RwMode.Write)
					{
						bool exceptionPending = this._exceptionPending;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0400003A RID: 58
		private ZipSegmentedStream.RwMode rwMode;

		// Token: 0x0400003B RID: 59
		private bool _exceptionPending;

		// Token: 0x0400003C RID: 60
		private string _baseName;

		// Token: 0x0400003D RID: 61
		private string _baseDir;

		// Token: 0x0400003E RID: 62
		private string _currentName;

		// Token: 0x0400003F RID: 63
		private string _currentTempName;

		// Token: 0x04000040 RID: 64
		private uint _currentDiskNumber;

		// Token: 0x04000041 RID: 65
		private uint _maxDiskNumber;

		// Token: 0x04000042 RID: 66
		private int _maxSegmentSize;

		// Token: 0x04000043 RID: 67
		private Stream _innerStream;

		// Token: 0x02000007 RID: 7
		private enum RwMode
		{
			// Token: 0x04000046 RID: 70
			None,
			// Token: 0x04000047 RID: 71
			ReadOnly,
			// Token: 0x04000048 RID: 72
			Write
		}
	}
}
