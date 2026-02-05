using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200005D RID: 93
	internal static class XmlExceptionHelper
	{
		// Token: 0x060006BA RID: 1722 RVA: 0x0001EB66 File Offset: 0x0001CD66
		private static void ThrowXmlException(XmlDictionaryReader reader, string res)
		{
			XmlExceptionHelper.ThrowXmlException(reader, res, null);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001EB70 File Offset: 0x0001CD70
		private static void ThrowXmlException(XmlDictionaryReader reader, string res, string arg1)
		{
			XmlExceptionHelper.ThrowXmlException(reader, res, arg1, null);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001EB7B File Offset: 0x0001CD7B
		private static void ThrowXmlException(XmlDictionaryReader reader, string res, string arg1, string arg2)
		{
			XmlExceptionHelper.ThrowXmlException(reader, res, arg1, arg2, null);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001EB88 File Offset: 0x0001CD88
		private static void ThrowXmlException(XmlDictionaryReader reader, string res, string arg1, string arg2, string arg3)
		{
			string text = global::System.Runtime.Serialization.SR.GetString(res, new object[] { arg1, arg2, arg3 });
			IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
			if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
			{
				text = text + " " + global::System.Runtime.Serialization.SR.GetString("Line {0}, position {1}.", new object[] { xmlLineInfo.LineNumber, xmlLineInfo.LinePosition });
			}
			if (TD.ReaderQuotaExceededIsEnabled())
			{
				TD.ReaderQuotaExceeded(text);
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(text));
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001EC14 File Offset: 0x0001CE14
		public static void ThrowXmlException(XmlDictionaryReader reader, XmlException exception)
		{
			string text = exception.Message;
			IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
			if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
			{
				text = text + " " + global::System.Runtime.Serialization.SR.GetString("Line {0}, position {1}.", new object[] { xmlLineInfo.LineNumber, xmlLineInfo.LinePosition });
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(text));
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001EC7D File Offset: 0x0001CE7D
		private static string GetName(string prefix, string localName)
		{
			if (prefix.Length == 0)
			{
				return localName;
			}
			return prefix + ":" + localName;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001EC98 File Offset: 0x0001CE98
		private static string GetWhatWasFound(XmlDictionaryReader reader)
		{
			if (reader.EOF)
			{
				return global::System.Runtime.Serialization.SR.GetString("end of file");
			}
			XmlNodeType nodeType = reader.NodeType;
			if (nodeType <= XmlNodeType.Comment)
			{
				switch (nodeType)
				{
				case XmlNodeType.Element:
					return global::System.Runtime.Serialization.SR.GetString("element '{0}' from namespace '{1}'", new object[]
					{
						XmlExceptionHelper.GetName(reader.Prefix, reader.LocalName),
						reader.NamespaceURI
					});
				case XmlNodeType.Attribute:
					goto IL_00FD;
				case XmlNodeType.Text:
					break;
				case XmlNodeType.CDATA:
					return global::System.Runtime.Serialization.SR.GetString("cdata '{0}'", new object[] { reader.Value });
				default:
					if (nodeType != XmlNodeType.Comment)
					{
						goto IL_00FD;
					}
					return global::System.Runtime.Serialization.SR.GetString("comment '{0}'", new object[] { reader.Value });
				}
			}
			else if (nodeType - XmlNodeType.Whitespace > 1)
			{
				if (nodeType != XmlNodeType.EndElement)
				{
					goto IL_00FD;
				}
				return global::System.Runtime.Serialization.SR.GetString("end element '{0}' from namespace '{1}'", new object[]
				{
					XmlExceptionHelper.GetName(reader.Prefix, reader.LocalName),
					reader.NamespaceURI
				});
			}
			return global::System.Runtime.Serialization.SR.GetString("text '{0}'", new object[] { reader.Value });
			IL_00FD:
			return global::System.Runtime.Serialization.SR.GetString("node {0}", new object[] { reader.NodeType });
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
		public static void ThrowStartElementExpected(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Start element expected. Found {0}.", XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001EDD3 File Offset: 0x0001CFD3
		public static void ThrowStartElementExpected(XmlDictionaryReader reader, string name)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Start element '{0}' expected. Found {1}.", name, XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001EDE7 File Offset: 0x0001CFE7
		public static void ThrowStartElementExpected(XmlDictionaryReader reader, string localName, string ns)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Start element '{0}' from namespace '{1}' expected. Found {2}.", localName, ns, XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001EDFC File Offset: 0x0001CFFC
		public static void ThrowStartElementExpected(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString ns)
		{
			XmlExceptionHelper.ThrowStartElementExpected(reader, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(ns));
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001EE10 File Offset: 0x0001D010
		public static void ThrowFullStartElementExpected(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Non-empty start element expected. Found {0}.", XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001EE23 File Offset: 0x0001D023
		public static void ThrowFullStartElementExpected(XmlDictionaryReader reader, string name)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Non-empty start element '{0}' expected. Found {1}.", name, XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001EE37 File Offset: 0x0001D037
		public static void ThrowFullStartElementExpected(XmlDictionaryReader reader, string localName, string ns)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Non-empty start element '{0}' from namespace '{1}' expected. Found {2}.", localName, ns, XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001EE4C File Offset: 0x0001D04C
		public static void ThrowFullStartElementExpected(XmlDictionaryReader reader, XmlDictionaryString localName, XmlDictionaryString ns)
		{
			XmlExceptionHelper.ThrowFullStartElementExpected(reader, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(ns));
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001EE60 File Offset: 0x0001D060
		public static void ThrowEndElementExpected(XmlDictionaryReader reader, string localName, string ns)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "End element '{0}' from namespace '{1}' expected. Found {2}.", localName, ns, XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001EE75 File Offset: 0x0001D075
		public static void ThrowMaxStringContentLengthExceeded(XmlDictionaryReader reader, int maxStringContentLength)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "XML max string content length exceeded. It must be less than {0}.", maxStringContentLength.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001EE8E File Offset: 0x0001D08E
		public static void ThrowMaxArrayLengthExceeded(XmlDictionaryReader reader, int maxArrayLength)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The maximum array length quota ({0}) has been exceeded while reading XML data. This quota may be increased by changing the MaxArrayLength property on the XmlDictionaryReaderQuotas object used when creating the XML reader.", maxArrayLength.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001EEA7 File Offset: 0x0001D0A7
		public static void ThrowMaxArrayLengthOrMaxItemsQuotaExceeded(XmlDictionaryReader reader, int maxQuota)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "XML max array length or max items quota exceeded. It must be less than {0}.", maxQuota.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001EEC0 File Offset: 0x0001D0C0
		public static void ThrowMaxDepthExceeded(XmlDictionaryReader reader, int maxDepth)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "XML max depth exceeded. It must be less than {0}.", maxDepth.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001EED9 File Offset: 0x0001D0D9
		public static void ThrowMaxBytesPerReadExceeded(XmlDictionaryReader reader, int maxBytesPerRead)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "XML max bytes per read exceeded. It must be less than {0}.", maxBytesPerRead.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001EEF2 File Offset: 0x0001D0F2
		public static void ThrowMaxNameTableCharCountExceeded(XmlDictionaryReader reader, int maxNameTableCharCount)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The maximum nametable character count quota ({0}) has been exceeded while reading XML data. The nametable is a data structure used to store strings encountered during XML processing - long XML documents with non-repeating element names, attribute names and attribute values may trigger this quota. This quota may be increased by changing the MaxNameTableCharCount property on the XmlDictionaryReaderQuotas object used when creating the XML reader.", maxNameTableCharCount.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001EF0B File Offset: 0x0001D10B
		public static void ThrowBase64DataExpected(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Base64 encoded data expected. Found {0}.", XmlExceptionHelper.GetWhatWasFound(reader));
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001EF1E File Offset: 0x0001D11E
		public static void ThrowUndefinedPrefix(XmlDictionaryReader reader, string prefix)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The prefix '{0}' is not defined.", prefix);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001EF2C File Offset: 0x0001D12C
		public static void ThrowProcessingInstructionNotSupported(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Processing instructions (other than the XML declaration) and DTDs are not supported.");
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001EF39 File Offset: 0x0001D139
		public static void ThrowInvalidXml(XmlDictionaryReader reader, byte b)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The byte 0x{0} is not valid at this location.", b.ToString("X2", CultureInfo.InvariantCulture));
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001EF57 File Offset: 0x0001D157
		public static void ThrowUnexpectedEndOfFile(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Unexpected end of file. Following elements are not closed: {0}.", ((XmlBaseReader)reader).GetOpenElements());
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001EF6F File Offset: 0x0001D16F
		public static void ThrowUnexpectedEndElement(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "No matching start tag for end element.");
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001EF7C File Offset: 0x0001D17C
		public static void ThrowTokenExpected(XmlDictionaryReader reader, string expected, char found)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The token '{0}' was expected but found '{1}'.", expected, found.ToString());
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001EF91 File Offset: 0x0001D191
		public static void ThrowTokenExpected(XmlDictionaryReader reader, string expected, string found)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The token '{0}' was expected but found '{1}'.", expected, found);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
		public static void ThrowInvalidCharRef(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Character reference not valid.");
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001EFAD File Offset: 0x0001D1AD
		public static void ThrowTagMismatch(XmlDictionaryReader reader, string expectedPrefix, string expectedLocalName, string foundPrefix, string foundLocalName)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Start element '{0}' does not match end element '{1}'.", XmlExceptionHelper.GetName(expectedPrefix, expectedLocalName), XmlExceptionHelper.GetName(foundPrefix, foundLocalName));
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001EFCC File Offset: 0x0001D1CC
		public static void ThrowDuplicateXmlnsAttribute(XmlDictionaryReader reader, string localName, string ns)
		{
			string text;
			if (localName.Length == 0)
			{
				text = "xmlns";
			}
			else
			{
				text = "xmlns:" + localName;
			}
			XmlExceptionHelper.ThrowXmlException(reader, "Duplicate attribute found. Both '{0}' and '{1}' are from the namespace '{2}'.", text, text, ns);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001F003 File Offset: 0x0001D203
		public static void ThrowDuplicateAttribute(XmlDictionaryReader reader, string prefix1, string prefix2, string localName, string ns)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "Duplicate attribute found. Both '{0}' and '{1}' are from the namespace '{2}'.", XmlExceptionHelper.GetName(prefix1, localName), XmlExceptionHelper.GetName(prefix2, localName), ns);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001F020 File Offset: 0x0001D220
		public static void ThrowInvalidBinaryFormat(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The input source is not correctly formatted.");
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001F02D File Offset: 0x0001D22D
		public static void ThrowInvalidRootData(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The data at the root level is invalid.");
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001F03A File Offset: 0x0001D23A
		public static void ThrowMultipleRootElements(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "There are multiple root elements.");
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001F047 File Offset: 0x0001D247
		public static void ThrowDeclarationNotFirst(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "No characters can appear before the XML declaration.");
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001F054 File Offset: 0x0001D254
		public static void ThrowConversionOverflow(XmlDictionaryReader reader, string value, string type)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The value '{0}' cannot be represented with the type '{1}'.", value, type);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001F064 File Offset: 0x0001D264
		public static void ThrowXmlDictionaryStringIDOutOfRange(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "XmlDictionaryString IDs must be in the range from {0} to {1}.", 0.ToString(NumberFormatInfo.CurrentInfo), 536870911.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001F09C File Offset: 0x0001D29C
		public static void ThrowXmlDictionaryStringIDUndefinedStatic(XmlDictionaryReader reader, int key)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "XmlDictionaryString ID {0} not defined in the static dictionary.", key.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001F0B5 File Offset: 0x0001D2B5
		public static void ThrowXmlDictionaryStringIDUndefinedSession(XmlDictionaryReader reader, int key)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "XmlDictionaryString ID {0} not defined in the XmlBinaryReaderSession.", key.ToString(NumberFormatInfo.CurrentInfo));
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001F0CE File Offset: 0x0001D2CE
		public static void ThrowEmptyNamespace(XmlDictionaryReader reader)
		{
			XmlExceptionHelper.ThrowXmlException(reader, "The empty namespace requires a null or empty prefix.");
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001F0DB File Offset: 0x0001D2DB
		public static XmlException CreateConversionException(string value, string type, Exception exception)
		{
			return new XmlException(global::System.Runtime.Serialization.SR.GetString("The value '{0}' cannot be parsed as the type '{1}'.", new object[] { value, type }), exception);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001F0FB File Offset: 0x0001D2FB
		public static XmlException CreateEncodingException(byte[] buffer, int offset, int count, Exception exception)
		{
			return XmlExceptionHelper.CreateEncodingException(new UTF8Encoding(false, false).GetString(buffer, offset, count), exception);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001F112 File Offset: 0x0001D312
		public static XmlException CreateEncodingException(string value, Exception exception)
		{
			return new XmlException(global::System.Runtime.Serialization.SR.GetString("'{0}' contains invalid UTF8 bytes.", new object[] { value }), exception);
		}
	}
}
