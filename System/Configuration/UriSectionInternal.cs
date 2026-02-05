using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000076 RID: 118
	internal sealed class UriSectionInternal
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x0001FB66 File Offset: 0x0001DD66
		private UriSectionInternal()
		{
			this.schemeSettings = new Dictionary<string, SchemeSettingInternal>();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001FB7C File Offset: 0x0001DD7C
		private UriSectionInternal(UriSection section)
			: this()
		{
			this.idnScope = section.Idn.Enabled;
			this.iriParsing = section.IriParsing.Enabled;
			if (section.SchemeSettings != null)
			{
				foreach (object obj in section.SchemeSettings)
				{
					SchemeSettingElement schemeSettingElement = (SchemeSettingElement)obj;
					SchemeSettingInternal schemeSettingInternal = new SchemeSettingInternal(schemeSettingElement.Name, schemeSettingElement.GenericUriParserOptions);
					this.schemeSettings.Add(schemeSettingInternal.Name, schemeSettingInternal);
				}
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001FC24 File Offset: 0x0001DE24
		private UriSectionInternal(UriIdnScope idnScope, bool iriParsing, IEnumerable<SchemeSettingInternal> schemeSettings)
			: this()
		{
			this.idnScope = idnScope;
			this.iriParsing = iriParsing;
			if (schemeSettings != null)
			{
				foreach (SchemeSettingInternal schemeSettingInternal in schemeSettings)
				{
					this.schemeSettings.Add(schemeSettingInternal.Name, schemeSettingInternal);
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001FC90 File Offset: 0x0001DE90
		internal UriIdnScope IdnScope
		{
			get
			{
				return this.idnScope;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x0001FC98 File Offset: 0x0001DE98
		internal bool IriParsing
		{
			get
			{
				return this.iriParsing;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001FCA0 File Offset: 0x0001DEA0
		internal SchemeSettingInternal GetSchemeSetting(string scheme)
		{
			SchemeSettingInternal schemeSettingInternal;
			if (this.schemeSettings.TryGetValue(scheme.ToLowerInvariant(), out schemeSettingInternal))
			{
				return schemeSettingInternal;
			}
			return null;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001FCC8 File Offset: 0x0001DEC8
		internal static UriSectionInternal GetSection()
		{
			object obj = UriSectionInternal.classSyncObject;
			UriSectionInternal uriSectionInternal;
			lock (obj)
			{
				string text = null;
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				try
				{
					text = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (UriSectionInternal.IsWebConfig(text))
				{
					uriSectionInternal = UriSectionInternal.LoadUsingSystemConfiguration();
				}
				else
				{
					uriSectionInternal = UriSectionInternal.LoadUsingCustomParser(text);
				}
			}
			return uriSectionInternal;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001FD4C File Offset: 0x0001DF4C
		private static UriSectionInternal LoadUsingSystemConfiguration()
		{
			UriSectionInternal uriSectionInternal;
			try
			{
				UriSection uriSection = PrivilegedConfigurationManager.GetSection("uri") as UriSection;
				if (uriSection == null)
				{
					uriSectionInternal = null;
				}
				else
				{
					uriSectionInternal = new UriSectionInternal(uriSection);
				}
			}
			catch (ConfigurationException)
			{
				uriSectionInternal = null;
			}
			return uriSectionInternal;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001FD90 File Offset: 0x0001DF90
		private static UriSectionInternal LoadUsingCustomParser(string appConfigFilePath)
		{
			string text = null;
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			try
			{
				text = RuntimeEnvironment.GetRuntimeDirectory();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			string text2 = Path.Combine(Path.Combine(text, "Config"), "machine.config");
			UriSectionData uriSectionData = UriSectionReader.Read(text2);
			UriSectionData uriSectionData2 = UriSectionReader.Read(appConfigFilePath, uriSectionData);
			UriSectionData uriSectionData3 = null;
			if (uriSectionData2 != null)
			{
				uriSectionData3 = uriSectionData2;
			}
			else if (uriSectionData != null)
			{
				uriSectionData3 = uriSectionData;
			}
			if (uriSectionData3 != null)
			{
				UriIdnScope valueOrDefault = uriSectionData3.IdnScope.GetValueOrDefault();
				bool valueOrDefault2 = uriSectionData3.IriParsing.GetValueOrDefault();
				IEnumerable<SchemeSettingInternal> values = uriSectionData3.SchemeSettings.Values;
				return new UriSectionInternal(valueOrDefault, valueOrDefault2, values);
			}
			return null;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001FE44 File Offset: 0x0001E044
		private static bool IsWebConfig(string appConfigFile)
		{
			string text = AppDomain.CurrentDomain.GetData(".appVPath") as string;
			return text != null || (appConfigFile != null && (appConfigFile.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || appConfigFile.StartsWith("https://", StringComparison.OrdinalIgnoreCase)));
		}

		// Token: 0x04000BF7 RID: 3063
		private static readonly object classSyncObject = new object();

		// Token: 0x04000BF8 RID: 3064
		private UriIdnScope idnScope;

		// Token: 0x04000BF9 RID: 3065
		private bool iriParsing;

		// Token: 0x04000BFA RID: 3066
		private Dictionary<string, SchemeSettingInternal> schemeSettings;
	}
}
