using System;
using System.Threading;

namespace IPA.Config
{
	/// <summary>
	/// A storage for a config structure.
	/// </summary>
	// Token: 0x0200005F RID: 95
	public interface IConfigStore
	{
		/// <summary>
		/// A synchronization object for the save thread to wait on for changes. 
		/// It should be signaled whenever the internal state of the object is changed.
		/// The writer will never signal this handle. 
		/// </summary>
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002A6 RID: 678
		WaitHandle SyncObject { get; }

		/// <summary>
		/// A synchronization object for the load thread and accessors to maintain safe synchronization.
		/// Any readers should take a read lock with <see cref="M:System.Threading.ReaderWriterLockSlim.EnterReadLock" /> or
		/// <see cref="M:System.Threading.ReaderWriterLockSlim.EnterUpgradeableReadLock" />, and any writers should take a 
		/// write lock with <see cref="M:System.Threading.ReaderWriterLockSlim.EnterWriteLock" />.
		/// </summary>
		/// <remarks>
		/// Read and write are read and write to *this object*, not to the file on disk.
		/// </remarks>
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002A7 RID: 679
		ReaderWriterLockSlim WriteSyncObject { get; }

		/// <summary>
		/// Writes the config structure stored by the current <see cref="T:IPA.Config.IConfigStore" /> to the given
		/// <see cref="T:IPA.Config.IConfigProvider" />.
		/// </summary>
		/// <remarks>
		/// The calling code will have entered a read lock on <see cref="P:IPA.Config.IConfigStore.WriteSyncObject" /> when
		/// this is called.
		/// </remarks>
		/// <param name="provider">the provider to write to</param>
		// Token: 0x060002A8 RID: 680
		void WriteTo(ConfigProvider provider);

		/// <summary>
		/// Reads the config structure from the given <see cref="T:IPA.Config.IConfigProvider" /> into the current 
		/// <see cref="T:IPA.Config.IConfigStore" />.
		/// </summary>
		/// <remarks>
		/// The calling code will have entered a write lock on <see cref="P:IPA.Config.IConfigStore.WriteSyncObject" /> when
		/// this is called.
		/// </remarks>
		/// <param name="provider">the provider to read from</param>
		// Token: 0x060002A9 RID: 681
		void ReadFrom(ConfigProvider provider);
	}
}
