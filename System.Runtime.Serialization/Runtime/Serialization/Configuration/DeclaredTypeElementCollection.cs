using System;
using System.Configuration;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000123 RID: 291
	[ConfigurationCollection(typeof(DeclaredTypeElement))]
	public sealed class DeclaredTypeElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000379 RID: 889
		public DeclaredTypeElement this[int index]
		{
			get
			{
				return (DeclaredTypeElement)base.BaseGet(index);
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

		// Token: 0x1700037A RID: 890
		public DeclaredTypeElement this[string typeName]
		{
			get
			{
				if (string.IsNullOrEmpty(typeName))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
				}
				return (DeclaredTypeElement)base.BaseGet(typeName);
			}
			set
			{
				if (!this.IsReadOnly())
				{
					if (string.IsNullOrEmpty(typeName))
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
					}
					if (value == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
					}
					if (base.BaseGet(typeName) == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new IndexOutOfRangeException(SR.GetString("For type '{0}', configuration index is out of range.", new object[] { typeName })));
					}
					base.BaseRemove(typeName);
				}
				this.Add(value);
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004AB8A File Offset: 0x00048D8A
		public void Add(DeclaredTypeElement element)
		{
			if (!this.IsReadOnly() && element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			this.BaseAdd(element);
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0004ABA9 File Offset: 0x00048DA9
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0004ABB1 File Offset: 0x00048DB1
		public bool Contains(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
			}
			return base.BaseGet(typeName) != null;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004ABD0 File Offset: 0x00048DD0
		protected override ConfigurationElement CreateNewElement()
		{
			return new DeclaredTypeElement();
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0004ABD7 File Offset: 0x00048DD7
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			return ((DeclaredTypeElement)element).Type;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0004ABF2 File Offset: 0x00048DF2
		public int IndexOf(DeclaredTypeElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			return base.BaseIndexOf(element);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0004AC09 File Offset: 0x00048E09
		public void Remove(DeclaredTypeElement element)
		{
			if (!this.IsReadOnly() && element == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
			}
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0004AC2E File Offset: 0x00048E2E
		public void Remove(string typeName)
		{
			if (!this.IsReadOnly() && string.IsNullOrEmpty(typeName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
			}
			base.BaseRemove(typeName);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0004AC52 File Offset: 0x00048E52
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
