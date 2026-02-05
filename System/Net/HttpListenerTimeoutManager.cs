using System;
using System.Net.Configuration;

namespace System.Net
{
	// Token: 0x02000101 RID: 257
	public class HttpListenerTimeoutManager
	{
		// Token: 0x0600099B RID: 2459 RVA: 0x00035C62 File Offset: 0x00033E62
		internal HttpListenerTimeoutManager(HttpListener context)
		{
			this.listener = context;
			this.timeouts = new int[5];
			this.LoadConfigurationSettings();
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00035C84 File Offset: 0x00033E84
		private void LoadConfigurationSettings()
		{
			long[] httpListenerTimeouts = SettingsSectionInternal.Section.HttpListenerTimeouts;
			bool flag = false;
			for (int i = 0; i < this.timeouts.Length; i++)
			{
				if (httpListenerTimeouts[i] != 0L)
				{
					this.timeouts[i] = (int)httpListenerTimeouts[i];
					flag = true;
				}
			}
			if (httpListenerTimeouts[5] != 0L)
			{
				this.minSendBytesPerSecond = (uint)httpListenerTimeouts[5];
				flag = true;
			}
			if (flag)
			{
				this.listener.SetServerTimeout(this.timeouts, this.minSendBytesPerSecond);
			}
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00035CEF File Offset: 0x00033EEF
		private TimeSpan GetTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE type)
		{
			return new TimeSpan(0, 0, this.timeouts[(int)type]);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00035D00 File Offset: 0x00033F00
		private void SetTimespanTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE type, TimeSpan value)
		{
			long num = Convert.ToInt64(value.TotalSeconds);
			if (num < 0L || num > 65535L)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			int[] array = this.timeouts;
			array[(int)type] = (int)num;
			this.listener.SetServerTimeout(array, this.minSendBytesPerSecond);
			this.timeouts[(int)type] = (int)num;
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x00035D5B File Offset: 0x00033F5B
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x00035D64 File Offset: 0x00033F64
		public TimeSpan EntityBody
		{
			get
			{
				return this.GetTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.EntityBody);
			}
			set
			{
				this.SetTimespanTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.EntityBody, value);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x00035D6E File Offset: 0x00033F6E
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x00035D77 File Offset: 0x00033F77
		public TimeSpan DrainEntityBody
		{
			get
			{
				return this.GetTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.DrainEntityBody);
			}
			set
			{
				this.SetTimespanTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.DrainEntityBody, value);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x00035D81 File Offset: 0x00033F81
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x00035D8A File Offset: 0x00033F8A
		public TimeSpan RequestQueue
		{
			get
			{
				return this.GetTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.RequestQueue);
			}
			set
			{
				this.SetTimespanTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.RequestQueue, value);
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00035D94 File Offset: 0x00033F94
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x00035D9D File Offset: 0x00033F9D
		public TimeSpan IdleConnection
		{
			get
			{
				return this.GetTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.IdleConnection);
			}
			set
			{
				this.SetTimespanTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.IdleConnection, value);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x00035DA7 File Offset: 0x00033FA7
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x00035DB0 File Offset: 0x00033FB0
		public TimeSpan HeaderWait
		{
			get
			{
				return this.GetTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.HeaderWait);
			}
			set
			{
				this.SetTimespanTimeout(UnsafeNclNativeMethods.HttpApi.HTTP_TIMEOUT_TYPE.HeaderWait, value);
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00035DBA File Offset: 0x00033FBA
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x00035DC3 File Offset: 0x00033FC3
		public long MinSendBytesPerSecond
		{
			get
			{
				return (long)((ulong)this.minSendBytesPerSecond);
			}
			set
			{
				if (value < 0L || value > (long)((ulong)(-1)))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.listener.SetServerTimeout(this.timeouts, (uint)value);
				this.minSendBytesPerSecond = (uint)value;
			}
		}

		// Token: 0x04000E4A RID: 3658
		private HttpListener listener;

		// Token: 0x04000E4B RID: 3659
		private int[] timeouts;

		// Token: 0x04000E4C RID: 3660
		private uint minSendBytesPerSecond;
	}
}
