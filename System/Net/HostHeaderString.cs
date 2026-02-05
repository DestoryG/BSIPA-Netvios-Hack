using System;
using System.Text;

namespace System.Net
{
	// Token: 0x02000184 RID: 388
	internal class HostHeaderString
	{
		// Token: 0x06000E6B RID: 3691 RVA: 0x0004B776 File Offset: 0x00049976
		internal HostHeaderString()
		{
			this.Init(null);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0004B785 File Offset: 0x00049985
		internal HostHeaderString(string s)
		{
			this.Init(s);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0004B794 File Offset: 0x00049994
		private void Init(string s)
		{
			this.m_String = s;
			this.m_Converted = false;
			this.m_Bytes = null;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0004B7AC File Offset: 0x000499AC
		private void Convert()
		{
			if (this.m_String != null && !this.m_Converted)
			{
				this.m_Bytes = Encoding.Default.GetBytes(this.m_String);
				string @string = Encoding.Default.GetString(this.m_Bytes);
				if (string.Compare(this.m_String, @string, StringComparison.Ordinal) != 0)
				{
					this.m_Bytes = Encoding.UTF8.GetBytes(this.m_String);
				}
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0004B815 File Offset: 0x00049A15
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x0004B81D File Offset: 0x00049A1D
		internal string String
		{
			get
			{
				return this.m_String;
			}
			set
			{
				this.Init(value);
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0004B826 File Offset: 0x00049A26
		internal int ByteCount
		{
			get
			{
				this.Convert();
				return this.m_Bytes.Length;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0004B836 File Offset: 0x00049A36
		internal byte[] Bytes
		{
			get
			{
				this.Convert();
				return this.m_Bytes;
			}
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0004B844 File Offset: 0x00049A44
		internal void Copy(byte[] destBytes, int destByteIndex)
		{
			this.Convert();
			Array.Copy(this.m_Bytes, 0, destBytes, destByteIndex, this.m_Bytes.Length);
		}

		// Token: 0x0400126A RID: 4714
		private bool m_Converted;

		// Token: 0x0400126B RID: 4715
		private string m_String;

		// Token: 0x0400126C RID: 4716
		private byte[] m_Bytes;
	}
}
