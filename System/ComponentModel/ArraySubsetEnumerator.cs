using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000510 RID: 1296
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal class ArraySubsetEnumerator : IEnumerator
	{
		// Token: 0x06003119 RID: 12569 RVA: 0x000DEA24 File Offset: 0x000DCC24
		public ArraySubsetEnumerator(Array array, int count)
		{
			this.array = array;
			this.total = count;
			this.current = -1;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000DEA41 File Offset: 0x000DCC41
		public bool MoveNext()
		{
			if (this.current < this.total - 1)
			{
				this.current++;
				return true;
			}
			return false;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000DEA64 File Offset: 0x000DCC64
		public void Reset()
		{
			this.current = -1;
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600311C RID: 12572 RVA: 0x000DEA6D File Offset: 0x000DCC6D
		public object Current
		{
			get
			{
				if (this.current == -1)
				{
					throw new InvalidOperationException();
				}
				return this.array.GetValue(this.current);
			}
		}

		// Token: 0x040028FB RID: 10491
		private Array array;

		// Token: 0x040028FC RID: 10492
		private int total;

		// Token: 0x040028FD RID: 10493
		private int current;
	}
}
