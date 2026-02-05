using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x020000A7 RID: 167
	public abstract class SettingsBase
	{
		// Token: 0x060005AC RID: 1452 RVA: 0x00022AF3 File Offset: 0x00020CF3
		protected SettingsBase()
		{
			this._PropertyValues = new SettingsPropertyValueCollection();
		}

		// Token: 0x170000DE RID: 222
		public virtual object this[string propertyName]
		{
			get
			{
				if (this.IsSynchronized)
				{
					lock (this)
					{
						return this.GetPropertyValueByName(propertyName);
					}
				}
				return this.GetPropertyValueByName(propertyName);
			}
			set
			{
				if (this.IsSynchronized)
				{
					lock (this)
					{
						this.SetPropertyValueByName(propertyName, value);
						return;
					}
				}
				this.SetPropertyValueByName(propertyName, value);
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00022BA8 File Offset: 0x00020DA8
		private object GetPropertyValueByName(string propertyName)
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			SettingsPropertyValue settingsPropertyValue = this._PropertyValues[propertyName];
			if (settingsPropertyValue == null)
			{
				this.GetPropertiesFromProvider(settingsProperty.Provider);
				settingsPropertyValue = this._PropertyValues[propertyName];
				if (settingsPropertyValue == null)
				{
					throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
				}
			}
			return settingsPropertyValue.PropertyValue;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00022C64 File Offset: 0x00020E64
		private void SetPropertyValueByName(string propertyName, object propertyValue)
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			if (settingsProperty.IsReadOnly)
			{
				throw new SettingsPropertyIsReadOnlyException(SR.GetString("SettingsPropertyReadOnly", new object[] { propertyName }));
			}
			if (propertyValue != null && !settingsProperty.PropertyType.IsInstanceOfType(propertyValue))
			{
				throw new SettingsPropertyWrongTypeException(SR.GetString("SettingsPropertyWrongType", new object[] { propertyName }));
			}
			SettingsPropertyValue settingsPropertyValue = this._PropertyValues[propertyName];
			if (settingsPropertyValue == null)
			{
				this.GetPropertiesFromProvider(settingsProperty.Provider);
				settingsPropertyValue = this._PropertyValues[propertyName];
				if (settingsPropertyValue == null)
				{
					throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
				}
			}
			settingsPropertyValue.PropertyValue = propertyValue;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00022D6C File Offset: 0x00020F6C
		public void Initialize(SettingsContext context, SettingsPropertyCollection properties, SettingsProviderCollection providers)
		{
			this._Context = context;
			this._Properties = properties;
			this._Providers = providers;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00022D84 File Offset: 0x00020F84
		public virtual void Save()
		{
			if (this.IsSynchronized)
			{
				lock (this)
				{
					this.SaveCore();
					return;
				}
			}
			this.SaveCore();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00022DD0 File Offset: 0x00020FD0
		private void SaveCore()
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				return;
			}
			foreach (object obj in this.Providers)
			{
				SettingsProvider settingsProvider = (SettingsProvider)obj;
				SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
				foreach (object obj2 in this.PropertyValues)
				{
					SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj2;
					if (settingsPropertyValue.Property.Provider == settingsProvider)
					{
						settingsPropertyValueCollection.Add(settingsPropertyValue);
					}
				}
				if (settingsPropertyValueCollection.Count > 0)
				{
					settingsProvider.SetPropertyValues(this.Context, settingsPropertyValueCollection);
				}
			}
			foreach (object obj3 in this.PropertyValues)
			{
				SettingsPropertyValue settingsPropertyValue2 = (SettingsPropertyValue)obj3;
				settingsPropertyValue2.IsDirty = false;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00022F0C File Offset: 0x0002110C
		public virtual SettingsPropertyCollection Properties
		{
			get
			{
				return this._Properties;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00022F14 File Offset: 0x00021114
		public virtual SettingsProviderCollection Providers
		{
			get
			{
				return this._Providers;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00022F1C File Offset: 0x0002111C
		public virtual SettingsPropertyValueCollection PropertyValues
		{
			get
			{
				return this._PropertyValues;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00022F24 File Offset: 0x00021124
		public virtual SettingsContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00022F2C File Offset: 0x0002112C
		private void GetPropertiesFromProvider(SettingsProvider provider)
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
			if (settingsPropertyCollection.Count > 0)
			{
				SettingsPropertyValueCollection propertyValues = provider.GetPropertyValues(this.Context, settingsPropertyCollection);
				foreach (object obj2 in propertyValues)
				{
					SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj2;
					if (this._PropertyValues[settingsPropertyValue.Name] == null)
					{
						this._PropertyValues.Add(settingsPropertyValue);
					}
				}
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00023010 File Offset: 0x00021210
		public static SettingsBase Synchronized(SettingsBase settingsBase)
		{
			settingsBase._IsSynchronized = true;
			return settingsBase;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0002301A File Offset: 0x0002121A
		[Browsable(false)]
		public bool IsSynchronized
		{
			get
			{
				return this._IsSynchronized;
			}
		}

		// Token: 0x04000C44 RID: 3140
		private SettingsPropertyCollection _Properties;

		// Token: 0x04000C45 RID: 3141
		private SettingsProviderCollection _Providers;

		// Token: 0x04000C46 RID: 3142
		private SettingsPropertyValueCollection _PropertyValues;

		// Token: 0x04000C47 RID: 3143
		private SettingsContext _Context;

		// Token: 0x04000C48 RID: 3144
		private bool _IsSynchronized;
	}
}
