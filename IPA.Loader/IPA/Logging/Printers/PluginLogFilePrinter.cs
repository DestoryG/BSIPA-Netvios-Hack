using System;
using System.IO;
using IPA.Utilities;

namespace IPA.Logging.Printers
{
	/// <summary>
	/// Prints log messages to the file specified by the name.
	/// </summary>
	// Token: 0x0200003C RID: 60
	public class PluginLogFilePrinter : GZFilePrinter
	{
		/// <summary>
		/// Provides a filter for this specific printer.
		/// </summary>
		/// <value>the filter level for this printer</value>
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000063A8 File Offset: 0x000045A8
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000063B0 File Offset: 0x000045B0
		public override Logger.LogLevel Filter { get; set; } = Logger.LogLevel.All;

		/// <summary>
		/// Gets the <see cref="T:System.IO.FileInfo" /> for the target file.
		/// </summary>
		/// <returns>the file to write to</returns>
		// Token: 0x0600017B RID: 379 RVA: 0x000063B9 File Offset: 0x000045B9
		protected override FileInfo GetFileInfo()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine("Logs", this.name));
			directoryInfo.Create();
			return new FileInfo(Path.Combine(directoryInfo.FullName, string.Format("{0:yyyy.MM.dd.HH.mm.ss}.log", Utils.CurrentTime())));
		}

		/// <summary>
		/// Creates a new printer with the given name.
		/// </summary>
		/// <param name="name">the name of the logger</param>
		// Token: 0x0600017C RID: 380 RVA: 0x000063F9 File Offset: 0x000045F9
		public PluginLogFilePrinter(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Prints an entry to the associated file.
		/// </summary>
		/// <param name="level">the <see cref="T:IPA.Logging.Logger.Level" /> of the message</param>
		/// <param name="time">the <see cref="T:System.DateTime" /> the message was recorded at</param>
		/// <param name="logName">the name of the log that sent the message</param>
		/// <param name="message">the message to print</param>
		// Token: 0x0600017D RID: 381 RVA: 0x00006410 File Offset: 0x00004610
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

		// Token: 0x0400008C RID: 140
		private string name;
	}
}
