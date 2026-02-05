using System;

namespace System.ComponentModel
{
	// Token: 0x0200055F RID: 1375
	public interface ICustomTypeDescriptor
	{
		// Token: 0x0600338A RID: 13194
		AttributeCollection GetAttributes();

		// Token: 0x0600338B RID: 13195
		string GetClassName();

		// Token: 0x0600338C RID: 13196
		string GetComponentName();

		// Token: 0x0600338D RID: 13197
		TypeConverter GetConverter();

		// Token: 0x0600338E RID: 13198
		EventDescriptor GetDefaultEvent();

		// Token: 0x0600338F RID: 13199
		PropertyDescriptor GetDefaultProperty();

		// Token: 0x06003390 RID: 13200
		object GetEditor(Type editorBaseType);

		// Token: 0x06003391 RID: 13201
		EventDescriptorCollection GetEvents();

		// Token: 0x06003392 RID: 13202
		EventDescriptorCollection GetEvents(Attribute[] attributes);

		// Token: 0x06003393 RID: 13203
		PropertyDescriptorCollection GetProperties();

		// Token: 0x06003394 RID: 13204
		PropertyDescriptorCollection GetProperties(Attribute[] attributes);

		// Token: 0x06003395 RID: 13205
		object GetPropertyOwner(PropertyDescriptor pd);
	}
}
