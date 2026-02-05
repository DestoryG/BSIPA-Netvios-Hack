using System;

namespace System.Configuration
{
	// Token: 0x020000A9 RID: 169
	public class SettingsProperty
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0002302A File Offset: 0x0002122A
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x00023032 File Offset: 0x00021232
		public virtual string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0002303B File Offset: 0x0002123B
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x00023043 File Offset: 0x00021243
		public virtual bool IsReadOnly
		{
			get
			{
				return this._IsReadOnly;
			}
			set
			{
				this._IsReadOnly = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0002304C File Offset: 0x0002124C
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x00023054 File Offset: 0x00021254
		public virtual object DefaultValue
		{
			get
			{
				return this._DefaultValue;
			}
			set
			{
				this._DefaultValue = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0002305D File Offset: 0x0002125D
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00023065 File Offset: 0x00021265
		public virtual Type PropertyType
		{
			get
			{
				return this._PropertyType;
			}
			set
			{
				this._PropertyType = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0002306E File Offset: 0x0002126E
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00023076 File Offset: 0x00021276
		public virtual SettingsSerializeAs SerializeAs
		{
			get
			{
				return this._SerializeAs;
			}
			set
			{
				this._SerializeAs = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0002307F File Offset: 0x0002127F
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x00023087 File Offset: 0x00021287
		public virtual SettingsProvider Provider
		{
			get
			{
				return this._Provider;
			}
			set
			{
				this._Provider = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00023090 File Offset: 0x00021290
		public virtual SettingsAttributeDictionary Attributes
		{
			get
			{
				return this._Attributes;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00023098 File Offset: 0x00021298
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x000230A0 File Offset: 0x000212A0
		public bool ThrowOnErrorDeserializing
		{
			get
			{
				return this._ThrowOnErrorDeserializing;
			}
			set
			{
				this._ThrowOnErrorDeserializing = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x000230A9 File Offset: 0x000212A9
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x000230B1 File Offset: 0x000212B1
		public bool ThrowOnErrorSerializing
		{
			get
			{
				return this._ThrowOnErrorSerializing;
			}
			set
			{
				this._ThrowOnErrorSerializing = value;
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000230BA File Offset: 0x000212BA
		public SettingsProperty(string name)
		{
			this._Name = name;
			this._Attributes = new SettingsAttributeDictionary();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000230D4 File Offset: 0x000212D4
		public SettingsProperty(string name, Type propertyType, SettingsProvider provider, bool isReadOnly, object defaultValue, SettingsSerializeAs serializeAs, SettingsAttributeDictionary attributes, bool throwOnErrorDeserializing, bool throwOnErrorSerializing)
		{
			this._Name = name;
			this._PropertyType = propertyType;
			this._Provider = provider;
			this._IsReadOnly = isReadOnly;
			this._DefaultValue = defaultValue;
			this._SerializeAs = serializeAs;
			this._Attributes = attributes;
			this._ThrowOnErrorDeserializing = throwOnErrorDeserializing;
			this._ThrowOnErrorSerializing = throwOnErrorSerializing;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0002312C File Offset: 0x0002132C
		public SettingsProperty(SettingsProperty propertyToCopy)
		{
			this._Name = propertyToCopy.Name;
			this._IsReadOnly = propertyToCopy.IsReadOnly;
			this._DefaultValue = propertyToCopy.DefaultValue;
			this._SerializeAs = propertyToCopy.SerializeAs;
			this._Provider = propertyToCopy.Provider;
			this._PropertyType = propertyToCopy.PropertyType;
			this._ThrowOnErrorDeserializing = propertyToCopy.ThrowOnErrorDeserializing;
			this._ThrowOnErrorSerializing = propertyToCopy.ThrowOnErrorSerializing;
			this._Attributes = new SettingsAttributeDictionary(propertyToCopy.Attributes);
		}

		// Token: 0x04000C49 RID: 3145
		private string _Name;

		// Token: 0x04000C4A RID: 3146
		private bool _IsReadOnly;

		// Token: 0x04000C4B RID: 3147
		private object _DefaultValue;

		// Token: 0x04000C4C RID: 3148
		private SettingsSerializeAs _SerializeAs;

		// Token: 0x04000C4D RID: 3149
		private SettingsProvider _Provider;

		// Token: 0x04000C4E RID: 3150
		private SettingsAttributeDictionary _Attributes;

		// Token: 0x04000C4F RID: 3151
		private Type _PropertyType;

		// Token: 0x04000C50 RID: 3152
		private bool _ThrowOnErrorDeserializing;

		// Token: 0x04000C51 RID: 3153
		private bool _ThrowOnErrorSerializing;
	}
}
