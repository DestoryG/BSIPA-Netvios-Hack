using System;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x0200010A RID: 266
	// (Invoke) Token: 0x06001029 RID: 4137
	internal delegate void JsonFormatClassWriterDelegate(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, ClassDataContract dataContract, XmlDictionaryString[] memberNames);
}
