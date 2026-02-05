using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Timers
{
	// Token: 0x0200006D RID: 109
	[DefaultProperty("Interval")]
	[DefaultEvent("Elapsed")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class Timer : Component, ISupportInitialize
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
		public Timer()
		{
			this.interval = 100.0;
			this.enabled = false;
			this.autoReset = true;
			this.initializing = false;
			this.delayedEnable = false;
			this.callback = new TimerCallback(this.MyTimerCallback);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001F000 File Offset: 0x0001D200
		public Timer(double interval)
			: this()
		{
			if (interval <= 0.0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "interval", interval }));
			}
			double num = Math.Ceiling(interval);
			if (num > 2147483647.0 || num <= 0.0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "interval", interval }));
			}
			this.interval = (double)((int)num);
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0001F095 File Offset: 0x0001D295
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x0001F09D File Offset: 0x0001D29D
		[Category("Behavior")]
		[TimersDescription("TimerAutoReset")]
		[DefaultValue(true)]
		public bool AutoReset
		{
			get
			{
				return this.autoReset;
			}
			set
			{
				if (base.DesignMode)
				{
					this.autoReset = value;
					return;
				}
				if (this.autoReset != value)
				{
					this.autoReset = value;
					if (this.timer != null)
					{
						this.UpdateTimer();
					}
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0001F0CD File Offset: 0x0001D2CD
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x0001F0D8 File Offset: 0x0001D2D8
		[Category("Behavior")]
		[TimersDescription("TimerEnabled")]
		[DefaultValue(false)]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (base.DesignMode)
				{
					this.delayedEnable = value;
					this.enabled = value;
					return;
				}
				if (this.initializing)
				{
					this.delayedEnable = value;
					return;
				}
				if (this.enabled != value)
				{
					if (!value)
					{
						if (this.timer != null)
						{
							this.cookie = null;
							this.timer.Dispose();
							this.timer = null;
						}
						this.enabled = value;
						return;
					}
					this.enabled = value;
					if (this.timer == null)
					{
						if (this.disposed)
						{
							throw new ObjectDisposedException(base.GetType().Name);
						}
						int num = (int)Math.Ceiling(this.interval);
						this.cookie = new object();
						this.timer = new Timer(this.callback, this.cookie, num, this.autoReset ? num : (-1));
						return;
					}
					else
					{
						this.UpdateTimer();
					}
				}
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001F1B0 File Offset: 0x0001D3B0
		private void UpdateTimer()
		{
			int num = (int)Math.Ceiling(this.interval);
			this.timer.Change(num, this.autoReset ? num : (-1));
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0001F1E3 File Offset: 0x0001D3E3
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x0001F1EC File Offset: 0x0001D3EC
		[Category("Behavior")]
		[TimersDescription("TimerInterval")]
		[DefaultValue(100.0)]
		[SettingsBindable(true)]
		public double Interval
		{
			get
			{
				return this.interval;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException(SR.GetString("TimerInvalidInterval", new object[] { value, 0 }));
				}
				this.interval = value;
				if (this.timer != null)
				{
					this.UpdateTimer();
				}
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000481 RID: 1153 RVA: 0x0001F242 File Offset: 0x0001D442
		// (remove) Token: 0x06000482 RID: 1154 RVA: 0x0001F25B File Offset: 0x0001D45B
		[Category("Behavior")]
		[TimersDescription("TimerIntervalElapsed")]
		public event ElapsedEventHandler Elapsed
		{
			add
			{
				this.onIntervalElapsed = (ElapsedEventHandler)Delegate.Combine(this.onIntervalElapsed, value);
			}
			remove
			{
				this.onIntervalElapsed = (ElapsedEventHandler)Delegate.Remove(this.onIntervalElapsed, value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0001F28C File Offset: 0x0001D48C
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x0001F274 File Offset: 0x0001D474
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (base.DesignMode)
				{
					this.enabled = true;
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0001F294 File Offset: 0x0001D494
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x0001F2EE File Offset: 0x0001D4EE
		[Browsable(false)]
		[DefaultValue(null)]
		[TimersDescription("TimerSynchronizingObject")]
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

		// Token: 0x06000487 RID: 1159 RVA: 0x0001F2F7 File Offset: 0x0001D4F7
		public void BeginInit()
		{
			this.Close();
			this.initializing = true;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001F306 File Offset: 0x0001D506
		public void Close()
		{
			this.initializing = false;
			this.delayedEnable = false;
			this.enabled = false;
			if (this.timer != null)
			{
				this.timer.Dispose();
				this.timer = null;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001F337 File Offset: 0x0001D537
		protected override void Dispose(bool disposing)
		{
			this.Close();
			this.disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001F34D File Offset: 0x0001D54D
		public void EndInit()
		{
			this.initializing = false;
			this.Enabled = this.delayedEnable;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001F362 File Offset: 0x0001D562
		public void Start()
		{
			this.Enabled = true;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001F36B File Offset: 0x0001D56B
		public void Stop()
		{
			this.Enabled = false;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001F374 File Offset: 0x0001D574
		private void MyTimerCallback(object state)
		{
			if (state != this.cookie)
			{
				return;
			}
			if (!this.autoReset)
			{
				this.enabled = false;
			}
			Timer.FILE_TIME file_TIME = default(Timer.FILE_TIME);
			Timer.GetSystemTimeAsFileTime(ref file_TIME);
			ElapsedEventArgs elapsedEventArgs = new ElapsedEventArgs(file_TIME.ftTimeLow, file_TIME.ftTimeHigh);
			try
			{
				ElapsedEventHandler elapsedEventHandler = this.onIntervalElapsed;
				if (elapsedEventHandler != null)
				{
					if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
					{
						this.SynchronizingObject.BeginInvoke(elapsedEventHandler, new object[] { this, elapsedEventArgs });
					}
					else
					{
						elapsedEventHandler(this, elapsedEventArgs);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600048E RID: 1166
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern void GetSystemTimeAsFileTime(ref Timer.FILE_TIME lpSystemTimeAsFileTime);

		// Token: 0x04000BD1 RID: 3025
		private double interval;

		// Token: 0x04000BD2 RID: 3026
		private bool enabled;

		// Token: 0x04000BD3 RID: 3027
		private bool initializing;

		// Token: 0x04000BD4 RID: 3028
		private bool delayedEnable;

		// Token: 0x04000BD5 RID: 3029
		private ElapsedEventHandler onIntervalElapsed;

		// Token: 0x04000BD6 RID: 3030
		private bool autoReset;

		// Token: 0x04000BD7 RID: 3031
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x04000BD8 RID: 3032
		private bool disposed;

		// Token: 0x04000BD9 RID: 3033
		private Timer timer;

		// Token: 0x04000BDA RID: 3034
		private TimerCallback callback;

		// Token: 0x04000BDB RID: 3035
		private object cookie;

		// Token: 0x020006E6 RID: 1766
		internal struct FILE_TIME
		{
			// Token: 0x04003063 RID: 12387
			internal int ftTimeLow;

			// Token: 0x04003064 RID: 12388
			internal int ftTimeHigh;
		}
	}
}
