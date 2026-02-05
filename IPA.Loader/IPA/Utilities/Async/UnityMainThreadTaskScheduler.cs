using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IPA.Utilities.Async
{
	/// <summary>
	/// A task scheduler that runs tasks on the Unity main thread via coroutines.
	/// </summary>
	// Token: 0x02000024 RID: 36
	public class UnityMainThreadTaskScheduler : TaskScheduler, IDisposable
	{
		/// <summary>
		/// Gets the default main thread scheduler that is managed by BSIPA.
		/// </summary>
		/// <value>a scheduler that is managed by BSIPA</value>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000042F9 File Offset: 0x000024F9
		public new static TaskScheduler Default { get; } = new UnityMainThreadTaskScheduler();

		/// <summary>
		/// Gets a factory for creating tasks on <see cref="P:IPA.Utilities.Async.UnityMainThreadTaskScheduler.Default" />.
		/// </summary>
		/// <value>a factory for creating tasks on the default scheduler</value>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004300 File Offset: 0x00002500
		public static TaskFactory Factory { get; } = new TaskFactory(UnityMainThreadTaskScheduler.Default);

		/// <summary>
		/// Gets whether or not this scheduler is currently executing tasks.
		/// </summary>
		/// <value><see langword="true" /> if the scheduler is running, <see langword="false" /> otherwise</value>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004307 File Offset: 0x00002507
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000430F File Offset: 0x0000250F
		public bool IsRunning { get; private set; }

		/// <summary>
		/// Gets whether or not this scheduler is in the process of shutting down.
		/// </summary>
		/// <value><see langword="true" /> if the scheduler is shutting down, <see langword="false" /> otherwise</value>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004318 File Offset: 0x00002518
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004320 File Offset: 0x00002520
		public bool Cancelling { get; private set; }

		/// <summary>
		/// Gets or sets the number of tasks to execute before yielding back to Unity.
		/// </summary>
		/// <value>the number of tasks to execute per resume</value>
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004329 File Offset: 0x00002529
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004331 File Offset: 0x00002531
		public int YieldAfterTasks
		{
			get
			{
				return this.yieldAfterTasks;
			}
			set
			{
				this.ThrowIfDisposed();
				if (value < 1)
				{
					throw new ArgumentException("Value cannot be less than 1", "value");
				}
				this.yieldAfterTasks = value;
			}
		}

		/// <summary>
		/// Gets or sets the amount of time to execute tasks for before yielding back to Unity. Default is 0.5ms.
		/// </summary>
		/// <value>the amount of time to execute tasks for before yielding back to Unity</value>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00004354 File Offset: 0x00002554
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x0000435C File Offset: 0x0000255C
		public TimeSpan YieldAfterTime
		{
			get
			{
				return this.yieldAfterTime;
			}
			set
			{
				this.ThrowIfDisposed();
				if (value <= TimeSpan.Zero)
				{
					throw new ArgumentException("Value must be greater than zero", "value");
				}
				this.yieldAfterTime = value;
			}
		}

		/// <summary>
		/// When used as a Unity coroutine, runs the scheduler. Otherwise, this is an invalid call.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Do not ever call <see cref="M:UnityEngine.MonoBehaviour.StopCoroutine(System.Collections.IEnumerator)" /> on this
		/// coroutine, nor <see cref="M:UnityEngine.MonoBehaviour.StopAllCoroutines" /> on the behaviour hosting
		/// this coroutine. This has no way to detect this, and this object will become invalid.
		/// </para>
		/// <para>
		/// If you need to stop this coroutine, first call <see cref="M:IPA.Utilities.Async.UnityMainThreadTaskScheduler.Cancel" />, then wait for it to
		/// exit on its own.
		/// </para>
		/// </remarks>
		/// <returns>a Unity coroutine</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this scheduler is disposed</exception>
		/// <exception cref="T:System.InvalidOperationException">if the scheduler is already running</exception>
		// Token: 0x060000C7 RID: 199 RVA: 0x00004388 File Offset: 0x00002588
		public IEnumerator Coroutine()
		{
			this.ThrowIfDisposed();
			if (this.IsRunning)
			{
				throw new InvalidOperationException("Scheduler already running");
			}
			this.Cancelling = false;
			this.IsRunning = true;
			yield return null;
			Stopwatch sw = new Stopwatch();
			try
			{
				while (!this.Cancelling)
				{
					if (!this.tasks.IsEmpty)
					{
						int yieldAfter = this.YieldAfterTasks;
						sw.Start();
						int i = 0;
						IL_00D9:
						while (i < yieldAfter && !this.tasks.IsEmpty && sw.Elapsed < this.YieldAfterTime)
						{
							UnityMainThreadTaskScheduler.QueueItem task;
							while (this.tasks.TryDequeue(out task))
							{
								if (task.HasTask)
								{
									base.TryExecuteTask(task.Task);
									i++;
									goto IL_00D9;
								}
							}
							break;
						}
						sw.Reset();
					}
					yield return null;
				}
			}
			finally
			{
				sw.Reset();
				this.IsRunning = false;
			}
			yield break;
			yield break;
		}

		/// <summary>
		/// Cancels the scheduler. If the scheduler is currently executing tasks, that batch will finish first.
		/// All remaining tasks will be left in the queue.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">if this scheduler is disposed</exception>
		/// <exception cref="T:System.InvalidOperationException">if the scheduler is not running</exception>
		// Token: 0x060000C8 RID: 200 RVA: 0x00004397 File Offset: 0x00002597
		public void Cancel()
		{
			this.ThrowIfDisposed();
			if (!this.IsRunning)
			{
				throw new InvalidOperationException("The scheduler is not running");
			}
			this.Cancelling = true;
		}

		/// <summary>
		/// Throws a <see cref="T:System.NotSupportedException" />.
		/// </summary>
		/// <returns>nothing</returns>
		/// <exception cref="T:System.NotSupportedException">Always.</exception>
		// Token: 0x060000C9 RID: 201 RVA: 0x000043BC File Offset: 0x000025BC
		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return (from q in this.tasks.ToArray()
				where q.HasTask
				select q.Task).ToArray<Task>();
		}

		/// <summary>
		/// Queues a given <see cref="T:System.Threading.Tasks.Task" /> to this scheduler. The <see cref="T:System.Threading.Tasks.Task" /> <i>must</i> be
		/// scheduled for this <see cref="T:System.Threading.Tasks.TaskScheduler" /> by the runtime.
		/// </summary>
		/// <param name="task">the <see cref="T:System.Threading.Tasks.Task" /> to queue</param>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x060000CA RID: 202 RVA: 0x00004424 File Offset: 0x00002624
		protected override void QueueTask(Task task)
		{
			this.ThrowIfDisposed();
			UnityMainThreadTaskScheduler.QueueItem item = new UnityMainThreadTaskScheduler.QueueItem(task);
			UnityMainThreadTaskScheduler.itemTable.Add(task, item);
			this.tasks.Enqueue(item);
		}

		/// <summary>
		/// Runs the task inline if the current thread is the Unity main thread.
		/// </summary>
		/// <param name="task">the task to attempt to execute</param>
		/// <param name="taskWasPreviouslyQueued">whether the task was previously queued to this scheduler</param>
		/// <returns><see langword="false" /> if the task could not be run, <see langword="true" /> if it was</returns>
		/// <exception cref="T:System.ObjectDisposedException">Thrown if this object has already been disposed.</exception>
		// Token: 0x060000CB RID: 203 RVA: 0x00004458 File Offset: 0x00002658
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			this.ThrowIfDisposed();
			if (!UnityGame.OnMainThread)
			{
				return false;
			}
			if (taskWasPreviouslyQueued)
			{
				UnityMainThreadTaskScheduler.QueueItem item;
				if (!UnityMainThreadTaskScheduler.itemTable.TryGetValue(task, out item))
				{
					return false;
				}
				if (!item.HasTask)
				{
					return false;
				}
				item.HasTask = false;
			}
			return base.TryExecuteTask(task);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000044A2 File Offset: 0x000026A2
		private void ThrowIfDisposed()
		{
			if (this.disposedValue)
			{
				throw new ObjectDisposedException("SingleThreadTaskScheduler");
			}
		}

		/// <summary>
		/// Disposes this object.
		/// </summary>
		/// <param name="disposing">whether or not to dispose managed objects</param>
		// Token: 0x060000CD RID: 205 RVA: 0x000044B7 File Offset: 0x000026B7
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				this.disposedValue = true;
			}
		}

		/// <summary>
		/// Disposes this object. This puts the object into an unusable state.
		/// </summary>
		// Token: 0x060000CE RID: 206 RVA: 0x000044CA File Offset: 0x000026CA
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x04000035 RID: 53
		private readonly ConcurrentQueue<UnityMainThreadTaskScheduler.QueueItem> tasks = new ConcurrentQueue<UnityMainThreadTaskScheduler.QueueItem>();

		// Token: 0x04000036 RID: 54
		private static readonly ConditionalWeakTable<Task, UnityMainThreadTaskScheduler.QueueItem> itemTable = new ConditionalWeakTable<Task, UnityMainThreadTaskScheduler.QueueItem>();

		// Token: 0x04000039 RID: 57
		private int yieldAfterTasks = 64;

		// Token: 0x0400003A RID: 58
		private TimeSpan yieldAfterTime = TimeSpan.FromMilliseconds(0.5);

		// Token: 0x0400003B RID: 59
		private bool disposedValue;

		// Token: 0x020000D6 RID: 214
		private class QueueItem : IEquatable<Task>, IEquatable<UnityMainThreadTaskScheduler.QueueItem>
		{
			// Token: 0x170000CB RID: 203
			// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0001605F File Offset: 0x0001425F
			// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00016067 File Offset: 0x00014267
			public bool HasTask
			{
				get
				{
					return this.hasTask;
				}
				set
				{
					this.hasTask = value;
					if (!this.hasTask)
					{
						this.Task = null;
					}
				}
			}

			// Token: 0x170000CC RID: 204
			// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0001607F File Offset: 0x0001427F
			// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00016087 File Offset: 0x00014287
			public Task Task { get; private set; }

			// Token: 0x060004C9 RID: 1225 RVA: 0x00016090 File Offset: 0x00014290
			public QueueItem(Task task)
			{
				this.HasTask = true;
				this.Task = task;
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x000160A6 File Offset: 0x000142A6
			public bool Equals(Task other)
			{
				return this.HasTask && other.Equals(this.Task);
			}

			// Token: 0x060004CB RID: 1227 RVA: 0x000160BE File Offset: 0x000142BE
			public bool Equals(UnityMainThreadTaskScheduler.QueueItem other)
			{
				return other.HasTask == this.HasTask && this.Equals(other.Task);
			}

			// Token: 0x040002D9 RID: 729
			private bool hasTask;
		}
	}
}
