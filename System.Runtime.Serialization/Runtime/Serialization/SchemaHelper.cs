using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization
{
	// Token: 0x020000D2 RID: 210
	internal static class SchemaHelper
	{
		// Token: 0x06000BCB RID: 3019 RVA: 0x00030BF9 File Offset: 0x0002EDF9
		internal static bool NamespacesEqual(string ns1, string ns2)
		{
			if (ns1 == null || ns1.Length == 0)
			{
				return ns2 == null || ns2.Length == 0;
			}
			return ns1 == ns2;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00030C1C File Offset: 0x0002EE1C
		internal static XmlSchemaType GetSchemaType(XmlSchemaSet schemas, XmlQualifiedName typeQName, out XmlSchema outSchema)
		{
			outSchema = null;
			IEnumerable enumerable = schemas.Schemas();
			string @namespace = typeQName.Namespace;
			foreach (object obj in enumerable)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (SchemaHelper.NamespacesEqual(@namespace, xmlSchema.TargetNamespace))
				{
					outSchema = xmlSchema;
					foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
					{
						XmlSchemaType xmlSchemaType = xmlSchemaObject as XmlSchemaType;
						if (xmlSchemaType != null && xmlSchemaType.Name == typeQName.Name)
						{
							return xmlSchemaType;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00030CF4 File Offset: 0x0002EEF4
		internal static XmlSchemaType GetSchemaType(Dictionary<XmlQualifiedName, SchemaObjectInfo> schemaInfo, XmlQualifiedName typeName)
		{
			SchemaObjectInfo schemaObjectInfo;
			if (schemaInfo.TryGetValue(typeName, out schemaObjectInfo))
			{
				return schemaObjectInfo.type;
			}
			return null;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00030D14 File Offset: 0x0002EF14
		internal static XmlSchema GetSchemaWithType(Dictionary<XmlQualifiedName, SchemaObjectInfo> schemaInfo, XmlSchemaSet schemas, XmlQualifiedName typeName)
		{
			SchemaObjectInfo schemaObjectInfo;
			if (schemaInfo.TryGetValue(typeName, out schemaObjectInfo) && schemaObjectInfo.schema != null)
			{
				return schemaObjectInfo.schema;
			}
			IEnumerable enumerable = schemas.Schemas();
			string @namespace = typeName.Namespace;
			foreach (object obj in enumerable)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (SchemaHelper.NamespacesEqual(@namespace, xmlSchema.TargetNamespace))
				{
					return xmlSchema;
				}
			}
			return null;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00030DA4 File Offset: 0x0002EFA4
		internal static XmlSchemaElement GetSchemaElement(XmlSchemaSet schemas, XmlQualifiedName elementQName, out XmlSchema outSchema)
		{
			outSchema = null;
			IEnumerable enumerable = schemas.Schemas();
			string @namespace = elementQName.Namespace;
			foreach (object obj in enumerable)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (SchemaHelper.NamespacesEqual(@namespace, xmlSchema.TargetNamespace))
				{
					outSchema = xmlSchema;
					foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
					{
						XmlSchemaElement xmlSchemaElement = xmlSchemaObject as XmlSchemaElement;
						if (xmlSchemaElement != null && xmlSchemaElement.Name == elementQName.Name)
						{
							return xmlSchemaElement;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00030E7C File Offset: 0x0002F07C
		internal static XmlSchemaElement GetSchemaElement(Dictionary<XmlQualifiedName, SchemaObjectInfo> schemaInfo, XmlQualifiedName elementName)
		{
			SchemaObjectInfo schemaObjectInfo;
			if (schemaInfo.TryGetValue(elementName, out schemaObjectInfo))
			{
				return schemaObjectInfo.element;
			}
			return null;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00030E9C File Offset: 0x0002F09C
		internal static XmlSchema GetSchema(string ns, XmlSchemaSet schemas)
		{
			if (ns == null)
			{
				ns = string.Empty;
			}
			foreach (object obj in schemas.Schemas())
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if ((xmlSchema.TargetNamespace == null && ns.Length == 0) || ns.Equals(xmlSchema.TargetNamespace))
				{
					return xmlSchema;
				}
			}
			return SchemaHelper.CreateSchema(ns, schemas);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00030F24 File Offset: 0x0002F124
		private static XmlSchema CreateSchema(string ns, XmlSchemaSet schemas)
		{
			XmlSchema xmlSchema = new XmlSchema();
			xmlSchema.ElementFormDefault = XmlSchemaForm.Qualified;
			if (ns.Length > 0)
			{
				xmlSchema.TargetNamespace = ns;
				xmlSchema.Namespaces.Add("tns", ns);
			}
			schemas.Add(xmlSchema);
			return xmlSchema;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00030F68 File Offset: 0x0002F168
		internal static void AddElementForm(XmlSchemaElement element, XmlSchema schema)
		{
			if (schema.ElementFormDefault != XmlSchemaForm.Qualified)
			{
				element.Form = XmlSchemaForm.Qualified;
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00030F7C File Offset: 0x0002F17C
		internal static void AddSchemaImport(string ns, XmlSchema schema)
		{
			if (SchemaHelper.NamespacesEqual(ns, schema.TargetNamespace) || SchemaHelper.NamespacesEqual(ns, "http://www.w3.org/2001/XMLSchema") || SchemaHelper.NamespacesEqual(ns, "http://www.w3.org/2001/XMLSchema-instance"))
			{
				return;
			}
			foreach (object obj in schema.Includes)
			{
				if (obj is XmlSchemaImport && SchemaHelper.NamespacesEqual(ns, ((XmlSchemaImport)obj).Namespace))
				{
					return;
				}
			}
			XmlSchemaImport xmlSchemaImport = new XmlSchemaImport();
			if (ns != null && ns.Length > 0)
			{
				xmlSchemaImport.Namespace = ns;
			}
			schema.Includes.Add(xmlSchemaImport);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00031038 File Offset: 0x0002F238
		internal static XmlSchema GetSchemaWithGlobalElementDeclaration(XmlSchemaElement element, XmlSchemaSet schemas)
		{
			foreach (object obj in schemas.Schemas())
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
				{
					XmlSchemaElement xmlSchemaElement = xmlSchemaObject as XmlSchemaElement;
					if (xmlSchemaElement != null && xmlSchemaElement == element)
					{
						return xmlSchema;
					}
				}
			}
			return null;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000310E4 File Offset: 0x0002F2E4
		internal static XmlQualifiedName GetGlobalElementDeclaration(XmlSchemaSet schemas, XmlQualifiedName typeQName, out bool isNullable)
		{
			IEnumerable enumerable = schemas.Schemas();
			if (typeQName.Namespace == null)
			{
				string empty = string.Empty;
			}
			isNullable = false;
			foreach (object obj in enumerable)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
				{
					XmlSchemaElement xmlSchemaElement = xmlSchemaObject as XmlSchemaElement;
					if (xmlSchemaElement != null && xmlSchemaElement.SchemaTypeName.Equals(typeQName))
					{
						isNullable = xmlSchemaElement.IsNillable;
						return new XmlQualifiedName(xmlSchemaElement.Name, xmlSchema.TargetNamespace);
					}
				}
			}
			return null;
		}
	}
}
