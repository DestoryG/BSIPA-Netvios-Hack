using System;
using System.Collections;
using System.Configuration;
using System.Xml;

namespace System.Diagnostics
{
	// Token: 0x020004A2 RID: 1186
	internal class SourceElement : ConfigurationElement
	{
		// Token: 0x06002BED RID: 11245 RVA: 0x000C6884 File Offset: 0x000C4A84
		static SourceElement()
		{
			SourceElement._properties.Add(SourceElement._propName);
			SourceElement._properties.Add(SourceElement._propSwitchName);
			SourceElement._properties.Add(SourceElement._propSwitchValue);
			SourceElement._properties.Add(SourceElement._propSwitchType);
			SourceElement._properties.Add(SourceElement._propListeners);
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06002BEE RID: 11246 RVA: 0x000C6975 File Offset: 0x000C4B75
		public Hashtable Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					this._attributes = new Hashtable(StringComparer.OrdinalIgnoreCase);
				}
				return this._attributes;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06002BEF RID: 11247 RVA: 0x000C6995 File Offset: 0x000C4B95
		[ConfigurationProperty("listeners")]
		public ListenerElementsCollection Listeners
		{
			get
			{
				return (ListenerElementsCollection)base[SourceElement._propListeners];
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x000C69A7 File Offset: 0x000C4BA7
		[ConfigurationProperty("name", IsRequired = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[SourceElement._propName];
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x000C69B9 File Offset: 0x000C4BB9
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SourceElement._properties;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x000C69C0 File Offset: 0x000C4BC0
		[ConfigurationProperty("switchName")]
		public string SwitchName
		{
			get
			{
				return (string)base[SourceElement._propSwitchName];
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000C69D2 File Offset: 0x000C4BD2
		[ConfigurationProperty("switchValue")]
		public string SwitchValue
		{
			get
			{
				return (string)base[SourceElement._propSwitchValue];
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x000C69E4 File Offset: 0x000C4BE4
		[ConfigurationProperty("switchType")]
		public string SwitchType
		{
			get
			{
				return (string)base[SourceElement._propSwitchType];
			}
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000C69F8 File Offset: 0x000C4BF8
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			base.DeserializeElement(reader, serializeCollectionKey);
			if (!string.IsNullOrEmpty(this.SwitchName) && !string.IsNullOrEmpty(this.SwitchValue))
			{
				throw new ConfigurationErrorsException(SR.GetString("Only_specify_one", new object[] { this.Name }));
			}
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000C6A46 File Offset: 0x000C4C46
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			this.Attributes.Add(name, value);
			return true;
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000C6A58 File Offset: 0x000C4C58
		protected override void PreSerialize(XmlWriter writer)
		{
			if (this._attributes != null)
			{
				IDictionaryEnumerator enumerator = this._attributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Value;
					string text2 = (string)enumerator.Key;
					if (text != null && writer != null)
					{
						writer.WriteAttributeString(text2, text);
					}
				}
			}
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000C6AAC File Offset: 0x000C4CAC
		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			return base.SerializeElement(writer, serializeCollectionKey) || (this._attributes != null && this._attributes.Count > 0);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000C6AE4 File Offset: 0x000C4CE4
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			SourceElement sourceElement2 = sourceElement as SourceElement;
			if (sourceElement2 != null && sourceElement2._attributes != null)
			{
				this._attributes = sourceElement2._attributes;
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000C6B18 File Offset: 0x000C4D18
		internal void ResetProperties()
		{
			if (this._attributes != null)
			{
				this._attributes.Clear();
				SourceElement._properties.Clear();
				SourceElement._properties.Add(SourceElement._propName);
				SourceElement._properties.Add(SourceElement._propSwitchName);
				SourceElement._properties.Add(SourceElement._propSwitchValue);
				SourceElement._properties.Add(SourceElement._propSwitchType);
				SourceElement._properties.Add(SourceElement._propListeners);
			}
		}

		// Token: 0x04002694 RID: 9876
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04002695 RID: 9877
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04002696 RID: 9878
		private static readonly ConfigurationProperty _propSwitchName = new ConfigurationProperty("switchName", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002697 RID: 9879
		private static readonly ConfigurationProperty _propSwitchValue = new ConfigurationProperty("switchValue", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002698 RID: 9880
		private static readonly ConfigurationProperty _propSwitchType = new ConfigurationProperty("switchType", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002699 RID: 9881
		private static readonly ConfigurationProperty _propListeners = new ConfigurationProperty("listeners", typeof(ListenerElementsCollection), new ListenerElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x0400269A RID: 9882
		private Hashtable _attributes;
	}
}
