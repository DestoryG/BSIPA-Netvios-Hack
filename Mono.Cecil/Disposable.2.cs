using System;

namespace Mono
{
	// Token: 0x02000004 RID: 4
	internal struct Disposable<T> : IDisposable where T : class, IDisposable
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002062 File Offset: 0x00000262
		public Disposable(T value, bool owned)
		{
			this.value = value;
			this.owned = owned;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002072 File Offset: 0x00000272
		public void Dispose()
		{
			if (this.value != null && this.owned)
			{
				this.value.Dispose();
			}
		}

		// Token: 0x04000003 RID: 3
		internal readonly T value;

		// Token: 0x04000004 RID: 4
		private readonly bool owned;
	}
}
