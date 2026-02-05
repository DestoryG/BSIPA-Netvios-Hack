using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPA.Loader;

namespace IPA.Utilities.Async
{
	/// <summary>
	/// A class providing coroutine helpers.
	/// </summary>
	// Token: 0x02000021 RID: 33
	public static class Coroutines
	{
		/// <summary>
		/// Stalls the coroutine until <paramref name="task" /> completes, faults, or is canceled.
		/// </summary>
		/// <param name="task">the <see cref="T:System.Threading.Tasks.Task" /> to wait for</param>
		/// <returns>a coroutine waiting for the given task</returns>
		// Token: 0x060000A0 RID: 160 RVA: 0x00003EEC File Offset: 0x000020EC
		public static IEnumerator WaitForTask(Task task)
		{
			return Coroutines.WaitForTask(task, false);
		}

		/// <summary>
		/// Stalls the coroutine until <paramref name="task" /> completes, faults, or is canceled.
		/// </summary>
		/// <param name="task">the <see cref="T:System.Threading.Tasks.Task" /> to wait for</param>
		/// <param name="throwOnFault">whether or not to throw if the task faulted</param>
		/// <returns>a coroutine waiting for the given task</returns>
		// Token: 0x060000A1 RID: 161 RVA: 0x00003EF5 File Offset: 0x000020F5
		public static IEnumerator WaitForTask(Task task, bool throwOnFault = false)
		{
			while (!task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
			{
				yield return null;
			}
			if (throwOnFault && task.IsFaulted)
			{
				throw task.Exception;
			}
			yield break;
		}

		/// <summary>
		/// Binds a <see cref="T:System.Threading.Tasks.Task" /> to a Unity coroutine, capturing exceptions as well as the coroutine call stack.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This may be called off of the Unity main thread. If it is, the coroutine start will be scheduled using the default
		/// <see cref="T:IPA.Utilities.Async.UnityMainThreadTaskScheduler" /> and will be run on the main thread as required by Unity.
		/// </para>
		/// <para>
		/// Unity provides a handful of coroutine helpers that are not <see cref="T:System.Collections.IEnumerable" />s. Most of these are not terribly
		/// helpful on their own, however <see cref="T:UnityEngine.WaitForSeconds" /> may be. Instead, prefer to use the typical .NET
		/// <see cref="M:System.Threading.Tasks.Task.Wait(System.TimeSpan)" /> or similar overloads, or use <see cref="T:UnityEngine.WaitForSecondsRealtime" />.
		/// </para>
		/// </remarks>
		/// <param name="coroutine">the coroutine to bind to a task</param>
		/// <returns>a <see cref="T:System.Threading.Tasks.Task" /> that completes when <paramref name="coroutine" /> completes, and fails when it throws</returns>
		// Token: 0x060000A2 RID: 162 RVA: 0x00003F0C File Offset: 0x0000210C
		public static Task AsTask(IEnumerator coroutine)
		{
			if (!UnityGame.OnMainThread)
			{
				return UnityMainThreadTaskScheduler.Factory.StartNew<Task>(() => Coroutines.AsTask(coroutine)).Unwrap();
			}
			TaskCompletionSource<Coroutines.VoidStruct> tcs = new TaskCompletionSource<Coroutines.VoidStruct>(coroutine, TaskCreationOptions.RunContinuationsAsynchronously);
			PluginComponent.Instance.StartCoroutine(new Coroutines.AsTaskCoroutineExecutor(coroutine, tcs));
			return tcs.Task;
		}

		// Token: 0x020000D0 RID: 208
		private struct VoidStruct
		{
		}

		// Token: 0x020000D1 RID: 209
		private class ExceptionLocation : Exception
		{
			// Token: 0x060004B5 RID: 1205 RVA: 0x00015E1F File Offset: 0x0001401F
			public ExceptionLocation(IEnumerable<string> locations)
				: base(string.Join("\n", locations.Select((string s) => "in " + s)))
			{
			}
		}

		// Token: 0x020000D2 RID: 210
		private class AsTaskCoroutineExecutor : IEnumerator
		{
			// Token: 0x060004B6 RID: 1206 RVA: 0x00015E56 File Offset: 0x00014056
			public AsTaskCoroutineExecutor(IEnumerator coroutine, TaskCompletionSource<Coroutines.VoidStruct> completion)
			{
				this.completionSource = completion;
				this.enumerators.Push(coroutine);
			}

			// Token: 0x170000C8 RID: 200
			// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00015E7D File Offset: 0x0001407D
			public object Current
			{
				get
				{
					IEnumerator enumerator = this.enumerators.FirstOrDefault<IEnumerator>();
					if (enumerator == null)
					{
						return null;
					}
					return enumerator.Current;
				}
			}

			// Token: 0x060004B8 RID: 1208 RVA: 0x00015E98 File Offset: 0x00014098
			public bool MoveNext()
			{
				while (this.enumerators.Count != 0)
				{
					bool flag;
					try
					{
						IEnumerator top = this.enumerators.Peek();
						if (!top.MoveNext())
						{
							this.enumerators.Pop();
							continue;
						}
						IEnumerator enumerator = top.Current as IEnumerator;
						if (enumerator != null)
						{
							this.enumerators.Push(enumerator);
							continue;
						}
						flag = true;
					}
					catch (Exception e)
					{
						TaskCompletionSource<Coroutines.VoidStruct> taskCompletionSource = this.completionSource;
						Exception[] array = new Exception[2];
						Exception e2;
						array[0] = e2;
						array[1] = new Coroutines.ExceptionLocation(this.enumerators.Select((IEnumerator e) => e.GetType().ToString()));
						taskCompletionSource.SetException(new AggregateException(array));
						flag = false;
					}
					return flag;
				}
				this.completionSource.SetResult(default(Coroutines.VoidStruct));
				return false;
			}

			// Token: 0x060004B9 RID: 1209 RVA: 0x00015F70 File Offset: 0x00014170
			public void Reset()
			{
				throw new InvalidOperationException();
			}

			// Token: 0x040002D0 RID: 720
			private readonly TaskCompletionSource<Coroutines.VoidStruct> completionSource;

			// Token: 0x040002D1 RID: 721
			private readonly Stack<IEnumerator> enumerators = new Stack<IEnumerator>(2);
		}
	}
}
