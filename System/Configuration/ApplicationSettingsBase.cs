using System;
using System.Collections;
using System.ComponentModel;
using System.Deployment.Internal;
using System.Reflection;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000079 RID: 121
	public abstract class ApplicationSettingsBase : SettingsBase, INotifyPropertyChanged
	{
		// Token: 0x060004CD RID: 1229 RVA: 0x0001FFA1 File Offset: 0x0001E1A1
		protected ApplicationSettingsBase()
		{
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001FFBB File Offset: 0x0001E1BB
		protected ApplicationSettingsBase(IComponent owner)
			: this(owner, string.Empty)
		{
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001FFC9 File Offset: 0x0001E1C9
		protected ApplicationSettingsBase(string settingsKey)
		{
			this._settingsKey = settingsKey;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001FFEC File Offset: 0x0001E1EC
		protected ApplicationSettingsBase(IComponent owner, string settingsKey)
			: this(settingsKey)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			if (owner.Site != null)
			{
				ISettingsProviderService settingsProviderService = owner.Site.GetService(typeof(ISettingsProviderService)) as ISettingsProviderService;
				if (settingsProviderService != null)
				{
					foreach (object obj in this.Properties)
					{
						SettingsProperty settingsProperty = (SettingsProperty)obj;
						SettingsProvider settingsProvider = settingsProviderService.GetSettingsProvider(settingsProperty);
						if (settingsProvider != null)
						{
							settingsProperty.Provider = settingsProvider;
						}
					}
					this.ResetProviders();
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0002009C File Offset: 0x0001E29C
		[Browsable(false)]
		public override SettingsContext Context
		{
			get
			{
				if (this._context == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._context == null)
							{
								this._context = new SettingsContext();
								this.EnsureInitialized();
							}
							goto IL_0052;
						}
					}
					this._context = new SettingsContext();
					this.EnsureInitialized();
				}
				IL_0052:
				return this._context;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00020114 File Offset: 0x0001E314
		[Browsable(false)]
		public override SettingsPropertyCollection Properties
		{
			get
			{
				if (this._settings == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._settings == null)
							{
								this._settings = new SettingsPropertyCollection();
								this.EnsureInitialized();
							}
							goto IL_0052;
						}
					}
					this._settings = new SettingsPropertyCollection();
					this.EnsureInitialized();
				}
				IL_0052:
				return this._settings;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0002018C File Offset: 0x0001E38C
		[Browsable(false)]
		public override SettingsPropertyValueCollection PropertyValues
		{
			get
			{
				return base.PropertyValues;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00020194 File Offset: 0x0001E394
		[Browsable(false)]
		public override SettingsProviderCollection Providers
		{
			get
			{
				if (this._providers == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._providers == null)
							{
								this._providers = new SettingsProviderCollection();
								this.EnsureInitialized();
							}
							goto IL_0052;
						}
					}
					this._providers = new SettingsProviderCollection();
					this.EnsureInitialized();
				}
				IL_0052:
				return this._providers;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0002020C File Offset: 0x0001E40C
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00020214 File Offset: 0x0001E414
		[Browsable(false)]
		public string SettingsKey
		{
			get
			{
				return this._settingsKey;
			}
			set
			{
				this._settingsKey = value;
				this.Context["SettingsKey"] = this._settingsKey;
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060004D7 RID: 1239 RVA: 0x00020233 File Offset: 0x0001E433
		// (remove) Token: 0x060004D8 RID: 1240 RVA: 0x0002024C File Offset: 0x0001E44C
		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				this._onPropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(this._onPropertyChanged, value);
			}
			remove
			{
				this._onPropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(this._onPropertyChanged, value);
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060004D9 RID: 1241 RVA: 0x00020265 File Offset: 0x0001E465
		// (remove) Token: 0x060004DA RID: 1242 RVA: 0x0002027E File Offset: 0x0001E47E
		public event SettingChangingEventHandler SettingChanging
		{
			add
			{
				this._onSettingChanging = (SettingChangingEventHandler)Delegate.Combine(this._onSettingChanging, value);
			}
			remove
			{
				this._onSettingChanging = (SettingChangingEventHandler)Delegate.Remove(this._onSettingChanging, value);
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060004DB RID: 1243 RVA: 0x00020297 File Offset: 0x0001E497
		// (remove) Token: 0x060004DC RID: 1244 RVA: 0x000202B0 File Offset: 0x0001E4B0
		public event SettingsLoadedEventHandler SettingsLoaded
		{
			add
			{
				this._onSettingsLoaded = (SettingsLoadedEventHandler)Delegate.Combine(this._onSettingsLoaded, value);
			}
			remove
			{
				this._onSettingsLoaded = (SettingsLoadedEventHandler)Delegate.Remove(this._onSettingsLoaded, value);
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060004DD RID: 1245 RVA: 0x000202C9 File Offset: 0x0001E4C9
		// (remove) Token: 0x060004DE RID: 1246 RVA: 0x000202E2 File Offset: 0x0001E4E2
		public event SettingsSavingEventHandler SettingsSaving
		{
			add
			{
				this._onSettingsSaving = (SettingsSavingEventHandler)Delegate.Combine(this._onSettingsSaving, value);
			}
			remove
			{
				this._onSettingsSaving = (SettingsSavingEventHandler)Delegate.Remove(this._onSettingsSaving, value);
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000202FC File Offset: 0x0001E4FC
		public object GetPreviousVersion(string propertyName)
		{
			if (this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException();
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			SettingsPropertyValue settingsPropertyValue = null;
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException();
			}
			IApplicationSettingsProvider applicationSettingsProvider = settingsProperty.Provider as IApplicationSettingsProvider;
			if (applicationSettingsProvider != null)
			{
				settingsPropertyValue = applicationSettingsProvider.GetPreviousVersion(this.Context, settingsProperty);
			}
			if (settingsPropertyValue != null)
			{
				return settingsPropertyValue.PropertyValue;
			}
			return null;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0002035C File Offset: 0x0001E55C
		protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this._onPropertyChanged != null)
			{
				this._onPropertyChanged(this, e);
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00020373 File Offset: 0x0001E573
		protected virtual void OnSettingChanging(object sender, SettingChangingEventArgs e)
		{
			if (this._onSettingChanging != null)
			{
				this._onSettingChanging(this, e);
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0002038A File Offset: 0x0001E58A
		protected virtual void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
		{
			if (this._onSettingsLoaded != null)
			{
				this._onSettingsLoaded(this, e);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000203A1 File Offset: 0x0001E5A1
		protected virtual void OnSettingsSaving(object sender, CancelEventArgs e)
		{
			if (this._onSettingsSaving != null)
			{
				this._onSettingsSaving(this, e);
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x000203B8 File Offset: 0x0001E5B8
		public void Reload()
		{
			if (this.PropertyValues != null)
			{
				this.PropertyValues.Clear();
			}
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(settingsProperty.Name);
				this.OnPropertyChanged(this, propertyChangedEventArgs);
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00020434 File Offset: 0x0001E634
		public void Reset()
		{
			if (this.Properties != null)
			{
				foreach (object obj in this.Providers)
				{
					SettingsProvider settingsProvider = (SettingsProvider)obj;
					IApplicationSettingsProvider applicationSettingsProvider = settingsProvider as IApplicationSettingsProvider;
					if (applicationSettingsProvider != null)
					{
						applicationSettingsProvider.Reset(this.Context);
					}
				}
			}
			this.Reload();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000204AC File Offset: 0x0001E6AC
		public override void Save()
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
			this.OnSettingsSaving(this, cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				base.Save();
			}
		}

		// Token: 0x170000AD RID: 173
		public override object this[string propertyName]
		{
			get
			{
				if (base.IsSynchronized)
				{
					lock (this)
					{
						return this.GetPropertyValue(propertyName);
					}
				}
				return this.GetPropertyValue(propertyName);
			}
			set
			{
				SettingChangingEventArgs settingChangingEventArgs = new SettingChangingEventArgs(propertyName, base.GetType().FullName, this.SettingsKey, value, false);
				this.OnSettingChanging(this, settingChangingEventArgs);
				if (!settingChangingEventArgs.Cancel)
				{
					base[propertyName] = value;
					PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
					this.OnPropertyChanged(this, propertyChangedEventArgs);
				}
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00020578 File Offset: 0x0001E778
		public virtual void Upgrade()
		{
			if (this.Properties != null)
			{
				foreach (object obj in this.Providers)
				{
					SettingsProvider settingsProvider = (SettingsProvider)obj;
					IApplicationSettingsProvider applicationSettingsProvider = settingsProvider as IApplicationSettingsProvider;
					if (applicationSettingsProvider != null)
					{
						applicationSettingsProvider.Upgrade(this.Context, this.GetPropertiesForProvider(settingsProvider));
					}
				}
			}
			this.Reload();
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000205F8 File Offset: 0x0001E7F8
		private SettingsProperty CreateSetting(PropertyInfo propInfo)
		{
			object[] customAttributes = propInfo.GetCustomAttributes(false);
			SettingsProperty settingsProperty = new SettingsProperty(this.Initializer);
			bool flag = this._explicitSerializeOnClass;
			settingsProperty.Name = propInfo.Name;
			settingsProperty.PropertyType = propInfo.PropertyType;
			for (int i = 0; i < customAttributes.Length; i++)
			{
				Attribute attribute = customAttributes[i] as Attribute;
				if (attribute != null)
				{
					if (attribute is DefaultSettingValueAttribute)
					{
						settingsProperty.DefaultValue = ((DefaultSettingValueAttribute)attribute).Value;
					}
					else if (attribute is ReadOnlyAttribute)
					{
						settingsProperty.IsReadOnly = true;
					}
					else if (attribute is SettingsProviderAttribute)
					{
						string providerTypeName = ((SettingsProviderAttribute)attribute).ProviderTypeName;
						Type type = Type.GetType(providerTypeName);
						if (!(type != null))
						{
							throw new ConfigurationErrorsException(SR.GetString("ProviderTypeLoadFailed", new object[] { providerTypeName }));
						}
						SettingsProvider settingsProvider = SecurityUtils.SecureCreateInstance(type) as SettingsProvider;
						if (settingsProvider == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("ProviderInstantiationFailed", new object[] { providerTypeName }));
						}
						settingsProvider.Initialize(null, null);
						settingsProvider.ApplicationName = ConfigurationManagerInternalFactory.Instance.ExeProductName;
						SettingsProvider settingsProvider2 = this._providers[settingsProvider.Name];
						if (settingsProvider2 != null)
						{
							settingsProvider = settingsProvider2;
						}
						settingsProperty.Provider = settingsProvider;
					}
					else if (attribute is SettingsSerializeAsAttribute)
					{
						settingsProperty.SerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
						flag = true;
					}
					else
					{
						settingsProperty.Attributes.Add(attribute.GetType(), attribute);
					}
				}
			}
			if (!flag)
			{
				settingsProperty.SerializeAs = this.GetSerializeAs(propInfo.PropertyType);
			}
			return settingsProperty;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00020790 File Offset: 0x0001E990
		private void EnsureInitialized()
		{
			if (!this._initialized)
			{
				this._initialized = true;
				Type type = base.GetType();
				if (this._context == null)
				{
					this._context = new SettingsContext();
				}
				this._context["GroupName"] = type.FullName;
				this._context["SettingsKey"] = this.SettingsKey;
				this._context["SettingsClassType"] = type;
				PropertyInfo[] array = this.SettingsFilter(type.GetProperties(BindingFlags.Instance | BindingFlags.Public));
				this._classAttributes = type.GetCustomAttributes(false);
				if (this._settings == null)
				{
					this._settings = new SettingsPropertyCollection();
				}
				if (this._providers == null)
				{
					this._providers = new SettingsProviderCollection();
				}
				for (int i = 0; i < array.Length; i++)
				{
					SettingsProperty settingsProperty = this.CreateSetting(array[i]);
					if (settingsProperty != null)
					{
						this._settings.Add(settingsProperty);
						if (settingsProperty.Provider != null && this._providers[settingsProperty.Provider.Name] == null)
						{
							this._providers.Add(settingsProperty.Provider);
						}
					}
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x000208A0 File Offset: 0x0001EAA0
		private SettingsProperty Initializer
		{
			get
			{
				if (this._init == null)
				{
					this._init = new SettingsProperty("");
					this._init.DefaultValue = null;
					this._init.IsReadOnly = false;
					this._init.PropertyType = null;
					SettingsProvider settingsProvider = new LocalFileSettingsProvider();
					if (this._classAttributes != null)
					{
						for (int i = 0; i < this._classAttributes.Length; i++)
						{
							Attribute attribute = this._classAttributes[i] as Attribute;
							if (attribute != null)
							{
								if (attribute is ReadOnlyAttribute)
								{
									this._init.IsReadOnly = true;
								}
								else if (attribute is SettingsGroupNameAttribute)
								{
									if (this._context == null)
									{
										this._context = new SettingsContext();
									}
									this._context["GroupName"] = ((SettingsGroupNameAttribute)attribute).GroupName;
								}
								else if (attribute is SettingsProviderAttribute)
								{
									string providerTypeName = ((SettingsProviderAttribute)attribute).ProviderTypeName;
									Type type = Type.GetType(providerTypeName);
									if (!(type != null))
									{
										throw new ConfigurationErrorsException(SR.GetString("ProviderTypeLoadFailed", new object[] { providerTypeName }));
									}
									SettingsProvider settingsProvider2 = SecurityUtils.SecureCreateInstance(type) as SettingsProvider;
									if (settingsProvider2 == null)
									{
										throw new ConfigurationErrorsException(SR.GetString("ProviderInstantiationFailed", new object[] { providerTypeName }));
									}
									settingsProvider = settingsProvider2;
								}
								else if (attribute is SettingsSerializeAsAttribute)
								{
									this._init.SerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
									this._explicitSerializeOnClass = true;
								}
								else
								{
									this._init.Attributes.Add(attribute.GetType(), attribute);
								}
							}
						}
					}
					settingsProvider.Initialize(null, null);
					settingsProvider.ApplicationName = ConfigurationManagerInternalFactory.Instance.ExeProductName;
					this._init.Provider = settingsProvider;
				}
				return this._init;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00020A58 File Offset: 0x0001EC58
		private SettingsPropertyCollection GetPropertiesForProvider(SettingsProvider provider)
		{
			SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				if (settingsProperty.Provider == provider)
				{
					settingsPropertyCollection.Add(settingsProperty);
				}
			}
			return settingsPropertyCollection;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00020AC4 File Offset: 0x0001ECC4
		private object GetPropertyValue(string propertyName)
		{
			if (this.PropertyValues[propertyName] == null)
			{
				if (this._firstLoad)
				{
					this._firstLoad = false;
					if (this.IsFirstRunOfClickOnceApp())
					{
						this.Upgrade();
					}
				}
				object obj = base[propertyName];
				SettingsProperty settingsProperty = this.Properties[propertyName];
				SettingsProvider settingsProvider = ((settingsProperty != null) ? settingsProperty.Provider : null);
				SettingsLoadedEventArgs settingsLoadedEventArgs = new SettingsLoadedEventArgs(settingsProvider);
				this.OnSettingsLoaded(this, settingsLoadedEventArgs);
				return base[propertyName];
			}
			return base[propertyName];
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00020B3C File Offset: 0x0001ED3C
		private SettingsSerializeAs GetSerializeAs(Type type)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			bool flag = converter.CanConvertTo(typeof(string));
			bool flag2 = converter.CanConvertFrom(typeof(string));
			if (flag && flag2)
			{
				return SettingsSerializeAs.String;
			}
			return SettingsSerializeAs.Xml;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00020B7C File Offset: 0x0001ED7C
		private bool IsFirstRunOfClickOnceApp()
		{
			ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
			return ApplicationSettingsBase.IsClickOnceDeployed(AppDomain.CurrentDomain) && InternalActivationContextHelper.IsFirstRun(activationContext);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00020BA8 File Offset: 0x0001EDA8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal static bool IsClickOnceDeployed(AppDomain appDomain)
		{
			ActivationContext activationContext = appDomain.ActivationContext;
			if (activationContext != null && activationContext.Form == ActivationContext.ContextForm.StoreBounded)
			{
				string fullName = activationContext.Identity.FullName;
				if (!string.IsNullOrEmpty(fullName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00020BE0 File Offset: 0x0001EDE0
		private PropertyInfo[] SettingsFilter(PropertyInfo[] allProps)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < allProps.Length; i++)
			{
				object[] customAttributes = allProps[i].GetCustomAttributes(false);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					Attribute attribute = customAttributes[j] as Attribute;
					if (attribute is SettingAttribute)
					{
						arrayList.Add(allProps[i]);
						break;
					}
				}
			}
			return (PropertyInfo[])arrayList.ToArray(typeof(PropertyInfo));
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00020C50 File Offset: 0x0001EE50
		private void ResetProviders()
		{
			this.Providers.Clear();
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				if (this.Providers[settingsProperty.Provider.Name] == null)
				{
					this.Providers.Add(settingsProperty.Provider);
				}
			}
		}

		// Token: 0x04000C01 RID: 3073
		private bool _explicitSerializeOnClass;

		// Token: 0x04000C02 RID: 3074
		private object[] _classAttributes;

		// Token: 0x04000C03 RID: 3075
		private IComponent _owner;

		// Token: 0x04000C04 RID: 3076
		private PropertyChangedEventHandler _onPropertyChanged;

		// Token: 0x04000C05 RID: 3077
		private SettingsContext _context;

		// Token: 0x04000C06 RID: 3078
		private SettingsProperty _init;

		// Token: 0x04000C07 RID: 3079
		private SettingsPropertyCollection _settings;

		// Token: 0x04000C08 RID: 3080
		private SettingsProviderCollection _providers;

		// Token: 0x04000C09 RID: 3081
		private SettingChangingEventHandler _onSettingChanging;

		// Token: 0x04000C0A RID: 3082
		private SettingsLoadedEventHandler _onSettingsLoaded;

		// Token: 0x04000C0B RID: 3083
		private SettingsSavingEventHandler _onSettingsSaving;

		// Token: 0x04000C0C RID: 3084
		private string _settingsKey = string.Empty;

		// Token: 0x04000C0D RID: 3085
		private bool _firstLoad = true;

		// Token: 0x04000C0E RID: 3086
		private bool _initialized;
	}
}
