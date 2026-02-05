using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Xml
{
	// Token: 0x02000047 RID: 71
	internal class MimeHeaderReader
	{
		// Token: 0x06000529 RID: 1321 RVA: 0x00018DAC File Offset: 0x00016FAC
		public MimeHeaderReader(Stream stream)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			this.stream = stream;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00018DD9 File Offset: 0x00016FD9
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00018DE1 File Offset: 0x00016FE1
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00018DE9 File Offset: 0x00016FE9
		public void Close()
		{
			this.stream.Close();
			this.readState = MimeHeaderReader.ReadState.EOF;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00018E00 File Offset: 0x00017000
		public bool Read(int maxBuffer, ref int remaining)
		{
			this.name = null;
			this.value = null;
			while (this.readState != MimeHeaderReader.ReadState.EOF)
			{
				if (this.offset == this.maxOffset)
				{
					this.maxOffset = this.stream.Read(this.buffer, 0, this.buffer.Length);
					this.offset = 0;
					if (this.BufferEnd())
					{
						break;
					}
				}
				if (this.ProcessBuffer(maxBuffer, ref remaining))
				{
					break;
				}
			}
			return this.value != null;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00018E78 File Offset: 0x00017078
		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		private unsafe bool ProcessBuffer(int maxBuffer, ref int remaining)
		{
			byte[] array;
			byte* ptr;
			if ((array = this.buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			byte* ptr2 = ptr + this.offset;
			byte* ptr3 = ptr + this.maxOffset;
			byte* ptr4 = ptr2;
			switch (this.readState)
			{
			case MimeHeaderReader.ReadState.ReadName:
				while (ptr4 < ptr3)
				{
					if (*ptr4 == 58)
					{
						this.AppendName(new string((sbyte*)ptr2, 0, (int)((long)(ptr4 - ptr2))), maxBuffer, ref remaining);
						ptr4++;
						goto IL_016E;
					}
					if (*ptr4 >= 65 && *ptr4 <= 90)
					{
						byte* ptr5 = ptr4;
						*ptr5 += 32;
					}
					else if (*ptr4 < 33 || *ptr4 > 126)
					{
						if (this.name != null || *ptr4 != 13)
						{
							string text = "MIME header has an invalid character ('{0}', {1} in hexadecimal value).";
							object[] array2 = new object[2];
							array2[0] = (char)(*ptr4);
							int num = 1;
							int num2 = (int)(*ptr4);
							array2[num] = num2.ToString("X", CultureInfo.InvariantCulture);
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString(text, array2)));
						}
						ptr4++;
						if (ptr4 >= ptr3 || *ptr4 != 10)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("Malformed MIME header.")));
						}
						goto IL_025F;
					}
					ptr4++;
				}
				this.AppendName(new string((sbyte*)ptr2, 0, (int)((long)(ptr4 - ptr2))), maxBuffer, ref remaining);
				this.readState = MimeHeaderReader.ReadState.ReadName;
				goto IL_0276;
			case MimeHeaderReader.ReadState.SkipWS:
				break;
			case MimeHeaderReader.ReadState.ReadValue:
				goto IL_017F;
			case MimeHeaderReader.ReadState.ReadLF:
				goto IL_01F4;
			case MimeHeaderReader.ReadState.ReadWS:
				goto IL_0226;
			case MimeHeaderReader.ReadState.EOF:
				goto IL_025F;
			default:
				goto IL_0276;
			}
			IL_016E:
			while (ptr4 < ptr3)
			{
				if (*ptr4 != 9 && *ptr4 != 32)
				{
					goto IL_017F;
				}
				ptr4++;
			}
			this.readState = MimeHeaderReader.ReadState.SkipWS;
			goto IL_0276;
			IL_017F:
			ptr2 = ptr4;
			while (ptr4 < ptr3)
			{
				if (*ptr4 == 13)
				{
					this.AppendValue(new string((sbyte*)ptr2, 0, (int)((long)(ptr4 - ptr2))), maxBuffer, ref remaining);
					ptr4++;
					goto IL_01F4;
				}
				if (*ptr4 == 10)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("Malformed MIME header.")));
				}
				ptr4++;
			}
			this.AppendValue(new string((sbyte*)ptr2, 0, (int)((long)(ptr4 - ptr2))), maxBuffer, ref remaining);
			this.readState = MimeHeaderReader.ReadState.ReadValue;
			goto IL_0276;
			IL_01F4:
			if (ptr4 >= ptr3)
			{
				this.readState = MimeHeaderReader.ReadState.ReadLF;
				goto IL_0276;
			}
			if (*ptr4 != 10)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("Malformed MIME header.")));
			}
			ptr4++;
			IL_0226:
			if (ptr4 >= ptr3)
			{
				this.readState = MimeHeaderReader.ReadState.ReadWS;
				goto IL_0276;
			}
			if (*ptr4 != 32 && *ptr4 != 9)
			{
				this.readState = MimeHeaderReader.ReadState.ReadName;
				this.offset = (int)((long)(ptr4 - ptr));
				return true;
			}
			goto IL_017F;
			IL_025F:
			this.readState = MimeHeaderReader.ReadState.EOF;
			this.offset = (int)((long)(ptr4 - ptr));
			return true;
			IL_0276:
			this.offset = (int)((long)(ptr4 - ptr));
			array = null;
			return false;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001910C File Offset: 0x0001730C
		private bool BufferEnd()
		{
			if (this.maxOffset != 0)
			{
				return false;
			}
			if (this.readState != MimeHeaderReader.ReadState.ReadWS && this.readState != MimeHeaderReader.ReadState.ReadValue)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("Malformed MIME header.")));
			}
			this.readState = MimeHeaderReader.ReadState.EOF;
			return true;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00019148 File Offset: 0x00017348
		public void Reset(Stream stream)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			if (this.readState != MimeHeaderReader.ReadState.EOF)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("On MimeReader, Reset method is called before EOF.")));
			}
			this.stream = stream;
			this.readState = MimeHeaderReader.ReadState.ReadName;
			this.maxOffset = 0;
			this.offset = 0;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001919D File Offset: 0x0001739D
		private void AppendValue(string value, int maxBuffer, ref int remaining)
		{
			XmlMtomReader.DecrementBufferQuota(maxBuffer, ref remaining, value.Length * 2);
			if (this.value == null)
			{
				this.value = value;
				return;
			}
			this.value += value;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000191D0 File Offset: 0x000173D0
		private void AppendName(string value, int maxBuffer, ref int remaining)
		{
			XmlMtomReader.DecrementBufferQuota(maxBuffer, ref remaining, value.Length * 2);
			if (this.name == null)
			{
				this.name = value;
				return;
			}
			this.name += value;
		}

		// Token: 0x0400022F RID: 559
		private string value;

		// Token: 0x04000230 RID: 560
		private byte[] buffer = new byte[1024];

		// Token: 0x04000231 RID: 561
		private int maxOffset;

		// Token: 0x04000232 RID: 562
		private string name;

		// Token: 0x04000233 RID: 563
		private int offset;

		// Token: 0x04000234 RID: 564
		private MimeHeaderReader.ReadState readState;

		// Token: 0x04000235 RID: 565
		private Stream stream;

		// Token: 0x0200015D RID: 349
		private enum ReadState
		{
			// Token: 0x04000977 RID: 2423
			ReadName,
			// Token: 0x04000978 RID: 2424
			SkipWS,
			// Token: 0x04000979 RID: 2425
			ReadValue,
			// Token: 0x0400097A RID: 2426
			ReadLF,
			// Token: 0x0400097B RID: 2427
			ReadWS,
			// Token: 0x0400097C RID: 2428
			EOF
		}
	}
}
