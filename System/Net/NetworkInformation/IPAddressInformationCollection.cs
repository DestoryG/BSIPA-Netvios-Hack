using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A0 RID: 672
	[global::__DynamicallyInvokable]
	public class IPAddressInformationCollection : ICollection<IPAddressInformation>, IEnumerable<IPAddressInformation>, IEnumerable
	{
		// Token: 0x06001902 RID: 6402 RVA: 0x0007DBD1 File Offset: 0x0007BDD1
		internal IPAddressInformationCollection()
		{
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0007DBE4 File Offset: 0x0007BDE4
		[global::__DynamicallyInvokable]
		public virtual void CopyTo(IPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0007DBF3 File Offset: 0x0007BDF3
		[global::__DynamicallyInvokable]
		public virtual int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x0007DC00 File Offset: 0x0007BE00
		[global::__DynamicallyInvokable]
		public virtual bool IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return true;
			}
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0007DC03 File Offset: 0x0007BE03
		[global::__DynamicallyInvokable]
		public virtual void Add(IPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0007DC14 File Offset: 0x0007BE14
		internal void InternalAdd(IPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x0007DC22 File Offset: 0x0007BE22
		[global::__DynamicallyInvokable]
		public virtual bool Contains(IPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x0007DC30 File Offset: 0x0007BE30
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0007DC38 File Offset: 0x0007BE38
		[global::__DynamicallyInvokable]
		public virtual IEnumerator<IPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x1700055A RID: 1370
		[global::__DynamicallyInvokable]
		public virtual IPAddressInformation this[int index]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0007DC53 File Offset: 0x0007BE53
		[global::__DynamicallyInvokable]
		public virtual bool Remove(IPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0007DC64 File Offset: 0x0007BE64
		[global::__DynamicallyInvokable]
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x040018C2 RID: 6338
		private Collection<IPAddressInformation> addresses = new Collection<IPAddressInformation>();
	}
}
