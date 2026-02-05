using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200008E RID: 142
	internal class ElementData
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x0002BD1C File Offset: 0x00029F1C
		public void AddAttribute(string prefix, string ns, string name, string value)
		{
			this.GrowAttributesIfNeeded();
			AttributeData attributeData = this.attributes[this.attributeCount];
			if (attributeData == null)
			{
				attributeData = (this.attributes[this.attributeCount] = new AttributeData());
			}
			attributeData.prefix = prefix;
			attributeData.ns = ns;
			attributeData.localName = name;
			attributeData.value = value;
			this.attributeCount++;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0002BD80 File Offset: 0x00029F80
		private void GrowAttributesIfNeeded()
		{
			if (this.attributes == null)
			{
				this.attributes = new AttributeData[4];
				return;
			}
			if (this.attributes.Length == this.attributeCount)
			{
				AttributeData[] array = new AttributeData[this.attributes.Length * 2];
				Array.Copy(this.attributes, 0, array, 0, this.attributes.Length);
				this.attributes = array;
			}
		}

		// Token: 0x040003CB RID: 971
		public string localName;

		// Token: 0x040003CC RID: 972
		public string ns;

		// Token: 0x040003CD RID: 973
		public string prefix;

		// Token: 0x040003CE RID: 974
		public int attributeCount;

		// Token: 0x040003CF RID: 975
		public AttributeData[] attributes;

		// Token: 0x040003D0 RID: 976
		public IDataNode dataNode;

		// Token: 0x040003D1 RID: 977
		public int childElementIndex;
	}
}
