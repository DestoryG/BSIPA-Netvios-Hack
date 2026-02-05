using System;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x02000517 RID: 1303
	[SRDescription("BackgroundWorker_Desc")]
	[DefaultEvent("DoWork")]
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class BackgroundWorker : Component
	{
		// Token: 0x0600314D RID: 12621 RVA: 0x000DF1E9 File Offset: 0x000DD3E9
		[global::__DynamicallyInvokable]
		public BackgroundWorker()
		{
			this.threadStart = new BackgroundWorker.WorkerThreadStartDelegate(this.WorkerThreadStart);
			this.operationCompleted = new SendOrPostCallback(this.AsyncOperationCompleted);
			this.progressReporter = new SendOrPostCallback(this.ProgressReporter);
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x000DF227 File Offset: 0x000DD427
		private void AsyncOperationCompleted(object arg)
		{
			this.isRunning = false;
			this.cancellationPending = false;
			this.OnRunWorkerCompleted((RunWorkerCompletedEventArgs)arg);
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x000DF243 File Offset: 0x000DD443
		[Browsable(false)]
		[SRDescription("BackgroundWorker_CancellationPending")]
		[global::__DynamicallyInvokable]
		public bool CancellationPending
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.cancellationPending;
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000DF24B File Offset: 0x000DD44B
		[global::__DynamicallyInvokable]
		public void CancelAsync()
		{
			if (!this.WorkerSupportsCancellation)
			{
				throw new InvalidOperationException(SR.GetString("BackgroundWorker_WorkerDoesntSupportCancellation"));
			}
			this.cancellationPending = true;
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06003151 RID: 12625 RVA: 0x000DF26C File Offset: 0x000DD46C
		// (remove) Token: 0x06003152 RID: 12626 RVA: 0x000DF27F File Offset: 0x000DD47F
		[SRCategory("PropertyCategoryAsynchronous")]
		[SRDescription("BackgroundWorker_DoWork")]
		[global::__DynamicallyInvokable]
		public event DoWorkEventHandler DoWork
		{
			[global::__DynamicallyInvokable]
			add
			{
				base.Events.AddHandler(BackgroundWorker.doWorkKey, value);
			}
			[global::__DynamicallyInvokable]
			remove
			{
				base.Events.RemoveHandler(BackgroundWorker.doWorkKey, value);
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06003153 RID: 12627 RVA: 0x000DF292 File Offset: 0x000DD492
		[Browsable(false)]
		[SRDescription("BackgroundWorker_IsBusy")]
		[global::__DynamicallyInvokable]
		public bool IsBusy
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.isRunning;
			}
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000DF29C File Offset: 0x000DD49C
		[global::__DynamicallyInvokable]
		protected virtual void OnDoWork(DoWorkEventArgs e)
		{
			DoWorkEventHandler doWorkEventHandler = (DoWorkEventHandler)base.Events[BackgroundWorker.doWorkKey];
			if (doWorkEventHandler != null)
			{
				doWorkEventHandler(this, e);
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000DF2CC File Offset: 0x000DD4CC
		[global::__DynamicallyInvokable]
		protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
		{
			RunWorkerCompletedEventHandler runWorkerCompletedEventHandler = (RunWorkerCompletedEventHandler)base.Events[BackgroundWorker.runWorkerCompletedKey];
			if (runWorkerCompletedEventHandler != null)
			{
				runWorkerCompletedEventHandler(this, e);
			}
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000DF2FC File Offset: 0x000DD4FC
		[global::__DynamicallyInvokable]
		protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
		{
			ProgressChangedEventHandler progressChangedEventHandler = (ProgressChangedEventHandler)base.Events[BackgroundWorker.progressChangedKey];
			if (progressChangedEventHandler != null)
			{
				progressChangedEventHandler(this, e);
			}
		}

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06003157 RID: 12631 RVA: 0x000DF32A File Offset: 0x000DD52A
		// (remove) Token: 0x06003158 RID: 12632 RVA: 0x000DF33D File Offset: 0x000DD53D
		[SRCategory("PropertyCategoryAsynchronous")]
		[SRDescription("BackgroundWorker_ProgressChanged")]
		[global::__DynamicallyInvokable]
		public event ProgressChangedEventHandler ProgressChanged
		{
			[global::__DynamicallyInvokable]
			add
			{
				base.Events.AddHandler(BackgroundWorker.progressChangedKey, value);
			}
			[global::__DynamicallyInvokable]
			remove
			{
				base.Events.RemoveHandler(BackgroundWorker.progressChangedKey, value);
			}
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x000DF350 File Offset: 0x000DD550
		private void ProgressReporter(object arg)
		{
			this.OnProgressChanged((ProgressChangedEventArgs)arg);
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x000DF35E File Offset: 0x000DD55E
		[global::__DynamicallyInvokable]
		public void ReportProgress(int percentProgress)
		{
			this.ReportProgress(percentProgress, null);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x000DF368 File Offset: 0x000DD568
		[global::__DynamicallyInvokable]
		public void ReportProgress(int percentProgress, object userState)
		{
			if (!this.WorkerReportsProgress)
			{
				throw new InvalidOperationException(SR.GetString("BackgroundWorker_WorkerDoesntReportProgress"));
			}
			ProgressChangedEventArgs progressChangedEventArgs = new ProgressChangedEventArgs(percentProgress, userState);
			if (this.asyncOperation != null)
			{
				this.asyncOperation.Post(this.progressReporter, progressChangedEventArgs);
				return;
			}
			this.progressReporter(progressChangedEventArgs);
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000DF3BC File Offset: 0x000DD5BC
		[global::__DynamicallyInvokable]
		public void RunWorkerAsync()
		{
			this.RunWorkerAsync(null);
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x000DF3C8 File Offset: 0x000DD5C8
		[global::__DynamicallyInvokable]
		public void RunWorkerAsync(object argument)
		{
			if (this.isRunning)
			{
				throw new InvalidOperationException(SR.GetString("BackgroundWorker_WorkerAlreadyRunning"));
			}
			this.isRunning = true;
			this.cancellationPending = false;
			this.asyncOperation = AsyncOperationManager.CreateOperation(null);
			this.threadStart.BeginInvoke(argument, null, null);
		}

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x0600315E RID: 12638 RVA: 0x000DF416 File Offset: 0x000DD616
		// (remove) Token: 0x0600315F RID: 12639 RVA: 0x000DF429 File Offset: 0x000DD629
		[SRCategory("PropertyCategoryAsynchronous")]
		[SRDescription("BackgroundWorker_RunWorkerCompleted")]
		[global::__DynamicallyInvokable]
		public event RunWorkerCompletedEventHandler RunWorkerCompleted
		{
			[global::__DynamicallyInvokable]
			add
			{
				base.Events.AddHandler(BackgroundWorker.runWorkerCompletedKey, value);
			}
			[global::__DynamicallyInvokable]
			remove
			{
				base.Events.RemoveHandler(BackgroundWorker.runWorkerCompletedKey, value);
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06003160 RID: 12640 RVA: 0x000DF43C File Offset: 0x000DD63C
		// (set) Token: 0x06003161 RID: 12641 RVA: 0x000DF444 File Offset: 0x000DD644
		[SRCategory("PropertyCategoryAsynchronous")]
		[SRDescription("BackgroundWorker_WorkerReportsProgress")]
		[DefaultValue(false)]
		[global::__DynamicallyInvokable]
		public bool WorkerReportsProgress
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.workerReportsProgress;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.workerReportsProgress = value;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x000DF44D File Offset: 0x000DD64D
		// (set) Token: 0x06003163 RID: 12643 RVA: 0x000DF455 File Offset: 0x000DD655
		[SRCategory("PropertyCategoryAsynchronous")]
		[SRDescription("BackgroundWorker_WorkerSupportsCancellation")]
		[DefaultValue(false)]
		[global::__DynamicallyInvokable]
		public bool WorkerSupportsCancellation
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.canCancelWorker;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.canCancelWorker = value;
			}
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000DF460 File Offset: 0x000DD660
		private void WorkerThreadStart(object argument)
		{
			object obj = null;
			Exception ex = null;
			bool flag = false;
			try
			{
				DoWorkEventArgs doWorkEventArgs = new DoWorkEventArgs(argument);
				this.OnDoWork(doWorkEventArgs);
				if (doWorkEventArgs.Cancel)
				{
					flag = true;
				}
				else
				{
					obj = doWorkEventArgs.Result;
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			RunWorkerCompletedEventArgs runWorkerCompletedEventArgs = new RunWorkerCompletedEventArgs(obj, ex, flag);
			this.asyncOperation.PostOperationCompleted(this.operationCompleted, runWorkerCompletedEventArgs);
		}

		// Token: 0x0400290D RID: 10509
		private static readonly object doWorkKey = new object();

		// Token: 0x0400290E RID: 10510
		private static readonly object runWorkerCompletedKey = new object();

		// Token: 0x0400290F RID: 10511
		private static readonly object progressChangedKey = new object();

		// Token: 0x04002910 RID: 10512
		private bool canCancelWorker;

		// Token: 0x04002911 RID: 10513
		private bool workerReportsProgress;

		// Token: 0x04002912 RID: 10514
		private bool cancellationPending;

		// Token: 0x04002913 RID: 10515
		private bool isRunning;

		// Token: 0x04002914 RID: 10516
		private AsyncOperation asyncOperation;

		// Token: 0x04002915 RID: 10517
		private readonly BackgroundWorker.WorkerThreadStartDelegate threadStart;

		// Token: 0x04002916 RID: 10518
		private readonly SendOrPostCallback operationCompleted;

		// Token: 0x04002917 RID: 10519
		private readonly SendOrPostCallback progressReporter;

		// Token: 0x02000890 RID: 2192
		// (Invoke) Token: 0x06004583 RID: 17795
		private delegate void WorkerThreadStartDelegate(object argument);
	}
}
