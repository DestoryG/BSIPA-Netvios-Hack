using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200007E RID: 126
	internal static class DictionaryGlobals
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x00029990 File Offset: 0x00027B90
		static DictionaryGlobals()
		{
			XmlDictionary xmlDictionary = new XmlDictionary(61);
			try
			{
				DictionaryGlobals.SchemaInstanceNamespace = xmlDictionary.Add("http://www.w3.org/2001/XMLSchema-instance");
				DictionaryGlobals.SerializationNamespace = xmlDictionary.Add("http://schemas.microsoft.com/2003/10/Serialization/");
				DictionaryGlobals.SchemaNamespace = xmlDictionary.Add("http://www.w3.org/2001/XMLSchema");
				DictionaryGlobals.XsiTypeLocalName = xmlDictionary.Add("type");
				DictionaryGlobals.XsiNilLocalName = xmlDictionary.Add("nil");
				DictionaryGlobals.IdLocalName = xmlDictionary.Add("Id");
				DictionaryGlobals.RefLocalName = xmlDictionary.Add("Ref");
				DictionaryGlobals.ArraySizeLocalName = xmlDictionary.Add("Size");
				DictionaryGlobals.EmptyString = xmlDictionary.Add(string.Empty);
				DictionaryGlobals.ISerializableFactoryTypeLocalName = xmlDictionary.Add("FactoryType");
				DictionaryGlobals.XmlnsNamespace = xmlDictionary.Add("http://www.w3.org/2000/xmlns/");
				DictionaryGlobals.CharLocalName = xmlDictionary.Add("char");
				DictionaryGlobals.BooleanLocalName = xmlDictionary.Add("boolean");
				DictionaryGlobals.SignedByteLocalName = xmlDictionary.Add("byte");
				DictionaryGlobals.UnsignedByteLocalName = xmlDictionary.Add("unsignedByte");
				DictionaryGlobals.ShortLocalName = xmlDictionary.Add("short");
				DictionaryGlobals.UnsignedShortLocalName = xmlDictionary.Add("unsignedShort");
				DictionaryGlobals.IntLocalName = xmlDictionary.Add("int");
				DictionaryGlobals.UnsignedIntLocalName = xmlDictionary.Add("unsignedInt");
				DictionaryGlobals.LongLocalName = xmlDictionary.Add("long");
				DictionaryGlobals.UnsignedLongLocalName = xmlDictionary.Add("unsignedLong");
				DictionaryGlobals.FloatLocalName = xmlDictionary.Add("float");
				DictionaryGlobals.DoubleLocalName = xmlDictionary.Add("double");
				DictionaryGlobals.DecimalLocalName = xmlDictionary.Add("decimal");
				DictionaryGlobals.DateTimeLocalName = xmlDictionary.Add("dateTime");
				DictionaryGlobals.StringLocalName = xmlDictionary.Add("string");
				DictionaryGlobals.ByteArrayLocalName = xmlDictionary.Add("base64Binary");
				DictionaryGlobals.ObjectLocalName = xmlDictionary.Add("anyType");
				DictionaryGlobals.TimeSpanLocalName = xmlDictionary.Add("duration");
				DictionaryGlobals.GuidLocalName = xmlDictionary.Add("guid");
				DictionaryGlobals.UriLocalName = xmlDictionary.Add("anyURI");
				DictionaryGlobals.QNameLocalName = xmlDictionary.Add("QName");
				DictionaryGlobals.ClrTypeLocalName = xmlDictionary.Add("Type");
				DictionaryGlobals.ClrAssemblyLocalName = xmlDictionary.Add("Assembly");
				DictionaryGlobals.Space = xmlDictionary.Add(" ");
				DictionaryGlobals.timeLocalName = xmlDictionary.Add("time");
				DictionaryGlobals.dateLocalName = xmlDictionary.Add("date");
				DictionaryGlobals.hexBinaryLocalName = xmlDictionary.Add("hexBinary");
				DictionaryGlobals.gYearMonthLocalName = xmlDictionary.Add("gYearMonth");
				DictionaryGlobals.gYearLocalName = xmlDictionary.Add("gYear");
				DictionaryGlobals.gMonthDayLocalName = xmlDictionary.Add("gMonthDay");
				DictionaryGlobals.gDayLocalName = xmlDictionary.Add("gDay");
				DictionaryGlobals.gMonthLocalName = xmlDictionary.Add("gMonth");
				DictionaryGlobals.integerLocalName = xmlDictionary.Add("integer");
				DictionaryGlobals.positiveIntegerLocalName = xmlDictionary.Add("positiveInteger");
				DictionaryGlobals.negativeIntegerLocalName = xmlDictionary.Add("negativeInteger");
				DictionaryGlobals.nonPositiveIntegerLocalName = xmlDictionary.Add("nonPositiveInteger");
				DictionaryGlobals.nonNegativeIntegerLocalName = xmlDictionary.Add("nonNegativeInteger");
				DictionaryGlobals.normalizedStringLocalName = xmlDictionary.Add("normalizedString");
				DictionaryGlobals.tokenLocalName = xmlDictionary.Add("token");
				DictionaryGlobals.languageLocalName = xmlDictionary.Add("language");
				DictionaryGlobals.NameLocalName = xmlDictionary.Add("Name");
				DictionaryGlobals.NCNameLocalName = xmlDictionary.Add("NCName");
				DictionaryGlobals.XSDIDLocalName = xmlDictionary.Add("ID");
				DictionaryGlobals.IDREFLocalName = xmlDictionary.Add("IDREF");
				DictionaryGlobals.IDREFSLocalName = xmlDictionary.Add("IDREFS");
				DictionaryGlobals.ENTITYLocalName = xmlDictionary.Add("ENTITY");
				DictionaryGlobals.ENTITIESLocalName = xmlDictionary.Add("ENTITIES");
				DictionaryGlobals.NMTOKENLocalName = xmlDictionary.Add("NMTOKEN");
				DictionaryGlobals.NMTOKENSLocalName = xmlDictionary.Add("NMTOKENS");
				DictionaryGlobals.AsmxTypesNamespace = xmlDictionary.Add("http://microsoft.com/wsdl/types/");
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
			}
		}

		// Token: 0x04000354 RID: 852
		public static readonly XmlDictionaryString EmptyString;

		// Token: 0x04000355 RID: 853
		public static readonly XmlDictionaryString SchemaInstanceNamespace;

		// Token: 0x04000356 RID: 854
		public static readonly XmlDictionaryString SchemaNamespace;

		// Token: 0x04000357 RID: 855
		public static readonly XmlDictionaryString SerializationNamespace;

		// Token: 0x04000358 RID: 856
		public static readonly XmlDictionaryString XmlnsNamespace;

		// Token: 0x04000359 RID: 857
		public static readonly XmlDictionaryString XsiTypeLocalName;

		// Token: 0x0400035A RID: 858
		public static readonly XmlDictionaryString XsiNilLocalName;

		// Token: 0x0400035B RID: 859
		public static readonly XmlDictionaryString ClrTypeLocalName;

		// Token: 0x0400035C RID: 860
		public static readonly XmlDictionaryString ClrAssemblyLocalName;

		// Token: 0x0400035D RID: 861
		public static readonly XmlDictionaryString ArraySizeLocalName;

		// Token: 0x0400035E RID: 862
		public static readonly XmlDictionaryString IdLocalName;

		// Token: 0x0400035F RID: 863
		public static readonly XmlDictionaryString RefLocalName;

		// Token: 0x04000360 RID: 864
		public static readonly XmlDictionaryString ISerializableFactoryTypeLocalName;

		// Token: 0x04000361 RID: 865
		public static readonly XmlDictionaryString CharLocalName;

		// Token: 0x04000362 RID: 866
		public static readonly XmlDictionaryString BooleanLocalName;

		// Token: 0x04000363 RID: 867
		public static readonly XmlDictionaryString SignedByteLocalName;

		// Token: 0x04000364 RID: 868
		public static readonly XmlDictionaryString UnsignedByteLocalName;

		// Token: 0x04000365 RID: 869
		public static readonly XmlDictionaryString ShortLocalName;

		// Token: 0x04000366 RID: 870
		public static readonly XmlDictionaryString UnsignedShortLocalName;

		// Token: 0x04000367 RID: 871
		public static readonly XmlDictionaryString IntLocalName;

		// Token: 0x04000368 RID: 872
		public static readonly XmlDictionaryString UnsignedIntLocalName;

		// Token: 0x04000369 RID: 873
		public static readonly XmlDictionaryString LongLocalName;

		// Token: 0x0400036A RID: 874
		public static readonly XmlDictionaryString UnsignedLongLocalName;

		// Token: 0x0400036B RID: 875
		public static readonly XmlDictionaryString FloatLocalName;

		// Token: 0x0400036C RID: 876
		public static readonly XmlDictionaryString DoubleLocalName;

		// Token: 0x0400036D RID: 877
		public static readonly XmlDictionaryString DecimalLocalName;

		// Token: 0x0400036E RID: 878
		public static readonly XmlDictionaryString DateTimeLocalName;

		// Token: 0x0400036F RID: 879
		public static readonly XmlDictionaryString StringLocalName;

		// Token: 0x04000370 RID: 880
		public static readonly XmlDictionaryString ByteArrayLocalName;

		// Token: 0x04000371 RID: 881
		public static readonly XmlDictionaryString ObjectLocalName;

		// Token: 0x04000372 RID: 882
		public static readonly XmlDictionaryString TimeSpanLocalName;

		// Token: 0x04000373 RID: 883
		public static readonly XmlDictionaryString GuidLocalName;

		// Token: 0x04000374 RID: 884
		public static readonly XmlDictionaryString UriLocalName;

		// Token: 0x04000375 RID: 885
		public static readonly XmlDictionaryString QNameLocalName;

		// Token: 0x04000376 RID: 886
		public static readonly XmlDictionaryString Space;

		// Token: 0x04000377 RID: 887
		public static readonly XmlDictionaryString timeLocalName;

		// Token: 0x04000378 RID: 888
		public static readonly XmlDictionaryString dateLocalName;

		// Token: 0x04000379 RID: 889
		public static readonly XmlDictionaryString hexBinaryLocalName;

		// Token: 0x0400037A RID: 890
		public static readonly XmlDictionaryString gYearMonthLocalName;

		// Token: 0x0400037B RID: 891
		public static readonly XmlDictionaryString gYearLocalName;

		// Token: 0x0400037C RID: 892
		public static readonly XmlDictionaryString gMonthDayLocalName;

		// Token: 0x0400037D RID: 893
		public static readonly XmlDictionaryString gDayLocalName;

		// Token: 0x0400037E RID: 894
		public static readonly XmlDictionaryString gMonthLocalName;

		// Token: 0x0400037F RID: 895
		public static readonly XmlDictionaryString integerLocalName;

		// Token: 0x04000380 RID: 896
		public static readonly XmlDictionaryString positiveIntegerLocalName;

		// Token: 0x04000381 RID: 897
		public static readonly XmlDictionaryString negativeIntegerLocalName;

		// Token: 0x04000382 RID: 898
		public static readonly XmlDictionaryString nonPositiveIntegerLocalName;

		// Token: 0x04000383 RID: 899
		public static readonly XmlDictionaryString nonNegativeIntegerLocalName;

		// Token: 0x04000384 RID: 900
		public static readonly XmlDictionaryString normalizedStringLocalName;

		// Token: 0x04000385 RID: 901
		public static readonly XmlDictionaryString tokenLocalName;

		// Token: 0x04000386 RID: 902
		public static readonly XmlDictionaryString languageLocalName;

		// Token: 0x04000387 RID: 903
		public static readonly XmlDictionaryString NameLocalName;

		// Token: 0x04000388 RID: 904
		public static readonly XmlDictionaryString NCNameLocalName;

		// Token: 0x04000389 RID: 905
		public static readonly XmlDictionaryString XSDIDLocalName;

		// Token: 0x0400038A RID: 906
		public static readonly XmlDictionaryString IDREFLocalName;

		// Token: 0x0400038B RID: 907
		public static readonly XmlDictionaryString IDREFSLocalName;

		// Token: 0x0400038C RID: 908
		public static readonly XmlDictionaryString ENTITYLocalName;

		// Token: 0x0400038D RID: 909
		public static readonly XmlDictionaryString ENTITIESLocalName;

		// Token: 0x0400038E RID: 910
		public static readonly XmlDictionaryString NMTOKENLocalName;

		// Token: 0x0400038F RID: 911
		public static readonly XmlDictionaryString NMTOKENSLocalName;

		// Token: 0x04000390 RID: 912
		public static readonly XmlDictionaryString AsmxTypesNamespace;
	}
}
