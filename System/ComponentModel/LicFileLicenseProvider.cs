using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000583 RID: 1411
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class LicFileLicenseProvider : LicenseProvider
	{
		// Token: 0x0600341C RID: 13340 RVA: 0x000E463A File Offset: 0x000E283A
		protected virtual bool IsKeyValid(string key, Type type)
		{
			return key != null && key.StartsWith(this.GetKey(type));
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000E464E File Offset: 0x000E284E
		protected virtual string GetKey(Type type)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} is a licensed component.", new object[] { type.FullName });
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000E4670 File Offset: 0x000E2870
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
			LicFileLicenseProvider.LicFileLicense licFileLicense = null;
			if (context != null)
			{
				if (context.UsageMode == LicenseUsageMode.Runtime)
				{
					string savedLicenseKey = context.GetSavedLicenseKey(type, null);
					if (savedLicenseKey != null && this.IsKeyValid(savedLicenseKey, type))
					{
						licFileLicense = new LicFileLicenseProvider.LicFileLicense(this, savedLicenseKey);
					}
				}
				if (licFileLicense == null)
				{
					string text = null;
					if (context != null)
					{
						ITypeResolutionService typeResolutionService = (ITypeResolutionService)context.GetService(typeof(ITypeResolutionService));
						if (typeResolutionService != null)
						{
							text = typeResolutionService.GetPathOfAssembly(type.Assembly.GetName());
						}
					}
					if (text == null)
					{
						text = type.Module.FullyQualifiedName;
					}
					string directoryName = Path.GetDirectoryName(text);
					string text2 = directoryName + "\\" + type.FullName + ".lic";
					if (File.Exists(text2))
					{
						Stream stream = new FileStream(text2, FileMode.Open, FileAccess.Read, FileShare.Read);
						StreamReader streamReader = new StreamReader(stream);
						string text3 = streamReader.ReadLine();
						streamReader.Close();
						if (this.IsKeyValid(text3, type))
						{
							licFileLicense = new LicFileLicenseProvider.LicFileLicense(this, this.GetKey(type));
						}
					}
					if (licFileLicense != null)
					{
						context.SetSavedLicenseKey(type, licFileLicense.LicenseKey);
					}
				}
			}
			return licFileLicense;
		}

		// Token: 0x02000896 RID: 2198
		private class LicFileLicense : License
		{
			// Token: 0x06004598 RID: 17816 RVA: 0x0012310B File Offset: 0x0012130B
			public LicFileLicense(LicFileLicenseProvider owner, string key)
			{
				this.owner = owner;
				this.key = key;
			}

			// Token: 0x17000FBA RID: 4026
			// (get) Token: 0x06004599 RID: 17817 RVA: 0x00123121 File Offset: 0x00121321
			public override string LicenseKey
			{
				get
				{
					return this.key;
				}
			}

			// Token: 0x0600459A RID: 17818 RVA: 0x00123129 File Offset: 0x00121329
			public override void Dispose()
			{
				GC.SuppressFinalize(this);
			}

			// Token: 0x040037C8 RID: 14280
			private LicFileLicenseProvider owner;

			// Token: 0x040037C9 RID: 14281
			private string key;
		}
	}
}
