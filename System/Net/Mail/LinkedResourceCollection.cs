using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000269 RID: 617
	public sealed class LinkedResourceCollection : Collection<LinkedResource>, IDisposable
	{
		// Token: 0x0600172A RID: 5930 RVA: 0x0007666C File Offset: 0x0007486C
		internal LinkedResourceCollection()
		{
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00076674 File Offset: 0x00074874
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (LinkedResource linkedResource in this)
			{
				linkedResource.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x000766D4 File Offset: 0x000748D4
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x000766F6 File Offset: 0x000748F6
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00076717 File Offset: 0x00074917
		protected override void SetItem(int index, LinkedResource item)
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

		// Token: 0x0600172F RID: 5935 RVA: 0x00076748 File Offset: 0x00074948
		protected override void InsertItem(int index, LinkedResource item)
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

		// Token: 0x0400179F RID: 6047
		private bool disposed;
	}
}
