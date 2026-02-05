using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000094 RID: 148
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class LocalFileSettingsProvider : SettingsProvider, IApplicationSettingsProvider
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00021D9C File Offset: 0x0001FF9C
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00021DA4 File Offset: 0x0001FFA4
		public override string ApplicationName
		{
			get
			{
				return this._appName;
			}
			set
			{
				this._appName = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00021DAD File Offset: 0x0001FFAD
		private LocalFileSettingsProvider.XmlEscaper Escaper
		{
			get
			{
				if (this._escaper == null)
				{
					this._escaper = new LocalFileSettingsProvider.XmlEscaper();
				}
				return this._escaper;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00021DC8 File Offset: 0x0001FFC8
		private ClientSettingsStore Store
		{
			get
			{
				if (this._store == null)
				{
					this._store = new ClientSettingsStore();
				}
				return this._store;
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00021DE3 File Offset: 0x0001FFE3
		public override void Initialize(string name, NameValueCollection values)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "LocalFileSettingsProvider";
			}
			base.Initialize(name, values);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00021DFC File Offset: 0x0001FFFC
		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
		{
			SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
			string sectionName = this.GetSectionName(context);
			IDictionary dictionary = this.Store.ReadSettings(sectionName, false);
			IDictionary dictionary2 = this.Store.ReadSettings(sectionName, true);
			ConnectionStringSettingsCollection connectionStringSettingsCollection = this.Store.ReadConnectionStrings();
			foreach (object obj in properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				string name = settingsProperty.Name;
				SettingsPropertyValue settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
				SpecialSettingAttribute specialSettingAttribute = settingsProperty.Attributes[typeof(SpecialSettingAttribute)] as SpecialSettingAttribute;
				bool flag = specialSettingAttribute != null && specialSettingAttribute.SpecialSetting == SpecialSetting.ConnectionString;
				if (flag)
				{
					string text = sectionName + "." + name;
					if (connectionStringSettingsCollection != null && connectionStringSettingsCollection[text] != null)
					{
						settingsPropertyValue.PropertyValue = connectionStringSettingsCollection[text].ConnectionString;
					}
					else if (settingsProperty.DefaultValue != null && settingsProperty.DefaultValue is string)
					{
						settingsPropertyValue.PropertyValue = settingsProperty.DefaultValue;
					}
					else
					{
						settingsPropertyValue.PropertyValue = string.Empty;
					}
					settingsPropertyValue.IsDirty = false;
					settingsPropertyValueCollection.Add(settingsPropertyValue);
				}
				else
				{
					bool flag2 = this.IsUserSetting(settingsProperty);
					if (flag2 && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
					{
						throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
					}
					IDictionary dictionary3 = (flag2 ? dictionary2 : dictionary);
					if (dictionary3.Contains(name))
					{
						StoredSetting storedSetting = (StoredSetting)dictionary3[name];
						string text2 = storedSetting.Value.InnerXml;
						if (storedSetting.SerializeAs == SettingsSerializeAs.String)
						{
							text2 = this.Escaper.Unescape(text2);
						}
						settingsPropertyValue.SerializedValue = text2;
					}
					else if (settingsProperty.DefaultValue != null)
					{
						settingsPropertyValue.SerializedValue = settingsProperty.DefaultValue;
					}
					else
					{
						settingsPropertyValue.PropertyValue = null;
					}
					settingsPropertyValue.IsDirty = false;
					settingsPropertyValueCollection.Add(settingsPropertyValue);
				}
			}
			return settingsPropertyValueCollection;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00022010 File Offset: 0x00020210
		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
		{
			string sectionName = this.GetSectionName(context);
			IDictionary dictionary = new Hashtable();
			IDictionary dictionary2 = new Hashtable();
			foreach (object obj in values)
			{
				SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj;
				SettingsProperty property = settingsPropertyValue.Property;
				bool flag = this.IsUserSetting(property);
				if (settingsPropertyValue.IsDirty && flag)
				{
					bool flag2 = LocalFileSettingsProvider.IsRoamingSetting(property);
					StoredSetting storedSetting = new StoredSetting(property.SerializeAs, this.SerializeToXmlElement(property, settingsPropertyValue));
					if (flag2)
					{
						dictionary[property.Name] = storedSetting;
					}
					else
					{
						dictionary2[property.Name] = storedSetting;
					}
					settingsPropertyValue.IsDirty = false;
				}
			}
			if (dictionary.Count > 0)
			{
				this.Store.WriteSettings(sectionName, true, dictionary);
			}
			if (dictionary2.Count > 0)
			{
				this.Store.WriteSettings(sectionName, false, dictionary2);
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00022120 File Offset: 0x00020320
		public void Reset(SettingsContext context)
		{
			string sectionName = this.GetSectionName(context);
			this.Store.RevertToParent(sectionName, true);
			this.Store.RevertToParent(sectionName, false);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00022150 File Offset: 0x00020350
		public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
		{
			SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
			SettingsPropertyCollection settingsPropertyCollection2 = new SettingsPropertyCollection();
			foreach (object obj in properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				bool flag = LocalFileSettingsProvider.IsRoamingSetting(settingsProperty);
				if (flag)
				{
					settingsPropertyCollection2.Add(settingsProperty);
				}
				else
				{
					settingsPropertyCollection.Add(settingsProperty);
				}
			}
			if (settingsPropertyCollection2.Count > 0)
			{
				this.Upgrade(context, settingsPropertyCollection2, true);
			}
			if (settingsPropertyCollection.Count > 0)
			{
				this.Upgrade(context, settingsPropertyCollection, false);
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000221EC File Offset: 0x000203EC
		private Version CreateVersion(string name)
		{
			Version version = null;
			try
			{
				version = new Version(name);
			}
			catch (ArgumentException)
			{
				version = null;
			}
			catch (OverflowException)
			{
				version = null;
			}
			catch (FormatException)
			{
				version = null;
			}
			return version;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0002223C File Offset: 0x0002043C
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
		{
			bool flag = LocalFileSettingsProvider.IsRoamingSetting(property);
			string previousConfigFileName = this.GetPreviousConfigFileName(flag);
			if (!string.IsNullOrEmpty(previousConfigFileName))
			{
				SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
				settingsPropertyCollection.Add(property);
				SettingsPropertyValueCollection settingValuesFromFile = this.GetSettingValuesFromFile(previousConfigFileName, this.GetSectionName(context), true, settingsPropertyCollection);
				return settingValuesFromFile[property.Name];
			}
			return new SettingsPropertyValue(property)
			{
				PropertyValue = null
			};
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000222A0 File Offset: 0x000204A0
		private string GetPreviousConfigFileName(bool isRoaming)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			string text = (isRoaming ? this._prevRoamingConfigFileName : this._prevLocalConfigFileName);
			if (string.IsNullOrEmpty(text))
			{
				string text2 = (isRoaming ? ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigDirectory : ConfigurationManagerInternalFactory.Instance.ExeLocalConfigDirectory);
				Version version = this.CreateVersion(ConfigurationManagerInternalFactory.Instance.ExeProductVersion);
				Version version2 = null;
				DirectoryInfo directoryInfo = null;
				string text3 = null;
				if (version == null)
				{
					return null;
				}
				DirectoryInfo parent = Directory.GetParent(text2);
				if (parent.Exists)
				{
					foreach (DirectoryInfo directoryInfo2 in parent.GetDirectories())
					{
						Version version3 = this.CreateVersion(directoryInfo2.Name);
						if (version3 != null && version3 < version)
						{
							if (version2 == null)
							{
								version2 = version3;
								directoryInfo = directoryInfo2;
							}
							else if (version3 > version2)
							{
								version2 = version3;
								directoryInfo = directoryInfo2;
							}
						}
					}
					if (directoryInfo != null)
					{
						text3 = Path.Combine(directoryInfo.FullName, ConfigurationManagerInternalFactory.Instance.UserConfigFilename);
					}
					if (File.Exists(text3))
					{
						text = text3;
					}
				}
				if (isRoaming)
				{
					this._prevRoamingConfigFileName = text;
				}
				else
				{
					this._prevLocalConfigFileName = text;
				}
			}
			return text;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000223E0 File Offset: 0x000205E0
		private string GetSectionName(SettingsContext context)
		{
			string text = (string)context["GroupName"];
			string text2 = (string)context["SettingsKey"];
			string text3 = text;
			if (!string.IsNullOrEmpty(text2))
			{
				text3 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[] { text3, text2 });
			}
			return XmlConvert.EncodeLocalName(text3);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00022440 File Offset: 0x00020640
		private SettingsPropertyValueCollection GetSettingValuesFromFile(string configFileName, string sectionName, bool userScoped, SettingsPropertyCollection properties)
		{
			SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
			IDictionary dictionary = ClientSettingsStore.ReadSettingsFromFile(configFileName, sectionName, userScoped);
			foreach (object obj in properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				string name = settingsProperty.Name;
				SettingsPropertyValue settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
				if (dictionary.Contains(name))
				{
					StoredSetting storedSetting = (StoredSetting)dictionary[name];
					string text = storedSetting.Value.InnerXml;
					if (storedSetting.SerializeAs == SettingsSerializeAs.String)
					{
						text = this.Escaper.Unescape(text);
					}
					settingsPropertyValue.SerializedValue = text;
					settingsPropertyValue.IsDirty = true;
					settingsPropertyValueCollection.Add(settingsPropertyValue);
				}
			}
			return settingsPropertyValueCollection;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0002250C File Offset: 0x0002070C
		private static bool IsRoamingSetting(SettingsProperty setting)
		{
			bool flag = !ApplicationSettingsBase.IsClickOnceDeployed(AppDomain.CurrentDomain);
			bool flag2 = false;
			if (flag)
			{
				SettingsManageabilityAttribute settingsManageabilityAttribute = setting.Attributes[typeof(SettingsManageabilityAttribute)] as SettingsManageabilityAttribute;
				flag2 = settingsManageabilityAttribute != null && (settingsManageabilityAttribute.Manageability & SettingsManageability.Roaming) == SettingsManageability.Roaming;
			}
			return flag2;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0002255C File Offset: 0x0002075C
		private bool IsUserSetting(SettingsProperty setting)
		{
			bool flag = setting.Attributes[typeof(UserScopedSettingAttribute)] is UserScopedSettingAttribute;
			bool flag2 = setting.Attributes[typeof(ApplicationScopedSettingAttribute)] is ApplicationScopedSettingAttribute;
			if (flag && flag2)
			{
				throw new ConfigurationErrorsException(SR.GetString("BothScopeAttributes"));
			}
			if (!flag && !flag2)
			{
				throw new ConfigurationErrorsException(SR.GetString("NoScopeAttributes"));
			}
			return flag;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000225D0 File Offset: 0x000207D0
		private XmlNode SerializeToXmlElement(SettingsProperty setting, SettingsPropertyValue value)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("value");
			string text = value.SerializedValue as string;
			if (text == null && setting.SerializeAs == SettingsSerializeAs.Binary)
			{
				byte[] array = value.SerializedValue as byte[];
				if (array != null)
				{
					text = Convert.ToBase64String(array);
				}
			}
			if (text == null)
			{
				text = string.Empty;
			}
			if (setting.SerializeAs == SettingsSerializeAs.String)
			{
				text = this.Escaper.Escape(text);
			}
			xmlElement.InnerXml = text;
			XmlNode xmlNode = null;
			foreach (object obj in xmlElement.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.NodeType == XmlNodeType.XmlDeclaration)
				{
					xmlNode = xmlNode2;
					break;
				}
			}
			if (xmlNode != null)
			{
				xmlElement.RemoveChild(xmlNode);
			}
			return xmlElement;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000226B0 File Offset: 0x000208B0
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)]
		private void Upgrade(SettingsContext context, SettingsPropertyCollection properties, bool isRoaming)
		{
			string previousConfigFileName = this.GetPreviousConfigFileName(isRoaming);
			if (!string.IsNullOrEmpty(previousConfigFileName))
			{
				SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
				foreach (object obj in properties)
				{
					SettingsProperty settingsProperty = (SettingsProperty)obj;
					if (!(settingsProperty.Attributes[typeof(NoSettingsVersionUpgradeAttribute)] is NoSettingsVersionUpgradeAttribute))
					{
						settingsPropertyCollection.Add(settingsProperty);
					}
				}
				SettingsPropertyValueCollection settingValuesFromFile = this.GetSettingValuesFromFile(previousConfigFileName, this.GetSectionName(context), true, settingsPropertyCollection);
				this.SetPropertyValues(context, settingValuesFromFile);
			}
		}

		// Token: 0x04000C30 RID: 3120
		private string _appName = string.Empty;

		// Token: 0x04000C31 RID: 3121
		private ClientSettingsStore _store;

		// Token: 0x04000C32 RID: 3122
		private string _prevLocalConfigFileName;

		// Token: 0x04000C33 RID: 3123
		private string _prevRoamingConfigFileName;

		// Token: 0x04000C34 RID: 3124
		private LocalFileSettingsProvider.XmlEscaper _escaper;

		// Token: 0x020006EA RID: 1770
		private class XmlEscaper
		{
			// Token: 0x06004062 RID: 16482 RVA: 0x0010E34D File Offset: 0x0010C54D
			internal XmlEscaper()
			{
				this.doc = new XmlDocument();
				this.temp = this.doc.CreateElement("temp");
			}

			// Token: 0x06004063 RID: 16483 RVA: 0x0010E376 File Offset: 0x0010C576
			internal string Escape(string xmlString)
			{
				if (string.IsNullOrEmpty(xmlString))
				{
					return xmlString;
				}
				this.temp.InnerText = xmlString;
				return this.temp.InnerXml;
			}

			// Token: 0x06004064 RID: 16484 RVA: 0x0010E399 File Offset: 0x0010C599
			internal string Unescape(string escapedString)
			{
				if (string.IsNullOrEmpty(escapedString))
				{
					return escapedString;
				}
				this.temp.InnerXml = escapedString;
				return this.temp.InnerText;
			}

			// Token: 0x0400306A RID: 12394
			private XmlDocument doc;

			// Token: 0x0400306B RID: 12395
			private XmlElement temp;
		}
	}
}
