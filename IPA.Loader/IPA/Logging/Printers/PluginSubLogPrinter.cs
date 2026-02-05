using System;
using System.IO;
using IPA.Utilities;

namespace IPA.Logging.Printers
{
	/// <summary>
	/// Prints log messages to the file specified by the name.
	/// </summary>
	// Token: 0x02000038 RID: 56
	public class PluginSubLogPrinter : GZFilePrinter
	{
		/// <summary>
		/// Provides a filter for this specific printer.
		/// </summary>
		/// <value>the filter for this printer</value>
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00005D5A File Offset: 0x00003F5A
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00005D62 File Offset: 0x00003F62
		public override Logger.LogLevel Filter { get; set; } = Logger.LogLevel.All;

		/// <summary>
		/// Gets the <see cref="T:System.IO.FileInfo" /> for the target file.
		/// </summary>
		/// <returns>the file to write to</returns>
		// Token: 0x0600015A RID: 346 RVA: 0x00005D6C File Offset: 0x00003F6C
		protected override FileInfo GetFileInfo()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine("Logs", this.mainName, this.name));
			directoryInfo.Create();
			return new FileInfo(Path.Combine(directoryInfo.FullName, string.Format("{0:yyyy.MM.dd.HH.mm.ss}.log", Utils.CurrentTime())));
		}

		/// <summary>
		/// Creates a new printer with the given name.
		/// </summary>
		/// <param name="mainname">the name of the main logger</param>
		/// <param name="name">the name of the logger</param>
		// Token: 0x0600015B RID: 347 RVA: 0x00005DBD File Offset: 0x00003FBD
		public PluginSubLogPrinter(string mainname, string name)
		{
			this.name = name;
			this.mainName = mainname;
		}

		/// <summary>
		/// Prints an entry to the associated file.
		/// </summary>
		/// <param name="level">the <see cref="T:IPA.Logging.Logger.Level" /> of the message</param>
		/// <param name="time">the <see cref="T:System.DateTime" /> the message was recorded at</param>
		/// <param name="logName">the name of the log that sent the message</param>
		/// <param name="message">the message to print</param>
		// Token: 0x0600015C RID: 348 RVA: 0x00005DDC File Offset: 0x00003FDC
		public override void Print(Logger.Level level, DateTime time, string logName, string message)
		{
			foreach (string line in GZFilePrinter.removeControlCodes.Replace(message, "").Split(new string[]
			{
				"\n",
				Environment.NewLine
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				this.FileWriter.WriteLine("[{2} @ {1:HH:mm:ss}] {0}", line, time, level.ToString().ToUpper());
			}
		}

		// Token: 0x0400007E RID: 126
		private string name;

		// Token: 0x0400007F RID: 127
		private string mainName;
	}
}
