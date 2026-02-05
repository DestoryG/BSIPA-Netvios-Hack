using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D4 RID: 1492
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class DesignerOptionService : IDesignerOptionService
	{
		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x0600377C RID: 14204 RVA: 0x000F03AD File Offset: 0x000EE5AD
		public DesignerOptionService.DesignerOptionCollection Options
		{
			get
			{
				if (this._options == null)
				{
					this._options = new DesignerOptionService.DesignerOptionCollection(this, null, string.Empty, null);
				}
				return this._options;
			}
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x000F03D0 File Offset: 0x000EE5D0
		protected DesignerOptionService.DesignerOptionCollection CreateOptionCollection(DesignerOptionService.DesignerOptionCollection parent, string name, object value)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					name.Length.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}), "name.Length");
			}
			return new DesignerOptionService.DesignerOptionCollection(this, parent, name, value);
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x000F044C File Offset: 0x000EE64C
		private PropertyDescriptor GetOptionProperty(string pageName, string valueName)
		{
			if (pageName == null)
			{
				throw new ArgumentNullException("pageName");
			}
			if (valueName == null)
			{
				throw new ArgumentNullException("valueName");
			}
			string[] array = pageName.Split(new char[] { '\\' });
			DesignerOptionService.DesignerOptionCollection designerOptionCollection = this.Options;
			foreach (string text in array)
			{
				designerOptionCollection = designerOptionCollection[text];
				if (designerOptionCollection == null)
				{
					return null;
				}
			}
			return designerOptionCollection.Properties[valueName];
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x000F04BD File Offset: 0x000EE6BD
		protected virtual void PopulateOptionCollection(DesignerOptionService.DesignerOptionCollection options)
		{
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x000F04BF File Offset: 0x000EE6BF
		protected virtual bool ShowDialog(DesignerOptionService.DesignerOptionCollection options, object optionObject)
		{
			return false;
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x000F04C4 File Offset: 0x000EE6C4
		object IDesignerOptionService.GetOptionValue(string pageName, string valueName)
		{
			PropertyDescriptor optionProperty = this.GetOptionProperty(pageName, valueName);
			if (optionProperty != null)
			{
				return optionProperty.GetValue(null);
			}
			return null;
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x000F04E8 File Offset: 0x000EE6E8
		void IDesignerOptionService.SetOptionValue(string pageName, string valueName, object value)
		{
			PropertyDescriptor optionProperty = this.GetOptionProperty(pageName, valueName);
			if (optionProperty != null)
			{
				optionProperty.SetValue(null, value);
			}
		}

		// Token: 0x04002AED RID: 10989
		private DesignerOptionService.DesignerOptionCollection _options;

		// Token: 0x020008AD RID: 2221
		[TypeConverter(typeof(DesignerOptionService.DesignerOptionConverter))]
		[Editor("", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public sealed class DesignerOptionCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060045FB RID: 17915 RVA: 0x00124450 File Offset: 0x00122650
			internal DesignerOptionCollection(DesignerOptionService service, DesignerOptionService.DesignerOptionCollection parent, string name, object value)
			{
				this._service = service;
				this._parent = parent;
				this._name = name;
				this._value = value;
				if (this._parent != null)
				{
					if (this._parent._children == null)
					{
						this._parent._children = new ArrayList(1);
					}
					this._parent._children.Add(this);
				}
			}

			// Token: 0x17000FCE RID: 4046
			// (get) Token: 0x060045FC RID: 17916 RVA: 0x001244B8 File Offset: 0x001226B8
			public int Count
			{
				get
				{
					this.EnsurePopulated();
					return this._children.Count;
				}
			}

			// Token: 0x17000FCF RID: 4047
			// (get) Token: 0x060045FD RID: 17917 RVA: 0x001244CB File Offset: 0x001226CB
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000FD0 RID: 4048
			// (get) Token: 0x060045FE RID: 17918 RVA: 0x001244D3 File Offset: 0x001226D3
			public DesignerOptionService.DesignerOptionCollection Parent
			{
				get
				{
					return this._parent;
				}
			}

			// Token: 0x17000FD1 RID: 4049
			// (get) Token: 0x060045FF RID: 17919 RVA: 0x001244DC File Offset: 0x001226DC
			public PropertyDescriptorCollection Properties
			{
				get
				{
					if (this._properties == null)
					{
						ArrayList arrayList;
						if (this._value != null)
						{
							PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this._value);
							arrayList = new ArrayList(properties.Count);
							using (IEnumerator enumerator = properties.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object obj = enumerator.Current;
									PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
									arrayList.Add(new DesignerOptionService.DesignerOptionCollection.WrappedPropertyDescriptor(propertyDescriptor, this._value));
								}
								goto IL_007A;
							}
						}
						arrayList = new ArrayList(1);
						IL_007A:
						this.EnsurePopulated();
						foreach (object obj2 in this._children)
						{
							DesignerOptionService.DesignerOptionCollection designerOptionCollection = (DesignerOptionService.DesignerOptionCollection)obj2;
							arrayList.AddRange(designerOptionCollection.Properties);
						}
						PropertyDescriptor[] array = (PropertyDescriptor[])arrayList.ToArray(typeof(PropertyDescriptor));
						this._properties = new PropertyDescriptorCollection(array, true);
					}
					return this._properties;
				}
			}

			// Token: 0x17000FD2 RID: 4050
			public DesignerOptionService.DesignerOptionCollection this[int index]
			{
				get
				{
					this.EnsurePopulated();
					if (index < 0 || index >= this._children.Count)
					{
						throw new IndexOutOfRangeException("index");
					}
					return (DesignerOptionService.DesignerOptionCollection)this._children[index];
				}
			}

			// Token: 0x17000FD3 RID: 4051
			public DesignerOptionService.DesignerOptionCollection this[string name]
			{
				get
				{
					this.EnsurePopulated();
					foreach (object obj in this._children)
					{
						DesignerOptionService.DesignerOptionCollection designerOptionCollection = (DesignerOptionService.DesignerOptionCollection)obj;
						if (string.Compare(designerOptionCollection.Name, name, true, CultureInfo.InvariantCulture) == 0)
						{
							return designerOptionCollection;
						}
					}
					return null;
				}
			}

			// Token: 0x06004602 RID: 17922 RVA: 0x001246A4 File Offset: 0x001228A4
			public void CopyTo(Array array, int index)
			{
				this.EnsurePopulated();
				this._children.CopyTo(array, index);
			}

			// Token: 0x06004603 RID: 17923 RVA: 0x001246B9 File Offset: 0x001228B9
			private void EnsurePopulated()
			{
				if (this._children == null)
				{
					this._service.PopulateOptionCollection(this);
					if (this._children == null)
					{
						this._children = new ArrayList(1);
					}
				}
			}

			// Token: 0x06004604 RID: 17924 RVA: 0x001246E3 File Offset: 0x001228E3
			public IEnumerator GetEnumerator()
			{
				this.EnsurePopulated();
				return this._children.GetEnumerator();
			}

			// Token: 0x06004605 RID: 17925 RVA: 0x001246F6 File Offset: 0x001228F6
			public int IndexOf(DesignerOptionService.DesignerOptionCollection value)
			{
				this.EnsurePopulated();
				return this._children.IndexOf(value);
			}

			// Token: 0x06004606 RID: 17926 RVA: 0x0012470C File Offset: 0x0012290C
			private static object RecurseFindValue(DesignerOptionService.DesignerOptionCollection options)
			{
				if (options._value != null)
				{
					return options._value;
				}
				foreach (object obj in options)
				{
					DesignerOptionService.DesignerOptionCollection designerOptionCollection = (DesignerOptionService.DesignerOptionCollection)obj;
					object obj2 = DesignerOptionService.DesignerOptionCollection.RecurseFindValue(designerOptionCollection);
					if (obj2 != null)
					{
						return obj2;
					}
				}
				return null;
			}

			// Token: 0x06004607 RID: 17927 RVA: 0x0012477C File Offset: 0x0012297C
			public bool ShowDialog()
			{
				object obj = DesignerOptionService.DesignerOptionCollection.RecurseFindValue(this);
				return obj != null && this._service.ShowDialog(this, obj);
			}

			// Token: 0x17000FD4 RID: 4052
			// (get) Token: 0x06004608 RID: 17928 RVA: 0x001247A2 File Offset: 0x001229A2
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000FD5 RID: 4053
			// (get) Token: 0x06004609 RID: 17929 RVA: 0x001247A5 File Offset: 0x001229A5
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000FD6 RID: 4054
			// (get) Token: 0x0600460A RID: 17930 RVA: 0x001247A8 File Offset: 0x001229A8
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000FD7 RID: 4055
			// (get) Token: 0x0600460B RID: 17931 RVA: 0x001247AB File Offset: 0x001229AB
			bool IList.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000FD8 RID: 4056
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x0600460E RID: 17934 RVA: 0x001247BE File Offset: 0x001229BE
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600460F RID: 17935 RVA: 0x001247C5 File Offset: 0x001229C5
			void IList.Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06004610 RID: 17936 RVA: 0x001247CC File Offset: 0x001229CC
			bool IList.Contains(object value)
			{
				this.EnsurePopulated();
				return this._children.Contains(value);
			}

			// Token: 0x06004611 RID: 17937 RVA: 0x001247E0 File Offset: 0x001229E0
			int IList.IndexOf(object value)
			{
				this.EnsurePopulated();
				return this._children.IndexOf(value);
			}

			// Token: 0x06004612 RID: 17938 RVA: 0x001247F4 File Offset: 0x001229F4
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06004613 RID: 17939 RVA: 0x001247FB File Offset: 0x001229FB
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06004614 RID: 17940 RVA: 0x00124802 File Offset: 0x00122A02
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x040037F6 RID: 14326
			private DesignerOptionService _service;

			// Token: 0x040037F7 RID: 14327
			private DesignerOptionService.DesignerOptionCollection _parent;

			// Token: 0x040037F8 RID: 14328
			private string _name;

			// Token: 0x040037F9 RID: 14329
			private object _value;

			// Token: 0x040037FA RID: 14330
			private ArrayList _children;

			// Token: 0x040037FB RID: 14331
			private PropertyDescriptorCollection _properties;

			// Token: 0x02000938 RID: 2360
			private sealed class WrappedPropertyDescriptor : PropertyDescriptor
			{
				// Token: 0x060046D0 RID: 18128 RVA: 0x00127D88 File Offset: 0x00125F88
				internal WrappedPropertyDescriptor(PropertyDescriptor property, object target)
					: base(property.Name, null)
				{
					this.property = property;
					this.target = target;
				}

				// Token: 0x17000FEC RID: 4076
				// (get) Token: 0x060046D1 RID: 18129 RVA: 0x00127DA5 File Offset: 0x00125FA5
				public override AttributeCollection Attributes
				{
					get
					{
						return this.property.Attributes;
					}
				}

				// Token: 0x17000FED RID: 4077
				// (get) Token: 0x060046D2 RID: 18130 RVA: 0x00127DB2 File Offset: 0x00125FB2
				public override Type ComponentType
				{
					get
					{
						return this.property.ComponentType;
					}
				}

				// Token: 0x17000FEE RID: 4078
				// (get) Token: 0x060046D3 RID: 18131 RVA: 0x00127DBF File Offset: 0x00125FBF
				public override bool IsReadOnly
				{
					get
					{
						return this.property.IsReadOnly;
					}
				}

				// Token: 0x17000FEF RID: 4079
				// (get) Token: 0x060046D4 RID: 18132 RVA: 0x00127DCC File Offset: 0x00125FCC
				public override Type PropertyType
				{
					get
					{
						return this.property.PropertyType;
					}
				}

				// Token: 0x060046D5 RID: 18133 RVA: 0x00127DD9 File Offset: 0x00125FD9
				public override bool CanResetValue(object component)
				{
					return this.property.CanResetValue(this.target);
				}

				// Token: 0x060046D6 RID: 18134 RVA: 0x00127DEC File Offset: 0x00125FEC
				public override object GetValue(object component)
				{
					return this.property.GetValue(this.target);
				}

				// Token: 0x060046D7 RID: 18135 RVA: 0x00127DFF File Offset: 0x00125FFF
				public override void ResetValue(object component)
				{
					this.property.ResetValue(this.target);
				}

				// Token: 0x060046D8 RID: 18136 RVA: 0x00127E12 File Offset: 0x00126012
				public override void SetValue(object component, object value)
				{
					this.property.SetValue(this.target, value);
				}

				// Token: 0x060046D9 RID: 18137 RVA: 0x00127E26 File Offset: 0x00126026
				public override bool ShouldSerializeValue(object component)
				{
					return this.property.ShouldSerializeValue(this.target);
				}

				// Token: 0x04003DE2 RID: 15842
				private object target;

				// Token: 0x04003DE3 RID: 15843
				private PropertyDescriptor property;
			}
		}

		// Token: 0x020008AE RID: 2222
		internal sealed class DesignerOptionConverter : TypeConverter
		{
			// Token: 0x06004615 RID: 17941 RVA: 0x00124809 File Offset: 0x00122A09
			public override bool GetPropertiesSupported(ITypeDescriptorContext cxt)
			{
				return true;
			}

			// Token: 0x06004616 RID: 17942 RVA: 0x0012480C File Offset: 0x00122A0C
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext cxt, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
				DesignerOptionService.DesignerOptionCollection designerOptionCollection = value as DesignerOptionService.DesignerOptionCollection;
				if (designerOptionCollection == null)
				{
					return propertyDescriptorCollection;
				}
				foreach (object obj in designerOptionCollection)
				{
					DesignerOptionService.DesignerOptionCollection designerOptionCollection2 = (DesignerOptionService.DesignerOptionCollection)obj;
					propertyDescriptorCollection.Add(new DesignerOptionService.DesignerOptionConverter.OptionPropertyDescriptor(designerOptionCollection2));
				}
				foreach (object obj2 in designerOptionCollection.Properties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
					propertyDescriptorCollection.Add(propertyDescriptor);
				}
				return propertyDescriptorCollection;
			}

			// Token: 0x06004617 RID: 17943 RVA: 0x001248D0 File Offset: 0x00122AD0
			public override object ConvertTo(ITypeDescriptorContext cxt, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return SR.GetString("CollectionConverterText");
				}
				return base.ConvertTo(cxt, culture, value, destinationType);
			}

			// Token: 0x02000939 RID: 2361
			private class OptionPropertyDescriptor : PropertyDescriptor
			{
				// Token: 0x060046DA RID: 18138 RVA: 0x00127E39 File Offset: 0x00126039
				internal OptionPropertyDescriptor(DesignerOptionService.DesignerOptionCollection option)
					: base(option.Name, null)
				{
					this._option = option;
				}

				// Token: 0x17000FF0 RID: 4080
				// (get) Token: 0x060046DB RID: 18139 RVA: 0x00127E4F File Offset: 0x0012604F
				public override Type ComponentType
				{
					get
					{
						return this._option.GetType();
					}
				}

				// Token: 0x17000FF1 RID: 4081
				// (get) Token: 0x060046DC RID: 18140 RVA: 0x00127E5C File Offset: 0x0012605C
				public override bool IsReadOnly
				{
					get
					{
						return true;
					}
				}

				// Token: 0x17000FF2 RID: 4082
				// (get) Token: 0x060046DD RID: 18141 RVA: 0x00127E5F File Offset: 0x0012605F
				public override Type PropertyType
				{
					get
					{
						return this._option.GetType();
					}
				}

				// Token: 0x060046DE RID: 18142 RVA: 0x00127E6C File Offset: 0x0012606C
				public override bool CanResetValue(object component)
				{
					return false;
				}

				// Token: 0x060046DF RID: 18143 RVA: 0x00127E6F File Offset: 0x0012606F
				public override object GetValue(object component)
				{
					return this._option;
				}

				// Token: 0x060046E0 RID: 18144 RVA: 0x00127E77 File Offset: 0x00126077
				public override void ResetValue(object component)
				{
				}

				// Token: 0x060046E1 RID: 18145 RVA: 0x00127E79 File Offset: 0x00126079
				public override void SetValue(object component, object value)
				{
				}

				// Token: 0x060046E2 RID: 18146 RVA: 0x00127E7B File Offset: 0x0012607B
				public override bool ShouldSerializeValue(object component)
				{
					return false;
				}

				// Token: 0x04003DE4 RID: 15844
				private DesignerOptionService.DesignerOptionCollection _option;
			}
		}
	}
}
