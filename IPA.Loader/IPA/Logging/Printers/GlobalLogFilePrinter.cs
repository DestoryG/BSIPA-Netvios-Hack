using System;
using System.IO;
using IPA.Utilities;

namespace IPA.Logging.Printers
{
	/// <summary>
	/// A printer for all messages to a unified log location.
	/// </summary>
	// Token: 0x0200003A RID: 58
	public class GlobalLogFilePrinter : GZFilePrinter
	{
		/// <summary>
		/// Provides a filter for this specific printer.
		/// </summary>
		/// <value>the filter level for this printer</value>
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00005FFF File Offset: 0x000041FF
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00006007 File Offset: 0x00004207
		public override Logger.LogLevel Filter { get; set; } = Logger.LogLevel.WarningUp;

		/// <summary>
		/// Prints an entry to the associated file.
		/// </summary>
		/// <param name="level">the <see cref="T:IPA.Logging.Logger.Level" /> of the message</param>
		/// <param name="time">the <see cref="T:System.DateTime" /> the message was recorded at</param>
		/// <param name="logName">the name of the log that sent the message</param>
		/// <param name="message">the message to print</param>
		// Token: 0x0600016C RID: 364 RVA: 0x00006010 File Offset: 0x00004210
		public override void Print(Logger.Level level, DateTime time, string logName, string message)
		{
			foreach (string line in GZFilePrinter.removeControlCodes.Replace(message, "").Split(new string[]
			{
				"\n",
				Environment.NewLine
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				this.FileWriter.WriteLine(Logger.LogFormat, new object[]
				{
					line,
					logName,
					time,
					level.ToString().ToUpper()
				});
			}
		}

		/// <summary>
		/// Gets the <see cref="T:System.IO.FileInfo" /> for the target file.
		/// </summary>
		/// <returns>the target file to write to</returns>
		// Token: 0x0600016D RID: 365 RVA: 0x00006099 File Offset: 0x00004299
		protected override FileInfo GetFileInfo()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo("Logs");
			directoryInfo.Create();
			return new FileInfo(Path.Combine(directoryInfo.FullName, string.Format("{0:yyyy.MM.dd.HH.mm.ss}.log", Utils.CurrentTime())));
		}
	}
}
