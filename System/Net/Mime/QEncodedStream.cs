using System;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x0200024F RID: 591
	internal class QEncodedStream : DelegatedStream, IEncodableStream
	{
		// Token: 0x06001669 RID: 5737 RVA: 0x00073ED9 File Offset: 0x000720D9
		internal QEncodedStream(WriteStateInfoBase wsi)
		{
			this.writeState = wsi;
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x00073EE8 File Offset: 0x000720E8
		private QEncodedStream.ReadStateInfo ReadState
		{
			get
			{
				if (this.readState == null)
				{
					this.readState = new QEncodedStream.ReadStateInfo();
				}
				return this.readState;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x00073F03 File Offset: 0x00072103
		internal WriteStateInfoBase WriteState
		{
			get
			{
				return this.writeState;
			}
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00073F0C File Offset: 0x0007210C
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			QEncodedStream.WriteAsyncResult writeAsyncResult = new QEncodedStream.WriteAsyncResult(this, buffer, offset, count, callback, state);
			writeAsyncResult.Write();
			return writeAsyncResult;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00073F64 File Offset: 0x00072164
		public override void Close()
		{
			this.FlushInternal();
			base.Close();
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00073F74 File Offset: 0x00072174
		public unsafe int DecodeBytes(byte[] buffer, int offset, int count)
		{
			fixed (byte[] array = buffer)
			{
				byte* ptr;
				if (buffer == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				byte* ptr2 = ptr + offset;
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr2;
				byte* ptr5 = ptr2 + count;
				if (this.ReadState.IsEscaped)
				{
					if (this.ReadState.Byte == -1)
					{
						if (count == 1)
						{
							this.ReadState.Byte = (short)(*ptr3);
							return 0;
						}
						if (*ptr3 != 13 || ptr3[1] != 10)
						{
							byte b = QEncodedStream.hexDecodeMap[(int)(*ptr3)];
							byte b2 = QEncodedStream.hexDecodeMap[(int)ptr3[1]];
							if (b == 255)
							{
								throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b }));
							}
							if (b2 == 255)
							{
								throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b2 }));
							}
							*(ptr4++) = (byte)(((int)b << 4) + (int)b2);
						}
						ptr3 += 2;
					}
					else
					{
						if (this.ReadState.Byte != 13 || *ptr3 != 10)
						{
							byte b3 = QEncodedStream.hexDecodeMap[(int)this.ReadState.Byte];
							byte b4 = QEncodedStream.hexDecodeMap[(int)(*ptr3)];
							if (b3 == 255)
							{
								throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b3 }));
							}
							if (b4 == 255)
							{
								throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b4 }));
							}
							*(ptr4++) = (byte)(((int)b3 << 4) + (int)b4);
						}
						ptr3++;
					}
					this.ReadState.IsEscaped = false;
					this.ReadState.Byte = -1;
				}
				while (ptr3 < ptr5)
				{
					if (*ptr3 == 61)
					{
						long num = (long)(ptr5 - ptr3);
						if (num != 1L)
						{
							if (num != 2L)
							{
								if (ptr3[1] != 13 || ptr3[2] != 10)
								{
									byte b5 = QEncodedStream.hexDecodeMap[(int)ptr3[1]];
									byte b6 = QEncodedStream.hexDecodeMap[(int)ptr3[2]];
									if (b5 == 255)
									{
										throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b5 }));
									}
									if (b6 == 255)
									{
										throw new FormatException(SR.GetString("InvalidHexDigit", new object[] { b6 }));
									}
									*(ptr4++) = (byte)(((int)b5 << 4) + (int)b6);
								}
								ptr3 += 3;
								continue;
							}
							this.ReadState.Byte = (short)ptr3[1];
						}
						this.ReadState.IsEscaped = true;
						break;
					}
					if (*ptr3 == 95)
					{
						*(ptr4++) = 32;
						ptr3++;
					}
					else
					{
						*(ptr4++) = *(ptr3++);
					}
				}
				count = (int)((long)(ptr4 - ptr2));
			}
			return count;
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x0007422C File Offset: 0x0007242C
		public int EncodeBytes(byte[] buffer, int offset, int count)
		{
			this.writeState.AppendHeader();
			int i;
			for (i = offset; i < count + offset; i++)
			{
				if ((this.WriteState.CurrentLineLength + 3 + this.WriteState.FooterLength >= this.WriteState.MaxLineLength && (buffer[i] == 32 || buffer[i] == 9 || buffer[i] == 13 || buffer[i] == 10)) || this.WriteState.CurrentLineLength + this.writeState.FooterLength >= this.WriteState.MaxLineLength)
				{
					this.WriteState.AppendCRLF(true);
				}
				if (buffer[i] == 13 && i + 1 < count + offset && buffer[i + 1] == 10)
				{
					i++;
					this.WriteState.Append(new byte[] { 61, 48, 68, 61, 48, 65 });
				}
				else if (buffer[i] == 32)
				{
					this.WriteState.Append(95);
				}
				else if (Uri.IsAsciiLetterOrDigit((char)buffer[i]))
				{
					this.WriteState.Append(buffer[i]);
				}
				else
				{
					this.WriteState.Append(61);
					this.WriteState.Append(QEncodedStream.hexEncodeMap[buffer[i] >> 4]);
					this.WriteState.Append(QEncodedStream.hexEncodeMap[(int)(buffer[i] & 15)]);
				}
			}
			this.WriteState.AppendFooter();
			return i - offset;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0007437C File Offset: 0x0007257C
		public Stream GetStream()
		{
			return this;
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x0007437F File Offset: 0x0007257F
		public string GetEncodedString()
		{
			return Encoding.ASCII.GetString(this.WriteState.Buffer, 0, this.WriteState.Length);
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x000743A2 File Offset: 0x000725A2
		public override void EndWrite(IAsyncResult asyncResult)
		{
			QEncodedStream.WriteAsyncResult.End(asyncResult);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x000743AA File Offset: 0x000725AA
		public override void Flush()
		{
			this.FlushInternal();
			base.Flush();
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x000743B8 File Offset: 0x000725B8
		private void FlushInternal()
		{
			if (this.writeState != null && this.writeState.Length > 0)
			{
				base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
				this.WriteState.Reset();
			}
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000743F8 File Offset: 0x000725F8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num = 0;
			for (;;)
			{
				num += this.EncodeBytes(buffer, offset + num, count - num);
				if (num >= count)
				{
					break;
				}
				this.FlushInternal();
			}
		}

		// Token: 0x04001736 RID: 5942
		private const int sizeOfFoldingCRLF = 3;

		// Token: 0x04001737 RID: 5943
		private static byte[] hexDecodeMap = new byte[]
		{
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 1,
			2, 3, 4, 5, 6, 7, 8, 9, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 10, 11, 12, 13, 14,
			15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 10, 11, 12,
			13, 14, 15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue
		};

		// Token: 0x04001738 RID: 5944
		private static byte[] hexEncodeMap = new byte[]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			65, 66, 67, 68, 69, 70
		};

		// Token: 0x04001739 RID: 5945
		private QEncodedStream.ReadStateInfo readState;

		// Token: 0x0400173A RID: 5946
		private WriteStateInfoBase writeState;

		// Token: 0x02000799 RID: 1945
		private class ReadStateInfo
		{
			// Token: 0x17000F3C RID: 3900
			// (get) Token: 0x060042D5 RID: 17109 RVA: 0x00117D8C File Offset: 0x00115F8C
			// (set) Token: 0x060042D6 RID: 17110 RVA: 0x00117D94 File Offset: 0x00115F94
			internal bool IsEscaped
			{
				get
				{
					return this.isEscaped;
				}
				set
				{
					this.isEscaped = value;
				}
			}

			// Token: 0x17000F3D RID: 3901
			// (get) Token: 0x060042D7 RID: 17111 RVA: 0x00117D9D File Offset: 0x00115F9D
			// (set) Token: 0x060042D8 RID: 17112 RVA: 0x00117DA5 File Offset: 0x00115FA5
			internal short Byte
			{
				get
				{
					return this.b1;
				}
				set
				{
					this.b1 = value;
				}
			}

			// Token: 0x0400338E RID: 13198
			private bool isEscaped;

			// Token: 0x0400338F RID: 13199
			private short b1 = -1;
		}

		// Token: 0x0200079A RID: 1946
		private class WriteAsyncResult : LazyAsyncResult
		{
			// Token: 0x060042DA RID: 17114 RVA: 0x00117DBD File Offset: 0x00115FBD
			internal WriteAsyncResult(QEncodedStream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
			}

			// Token: 0x060042DB RID: 17115 RVA: 0x00117DE7 File Offset: 0x00115FE7
			private void CompleteWrite(IAsyncResult result)
			{
				this.parent.BaseStream.EndWrite(result);
				this.parent.WriteState.Reset();
			}

			// Token: 0x060042DC RID: 17116 RVA: 0x00117E0C File Offset: 0x0011600C
			internal static void End(IAsyncResult result)
			{
				QEncodedStream.WriteAsyncResult writeAsyncResult = (QEncodedStream.WriteAsyncResult)result;
				writeAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x060042DD RID: 17117 RVA: 0x00117E28 File Offset: 0x00116028
			private static void OnWrite(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					QEncodedStream.WriteAsyncResult writeAsyncResult = (QEncodedStream.WriteAsyncResult)result.AsyncState;
					try
					{
						writeAsyncResult.CompleteWrite(result);
						writeAsyncResult.Write();
					}
					catch (Exception ex)
					{
						writeAsyncResult.InvokeCallback(ex);
					}
				}
			}

			// Token: 0x060042DE RID: 17118 RVA: 0x00117E74 File Offset: 0x00116074
			internal void Write()
			{
				for (;;)
				{
					this.written += this.parent.EncodeBytes(this.buffer, this.offset + this.written, this.count - this.written);
					if (this.written >= this.count)
					{
						break;
					}
					IAsyncResult asyncResult = this.parent.BaseStream.BeginWrite(this.parent.WriteState.Buffer, 0, this.parent.WriteState.Length, QEncodedStream.WriteAsyncResult.onWrite, this);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					this.CompleteWrite(asyncResult);
				}
				base.InvokeCallback();
			}

			// Token: 0x04003390 RID: 13200
			private QEncodedStream parent;

			// Token: 0x04003391 RID: 13201
			private byte[] buffer;

			// Token: 0x04003392 RID: 13202
			private int offset;

			// Token: 0x04003393 RID: 13203
			private int count;

			// Token: 0x04003394 RID: 13204
			private static AsyncCallback onWrite = new AsyncCallback(QEncodedStream.WriteAsyncResult.OnWrite);

			// Token: 0x04003395 RID: 13205
			private int written;
		}
	}
}
