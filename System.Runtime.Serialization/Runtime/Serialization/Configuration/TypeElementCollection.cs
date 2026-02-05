using System;
using System.Configuration;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x0200012B RID: 299
	[ConfigurationCollection(typeof(TypeElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public sealed class TypeElementCollection : ConfigurationElementCollection
	{
		// Token: 0x1700038C RID: 908
		public TypeElement this[int index]
		{
			get
			{
				return (TypeElement)base.BaseGet(index);
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

		// Token: 0x06001201 RID: 4609 RVA: 0x0004B3F4 File Offset: 0x000495F4
		public void Add(TypeElement element)
		{
			if (!this.IsReadOnly() && element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			this.BaseAdd(element);
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0004B413 File Offset: 0x00049613
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x0004B41B File Offset: 0x0004961B
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0004B41E File Offset: 0x0004961E
		protected override ConfigurationElement CreateNewElement()
		{
			return new TypeElement();
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0004B425 File Offset: 0x00049625
		protected override string ElementName
		{
			get
			{
				return "knownType";
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0004B42C File Offset: 0x0004962C
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			return ((TypeElement)element).Key;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0004B447 File Offset: 0x00049647
		public int IndexOf(TypeElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			return base.BaseIndexOf(element);
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0004B45E File Offset: 0x0004965E
		public void Remove(TypeElement element)
		{
			if (!this.IsReadOnly() && element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0004B483 File Offset: 0x00049683
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x040008A2 RID: 2210
		private const string KnownTypeConfig = "knownType";
	}
}
