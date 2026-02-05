using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000255 RID: 597
	public sealed class AlternateViewCollection : Collection<AlternateView>, IDisposable
	{
		// Token: 0x060016B3 RID: 5811 RVA: 0x00075460 File Offset: 0x00073660
		internal AlternateViewCollection()
		{
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00075468 File Offset: 0x00073668
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (AlternateView alternateView in this)
			{
				alternateView.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000754C8 File Offset: 0x000736C8
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x000754EA File Offset: 0x000736EA
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0007550B File Offset: 0x0007370B
		protected override void SetItem(int index, AlternateView item)
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

		// Token: 0x060016B8 RID: 5816 RVA: 0x0007553C File Offset: 0x0007373C
		protected override void InsertItem(int index, AlternateView item)
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

		// Token: 0x04001763 RID: 5987
		private bool disposed;
	}
}
