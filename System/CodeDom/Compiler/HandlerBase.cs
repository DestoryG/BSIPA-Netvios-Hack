using System;
using System.Configuration;
using System.Globalization;
using System.Xml;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200066F RID: 1647
	internal static class HandlerBase
	{
		// Token: 0x06003B9D RID: 15261 RVA: 0x000F694C File Offset: 0x000F4B4C
		private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attrib);
			if (fRequired && xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_missing_required_attribute", new object[] { attrib, node.Name }), node);
			}
			return xmlNode;
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x000F6994 File Offset: 0x000F4B94
		private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = andRemoveAttribute.Value;
			}
			return andRemoveAttribute;
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x000F69B6 File Offset: 0x000F4BB6
		internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x000F69C1 File Offset: 0x000F4BC1
		internal static XmlNode GetAndRemoveRequiredNonEmptyStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveNonEmptyStringAttributeInternal(node, attrib, true, ref val);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x000F69CC File Offset: 0x000F4BCC
		private static XmlNode GetAndRemoveNonEmptyStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveStringAttributeInternal = HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, fRequired, ref val);
			if (andRemoveStringAttributeInternal != null && val.Length == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Empty_attribute", new object[] { attrib }), andRemoveStringAttributeInternal);
			}
			return andRemoveStringAttributeInternal;
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x000F6A0C File Offset: 0x000F4C0C
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

		// Token: 0x06003BA3 RID: 15267 RVA: 0x000F6AA8 File Offset: 0x000F4CA8
		private static XmlNode GetAndRemoveNonNegativeAttributeInternal(XmlNode node, string attrib, bool fRequired, ref int val)
		{
			XmlNode andRemoveIntegerAttributeInternal = HandlerBase.GetAndRemoveIntegerAttributeInternal(node, attrib, fRequired, ref val);
			if (andRemoveIntegerAttributeInternal != null && val < 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_nonnegative_integer_attribute", new object[] { attrib }), andRemoveIntegerAttributeInternal);
			}
			return andRemoveIntegerAttributeInternal;
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x000F6AE3 File Offset: 0x000F4CE3
		internal static XmlNode GetAndRemoveNonNegativeIntegerAttribute(XmlNode node, string attrib, ref int val)
		{
			return HandlerBase.GetAndRemoveNonNegativeAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x000F6AF0 File Offset: 0x000F4CF0
		internal static void CheckForUnrecognizedAttributes(XmlNode node)
		{
			if (node.Attributes.Count != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { node.Attributes[0].Name }), node.Attributes[0]);
			}
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x000F6B40 File Offset: 0x000F4D40
		internal static void CheckForNonElement(XmlNode node)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_elements_only"), node);
			}
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000F6B5C File Offset: 0x000F4D5C
		internal static bool IsIgnorableAlsoCheckForNonElement(XmlNode node)
		{
			if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.Whitespace)
			{
				return true;
			}
			HandlerBase.CheckForNonElement(node);
			return false;
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000F6B7A File Offset: 0x000F4D7A
		internal static void CheckForChildNodes(XmlNode node)
		{
			if (node.HasChildNodes)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_no_child_nodes"), node.FirstChild);
			}
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000F6B9A File Offset: 0x000F4D9A
		internal static void ThrowUnrecognizedElement(XmlNode node)
		{
			throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), node);
		}
	}
}
