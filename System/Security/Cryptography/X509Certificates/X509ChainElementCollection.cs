using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200046F RID: 1135
	public sealed class X509ChainElementCollection : ICollection, IEnumerable
	{
		// Token: 0x06002A36 RID: 10806 RVA: 0x000C0CCE File Offset: 0x000BEECE
		internal X509ChainElementCollection()
		{
			this.m_elements = new X509ChainElement[0];
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000C0CE4 File Offset: 0x000BEEE4
		internal unsafe X509ChainElementCollection(IntPtr pSimpleChain)
		{
			CAPIBase.CERT_SIMPLE_CHAIN cert_SIMPLE_CHAIN = new CAPIBase.CERT_SIMPLE_CHAIN(Marshal.SizeOf(typeof(CAPIBase.CERT_SIMPLE_CHAIN)));
			uint num = (uint)Marshal.ReadInt32(pSimpleChain);
			if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_SIMPLE_CHAIN)))
			{
				num = (uint)Marshal.SizeOf(cert_SIMPLE_CHAIN);
			}
			X509Utils.memcpy(pSimpleChain, new IntPtr((void*)(&cert_SIMPLE_CHAIN)), num);
			this.m_elements = new X509ChainElement[cert_SIMPLE_CHAIN.cElement];
			for (int i = 0; i < this.m_elements.Length; i++)
			{
				this.m_elements[i] = new X509ChainElement(Marshal.ReadIntPtr(new IntPtr((long)cert_SIMPLE_CHAIN.rgpElement + (long)(i * Marshal.SizeOf(typeof(IntPtr))))));
			}
		}

		// Token: 0x17000A3D RID: 2621
		public X509ChainElement this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_elements.Length)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
				}
				return this.m_elements[index];
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000C0DD6 File Offset: 0x000BEFD6
		public int Count
		{
			get
			{
				return this.m_elements.Length;
			}
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000C0DE0 File Offset: 0x000BEFE0
		public X509ChainElementEnumerator GetEnumerator()
		{
			return new X509ChainElementEnumerator(this);
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000C0DE8 File Offset: 0x000BEFE8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new X509ChainElementEnumerator(this);
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000C0DF0 File Offset: 0x000BEFF0
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000C0E8A File Offset: 0x000BF08A
		public void CopyTo(X509ChainElement[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x000C0E94 File Offset: 0x000BF094
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002A3F RID: 10815 RVA: 0x000C0E97 File Offset: 0x000BF097
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x040025FC RID: 9724
		private X509ChainElement[] m_elements;
	}
}
