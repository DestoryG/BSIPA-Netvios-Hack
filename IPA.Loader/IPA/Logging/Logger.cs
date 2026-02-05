using System;

namespace IPA.Logging
{
	/// <summary>
	/// The logger base class. Provides the format for console logs.
	/// </summary>
	// Token: 0x02000031 RID: 49
	public abstract class Logger
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000050D0 File Offset: 0x000032D0
		internal static Logger log
		{
			get
			{
				if (Logger._log == null)
				{
					Logger._log = new StandardLogger("IPA");
				}
				return Logger._log;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000050ED File Offset: 0x000032ED
		internal static StandardLogger stdout
		{
			get
			{
				if (Logger._stdout == null)
				{
					Logger._stdout = new StandardLogger("_");
				}
				return Logger._stdout;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000510A File Offset: 0x0000330A
		internal static Logger updater
		{
			get
			{
				return Logger.log.GetChildLogger("Updater");
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000511B File Offset: 0x0000331B
		internal static Logger libLoader
		{
			get
			{
				return Logger.log.GetChildLogger("LibraryLoader");
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000512C File Offset: 0x0000332C
		internal static Logger injector
		{
			get
			{
				return Logger.log.GetChildLogger("Injector");
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000513D File Offset: 0x0000333D
		internal static Logger loader
		{
			get
			{
				return Logger.log.GetChildLogger("Loader");
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000514E File Offset: 0x0000334E
		internal static Logger features
		{
			get
			{
				return Logger.loader.GetChildLogger("Features");
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000515F File Offset: 0x0000335F
		internal static Logger config
		{
			get
			{
				return Logger.log.GetChildLogger("Config");
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00005170 File Offset: 0x00003370
		internal static bool LogCreated
		{
			get
			{
				return Logger._log != null;
			}
		}

		/// <summary>
		/// The standard format for log messages.
		/// </summary>
		/// <value>the format for the standard loggers to print in</value>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000517A File Offset: 0x0000337A
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00005181 File Offset: 0x00003381
		public static string LogFormat { get; protected internal set; } = "[{3} @ {2:HH:mm:ss} | {1}] {0}";

		/// <summary>
		/// A basic log function.
		/// </summary>
		/// <param name="level">the level of the message</param>
		/// <param name="message">the message to log</param>
		// Token: 0x06000126 RID: 294
		public abstract void Log(Logger.Level level, string message);

		/// <summary>
		/// A basic log function taking an exception to log.
		/// </summary>
		/// <param name="level">the level of the message</param>
		/// <param name="e">the exception to log</param>
		// Token: 0x06000127 RID: 295 RVA: 0x00005189 File Offset: 0x00003389
		public virtual void Log(Logger.Level level, Exception e)
		{
			this.Log(level, e.ToString());
		}

		/// <summary>
		/// Sends a trace message.
		/// Equivalent to <c>Log(Level.Trace, message);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.String)" />
		/// <param name="message">the message to log</param>
		// Token: 0x06000128 RID: 296 RVA: 0x00005198 File Offset: 0x00003398
		public virtual void Trace(string message)
		{
			this.Log(Logger.Level.Trace, message);
		}

		/// <summary>
		/// Sends an exception as a trace message.
		/// Equivalent to <c>Log(Level.Trace, e);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.Exception)" />
		/// <param name="e">the exception to log</param>
		// Token: 0x06000129 RID: 297 RVA: 0x000051A3 File Offset: 0x000033A3
		public virtual void Trace(Exception e)
		{
			this.Log(Logger.Level.Trace, e);
		}

		/// <summary>
		/// Sends a debug message.
		/// Equivalent to <c>Log(Level.Debug, message);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.String)" />
		/// <param name="message">the message to log</param>
		// Token: 0x0600012A RID: 298 RVA: 0x000051AE File Offset: 0x000033AE
		public virtual void Debug(string message)
		{
			this.Log(Logger.Level.Debug, message);
		}

		/// <summary>
		/// Sends an exception as a debug message.
		/// Equivalent to <c>Log(Level.Debug, e);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.Exception)" />
		/// <param name="e">the exception to log</param>
		// Token: 0x0600012B RID: 299 RVA: 0x000051B8 File Offset: 0x000033B8
		public virtual void Debug(Exception e)
		{
			this.Log(Logger.Level.Debug, e);
		}

		/// <summary>
		/// Sends an info message.
		/// Equivalent to <c>Log(Level.Info, message);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.String)" />
		/// <param name="message">the message to log</param>
		// Token: 0x0600012C RID: 300 RVA: 0x000051C2 File Offset: 0x000033C2
		public virtual void Info(string message)
		{
			this.Log(Logger.Level.Info, message);
		}

		/// <summary>
		/// Sends an exception as an info message.
		/// Equivalent to <c>Log(Level.Info, e);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.Exception)" />
		/// <param name="e">the exception to log</param>
		// Token: 0x0600012D RID: 301 RVA: 0x000051CC File Offset: 0x000033CC
		public virtual void Info(Exception e)
		{
			this.Log(Logger.Level.Info, e);
		}

		/// <summary>
		/// Sends a notice message.
		/// Equivalent to <c>Log(Level.Notice, message);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.String)" />
		/// <param name="message">the message to log</param>
		// Token: 0x0600012E RID: 302 RVA: 0x000051D6 File Offset: 0x000033D6
		public virtual void Notice(string message)
		{
			this.Log(Logger.Level.Notice, message);
		}

		/// <summary>
		/// Sends an exception as a notice message.
		/// Equivalent to <c>Log(Level.Notice, e);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.Exception)" />
		/// <param name="e">the exception to log</param>
		// Token: 0x0600012F RID: 303 RVA: 0x000051E1 File Offset: 0x000033E1
		public virtual void Notice(Exception e)
		{
			this.Log(Logger.Level.Notice, e);
		}

		/// <summary>
		/// Sends a warning message.
		/// Equivalent to <c>Log(Level.Warning, message);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.String)" />
		/// <param name="message">the message to log</param>
		// Token: 0x06000130 RID: 304 RVA: 0x000051EC File Offset: 0x000033EC
		public virtual void Warn(string message)
		{
			this.Log(Logger.Level.Warning, message);
		}

		/// <summary>
		/// Sends an exception as a warning message.
		/// Equivalent to <c>Log(Level.Warning, e);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.Exception)" />
		/// <param name="e">the exception to log</param>
		// Token: 0x06000131 RID: 305 RVA: 0x000051F6 File Offset: 0x000033F6
		public virtual void Warn(Exception e)
		{
			this.Log(Logger.Level.Warning, e);
		}

		/// <summary>
		/// Sends an error message.
		/// Equivalent to <c>Log(Level.Error, message);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.String)" />
		/// <param name="message">the message to log</param>
		// Token: 0x06000132 RID: 306 RVA: 0x00005200 File Offset: 0x00003400
		public virtual void Error(string message)
		{
			this.Log(Logger.Level.Error, message);
		}

		/// <summary>
		/// Sends an exception as an error message.
		/// Equivalent to <c>Log(Level.Error, e);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.Exception)" />
		/// <param name="e">the exception to log</param>
		// Token: 0x06000133 RID: 307 RVA: 0x0000520A File Offset: 0x0000340A
		public virtual void Error(Exception e)
		{
			this.Log(Logger.Level.Error, e);
		}

		/// <summary>
		/// Sends a critical message.
		/// Equivalent to <c>Log(Level.Critical, message);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.String)" />
		/// <param name="message">the message to log</param>
		// Token: 0x06000134 RID: 308 RVA: 0x00005214 File Offset: 0x00003414
		public virtual void Critical(string message)
		{
			this.Log(Logger.Level.Critical, message);
		}

		/// <summary>
		/// Sends an exception as a critical message.
		/// Equivalent to <c>Log(Level.Critical, e);</c>
		/// </summary>
		/// <seealso cref="M:IPA.Logging.Logger.Log(IPA.Logging.Logger.Level,System.Exception)" />
		/// <param name="e">the exception to log</param>
		// Token: 0x06000135 RID: 309 RVA: 0x0000521F File Offset: 0x0000341F
		public virtual void Critical(Exception e)
		{
			this.Log(Logger.Level.Critical, e);
		}

		// Token: 0x04000064 RID: 100
		private static Logger _log;

		// Token: 0x04000065 RID: 101
		private static StandardLogger _stdout;

		/// <summary>
		/// An enum specifying the level of the message. Resembles Syslog.
		/// </summary>
		// Token: 0x020000E2 RID: 226
		public enum Level : byte
		{
			/// <summary>
			/// No associated level. These never get shown.
			/// </summary>
			// Token: 0x040002EF RID: 751
			None,
			/// <summary>
			/// A trace message. These are ignored <i>incredibly</i> early.
			/// </summary>
			// Token: 0x040002F0 RID: 752
			Trace = 64,
			/// <summary>
			/// A debug message.
			/// </summary>
			// Token: 0x040002F1 RID: 753
			Debug = 1,
			/// <summary>
			/// An informational message.
			/// </summary>
			// Token: 0x040002F2 RID: 754
			Info,
			/// <summary>
			/// A notice. More significant than Info, but less than a warning.
			/// </summary>
			// Token: 0x040002F3 RID: 755
			Notice = 32,
			/// <summary>
			/// A warning message.
			/// </summary>
			// Token: 0x040002F4 RID: 756
			Warning = 4,
			/// <summary>
			/// An error message.
			/// </summary>
			// Token: 0x040002F5 RID: 757
			Error = 8,
			/// <summary>
			/// A critical error message.
			/// </summary>
			// Token: 0x040002F6 RID: 758
			Critical = 16
		}

		/// <summary>
		/// An enum providing log level filters.
		/// </summary>
		// Token: 0x020000E3 RID: 227
		[Flags]
		public enum LogLevel : byte
		{
			/// <summary>
			/// Allow no messages through.
			/// </summary>
			// Token: 0x040002F8 RID: 760
			None = 0,
			/// <summary>
			/// Only shows Trace messages.
			/// </summary>
			// Token: 0x040002F9 RID: 761
			TraceOnly = 64,
			/// <summary>
			/// Only shows Debug messages.
			/// </summary>
			// Token: 0x040002FA RID: 762
			DebugOnly = 1,
			/// <summary>
			/// Only shows info messages.
			/// </summary>
			// Token: 0x040002FB RID: 763
			InfoOnly = 2,
			/// <summary>
			/// Only shows notice messages.
			/// </summary>
			// Token: 0x040002FC RID: 764
			NoticeOnly = 32,
			/// <summary>
			/// Only shows Warning messages.
			/// </summary>
			// Token: 0x040002FD RID: 765
			WarningOnly = 4,
			/// <summary>
			/// Only shows Error messages.
			/// </summary>
			// Token: 0x040002FE RID: 766
			ErrorOnly = 8,
			/// <summary>
			/// Only shows Critical messages.
			/// </summary>
			// Token: 0x040002FF RID: 767
			CriticalOnly = 16,
			/// <summary>
			/// Shows all messages error and up.
			/// </summary>
			// Token: 0x04000300 RID: 768
			ErrorUp = 24,
			/// <summary>
			/// Shows all messages warning and up.
			/// </summary>
			// Token: 0x04000301 RID: 769
			WarningUp = 28,
			/// <summary>
			/// Shows all messages Notice and up.
			/// </summary>
			// Token: 0x04000302 RID: 770
			NoticeUp = 60,
			/// <summary>
			/// Shows all messages info and up.
			/// </summary>
			// Token: 0x04000303 RID: 771
			InfoUp = 62,
			/// <summary>
			/// Shows all messages debug and up.
			/// </summary>
			// Token: 0x04000304 RID: 772
			DebugUp = 63,
			/// <summary>
			/// Shows all messages.
			/// </summary>
			// Token: 0x04000305 RID: 773
			All = 127,
			/// <summary>
			/// Used for when the level is undefined.
			/// </summary>
			// Token: 0x04000306 RID: 774
			Undefined = 255
		}
	}
}
