using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000515 RID: 1301
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class AttributeCollection : ICollection, IEnumerable
	{
		// Token: 0x06003135 RID: 12597 RVA: 0x000DEC6C File Offset: 0x000DCE6C
		public AttributeCollection(params Attribute[] attributes)
		{
			if (attributes == null)
			{
				attributes = new Attribute[0];
			}
			this._attributes = attributes;
			for (int i = 0; i < attributes.Length; i++)
			{
				if (attributes[i] == null)
				{
					throw new ArgumentNullException("attributes");
				}
			}
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000DECAF File Offset: 0x000DCEAF
		protected AttributeCollection()
		{
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000DECB8 File Offset: 0x000DCEB8
		public static AttributeCollection FromExisting(AttributeCollection existing, params Attribute[] newAttributes)
		{
			if (existing == null)
			{
				throw new ArgumentNullException("existing");
			}
			if (newAttributes == null)
			{
				newAttributes = new Attribute[0];
			}
			Attribute[] array = new Attribute[existing.Count + newAttributes.Length];
			int count = existing.Count;
			existing.CopyTo(array, 0);
			for (int i = 0; i < newAttributes.Length; i++)
			{
				if (newAttributes[i] == null)
				{
					throw new ArgumentNullException("newAttributes");
				}
				bool flag = false;
				for (int j = 0; j < existing.Count; j++)
				{
					if (array[j].TypeId.Equals(newAttributes[i].TypeId))
					{
						flag = true;
						array[j] = newAttributes[i];
						break;
					}
				}
				if (!flag)
				{
					array[count++] = newAttributes[i];
				}
			}
			Attribute[] array2;
			if (count < array.Length)
			{
				array2 = new Attribute[count];
				Array.Copy(array, 0, array2, 0, count);
			}
			else
			{
				array2 = array;
			}
			return new AttributeCollection(array2);
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06003138 RID: 12600 RVA: 0x000DED88 File Offset: 0x000DCF88
		protected virtual Attribute[] Attributes
		{
			get
			{
				return this._attributes;
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x000DED90 File Offset: 0x000DCF90
		public int Count
		{
			get
			{
				return this.Attributes.Length;
			}
		}

		// Token: 0x17000C09 RID: 3081
		public virtual Attribute this[int index]
		{
			get
			{
				return this.Attributes[index];
			}
		}

		// Token: 0x17000C0A RID: 3082
		public virtual Attribute this[Type attributeType]
		{
			get
			{
				object obj = AttributeCollection.internalSyncObject;
				Attribute defaultAttribute;
				lock (obj)
				{
					if (this._foundAttributeTypes == null)
					{
						this._foundAttributeTypes = new AttributeCollection.AttributeEntry[5];
					}
					int i = 0;
					while (i < 5)
					{
						if (this._foundAttributeTypes[i].type == attributeType)
						{
							int index = this._foundAttributeTypes[i].index;
							if (index != -1)
							{
								return this.Attributes[index];
							}
							return this.GetDefaultAttribute(attributeType);
						}
						else
						{
							if (this._foundAttributeTypes[i].type == null)
							{
								break;
							}
							i++;
						}
					}
					int index2 = this._index;
					this._index = index2 + 1;
					i = index2;
					if (this._index >= 5)
					{
						this._index = 0;
					}
					this._foundAttributeTypes[i].type = attributeType;
					int num = this.Attributes.Length;
					for (int j = 0; j < num; j++)
					{
						Attribute attribute = this.Attributes[j];
						Type type = attribute.GetType();
						if (type == attributeType)
						{
							this._foundAttributeTypes[i].index = j;
							return attribute;
						}
					}
					for (int k = 0; k < num; k++)
					{
						Attribute attribute2 = this.Attributes[k];
						Type type2 = attribute2.GetType();
						if (attributeType.IsAssignableFrom(type2))
						{
							this._foundAttributeTypes[i].index = k;
							return attribute2;
						}
					}
					this._foundAttributeTypes[i].index = -1;
					defaultAttribute = this.GetDefaultAttribute(attributeType);
				}
				return defaultAttribute;
			}
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000DEF5C File Offset: 0x000DD15C
		public bool Contains(Attribute attribute)
		{
			Attribute attribute2 = this[attribute.GetType()];
			return attribute2 != null && attribute2.Equals(attribute);
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000DEF88 File Offset: 0x000DD188
		public bool Contains(Attribute[] attributes)
		{
			if (attributes == null)
			{
				return true;
			}
			for (int i = 0; i < attributes.Length; i++)
			{
				if (!this.Contains(attributes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000DEFB8 File Offset: 0x000DD1B8
		protected Attribute GetDefaultAttribute(Type attributeType)
		{
			object obj = AttributeCollection.internalSyncObject;
			Attribute attribute;
			lock (obj)
			{
				if (AttributeCollection._defaultAttributes == null)
				{
					AttributeCollection._defaultAttributes = new Hashtable();
				}
				if (AttributeCollection._defaultAttributes.ContainsKey(attributeType))
				{
					attribute = (Attribute)AttributeCollection._defaultAttributes[attributeType];
				}
				else
				{
					Attribute attribute2 = null;
					Type reflectionType = TypeDescriptor.GetReflectionType(attributeType);
					FieldInfo field = reflectionType.GetField("Default", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
					if (field != null && field.IsStatic)
					{
						attribute2 = (Attribute)field.GetValue(null);
					}
					else
					{
						ConstructorInfo constructor = reflectionType.UnderlyingSystemType.GetConstructor(new Type[0]);
						if (constructor != null)
						{
							attribute2 = (Attribute)constructor.Invoke(new object[0]);
							if (!attribute2.IsDefaultAttribute())
							{
								attribute2 = null;
							}
						}
					}
					AttributeCollection._defaultAttributes[attributeType] = attribute2;
					attribute = attribute2;
				}
			}
			return attribute;
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000DF0B0 File Offset: 0x000DD2B0
		public IEnumerator GetEnumerator()
		{
			return this.Attributes.GetEnumerator();
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000DF0C0 File Offset: 0x000DD2C0
		public bool Matches(Attribute attribute)
		{
			for (int i = 0; i < this.Attributes.Length; i++)
			{
				if (this.Attributes[i].Match(attribute))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000DF0F4 File Offset: 0x000DD2F4
		public bool Matches(Attribute[] attributes)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				if (!this.Matches(attributes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x000DF11D File Offset: 0x000DD31D
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06003143 RID: 12611 RVA: 0x000DF125 File Offset: 0x000DD325
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x000DF128 File Offset: 0x000DD328
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000DF12B File Offset: 0x000DD32B
		public void CopyTo(Array array, int index)
		{
			Array.Copy(this.Attributes, 0, array, index, this.Attributes.Length);
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x000DF143 File Offset: 0x000DD343
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04002904 RID: 10500
		public static readonly AttributeCollection Empty = new AttributeCollection(null);

		// Token: 0x04002905 RID: 10501
		private static Hashtable _defaultAttributes;

		// Token: 0x04002906 RID: 10502
		private Attribute[] _attributes;

		// Token: 0x04002907 RID: 10503
		private static object internalSyncObject = new object();

		// Token: 0x04002908 RID: 10504
		private const int FOUND_TYPES_LIMIT = 5;

		// Token: 0x04002909 RID: 10505
		private AttributeCollection.AttributeEntry[] _foundAttributeTypes;

		// Token: 0x0400290A RID: 10506
		private int _index;

		// Token: 0x0200088F RID: 2191
		private struct AttributeEntry
		{
			// Token: 0x040037B7 RID: 14263
			public Type type;

			// Token: 0x040037B8 RID: 14264
			public int index;
		}
	}
}
