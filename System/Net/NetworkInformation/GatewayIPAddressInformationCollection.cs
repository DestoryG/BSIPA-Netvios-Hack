using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002B1 RID: 689
	[global::__DynamicallyInvokable]
	public class GatewayIPAddressInformationCollection : ICollection<GatewayIPAddressInformation>, IEnumerable<GatewayIPAddressInformation>, IEnumerable
	{
		// Token: 0x0600199C RID: 6556 RVA: 0x0007DF68 File Offset: 0x0007C168
		[global::__DynamicallyInvokable]
		protected internal GatewayIPAddressInformationCollection()
		{
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0007DF7B File Offset: 0x0007C17B
		[global::__DynamicallyInvokable]
		public virtual void CopyTo(GatewayIPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x0007DF8A File Offset: 0x0007C18A
		[global::__DynamicallyInvokable]
		public virtual int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x0007DF97 File Offset: 0x0007C197
		[global::__DynamicallyInvokable]
		public virtual bool IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return true;
			}
		}

		// Token: 0x170005B3 RID: 1459
		[global::__DynamicallyInvokable]
		public virtual GatewayIPAddressInformation this[int index]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0007DFA8 File Offset: 0x0007C1A8
		[global::__DynamicallyInvokable]
		public virtual void Add(GatewayIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0007DFB9 File Offset: 0x0007C1B9
		internal void InternalAdd(GatewayIPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0007DFC7 File Offset: 0x0007C1C7
		[global::__DynamicallyInvokable]
		public virtual bool Contains(GatewayIPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0007DFD5 File Offset: 0x0007C1D5
		[global::__DynamicallyInvokable]
		public virtual IEnumerator<GatewayIPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0007DFE2 File Offset: 0x0007C1E2
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0007DFEA File Offset: 0x0007C1EA
		[global::__DynamicallyInvokable]
		public virtual bool Remove(GatewayIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0007DFFB File Offset: 0x0007C1FB
		[global::__DynamicallyInvokable]
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04001906 RID: 6406
		private Collection<GatewayIPAddressInformation> addresses = new Collection<GatewayIPAddressInformation>();
	}
}
