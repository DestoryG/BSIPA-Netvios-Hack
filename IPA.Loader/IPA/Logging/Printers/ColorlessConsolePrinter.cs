using System;

namespace IPA.Logging.Printers
{
	/// <summary>
	/// A colorless version of <see cref="T:IPA.Logging.Printers.ColoredConsolePrinter" />, that indiscriminantly prints to standard out.
	/// </summary>
	// Token: 0x02000037 RID: 55
	public class ColorlessConsolePrinter : LogPrinter
	{
		/// <summary>
		/// A filter for this specific printer.
		/// </summary>
		/// <value>the filter level for this printer</value>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005CC1 File Offset: 0x00003EC1
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00005CC9 File Offset: 0x00003EC9
		public override Logger.LogLevel Filter { get; set; }

		/// <summary>
		/// Prints an entry to standard out.
		/// </summary>
		/// <param name="level">the <see cref="T:IPA.Logging.Logger.Level" /> of the message</param>
		/// <param name="time">the <see cref="T:System.DateTime" /> the message was recorded at</param>
		/// <param name="logName">the name of the log that sent the message</param>
		/// <param name="message">the message to print</param>
		// Token: 0x06000156 RID: 342 RVA: 0x00005CD4 File Offset: 0x00003ED4
		public override void Print(Logger.Level level, DateTime time, string logName, string message)
		{
			if ((level & (Logger.Level)StandardLogger.PrintFilter) == Logger.Level.None)
			{
				return;
			}
			foreach (string line in message.Split(new string[]
			{
				"\n",
				Environment.NewLine
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				Console.WriteLine(Logger.LogFormat, new object[]
				{
					line,
					logName,
					time,
					level.ToString().ToUpper()
				});
			}
		}
	}
}
