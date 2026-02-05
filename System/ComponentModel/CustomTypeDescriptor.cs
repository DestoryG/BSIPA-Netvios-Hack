using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000532 RID: 1330
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class CustomTypeDescriptor : ICustomTypeDescriptor
	{
		// Token: 0x06003238 RID: 12856 RVA: 0x000E1429 File Offset: 0x000DF629
		protected CustomTypeDescriptor()
		{
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x000E1431 File Offset: 0x000DF631
		protected CustomTypeDescriptor(ICustomTypeDescriptor parent)
		{
			this._parent = parent;
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x000E1440 File Offset: 0x000DF640
		public virtual AttributeCollection GetAttributes()
		{
			if (this._parent != null)
			{
				return this._parent.GetAttributes();
			}
			return AttributeCollection.Empty;
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x000E145B File Offset: 0x000DF65B
		public virtual string GetClassName()
		{
			if (this._parent != null)
			{
				return this._parent.GetClassName();
			}
			return null;
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000E1472 File Offset: 0x000DF672
		public virtual string GetComponentName()
		{
			if (this._parent != null)
			{
				return this._parent.GetComponentName();
			}
			return null;
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000E1489 File Offset: 0x000DF689
		public virtual TypeConverter GetConverter()
		{
			if (this._parent != null)
			{
				return this._parent.GetConverter();
			}
			return new TypeConverter();
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x000E14A4 File Offset: 0x000DF6A4
		public virtual EventDescriptor GetDefaultEvent()
		{
			if (this._parent != null)
			{
				return this._parent.GetDefaultEvent();
			}
			return null;
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x000E14BB File Offset: 0x000DF6BB
		public virtual PropertyDescriptor GetDefaultProperty()
		{
			if (this._parent != null)
			{
				return this._parent.GetDefaultProperty();
			}
			return null;
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x000E14D2 File Offset: 0x000DF6D2
		public virtual object GetEditor(Type editorBaseType)
		{
			if (this._parent != null)
			{
				return this._parent.GetEditor(editorBaseType);
			}
			return null;
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x000E14EA File Offset: 0x000DF6EA
		public virtual EventDescriptorCollection GetEvents()
		{
			if (this._parent != null)
			{
				return this._parent.GetEvents();
			}
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000E1505 File Offset: 0x000DF705
		public virtual EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			if (this._parent != null)
			{
				return this._parent.GetEvents(attributes);
			}
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000E1521 File Offset: 0x000DF721
		public virtual PropertyDescriptorCollection GetProperties()
		{
			if (this._parent != null)
			{
				return this._parent.GetProperties();
			}
			return PropertyDescriptorCollection.Empty;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000E153C File Offset: 0x000DF73C
		public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			if (this._parent != null)
			{
				return this._parent.GetProperties(attributes);
			}
			return PropertyDescriptorCollection.Empty;
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x000E1558 File Offset: 0x000DF758
		public virtual object GetPropertyOwner(PropertyDescriptor pd)
		{
			if (this._parent != null)
			{
				return this._parent.GetPropertyOwner(pd);
			}
			return null;
		}

		// Token: 0x0400295E RID: 10590
		private ICustomTypeDescriptor _parent;
	}
}
