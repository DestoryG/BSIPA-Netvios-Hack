using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000258 RID: 600
	public sealed class AttachmentCollection : Collection<Attachment>, IDisposable
	{
		// Token: 0x060016E4 RID: 5860 RVA: 0x00075E20 File Offset: 0x00074020
		internal AttachmentCollection()
		{
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00075E28 File Offset: 0x00074028
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (Attachment attachment in this)
			{
				attachment.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00075E88 File Offset: 0x00074088
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00075EAA File Offset: 0x000740AA
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00075ECB File Offset: 0x000740CB
		protected override void SetItem(int index, Attachment item)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00075EFC File Offset: 0x000740FC
		protected override void InsertItem(int index, Attachment item)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x04001768 RID: 5992
		private bool disposed;
	}
}
