using System;
using System.Configuration;
using System.Xml;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000127 RID: 295
	public sealed class ParameterElement : ConfigurationElement
	{
		// Token: 0x060011D4 RID: 4564 RVA: 0x0004AD5B File Offset: 0x00048F5B
		public ParameterElement()
		{
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0004AD6E File Offset: 0x00048F6E
		public ParameterElement(string typeName)
			: this()
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
			}
			this.Type = typeName;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004AD90 File Offset: 0x00048F90
		public ParameterElement(int index)
			: this()
		{
			this.Index = index;
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x0004AD9F File Offset: 0x00048F9F
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x0004ADB1 File Offset: 0x00048FB1
		[ConfigurationProperty("index", DefaultValue = 0)]
		[IntegerValidator(MinValue = 0)]
		public int Index
		{
			get
			{
				return (int)base["index"];
			}
			set
			{
				base["index"] = value;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x0004ADC4 File Offset: 0x00048FC4
		[ConfigurationProperty("", DefaultValue = null, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public ParameterElementCollection Parameters
		{
			get
			{
				return (ParameterElementCollection)base[""];
			}
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0004ADD6 File Offset: 0x00048FD6
		protected override void PostDeserialize()
		{
			this.Validate();
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0004ADDE File Offset: 0x00048FDE
		protected override void PreSerialize(XmlWriter writer)
		{
			this.Validate();
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x0004ADE6 File Offset: 0x00048FE6
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x0004ADF8 File Offset: 0x00048FF8
		[ConfigurationProperty("type", DefaultValue = "")]
		[StringValidator(MinLength = 0)]
		public string Type
		{
			get
			{
				return (string)base["type"];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = string.Empty;
				}
				base["type"] = value;
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004AE18 File Offset: 0x00049018
		private void Validate()
		{
			PropertyInformationCollection propertyInformationCollection = base.ElementInformation.Properties;
			if (propertyInformationCollection["index"].ValueOrigin == PropertyValueOrigin.Default && propertyInformationCollection["type"].ValueOrigin == PropertyValueOrigin.Default)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(SR.GetString("Configuration parameter element must set either type or index.")));
			}
			if (propertyInformationCollection["index"].ValueOrigin != PropertyValueOrigin.Default && propertyInformationCollection["type"].ValueOrigin != PropertyValueOrigin.Default)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(SR.GetString("Configuration parameter element can set only one of either type or index.")));
			}
			if (propertyInformationCollection["index"].ValueOrigin != PropertyValueOrigin.Default && this.Parameters.Count > 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(SR.GetString("Configuration parameter element must only add params with type.")));
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0004AED8 File Offset: 0x000490D8
		internal Type GetType(string rootType, Type[] typeArgs)
		{
			return TypeElement.GetType(rootType, typeArgs, this.Type, this.Index, this.Parameters);
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x0004AEF4 File Offset: 0x000490F4
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new ConfigurationPropertyCollection
					{
						new ConfigurationProperty("index", typeof(int), 0, null, new IntegerValidator(0, int.MaxValue, false), ConfigurationPropertyOptions.None),
						new ConfigurationProperty("", typeof(ParameterElementCollection), null, null, null, ConfigurationPropertyOptions.IsDefaultCollection),
						new ConfigurationProperty("type", typeof(string), string.Empty, null, new StringValidator(0, int.MaxValue, null), ConfigurationPropertyOptions.None)
					};
				}
				return this.properties;
			}
		}

		// Token: 0x0400089E RID: 2206
		internal readonly Guid identity = Guid.NewGuid();

		// Token: 0x0400089F RID: 2207
		private ConfigurationPropertyCollection properties;
	}
}
