using System;

namespace IPA.Logging
{
	/// <summary>
	/// The log printer's base class.
	/// </summary>
	// Token: 0x02000032 RID: 50
	public abstract class LogPrinter
	{
		/// <summary>
		/// Provides a filter for which log levels to allow through.
		/// </summary>
		/// <value>the level to filter to</value>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000138 RID: 312
		// (set) Token: 0x06000139 RID: 313
		public abstract Logger.LogLevel Filter { get; set; }

		/// <summary>
		/// Prints a provided message from a given log at the specified time.
		/// </summary>
		/// <param name="level">the log level</param>
		/// <param name="time">the time the message was composed</param>
		/// <param name="logName">the name of the log that created this message</param>
		/// <param name="message">the message</param>
		// Token: 0x0600013A RID: 314
		public abstract void Print(Logger.Level level, DateTime time, string logName, string message);

		/// <summary>
		/// Called before the first print in a group. May be called multiple times.
		/// Use this to create file handles and the like.
		/// </summary>
		// Token: 0x0600013B RID: 315 RVA: 0x0000523E File Offset: 0x0000343E
		public virtual void StartPrint()
		{
		}

		/// <summary>
		/// Called after the last print in a group. May be called multiple times.
		/// Use this to dispose file handles and the like.
		/// </summary>
		// Token: 0x0600013C RID: 316 RVA: 0x00005240 File Offset: 0x00003440
		public virtual void EndPrint()
		{
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00005242 File Offset: 0x00003442
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000524A File Offset: 0x0000344A
		internal DateTime LastUse { get; set; }
	}
}
