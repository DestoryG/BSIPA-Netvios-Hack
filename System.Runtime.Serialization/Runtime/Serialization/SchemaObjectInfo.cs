using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Runtime.Serialization
{
	// Token: 0x020000D1 RID: 209
	internal class SchemaObjectInfo
	{
		// Token: 0x06000BCA RID: 3018 RVA: 0x00030BD4 File Offset: 0x0002EDD4
		internal SchemaObjectInfo(XmlSchemaType type, XmlSchemaElement element, XmlSchema schema, List<XmlSchemaType> knownTypes)
		{
			this.type = type;
			this.element = element;
			this.schema = schema;
			this.knownTypes = knownTypes;
		}

		// Token: 0x040004E0 RID: 1248
		internal XmlSchemaType type;

		// Token: 0x040004E1 RID: 1249
		internal XmlSchemaElement element;

		// Token: 0x040004E2 RID: 1250
		internal XmlSchema schema;

		// Token: 0x040004E3 RID: 1251
		internal List<XmlSchemaType> knownTypes;
	}
}
