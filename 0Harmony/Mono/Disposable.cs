using System;

namespace Mono
{
	// Token: 0x020000AD RID: 173
	internal static class Disposable
	{
		// Token: 0x06000359 RID: 857 RVA: 0x000101B6 File Offset: 0x0000E3B6
		public static Disposable<T> Owned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, true);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000101BF File Offset: 0x0000E3BF
		public static Disposable<T> NotOwned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, false);
		}
	}
}
