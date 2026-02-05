using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace IPA.Utilities.Async
{
	/// <summary>
	/// Utilities for inter-thread synchronization. All Locker method acquire their object immediately,
	/// and should only be used with <see langword="using" /> to automatically release them.
	/// </summary>
	/// <example>
	/// <para>
	/// The canonical usage of *all* of the member functions is as follows, substituting <see cref="M:IPA.Utilities.Async.Synchronization.Lock(System.Threading.Mutex)" />
	/// with whichever member you want to use, according to your lock type.
	/// </para>
	/// <code>
	/// using var _locker = Synchronization.Lock(mutex);
	/// </code>
	/// </example>
	// Token: 0x02000025 RID: 37
	public static class Synchronization
	{
		/// <summary>
		/// Creates a locker for a mutex.
		/// </summary>
		/// <param name="mut">the mutex to acquire</param>
		/// <returns>the locker to use with <see langword="using" /></returns>
		// Token: 0x060000D1 RID: 209 RVA: 0x00004527 File Offset: 0x00002727
		public static Synchronization.MutexLocker Lock(Mutex mut)
		{
			return new Synchronization.MutexLocker(mut);
		}

		/// <summary>
		/// Creates a locker for a semaphore.
		/// </summary>
		/// <param name="sem">the semaphore to acquire</param>
		/// <returns>the locker to use with <see langword="using" /></returns>
		// Token: 0x060000D2 RID: 210 RVA: 0x0000452F File Offset: 0x0000272F
		public static Synchronization.SemaphoreLocker Lock(Semaphore sem)
		{
			return new Synchronization.SemaphoreLocker(sem);
		}

		/// <summary>
		/// Creates a locker for a slim semaphore.
		/// </summary>
		/// <param name="sem">the slim semaphore to acquire</param>
		/// <returns>the locker to use with <see langword="using" /></returns>
		// Token: 0x060000D3 RID: 211 RVA: 0x00004537 File Offset: 0x00002737
		public static Synchronization.SemaphoreSlimLocker Lock(SemaphoreSlim sem)
		{
			return new Synchronization.SemaphoreSlimLocker(sem);
		}

		/// <summary>
		/// Creates a locker for a slim semaphore asynchronously.
		/// </summary>
		/// <param name="sem">the slim semaphore to acquire async</param>
		/// <returns>the locker to use with <see langword="using" /></returns>
		// Token: 0x060000D4 RID: 212 RVA: 0x00004540 File Offset: 0x00002740
		public static Task<Synchronization.SemaphoreSlimAsyncLocker> LockAsync(SemaphoreSlim sem)
		{
			Synchronization.<LockAsync>d__10 <LockAsync>d__;
			<LockAsync>d__.sem = sem;
			<LockAsync>d__.<>t__builder = AsyncTaskMethodBuilder<Synchronization.SemaphoreSlimAsyncLocker>.Create();
			<LockAsync>d__.<>1__state = -1;
			<LockAsync>d__.<>t__builder.Start<Synchronization.<LockAsync>d__10>(ref <LockAsync>d__);
			return <LockAsync>d__.<>t__builder.Task;
		}

		/// <summary>
		/// Creates a locker for a write lock <see cref="T:System.Threading.ReaderWriterLockSlim" />.
		/// </summary>
		/// <param name="rwl">the lock to acquire in write mode</param>
		/// <returns>the locker to use with <see langword="using" /></returns>
		// Token: 0x060000D5 RID: 213 RVA: 0x00004583 File Offset: 0x00002783
		public static Synchronization.ReaderWriterLockSlimWriteLocker LockWrite(ReaderWriterLockSlim rwl)
		{
			return new Synchronization.ReaderWriterLockSlimWriteLocker(rwl);
		}

		/// <summary>
		/// Creates a locker for a read lock on a <see cref="T:System.Threading.ReaderWriterLockSlim" />.
		/// </summary>
		/// <param name="rwl">the lock to acquire in read mode</param>
		/// <returns>the locker to use with <see langword="using" /></returns>
		// Token: 0x060000D6 RID: 214 RVA: 0x0000458B File Offset: 0x0000278B
		public static Synchronization.ReaderWriterLockSlimReadLocker LockRead(ReaderWriterLockSlim rwl)
		{
			return new Synchronization.ReaderWriterLockSlimReadLocker(rwl);
		}

		/// <summary>
		/// Creates a locker for an upgradable read lock on a <see cref="T:System.Threading.ReaderWriterLockSlim" />.
		/// </summary>
		/// <param name="rwl">the lock to acquire in upgradable read mode</param>
		/// <returns>the locker to use with <see langword="using" /></returns>
		// Token: 0x060000D7 RID: 215 RVA: 0x00004593 File Offset: 0x00002793
		public static Synchronization.ReaderWriterLockSlimUpgradableReadLocker LockReadUpgradable(ReaderWriterLockSlim rwl)
		{
			return new Synchronization.ReaderWriterLockSlimUpgradableReadLocker(rwl);
		}

		/// <summary>
		/// A locker for a <see cref="T:System.Threading.Mutex" /> that automatically releases when it is disposed.
		/// Create this with <see cref="M:IPA.Utilities.Async.Synchronization.Lock(System.Threading.Mutex)" />.
		/// </summary>
		/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
		/// <seealso cref="M:IPA.Utilities.Async.Synchronization.Lock(System.Threading.Mutex)" />
		// Token: 0x020000D9 RID: 217
		public struct MutexLocker : IDisposable
		{
			// Token: 0x060004D7 RID: 1239 RVA: 0x000162EF File Offset: 0x000144EF
			internal MutexLocker(Mutex mutex)
			{
				this.mutex = mutex;
				mutex.WaitOne();
			}

			// Token: 0x060004D8 RID: 1240 RVA: 0x000162FF File Offset: 0x000144FF
			void IDisposable.Dispose()
			{
				this.mutex.ReleaseMutex();
			}

			// Token: 0x040002E2 RID: 738
			private readonly Mutex mutex;
		}

		/// <summary>
		/// A locker for a <see cref="T:System.Threading.Semaphore" /> that automatically releases when it is disposed.
		/// Create this with <see cref="M:IPA.Utilities.Async.Synchronization.Lock(System.Threading.Semaphore)" />.
		/// </summary>
		/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
		/// <seealso cref="M:IPA.Utilities.Async.Synchronization.Lock(System.Threading.Semaphore)" />
		// Token: 0x020000DA RID: 218
		public struct SemaphoreLocker : IDisposable
		{
			// Token: 0x060004D9 RID: 1241 RVA: 0x0001630C File Offset: 0x0001450C
			internal SemaphoreLocker(Semaphore sem)
			{
				this.sem = sem;
				sem.WaitOne();
			}

			// Token: 0x060004DA RID: 1242 RVA: 0x0001631C File Offset: 0x0001451C
			void IDisposable.Dispose()
			{
				this.sem.Release();
			}

			// Token: 0x040002E3 RID: 739
			private readonly Semaphore sem;
		}

		/// <summary>
		/// A locker for a <see cref="T:System.Threading.SemaphoreSlim" /> that automatically releases when it is disposed.
		/// Create this with <see cref="M:IPA.Utilities.Async.Synchronization.Lock(System.Threading.SemaphoreSlim)" />.
		/// </summary>
		/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
		/// <seealso cref="M:IPA.Utilities.Async.Synchronization.Lock(System.Threading.SemaphoreSlim)" />
		// Token: 0x020000DB RID: 219
		public struct SemaphoreSlimLocker : IDisposable
		{
			// Token: 0x060004DB RID: 1243 RVA: 0x0001632A File Offset: 0x0001452A
			internal SemaphoreSlimLocker(SemaphoreSlim sem)
			{
				this.sem = sem;
				sem.Wait();
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x00016339 File Offset: 0x00014539
			void IDisposable.Dispose()
			{
				this.sem.Release();
			}

			// Token: 0x040002E4 RID: 740
			private readonly SemaphoreSlim sem;
		}

		/// <summary>
		/// A locker for a <see cref="T:System.Threading.SemaphoreSlim" /> that was created asynchronously and automatically releases
		/// when it is disposed. Create this with <see cref="M:IPA.Utilities.Async.Synchronization.LockAsync(System.Threading.SemaphoreSlim)" />.
		/// </summary>
		/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
		/// <seealso cref="M:IPA.Utilities.Async.Synchronization.LockAsync(System.Threading.SemaphoreSlim)" />
		// Token: 0x020000DC RID: 220
		public struct SemaphoreSlimAsyncLocker : IDisposable
		{
			// Token: 0x060004DD RID: 1245 RVA: 0x00016347 File Offset: 0x00014547
			internal SemaphoreSlimAsyncLocker(SemaphoreSlim sem)
			{
				this.sem = sem;
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x00016350 File Offset: 0x00014550
			internal Task Lock()
			{
				return this.sem.WaitAsync();
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x0001635D File Offset: 0x0001455D
			void IDisposable.Dispose()
			{
				this.sem.Release();
			}

			// Token: 0x040002E5 RID: 741
			private readonly SemaphoreSlim sem;
		}

		/// <summary>
		/// A locker for a write lock on a <see cref="T:System.Threading.ReaderWriterLockSlim" /> that automatically releases when
		/// it is disposed. Create this with <see cref="M:IPA.Utilities.Async.Synchronization.LockWrite(System.Threading.ReaderWriterLockSlim)" />.
		/// </summary>
		/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
		/// <seealso cref="M:IPA.Utilities.Async.Synchronization.LockWrite(System.Threading.ReaderWriterLockSlim)" />
		// Token: 0x020000DD RID: 221
		public struct ReaderWriterLockSlimWriteLocker : IDisposable
		{
			// Token: 0x060004E0 RID: 1248 RVA: 0x0001636B File Offset: 0x0001456B
			internal ReaderWriterLockSlimWriteLocker(ReaderWriterLockSlim lck)
			{
				this.rwl = lck;
				this.rwl.EnterWriteLock();
			}

			// Token: 0x060004E1 RID: 1249 RVA: 0x0001637F File Offset: 0x0001457F
			void IDisposable.Dispose()
			{
				this.rwl.ExitWriteLock();
			}

			// Token: 0x040002E6 RID: 742
			private readonly ReaderWriterLockSlim rwl;
		}

		/// <summary>
		/// A locker for a read lock on a <see cref="T:System.Threading.ReaderWriterLockSlim" /> that automatically releases when
		/// it is disposed. Create this with <see cref="M:IPA.Utilities.Async.Synchronization.LockRead(System.Threading.ReaderWriterLockSlim)" />.
		/// </summary>
		/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
		/// <seealso cref="M:IPA.Utilities.Async.Synchronization.LockRead(System.Threading.ReaderWriterLockSlim)" />
		// Token: 0x020000DE RID: 222
		public struct ReaderWriterLockSlimReadLocker : IDisposable
		{
			// Token: 0x060004E2 RID: 1250 RVA: 0x0001638C File Offset: 0x0001458C
			internal ReaderWriterLockSlimReadLocker(ReaderWriterLockSlim lck)
			{
				this.rwl = lck;
				this.rwl.EnterReadLock();
			}

			// Token: 0x060004E3 RID: 1251 RVA: 0x000163A0 File Offset: 0x000145A0
			void IDisposable.Dispose()
			{
				this.rwl.ExitReadLock();
			}

			// Token: 0x040002E7 RID: 743
			private readonly ReaderWriterLockSlim rwl;
		}

		/// <summary>
		/// A locker for an upgradable read lock on a <see cref="T:System.Threading.ReaderWriterLockSlim" /> that automatically releases
		/// when it is disposed. Create this with <see cref="M:IPA.Utilities.Async.Synchronization.LockReadUpgradable(System.Threading.ReaderWriterLockSlim)" />.
		/// </summary>
		/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
		/// <seealso cref="M:IPA.Utilities.Async.Synchronization.LockReadUpgradable(System.Threading.ReaderWriterLockSlim)" />
		// Token: 0x020000DF RID: 223
		public struct ReaderWriterLockSlimUpgradableReadLocker : IDisposable
		{
			// Token: 0x060004E4 RID: 1252 RVA: 0x000163AD File Offset: 0x000145AD
			internal ReaderWriterLockSlimUpgradableReadLocker(ReaderWriterLockSlim lck)
			{
				this.rwl = lck;
				this.rwl.EnterUpgradeableReadLock();
			}

			/// <summary>
			/// Creates a locker for a write lock on the <see cref="T:System.Threading.ReaderWriterLockSlim" /> associated with this locker,
			/// upgrading the current thread's lock.
			/// </summary>
			/// <returns>a locker for the new write lock</returns>
			/// <seealso cref="T:IPA.Utilities.Async.Synchronization" />
			// Token: 0x060004E5 RID: 1253 RVA: 0x000163C1 File Offset: 0x000145C1
			public Synchronization.ReaderWriterLockSlimWriteLocker Upgrade()
			{
				return new Synchronization.ReaderWriterLockSlimWriteLocker(this.rwl);
			}

			// Token: 0x060004E6 RID: 1254 RVA: 0x000163CE File Offset: 0x000145CE
			void IDisposable.Dispose()
			{
				this.rwl.ExitUpgradeableReadLock();
			}

			// Token: 0x040002E8 RID: 744
			private readonly ReaderWriterLockSlim rwl;
		}
	}
}
