using System;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000039 RID: 57
	public class XmlDictionaryString
	{
		// Token: 0x0600044D RID: 1101 RVA: 0x00015DE4 File Offset: 0x00013FE4
		public XmlDictionaryString(IXmlDictionary dictionary, string value, int key)
		{
			if (dictionary == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("dictionary"));
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
			}
			if (key < 0 || key > 536870911)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("key", global::System.Runtime.Serialization.SR.GetString("The value of this argument must fall within the range {0} to {1}.", new object[] { 0, 536870911 })));
			}
			this.dictionary = dictionary;
			this.value = value;
			this.key = key;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00015E74 File Offset: 0x00014074
		internal static string GetString(XmlDictionaryString s)
		{
			if (s == null)
			{
				return null;
			}
			return s.Value;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00015E81 File Offset: 0x00014081
		public static XmlDictionaryString Empty
		{
			get
			{
				return XmlDictionaryString.emptyStringDictionary.EmptyString;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00015E8D File Offset: 0x0001408D
		public IXmlDictionary Dictionary
		{
			get
			{
				return this.dictionary;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00015E95 File Offset: 0x00014095
		public int Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00015E9D File Offset: 0x0001409D
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00015EA5 File Offset: 0x000140A5
		internal byte[] ToUTF8()
		{
			if (this.buffer == null)
			{
				this.buffer = Encoding.UTF8.GetBytes(this.value);
			}
			return this.buffer;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00015ECB File Offset: 0x000140CB
		public override string ToString()
		{
			return this.value;
		}

		// Token: 0x040001F9 RID: 505
		internal const int MinKey = 0;

		// Token: 0x040001FA RID: 506
		internal const int MaxKey = 536870911;

		// Token: 0x040001FB RID: 507
		private IXmlDictionary dictionary;

		// Token: 0x040001FC RID: 508
		private string value;

		// Token: 0x040001FD RID: 509
		private int key;

		// Token: 0x040001FE RID: 510
		private byte[] buffer;

		// Token: 0x040001FF RID: 511
		private static XmlDictionaryString.EmptyStringDictionary emptyStringDictionary = new XmlDictionaryString.EmptyStringDictionary();

		// Token: 0x02000153 RID: 339
		private class EmptyStringDictionary : IXmlDictionary
		{
			// Token: 0x060012ED RID: 4845 RVA: 0x0004E09C File Offset: 0x0004C29C
			public EmptyStringDictionary()
			{
				this.empty = new XmlDictionaryString(this, string.Empty, 0);
			}

			// Token: 0x170003CE RID: 974
			// (get) Token: 0x060012EE RID: 4846 RVA: 0x0004E0B6 File Offset: 0x0004C2B6
			public XmlDictionaryString EmptyString
			{
				get
				{
					return this.empty;
				}
			}

			// Token: 0x060012EF RID: 4847 RVA: 0x0004E0BE File Offset: 0x0004C2BE
			public bool TryLookup(string value, out XmlDictionaryString result)
			{
				if (value == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
				}
				if (value.Length == 0)
				{
					result = this.empty;
					return true;
				}
				result = null;
				return false;
			}

			// Token: 0x060012F0 RID: 4848 RVA: 0x0004E0E4 File Offset: 0x0004C2E4
			public bool TryLookup(int key, out XmlDictionaryString result)
			{
				if (key == 0)
				{
					result = this.empty;
					return true;
				}
				result = null;
				return false;
			}

			// Token: 0x060012F1 RID: 4849 RVA: 0x0004E0F7 File Offset: 0x0004C2F7
			public bool TryLookup(XmlDictionaryString value, out XmlDictionaryString result)
			{
				if (value == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
				}
				if (value.Dictionary != this)
				{
					result = null;
					return false;
				}
				result = value;
				return true;
			}

			// Token: 0x0400093D RID: 2365
			private XmlDictionaryString empty;
		}
	}
}
