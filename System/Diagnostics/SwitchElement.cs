using System;
using System.Collections;
using System.Configuration;
using System.Xml;

namespace System.Diagnostics
{
	// Token: 0x020004A9 RID: 1193
	internal class SwitchElement : ConfigurationElement
	{
		// Token: 0x06002C28 RID: 11304 RVA: 0x000C7368 File Offset: 0x000C5568
		static SwitchElement()
		{
			SwitchElement._properties.Add(SwitchElement._propName);
			SwitchElement._properties.Add(SwitchElement._propValue);
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06002C29 RID: 11305 RVA: 0x000C73D7 File Offset: 0x000C55D7
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

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x000C73F7 File Offset: 0x000C55F7
		[ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[SwitchElement._propName];
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x000C7409 File Offset: 0x000C5609
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SwitchElement._properties;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x000C7410 File Offset: 0x000C5610
		[ConfigurationProperty("value", IsRequired = true)]
		public string Value
		{
			get
			{
				return (string)base[SwitchElement._propValue];
			}
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000C7422 File Offset: 0x000C5622
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			this.Attributes.Add(name, value);
			return true;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000C7434 File Offset: 0x000C5634
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

		// Token: 0x06002C2F RID: 11311 RVA: 0x000C7488 File Offset: 0x000C5688
		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			return base.SerializeElement(writer, serializeCollectionKey) || (this._attributes != null && this._attributes.Count > 0);
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000C74C0 File Offset: 0x000C56C0
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			SwitchElement switchElement = sourceElement as SwitchElement;
			if (switchElement != null && switchElement._attributes != null)
			{
				this._attributes = switchElement._attributes;
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000C74F4 File Offset: 0x000C56F4
		internal void ResetProperties()
		{
			if (this._attributes != null)
			{
				this._attributes.Clear();
				SwitchElement._properties.Clear();
				SwitchElement._properties.Add(SwitchElement._propName);
				SwitchElement._properties.Add(SwitchElement._propValue);
			}
		}

		// Token: 0x040026B3 RID: 9907
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040026B4 RID: 9908
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040026B5 RID: 9909
		private static readonly ConfigurationProperty _propValue = new ConfigurationProperty("value", typeof(string), null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040026B6 RID: 9910
		private Hashtable _attributes;
	}
}
