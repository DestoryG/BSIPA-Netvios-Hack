using System;

namespace Mono
{
	// Token: 0x020000AE RID: 174
	internal struct Disposable<T> : IDisposable where T : class, IDisposable
	{
		// Token: 0x0600035B RID: 859 RVA: 0x000101C8 File Offset: 0x0000E3C8
		public Disposable(T value, bool owned)
		{
			this.value = value;
			this.owned = owned;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000101D8 File Offset: 0x0000E3D8
		public void Dispose()
		{
			if (this.value != null && this.owned)
			{
				this.value.Dispose();
			}
		}

		// Token: 0x040001E3 RID: 483
		internal readonly T value;

		// Token: 0x040001E4 RID: 484
		private readonly bool owned;
	}
}
