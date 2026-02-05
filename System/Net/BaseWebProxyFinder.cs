using System;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020000C7 RID: 199
	internal abstract class BaseWebProxyFinder : IWebProxyFinder, IDisposable
	{
		// Token: 0x060006A7 RID: 1703 RVA: 0x00025352 File Offset: 0x00023552
		public BaseWebProxyFinder(AutoWebProxyScriptEngine engine)
		{
			this.engine = engine;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00025361 File Offset: 0x00023561
		public bool IsValid
		{
			get
			{
				return this.state == BaseWebProxyFinder.AutoWebProxyState.Completed || this.state == BaseWebProxyFinder.AutoWebProxyState.Uninitialized;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x00025377 File Offset: 0x00023577
		public bool IsUnrecognizedScheme
		{
			get
			{
				return this.state == BaseWebProxyFinder.AutoWebProxyState.UnrecognizedScheme;
			}
		}

		// Token: 0x060006AA RID: 1706
		public abstract bool GetProxies(Uri destination, out IList<string> proxyList);

		// Token: 0x060006AB RID: 1707
		public abstract void Abort();

		// Token: 0x060006AC RID: 1708 RVA: 0x00025382 File Offset: 0x00023582
		public virtual void Reset()
		{
			this.State = BaseWebProxyFinder.AutoWebProxyState.Uninitialized;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002538B File Offset: 0x0002358B
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x00025394 File Offset: 0x00023594
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x0002539C File Offset: 0x0002359C
		protected BaseWebProxyFinder.AutoWebProxyState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x000253A5 File Offset: 0x000235A5
		protected AutoWebProxyScriptEngine Engine
		{
			get
			{
				return this.engine;
			}
		}

		// Token: 0x060006B1 RID: 1713
		protected abstract void Dispose(bool disposing);

		// Token: 0x04000C8C RID: 3212
		private BaseWebProxyFinder.AutoWebProxyState state;

		// Token: 0x04000C8D RID: 3213
		private AutoWebProxyScriptEngine engine;

		// Token: 0x020006ED RID: 1773
		protected enum AutoWebProxyState
		{
			// Token: 0x04003070 RID: 12400
			Uninitialized,
			// Token: 0x04003071 RID: 12401
			DiscoveryFailure,
			// Token: 0x04003072 RID: 12402
			DownloadFailure,
			// Token: 0x04003073 RID: 12403
			CompilationFailure,
			// Token: 0x04003074 RID: 12404
			UnrecognizedScheme,
			// Token: 0x04003075 RID: 12405
			Completed
		}
	}
}
