using System;
using System.Globalization;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200008D RID: 141
	internal class HandlerBase
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x00021AB9 File Offset: 0x0001FCB9
		private HandlerBase()
		{
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00021AC4 File Offset: 0x0001FCC4
		private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attrib);
			if (fRequired && xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_missing_required_attribute", new object[] { attrib, node.Name }), node);
			}
			return xmlNode;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00021B0C File Offset: 0x0001FD0C
		private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = andRemoveAttribute.Value;
			}
			return andRemoveAttribute;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00021B2E File Offset: 0x0001FD2E
		internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00021B3C File Offset: 0x0001FD3C
		private static XmlNode GetAndRemoveBooleanAttributeInternal(XmlNode node, string attrib, bool fRequired, ref bool val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				try
				{
					val = bool.Parse(andRemoveAttribute.Value);
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString(SR.GetString("Config_invalid_boolean_attribute", new object[] { andRemoveAttribute.Name })), ex, andRemoveAttribute);
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00021B9C File Offset: 0x0001FD9C
		internal static XmlNode GetAndRemoveBooleanAttribute(XmlNode node, string attrib, ref bool val)
		{
			return HandlerBase.GetAndRemoveBooleanAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00021BA8 File Offset: 0x0001FDA8
		private static XmlNode GetAndRemoveIntegerAttributeInternal(XmlNode node, string attrib, bool fRequired, ref int val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				if (andRemoveAttribute.Value.Trim() != andRemoveAttribute.Value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_integer_attribute", new object[] { andRemoveAttribute.Name }), andRemoveAttribute);
				}
				try
				{
					val = int.Parse(andRemoveAttribute.Value, CultureInfo.InvariantCulture);
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_integer_attribute", new object[] { andRemoveAttribute.Name }), ex, andRemoveAttribute);
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00021C44 File Offset: 0x0001FE44
		internal static XmlNode GetAndRemoveIntegerAttribute(XmlNode node, string attrib, ref int val)
		{
			return HandlerBase.GetAndRemoveIntegerAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00021C4F File Offset: 0x0001FE4F
		internal static void CheckForUnrecognizedAttributes(XmlNode node)
		{
			if (node.Attributes.Count != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { node.Attributes[0].Name }), node);
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00021C8C File Offset: 0x0001FE8C
		internal static string RemoveAttribute(XmlNode node, string name)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode != null)
			{
				return xmlNode.Value;
			}
			return null;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00021CB1 File Offset: 0x0001FEB1
		internal static string RemoveRequiredAttribute(XmlNode node, string name)
		{
			return HandlerBase.RemoveRequiredAttribute(node, name, false);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00021CBC File Offset: 0x0001FEBC
		internal static string RemoveRequiredAttribute(XmlNode node, string name, bool allowEmpty)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_missing", new object[] { name }), node);
			}
			if (string.IsNullOrEmpty(xmlNode.Value) && !allowEmpty)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_empty", new object[] { name }), node);
			}
			return xmlNode.Value;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00021D25 File Offset: 0x0001FF25
		internal static void CheckForNonElement(XmlNode node)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_elements_only"), node);
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00021D41 File Offset: 0x0001FF41
		internal static bool IsIgnorableAlsoCheckForNonElement(XmlNode node)
		{
			if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.Whitespace)
			{
				return true;
			}
			HandlerBase.CheckForNonElement(node);
			return false;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00021D5F File Offset: 0x0001FF5F
		internal static void CheckForChildNodes(XmlNode node)
		{
			if (node.HasChildNodes)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_no_child_nodes"), node.FirstChild);
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00021D7F File Offset: 0x0001FF7F
		internal static void ThrowUnrecognizedElement(XmlNode node)
		{
			throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), node);
		}
	}
}
