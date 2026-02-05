using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Protobuf
{
	// Token: 0x02000003 RID: 3
	public sealed class ByteString : IEnumerable<byte>, IEnumerable, IEquatable<ByteString>
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BD File Offset: 0x000002BD
		internal static ByteString AttachBytes(byte[] bytes)
		{
			return new ByteString(bytes);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C5 File Offset: 0x000002C5
		private ByteString(byte[] bytes)
		{
			this.bytes = bytes;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020D4 File Offset: 0x000002D4
		public static ByteString Empty
		{
			get
			{
				return ByteString.empty;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020DB File Offset: 0x000002DB
		public int Length
		{
			get
			{
				return this.bytes.Length;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020E5 File Offset: 0x000002E5
		public bool IsEmpty
		{
			get
			{
				return this.Length == 0;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020F0 File Offset: 0x000002F0
		public ReadOnlySpan<byte> Span
		{
			get
			{
				return new ReadOnlySpan<byte>(this.bytes);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020FD File Offset: 0x000002FD
		public byte[] ToByteArray()
		{
			return (byte[])this.bytes.Clone();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000210F File Offset: 0x0000030F
		public string ToBase64()
		{
			return Convert.ToBase64String(this.bytes);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000211C File Offset: 0x0000031C
		public static ByteString FromBase64(string bytes)
		{
			if (!(bytes == ""))
			{
				return new ByteString(Convert.FromBase64String(bytes));
			}
			return ByteString.Empty;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000213C File Offset: 0x0000033C
		public static ByteString FromStream(Stream stream)
		{
			ProtoPreconditions.CheckNotNull<Stream>(stream, "stream");
			MemoryStream memoryStream = new MemoryStream(stream.CanSeek ? checked((int)(stream.Length - stream.Position)) : 0);
			stream.CopyTo(memoryStream);
			return ByteString.AttachBytes(memoryStream.ToArray());
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002188 File Offset: 0x00000388
		public static async Task<ByteString> FromStreamAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
		{
			ProtoPreconditions.CheckNotNull<Stream>(stream, "stream");
			int num = (stream.CanSeek ? checked((int)(stream.Length - stream.Position)) : 0);
			MemoryStream memoryStream = new MemoryStream(num);
			await stream.CopyToAsync(memoryStream, 81920, cancellationToken);
			return ByteString.AttachBytes(memoryStream.ToArray());
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021D5 File Offset: 0x000003D5
		public static ByteString CopyFrom(params byte[] bytes)
		{
			return new ByteString((byte[])bytes.Clone());
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021E8 File Offset: 0x000003E8
		public static ByteString CopyFrom(byte[] bytes, int offset, int count)
		{
			byte[] array = new byte[count];
			ByteArray.Copy(bytes, offset, array, 0, count);
			return new ByteString(array);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000220C File Offset: 0x0000040C
		public static ByteString CopyFrom(ReadOnlySpan<byte> bytes)
		{
			return new ByteString(bytes.ToArray());
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000221A File Offset: 0x0000041A
		public static ByteString CopyFrom(string text, Encoding encoding)
		{
			return new ByteString(encoding.GetBytes(text));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002228 File Offset: 0x00000428
		public static ByteString CopyFromUtf8(string text)
		{
			return ByteString.CopyFrom(text, Encoding.UTF8);
		}

		// Token: 0x17000005 RID: 5
		public byte this[int index]
		{
			get
			{
				return this.bytes[index];
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000223F File Offset: 0x0000043F
		public string ToString(Encoding encoding)
		{
			return encoding.GetString(this.bytes, 0, this.bytes.Length);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002256 File Offset: 0x00000456
		public string ToStringUtf8()
		{
			return this.ToString(Encoding.UTF8);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002263 File Offset: 0x00000463
		public IEnumerator<byte> GetEnumerator()
		{
			return ((IEnumerable<byte>)this.bytes).GetEnumerator();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002270 File Offset: 0x00000470
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002278 File Offset: 0x00000478
		public CodedInputStream CreateCodedInput()
		{
			return new CodedInputStream(this.bytes);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002288 File Offset: 0x00000488
		public static bool operator ==(ByteString lhs, ByteString rhs)
		{
			if (lhs == rhs)
			{
				return true;
			}
			if (lhs == null || rhs == null)
			{
				return false;
			}
			if (lhs.bytes.Length != rhs.bytes.Length)
			{
				return false;
			}
			for (int i = 0; i < lhs.Length; i++)
			{
				if (rhs.bytes[i] != lhs.bytes[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022DD File Offset: 0x000004DD
		public static bool operator !=(ByteString lhs, ByteString rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022E9 File Offset: 0x000004E9
		public override bool Equals(object obj)
		{
			return this == obj as ByteString;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022F8 File Offset: 0x000004F8
		public override int GetHashCode()
		{
			int num = 23;
			foreach (byte b in this.bytes)
			{
				num = num * 31 + (int)b;
			}
			return num;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002329 File Offset: 0x00000529
		public bool Equals(ByteString other)
		{
			return this == other;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002332 File Offset: 0x00000532
		internal void WriteRawBytesTo(CodedOutputStream outputStream)
		{
			outputStream.WriteRawBytes(this.bytes, 0, this.bytes.Length);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002349 File Offset: 0x00000549
		public void CopyTo(byte[] array, int position)
		{
			ByteArray.Copy(this.bytes, 0, array, position, this.bytes.Length);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002361 File Offset: 0x00000561
		public void WriteTo(Stream outputStream)
		{
			outputStream.Write(this.bytes, 0, this.bytes.Length);
		}

		// Token: 0x04000002 RID: 2
		private static readonly ByteString empty = new ByteString(new byte[0]);

		// Token: 0x04000003 RID: 3
		private readonly byte[] bytes;

		// Token: 0x0200008A RID: 138
		internal static class Unsafe
		{
			// Token: 0x060008B5 RID: 2229 RVA: 0x0001E809 File Offset: 0x0001CA09
			internal static ByteString FromBytes(byte[] bytes)
			{
				return new ByteString(bytes);
			}
		}
	}
}
