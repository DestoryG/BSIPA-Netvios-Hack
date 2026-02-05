using System;
using System.Runtime.InteropServices;

namespace IPA.Logging.Printers
{
	/// <summary>
	/// Prints a pretty message to the console.
	/// </summary>
	// Token: 0x02000039 RID: 57
	public class ColoredConsolePrinter : LogPrinter
	{
		/// <summary>
		/// A filter for this specific printer.
		/// </summary>
		/// <value>the filter to apply to this printer</value>
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005E52 File Offset: 0x00004052
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00005E5A File Offset: 0x0000405A
		public override Logger.LogLevel Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
			}
		}

		/// <summary>
		/// The color to print messages as.
		/// </summary>
		/// <value>the color to print this message as</value>
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005E63 File Offset: 0x00004063
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00005E6B File Offset: 0x0000406B
		public ConsoleColor Color { get; set; } = ColoredConsolePrinter.GetConsoleColor(WinConsole.OutHandle);

		/// <summary>
		/// Prints an entry to the console window.
		/// </summary>
		/// <param name="level">the <see cref="T:IPA.Logging.Logger.Level" /> of the message</param>
		/// <param name="time">the <see cref="T:System.DateTime" /> the message was recorded at</param>
		/// <param name="logName">the name of the log that sent the message</param>
		/// <param name="message">the message to print</param>
		// Token: 0x06000161 RID: 353 RVA: 0x00005E74 File Offset: 0x00004074
		public override void Print(Logger.Level level, DateTime time, string logName, string message)
		{
			if ((level & (Logger.Level)StandardLogger.PrintFilter) == Logger.Level.None)
			{
				return;
			}
			this.EnsureDefaultsPopulated(WinConsole.OutHandle, false);
			this.SetColor(this.Color, WinConsole.OutHandle);
			foreach (string line in message.Split(new string[]
			{
				"\n",
				Environment.NewLine
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				WinConsole.ConOut.WriteLine(Logger.LogFormat, new object[]
				{
					line,
					logName,
					time,
					level.ToString().ToUpper()
				});
			}
			this.ResetColor(WinConsole.OutHandle);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005F20 File Offset: 0x00004120
		private void EnsureDefaultsPopulated(IntPtr handle, bool force = false)
		{
			if (!ColoredConsolePrinter._haveReadDefaultColors || force)
			{
				ColoredConsolePrinter.ConsoleScreenBufferInfo info;
				ColoredConsolePrinter.GetConsoleScreenBufferInfo(handle, out info);
				ColoredConsolePrinter._defaultColors = info.Attribute & -16;
				ColoredConsolePrinter._haveReadDefaultColors = true;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005F58 File Offset: 0x00004158
		private void ResetColor(IntPtr handle)
		{
			ColoredConsolePrinter.ConsoleScreenBufferInfo info;
			ColoredConsolePrinter.GetConsoleScreenBufferInfo(handle, out info);
			short otherAttrs = info.Attribute & -16;
			ColoredConsolePrinter.SetConsoleTextAttribute(handle, otherAttrs | ColoredConsolePrinter._defaultColors);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005F88 File Offset: 0x00004188
		private void SetColor(ConsoleColor col, IntPtr handle)
		{
			ColoredConsolePrinter.ConsoleScreenBufferInfo info;
			ColoredConsolePrinter.GetConsoleScreenBufferInfo(handle, out info);
			short attr = ColoredConsolePrinter.GetAttrForeground((int)info.Attribute, col);
			ColoredConsolePrinter.SetConsoleTextAttribute(handle, attr);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005FB3 File Offset: 0x000041B3
		private static short GetAttrForeground(int attr, ConsoleColor color)
		{
			attr &= -16;
			return (short)(attr | (int)color);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005FC0 File Offset: 0x000041C0
		private static ConsoleColor GetConsoleColor(IntPtr handle)
		{
			ColoredConsolePrinter.ConsoleScreenBufferInfo info;
			ColoredConsolePrinter.GetConsoleScreenBufferInfo(handle, out info);
			return (ConsoleColor)(info.Attribute & 15);
		}

		// Token: 0x06000167 RID: 359
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool GetConsoleScreenBufferInfo(IntPtr handle, out ColoredConsolePrinter.ConsoleScreenBufferInfo info);

		// Token: 0x06000168 RID: 360
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool SetConsoleTextAttribute(IntPtr handle, short attribute);

		// Token: 0x04000080 RID: 128
		private Logger.LogLevel filter = Logger.LogLevel.All;

		// Token: 0x04000082 RID: 130
		private static bool _haveReadDefaultColors;

		// Token: 0x04000083 RID: 131
		private static short _defaultColors;

		// Token: 0x020000E6 RID: 230
		private struct Coordinate
		{
			// Token: 0x0400030F RID: 783
			public short X;

			// Token: 0x04000310 RID: 784
			public short Y;
		}

		// Token: 0x020000E7 RID: 231
		private struct SmallRect
		{
			// Token: 0x04000311 RID: 785
			public short Left;

			// Token: 0x04000312 RID: 786
			public short Top;

			// Token: 0x04000313 RID: 787
			public short Right;

			// Token: 0x04000314 RID: 788
			public short Bottom;
		}

		// Token: 0x020000E8 RID: 232
		private struct ConsoleScreenBufferInfo
		{
			// Token: 0x04000315 RID: 789
			public ColoredConsolePrinter.Coordinate Size;

			// Token: 0x04000316 RID: 790
			public ColoredConsolePrinter.Coordinate CursorPosition;

			// Token: 0x04000317 RID: 791
			public short Attribute;

			// Token: 0x04000318 RID: 792
			public ColoredConsolePrinter.SmallRect Window;

			// Token: 0x04000319 RID: 793
			public ColoredConsolePrinter.Coordinate MaxWindowSize;
		}
	}
}
