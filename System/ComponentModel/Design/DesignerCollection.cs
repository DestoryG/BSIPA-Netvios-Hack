using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005DD RID: 1501
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerCollection : ICollection, IEnumerable
	{
		// Token: 0x060037BC RID: 14268 RVA: 0x000F0D02 File Offset: 0x000EEF02
		public DesignerCollection(IDesignerHost[] designers)
		{
			if (designers != null)
			{
				this.designers = new ArrayList(designers);
				return;
			}
			this.designers = new ArrayList();
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x000F0D25 File Offset: 0x000EEF25
		public DesignerCollection(IList designers)
		{
			this.designers = designers;
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x000F0D34 File Offset: 0x000EEF34
		public int Count
		{
			get
			{
				return this.designers.Count;
			}
		}

		// Token: 0x17000D65 RID: 3429
		public virtual IDesignerHost this[int index]
		{
			get
			{
				return (IDesignerHost)this.designers[index];
			}
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x000F0D54 File Offset: 0x000EEF54
		public IEnumerator GetEnumerator()
		{
			return this.designers.GetEnumerator();
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x060037C1 RID: 14273 RVA: 0x000F0D61 File Offset: 0x000EEF61
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x060037C2 RID: 14274 RVA: 0x000F0D69 File Offset: 0x000EEF69
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000F0D6C File Offset: 0x000EEF6C
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000F0D6F File Offset: 0x000EEF6F
		void ICollection.CopyTo(Array array, int index)
		{
			this.designers.CopyTo(array, index);
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000F0D7E File Offset: 0x000EEF7E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04002AF8 RID: 11000
		private IList designers;
	}
}
