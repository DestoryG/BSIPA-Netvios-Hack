using System;

namespace System.Xml
{
	// Token: 0x0200001D RID: 29
	public interface IXmlDictionary
	{
		// Token: 0x06000091 RID: 145
		bool TryLookup(string value, out XmlDictionaryString result);

		// Token: 0x06000092 RID: 146
		bool TryLookup(int key, out XmlDictionaryString result);

		// Token: 0x06000093 RID: 147
		bool TryLookup(XmlDictionaryString value, out XmlDictionaryString result);
	}
}
