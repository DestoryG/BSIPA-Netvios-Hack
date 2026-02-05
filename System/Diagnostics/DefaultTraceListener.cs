using System;
using System.IO;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000497 RID: 1175
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class DefaultTraceListener : TraceListener
	{
		// Token: 0x06002B8A RID: 11146 RVA: 0x000C5079 File Offset: 0x000C3279
		public DefaultTraceListener()
			: base("Default")
		{
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000C5086 File Offset: 0x000C3286
		// (set) Token: 0x06002B8C RID: 11148 RVA: 0x000C509C File Offset: 0x000C329C
		public bool AssertUiEnabled
		{
			get
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				return this.assertUIEnabled;
			}
			set
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				this.assertUIEnabled = value;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000C50B3 File Offset: 0x000C32B3
		// (set) Token: 0x06002B8E RID: 11150 RVA: 0x000C50C9 File Offset: 0x000C32C9
		public string LogFileName
		{
			get
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				return this.logFileName;
			}
			set
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				this.logFileName = value;
			}
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000C50E0 File Offset: 0x000C32E0
		public override void Fail(string message)
		{
			this.Fail(message, null);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000C50EC File Offset: 0x000C32EC
		public override void Fail(string message, string detailMessage)
		{
			StackTrace stackTrace = new StackTrace(true);
			int num = 0;
			bool uiPermission = DefaultTraceListener.UiPermission;
			string text;
			try
			{
				text = stackTrace.ToString();
			}
			catch
			{
				text = "";
			}
			this.WriteAssert(text, message, detailMessage);
			if (this.AssertUiEnabled && uiPermission)
			{
				AssertWrapper.ShowAssert(text, stackTrace.GetFrame(num), message, detailMessage);
			}
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000C514C File Offset: 0x000C334C
		private void InitializeSettings()
		{
			this.assertUIEnabled = DiagnosticsConfiguration.AssertUIEnabled;
			this.logFileName = DiagnosticsConfiguration.LogFileName;
			this.settingsInitialized = true;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000C516C File Offset: 0x000C336C
		private void WriteAssert(string stackTrace, string message, string detailMessage)
		{
			string text = string.Concat(new string[]
			{
				SR.GetString("DebugAssertBanner"),
				Environment.NewLine,
				SR.GetString("DebugAssertShortMessage"),
				Environment.NewLine,
				message,
				Environment.NewLine,
				SR.GetString("DebugAssertLongMessage"),
				Environment.NewLine,
				detailMessage,
				Environment.NewLine,
				stackTrace
			});
			this.WriteLine(text);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000C51EC File Offset: 0x000C33EC
		private void WriteToLogFile(string message, bool useWriteLine)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(this.LogFileName);
				using (Stream stream = fileInfo.Open(FileMode.OpenOrCreate))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream))
					{
						stream.Position = stream.Length;
						if (useWriteLine)
						{
							streamWriter.WriteLine(message);
						}
						else
						{
							streamWriter.Write(message);
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.WriteLine(SR.GetString("ExceptionOccurred", new object[]
				{
					this.LogFileName,
					ex.ToString()
				}), false);
			}
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000C52A4 File Offset: 0x000C34A4
		public override void Write(string message)
		{
			this.Write(message, true);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000C52B0 File Offset: 0x000C34B0
		private void Write(string message, bool useLogFile)
		{
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			if (message == null || message.Length <= 16384)
			{
				this.internalWrite(message);
			}
			else
			{
				int i;
				for (i = 0; i < message.Length - 16384; i += 16384)
				{
					this.internalWrite(message.Substring(i, 16384));
				}
				this.internalWrite(message.Substring(i));
			}
			if (useLogFile && this.LogFileName.Length != 0)
			{
				this.WriteToLogFile(message, false);
			}
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000C5336 File Offset: 0x000C3536
		private void internalWrite(string message)
		{
			if (Debugger.IsLogging())
			{
				Debugger.Log(0, null, message);
				return;
			}
			if (message == null)
			{
				SafeNativeMethods.OutputDebugString(string.Empty);
				return;
			}
			SafeNativeMethods.OutputDebugString(message);
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000C535C File Offset: 0x000C355C
		public override void WriteLine(string message)
		{
			this.WriteLine(message, true);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000C5366 File Offset: 0x000C3566
		private void WriteLine(string message, bool useLogFile)
		{
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			this.Write(message + Environment.NewLine, useLogFile);
			base.NeedIndent = true;
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06002B99 RID: 11161 RVA: 0x000C5390 File Offset: 0x000C3590
		private static bool UiPermission
		{
			get
			{
				bool flag = false;
				try
				{
					new UIPermission(UIPermissionWindow.SafeSubWindows).Demand();
					flag = true;
				}
				catch
				{
				}
				return flag;
			}
		}

		// Token: 0x0400267D RID: 9853
		private bool assertUIEnabled;

		// Token: 0x0400267E RID: 9854
		private string logFileName;

		// Token: 0x0400267F RID: 9855
		private bool settingsInitialized;

		// Token: 0x04002680 RID: 9856
		private const int internalWriteSize = 16384;
	}
}
