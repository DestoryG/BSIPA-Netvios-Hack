using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002AD RID: 685
	[global::__DynamicallyInvokable]
	public class MulticastIPAddressInformationCollection : ICollection<MulticastIPAddressInformation>, IEnumerable<MulticastIPAddressInformation>, IEnumerable
	{
		// Token: 0x0600197F RID: 6527 RVA: 0x0007DDAD File Offset: 0x0007BFAD
		[global::__DynamicallyInvokable]
		protected internal MulticastIPAddressInformationCollection()
		{
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0007DDC0 File Offset: 0x0007BFC0
		[global::__DynamicallyInvokable]
		public virtual void CopyTo(MulticastIPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x0007DDCF File Offset: 0x0007BFCF
		[global::__DynamicallyInvokable]
		public virtual int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x0007DDDC File Offset: 0x0007BFDC
		[global::__DynamicallyInvokable]
		public virtual bool IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return true;
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0007DDDF File Offset: 0x0007BFDF
		[global::__DynamicallyInvokable]
		public virtual void Add(MulticastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0007DDF0 File Offset: 0x0007BFF0
		internal void InternalAdd(MulticastIPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0007DDFE File Offset: 0x0007BFFE
		[global::__DynamicallyInvokable]
		public virtual bool Contains(MulticastIPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0007DE0C File Offset: 0x0007C00C
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0007DE14 File Offset: 0x0007C014
		[global::__DynamicallyInvokable]
		public virtual IEnumerator<MulticastIPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x170005AB RID: 1451
		[global::__DynamicallyInvokable]
		public virtual MulticastIPAddressInformation this[int index]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0007DE2F File Offset: 0x0007C02F
		[global::__DynamicallyInvokable]
		public virtual bool Remove(MulticastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0007DE40 File Offset: 0x0007C040
		[global::__DynamicallyInvokable]
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04001903 RID: 6403
		private Collection<MulticastIPAddressInformation> addresses = new Collection<MulticastIPAddressInformation>();
	}
}
