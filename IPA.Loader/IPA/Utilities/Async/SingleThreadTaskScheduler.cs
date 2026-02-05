using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IPA.Utilities.Async
{
	/// <summary>
	/// A single-threaded task scheduler that runs all of its tasks on the same thread.
	/// </summary>
	// Token: 0x02000023 RID: 35
	public class SingleThreadTaskScheduler : TaskScheduler, IDisposable
	{
		/// <summary>
		/// Gets whether or not the underlying thread has been started.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000416E File Offset: 0x0000236E
		public bool IsRunning
		{
			get
			{
				this.ThrowIfDisposed();
				return this.runThread.IsAlive;
			}
		}

		/// <summary>
		/// Starts the thread that executes tasks scheduled with this <see cref="T:System.Threading.Tasks.TaskScheduler" />
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x060000B1 RID: 177 RVA: 0x00004181 File Offset: 0x00002381
		public void Start()
		{
			this.ThrowIfDisposed();
			this.runThread.Start(this);
		}

		/// <summary>
		/// Terminates the runner thread, and waits for the currently running task to complete.
		/// </summary>
		/// <remarks>
		/// After this method returns, this object has been disposed and is no longer in a valid state.
		/// </remarks>
		/// <returns>an <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Threading.Tasks.Task" />s that did not execute</returns>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x060000B2 RID: 178 RVA: 0x00004195 File Offset: 0x00002395
		public IEnumerable<Task> Exit()
		{
			this.ThrowIfDisposed();
			this.tasks.CompleteAdding();
			this.exitTokenSource.Cancel();
			this.runThread.Join();
			List<Task> list = new List<Task>();
			list.AddRange(this.tasks);
			return list;
		}

		/// <summary>
		/// Waits for the runner thread to complete all tasks in the queue, then exits.
		/// </summary>
		/// <remarks>
		/// After this method returns, this object has been disposed and is no longer in a valid state.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x060000B3 RID: 179 RVA: 0x000041CF File Offset: 0x000023CF
		public void Join()
		{
			this.ThrowIfDisposed();
			this.tasks.CompleteAdding();
			this.runThread.Join();
		}

		/// <summary>
		/// Throws a <see cref="T:System.NotSupportedException" />.
		/// </summary>
		/// <returns>nothing</returns>
		/// <exception cref="T:System.NotSupportedException">Always.</exception>
		// Token: 0x060000B4 RID: 180 RVA: 0x000041ED File Offset: 0x000023ED
		protected override IEnumerable<Task> GetScheduledTasks()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Queues a given <see cref="T:System.Threading.Tasks.Task" /> to this scheduler. The <see cref="T:System.Threading.Tasks.Task" /> <i>must&gt;</i> be
		/// scheduled for this <see cref="T:System.Threading.Tasks.TaskScheduler" /> by the runtime.
		/// </summary>
		/// <param name="task">the <see cref="T:System.Threading.Tasks.Task" /> to queue</param>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x060000B5 RID: 181 RVA: 0x000041F4 File Offset: 0x000023F4
		protected override void QueueTask(Task task)
		{
			this.ThrowIfDisposed();
			this.tasks.Add(task);
		}

		/// <summary>
		/// Rejects any attempts to execute a task inline.
		/// </summary>
		/// <remarks>
		/// This task scheduler <i>always</i> runs its tasks on the thread that it manages, therefore it doesn't
		/// make sense to run it inline.
		/// </remarks>
		/// <param name="task">the task to attempt to execute</param>
		/// <param name="taskWasPreviouslyQueued">whether the task was previously queued to this scheduler</param>
		/// <returns><see langword="false" /></returns>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x060000B6 RID: 182 RVA: 0x00004208 File Offset: 0x00002408
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			this.ThrowIfDisposed();
			return false;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004211 File Offset: 0x00002411
		private void ThrowIfDisposed()
		{
			if (this.disposedValue)
			{
				throw new ObjectDisposedException("SingleThreadTaskScheduler");
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004228 File Offset: 0x00002428
		private void ExecuteTasks()
		{
			this.ThrowIfDisposed();
			CancellationToken token = this.exitTokenSource.Token;
			try
			{
				Task task;
				while (!this.tasks.IsCompleted && this.tasks.TryTake(out task, -1, token))
				{
					base.TryExecuteTask(task);
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004284 File Offset: 0x00002484
		private static void ExecuteTasksS(object param)
		{
			(param as SingleThreadTaskScheduler).ExecuteTasks();
		}

		/// <summary>
		/// Disposes this object.
		/// </summary>
		/// <param name="disposing">whether or not to dispose managed objects</param>
		// Token: 0x060000BA RID: 186 RVA: 0x00004291 File Offset: 0x00002491
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.exitTokenSource.Dispose();
					this.tasks.Dispose();
				}
				this.disposedValue = true;
			}
		}

		/// <summary>
		/// Disposes this object. This puts the object into an unusable state.
		/// </summary>
		// Token: 0x060000BB RID: 187 RVA: 0x000042BB File Offset: 0x000024BB
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0400002F RID: 47
		private readonly Thread runThread = new Thread(new ParameterizedThreadStart(SingleThreadTaskScheduler.ExecuteTasksS));

		// Token: 0x04000030 RID: 48
		private readonly BlockingCollection<Task> tasks = new BlockingCollection<Task>();

		// Token: 0x04000031 RID: 49
		private readonly CancellationTokenSource exitTokenSource = new CancellationTokenSource();

		// Token: 0x04000032 RID: 50
		private bool disposedValue;
	}
}
