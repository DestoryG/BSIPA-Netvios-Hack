using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020000BA RID: 186
	public sealed class SettingValueElement : ConfigurationElement
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00024057 File Offset: 0x00022257
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (SettingValueElement._properties == null)
				{
					SettingValueElement._properties = new ConfigurationPropertyCollection();
				}
				return SettingValueElement._properties;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00024075 File Offset: 0x00022275
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x0002407D File Offset: 0x0002227D
		public XmlNode ValueXml
		{
			get
			{
				return this._valueXml;
			}
			set
			{
				this._valueXml = value;
				this.isModified = true;
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002408D File Offset: 0x0002228D
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.ValueXml = SettingValueElement.doc.ReadNode(reader);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000240A0 File Offset: 0x000222A0
		public override bool Equals(object settingValue)
		{
			SettingValueElement settingValueElement = settingValue as SettingValueElement;
			return settingValueElement != null && object.Equals(settingValueElement.ValueXml, this.ValueXml);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x000240CA File Offset: 0x000222CA
		public override int GetHashCode()
		{
			return this.ValueXml.GetHashCode();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x000240D7 File Offset: 0x000222D7
		protected override bool IsModified()
		{
			return this.isModified;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000240DF File Offset: 0x000222DF
		protected override void ResetModified()
		{
			this.isModified = false;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000240E8 File Offset: 0x000222E8
		protected override bool SerializeToXmlElement(XmlWriter writer, string elementName)
		{
			if (this.ValueXml != null)
			{
				if (writer != null)
				{
					this.ValueXml.WriteTo(writer);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00024104 File Offset: 0x00022304
		protected override void Reset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
			this.ValueXml = ((SettingValueElement)parentElement).ValueXml;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0002411E File Offset: 0x0002231E
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			this.ValueXml = ((SettingValueElement)sourceElement).ValueXml;
		}

		// Token: 0x04000C6A RID: 3178
		private static volatile ConfigurationPropertyCollection _properties;

		// Token: 0x04000C6B RID: 3179
		private static XmlDocument doc = new XmlDocument();

		// Token: 0x04000C6C RID: 3180
		private XmlNode _valueXml;

		// Token: 0x04000C6D RID: 3181
		private bool isModified;
	}
}
