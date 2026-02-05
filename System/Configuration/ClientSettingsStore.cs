using System;
using System.Collections;
using System.Configuration.Internal;
using System.IO;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200007F RID: 127
	internal sealed class ClientSettingsStore
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x00020D38 File Offset: 0x0001EF38
		private Configuration GetUserConfig(bool isRoaming)
		{
			ConfigurationUserLevel configurationUserLevel = (isRoaming ? ConfigurationUserLevel.PerUserRoaming : ConfigurationUserLevel.PerUserRoamingAndLocal);
			return ClientSettingsStore.ClientSettingsConfigurationHost.OpenExeConfiguration(configurationUserLevel);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00020D58 File Offset: 0x0001EF58
		private ClientSettingsSection GetConfigSection(Configuration config, string sectionName, bool declare)
		{
			string text = "userSettings/" + sectionName;
			ClientSettingsSection clientSettingsSection = null;
			if (config != null)
			{
				clientSettingsSection = config.GetSection(text) as ClientSettingsSection;
				if (clientSettingsSection == null && declare)
				{
					this.DeclareSection(config, sectionName);
					clientSettingsSection = config.GetSection(text) as ClientSettingsSection;
				}
			}
			return clientSettingsSection;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00020DA4 File Offset: 0x0001EFA4
		private void DeclareSection(Configuration config, string sectionName)
		{
			if (config.GetSectionGroup("userSettings") == null)
			{
				ConfigurationSectionGroup configurationSectionGroup = new UserSettingsGroup();
				config.SectionGroups.Add("userSettings", configurationSectionGroup);
			}
			ConfigurationSectionGroup sectionGroup = config.GetSectionGroup("userSettings");
			if (sectionGroup != null && sectionGroup.Sections[sectionName] == null)
			{
				ConfigurationSection configurationSection = new ClientSettingsSection();
				configurationSection.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
				configurationSection.SectionInformation.RequirePermission = false;
				sectionGroup.Sections.Add(sectionName, configurationSection);
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00020E28 File Offset: 0x0001F028
		internal IDictionary ReadSettings(string sectionName, bool isUserScoped)
		{
			IDictionary dictionary = new Hashtable();
			if (isUserScoped && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				return dictionary;
			}
			string text = (isUserScoped ? "userSettings/" : "applicationSettings/");
			ConfigurationManager.RefreshSection(text + sectionName);
			ClientSettingsSection clientSettingsSection = ConfigurationManager.GetSection(text + sectionName) as ClientSettingsSection;
			if (clientSettingsSection != null)
			{
				foreach (object obj in clientSettingsSection.Settings)
				{
					SettingElement settingElement = (SettingElement)obj;
					dictionary[settingElement.Name] = new StoredSetting(settingElement.SerializeAs, settingElement.Value.ValueXml);
				}
			}
			return dictionary;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00020EF4 File Offset: 0x0001F0F4
		internal static IDictionary ReadSettingsFromFile(string configFileName, string sectionName, bool isUserScoped)
		{
			IDictionary dictionary = new Hashtable();
			if (isUserScoped && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				return dictionary;
			}
			string text = (isUserScoped ? "userSettings/" : "applicationSettings/");
			ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
			ConfigurationUserLevel configurationUserLevel = (isUserScoped ? ConfigurationUserLevel.PerUserRoaming : ConfigurationUserLevel.None);
			if (isUserScoped)
			{
				exeConfigurationFileMap.ExeConfigFilename = ConfigurationManagerInternalFactory.Instance.ApplicationConfigUri;
				exeConfigurationFileMap.RoamingUserConfigFilename = configFileName;
			}
			else
			{
				exeConfigurationFileMap.ExeConfigFilename = configFileName;
			}
			Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, configurationUserLevel);
			ClientSettingsSection clientSettingsSection = configuration.GetSection(text + sectionName) as ClientSettingsSection;
			if (clientSettingsSection != null)
			{
				foreach (object obj in clientSettingsSection.Settings)
				{
					SettingElement settingElement = (SettingElement)obj;
					dictionary[settingElement.Name] = new StoredSetting(settingElement.SerializeAs, settingElement.Value.ValueXml);
				}
			}
			return dictionary;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00020FF8 File Offset: 0x0001F1F8
		internal ConnectionStringSettingsCollection ReadConnectionStrings()
		{
			return PrivilegedConfigurationManager.ConnectionStrings;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00021000 File Offset: 0x0001F200
		internal void RevertToParent(string sectionName, bool isRoaming)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			Configuration userConfig = this.GetUserConfig(isRoaming);
			ClientSettingsSection configSection = this.GetConfigSection(userConfig, sectionName, false);
			if (configSection != null)
			{
				configSection.SectionInformation.RevertToParent();
				userConfig.Save();
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00021050 File Offset: 0x0001F250
		internal void WriteSettings(string sectionName, bool isRoaming, IDictionary newSettings)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			Configuration userConfig = this.GetUserConfig(isRoaming);
			ClientSettingsSection configSection = this.GetConfigSection(userConfig, sectionName, true);
			if (configSection != null)
			{
				SettingElementCollection settings = configSection.Settings;
				foreach (object obj in newSettings)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					SettingElement settingElement = settings.Get((string)dictionaryEntry.Key);
					if (settingElement == null)
					{
						settingElement = new SettingElement();
						settingElement.Name = (string)dictionaryEntry.Key;
						settings.Add(settingElement);
					}
					StoredSetting storedSetting = (StoredSetting)dictionaryEntry.Value;
					settingElement.SerializeAs = storedSetting.SerializeAs;
					settingElement.Value.ValueXml = storedSetting.Value;
				}
				try
				{
					userConfig.Save();
					return;
				}
				catch (ConfigurationErrorsException ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("SettingsSaveFailed", new object[] { ex.Message }), ex);
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("SettingsSaveFailedNoSection"));
		}

		// Token: 0x04000C14 RID: 3092
		private const string ApplicationSettingsGroupName = "applicationSettings";

		// Token: 0x04000C15 RID: 3093
		private const string UserSettingsGroupName = "userSettings";

		// Token: 0x04000C16 RID: 3094
		private const string ApplicationSettingsGroupPrefix = "applicationSettings/";

		// Token: 0x04000C17 RID: 3095
		private const string UserSettingsGroupPrefix = "userSettings/";

		// Token: 0x020006E8 RID: 1768
		private sealed class ClientSettingsConfigurationHost : DelegatingConfigHost
		{
			// Token: 0x17000EDD RID: 3805
			// (get) Token: 0x06004043 RID: 16451 RVA: 0x0010DDBE File Offset: 0x0010BFBE
			private IInternalConfigClientHost ClientHost
			{
				get
				{
					return (IInternalConfigClientHost)base.Host;
				}
			}

			// Token: 0x17000EDE RID: 3806
			// (get) Token: 0x06004044 RID: 16452 RVA: 0x0010DDCB File Offset: 0x0010BFCB
			internal static IInternalConfigConfigurationFactory ConfigFactory
			{
				get
				{
					if (ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory == null)
					{
						ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory = (IInternalConfigConfigurationFactory)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.Internal.InternalConfigConfigurationFactory,System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
					}
					return ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory;
				}
			}

			// Token: 0x06004045 RID: 16453 RVA: 0x0010DDF3 File Offset: 0x0010BFF3
			private ClientSettingsConfigurationHost()
			{
			}

			// Token: 0x06004046 RID: 16454 RVA: 0x0010DDFB File Offset: 0x0010BFFB
			public override void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
			{
			}

			// Token: 0x06004047 RID: 16455 RVA: 0x0010DE00 File Offset: 0x0010C000
			public override void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
			{
				ConfigurationUserLevel configurationUserLevel = (ConfigurationUserLevel)hostInitConfigurationParams[0];
				base.Host = (IInternalConfigHost)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.ClientConfigurationHost,System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				string text;
				if (configurationUserLevel != ConfigurationUserLevel.None)
				{
					if (configurationUserLevel != ConfigurationUserLevel.PerUserRoaming)
					{
						if (configurationUserLevel != ConfigurationUserLevel.PerUserRoamingAndLocal)
						{
							throw new ArgumentException(SR.GetString("UnknownUserLevel"));
						}
						text = this.ClientHost.GetLocalUserConfigPath();
					}
					else
					{
						text = this.ClientHost.GetRoamingUserConfigPath();
					}
				}
				else
				{
					text = this.ClientHost.GetExeConfigPath();
				}
				base.Host.InitForConfiguration(ref locationSubPath, out configPath, out locationConfigPath, configRoot, new object[] { null, null, text });
			}

			// Token: 0x06004048 RID: 16456 RVA: 0x0010DE94 File Offset: 0x0010C094
			private bool IsKnownConfigFile(string filename)
			{
				return string.Equals(filename, ConfigurationManagerInternalFactory.Instance.MachineConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ApplicationConfigUri, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06004049 RID: 16457 RVA: 0x0010DEED File Offset: 0x0010C0ED
			internal static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
			{
				return ClientSettingsStore.ClientSettingsConfigurationHost.ConfigFactory.Create(typeof(ClientSettingsStore.ClientSettingsConfigurationHost), new object[] { userLevel });
			}

			// Token: 0x0600404A RID: 16458 RVA: 0x0010DF12 File Offset: 0x0010C112
			public override Stream OpenStreamForRead(string streamName)
			{
				if (this.IsKnownConfigFile(streamName))
				{
					return base.Host.OpenStreamForRead(streamName, true);
				}
				return base.Host.OpenStreamForRead(streamName);
			}

			// Token: 0x0600404B RID: 16459 RVA: 0x0010DF38 File Offset: 0x0010C138
			public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
			{
				Stream stream;
				if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					stream = new ClientSettingsStore.QuotaEnforcedStream(base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, true), false);
				}
				else if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					stream = new ClientSettingsStore.QuotaEnforcedStream(base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, true), true);
				}
				else
				{
					stream = base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext);
				}
				return stream;
			}

			// Token: 0x0600404C RID: 16460 RVA: 0x0010DFB0 File Offset: 0x0010C1B0
			public override void WriteCompleted(string streamName, bool success, object writeContext)
			{
				if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					base.Host.WriteCompleted(streamName, success, writeContext, true);
					return;
				}
				base.Host.WriteCompleted(streamName, success, writeContext);
			}

			// Token: 0x04003065 RID: 12389
			private const string ClientConfigurationHostTypeName = "System.Configuration.ClientConfigurationHost,System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			// Token: 0x04003066 RID: 12390
			private const string InternalConfigConfigurationFactoryTypeName = "System.Configuration.Internal.InternalConfigConfigurationFactory,System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			// Token: 0x04003067 RID: 12391
			private static volatile IInternalConfigConfigurationFactory s_configFactory;
		}

		// Token: 0x020006E9 RID: 1769
		private sealed class QuotaEnforcedStream : Stream
		{
			// Token: 0x0600404D RID: 16461 RVA: 0x0010E001 File Offset: 0x0010C201
			internal QuotaEnforcedStream(Stream originalStream, bool isRoaming)
			{
				this._originalStream = originalStream;
				this._isRoaming = isRoaming;
			}

			// Token: 0x17000EDF RID: 3807
			// (get) Token: 0x0600404E RID: 16462 RVA: 0x0010E017 File Offset: 0x0010C217
			public override bool CanRead
			{
				get
				{
					return this._originalStream.CanRead;
				}
			}

			// Token: 0x17000EE0 RID: 3808
			// (get) Token: 0x0600404F RID: 16463 RVA: 0x0010E024 File Offset: 0x0010C224
			public override bool CanWrite
			{
				get
				{
					return this._originalStream.CanWrite;
				}
			}

			// Token: 0x17000EE1 RID: 3809
			// (get) Token: 0x06004050 RID: 16464 RVA: 0x0010E031 File Offset: 0x0010C231
			public override bool CanSeek
			{
				get
				{
					return this._originalStream.CanSeek;
				}
			}

			// Token: 0x17000EE2 RID: 3810
			// (get) Token: 0x06004051 RID: 16465 RVA: 0x0010E03E File Offset: 0x0010C23E
			public override long Length
			{
				get
				{
					return this._originalStream.Length;
				}
			}

			// Token: 0x17000EE3 RID: 3811
			// (get) Token: 0x06004052 RID: 16466 RVA: 0x0010E04B File Offset: 0x0010C24B
			// (set) Token: 0x06004053 RID: 16467 RVA: 0x0010E058 File Offset: 0x0010C258
			public override long Position
			{
				get
				{
					return this._originalStream.Position;
				}
				set
				{
					if (value < 0L)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("PositionOutOfRange"));
					}
					this.Seek(value, SeekOrigin.Begin);
				}
			}

			// Token: 0x06004054 RID: 16468 RVA: 0x0010E07D File Offset: 0x0010C27D
			public override void Close()
			{
				this._originalStream.Close();
			}

			// Token: 0x06004055 RID: 16469 RVA: 0x0010E08A File Offset: 0x0010C28A
			protected override void Dispose(bool disposing)
			{
				if (disposing && this._originalStream != null)
				{
					((IDisposable)this._originalStream).Dispose();
					this._originalStream = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x06004056 RID: 16470 RVA: 0x0010E0B0 File Offset: 0x0010C2B0
			public override void Flush()
			{
				this._originalStream.Flush();
			}

			// Token: 0x06004057 RID: 16471 RVA: 0x0010E0C0 File Offset: 0x0010C2C0
			public override void SetLength(long value)
			{
				long length = this._originalStream.Length;
				this.EnsureQuota(Math.Max(length, value));
				this._originalStream.SetLength(value);
			}

			// Token: 0x06004058 RID: 16472 RVA: 0x0010E0F4 File Offset: 0x0010C2F4
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this._originalStream.Read(buffer, offset, count);
			}

			// Token: 0x06004059 RID: 16473 RVA: 0x0010E104 File Offset: 0x0010C304
			public override int ReadByte()
			{
				return this._originalStream.ReadByte();
			}

			// Token: 0x0600405A RID: 16474 RVA: 0x0010E114 File Offset: 0x0010C314
			public override long Seek(long offset, SeekOrigin origin)
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num;
				switch (origin)
				{
				case SeekOrigin.Begin:
					num = offset;
					break;
				case SeekOrigin.Current:
					num = this._originalStream.Position + offset;
					break;
				case SeekOrigin.End:
					num = length + offset;
					break;
				default:
					throw new ArgumentException(SR.GetString("UnknownSeekOrigin"), "origin");
				}
				this.EnsureQuota(Math.Max(length, num));
				return this._originalStream.Seek(offset, origin);
			}

			// Token: 0x0600405B RID: 16475 RVA: 0x0010E198 File Offset: 0x0010C398
			public override void Write(byte[] buffer, int offset, int count)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num = (this._originalStream.CanSeek ? (this._originalStream.Position + (long)count) : (this._originalStream.Length + (long)count));
				this.EnsureQuota(Math.Max(length, num));
				this._originalStream.Write(buffer, offset, count);
			}

			// Token: 0x0600405C RID: 16476 RVA: 0x0010E208 File Offset: 0x0010C408
			public override void WriteByte(byte value)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num = (this._originalStream.CanSeek ? (this._originalStream.Position + 1L) : (this._originalStream.Length + 1L));
				this.EnsureQuota(Math.Max(length, num));
				this._originalStream.WriteByte(value);
			}

			// Token: 0x0600405D RID: 16477 RVA: 0x0010E274 File Offset: 0x0010C474
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
			{
				return this._originalStream.BeginRead(buffer, offset, numBytes, userCallback, stateObject);
			}

			// Token: 0x0600405E RID: 16478 RVA: 0x0010E288 File Offset: 0x0010C488
			public override int EndRead(IAsyncResult asyncResult)
			{
				return this._originalStream.EndRead(asyncResult);
			}

			// Token: 0x0600405F RID: 16479 RVA: 0x0010E298 File Offset: 0x0010C498
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num = (this._originalStream.CanSeek ? (this._originalStream.Position + (long)numBytes) : (this._originalStream.Length + (long)numBytes));
				this.EnsureQuota(Math.Max(length, num));
				return this._originalStream.BeginWrite(buffer, offset, numBytes, userCallback, stateObject);
			}

			// Token: 0x06004060 RID: 16480 RVA: 0x0010E30A File Offset: 0x0010C50A
			public override void EndWrite(IAsyncResult asyncResult)
			{
				this._originalStream.EndWrite(asyncResult);
			}

			// Token: 0x06004061 RID: 16481 RVA: 0x0010E318 File Offset: 0x0010C518
			private void EnsureQuota(long size)
			{
				new IsolatedStorageFilePermission(PermissionState.None)
				{
					UserQuota = size,
					UsageAllowed = (this._isRoaming ? IsolatedStorageContainment.DomainIsolationByRoamingUser : IsolatedStorageContainment.DomainIsolationByUser)
				}.Demand();
			}

			// Token: 0x04003068 RID: 12392
			private Stream _originalStream;

			// Token: 0x04003069 RID: 12393
			private bool _isRoaming;
		}
	}
}
