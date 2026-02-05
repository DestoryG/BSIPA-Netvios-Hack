using System;
using System.Configuration;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000128 RID: 296
	[ConfigurationCollection(typeof(ParameterElement), AddItemName = "parameter", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public sealed class ParameterElementCollection : ConfigurationElementCollection
	{
		// Token: 0x060011E1 RID: 4577 RVA: 0x0004AF98 File Offset: 0x00049198
		public ParameterElementCollection()
		{
			base.AddElementName = "parameter";
		}

		// Token: 0x17000382 RID: 898
		public ParameterElement this[int index]
		{
			get
			{
				return (ParameterElement)base.BaseGet(index);
			}
			set
			{
				if (!this.IsReadOnly())
				{
					if (value == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
					}
					if (base.BaseGet(index) != null)
					{
						base.BaseRemoveAt(index);
					}
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0004AFE9 File Offset: 0x000491E9
		public void Add(ParameterElement element)
		{
			if (!this.IsReadOnly() && element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			this.BaseAdd(element);
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004B008 File Offset: 0x00049208
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0004B010 File Offset: 0x00049210
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0004B013 File Offset: 0x00049213
		public bool Contains(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
			}
			return base.BaseGet(typeName) != null;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004B032 File Offset: 0x00049232
		protected override ConfigurationElement CreateNewElement()
		{
			return new ParameterElement();
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x0004B039 File Offset: 0x00049239
		protected override string ElementName
		{
			get
			{
				return "parameter";
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004B040 File Offset: 0x00049240
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			return ((ParameterElement)element).identity;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0004B060 File Offset: 0x00049260
		public int IndexOf(ParameterElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			return base.BaseIndexOf(element);
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0004B077 File Offset: 0x00049277
		public void Remove(ParameterElement element)
		{
			if (!this.IsReadOnly() && element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004B09C File Offset: 0x0004929C
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
