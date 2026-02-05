using System;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000250 RID: 592
	internal class QuotedPrintableStream : DelegatedStream, IEncodableStream
	{
		// Token: 0x06001677 RID: 5751 RVA: 0x0007448C File Offset: 0x0007268C
		internal QuotedPrintableStream(Stream stream, int lineLength)
			: base(stream)
		{
			if (lineLength < 0)
			{
				throw new ArgumentOutOfRangeException("lineLength");
			}
			this.lineLength = lineLength;
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x000744AB File Offset: 0x000726AB
		internal QuotedPrintableStream(Stream stream, bool encodeCRLF)
			: this(stream, EncodedStreamFactory.DefaultMaxLineLength)
		{
			this.encodeCRLF = encodeCRLF;
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x000744C0 File Offset: 0x000726C0
		private QuotedPrintableStream.ReadStateInfo ReadState
		{
			get
			{
				if (this.readState == null)
				{
					this.readState = new QuotedPrintableStream.ReadStateInfo();
				}
				return this.readState;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x000744DB File Offset: 0x000726DB
		internal WriteStateInfoBase WriteState
		{
			get
			{
				if (this.writeState == null)
				{
					this.writeState = new WriteStateInfoBase(1024, null, null, this.lineLength);
				}
				return this.writeState;
			}
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00074504 File Offset: 0x00072704
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
			QuotedPrintableStream.WriteAsyncResult writeAsyncResult = new QuotedPrintableStream.WriteAsyncResult(this, buffer, offset, count, callback, state);
			writeAsyncResult.Write();
			return writeAsyncResult;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0007455C File Offset: 0x0007275C
		public override void Close()
		{
			this.FlushInternal();
			base.Close();
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0007456C File Offset: 0x0007276C
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
							byte b = QuotedPrintableStream.hexDecodeMap[(int)(*ptr3)];
							byte b2 = QuotedPrintableStream.hexDecodeMap[(int)ptr3[1]];
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
							byte b3 = QuotedPrintableStream.hexDecodeMap[(int)this.ReadState.Byte];
							byte b4 = QuotedPrintableStream.hexDecodeMap[(int)(*ptr3)];
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
									byte b5 = QuotedPrintableStream.hexDecodeMap[(int)ptr3[1]];
									byte b6 = QuotedPrintableStream.hexDecodeMap[(int)ptr3[2]];
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
					*(ptr4++) = *(ptr3++);
				}
				count = (int)((long)(ptr4 - ptr2));
			}
			return count;
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00074808 File Offset: 0x00072A08
		public int EncodeBytes(byte[] buffer, int offset, int count)
		{
			int i;
			for (i = offset; i < count + offset; i++)
			{
				if ((this.lineLength != -1 && this.WriteState.CurrentLineLength + 3 + 2 >= this.lineLength && (buffer[i] == 32 || buffer[i] == 9 || buffer[i] == 13 || buffer[i] == 10)) || this.writeState.CurrentLineLength + 3 + 2 >= EncodedStreamFactory.DefaultMaxLineLength)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
					{
						return i - offset;
					}
					this.WriteState.Append(61);
					this.WriteState.AppendCRLF(false);
				}
				if (buffer[i] == 13 && i + 1 < count + offset && buffer[i + 1] == 10)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < (this.encodeCRLF ? 6 : 2))
					{
						return i - offset;
					}
					i++;
					if (this.encodeCRLF)
					{
						this.WriteState.Append(new byte[] { 61, 48, 68, 61, 48, 65 });
					}
					else
					{
						this.WriteState.AppendCRLF(false);
					}
				}
				else if ((buffer[i] < 32 && buffer[i] != 9) || buffer[i] == 61 || buffer[i] > 126)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
					{
						return i - offset;
					}
					this.WriteState.Append(61);
					this.WriteState.Append(QuotedPrintableStream.hexEncodeMap[buffer[i] >> 4]);
					this.WriteState.Append(QuotedPrintableStream.hexEncodeMap[(int)(buffer[i] & 15)]);
				}
				else
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 1)
					{
						return i - offset;
					}
					if ((buffer[i] == 9 || buffer[i] == 32) && i + 1 >= count + offset)
					{
						if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
						{
							return i - offset;
						}
						this.WriteState.Append(61);
						this.WriteState.Append(QuotedPrintableStream.hexEncodeMap[buffer[i] >> 4]);
						this.WriteState.Append(QuotedPrintableStream.hexEncodeMap[(int)(buffer[i] & 15)]);
					}
					else
					{
						this.WriteState.Append(buffer[i]);
					}
				}
			}
			return i - offset;
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00074A53 File Offset: 0x00072C53
		public Stream GetStream()
		{
			return this;
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00074A56 File Offset: 0x00072C56
		public string GetEncodedString()
		{
			return Encoding.ASCII.GetString(this.WriteState.Buffer, 0, this.WriteState.Length);
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x00074A79 File Offset: 0x00072C79
		public override void EndWrite(IAsyncResult asyncResult)
		{
			QuotedPrintableStream.WriteAsyncResult.End(asyncResult);
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00074A81 File Offset: 0x00072C81
		public override void Flush()
		{
			this.FlushInternal();
			base.Flush();
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00074A8F File Offset: 0x00072C8F
		private void FlushInternal()
		{
			if (this.writeState != null && this.writeState.Length > 0)
			{
				base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
				this.WriteState.BufferFlushed();
			}
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00074AD0 File Offset: 0x00072CD0
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

		// Token: 0x0400173B RID: 5947
		private bool encodeCRLF;

		// Token: 0x0400173C RID: 5948
		private const int sizeOfSoftCRLF = 3;

		// Token: 0x0400173D RID: 5949
		private const int sizeOfEncodedChar = 3;

		// Token: 0x0400173E RID: 5950
		private const int sizeOfEncodedCRLF = 6;

		// Token: 0x0400173F RID: 5951
		private const int sizeOfNonEncodedCRLF = 2;

		// Token: 0x04001740 RID: 5952
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

		// Token: 0x04001741 RID: 5953
		private static byte[] hexEncodeMap = new byte[]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			65, 66, 67, 68, 69, 70
		};

		// Token: 0x04001742 RID: 5954
		private int lineLength;

		// Token: 0x04001743 RID: 5955
		private QuotedPrintableStream.ReadStateInfo readState;

		// Token: 0x04001744 RID: 5956
		private WriteStateInfoBase writeState;

		// Token: 0x0200079B RID: 1947
		private class ReadStateInfo
		{
			// Token: 0x17000F3E RID: 3902
			// (get) Token: 0x060042E0 RID: 17120 RVA: 0x00117F2C File Offset: 0x0011612C
			// (set) Token: 0x060042E1 RID: 17121 RVA: 0x00117F34 File Offset: 0x00116134
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

			// Token: 0x17000F3F RID: 3903
			// (get) Token: 0x060042E2 RID: 17122 RVA: 0x00117F3D File Offset: 0x0011613D
			// (set) Token: 0x060042E3 RID: 17123 RVA: 0x00117F45 File Offset: 0x00116145
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

			// Token: 0x04003396 RID: 13206
			private bool isEscaped;

			// Token: 0x04003397 RID: 13207
			private short b1 = -1;
		}

		// Token: 0x0200079C RID: 1948
		private class WriteAsyncResult : LazyAsyncResult
		{
			// Token: 0x060042E5 RID: 17125 RVA: 0x00117F5D File Offset: 0x0011615D
			internal WriteAsyncResult(QuotedPrintableStream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
			}

			// Token: 0x060042E6 RID: 17126 RVA: 0x00117F87 File Offset: 0x00116187
			private void CompleteWrite(IAsyncResult result)
			{
				this.parent.BaseStream.EndWrite(result);
				this.parent.WriteState.BufferFlushed();
			}

			// Token: 0x060042E7 RID: 17127 RVA: 0x00117FAC File Offset: 0x001161AC
			internal static void End(IAsyncResult result)
			{
				QuotedPrintableStream.WriteAsyncResult writeAsyncResult = (QuotedPrintableStream.WriteAsyncResult)result;
				writeAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x060042E8 RID: 17128 RVA: 0x00117FC8 File Offset: 0x001161C8
			private static void OnWrite(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					QuotedPrintableStream.WriteAsyncResult writeAsyncResult = (QuotedPrintableStream.WriteAsyncResult)result.AsyncState;
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

			// Token: 0x060042E9 RID: 17129 RVA: 0x00118014 File Offset: 0x00116214
			internal void Write()
			{
				for (;;)
				{
					this.written += this.parent.EncodeBytes(this.buffer, this.offset + this.written, this.count - this.written);
					if (this.written >= this.count)
					{
						break;
					}
					IAsyncResult asyncResult = this.parent.BaseStream.BeginWrite(this.parent.WriteState.Buffer, 0, this.parent.WriteState.Length, QuotedPrintableStream.WriteAsyncResult.onWrite, this);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					this.CompleteWrite(asyncResult);
				}
				base.InvokeCallback();
			}

			// Token: 0x04003398 RID: 13208
			private QuotedPrintableStream parent;

			// Token: 0x04003399 RID: 13209
			private byte[] buffer;

			// Token: 0x0400339A RID: 13210
			private int offset;

			// Token: 0x0400339B RID: 13211
			private int count;

			// Token: 0x0400339C RID: 13212
			private static AsyncCallback onWrite = new AsyncCallback(QuotedPrintableStream.WriteAsyncResult.OnWrite);

			// Token: 0x0400339D RID: 13213
			private int written;
		}
	}
}
