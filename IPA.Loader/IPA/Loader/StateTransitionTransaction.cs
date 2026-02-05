using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPA.Loader
{
	/// <summary>
	/// A class to represent a transaction for changing the state of loaded mods.
	/// </summary>
	// Token: 0x0200004A RID: 74
	public sealed class StateTransitionTransaction : IDisposable
	{
		// Token: 0x060001FC RID: 508 RVA: 0x0000A9A8 File Offset: 0x00008BA8
		internal StateTransitionTransaction(IEnumerable<PluginMetadata> enabled, IEnumerable<PluginMetadata> disabled)
		{
			this.currentlyEnabled = new HashSet<PluginMetadata>(enabled.ToArray<PluginMetadata>());
			this.currentlyDisabled = new HashSet<PluginMetadata>(disabled.ToArray<PluginMetadata>());
		}

		/// <summary>
		/// Gets whether or not a game restart will be necessary to fully apply this transaction.
		/// </summary>
		/// <value><see langword="true" /> if any mod who's state is changed cannot be changed at runtime, <see langword="false" /> otherwise</value>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000A9E8 File Offset: 0x00008BE8
		public bool WillNeedRestart
		{
			get
			{
				if (this.ThrowIfDisposed<bool>())
				{
					return true;
				}
				if (this.stateChanged)
				{
					return this.toEnable.Concat(this.toDisable).Any((PluginMetadata m) => m.RuntimeOptions != RuntimeOptions.DynamicInit);
				}
				return false;
			}
		}

		/// <summary>
		/// Gets whether or not the current state has changed.
		/// </summary>
		/// <value><see langword="true" /> if the current state of the transaction is different from its construction, <see langword="false" /> otherwise</value>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000AA3E File Offset: 0x00008C3E
		public bool HasStateChanged
		{
			get
			{
				return this.ThrowIfDisposed<bool>() || this.stateChanged;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000AA50 File Offset: 0x00008C50
		internal IEnumerable<PluginMetadata> CurrentlyEnabled
		{
			get
			{
				return this.currentlyEnabled;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000AA58 File Offset: 0x00008C58
		internal IEnumerable<PluginMetadata> CurrentlyDisabled
		{
			get
			{
				return this.currentlyDisabled;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000AA60 File Offset: 0x00008C60
		internal IEnumerable<PluginMetadata> ToEnable
		{
			get
			{
				return this.toEnable;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000AA68 File Offset: 0x00008C68
		internal IEnumerable<PluginMetadata> ToDisable
		{
			get
			{
				return this.toDisable;
			}
		}

		/// <summary>
		/// Gets a list of plugins that are enabled according to this transaction's current state.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000AA70 File Offset: 0x00008C70
		public IEnumerable<PluginMetadata> EnabledPlugins
		{
			get
			{
				return this.ThrowIfDisposed<IEnumerable<PluginMetadata>>() ?? this.EnabledPluginsInternal;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000AA82 File Offset: 0x00008C82
		private IEnumerable<PluginMetadata> EnabledPluginsInternal
		{
			get
			{
				return this.currentlyEnabled.Except(this.toDisable).Concat(this.toEnable);
			}
		}

		/// <summary>
		/// Gets a list of plugins that are disabled according to this transaction's current state.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000AAA0 File Offset: 0x00008CA0
		public IEnumerable<PluginMetadata> DisabledPlugins
		{
			get
			{
				return this.ThrowIfDisposed<IEnumerable<PluginMetadata>>() ?? this.DisabledPluginsInternal;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000AAB2 File Offset: 0x00008CB2
		private IEnumerable<PluginMetadata> DisabledPluginsInternal
		{
			get
			{
				return this.currentlyDisabled.Except(this.toEnable).Concat(this.toDisable);
			}
		}

		/// <summary>
		/// Checks if a plugin is enabled according to this transaction's current state.
		/// </summary>
		/// <remarks>
		/// <para>This should be roughly equivalent to <c>EnabledPlugins.Contains(meta)</c>, but more performant.</para>
		/// <para>This should also always return the inverse of <see cref="M:IPA.Loader.StateTransitionTransaction.IsDisabled(IPA.Loader.PluginMetadata)" /> for valid plugins.</para>
		/// </remarks>
		/// <param name="meta">the plugin to check</param>
		/// <returns><see langword="true" /> if the plugin is enabled, <see langword="false" /> otherwise</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		/// <seealso cref="P:IPA.Loader.StateTransitionTransaction.EnabledPlugins" />
		/// <seealso cref="M:IPA.Loader.StateTransitionTransaction.IsDisabled(IPA.Loader.PluginMetadata)" />
		// Token: 0x06000207 RID: 519 RVA: 0x0000AAD0 File Offset: 0x00008CD0
		public bool IsEnabled(PluginMetadata meta)
		{
			return this.ThrowIfDisposed<bool>() || this.IsEnabledInternal(meta);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000AAE3 File Offset: 0x00008CE3
		private bool IsEnabledInternal(PluginMetadata meta)
		{
			return (this.currentlyEnabled.Contains(meta) && !this.toDisable.Contains(meta)) || this.toEnable.Contains(meta);
		}

		/// <summary>
		/// Checks if a plugin is disabled according to this transaction's current state.
		/// </summary>
		/// <remarks>
		/// <para>This should be roughly equivalent to <c>DisabledPlugins.Contains(meta)</c>, but more performant.</para>
		/// <para>This should also always return the inverse of <see cref="M:IPA.Loader.StateTransitionTransaction.IsEnabled(IPA.Loader.PluginMetadata)" /> for valid plugins.</para>
		/// </remarks>
		/// <param name="meta">the plugin to check</param>
		/// <returns><see langword="true" /> if the plugin is disabled, <see langword="false" /> otherwise</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		/// <seealso cref="P:IPA.Loader.StateTransitionTransaction.DisabledPlugins" />
		/// <seealso cref="M:IPA.Loader.StateTransitionTransaction.IsEnabled(IPA.Loader.PluginMetadata)" />
		// Token: 0x06000209 RID: 521 RVA: 0x0000AB0F File Offset: 0x00008D0F
		public bool IsDisabled(PluginMetadata meta)
		{
			return this.ThrowIfDisposed<bool>() || this.IsDisabledInternal(meta);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000AB22 File Offset: 0x00008D22
		private bool IsDisabledInternal(PluginMetadata meta)
		{
			return (this.currentlyDisabled.Contains(meta) && !this.toEnable.Contains(meta)) || this.toDisable.Contains(meta);
		}

		/// <summary>
		/// Enables a plugin in this transaction.
		/// </summary>
		/// <param name="meta">the plugin to enable</param>
		/// <param name="autoDeps">whether or not to automatically enable all dependencies of the plugin</param>
		/// <returns><see langword="true" /> if the transaction's state was changed, <see langword="false" /> otherwise</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		/// <exception cref="T:System.ArgumentException">if <paramref name="meta" /> is not loadable</exception>
		/// <seealso cref="M:IPA.Loader.StateTransitionTransaction.Enable(IPA.Loader.PluginMetadata,System.Collections.Generic.IEnumerable{IPA.Loader.PluginMetadata}@,System.Boolean)" />
		// Token: 0x0600020B RID: 523 RVA: 0x0000AB50 File Offset: 0x00008D50
		public bool Enable(PluginMetadata meta, bool autoDeps = true)
		{
			IEnumerable<PluginMetadata> enumerable;
			return this.Enable(meta, out enumerable, autoDeps);
		}

		/// <summary>
		/// Enables a plugin in this transaction.
		/// </summary>
		/// <remarks>
		/// <paramref name="disabledDeps" /> will only be set when <paramref name="autoDeps" /> is <see langword="false" />.
		/// </remarks>
		/// <param name="meta">the plugin to enable</param>
		/// <param name="disabledDeps"><see langword="null" /> if successful, otherwise a set of plugins that need to be enabled first</param>
		/// <param name="autoDeps">whether or not to automatically enable all dependencies</param>
		/// <returns><see langword="true" /> if the transaction's state was changed, <see langword="false" /> otherwise</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		/// <exception cref="T:System.ArgumentException">if <paramref name="meta" /> is not loadable</exception>
		// Token: 0x0600020C RID: 524 RVA: 0x0000AB68 File Offset: 0x00008D68
		public bool Enable(PluginMetadata meta, out IEnumerable<PluginMetadata> disabledDeps, bool autoDeps = false)
		{
			this.ThrowIfDisposed();
			if (!this.currentlyEnabled.Contains(meta) && !this.currentlyDisabled.Contains(meta))
			{
				throw new ArgumentException("meta", "Plugin metadata does not represent a loadable plugin");
			}
			disabledDeps = null;
			if (this.IsEnabledInternal(meta))
			{
				return false;
			}
			IEnumerable<PluginMetadata> needsEnabled = meta.Dependencies.Where((PluginMetadata m) => this.DisabledPluginsInternal.Contains(m));
			if (autoDeps)
			{
				using (IEnumerator<PluginMetadata> enumerator = needsEnabled.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PluginMetadata dep = enumerator.Current;
						IEnumerable<PluginMetadata> failedDisabled;
						bool res = this.Enable(dep, out failedDisabled, true);
						if (failedDisabled != null)
						{
							disabledDeps = failedDisabled;
							return res;
						}
					}
					goto IL_00A4;
				}
			}
			if (needsEnabled.Any<PluginMetadata>())
			{
				disabledDeps = needsEnabled;
				return false;
			}
			IL_00A4:
			this.toDisable.Remove(meta);
			this.toEnable.Add(meta);
			this.stateChanged = true;
			return true;
		}

		/// <summary>
		/// Disables a plugin in this transaction.
		/// </summary>
		/// <param name="meta">the plugin to disable</param>
		/// <param name="autoDependents">whether or not to automatically disable all dependents of the plugin</param>
		/// <returns><see langword="true" /> if the transaction's state was changed, <see langword="false" /> otherwise</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		/// <exception cref="T:System.ArgumentException">if <paramref name="meta" /> is not loadable</exception>
		/// <seealso cref="M:IPA.Loader.StateTransitionTransaction.Disable(IPA.Loader.PluginMetadata,System.Collections.Generic.IEnumerable{IPA.Loader.PluginMetadata}@,System.Boolean)" />
		// Token: 0x0600020D RID: 525 RVA: 0x0000AC50 File Offset: 0x00008E50
		public bool Disable(PluginMetadata meta, bool autoDependents = true)
		{
			IEnumerable<PluginMetadata> enumerable;
			return this.Disable(meta, out enumerable, autoDependents);
		}

		/// <summary>
		/// Disables a plugin in this transaction.
		/// </summary>
		/// <remarks>
		/// <paramref name="enabledDependents" /> will only be set when <paramref name="autoDependents" /> is <see langword="false" />.
		/// </remarks>
		/// <param name="meta">the plugin to disable</param>
		/// <param name="enabledDependents"><see langword="null" /> if successful, otherwise a set of plugins that need to be disabled first</param>
		/// <param name="autoDependents">whether or not to automatically disable all dependents of the plugin</param>
		/// <returns><see langword="true" /> if the transaction's state was changed, <see langword="false" /> otherwise</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		/// <exception cref="T:System.ArgumentException">if <paramref name="meta" /> is not loadable</exception>
		// Token: 0x0600020E RID: 526 RVA: 0x0000AC68 File Offset: 0x00008E68
		public bool Disable(PluginMetadata meta, out IEnumerable<PluginMetadata> enabledDependents, bool autoDependents = false)
		{
			this.ThrowIfDisposed();
			if (!this.currentlyEnabled.Contains(meta) && !this.currentlyDisabled.Contains(meta))
			{
				throw new ArgumentException("meta", "Plugin metadata does not represent a loadable plugin");
			}
			enabledDependents = null;
			if (this.IsDisabledInternal(meta))
			{
				return false;
			}
			IEnumerable<PluginMetadata> needsDisabled = this.EnabledPluginsInternal.Where((PluginMetadata m) => m.Dependencies.Contains(meta));
			if (autoDependents)
			{
				using (IEnumerator<PluginMetadata> enumerator = needsDisabled.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PluginMetadata dep = enumerator.Current;
						IEnumerable<PluginMetadata> failedEnabled;
						bool res = this.Disable(dep, out failedEnabled, true);
						if (failedEnabled != null)
						{
							enabledDependents = failedEnabled;
							return res;
						}
					}
					goto IL_00C2;
				}
			}
			if (needsDisabled.Any<PluginMetadata>())
			{
				enabledDependents = needsDisabled;
				return false;
			}
			IL_00C2:
			this.toDisable.Add(meta);
			this.toEnable.Remove(meta);
			this.stateChanged = true;
			return true;
		}

		/// <summary>
		/// Commits this transaction to actual state, enabling and disabling plugins as necessary.
		/// </summary>
		/// <remarks>
		/// <para>After this completes, this transaction will be disposed.</para>
		/// <para>
		/// The <see cref="T:System.Threading.Tasks.Task" /> that is returned will error if <b>any</b> of the mods being <b>disabled</b>
		/// error. It is up to the caller to handle these in a sane way, like logging them. If nothing else, do something like this:
		/// <code lang="csharp">
		/// // get your transaction...
		/// var complete = transaction.Commit();
		/// await complete.ContinueWith(t =&gt; {
		///     if (t.IsFaulted)
		///         Logger.log.Error($"Error disabling plugins: {t.Exception}");
		/// });
		/// </code>
		/// If you are running in a coroutine, you can use <see cref="M:IPA.Utilities.Async.Coroutines.WaitForTask(System.Threading.Tasks.Task)" /> instead of <see langword="await" />.
		/// </para>
		/// <para>
		/// If you are running on the Unity main thread, this will block until all enabling is done, and will return a task representing the disables.
		/// Otherwise, the task returned represents both, and <i>will not complete</i> until Unity has done (possibly) several updates, depending on
		/// the number of plugins being disabled, and the time they take.
		/// </para>
		/// </remarks>
		/// <returns>a <see cref="T:System.Threading.Tasks.Task" /> which completes whenever all disables complete</returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		/// <exception cref="T:System.InvalidOperationException">if the plugins' state no longer matches this transaction's original state</exception>
		// Token: 0x0600020F RID: 527 RVA: 0x0000AD78 File Offset: 0x00008F78
		public Task Commit()
		{
			return this.ThrowIfDisposed<Task>() ?? PluginManager.CommitTransaction(this);
		}

		/// <summary>
		/// Clones this transaction to be identical, but with unrelated underlying sets.
		/// </summary>
		/// <returns>the new <see cref="T:IPA.Loader.StateTransitionTransaction" /></returns>
		/// <exception cref="T:System.ObjectDisposedException">if this object has been disposed</exception>
		// Token: 0x06000210 RID: 528 RVA: 0x0000AD8C File Offset: 0x00008F8C
		public StateTransitionTransaction Clone()
		{
			this.ThrowIfDisposed();
			StateTransitionTransaction copy = new StateTransitionTransaction(this.CurrentlyEnabled, this.CurrentlyDisabled);
			foreach (PluginMetadata toEnable in this.ToEnable)
			{
				copy.toEnable.Add(toEnable);
			}
			foreach (PluginMetadata toDisable in this.ToDisable)
			{
				copy.toDisable.Add(toDisable);
			}
			copy.stateChanged = this.stateChanged;
			return copy;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000AE48 File Offset: 0x00009048
		private void ThrowIfDisposed()
		{
			this.ThrowIfDisposed<byte>();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000AE54 File Offset: 0x00009054
		private T ThrowIfDisposed<T>()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("StateTransitionTransaction");
			}
			return default(T);
		}

		/// <summary>
		/// Disposes and discards this transaction without committing it.
		/// </summary>
		// Token: 0x06000213 RID: 531 RVA: 0x0000AE7D File Offset: 0x0000907D
		public void Dispose()
		{
			this.disposed = true;
		}

		// Token: 0x040000D3 RID: 211
		private readonly HashSet<PluginMetadata> currentlyEnabled;

		// Token: 0x040000D4 RID: 212
		private readonly HashSet<PluginMetadata> currentlyDisabled;

		// Token: 0x040000D5 RID: 213
		private readonly HashSet<PluginMetadata> toEnable = new HashSet<PluginMetadata>();

		// Token: 0x040000D6 RID: 214
		private readonly HashSet<PluginMetadata> toDisable = new HashSet<PluginMetadata>();

		// Token: 0x040000D7 RID: 215
		private bool stateChanged;

		// Token: 0x040000D8 RID: 216
		private bool disposed;
	}
}
