using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x020003FF RID: 1023
	[DefaultEvent("Changed")]
	[IODescription("FileSystemWatcherDesc")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class FileSystemWatcher : Component, ISupportInitialize
	{
		// Token: 0x0600265F RID: 9823 RVA: 0x000B0ADC File Offset: 0x000AECDC
		static FileSystemWatcher()
		{
			foreach (object obj in Enum.GetValues(typeof(NotifyFilters)))
			{
				int num = (int)obj;
				FileSystemWatcher.notifyFiltersValidMask |= num;
			}
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000B0B60 File Offset: 0x000AED60
		public FileSystemWatcher()
		{
			this.directory = string.Empty;
			this.filter = "*.*";
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000B0B91 File Offset: 0x000AED91
		public FileSystemWatcher(string path)
			: this(path, "*.*")
		{
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000B0BA0 File Offset: 0x000AEDA0
		public FileSystemWatcher(string path, string filter)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			if (path.Length == 0 || !Directory.Exists(path))
			{
				throw new ArgumentException(SR.GetString("InvalidDirName", new object[] { path }));
			}
			this.directory = path;
			this.filter = filter;
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x000B0C1A File Offset: 0x000AEE1A
		// (set) Token: 0x06002664 RID: 9828 RVA: 0x000B0C22 File Offset: 0x000AEE22
		[DefaultValue(NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite)]
		[IODescription("FSW_ChangedFilter")]
		public NotifyFilters NotifyFilter
		{
			get
			{
				return this.notifyFilters;
			}
			set
			{
				if ((value & (NotifyFilters)(~(NotifyFilters)FileSystemWatcher.notifyFiltersValidMask)) != (NotifyFilters)0)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(NotifyFilters));
				}
				if (this.notifyFilters != value)
				{
					this.notifyFilters = value;
					this.Restart();
				}
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x000B0C5A File Offset: 0x000AEE5A
		// (set) Token: 0x06002666 RID: 9830 RVA: 0x000B0C62 File Offset: 0x000AEE62
		[DefaultValue(false)]
		[IODescription("FSW_Enabled")]
		public bool EnableRaisingEvents
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled == value)
				{
					return;
				}
				this.enabled = value;
				if (!this.IsSuspended())
				{
					if (this.enabled)
					{
						this.StartRaisingEvents();
						return;
					}
					this.StopRaisingEvents();
				}
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x000B0C92 File Offset: 0x000AEE92
		// (set) Token: 0x06002668 RID: 9832 RVA: 0x000B0C9A File Offset: 0x000AEE9A
		[DefaultValue("*.*")]
		[IODescription("FSW_Filter")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SettingsBindable(true)]
		public string Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = "*.*";
				}
				if (string.Compare(this.filter, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.filter = value;
				}
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x000B0CC1 File Offset: 0x000AEEC1
		// (set) Token: 0x0600266A RID: 9834 RVA: 0x000B0CC9 File Offset: 0x000AEEC9
		[DefaultValue(false)]
		[IODescription("FSW_IncludeSubdirectories")]
		public bool IncludeSubdirectories
		{
			get
			{
				return this.includeSubdirectories;
			}
			set
			{
				if (this.includeSubdirectories != value)
				{
					this.includeSubdirectories = value;
					this.Restart();
				}
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600266B RID: 9835 RVA: 0x000B0CE1 File Offset: 0x000AEEE1
		// (set) Token: 0x0600266C RID: 9836 RVA: 0x000B0CE9 File Offset: 0x000AEEE9
		[Browsable(false)]
		[DefaultValue(8192)]
		public int InternalBufferSize
		{
			get
			{
				return this.internalBufferSize;
			}
			set
			{
				if (this.internalBufferSize != value)
				{
					if (value < 4096)
					{
						value = 4096;
					}
					this.internalBufferSize = value;
					this.Restart();
				}
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x000B0D10 File Offset: 0x000AEF10
		private bool IsHandleInvalid
		{
			get
			{
				return this.directoryHandle == null || this.directoryHandle.IsInvalid;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x0600266E RID: 9838 RVA: 0x000B0D27 File Offset: 0x000AEF27
		// (set) Token: 0x0600266F RID: 9839 RVA: 0x000B0D30 File Offset: 0x000AEF30
		[DefaultValue("")]
		[IODescription("FSW_Path")]
		[Editor("System.Diagnostics.Design.FSWPathEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SettingsBindable(true)]
		public string Path
		{
			get
			{
				return this.directory;
			}
			set
			{
				value = ((value == null) ? string.Empty : value);
				if (string.Compare(this.directory, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					if (base.DesignMode)
					{
						if (value.IndexOfAny(FileSystemWatcher.wildcards) != -1 || value.IndexOfAny(global::System.IO.Path.GetInvalidPathChars()) != -1)
						{
							throw new ArgumentException(SR.GetString("InvalidDirName", new object[] { value }));
						}
					}
					else if (!Directory.Exists(value))
					{
						throw new ArgumentException(SR.GetString("InvalidDirName", new object[] { value }));
					}
					this.directory = value;
					this.readGranted = false;
					this.Restart();
				}
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06002670 RID: 9840 RVA: 0x000B0DCD File Offset: 0x000AEFCD
		// (set) Token: 0x06002671 RID: 9841 RVA: 0x000B0DD5 File Offset: 0x000AEFD5
		[Browsable(false)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (this.Site != null && this.Site.DesignMode)
				{
					this.EnableRaisingEvents = true;
				}
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06002672 RID: 9842 RVA: 0x000B0DFC File Offset: 0x000AEFFC
		// (set) Token: 0x06002673 RID: 9843 RVA: 0x000B0E56 File Offset: 0x000AF056
		[Browsable(false)]
		[DefaultValue(null)]
		[IODescription("FSW_SynchronizingObject")]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				if (this.synchronizingObject == null && base.DesignMode)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						object rootComponent = designerHost.RootComponent;
						if (rootComponent != null && rootComponent is ISynchronizeInvoke)
						{
							this.synchronizingObject = (ISynchronizeInvoke)rootComponent;
						}
					}
				}
				return this.synchronizingObject;
			}
			set
			{
				this.synchronizingObject = value;
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06002674 RID: 9844 RVA: 0x000B0E5F File Offset: 0x000AF05F
		// (remove) Token: 0x06002675 RID: 9845 RVA: 0x000B0E78 File Offset: 0x000AF078
		[IODescription("FSW_Changed")]
		public event FileSystemEventHandler Changed
		{
			add
			{
				this.onChangedHandler = (FileSystemEventHandler)Delegate.Combine(this.onChangedHandler, value);
			}
			remove
			{
				this.onChangedHandler = (FileSystemEventHandler)Delegate.Remove(this.onChangedHandler, value);
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06002676 RID: 9846 RVA: 0x000B0E91 File Offset: 0x000AF091
		// (remove) Token: 0x06002677 RID: 9847 RVA: 0x000B0EAA File Offset: 0x000AF0AA
		[IODescription("FSW_Created")]
		public event FileSystemEventHandler Created
		{
			add
			{
				this.onCreatedHandler = (FileSystemEventHandler)Delegate.Combine(this.onCreatedHandler, value);
			}
			remove
			{
				this.onCreatedHandler = (FileSystemEventHandler)Delegate.Remove(this.onCreatedHandler, value);
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06002678 RID: 9848 RVA: 0x000B0EC3 File Offset: 0x000AF0C3
		// (remove) Token: 0x06002679 RID: 9849 RVA: 0x000B0EDC File Offset: 0x000AF0DC
		[IODescription("FSW_Deleted")]
		public event FileSystemEventHandler Deleted
		{
			add
			{
				this.onDeletedHandler = (FileSystemEventHandler)Delegate.Combine(this.onDeletedHandler, value);
			}
			remove
			{
				this.onDeletedHandler = (FileSystemEventHandler)Delegate.Remove(this.onDeletedHandler, value);
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x0600267A RID: 9850 RVA: 0x000B0EF5 File Offset: 0x000AF0F5
		// (remove) Token: 0x0600267B RID: 9851 RVA: 0x000B0F0E File Offset: 0x000AF10E
		[Browsable(false)]
		public event ErrorEventHandler Error
		{
			add
			{
				this.onErrorHandler = (ErrorEventHandler)Delegate.Combine(this.onErrorHandler, value);
			}
			remove
			{
				this.onErrorHandler = (ErrorEventHandler)Delegate.Remove(this.onErrorHandler, value);
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x0600267C RID: 9852 RVA: 0x000B0F27 File Offset: 0x000AF127
		// (remove) Token: 0x0600267D RID: 9853 RVA: 0x000B0F40 File Offset: 0x000AF140
		[IODescription("FSW_Renamed")]
		public event RenamedEventHandler Renamed
		{
			add
			{
				this.onRenamedHandler = (RenamedEventHandler)Delegate.Combine(this.onRenamedHandler, value);
			}
			remove
			{
				this.onRenamedHandler = (RenamedEventHandler)Delegate.Remove(this.onRenamedHandler, value);
			}
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000B0F5C File Offset: 0x000AF15C
		public void BeginInit()
		{
			bool flag = this.enabled;
			this.StopRaisingEvents();
			this.enabled = flag;
			this.initializing = true;
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000B0F84 File Offset: 0x000AF184
		private unsafe void CompletionStatusChanged(uint errorCode, uint numBytes, NativeOverlapped* overlappedPointer)
		{
			Overlapped overlapped = Overlapped.Unpack(overlappedPointer);
			FileSystemWatcher.FSWAsyncResult fswasyncResult = (FileSystemWatcher.FSWAsyncResult)overlapped.AsyncResult;
			try
			{
				if (!this.stopListening)
				{
					lock (this)
					{
						if (errorCode != 0U)
						{
							if (errorCode != 995U)
							{
								this.OnError(new ErrorEventArgs(new Win32Exception((int)errorCode)));
								this.EnableRaisingEvents = false;
							}
						}
						else if (fswasyncResult.session == this.currentSession)
						{
							if (numBytes == 0U)
							{
								this.NotifyInternalBufferOverflowEvent();
							}
							else
							{
								int num = 0;
								string text = null;
								string text2 = null;
								int num2;
								do
								{
									int num3;
									try
									{
										byte[] array;
										byte* ptr;
										if ((array = fswasyncResult.buffer) == null || array.Length == 0)
										{
											ptr = null;
										}
										else
										{
											ptr = &array[0];
										}
										num2 = *(int*)(ptr + num);
										num3 = *(int*)(ptr + num + 4);
										int num4 = *(int*)(ptr + num + 8);
										text2 = new string((char*)(ptr + num + 12), 0, num4 / 2);
									}
									finally
									{
										byte[] array = null;
									}
									if (num3 == 4)
									{
										text = text2;
									}
									else if (num3 == 5)
									{
										if (text != null)
										{
											this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, text2, text);
											text = null;
										}
										else
										{
											this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, text2, text);
											text = null;
										}
									}
									else
									{
										if (text != null)
										{
											this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, null, text);
											text = null;
										}
										this.NotifyFileSystemEventArgs(num3, text2);
									}
									num += num2;
								}
								while (num2 != 0);
								if (text != null)
								{
									this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, null, text);
									text = null;
								}
							}
						}
					}
				}
			}
			finally
			{
				Overlapped.Free(overlappedPointer);
				if (!this.stopListening && !this.runOnce)
				{
					this.Monitor(fswasyncResult.buffer);
				}
			}
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000B1158 File Offset: 0x000AF358
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.StopRaisingEvents();
					this.onChangedHandler = null;
					this.onCreatedHandler = null;
					this.onDeletedHandler = null;
					this.onRenamedHandler = null;
					this.onErrorHandler = null;
					this.readGranted = false;
				}
				else
				{
					this.stopListening = true;
					if (!this.IsHandleInvalid)
					{
						this.directoryHandle.Close();
					}
				}
			}
			finally
			{
				this.disposed = true;
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000B11D8 File Offset: 0x000AF3D8
		public void EndInit()
		{
			this.initializing = false;
			if (this.directory.Length != 0 && this.enabled)
			{
				this.StartRaisingEvents();
			}
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000B11FC File Offset: 0x000AF3FC
		private bool IsSuspended()
		{
			return this.initializing || base.DesignMode;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000B1210 File Offset: 0x000AF410
		private bool MatchPattern(string relativePath)
		{
			string fileName = global::System.IO.Path.GetFileName(relativePath);
			return fileName != null && PatternMatcher.StrictMatchPattern(this.filter.ToUpper(CultureInfo.InvariantCulture), fileName.ToUpper(CultureInfo.InvariantCulture));
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000B124C File Offset: 0x000AF44C
		private unsafe void Monitor(byte[] buffer)
		{
			if (!this.enabled || this.IsHandleInvalid)
			{
				return;
			}
			Overlapped overlapped = new Overlapped();
			if (buffer == null)
			{
				try
				{
					buffer = new byte[this.internalBufferSize];
				}
				catch (OutOfMemoryException)
				{
					throw new OutOfMemoryException(SR.GetString("BufferSizeTooLarge", new object[] { this.internalBufferSize.ToString(CultureInfo.CurrentCulture) }));
				}
			}
			overlapped.AsyncResult = new FileSystemWatcher.FSWAsyncResult
			{
				session = this.currentSession,
				buffer = buffer
			};
			NativeOverlapped* ptr = overlapped.Pack(new IOCompletionCallback(this.CompletionStatusChanged), buffer);
			bool flag = false;
			try
			{
				if (!this.IsHandleInvalid)
				{
					try
					{
						byte[] array;
						byte* ptr2;
						if ((array = buffer) == null || array.Length == 0)
						{
							ptr2 = null;
						}
						else
						{
							ptr2 = &array[0];
						}
						int num;
						flag = Microsoft.Win32.UnsafeNativeMethods.ReadDirectoryChangesW(this.directoryHandle, new HandleRef(this, (IntPtr)((void*)ptr2)), this.internalBufferSize, this.includeSubdirectories ? 1 : 0, (int)this.notifyFilters, out num, ptr, Microsoft.Win32.NativeMethods.NullHandleRef);
					}
					finally
					{
						byte[] array = null;
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
			catch (ArgumentNullException)
			{
			}
			finally
			{
				if (!flag)
				{
					Overlapped.Free(ptr);
					if (!this.IsHandleInvalid)
					{
						this.OnError(new ErrorEventArgs(new Win32Exception()));
					}
				}
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000B13B4 File Offset: 0x000AF5B4
		private void NotifyFileSystemEventArgs(int action, string name)
		{
			if (!this.MatchPattern(name))
			{
				return;
			}
			switch (action)
			{
			case 1:
				this.OnCreated(new FileSystemEventArgs(WatcherChangeTypes.Created, this.directory, name));
				return;
			case 2:
				this.OnDeleted(new FileSystemEventArgs(WatcherChangeTypes.Deleted, this.directory, name));
				return;
			case 3:
				this.OnChanged(new FileSystemEventArgs(WatcherChangeTypes.Changed, this.directory, name));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000B141C File Offset: 0x000AF61C
		private void NotifyInternalBufferOverflowEvent()
		{
			InternalBufferOverflowException ex = new InternalBufferOverflowException(SR.GetString("FSW_BufferOverflow", new object[] { this.directory }));
			ErrorEventArgs errorEventArgs = new ErrorEventArgs(ex);
			this.OnError(errorEventArgs);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000B1458 File Offset: 0x000AF658
		private void NotifyRenameEventArgs(WatcherChangeTypes action, string name, string oldName)
		{
			if (!this.MatchPattern(name) && !this.MatchPattern(oldName))
			{
				return;
			}
			RenamedEventArgs renamedEventArgs = new RenamedEventArgs(action, this.directory, name, oldName);
			this.OnRenamed(renamedEventArgs);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000B1490 File Offset: 0x000AF690
		protected void OnChanged(FileSystemEventArgs e)
		{
			FileSystemEventHandler fileSystemEventHandler = this.onChangedHandler;
			if (fileSystemEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(fileSystemEventHandler, new object[] { this, e });
					return;
				}
				fileSystemEventHandler(this, e);
			}
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000B14E0 File Offset: 0x000AF6E0
		protected void OnCreated(FileSystemEventArgs e)
		{
			FileSystemEventHandler fileSystemEventHandler = this.onCreatedHandler;
			if (fileSystemEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(fileSystemEventHandler, new object[] { this, e });
					return;
				}
				fileSystemEventHandler(this, e);
			}
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000B1530 File Offset: 0x000AF730
		protected void OnDeleted(FileSystemEventArgs e)
		{
			FileSystemEventHandler fileSystemEventHandler = this.onDeletedHandler;
			if (fileSystemEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(fileSystemEventHandler, new object[] { this, e });
					return;
				}
				fileSystemEventHandler(this, e);
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000B1580 File Offset: 0x000AF780
		protected void OnError(ErrorEventArgs e)
		{
			ErrorEventHandler errorEventHandler = this.onErrorHandler;
			if (errorEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(errorEventHandler, new object[] { this, e });
					return;
				}
				errorEventHandler(this, e);
			}
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000B15D0 File Offset: 0x000AF7D0
		private void OnInternalFileSystemEventArgs(object sender, FileSystemEventArgs e)
		{
			lock (this)
			{
				if (!this.isChanged)
				{
					this.changedResult = new WaitForChangedResult(e.ChangeType, e.Name, false);
					this.isChanged = true;
					global::System.Threading.Monitor.Pulse(this);
				}
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000B1634 File Offset: 0x000AF834
		private void OnInternalRenameEventArgs(object sender, RenamedEventArgs e)
		{
			lock (this)
			{
				if (!this.isChanged)
				{
					this.changedResult = new WaitForChangedResult(e.ChangeType, e.Name, e.OldName, false);
					this.isChanged = true;
					global::System.Threading.Monitor.Pulse(this);
				}
			}
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000B169C File Offset: 0x000AF89C
		protected void OnRenamed(RenamedEventArgs e)
		{
			RenamedEventHandler renamedEventHandler = this.onRenamedHandler;
			if (renamedEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(renamedEventHandler, new object[] { this, e });
					return;
				}
				renamedEventHandler(this, e);
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000B16EC File Offset: 0x000AF8EC
		private void Restart()
		{
			if (!this.IsSuspended() && this.enabled)
			{
				this.StopRaisingEvents();
				this.StartRaisingEvents();
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000B170C File Offset: 0x000AF90C
		private void StartRaisingEvents()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				new EnvironmentPermission(PermissionState.Unrestricted).Assert();
				if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (this.IsSuspended())
			{
				this.enabled = true;
				return;
			}
			if (!this.readGranted)
			{
				string fullPath = global::System.IO.Path.GetFullPath(this.directory);
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, fullPath);
				fileIOPermission.Demand();
				this.readGranted = true;
			}
			if (!this.IsHandleInvalid)
			{
				return;
			}
			this.directoryHandle = Microsoft.Win32.NativeMethods.CreateFile(this.directory, 1, 7, null, 3, 1107296256, new SafeFileHandle(IntPtr.Zero, false));
			if (this.IsHandleInvalid)
			{
				throw new FileNotFoundException(SR.GetString("FSW_IOError", new object[] { this.directory }));
			}
			this.stopListening = false;
			Interlocked.Increment(ref this.currentSession);
			SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
			securityPermission.Assert();
			try
			{
				ThreadPool.BindHandle(this.directoryHandle);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.enabled = true;
			this.Monitor(null);
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000B1850 File Offset: 0x000AFA50
		private void StopRaisingEvents()
		{
			if (this.IsSuspended())
			{
				this.enabled = false;
				return;
			}
			if (this.IsHandleInvalid)
			{
				return;
			}
			this.stopListening = true;
			this.directoryHandle.Close();
			this.directoryHandle = null;
			Interlocked.Increment(ref this.currentSession);
			this.enabled = false;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000B18A2 File Offset: 0x000AFAA2
		public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
		{
			return this.WaitForChanged(changeType, -1);
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000B18AC File Offset: 0x000AFAAC
		public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
		{
			FileSystemEventHandler fileSystemEventHandler = new FileSystemEventHandler(this.OnInternalFileSystemEventArgs);
			RenamedEventHandler renamedEventHandler = new RenamedEventHandler(this.OnInternalRenameEventArgs);
			this.isChanged = false;
			this.changedResult = WaitForChangedResult.TimedOutResult;
			if ((changeType & WatcherChangeTypes.Created) != (WatcherChangeTypes)0)
			{
				this.Created += fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Deleted) != (WatcherChangeTypes)0)
			{
				this.Deleted += fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Changed) != (WatcherChangeTypes)0)
			{
				this.Changed += fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Renamed) != (WatcherChangeTypes)0)
			{
				this.Renamed += renamedEventHandler;
			}
			bool enableRaisingEvents = this.EnableRaisingEvents;
			if (!enableRaisingEvents)
			{
				this.runOnce = true;
				this.EnableRaisingEvents = true;
			}
			WaitForChangedResult timedOutResult = WaitForChangedResult.TimedOutResult;
			lock (this)
			{
				if (timeout == -1)
				{
					while (!this.isChanged)
					{
						global::System.Threading.Monitor.Wait(this);
					}
				}
				else
				{
					global::System.Threading.Monitor.Wait(this, timeout, true);
				}
				timedOutResult = this.changedResult;
			}
			this.EnableRaisingEvents = enableRaisingEvents;
			this.runOnce = false;
			if ((changeType & WatcherChangeTypes.Created) != (WatcherChangeTypes)0)
			{
				this.Created -= fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Deleted) != (WatcherChangeTypes)0)
			{
				this.Deleted -= fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Changed) != (WatcherChangeTypes)0)
			{
				this.Changed -= fileSystemEventHandler;
			}
			if ((changeType & WatcherChangeTypes.Renamed) != (WatcherChangeTypes)0)
			{
				this.Renamed -= renamedEventHandler;
			}
			return timedOutResult;
		}

		// Token: 0x040020B0 RID: 8368
		private string directory;

		// Token: 0x040020B1 RID: 8369
		private string filter;

		// Token: 0x040020B2 RID: 8370
		private SafeFileHandle directoryHandle;

		// Token: 0x040020B3 RID: 8371
		private const NotifyFilters defaultNotifyFilters = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;

		// Token: 0x040020B4 RID: 8372
		private NotifyFilters notifyFilters = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;

		// Token: 0x040020B5 RID: 8373
		private bool includeSubdirectories;

		// Token: 0x040020B6 RID: 8374
		private bool enabled;

		// Token: 0x040020B7 RID: 8375
		private bool initializing;

		// Token: 0x040020B8 RID: 8376
		private int internalBufferSize = 8192;

		// Token: 0x040020B9 RID: 8377
		private WaitForChangedResult changedResult;

		// Token: 0x040020BA RID: 8378
		private bool isChanged;

		// Token: 0x040020BB RID: 8379
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x040020BC RID: 8380
		private bool readGranted;

		// Token: 0x040020BD RID: 8381
		private bool disposed;

		// Token: 0x040020BE RID: 8382
		private int currentSession;

		// Token: 0x040020BF RID: 8383
		private FileSystemEventHandler onChangedHandler;

		// Token: 0x040020C0 RID: 8384
		private FileSystemEventHandler onCreatedHandler;

		// Token: 0x040020C1 RID: 8385
		private FileSystemEventHandler onDeletedHandler;

		// Token: 0x040020C2 RID: 8386
		private RenamedEventHandler onRenamedHandler;

		// Token: 0x040020C3 RID: 8387
		private ErrorEventHandler onErrorHandler;

		// Token: 0x040020C4 RID: 8388
		private bool stopListening;

		// Token: 0x040020C5 RID: 8389
		private bool runOnce;

		// Token: 0x040020C6 RID: 8390
		private static readonly char[] wildcards = new char[] { '?', '*' };

		// Token: 0x040020C7 RID: 8391
		private static int notifyFiltersValidMask = 0;

		// Token: 0x02000813 RID: 2067
		private sealed class FSWAsyncResult : IAsyncResult
		{
			// Token: 0x17000FA0 RID: 4000
			// (get) Token: 0x060044F0 RID: 17648 RVA: 0x0012082E File Offset: 0x0011EA2E
			public bool IsCompleted
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000FA1 RID: 4001
			// (get) Token: 0x060044F1 RID: 17649 RVA: 0x00120835 File Offset: 0x0011EA35
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000FA2 RID: 4002
			// (get) Token: 0x060044F2 RID: 17650 RVA: 0x0012083C File Offset: 0x0011EA3C
			public object AsyncState
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000FA3 RID: 4003
			// (get) Token: 0x060044F3 RID: 17651 RVA: 0x00120843 File Offset: 0x0011EA43
			public bool CompletedSynchronously
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x04003574 RID: 13684
			internal int session;

			// Token: 0x04003575 RID: 13685
			internal byte[] buffer;
		}
	}
}
