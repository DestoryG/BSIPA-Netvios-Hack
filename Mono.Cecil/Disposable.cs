using System;

namespace Mono
{
	// Token: 0x02000003 RID: 3
	internal static class Disposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Disposable<T> Owned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, true);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002059 File Offset: 0x00000259
		public static Disposable<T> NotOwned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, false);
		}
	}
}
