using System;
using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x02000004 RID: 4
	internal class ZipContainer
	{
		// Token: 0x0600003A RID: 58 RVA: 0x000028EA File Offset: 0x00000AEA
		public ZipContainer(object o)
		{
			this._zf = o as ZipFile;
			this._zos = o as ZipOutputStream;
			this._zis = o as ZipInputStream;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002916 File Offset: 0x00000B16
		public ZipFile ZipFile
		{
			get
			{
				return this._zf;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000291E File Offset: 0x00000B1E
		public ZipOutputStream ZipOutputStream
		{
			get
			{
				return this._zos;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002926 File Offset: 0x00000B26
		public string Name
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Name;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos.Name;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002955 File Offset: 0x00000B55
		public string Password
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._Password;
				}
				if (this._zis != null)
				{
					return this._zis._Password;
				}
				return this._zos._password;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0000298A File Offset: 0x00000B8A
		public Zip64Option Zip64
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._zip64;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos._zip64;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000029B9 File Offset: 0x00000BB9
		public int BufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.BufferSize;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return 0;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000029DE File Offset: 0x00000BDE
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002A09 File Offset: 0x00000C09
		public ParallelDeflateOutputStream ParallelDeflater
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflater;
				}
				if (this._zis != null)
				{
					return null;
				}
				return this._zos.ParallelDeflater;
			}
			set
			{
				if (this._zf != null)
				{
					this._zf.ParallelDeflater = value;
					return;
				}
				if (this._zos != null)
				{
					this._zos.ParallelDeflater = value;
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002A34 File Offset: 0x00000C34
		public long ParallelDeflateThreshold
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateThreshold;
				}
				return this._zos.ParallelDeflateThreshold;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002A55 File Offset: 0x00000C55
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateMaxBufferPairs;
				}
				return this._zos.ParallelDeflateMaxBufferPairs;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002A76 File Offset: 0x00000C76
		public int CodecBufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.CodecBufferSize;
				}
				if (this._zis != null)
				{
					return this._zis.CodecBufferSize;
				}
				return this._zos.CodecBufferSize;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002AAB File Offset: 0x00000CAB
		public CompressionStrategy Strategy
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Strategy;
				}
				return this._zos.Strategy;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002ACC File Offset: 0x00000CCC
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.UseZip64WhenSaving;
				}
				return this._zos.EnableZip64;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002AED File Offset: 0x00000CED
		public Encoding AlternateEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncoding;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncoding;
				}
				return null;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002B18 File Offset: 0x00000D18
		public Encoding DefaultEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return ZipFile.DefaultEncoding;
				}
				if (this._zos != null)
				{
					return ZipOutputStream.DefaultEncoding;
				}
				return null;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002B37 File Offset: 0x00000D37
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncodingUsage;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncodingUsage;
				}
				return ZipOption.Default;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002B62 File Offset: 0x00000D62
		public Stream ReadStream
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ReadStream;
				}
				return this._zis.ReadStream;
			}
		}

		// Token: 0x04000027 RID: 39
		private ZipFile _zf;

		// Token: 0x04000028 RID: 40
		private ZipOutputStream _zos;

		// Token: 0x04000029 RID: 41
		private ZipInputStream _zis;
	}
}
