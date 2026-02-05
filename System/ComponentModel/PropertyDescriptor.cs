using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x0200059C RID: 1436
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class PropertyDescriptor : MemberDescriptor
	{
		// Token: 0x0600352D RID: 13613 RVA: 0x000E78B0 File Offset: 0x000E5AB0
		protected PropertyDescriptor(string name, Attribute[] attrs)
			: base(name, attrs)
		{
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x000E78BA File Offset: 0x000E5ABA
		protected PropertyDescriptor(MemberDescriptor descr)
			: base(descr)
		{
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000E78C3 File Offset: 0x000E5AC3
		protected PropertyDescriptor(MemberDescriptor descr, Attribute[] attrs)
			: base(descr, attrs)
		{
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003530 RID: 13616 RVA: 0x000E78CD File Offset: 0x000E5ACD
		private object SyncObject
		{
			get
			{
				return LazyInitializer.EnsureInitialized<object>(ref this.syncObject);
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003531 RID: 13617
		public abstract Type ComponentType { get; }

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003532 RID: 13618 RVA: 0x000E78DC File Offset: 0x000E5ADC
		public virtual TypeConverter Converter
		{
			get
			{
				AttributeCollection attributes = this.Attributes;
				if (this.converter == null)
				{
					TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)attributes[typeof(TypeConverterAttribute)];
					if (typeConverterAttribute.ConverterTypeName != null && typeConverterAttribute.ConverterTypeName.Length > 0)
					{
						Type typeFromName = this.GetTypeFromName(typeConverterAttribute.ConverterTypeName);
						if (typeFromName != null && typeof(TypeConverter).IsAssignableFrom(typeFromName))
						{
							this.converter = (TypeConverter)this.CreateInstance(typeFromName);
						}
					}
					if (this.converter == null)
					{
						this.converter = TypeDescriptor.GetConverter(this.PropertyType);
					}
				}
				return this.converter;
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003533 RID: 13619 RVA: 0x000E797D File Offset: 0x000E5B7D
		public virtual bool IsLocalizable
		{
			get
			{
				return LocalizableAttribute.Yes.Equals(this.Attributes[typeof(LocalizableAttribute)]);
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003534 RID: 13620
		public abstract bool IsReadOnly { get; }

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003535 RID: 13621 RVA: 0x000E79A0 File Offset: 0x000E5BA0
		public DesignerSerializationVisibility SerializationVisibility
		{
			get
			{
				DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = (DesignerSerializationVisibilityAttribute)this.Attributes[typeof(DesignerSerializationVisibilityAttribute)];
				return designerSerializationVisibilityAttribute.Visibility;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003536 RID: 13622
		public abstract Type PropertyType { get; }

		// Token: 0x06003537 RID: 13623 RVA: 0x000E79D0 File Offset: 0x000E5BD0
		public virtual void AddValueChanged(object component, EventHandler handler)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			object obj = this.SyncObject;
			lock (obj)
			{
				if (this.valueChangedHandlers == null)
				{
					this.valueChangedHandlers = new Hashtable();
				}
				EventHandler eventHandler = (EventHandler)this.valueChangedHandlers[component];
				this.valueChangedHandlers[component] = Delegate.Combine(eventHandler, handler);
			}
		}

		// Token: 0x06003538 RID: 13624
		public abstract bool CanResetValue(object component);

		// Token: 0x06003539 RID: 13625 RVA: 0x000E7A60 File Offset: 0x000E5C60
		public override bool Equals(object obj)
		{
			try
			{
				if (obj == this)
				{
					return true;
				}
				if (obj == null)
				{
					return false;
				}
				PropertyDescriptor propertyDescriptor = obj as PropertyDescriptor;
				if (propertyDescriptor != null && propertyDescriptor.NameHashCode == this.NameHashCode && propertyDescriptor.PropertyType == this.PropertyType && propertyDescriptor.Name.Equals(this.Name))
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000E7AD8 File Offset: 0x000E5CD8
		protected object CreateInstance(Type type)
		{
			Type[] array = new Type[] { typeof(Type) };
			ConstructorInfo constructor = type.GetConstructor(array);
			if (constructor != null)
			{
				return TypeDescriptor.CreateInstance(null, type, array, new object[] { this.PropertyType });
			}
			return TypeDescriptor.CreateInstance(null, type, null, null);
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000E7B2B File Offset: 0x000E5D2B
		protected override void FillAttributes(IList attributeList)
		{
			this.converter = null;
			this.editors = null;
			this.editorTypes = null;
			this.editorCount = 0;
			base.FillAttributes(attributeList);
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x000E7B50 File Offset: 0x000E5D50
		public PropertyDescriptorCollection GetChildProperties()
		{
			return this.GetChildProperties(null, null);
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000E7B5A File Offset: 0x000E5D5A
		public PropertyDescriptorCollection GetChildProperties(Attribute[] filter)
		{
			return this.GetChildProperties(null, filter);
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000E7B64 File Offset: 0x000E5D64
		public PropertyDescriptorCollection GetChildProperties(object instance)
		{
			return this.GetChildProperties(instance, null);
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000E7B6E File Offset: 0x000E5D6E
		public virtual PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			if (instance == null)
			{
				return TypeDescriptor.GetProperties(this.PropertyType, filter);
			}
			return TypeDescriptor.GetProperties(instance, filter);
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000E7B88 File Offset: 0x000E5D88
		public virtual object GetEditor(Type editorBaseType)
		{
			object obj = null;
			AttributeCollection attributes = this.Attributes;
			if (this.editorTypes != null)
			{
				for (int i = 0; i < this.editorCount; i++)
				{
					if (this.editorTypes[i] == editorBaseType)
					{
						return this.editors[i];
					}
				}
			}
			if (obj == null)
			{
				for (int j = 0; j < attributes.Count; j++)
				{
					EditorAttribute editorAttribute = attributes[j] as EditorAttribute;
					if (editorAttribute != null)
					{
						Type typeFromName = this.GetTypeFromName(editorAttribute.EditorBaseTypeName);
						if (editorBaseType == typeFromName)
						{
							Type typeFromName2 = this.GetTypeFromName(editorAttribute.EditorTypeName);
							if (typeFromName2 != null)
							{
								obj = this.CreateInstance(typeFromName2);
								break;
							}
						}
					}
				}
				if (obj == null)
				{
					obj = TypeDescriptor.GetEditor(this.PropertyType, editorBaseType);
				}
				if (this.editorTypes == null)
				{
					this.editorTypes = new Type[5];
					this.editors = new object[5];
				}
				if (this.editorCount >= this.editorTypes.Length)
				{
					Type[] array = new Type[this.editorTypes.Length * 2];
					object[] array2 = new object[this.editors.Length * 2];
					Array.Copy(this.editorTypes, array, this.editorTypes.Length);
					Array.Copy(this.editors, array2, this.editors.Length);
					this.editorTypes = array;
					this.editors = array2;
				}
				this.editorTypes[this.editorCount] = editorBaseType;
				object[] array3 = this.editors;
				int num = this.editorCount;
				this.editorCount = num + 1;
				array3[num] = obj;
			}
			return obj;
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000E7CFD File Offset: 0x000E5EFD
		public override int GetHashCode()
		{
			return this.NameHashCode ^ this.PropertyType.GetHashCode();
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000E7D14 File Offset: 0x000E5F14
		protected override object GetInvocationTarget(Type type, object instance)
		{
			object obj = base.GetInvocationTarget(type, instance);
			ICustomTypeDescriptor customTypeDescriptor = obj as ICustomTypeDescriptor;
			if (customTypeDescriptor != null)
			{
				obj = customTypeDescriptor.GetPropertyOwner(this);
			}
			return obj;
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000E7D40 File Offset: 0x000E5F40
		protected Type GetTypeFromName(string typeName)
		{
			if (typeName == null || typeName.Length == 0)
			{
				return null;
			}
			Type type = Type.GetType(typeName);
			Type type2 = null;
			if (this.ComponentType != null && (type == null || this.ComponentType.Assembly.FullName.Equals(type.Assembly.FullName)))
			{
				int num = typeName.IndexOf(',');
				if (num != -1)
				{
					typeName = typeName.Substring(0, num);
				}
				type2 = this.ComponentType.Assembly.GetType(typeName);
			}
			return type2 ?? type;
		}

		// Token: 0x06003544 RID: 13636
		public abstract object GetValue(object component);

		// Token: 0x06003545 RID: 13637 RVA: 0x000E7DCC File Offset: 0x000E5FCC
		protected virtual void OnValueChanged(object component, EventArgs e)
		{
			if (component != null && this.valueChangedHandlers != null)
			{
				EventHandler eventHandler = (EventHandler)this.valueChangedHandlers[component];
				if (eventHandler != null)
				{
					eventHandler(component, e);
				}
			}
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000E7E04 File Offset: 0x000E6004
		public virtual void RemoveValueChanged(object component, EventHandler handler)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (this.valueChangedHandlers != null)
			{
				object obj = this.SyncObject;
				lock (obj)
				{
					EventHandler eventHandler = (EventHandler)this.valueChangedHandlers[component];
					eventHandler = (EventHandler)Delegate.Remove(eventHandler, handler);
					if (eventHandler != null)
					{
						this.valueChangedHandlers[component] = eventHandler;
					}
					else
					{
						this.valueChangedHandlers.Remove(component);
					}
				}
			}
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000E7EA0 File Offset: 0x000E60A0
		protected internal EventHandler GetValueChangedHandler(object component)
		{
			if (component != null && this.valueChangedHandlers != null)
			{
				return (EventHandler)this.valueChangedHandlers[component];
			}
			return null;
		}

		// Token: 0x06003548 RID: 13640
		public abstract void ResetValue(object component);

		// Token: 0x06003549 RID: 13641
		public abstract void SetValue(object component, object value);

		// Token: 0x0600354A RID: 13642
		public abstract bool ShouldSerializeValue(object component);

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x0600354B RID: 13643 RVA: 0x000E7EC0 File Offset: 0x000E60C0
		public virtual bool SupportsChangeEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04002A35 RID: 10805
		private TypeConverter converter;

		// Token: 0x04002A36 RID: 10806
		private Hashtable valueChangedHandlers;

		// Token: 0x04002A37 RID: 10807
		private object[] editors;

		// Token: 0x04002A38 RID: 10808
		private Type[] editorTypes;

		// Token: 0x04002A39 RID: 10809
		private int editorCount;

		// Token: 0x04002A3A RID: 10810
		private object syncObject;
	}
}
