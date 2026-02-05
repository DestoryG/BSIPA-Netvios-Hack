using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization
{
	// Token: 0x020000ED RID: 237
	public static class XmlSerializableServices
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x0003BE24 File Offset: 0x0003A024
		public static XmlNode[] ReadNodes(XmlReader xmlReader)
		{
			if (xmlReader == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("xmlReader");
			}
			XmlDocument xmlDocument = new XmlDocument();
			List<XmlNode> list = new List<XmlNode>();
			if (xmlReader.MoveToFirstAttribute())
			{
				for (;;)
				{
					if (XmlSerializableServices.IsValidAttribute(xmlReader))
					{
						XmlNode xmlNode = xmlDocument.ReadNode(xmlReader);
						if (xmlNode == null)
						{
							break;
						}
						list.Add(xmlNode);
					}
					if (!xmlReader.MoveToNextAttribute())
					{
						goto IL_0059;
					}
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unexpected end of file.")));
			}
			IL_0059:
			xmlReader.MoveToElement();
			if (!xmlReader.IsEmptyElement)
			{
				int depth = xmlReader.Depth;
				xmlReader.Read();
				while (xmlReader.Depth > depth && xmlReader.NodeType != XmlNodeType.EndElement)
				{
					XmlNode xmlNode2 = xmlDocument.ReadNode(xmlReader);
					if (xmlNode2 == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unexpected end of file.")));
					}
					list.Add(xmlNode2);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0003BEEC File Offset: 0x0003A0EC
		private static bool IsValidAttribute(XmlReader xmlReader)
		{
			return xmlReader.NamespaceURI != "http://schemas.microsoft.com/2003/10/Serialization/" && xmlReader.NamespaceURI != "http://www.w3.org/2001/XMLSchema-instance" && xmlReader.Prefix != "xmlns" && xmlReader.LocalName != "xmlns";
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0003BF44 File Offset: 0x0003A144
		public static void WriteNodes(XmlWriter xmlWriter, XmlNode[] nodes)
		{
			if (xmlWriter == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("xmlWriter");
			}
			if (nodes != null)
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					if (nodes[i] != null)
					{
						nodes[i].WriteTo(xmlWriter);
					}
				}
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0003BF7E File Offset: 0x0003A17E
		public static void AddDefaultSchema(XmlSchemaSet schemas, XmlQualifiedName typeQName)
		{
			if (schemas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("schemas");
			}
			if (typeQName == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeQName");
			}
			SchemaExporter.AddDefaultXmlType(schemas, typeQName.Name, typeQName.Namespace);
		}

		// Token: 0x04000583 RID: 1411
		internal static readonly string ReadNodesMethodName = "ReadNodes";

		// Token: 0x04000584 RID: 1412
		internal static string WriteNodesMethodName = "WriteNodes";

		// Token: 0x04000585 RID: 1413
		internal static string AddDefaultSchemaMethodName = "AddDefaultSchema";
	}
}
