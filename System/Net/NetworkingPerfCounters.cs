using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Configuration;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001CD RID: 461
	internal sealed class NetworkingPerfCounters
	{
		// Token: 0x0600122F RID: 4655 RVA: 0x00060F6C File Offset: 0x0005F16C
		private NetworkingPerfCounters()
		{
			this.enabled = SettingsSectionInternal.Section.PerformanceCountersEnabled;
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00060F84 File Offset: 0x0005F184
		public static NetworkingPerfCounters Instance
		{
			get
			{
				if (NetworkingPerfCounters.instance == null)
				{
					object obj = NetworkingPerfCounters.lockObject;
					lock (obj)
					{
						if (NetworkingPerfCounters.instance == null)
						{
							NetworkingPerfCounters.CreateInstance();
						}
					}
				}
				return NetworkingPerfCounters.instance;
			}
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00060FDC File Offset: 0x0005F1DC
		public static long GetTimestamp()
		{
			return Stopwatch.GetTimestamp();
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x00060FE3 File Offset: 0x0005F1E3
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00060FEB File Offset: 0x0005F1EB
		public void Increment(NetworkingPerfCounterName perfCounter)
		{
			this.Increment(perfCounter, 1L);
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00060FF8 File Offset: 0x0005F1F8
		public void Increment(NetworkingPerfCounterName perfCounter, long amount)
		{
			if (this.CounterAvailable())
			{
				try
				{
					NetworkingPerfCounters.CounterPair counterPair = this.counters[(int)perfCounter];
					counterPair.InstanceCounter.IncrementBy(amount);
					counterPair.GlobalCounter.IncrementBy(amount);
				}
				catch (InvalidOperationException ex)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, "NetworkingPerfCounters", "Increment", ex);
					}
				}
				catch (Win32Exception ex2)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, "NetworkingPerfCounters", "Increment", ex2);
					}
				}
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0006108C File Offset: 0x0005F28C
		public void Decrement(NetworkingPerfCounterName perfCounter)
		{
			this.Increment(perfCounter, -1L);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00061097 File Offset: 0x0005F297
		public void Decrement(NetworkingPerfCounterName perfCounter, long amount)
		{
			this.Increment(perfCounter, -amount);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000610A4 File Offset: 0x0005F2A4
		public void IncrementAverage(NetworkingPerfCounterName perfCounter, long startTimestamp)
		{
			if (this.CounterAvailable())
			{
				long timestamp = NetworkingPerfCounters.GetTimestamp();
				long num = (timestamp - startTimestamp) * 1000L / Stopwatch.Frequency;
				this.Increment(perfCounter, num);
				this.Increment(perfCounter + 1, 1L);
			}
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000610E8 File Offset: 0x0005F2E8
		private void Initialize(object state)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_perfcounter_initialization_started"));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PermissionState.Unrestricted);
			performanceCounterPermission.Assert();
			try
			{
				if (!PerformanceCounterCategory.Exists(".NET CLR Networking 4.0.0.0"))
				{
					if (Logging.On)
					{
						Logging.PrintError(Logging.Web, SR.GetString("net_perfcounter_nocategory", new object[] { ".NET CLR Networking 4.0.0.0" }));
					}
				}
				else
				{
					string instanceName = NetworkingPerfCounters.GetInstanceName();
					this.counters = new NetworkingPerfCounters.CounterPair[NetworkingPerfCounters.counterNames.Length];
					for (int i = 0; i < NetworkingPerfCounters.counterNames.Length; i++)
					{
						this.counters[i] = NetworkingPerfCounters.CreateCounterPair(NetworkingPerfCounters.counterNames[i], instanceName);
					}
					AppDomain.CurrentDomain.DomainUnload += this.UnloadEventHandler;
					AppDomain.CurrentDomain.ProcessExit += this.ExitEventHandler;
					AppDomain.CurrentDomain.UnhandledException += this.ExceptionEventHandler;
					this.initSuccessful = true;
				}
			}
			catch (Win32Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, "NetworkingPerfCounters", "Initialize", ex);
				}
				this.Cleanup();
			}
			catch (InvalidOperationException ex2)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, "NetworkingPerfCounters", "Initialize", ex2);
				}
				this.Cleanup();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				this.initDone = true;
				if (Logging.On)
				{
					if (this.initSuccessful)
					{
						Logging.PrintInfo(Logging.Web, SR.GetString("net_perfcounter_initialized_success"));
					}
					else
					{
						Logging.PrintInfo(Logging.Web, SR.GetString("net_perfcounter_initialized_error"));
					}
				}
			}
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x000612C4 File Offset: 0x0005F4C4
		private static void CreateInstance()
		{
			NetworkingPerfCounters.instance = new NetworkingPerfCounters();
			if (NetworkingPerfCounters.instance.Enabled && !ThreadPool.QueueUserWorkItem(new WaitCallback(NetworkingPerfCounters.instance.Initialize)) && Logging.On)
			{
				Logging.PrintError(Logging.Web, SR.GetString("net_perfcounter_cant_queue_workitem"));
			}
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00061320 File Offset: 0x0005F520
		private static NetworkingPerfCounters.CounterPair CreateCounterPair(string counterName, string instanceName)
		{
			PerformanceCounter performanceCounter = new PerformanceCounter(".NET CLR Networking 4.0.0.0", counterName, "_Global_", false);
			return new NetworkingPerfCounters.CounterPair(new PerformanceCounter
			{
				CategoryName = ".NET CLR Networking 4.0.0.0",
				CounterName = counterName,
				InstanceName = instanceName,
				InstanceLifetime = PerformanceCounterInstanceLifetime.Process,
				ReadOnly = false,
				RawValue = 0L
			}, performanceCounter);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0006137B File Offset: 0x0005F57B
		private void ExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
			{
				this.Cleanup();
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0006138B File Offset: 0x0005F58B
		private void UnloadEventHandler(object sender, EventArgs e)
		{
			this.Cleanup();
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00061393 File Offset: 0x0005F593
		private void ExitEventHandler(object sender, EventArgs e)
		{
			this.Cleanup();
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0006139C File Offset: 0x0005F59C
		private void Cleanup()
		{
			object obj = NetworkingPerfCounters.lockObject;
			lock (obj)
			{
				if (!this.cleanupCalled)
				{
					this.cleanupCalled = true;
					if (this.counters != null)
					{
						foreach (NetworkingPerfCounters.CounterPair counterPair in this.counters)
						{
							if (!Environment.HasShutdownStarted && counterPair != null)
							{
								try
								{
									counterPair.InstanceCounter.RemoveInstance();
								}
								catch (InvalidOperationException ex)
								{
									if (Logging.On)
									{
										Logging.Exception(Logging.Web, "NetworkingPerfCounters", "Cleanup", ex);
									}
								}
								catch (Win32Exception ex2)
								{
									if (Logging.On)
									{
										Logging.Exception(Logging.Web, "NetworkingPerfCounters", "Cleanup", ex2);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00061480 File Offset: 0x0005F680
		private static string GetInstanceName()
		{
			string text = NetworkingPerfCounters.ReplaceInvalidChars(AppDomain.CurrentDomain.FriendlyName);
			string text2 = VersioningHelper.MakeVersionSafeName(string.Empty, ResourceScope.Machine, ResourceScope.AppDomain);
			string text3 = text + text2;
			if (text3.Length > 127)
			{
				text3 = text.Substring(0, 127 - text2.Length) + text2;
			}
			return text3;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x000614D4 File Offset: 0x0005F6D4
		private static string ReplaceInvalidChars(string instanceName)
		{
			StringBuilder stringBuilder = new StringBuilder(instanceName);
			int i = 0;
			while (i < stringBuilder.Length)
			{
				char c = stringBuilder[i];
				if (c <= '(')
				{
					if (c == '#')
					{
						goto IL_004B;
					}
					if (c == '(')
					{
						stringBuilder[i] = '[';
					}
				}
				else if (c != ')')
				{
					if (c == '/' || c == '\\')
					{
						goto IL_004B;
					}
				}
				else
				{
					stringBuilder[i] = ']';
				}
				IL_0054:
				i++;
				continue;
				IL_004B:
				stringBuilder[i] = '_';
				goto IL_0054;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00061548 File Offset: 0x0005F748
		private bool CounterAvailable()
		{
			return this.enabled && !this.cleanupCalled && this.initDone && this.initSuccessful;
		}

		// Token: 0x04001490 RID: 5264
		private const int instanceNameMaxLength = 127;

		// Token: 0x04001491 RID: 5265
		private const string categoryName = ".NET CLR Networking 4.0.0.0";

		// Token: 0x04001492 RID: 5266
		private const string globalInstanceName = "_Global_";

		// Token: 0x04001493 RID: 5267
		private static readonly string[] counterNames = new string[]
		{
			"Connections Established", "Bytes Received", "Bytes Sent", "Datagrams Received", "Datagrams Sent", "HttpWebRequests Created/Sec", "HttpWebRequests Average Lifetime", "HttpWebRequests Average Lifetime Base", "HttpWebRequests Queued/Sec", "HttpWebRequests Average Queue Time",
			"HttpWebRequests Average Queue Time Base", "HttpWebRequests Aborted/Sec", "HttpWebRequests Failed/Sec"
		};

		// Token: 0x04001494 RID: 5268
		private static volatile NetworkingPerfCounters instance;

		// Token: 0x04001495 RID: 5269
		private static object lockObject = new object();

		// Token: 0x04001496 RID: 5270
		private volatile bool initDone;

		// Token: 0x04001497 RID: 5271
		private bool initSuccessful;

		// Token: 0x04001498 RID: 5272
		private NetworkingPerfCounters.CounterPair[] counters;

		// Token: 0x04001499 RID: 5273
		private bool enabled;

		// Token: 0x0400149A RID: 5274
		private volatile bool cleanupCalled;

		// Token: 0x02000752 RID: 1874
		private class CounterPair
		{
			// Token: 0x17000F11 RID: 3857
			// (get) Token: 0x060041F4 RID: 16884 RVA: 0x00112054 File Offset: 0x00110254
			public PerformanceCounter InstanceCounter
			{
				get
				{
					return this.instanceCounter;
				}
			}

			// Token: 0x17000F12 RID: 3858
			// (get) Token: 0x060041F5 RID: 16885 RVA: 0x0011205C File Offset: 0x0011025C
			public PerformanceCounter GlobalCounter
			{
				get
				{
					return this.globalCounter;
				}
			}

			// Token: 0x060041F6 RID: 16886 RVA: 0x00112064 File Offset: 0x00110264
			public CounterPair(PerformanceCounter instanceCounter, PerformanceCounter globalCounter)
			{
				this.instanceCounter = instanceCounter;
				this.globalCounter = globalCounter;
			}

			// Token: 0x040031FE RID: 12798
			private PerformanceCounter instanceCounter;

			// Token: 0x040031FF RID: 12799
			private PerformanceCounter globalCounter;
		}
	}
}
