using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020001DF RID: 479
	internal abstract class ProxyChain : IEnumerable<Uri>, IEnumerable, IDisposable
	{
		// Token: 0x060012BA RID: 4794 RVA: 0x000635C8 File Offset: 0x000617C8
		protected ProxyChain(Uri destination)
		{
			this.m_Destination = destination;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000635E4 File Offset: 0x000617E4
		public IEnumerator<Uri> GetEnumerator()
		{
			ProxyChain.ProxyEnumerator proxyEnumerator = new ProxyChain.ProxyEnumerator(this);
			if (this.m_MainEnumerator == null)
			{
				this.m_MainEnumerator = proxyEnumerator;
			}
			return proxyEnumerator;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00063608 File Offset: 0x00061808
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00063610 File Offset: 0x00061810
		public virtual void Dispose()
		{
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x00063614 File Offset: 0x00061814
		internal IEnumerator<Uri> Enumerator
		{
			get
			{
				if (this.m_MainEnumerator != null)
				{
					return this.m_MainEnumerator;
				}
				return this.GetEnumerator();
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x00063638 File Offset: 0x00061838
		internal Uri Destination
		{
			get
			{
				return this.m_Destination;
			}
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00063640 File Offset: 0x00061840
		internal virtual void Abort()
		{
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00063642 File Offset: 0x00061842
		internal bool HttpAbort(HttpWebRequest request, WebException webException)
		{
			this.Abort();
			return true;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x0006364B File Offset: 0x0006184B
		internal HttpAbortDelegate HttpAbortDelegate
		{
			get
			{
				if (this.m_HttpAbortDelegate == null)
				{
					this.m_HttpAbortDelegate = new HttpAbortDelegate(this.HttpAbort);
				}
				return this.m_HttpAbortDelegate;
			}
		}

		// Token: 0x060012C3 RID: 4803
		protected abstract bool GetNextProxy(out Uri proxy);

		// Token: 0x0400150F RID: 5391
		private List<Uri> m_Cache = new List<Uri>();

		// Token: 0x04001510 RID: 5392
		private bool m_CacheComplete;

		// Token: 0x04001511 RID: 5393
		private ProxyChain.ProxyEnumerator m_MainEnumerator;

		// Token: 0x04001512 RID: 5394
		private Uri m_Destination;

		// Token: 0x04001513 RID: 5395
		private HttpAbortDelegate m_HttpAbortDelegate;

		// Token: 0x02000755 RID: 1877
		private class ProxyEnumerator : IEnumerator<Uri>, IDisposable, IEnumerator
		{
			// Token: 0x060041F9 RID: 16889 RVA: 0x001120CD File Offset: 0x001102CD
			internal ProxyEnumerator(ProxyChain chain)
			{
				this.m_Chain = chain;
			}

			// Token: 0x17000F13 RID: 3859
			// (get) Token: 0x060041FA RID: 16890 RVA: 0x001120E3 File Offset: 0x001102E3
			public Uri Current
			{
				get
				{
					if (this.m_Finished || this.m_CurrentIndex < 0)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.m_Chain.m_Cache[this.m_CurrentIndex];
				}
			}

			// Token: 0x17000F14 RID: 3860
			// (get) Token: 0x060041FB RID: 16891 RVA: 0x0011211C File Offset: 0x0011031C
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060041FC RID: 16892 RVA: 0x00112124 File Offset: 0x00110324
			public bool MoveNext()
			{
				if (this.m_Finished)
				{
					return false;
				}
				checked
				{
					this.m_CurrentIndex++;
					if (this.m_Chain.m_Cache.Count > this.m_CurrentIndex)
					{
						return true;
					}
					if (this.m_Chain.m_CacheComplete)
					{
						this.m_Finished = true;
						return false;
					}
					List<Uri> cache = this.m_Chain.m_Cache;
					bool flag2;
					lock (cache)
					{
						if (this.m_Chain.m_Cache.Count > this.m_CurrentIndex)
						{
							flag2 = true;
						}
						else if (this.m_Chain.m_CacheComplete)
						{
							this.m_Finished = true;
							flag2 = false;
						}
						else
						{
							Uri uri;
							while (this.m_Chain.GetNextProxy(out uri))
							{
								if (uri == null)
								{
									if (this.m_TriedDirect)
									{
										continue;
									}
									this.m_TriedDirect = true;
								}
								this.m_Chain.m_Cache.Add(uri);
								return true;
							}
							this.m_Finished = true;
							this.m_Chain.m_CacheComplete = true;
							flag2 = false;
						}
					}
					return flag2;
				}
			}

			// Token: 0x060041FD RID: 16893 RVA: 0x00112234 File Offset: 0x00110434
			public void Reset()
			{
				this.m_Finished = false;
				this.m_CurrentIndex = -1;
			}

			// Token: 0x060041FE RID: 16894 RVA: 0x00112244 File Offset: 0x00110444
			public void Dispose()
			{
			}

			// Token: 0x04003209 RID: 12809
			private ProxyChain m_Chain;

			// Token: 0x0400320A RID: 12810
			private bool m_Finished;

			// Token: 0x0400320B RID: 12811
			private int m_CurrentIndex = -1;

			// Token: 0x0400320C RID: 12812
			private bool m_TriedDirect;
		}
	}
}
