using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000117 RID: 279
	internal struct InterlockedGate
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x0003D140 File Offset: 0x0003B340
		internal void Reset()
		{
			this.m_State = 0;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0003D14C File Offset: 0x0003B34C
		internal bool Trigger(bool exclusive)
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 2, 0);
			if (exclusive && (num == 1 || num == 2))
			{
				throw new InternalException();
			}
			return num == 0;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0003D17C File Offset: 0x0003B37C
		internal bool StartTriggering(bool exclusive)
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 1, 0);
			if (exclusive && (num == 1 || num == 2))
			{
				throw new InternalException();
			}
			return num == 0;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0003D1AC File Offset: 0x0003B3AC
		internal void FinishTriggering()
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 2, 1);
			if (num != 1)
			{
				throw new InternalException();
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0003D1D4 File Offset: 0x0003B3D4
		internal bool StartSignaling(bool exclusive)
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 3, 2);
			if (exclusive && (num == 3 || num == 4))
			{
				throw new InternalException();
			}
			return num == 2;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0003D204 File Offset: 0x0003B404
		internal void FinishSignaling()
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 4, 3);
			if (num != 3)
			{
				throw new InternalException();
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0003D22C File Offset: 0x0003B42C
		internal bool Complete()
		{
			int num = Interlocked.CompareExchange(ref this.m_State, 5, 4);
			return num == 4;
		}

		// Token: 0x04000F60 RID: 3936
		private int m_State;

		// Token: 0x04000F61 RID: 3937
		internal const int Open = 0;

		// Token: 0x04000F62 RID: 3938
		internal const int Triggering = 1;

		// Token: 0x04000F63 RID: 3939
		internal const int Triggered = 2;

		// Token: 0x04000F64 RID: 3940
		internal const int Signaling = 3;

		// Token: 0x04000F65 RID: 3941
		internal const int Signaled = 4;

		// Token: 0x04000F66 RID: 3942
		internal const int Completed = 5;
	}
}
