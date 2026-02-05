using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002AB RID: 683
	[global::__DynamicallyInvokable]
	public class UnicastIPAddressInformationCollection : ICollection<UnicastIPAddressInformation>, IEnumerable<UnicastIPAddressInformation>, IEnumerable
	{
		// Token: 0x0600196C RID: 6508 RVA: 0x0007DD01 File Offset: 0x0007BF01
		[global::__DynamicallyInvokable]
		protected internal UnicastIPAddressInformationCollection()
		{
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0007DD14 File Offset: 0x0007BF14
		[global::__DynamicallyInvokable]
		public virtual void CopyTo(UnicastIPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x0007DD23 File Offset: 0x0007BF23
		[global::__DynamicallyInvokable]
		public virtual int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0007DD30 File Offset: 0x0007BF30
		[global::__DynamicallyInvokable]
		public virtual bool IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return true;
			}
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0007DD33 File Offset: 0x0007BF33
		[global::__DynamicallyInvokable]
		public virtual void Add(UnicastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0007DD44 File Offset: 0x0007BF44
		internal void InternalAdd(UnicastIPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0007DD52 File Offset: 0x0007BF52
		[global::__DynamicallyInvokable]
		public virtual bool Contains(UnicastIPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0007DD60 File Offset: 0x0007BF60
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0007DD68 File Offset: 0x0007BF68
		[global::__DynamicallyInvokable]
		public virtual IEnumerator<UnicastIPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x170005A2 RID: 1442
		[global::__DynamicallyInvokable]
		public virtual UnicastIPAddressInformation this[int index]
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0007DD83 File Offset: 0x0007BF83
		[global::__DynamicallyInvokable]
		public virtual bool Remove(UnicastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0007DD94 File Offset: 0x0007BF94
		[global::__DynamicallyInvokable]
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04001902 RID: 6402
		private Collection<UnicastIPAddressInformation> addresses = new Collection<UnicastIPAddressInformation>();
	}
}
