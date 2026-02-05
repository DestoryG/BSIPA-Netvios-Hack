using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002AE RID: 686
	[global::__DynamicallyInvokable]
	public class IPAddressCollection : ICollection<IPAddress>, IEnumerable<IPAddress>, IEnumerable
	{
		// Token: 0x0600198B RID: 6539 RVA: 0x0007DE51 File Offset: 0x0007C051
		[global::__DynamicallyInvokable]
		protected internal IPAddressCollection()
		{
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0007DE64 File Offset: 0x0007C064
		[global::__DynamicallyInvokable]
		public virtual void CopyTo(IPAddress[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600198D RID: 6541 RVA: 0x0007DE73 File Offset: 0x0007C073
		[global::__DynamicallyInvokable]
		public virtual int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x0007DE80 File Offset: 0x0007C080
		[global::__DynamicallyInvokable]
		public virtual bool IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return true;
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0007DE83 File Offset: 0x0007C083
		[global::__DynamicallyInvokable]
		public virtual void Add(IPAddress address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0007DE94 File Offset: 0x0007C094
		internal void InternalAdd(IPAddress address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0007DEA2 File Offset: 0x0007C0A2
		[global::__DynamicallyInvokable]
		public virtual bool Contains(IPAddress address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0007DEB0 File Offset: 0x0007C0B0
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0007DEB8 File Offset: 0x0007C0B8
		[global::__DynamicallyInvokable]
		public virtual IEnumerator<IPAddress> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x170005AE RID: 1454
		[global::__DynamicallyInvokable]
		public virtual IPAddress this[int index]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0007DED3 File Offset: 0x0007C0D3
		[global::__DynamicallyInvokable]
		public virtual bool Remove(IPAddress address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0007DEE4 File Offset: 0x0007C0E4
		[global::__DynamicallyInvokable]
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04001904 RID: 6404
		private Collection<IPAddress> addresses = new Collection<IPAddress>();
	}
}
