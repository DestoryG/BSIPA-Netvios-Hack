using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000034 RID: 52
	public class XmlDictionary : IXmlDictionary
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0001432E File Offset: 0x0001252E
		public static IXmlDictionary Empty
		{
			get
			{
				if (XmlDictionary.empty == null)
				{
					XmlDictionary.empty = new XmlDictionary.EmptyDictionary();
				}
				return XmlDictionary.empty;
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00014346 File Offset: 0x00012546
		public XmlDictionary()
		{
			this.lookup = new Dictionary<string, XmlDictionaryString>();
			this.strings = null;
			this.nextId = 0;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00014367 File Offset: 0x00012567
		public XmlDictionary(int capacity)
		{
			this.lookup = new Dictionary<string, XmlDictionaryString>(capacity);
			this.strings = new XmlDictionaryString[capacity];
			this.nextId = 0;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00014390 File Offset: 0x00012590
		public virtual XmlDictionaryString Add(string value)
		{
			XmlDictionaryString xmlDictionaryString;
			if (!this.lookup.TryGetValue(value, out xmlDictionaryString))
			{
				if (this.strings == null)
				{
					this.strings = new XmlDictionaryString[4];
				}
				else if (this.nextId == this.strings.Length)
				{
					int num = this.nextId * 2;
					if (num == 0)
					{
						num = 4;
					}
					Array.Resize<XmlDictionaryString>(ref this.strings, num);
				}
				xmlDictionaryString = new XmlDictionaryString(this, value, this.nextId);
				this.strings[this.nextId] = xmlDictionaryString;
				this.lookup.Add(value, xmlDictionaryString);
				this.nextId++;
			}
			return xmlDictionaryString;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00014425 File Offset: 0x00012625
		public virtual bool TryLookup(string value, out XmlDictionaryString result)
		{
			return this.lookup.TryGetValue(value, out result);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00014434 File Offset: 0x00012634
		public virtual bool TryLookup(int key, out XmlDictionaryString result)
		{
			if (key < 0 || key >= this.nextId)
			{
				result = null;
				return false;
			}
			result = this.strings[key];
			return true;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00014453 File Offset: 0x00012653
		public virtual bool TryLookup(XmlDictionaryString value, out XmlDictionaryString result)
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

		// Token: 0x040001E0 RID: 480
		private static IXmlDictionary empty;

		// Token: 0x040001E1 RID: 481
		private Dictionary<string, XmlDictionaryString> lookup;

		// Token: 0x040001E2 RID: 482
		private XmlDictionaryString[] strings;

		// Token: 0x040001E3 RID: 483
		private int nextId;

		// Token: 0x02000151 RID: 337
		private class EmptyDictionary : IXmlDictionary
		{
			// Token: 0x060012A7 RID: 4775 RVA: 0x0004DC9C File Offset: 0x0004BE9C
			public bool TryLookup(string value, out XmlDictionaryString result)
			{
				result = null;
				return false;
			}

			// Token: 0x060012A8 RID: 4776 RVA: 0x0004DCA2 File Offset: 0x0004BEA2
			public bool TryLookup(int key, out XmlDictionaryString result)
			{
				result = null;
				return false;
			}

			// Token: 0x060012A9 RID: 4777 RVA: 0x0004DCA8 File Offset: 0x0004BEA8
			public bool TryLookup(XmlDictionaryString value, out XmlDictionaryString result)
			{
				result = null;
				return false;
			}
		}
	}
}
