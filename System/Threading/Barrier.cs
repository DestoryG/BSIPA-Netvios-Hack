using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x020003D6 RID: 982
	[ComVisible(false)]
	[DebuggerDisplay("Participant Count={ParticipantCount},Participants Remaining={ParticipantsRemaining}")]
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class Barrier : IDisposable
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x000AF478 File Offset: 0x000AD678
		[global::__DynamicallyInvokable]
		public int ParticipantsRemaining
		{
			[global::__DynamicallyInvokable]
			get
			{
				int currentTotalCount = this.m_currentTotalCount;
				int num = currentTotalCount & 32767;
				int num2 = (currentTotalCount & 2147418112) >> 16;
				return num - num2;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x000AF4A4 File Offset: 0x000AD6A4
		[global::__DynamicallyInvokable]
		public int ParticipantCount
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_currentTotalCount & 32767;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x000AF4B4 File Offset: 0x000AD6B4
		// (set) Token: 0x060025C2 RID: 9666 RVA: 0x000AF4C1 File Offset: 0x000AD6C1
		[global::__DynamicallyInvokable]
		public long CurrentPhaseNumber
		{
			[global::__DynamicallyInvokable]
			get
			{
				return Volatile.Read(ref this.m_currentPhase);
			}
			internal set
			{
				Volatile.Write(ref this.m_currentPhase, value);
			}
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000AF4CF File Offset: 0x000AD6CF
		[global::__DynamicallyInvokable]
		public Barrier(int participantCount)
			: this(participantCount, null)
		{
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000AF4DC File Offset: 0x000AD6DC
		[global::__DynamicallyInvokable]
		public Barrier(int participantCount, Action<Barrier> postPhaseAction)
		{
			if (participantCount < 0 || participantCount > 32767)
			{
				throw new ArgumentOutOfRangeException("participantCount", participantCount, SR.GetString("Barrier_ctor_ArgumentOutOfRange"));
			}
			this.m_currentTotalCount = participantCount;
			this.m_postPhaseAction = postPhaseAction;
			this.m_oddEvent = new ManualResetEventSlim(true);
			this.m_evenEvent = new ManualResetEventSlim(false);
			if (postPhaseAction != null && !ExecutionContext.IsFlowSuppressed())
			{
				this.m_ownerThreadContext = ExecutionContext.Capture();
			}
			this.m_actionCallerID = 0;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000AF55A File Offset: 0x000AD75A
		private void GetCurrentTotal(int currentTotal, out int current, out int total, out bool sense)
		{
			total = currentTotal & 32767;
			current = (currentTotal & 2147418112) >> 16;
			sense = (currentTotal & int.MinValue) == 0;
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000AF584 File Offset: 0x000AD784
		private bool SetCurrentTotal(int currentTotal, int current, int total, bool sense)
		{
			int num = (current << 16) | total;
			if (!sense)
			{
				num |= int.MinValue;
			}
			return Interlocked.CompareExchange(ref this.m_currentTotalCount, num, currentTotal) == currentTotal;
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000AF5B4 File Offset: 0x000AD7B4
		[global::__DynamicallyInvokable]
		public long AddParticipant()
		{
			long num;
			try
			{
				num = this.AddParticipants(1);
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new InvalidOperationException(SR.GetString("Barrier_AddParticipants_Overflow_ArgumentOutOfRange"));
			}
			return num;
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000AF5F0 File Offset: 0x000AD7F0
		[global::__DynamicallyInvokable]
		public long AddParticipants(int participantCount)
		{
			this.ThrowIfDisposed();
			if (participantCount < 1)
			{
				throw new ArgumentOutOfRangeException("participantCount", participantCount, SR.GetString("Barrier_AddParticipants_NonPositive_ArgumentOutOfRange"));
			}
			if (participantCount > 32767)
			{
				throw new ArgumentOutOfRangeException("participantCount", SR.GetString("Barrier_AddParticipants_Overflow_ArgumentOutOfRange"));
			}
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
			{
				throw new InvalidOperationException(SR.GetString("Barrier_InvalidOperation_CalledFromPHA"));
			}
			SpinWait spinWait = default(SpinWait);
			bool flag;
			for (;;)
			{
				int currentTotalCount = this.m_currentTotalCount;
				int num;
				int num2;
				this.GetCurrentTotal(currentTotalCount, out num, out num2, out flag);
				if (participantCount + num2 > 32767)
				{
					break;
				}
				if (this.SetCurrentTotal(currentTotalCount, num, num2 + participantCount, flag))
				{
					goto Block_6;
				}
				spinWait.SpinOnce();
			}
			throw new ArgumentOutOfRangeException("participantCount", SR.GetString("Barrier_AddParticipants_Overflow_ArgumentOutOfRange"));
			Block_6:
			long currentPhaseNumber = this.CurrentPhaseNumber;
			long num3 = ((flag != (currentPhaseNumber % 2L == 0L)) ? (currentPhaseNumber + 1L) : currentPhaseNumber);
			if (num3 != currentPhaseNumber)
			{
				if (flag)
				{
					this.m_oddEvent.Wait();
				}
				else
				{
					this.m_evenEvent.Wait();
				}
			}
			else if (flag && this.m_evenEvent.IsSet)
			{
				this.m_evenEvent.Reset();
			}
			else if (!flag && this.m_oddEvent.IsSet)
			{
				this.m_oddEvent.Reset();
			}
			return num3;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000AF744 File Offset: 0x000AD944
		[global::__DynamicallyInvokable]
		public void RemoveParticipant()
		{
			this.RemoveParticipants(1);
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000AF750 File Offset: 0x000AD950
		[global::__DynamicallyInvokable]
		public void RemoveParticipants(int participantCount)
		{
			this.ThrowIfDisposed();
			if (participantCount < 1)
			{
				throw new ArgumentOutOfRangeException("participantCount", participantCount, SR.GetString("Barrier_RemoveParticipants_NonPositive_ArgumentOutOfRange"));
			}
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
			{
				throw new InvalidOperationException(SR.GetString("Barrier_InvalidOperation_CalledFromPHA"));
			}
			SpinWait spinWait = default(SpinWait);
			bool flag;
			for (;;)
			{
				int currentTotalCount = this.m_currentTotalCount;
				int num;
				int num2;
				this.GetCurrentTotal(currentTotalCount, out num, out num2, out flag);
				if (num2 < participantCount)
				{
					break;
				}
				if (num2 - participantCount < num)
				{
					goto Block_5;
				}
				int num3 = num2 - participantCount;
				if (num3 > 0 && num == num3)
				{
					if (this.SetCurrentTotal(currentTotalCount, 0, num2 - participantCount, !flag))
					{
						goto Block_8;
					}
				}
				else if (this.SetCurrentTotal(currentTotalCount, num, num2 - participantCount, flag))
				{
					return;
				}
				spinWait.SpinOnce();
			}
			throw new ArgumentOutOfRangeException("participantCount", SR.GetString("Barrier_RemoveParticipants_ArgumentOutOfRange"));
			Block_5:
			throw new InvalidOperationException(SR.GetString("Barrier_RemoveParticipants_InvalidOperation"));
			Block_8:
			this.FinishPhase(flag);
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000AF840 File Offset: 0x000ADA40
		[global::__DynamicallyInvokable]
		public void SignalAndWait()
		{
			this.SignalAndWait(default(CancellationToken));
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000AF85C File Offset: 0x000ADA5C
		[global::__DynamicallyInvokable]
		public void SignalAndWait(CancellationToken cancellationToken)
		{
			this.SignalAndWait(-1, cancellationToken);
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000AF868 File Offset: 0x000ADA68
		[global::__DynamicallyInvokable]
		public bool SignalAndWait(TimeSpan timeout)
		{
			return this.SignalAndWait(timeout, default(CancellationToken));
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000AF888 File Offset: 0x000ADA88
		[global::__DynamicallyInvokable]
		public bool SignalAndWait(TimeSpan timeout, CancellationToken cancellationToken)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", timeout, SR.GetString("Barrier_SignalAndWait_ArgumentOutOfRange"));
			}
			return this.SignalAndWait((int)timeout.TotalMilliseconds, cancellationToken);
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x000AF8D8 File Offset: 0x000ADAD8
		[global::__DynamicallyInvokable]
		public bool SignalAndWait(int millisecondsTimeout)
		{
			return this.SignalAndWait(millisecondsTimeout, default(CancellationToken));
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000AF8F8 File Offset: 0x000ADAF8
		[global::__DynamicallyInvokable]
		public bool SignalAndWait(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			this.ThrowIfDisposed();
			cancellationToken.ThrowIfCancellationRequested();
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, SR.GetString("Barrier_SignalAndWait_ArgumentOutOfRange"));
			}
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
			{
				throw new InvalidOperationException(SR.GetString("Barrier_InvalidOperation_CalledFromPHA"));
			}
			SpinWait spinWait = default(SpinWait);
			bool flag;
			long currentPhaseNumber;
			for (;;)
			{
				int num = this.m_currentTotalCount;
				int num2;
				int num3;
				this.GetCurrentTotal(num, out num2, out num3, out flag);
				currentPhaseNumber = this.CurrentPhaseNumber;
				if (num3 == 0)
				{
					break;
				}
				if (num2 == 0 && flag != (this.CurrentPhaseNumber % 2L == 0L))
				{
					goto Block_6;
				}
				if (num2 + 1 == num3)
				{
					if (this.SetCurrentTotal(num, 0, num3, !flag))
					{
						goto Block_8;
					}
				}
				else if (this.SetCurrentTotal(num, num2 + 1, num3, flag))
				{
					goto IL_0107;
				}
				spinWait.SpinOnce();
			}
			throw new InvalidOperationException(SR.GetString("Barrier_SignalAndWait_InvalidOperation_ZeroTotal"));
			Block_6:
			throw new InvalidOperationException(SR.GetString("Barrier_SignalAndWait_InvalidOperation_ThreadsExceeded"));
			Block_8:
			if (CdsSyncEtwBCLProvider.Log.IsEnabled())
			{
				CdsSyncEtwBCLProvider.Log.Barrier_PhaseFinished(flag, this.CurrentPhaseNumber);
			}
			this.FinishPhase(flag);
			return true;
			IL_0107:
			ManualResetEventSlim manualResetEventSlim = (flag ? this.m_evenEvent : this.m_oddEvent);
			bool flag2 = false;
			bool flag3 = false;
			try
			{
				flag3 = this.DiscontinuousWait(manualResetEventSlim, millisecondsTimeout, cancellationToken, currentPhaseNumber);
			}
			catch (OperationCanceledException)
			{
				flag2 = true;
			}
			catch (ObjectDisposedException)
			{
				if (currentPhaseNumber >= this.CurrentPhaseNumber)
				{
					throw;
				}
				flag3 = true;
			}
			if (!flag3)
			{
				spinWait.Reset();
				for (;;)
				{
					int num = this.m_currentTotalCount;
					int num2;
					int num3;
					bool flag4;
					this.GetCurrentTotal(num, out num2, out num3, out flag4);
					if (currentPhaseNumber < this.CurrentPhaseNumber || flag != flag4)
					{
						break;
					}
					if (this.SetCurrentTotal(num, num2 - 1, num3, flag))
					{
						goto Block_14;
					}
					spinWait.SpinOnce();
				}
				this.WaitCurrentPhase(manualResetEventSlim, currentPhaseNumber);
				goto IL_01B4;
				Block_14:
				if (flag2)
				{
					throw new OperationCanceledException(SR.GetString("Common_OperationCanceled"), cancellationToken);
				}
				return false;
			}
			IL_01B4:
			if (this.m_exception != null)
			{
				throw new BarrierPostPhaseException(this.m_exception);
			}
			return true;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000AFAEC File Offset: 0x000ADCEC
		[SecuritySafeCritical]
		private void FinishPhase(bool observedSense)
		{
			if (this.m_postPhaseAction != null)
			{
				try
				{
					this.m_actionCallerID = Thread.CurrentThread.ManagedThreadId;
					if (this.m_ownerThreadContext != null)
					{
						ExecutionContext ownerThreadContext = this.m_ownerThreadContext;
						this.m_ownerThreadContext = this.m_ownerThreadContext.CreateCopy();
						ContextCallback contextCallback = Barrier.s_invokePostPhaseAction;
						if (contextCallback == null)
						{
							contextCallback = (Barrier.s_invokePostPhaseAction = new ContextCallback(Barrier.InvokePostPhaseAction));
						}
						ExecutionContext.Run(ownerThreadContext, contextCallback, this);
						ownerThreadContext.Dispose();
					}
					else
					{
						this.m_postPhaseAction(this);
					}
					this.m_exception = null;
					return;
				}
				catch (Exception ex)
				{
					this.m_exception = ex;
					return;
				}
				finally
				{
					this.m_actionCallerID = 0;
					this.SetResetEvents(observedSense);
					if (this.m_exception != null)
					{
						throw new BarrierPostPhaseException(this.m_exception);
					}
				}
			}
			this.SetResetEvents(observedSense);
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000AFBC8 File Offset: 0x000ADDC8
		[SecurityCritical]
		private static void InvokePostPhaseAction(object obj)
		{
			Barrier barrier = (Barrier)obj;
			barrier.m_postPhaseAction(barrier);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000AFBE8 File Offset: 0x000ADDE8
		private void SetResetEvents(bool observedSense)
		{
			this.CurrentPhaseNumber += 1L;
			if (observedSense)
			{
				this.m_oddEvent.Reset();
				this.m_evenEvent.Set();
				return;
			}
			this.m_evenEvent.Reset();
			this.m_oddEvent.Set();
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000AFC34 File Offset: 0x000ADE34
		private void WaitCurrentPhase(ManualResetEventSlim currentPhaseEvent, long observedPhase)
		{
			SpinWait spinWait = default(SpinWait);
			while (!currentPhaseEvent.IsSet && this.CurrentPhaseNumber - observedPhase <= 1L)
			{
				spinWait.SpinOnce();
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x000AFC68 File Offset: 0x000ADE68
		private bool DiscontinuousWait(ManualResetEventSlim currentPhaseEvent, int totalTimeout, CancellationToken token, long observedPhase)
		{
			int num = 100;
			int num2 = 10000;
			while (observedPhase == this.CurrentPhaseNumber)
			{
				int num3 = ((totalTimeout == -1) ? num : Math.Min(num, totalTimeout));
				if (currentPhaseEvent.Wait(num3, token))
				{
					return true;
				}
				if (totalTimeout != -1)
				{
					totalTimeout -= num3;
					if (totalTimeout <= 0)
					{
						return false;
					}
				}
				num = ((num >= num2) ? num2 : Math.Min(num << 1, num2));
			}
			this.WaitCurrentPhase(currentPhaseEvent, observedPhase);
			return true;
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000AFCCF File Offset: 0x000ADECF
		[global::__DynamicallyInvokable]
		public void Dispose()
		{
			if (this.m_actionCallerID != 0 && Thread.CurrentThread.ManagedThreadId == this.m_actionCallerID)
			{
				throw new InvalidOperationException(SR.GetString("Barrier_InvalidOperation_CalledFromPHA"));
			}
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000AFD08 File Offset: 0x000ADF08
		[global::__DynamicallyInvokable]
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (disposing)
				{
					this.m_oddEvent.Dispose();
					this.m_evenEvent.Dispose();
					if (this.m_ownerThreadContext != null)
					{
						this.m_ownerThreadContext.Dispose();
						this.m_ownerThreadContext = null;
					}
				}
				this.m_disposed = true;
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000AFD57 File Offset: 0x000ADF57
		private void ThrowIfDisposed()
		{
			if (this.m_disposed)
			{
				throw new ObjectDisposedException("Barrier", SR.GetString("Barrier_Dispose"));
			}
		}

		// Token: 0x04002058 RID: 8280
		private volatile int m_currentTotalCount;

		// Token: 0x04002059 RID: 8281
		private const int CURRENT_MASK = 2147418112;

		// Token: 0x0400205A RID: 8282
		private const int TOTAL_MASK = 32767;

		// Token: 0x0400205B RID: 8283
		private const int SENSE_MASK = -2147483648;

		// Token: 0x0400205C RID: 8284
		private const int MAX_PARTICIPANTS = 32767;

		// Token: 0x0400205D RID: 8285
		private long m_currentPhase;

		// Token: 0x0400205E RID: 8286
		private bool m_disposed;

		// Token: 0x0400205F RID: 8287
		private ManualResetEventSlim m_oddEvent;

		// Token: 0x04002060 RID: 8288
		private ManualResetEventSlim m_evenEvent;

		// Token: 0x04002061 RID: 8289
		private ExecutionContext m_ownerThreadContext;

		// Token: 0x04002062 RID: 8290
		[SecurityCritical]
		private static ContextCallback s_invokePostPhaseAction;

		// Token: 0x04002063 RID: 8291
		private Action<Barrier> m_postPhaseAction;

		// Token: 0x04002064 RID: 8292
		private Exception m_exception;

		// Token: 0x04002065 RID: 8293
		private int m_actionCallerID;
	}
}
