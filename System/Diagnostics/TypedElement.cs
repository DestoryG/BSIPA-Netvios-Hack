using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020004BA RID: 1210
	internal class TypedElement : ConfigurationElement
	{
		// Token: 0x06002D2B RID: 11563 RVA: 0x000CB5C4 File Offset: 0x000C97C4
		public TypedElement(Type baseType)
		{
			this._properties = new ConfigurationPropertyCollection();
			this._properties.Add(TypedElement._propTypeName);
			this._properties.Add(TypedElement._propInitData);
			this._baseType = baseType;
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06002D2C RID: 11564 RVA: 0x000CB5FE File Offset: 0x000C97FE
		// (set) Token: 0x06002D2D RID: 11565 RVA: 0x000CB610 File Offset: 0x000C9810
		[ConfigurationProperty("initializeData", DefaultValue = "")]
		public string InitData
		{
			get
			{
				return (string)base[TypedElement._propInitData];
			}
			set
			{
				base[TypedElement._propInitData] = value;
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000CB61E File Offset: 0x000C981E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x000CB626 File Offset: 0x000C9826
		// (set) Token: 0x06002D30 RID: 11568 RVA: 0x000CB638 File Offset: 0x000C9838
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public virtual string TypeName
		{
			get
			{
				return (string)base[TypedElement._propTypeName];
			}
			set
			{
				base[TypedElement._propTypeName] = value;
			}
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000CB646 File Offset: 0x000C9846
		protected object BaseGetRuntimeObject()
		{
			if (this._runtimeObject == null)
			{
				this._runtimeObject = TraceUtils.GetRuntimeObject(this.TypeName, this._baseType, this.InitData);
			}
			return this._runtimeObject;
		}

		// Token: 0x04002700 RID: 9984
		protected static readonly ConfigurationProperty _propTypeName = new ConfigurationProperty("type", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsTypeStringTransformationRequired);

		// Token: 0x04002701 RID: 9985
		protected static readonly ConfigurationProperty _propInitData = new ConfigurationProperty("initializeData", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04002702 RID: 9986
		protected ConfigurationPropertyCollection _properties;

		// Token: 0x04002703 RID: 9987
		protected object _runtimeObject;

		// Token: 0x04002704 RID: 9988
		private Type _baseType;
	}
}
