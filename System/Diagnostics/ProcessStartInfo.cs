using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004FF RID: 1279
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true, SelfAffectingProcessMgmt = true)]
	public sealed class ProcessStartInfo
	{
		// Token: 0x06003063 RID: 12387 RVA: 0x000DB67B File Offset: 0x000D987B
		public ProcessStartInfo()
		{
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000DB68A File Offset: 0x000D988A
		internal ProcessStartInfo(Process parent)
		{
			this.weakParentProcess = new WeakReference(parent);
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000DB6A5 File Offset: 0x000D98A5
		public ProcessStartInfo(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000DB6BB File Offset: 0x000D98BB
		public ProcessStartInfo(string fileName, string arguments)
		{
			this.fileName = fileName;
			this.arguments = arguments;
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x000DB6D8 File Offset: 0x000D98D8
		// (set) Token: 0x06003068 RID: 12392 RVA: 0x000DB6EE File Offset: 0x000D98EE
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.VerbConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[MonitoringDescription("ProcessVerb")]
		[NotifyParentProperty(true)]
		public string Verb
		{
			get
			{
				if (this.verb == null)
				{
					return string.Empty;
				}
				return this.verb;
			}
			set
			{
				this.verb = value;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x000DB6F7 File Offset: 0x000D98F7
		// (set) Token: 0x0600306A RID: 12394 RVA: 0x000DB70D File Offset: 0x000D990D
		[DefaultValue("")]
		[MonitoringDescription("ProcessArguments")]
		[SettingsBindable(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		public string Arguments
		{
			get
			{
				if (this.arguments == null)
				{
					return string.Empty;
				}
				return this.arguments;
			}
			set
			{
				this.arguments = value;
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x000DB716 File Offset: 0x000D9916
		// (set) Token: 0x0600306C RID: 12396 RVA: 0x000DB71E File Offset: 0x000D991E
		[DefaultValue(false)]
		[MonitoringDescription("ProcessCreateNoWindow")]
		[NotifyParentProperty(true)]
		public bool CreateNoWindow
		{
			get
			{
				return this.createNoWindow;
			}
			set
			{
				this.createNoWindow = value;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600306D RID: 12397 RVA: 0x000DB728 File Offset: 0x000D9928
		[Editor("System.Diagnostics.Design.StringDictionaryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[MonitoringDescription("ProcessEnvironmentVariables")]
		[NotifyParentProperty(true)]
		public StringDictionary EnvironmentVariables
		{
			get
			{
				if (this.environmentVariables == null)
				{
					this.environmentVariables = new StringDictionaryWithComparer();
					if (this.weakParentProcess == null || !this.weakParentProcess.IsAlive || ((Component)this.weakParentProcess.Target).Site == null || !((Component)this.weakParentProcess.Target).Site.DesignMode)
					{
						foreach (object obj in global::System.Environment.GetEnvironmentVariables())
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
							this.environmentVariables.Add((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
						}
					}
				}
				return this.environmentVariables;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600306E RID: 12398 RVA: 0x000DB800 File Offset: 0x000D9A00
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public IDictionary<string, string> Environment
		{
			get
			{
				if (this.environment == null)
				{
					this.environment = this.EnvironmentVariables.AsGenericDictionary();
				}
				return this.environment;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x0600306F RID: 12399 RVA: 0x000DB821 File Offset: 0x000D9A21
		// (set) Token: 0x06003070 RID: 12400 RVA: 0x000DB829 File Offset: 0x000D9A29
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardInput")]
		[NotifyParentProperty(true)]
		public bool RedirectStandardInput
		{
			get
			{
				return this.redirectStandardInput;
			}
			set
			{
				this.redirectStandardInput = value;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06003071 RID: 12401 RVA: 0x000DB832 File Offset: 0x000D9A32
		// (set) Token: 0x06003072 RID: 12402 RVA: 0x000DB83A File Offset: 0x000D9A3A
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardOutput")]
		[NotifyParentProperty(true)]
		public bool RedirectStandardOutput
		{
			get
			{
				return this.redirectStandardOutput;
			}
			set
			{
				this.redirectStandardOutput = value;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x000DB843 File Offset: 0x000D9A43
		// (set) Token: 0x06003074 RID: 12404 RVA: 0x000DB84B File Offset: 0x000D9A4B
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardError")]
		[NotifyParentProperty(true)]
		public bool RedirectStandardError
		{
			get
			{
				return this.redirectStandardError;
			}
			set
			{
				this.redirectStandardError = value;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x000DB854 File Offset: 0x000D9A54
		// (set) Token: 0x06003076 RID: 12406 RVA: 0x000DB85C File Offset: 0x000D9A5C
		public Encoding StandardErrorEncoding
		{
			get
			{
				return this.standardErrorEncoding;
			}
			set
			{
				this.standardErrorEncoding = value;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06003077 RID: 12407 RVA: 0x000DB865 File Offset: 0x000D9A65
		// (set) Token: 0x06003078 RID: 12408 RVA: 0x000DB86D File Offset: 0x000D9A6D
		public Encoding StandardOutputEncoding
		{
			get
			{
				return this.standardOutputEncoding;
			}
			set
			{
				this.standardOutputEncoding = value;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x000DB876 File Offset: 0x000D9A76
		// (set) Token: 0x0600307A RID: 12410 RVA: 0x000DB87E File Offset: 0x000D9A7E
		[DefaultValue(true)]
		[MonitoringDescription("ProcessUseShellExecute")]
		[NotifyParentProperty(true)]
		public bool UseShellExecute
		{
			get
			{
				return this.useShellExecute;
			}
			set
			{
				this.useShellExecute = value;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600307B RID: 12411 RVA: 0x000DB888 File Offset: 0x000D9A88
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] Verbs
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				RegistryKey registryKey = null;
				string extension = Path.GetExtension(this.FileName);
				try
				{
					if (extension != null && extension.Length > 0)
					{
						registryKey = Registry.ClassesRoot.OpenSubKey(extension);
						if (registryKey != null)
						{
							string text = (string)registryKey.GetValue(string.Empty);
							registryKey.Close();
							registryKey = Registry.ClassesRoot.OpenSubKey(text + "\\shell");
							if (registryKey != null)
							{
								string[] subKeyNames = registryKey.GetSubKeyNames();
								for (int i = 0; i < subKeyNames.Length; i++)
								{
									if (string.Compare(subKeyNames[i], "new", StringComparison.OrdinalIgnoreCase) != 0)
									{
										arrayList.Add(subKeyNames[i]);
									}
								}
								registryKey.Close();
								registryKey = null;
							}
						}
					}
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
				}
				string[] array = new string[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000DB970 File Offset: 0x000D9B70
		// (set) Token: 0x0600307D RID: 12413 RVA: 0x000DB986 File Offset: 0x000D9B86
		[NotifyParentProperty(true)]
		public string UserName
		{
			get
			{
				if (this.userName == null)
				{
					return string.Empty;
				}
				return this.userName;
			}
			set
			{
				this.userName = value;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x000DB98F File Offset: 0x000D9B8F
		// (set) Token: 0x0600307F RID: 12415 RVA: 0x000DB997 File Offset: 0x000D9B97
		public SecureString Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x000DB9A0 File Offset: 0x000D9BA0
		// (set) Token: 0x06003081 RID: 12417 RVA: 0x000DB9A8 File Offset: 0x000D9BA8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string PasswordInClearText
		{
			get
			{
				return this.passwordInClearText;
			}
			set
			{
				this.passwordInClearText = value;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x000DB9B1 File Offset: 0x000D9BB1
		// (set) Token: 0x06003083 RID: 12419 RVA: 0x000DB9C7 File Offset: 0x000D9BC7
		[NotifyParentProperty(true)]
		public string Domain
		{
			get
			{
				if (this.domain == null)
				{
					return string.Empty;
				}
				return this.domain;
			}
			set
			{
				this.domain = value;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000DB9D0 File Offset: 0x000D9BD0
		// (set) Token: 0x06003085 RID: 12421 RVA: 0x000DB9D8 File Offset: 0x000D9BD8
		[NotifyParentProperty(true)]
		public bool LoadUserProfile
		{
			get
			{
				return this.loadUserProfile;
			}
			set
			{
				this.loadUserProfile = value;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000DB9E1 File Offset: 0x000D9BE1
		// (set) Token: 0x06003087 RID: 12423 RVA: 0x000DB9F7 File Offset: 0x000D9BF7
		[DefaultValue("")]
		[Editor("System.Diagnostics.Design.StartFileNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[MonitoringDescription("ProcessFileName")]
		[SettingsBindable(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		public string FileName
		{
			get
			{
				if (this.fileName == null)
				{
					return string.Empty;
				}
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000DBA00 File Offset: 0x000D9C00
		// (set) Token: 0x06003089 RID: 12425 RVA: 0x000DBA16 File Offset: 0x000D9C16
		[DefaultValue("")]
		[MonitoringDescription("ProcessWorkingDirectory")]
		[Editor("System.Diagnostics.Design.WorkingDirectoryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SettingsBindable(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		public string WorkingDirectory
		{
			get
			{
				if (this.directory == null)
				{
					return string.Empty;
				}
				return this.directory;
			}
			set
			{
				this.directory = value;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x000DBA1F File Offset: 0x000D9C1F
		// (set) Token: 0x0600308B RID: 12427 RVA: 0x000DBA27 File Offset: 0x000D9C27
		[DefaultValue(false)]
		[MonitoringDescription("ProcessErrorDialog")]
		[NotifyParentProperty(true)]
		public bool ErrorDialog
		{
			get
			{
				return this.errorDialog;
			}
			set
			{
				this.errorDialog = value;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x000DBA30 File Offset: 0x000D9C30
		// (set) Token: 0x0600308D RID: 12429 RVA: 0x000DBA38 File Offset: 0x000D9C38
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr ErrorDialogParentHandle
		{
			get
			{
				return this.errorDialogParentHandle;
			}
			set
			{
				this.errorDialogParentHandle = value;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000DBA41 File Offset: 0x000D9C41
		// (set) Token: 0x0600308F RID: 12431 RVA: 0x000DBA49 File Offset: 0x000D9C49
		[DefaultValue(ProcessWindowStyle.Normal)]
		[MonitoringDescription("ProcessWindowStyle")]
		[NotifyParentProperty(true)]
		public ProcessWindowStyle WindowStyle
		{
			get
			{
				return this.windowStyle;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ProcessWindowStyle), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ProcessWindowStyle));
				}
				this.windowStyle = value;
			}
		}

		// Token: 0x04002890 RID: 10384
		private string fileName;

		// Token: 0x04002891 RID: 10385
		private string arguments;

		// Token: 0x04002892 RID: 10386
		private string directory;

		// Token: 0x04002893 RID: 10387
		private string verb;

		// Token: 0x04002894 RID: 10388
		private ProcessWindowStyle windowStyle;

		// Token: 0x04002895 RID: 10389
		private bool errorDialog;

		// Token: 0x04002896 RID: 10390
		private IntPtr errorDialogParentHandle;

		// Token: 0x04002897 RID: 10391
		private bool useShellExecute = true;

		// Token: 0x04002898 RID: 10392
		private string userName;

		// Token: 0x04002899 RID: 10393
		private string domain;

		// Token: 0x0400289A RID: 10394
		private SecureString password;

		// Token: 0x0400289B RID: 10395
		private string passwordInClearText;

		// Token: 0x0400289C RID: 10396
		private bool loadUserProfile;

		// Token: 0x0400289D RID: 10397
		private bool redirectStandardInput;

		// Token: 0x0400289E RID: 10398
		private bool redirectStandardOutput;

		// Token: 0x0400289F RID: 10399
		private bool redirectStandardError;

		// Token: 0x040028A0 RID: 10400
		private Encoding standardOutputEncoding;

		// Token: 0x040028A1 RID: 10401
		private Encoding standardErrorEncoding;

		// Token: 0x040028A2 RID: 10402
		private bool createNoWindow;

		// Token: 0x040028A3 RID: 10403
		private WeakReference weakParentProcess;

		// Token: 0x040028A4 RID: 10404
		internal StringDictionary environmentVariables;

		// Token: 0x040028A5 RID: 10405
		private IDictionary<string, string> environment;
	}
}
