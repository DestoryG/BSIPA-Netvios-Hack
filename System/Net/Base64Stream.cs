using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net
{
	// Token: 0x02000224 RID: 548
	internal class Base64Stream : DelegatedStream, IEncodableStream
	{
		// Token: 0x0600141C RID: 5148 RVA: 0x0006A8FC File Offset: 0x00068AFC
		internal Base64Stream(Stream stream, Base64WriteStateInfo writeStateInfo)
			: base(stream)
		{
			this.writeState = new Base64WriteStateInfo();
			this.lineLength = writeStateInfo.MaxLineLength;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0006A91C File Offset: 0x00068B1C
		internal Base64Stream(Stream stream, int lineLength)
			: base(stream)
		{
			this.lineLength = lineLength;
			this.writeState = new Base64WriteStateInfo();
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0006A937 File Offset: 0x00068B37
		internal Base64Stream(Base64WriteStateInfo writeStateInfo)
		{
			this.lineLength = writeStateInfo.MaxLineLength;
			this.writeState = writeStateInfo;
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x0006A952 File Offset: 0x00068B52
		public override bool CanWrite
		{
			get
			{
				return base.CanWrite;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0006A95A File Offset: 0x00068B5A
		private Base64Stream.ReadStateInfo ReadState
		{
			get
			{
				if (this.readState == null)
				{
					this.readState = new Base64Stream.ReadStateInfo();
				}
				return this.readState;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0006A975 File Offset: 0x00068B75
		internal Base64WriteStateInfo WriteState
		{
			get
			{
				return this.writeState;
			}
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0006A980 File Offset: 0x00068B80
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
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
			Base64Stream.ReadAsyncResult readAsyncResult = new Base64Stream.ReadAsyncResult(this, buffer, offset, count, callback, state);
			readAsyncResult.Read();
			return readAsyncResult;
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0006A9D8 File Offset: 0x00068BD8
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
			Base64Stream.WriteAsyncResult writeAsyncResult = new Base64Stream.WriteAsyncResult(this, buffer, offset, count, callback, state);
			writeAsyncResult.Write();
			return writeAsyncResult;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0006AA30 File Offset: 0x00068C30
		public override void Close()
		{
			if (this.writeState != null && this.WriteState.Length > 0)
			{
				int padding = this.WriteState.Padding;
				if (padding != 1)
				{
					if (padding == 2)
					{
						this.WriteState.Append(new byte[]
						{
							Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits],
							Base64Stream.base64EncodeMap[64],
							Base64Stream.base64EncodeMap[64]
						});
					}
				}
				else
				{
					this.WriteState.Append(new byte[]
					{
						Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits],
						Base64Stream.base64EncodeMap[64]
					});
				}
				this.WriteState.Padding = 0;
				this.FlushInternal();
			}
			base.Close();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0006AAF4 File Offset: 0x00068CF4
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
				while (ptr3 < ptr5)
				{
					if (*ptr3 == 13 || *ptr3 == 10 || *ptr3 == 61 || *ptr3 == 32 || *ptr3 == 9)
					{
						ptr3++;
					}
					else
					{
						byte b = Base64Stream.base64DecodeMap[(int)(*ptr3)];
						if (b == 255)
						{
							throw new FormatException(SR.GetString("MailBase64InvalidCharacter"));
						}
						switch (this.ReadState.Pos)
						{
						case 0:
						{
							this.ReadState.Val = (byte)(b << 2);
							Base64Stream.ReadStateInfo readStateInfo = this.ReadState;
							byte b2 = readStateInfo.Pos;
							readStateInfo.Pos = b2 + 1;
							break;
						}
						case 1:
						{
							*(ptr4++) = (byte)((int)this.ReadState.Val + (b >> 4));
							this.ReadState.Val = (byte)(b << 4);
							Base64Stream.ReadStateInfo readStateInfo2 = this.ReadState;
							byte b2 = readStateInfo2.Pos;
							readStateInfo2.Pos = b2 + 1;
							break;
						}
						case 2:
						{
							*(ptr4++) = (byte)((int)this.ReadState.Val + (b >> 2));
							this.ReadState.Val = (byte)(b << 6);
							Base64Stream.ReadStateInfo readStateInfo3 = this.ReadState;
							byte b2 = readStateInfo3.Pos;
							readStateInfo3.Pos = b2 + 1;
							break;
						}
						case 3:
							*(ptr4++) = this.ReadState.Val + b;
							this.ReadState.Pos = 0;
							break;
						}
						ptr3++;
					}
				}
				count = (int)((long)(ptr4 - ptr2));
			}
			return count;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0006AC8E File Offset: 0x00068E8E
		public int EncodeBytes(byte[] buffer, int offset, int count)
		{
			return this.EncodeBytes(buffer, offset, count, true, true);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0006AC9C File Offset: 0x00068E9C
		internal int EncodeBytes(byte[] buffer, int offset, int count, bool dontDeferFinalBytes, bool shouldAppendSpaceToCRLF)
		{
			int i = offset;
			this.WriteState.AppendHeader();
			int padding = this.WriteState.Padding;
			if (padding != 1)
			{
				if (padding == 2)
				{
					this.WriteState.Append(Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits | ((buffer[i] & 240) >> 4)]);
					if (count == 1)
					{
						this.WriteState.LastBits = (byte)((buffer[i] & 15) << 2);
						this.WriteState.Padding = 1;
						i++;
						return i - offset;
					}
					this.WriteState.Append(Base64Stream.base64EncodeMap[((int)(buffer[i] & 15) << 2) | ((buffer[i + 1] & 192) >> 6)]);
					this.WriteState.Append(Base64Stream.base64EncodeMap[(int)(buffer[i + 1] & 63)]);
					i += 2;
					count -= 2;
					this.WriteState.Padding = 0;
				}
			}
			else
			{
				this.WriteState.Append(Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits | ((buffer[i] & 192) >> 6)]);
				this.WriteState.Append(Base64Stream.base64EncodeMap[(int)(buffer[i] & 63)]);
				i++;
				count--;
				this.WriteState.Padding = 0;
			}
			int num = i + (count - count % 3);
			while (i < num)
			{
				if (this.lineLength != -1 && this.WriteState.CurrentLineLength + 4 + this.writeState.FooterLength > this.lineLength)
				{
					this.WriteState.AppendCRLF(shouldAppendSpaceToCRLF);
				}
				this.WriteState.Append(Base64Stream.base64EncodeMap[(buffer[i] & 252) >> 2]);
				this.WriteState.Append(Base64Stream.base64EncodeMap[((int)(buffer[i] & 3) << 4) | ((buffer[i + 1] & 240) >> 4)]);
				this.WriteState.Append(Base64Stream.base64EncodeMap[((int)(buffer[i + 1] & 15) << 2) | ((buffer[i + 2] & 192) >> 6)]);
				this.WriteState.Append(Base64Stream.base64EncodeMap[(int)(buffer[i + 2] & 63)]);
				i += 3;
			}
			i = num;
			if (count % 3 != 0 && this.lineLength != -1 && this.WriteState.CurrentLineLength + 4 + this.writeState.FooterLength >= this.lineLength)
			{
				this.WriteState.AppendCRLF(shouldAppendSpaceToCRLF);
			}
			int num2 = count % 3;
			if (num2 != 1)
			{
				if (num2 == 2)
				{
					this.WriteState.Append(Base64Stream.base64EncodeMap[(buffer[i] & 252) >> 2]);
					this.WriteState.Append(Base64Stream.base64EncodeMap[((int)(buffer[i] & 3) << 4) | ((buffer[i + 1] & 240) >> 4)]);
					if (dontDeferFinalBytes)
					{
						this.WriteState.Append(Base64Stream.base64EncodeMap[(int)(buffer[i + 1] & 15) << 2]);
						this.WriteState.Append(Base64Stream.base64EncodeMap[64]);
						this.WriteState.Padding = 0;
					}
					else
					{
						this.WriteState.LastBits = (byte)((buffer[i + 1] & 15) << 2);
						this.WriteState.Padding = 1;
					}
					i += 2;
				}
			}
			else
			{
				this.WriteState.Append(Base64Stream.base64EncodeMap[(buffer[i] & 252) >> 2]);
				if (dontDeferFinalBytes)
				{
					this.WriteState.Append(Base64Stream.base64EncodeMap[(int)((byte)((buffer[i] & 3) << 4))]);
					this.WriteState.Append(Base64Stream.base64EncodeMap[64]);
					this.WriteState.Append(Base64Stream.base64EncodeMap[64]);
					this.WriteState.Padding = 0;
				}
				else
				{
					this.WriteState.LastBits = (byte)((buffer[i] & 3) << 4);
					this.WriteState.Padding = 2;
				}
				i++;
			}
			this.WriteState.AppendFooter();
			return i - offset;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0006B03D File Offset: 0x0006923D
		public Stream GetStream()
		{
			return this;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0006B040 File Offset: 0x00069240
		public string GetEncodedString()
		{
			return Encoding.ASCII.GetString(this.WriteState.Buffer, 0, this.WriteState.Length);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0006B064 File Offset: 0x00069264
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			return Base64Stream.ReadAsyncResult.End(asyncResult);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0006B087 File Offset: 0x00069287
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Base64Stream.WriteAsyncResult.End(asyncResult);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0006B09D File Offset: 0x0006929D
		public override void Flush()
		{
			if (this.writeState != null && this.WriteState.Length > 0)
			{
				this.FlushInternal();
			}
			base.Flush();
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0006B0C1 File Offset: 0x000692C1
		private void FlushInternal()
		{
			base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
			this.WriteState.Reset();
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0006B0EC File Offset: 0x000692EC
		public override int Read(byte[] buffer, int offset, int count)
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
			for (;;)
			{
				int num = base.Read(buffer, offset, count);
				if (num == 0)
				{
					break;
				}
				num = this.DecodeBytes(buffer, offset, num);
				if (num > 0)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0006B150 File Offset: 0x00069350
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
				num += this.EncodeBytes(buffer, offset + num, count - num, false, false);
				if (num >= count)
				{
					break;
				}
				this.FlushInternal();
			}
		}

		// Token: 0x04001613 RID: 5651
		private static byte[] base64DecodeMap = new byte[]
		{
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, 62, byte.MaxValue, byte.MaxValue, byte.MaxValue, 63, 52, 53,
			54, 55, 56, 57, 58, 59, 60, 61, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 1, 2, 3, 4,
			5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
			15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
			25, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 26, 27, 28,
			29, 30, 31, 32, 33, 34, 35, 36, 37, 38,
			39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
			49, 50, 51, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
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

		// Token: 0x04001614 RID: 5652
		private static byte[] base64EncodeMap = new byte[]
		{
			65, 66, 67, 68, 69, 70, 71, 72, 73, 74,
			75, 76, 77, 78, 79, 80, 81, 82, 83, 84,
			85, 86, 87, 88, 89, 90, 97, 98, 99, 100,
			101, 102, 103, 104, 105, 106, 107, 108, 109, 110,
			111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
			121, 122, 48, 49, 50, 51, 52, 53, 54, 55,
			56, 57, 43, 47, 61
		};

		// Token: 0x04001615 RID: 5653
		private int lineLength;

		// Token: 0x04001616 RID: 5654
		private Base64Stream.ReadStateInfo readState;

		// Token: 0x04001617 RID: 5655
		private Base64WriteStateInfo writeState;

		// Token: 0x04001618 RID: 5656
		private const int sizeOfBase64EncodedChar = 4;

		// Token: 0x04001619 RID: 5657
		private const byte invalidBase64Value = 255;

		// Token: 0x02000765 RID: 1893
		private class ReadAsyncResult : LazyAsyncResult
		{
			// Token: 0x06004236 RID: 16950 RVA: 0x00112B29 File Offset: 0x00110D29
			internal ReadAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
			}

			// Token: 0x06004237 RID: 16951 RVA: 0x00112B54 File Offset: 0x00110D54
			private bool CompleteRead(IAsyncResult result)
			{
				this.read = this.parent.BaseStream.EndRead(result);
				if (this.read == 0)
				{
					base.InvokeCallback();
					return true;
				}
				this.read = this.parent.DecodeBytes(this.buffer, this.offset, this.read);
				if (this.read > 0)
				{
					base.InvokeCallback();
					return true;
				}
				return false;
			}

			// Token: 0x06004238 RID: 16952 RVA: 0x00112BC0 File Offset: 0x00110DC0
			internal void Read()
			{
				IAsyncResult asyncResult;
				do
				{
					asyncResult = this.parent.BaseStream.BeginRead(this.buffer, this.offset, this.count, Base64Stream.ReadAsyncResult.onRead, this);
				}
				while (asyncResult.CompletedSynchronously && !this.CompleteRead(asyncResult));
			}

			// Token: 0x06004239 RID: 16953 RVA: 0x00112C08 File Offset: 0x00110E08
			private static void OnRead(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					Base64Stream.ReadAsyncResult readAsyncResult = (Base64Stream.ReadAsyncResult)result.AsyncState;
					try
					{
						if (!readAsyncResult.CompleteRead(result))
						{
							readAsyncResult.Read();
						}
					}
					catch (Exception ex)
					{
						if (readAsyncResult.IsCompleted)
						{
							throw;
						}
						readAsyncResult.InvokeCallback(ex);
					}
				}
			}

			// Token: 0x0600423A RID: 16954 RVA: 0x00112C60 File Offset: 0x00110E60
			internal static int End(IAsyncResult result)
			{
				Base64Stream.ReadAsyncResult readAsyncResult = (Base64Stream.ReadAsyncResult)result;
				readAsyncResult.InternalWaitForCompletion();
				return readAsyncResult.read;
			}

			// Token: 0x04003239 RID: 12857
			private Base64Stream parent;

			// Token: 0x0400323A RID: 12858
			private byte[] buffer;

			// Token: 0x0400323B RID: 12859
			private int offset;

			// Token: 0x0400323C RID: 12860
			private int count;

			// Token: 0x0400323D RID: 12861
			private int read;

			// Token: 0x0400323E RID: 12862
			private static AsyncCallback onRead = new AsyncCallback(Base64Stream.ReadAsyncResult.OnRead);
		}

		// Token: 0x02000766 RID: 1894
		private class WriteAsyncResult : LazyAsyncResult
		{
			// Token: 0x0600423C RID: 16956 RVA: 0x00112C94 File Offset: 0x00110E94
			internal WriteAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
			}

			// Token: 0x0600423D RID: 16957 RVA: 0x00112CC0 File Offset: 0x00110EC0
			internal void Write()
			{
				for (;;)
				{
					this.written += this.parent.EncodeBytes(this.buffer, this.offset + this.written, this.count - this.written, false, false);
					if (this.written >= this.count)
					{
						break;
					}
					IAsyncResult asyncResult = this.parent.BaseStream.BeginWrite(this.parent.WriteState.Buffer, 0, this.parent.WriteState.Length, Base64Stream.WriteAsyncResult.onWrite, this);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					this.CompleteWrite(asyncResult);
				}
				base.InvokeCallback();
			}

			// Token: 0x0600423E RID: 16958 RVA: 0x00112D67 File Offset: 0x00110F67
			private void CompleteWrite(IAsyncResult result)
			{
				this.parent.BaseStream.EndWrite(result);
				this.parent.WriteState.Reset();
			}

			// Token: 0x0600423F RID: 16959 RVA: 0x00112D8C File Offset: 0x00110F8C
			private static void OnWrite(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					Base64Stream.WriteAsyncResult writeAsyncResult = (Base64Stream.WriteAsyncResult)result.AsyncState;
					try
					{
						writeAsyncResult.CompleteWrite(result);
						writeAsyncResult.Write();
					}
					catch (Exception ex)
					{
						if (writeAsyncResult.IsCompleted)
						{
							throw;
						}
						writeAsyncResult.InvokeCallback(ex);
					}
				}
			}

			// Token: 0x06004240 RID: 16960 RVA: 0x00112DE0 File Offset: 0x00110FE0
			internal static void End(IAsyncResult result)
			{
				Base64Stream.WriteAsyncResult writeAsyncResult = (Base64Stream.WriteAsyncResult)result;
				writeAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x0400323F RID: 12863
			private Base64Stream parent;

			// Token: 0x04003240 RID: 12864
			private byte[] buffer;

			// Token: 0x04003241 RID: 12865
			private int offset;

			// Token: 0x04003242 RID: 12866
			private int count;

			// Token: 0x04003243 RID: 12867
			private static AsyncCallback onWrite = new AsyncCallback(Base64Stream.WriteAsyncResult.OnWrite);

			// Token: 0x04003244 RID: 12868
			private int written;
		}

		// Token: 0x02000767 RID: 1895
		private class ReadStateInfo
		{
			// Token: 0x17000F25 RID: 3877
			// (get) Token: 0x06004242 RID: 16962 RVA: 0x00112E0E File Offset: 0x0011100E
			// (set) Token: 0x06004243 RID: 16963 RVA: 0x00112E16 File Offset: 0x00111016
			internal byte Val
			{
				get
				{
					return this.val;
				}
				set
				{
					this.val = value;
				}
			}

			// Token: 0x17000F26 RID: 3878
			// (get) Token: 0x06004244 RID: 16964 RVA: 0x00112E1F File Offset: 0x0011101F
			// (set) Token: 0x06004245 RID: 16965 RVA: 0x00112E27 File Offset: 0x00111027
			internal byte Pos
			{
				get
				{
					return this.pos;
				}
				set
				{
					this.pos = value;
				}
			}

			// Token: 0x04003245 RID: 12869
			private byte val;

			// Token: 0x04003246 RID: 12870
			private byte pos;
		}
	}
}
