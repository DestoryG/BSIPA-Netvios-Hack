using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using IPA.Config;
using IPA.Logging.Printers;
using IPA.Utilities;

namespace IPA.Logging
{
	/// <summary>
	/// The default (and standard) <see cref="T:IPA.Logging.Logger" /> implementation.
	/// </summary>
	/// <remarks>
	/// <see cref="T:IPA.Logging.StandardLogger" /> uses a multi-threaded approach to logging. All actual I/O is done on another thread,
	/// where all messaged are guaranteed to be logged in the order they appeared. It is up to the printers to format them.
	///
	/// This logger supports child loggers. Use <see cref="M:IPA.Logging.LoggerExtensions.GetChildLogger(IPA.Logging.Logger,System.String)" /> to safely get a child.
	/// The modification of printers on a parent are reflected down the chain.
	/// </remarks>
	// Token: 0x02000033 RID: 51
	public class StandardLogger : Logger
	{
		// Token: 0x06000140 RID: 320 RVA: 0x0000525C File Offset: 0x0000345C
		static StandardLogger()
		{
			StandardLogger.ConsoleColorSupport();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000052C4 File Offset: 0x000034C4
		internal static void ConsoleColorSupport()
		{
			if (!StandardLogger.addedConsolePrinters && !StandardLogger.finalizedDefaultPrinters && WinConsole.IsInitialized)
			{
				StandardLogger.defaultPrinters.AddRange(new ColoredConsolePrinter[]
				{
					new ColoredConsolePrinter
					{
						Filter = Logger.LogLevel.TraceOnly,
						Color = ConsoleColor.DarkMagenta
					},
					new ColoredConsolePrinter
					{
						Filter = Logger.LogLevel.DebugOnly,
						Color = ConsoleColor.Green
					},
					new ColoredConsolePrinter
					{
						Filter = Logger.LogLevel.InfoOnly,
						Color = ConsoleColor.White
					},
					new ColoredConsolePrinter
					{
						Filter = Logger.LogLevel.NoticeOnly,
						Color = ConsoleColor.Cyan
					},
					new ColoredConsolePrinter
					{
						Filter = Logger.LogLevel.WarningOnly,
						Color = ConsoleColor.Yellow
					},
					new ColoredConsolePrinter
					{
						Filter = Logger.LogLevel.ErrorOnly,
						Color = ConsoleColor.Red
					},
					new ColoredConsolePrinter
					{
						Filter = Logger.LogLevel.CriticalOnly,
						Color = ConsoleColor.Magenta
					}
				});
				StandardLogger.addedConsolePrinters = true;
			}
		}

		/// <summary>
		/// The <see cref="T:System.IO.TextWriter" /> for writing directly to the console window, or stdout if no window open.
		/// </summary>
		/// <value>a <see cref="T:System.IO.TextWriter" /> for the current primary text output</value>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000053A8 File Offset: 0x000035A8
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000053AF File Offset: 0x000035AF
		public static TextWriter ConsoleWriter { get; internal set; } = Console.Out;

		/// <summary>
		/// Adds to the default printer pool that all printers inherit from. Printers added this way will be passed every message from every logger.
		/// </summary>
		/// <param name="printer">the printer to add</param>
		// Token: 0x06000144 RID: 324 RVA: 0x000053B7 File Offset: 0x000035B7
		internal static void AddDefaultPrinter(LogPrinter printer)
		{
			StandardLogger.defaultPrinters.Add(printer);
		}

		/// <summary>
		/// All levels defined by this filter will be sent to loggers. All others will be ignored.
		/// </summary>
		/// <value>the global filter level</value>
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000053C4 File Offset: 0x000035C4
		// (set) Token: 0x06000146 RID: 326 RVA: 0x000053CB File Offset: 0x000035CB
		public static Logger.LogLevel PrintFilter { get; internal set; } = Logger.LogLevel.All;

		/// <summary>
		/// Configures internal debug settings based on the config passed in.
		/// </summary>
		// Token: 0x06000147 RID: 327 RVA: 0x000053D3 File Offset: 0x000035D3
		internal static void Configure()
		{
			StandardLogger.showSourceClass = SelfConfig.Debug_.ShowCallSource_;
			StandardLogger.PrintFilter = (SelfConfig.Debug_.ShowDebug_ ? Logger.LogLevel.All : Logger.LogLevel.InfoUp);
			StandardLogger.showTrace = SelfConfig.Debug_.ShowTrace_;
			StandardLogger.syncLogging = SelfConfig.Debug_.SyncLogging_;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005408 File Offset: 0x00003608
		private StandardLogger(StandardLogger parent, string subName)
		{
			this.logName = parent.logName + "/" + subName;
			this.parent = parent;
			this.printers = new List<LogPrinter>();
			if (!SelfConfig.Debug_.CondenseModLogs_)
			{
				this.printers.Add(new PluginSubLogPrinter(parent.logName, subName));
			}
			if (StandardLogger.logThread == null || !StandardLogger.logThread.IsAlive)
			{
				StandardLogger.logThread = new Thread(new ThreadStart(StandardLogger.LogThread));
				StandardLogger.logThread.Start();
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000054AC File Offset: 0x000036AC
		internal StandardLogger(string name)
		{
			StandardLogger.ConsoleColorSupport();
			if (!StandardLogger.finalizedDefaultPrinters)
			{
				if (!StandardLogger.addedConsolePrinters)
				{
					StandardLogger.AddDefaultPrinter(new ColorlessConsolePrinter());
				}
				StandardLogger.finalizedDefaultPrinters = true;
			}
			this.logName = name;
			this.printers.Add(new PluginLogFilePrinter(name));
			if (StandardLogger.logThread == null || !StandardLogger.logThread.IsAlive)
			{
				StandardLogger.logThread = new Thread(new ThreadStart(StandardLogger.LogThread));
				StandardLogger.logThread.Start();
			}
		}

		/// <summary>
		/// Gets a child printer with the given name, either constructing a new one or using one that was already made.
		/// </summary>
		/// <param name="name"></param>
		/// <returns>a child <see cref="T:IPA.Logging.StandardLogger" /> with the given sub-name</returns>
		// Token: 0x0600014A RID: 330 RVA: 0x00005544 File Offset: 0x00003744
		internal StandardLogger GetChild(string name)
		{
			StandardLogger child;
			if (!this.children.TryGetValue(name, out child))
			{
				child = new StandardLogger(this, name);
				this.children.Add(name, child);
			}
			return child;
		}

		/// <summary>
		/// Adds a log printer to the logger.
		/// </summary>
		/// <param name="printer">the printer to add</param>
		// Token: 0x0600014B RID: 331 RVA: 0x00005577 File Offset: 0x00003777
		public void AddPrinter(LogPrinter printer)
		{
			this.printers.Add(printer);
		}

		/// <summary>
		/// Logs a specific message at a given level.
		/// </summary>
		/// <param name="level">the message level</param>
		/// <param name="message">the message to log</param>
		// Token: 0x0600014C RID: 332 RVA: 0x00005588 File Offset: 0x00003788
		public override void Log(Logger.Level level, string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			if (!StandardLogger.showTrace && level == Logger.Level.Trace)
			{
				return;
			}
			StandardLogger.logWaitEvent.Wait();
			try
			{
				bool flag = StandardLogger.syncLogging && !StandardLogger.IsOnLoggerThread;
				if (flag)
				{
					if (StandardLogger.threadSync == null)
					{
						StandardLogger.threadSync = new ManualResetEventSlim();
					}
					StandardLogger.threadSync.Reset();
				}
				StandardLogger.logQueue.Add(new StandardLogger.LogMessage
				{
					Level = level,
					Message = message,
					Logger = this,
					Time = Utils.CurrentTime(),
					Sync = StandardLogger.threadSync
				});
				if (flag)
				{
					StandardLogger.threadSync.Wait();
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		/// <inheritdoc />
		/// <summary>
		/// An override to <see cref="M:IPA.Logging.Logger.Debug(System.String)" /> which shows the method that called it.
		/// </summary>
		/// <param name="message">the message to log</param>
		// Token: 0x0600014D RID: 333 RVA: 0x00005650 File Offset: 0x00003850
		public override void Debug(string message)
		{
			if (StandardLogger.showSourceClass)
			{
				StackFrame stackFrame = new StackTrace(true).GetFrame(1);
				int lineNo = stackFrame.GetFileLineNumber();
				if (lineNo == 0)
				{
					MethodBase method = stackFrame.GetMethod();
					string paramString = string.Join(", ", (from p in method.GetParameters()
						select p.ParameterType.FullName).StrJP());
					string[] array = new string[8];
					array[0] = "{";
					int num = 1;
					Type declaringType = method.DeclaringType;
					array[num] = ((declaringType != null) ? declaringType.FullName : null);
					array[2] = "::";
					array[3] = method.Name;
					array[4] = "(";
					array[5] = paramString;
					array[6] = ")} ";
					array[7] = message;
					message = string.Concat(array);
				}
				else
				{
					message = string.Format("{{{0}:{1}}} {2}", stackFrame.GetFileName(), lineNo, message);
				}
			}
			base.Debug(message);
		}

		/// <summary>
		/// Whether or not the calling thread is the logger thread.
		/// </summary>
		/// <value><see langword="true" /> if the current thread is the logger thread, <see langword="false" /> otherwise</value>
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00005738 File Offset: 0x00003938
		public static bool IsOnLoggerThread
		{
			get
			{
				bool flag = StandardLogger.isOnLoggerThread.GetValueOrDefault();
				if (StandardLogger.isOnLoggerThread == null)
				{
					flag = Thread.CurrentThread.ManagedThreadId == StandardLogger.logThread.ManagedThreadId;
					StandardLogger.isOnLoggerThread = new bool?(flag);
					return flag;
				}
				return flag;
			}
		}

		/// <summary>
		/// The log printer thread for <see cref="T:IPA.Logging.StandardLogger" />.
		/// </summary>
		// Token: 0x0600014F RID: 335 RVA: 0x00005784 File Offset: 0x00003984
		private static void LogThread()
		{
			AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs args)
			{
				StandardLogger.StopLogThread();
			};
			StandardLogger.loggerLogger = new StandardLogger("Log Subsystem");
			StandardLogger.loggerLogger.printers.Clear();
			TimeSpan timeout = TimeSpan.FromMilliseconds(250.0);
			try
			{
				HashSet<LogPrinter> started = new HashSet<LogPrinter>();
				StandardLogger.LogMessage msg;
				while (StandardLogger.logQueue.TryTake(out msg, -1))
				{
					StdoutInterceptor.Intercept();
					do
					{
						StandardLogger logger = msg.Logger;
						IEnumerable<LogPrinter> printers = logger.printers;
						do
						{
							logger = logger.parent;
							if (logger != null)
							{
								printers = printers.Concat(logger.printers);
							}
						}
						while (logger != null);
						foreach (LogPrinter printer in printers.Concat(StandardLogger.defaultPrinters))
						{
							try
							{
								if ((msg.Level & (Logger.Level)printer.Filter) != Logger.Level.None)
								{
									if (!started.Contains(printer))
									{
										printer.StartPrint();
										started.Add(printer);
									}
									printer.LastUse = Utils.CurrentTime();
									printer.Print(msg.Level, msg.Time, msg.Logger.logName, msg.Message);
								}
							}
							catch (Exception e)
							{
								Console.WriteLine(string.Format("printer errored: {0}", e));
							}
						}
						ManualResetEventSlim sync = msg.Sync;
						if (sync != null)
						{
							sync.Set();
						}
						SelfConfig instance = SelfConfig.Instance;
						SelfConfig.Debug_ debugConfig = ((instance != null) ? instance.Debug : null);
						if (debugConfig != null && debugConfig.HideMessagesForPerformance && StandardLogger.logQueue.Count > debugConfig.HideLogThreshold)
						{
							StandardLogger.logWaitEvent.Reset();
							StandardLogger.loggerLogger.printers.Clear();
							HashSet<LogPrinter> prints = new HashSet<LogPrinter>();
							StandardLogger.LogMessage message;
							while (StandardLogger.logQueue.TryTake(out message))
							{
								StandardLogger messageLogger = message.Logger;
								foreach (LogPrinter print in messageLogger.printers)
								{
									prints.Add(print);
								}
								do
								{
									messageLogger = messageLogger.parent;
									if (messageLogger != null)
									{
										foreach (LogPrinter print2 in messageLogger.printers)
										{
											prints.Add(print2);
										}
									}
								}
								while (messageLogger != null);
								ManualResetEventSlim sync2 = message.Sync;
								if (sync2 != null)
								{
									sync2.Set();
								}
							}
							StandardLogger.loggerLogger.printers.AddRange(prints);
							StandardLogger.logQueue.Add(new StandardLogger.LogMessage
							{
								Level = Logger.Level.Warning,
								Logger = StandardLogger.loggerLogger,
								Message = StandardLogger.loggerLogger.logName.ToUpper() + ": Messages omitted to improve performance",
								Time = Utils.CurrentTime()
							});
							StandardLogger.logWaitEvent.Set();
						}
						DateTime now = Utils.CurrentTime();
						foreach (LogPrinter printer2 in new List<LogPrinter>(started))
						{
							if (now - printer2.LastUse > timeout)
							{
								try
								{
									printer2.EndPrint();
								}
								catch (Exception e2)
								{
									Console.WriteLine(string.Format("printer errored: {0}", e2));
								}
								started.Remove(printer2);
							}
						}
					}
					while (StandardLogger.logQueue.TryTake(out msg, timeout));
					if (StandardLogger.logQueue.Count == 0)
					{
						foreach (LogPrinter printer3 in started)
						{
							try
							{
								printer3.EndPrint();
							}
							catch (Exception e3)
							{
								Console.WriteLine(string.Format("printer errored: {0}", e3));
							}
						}
						started.Clear();
					}
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		/// <summary>
		/// Stops and joins the log printer thread.
		/// </summary>
		// Token: 0x06000150 RID: 336 RVA: 0x00005C40 File Offset: 0x00003E40
		internal static void StopLogThread()
		{
			StandardLogger.logQueue.CompleteAdding();
			StandardLogger.logThread.Join();
		}

		// Token: 0x04000068 RID: 104
		private static readonly List<LogPrinter> defaultPrinters = new List<LogPrinter>
		{
			new GlobalLogFilePrinter()
		};

		// Token: 0x04000069 RID: 105
		private static bool addedConsolePrinters;

		// Token: 0x0400006A RID: 106
		private static bool finalizedDefaultPrinters;

		// Token: 0x0400006C RID: 108
		private readonly string logName;

		// Token: 0x0400006D RID: 109
		private static bool showSourceClass;

		// Token: 0x0400006F RID: 111
		private static bool showTrace = false;

		// Token: 0x04000070 RID: 112
		private static volatile bool syncLogging = false;

		// Token: 0x04000071 RID: 113
		private readonly List<LogPrinter> printers = new List<LogPrinter>();

		// Token: 0x04000072 RID: 114
		private readonly StandardLogger parent;

		// Token: 0x04000073 RID: 115
		private readonly Dictionary<string, StandardLogger> children = new Dictionary<string, StandardLogger>();

		// Token: 0x04000074 RID: 116
		[ThreadStatic]
		private static ManualResetEventSlim threadSync;

		// Token: 0x04000075 RID: 117
		[ThreadStatic]
		private static bool? isOnLoggerThread = null;

		// Token: 0x04000076 RID: 118
		private static readonly ManualResetEventSlim logWaitEvent = new ManualResetEventSlim(true);

		// Token: 0x04000077 RID: 119
		private static readonly BlockingCollection<StandardLogger.LogMessage> logQueue = new BlockingCollection<StandardLogger.LogMessage>();

		// Token: 0x04000078 RID: 120
		private static Thread logThread;

		// Token: 0x04000079 RID: 121
		private static StandardLogger loggerLogger;

		// Token: 0x0400007A RID: 122
		private const int LogCloseTimeout = 250;

		// Token: 0x020000E4 RID: 228
		private struct LogMessage
		{
			// Token: 0x04000307 RID: 775
			public Logger.Level Level;

			// Token: 0x04000308 RID: 776
			public StandardLogger Logger;

			// Token: 0x04000309 RID: 777
			public string Message;

			// Token: 0x0400030A RID: 778
			public DateTime Time;

			// Token: 0x0400030B RID: 779
			public ManualResetEventSlim Sync;
		}
	}
}
