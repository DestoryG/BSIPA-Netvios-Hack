using System;

namespace System.ComponentModel
{
	// Token: 0x0200055C RID: 1372
	[Obsolete("This interface has been deprecated. Add a TypeDescriptionProvider to handle type TypeDescriptor.ComObjectType instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface IComNativeDescriptorHandler
	{
		// Token: 0x06003376 RID: 13174
		AttributeCollection GetAttributes(object component);

		// Token: 0x06003377 RID: 13175
		string GetClassName(object component);

		// Token: 0x06003378 RID: 13176
		TypeConverter GetConverter(object component);

		// Token: 0x06003379 RID: 13177
		EventDescriptor GetDefaultEvent(object component);

		// Token: 0x0600337A RID: 13178
		PropertyDescriptor GetDefaultProperty(object component);

		// Token: 0x0600337B RID: 13179
		object GetEditor(object component, Type baseEditorType);

		// Token: 0x0600337C RID: 13180
		string GetName(object component);

		// Token: 0x0600337D RID: 13181
		EventDescriptorCollection GetEvents(object component);

		// Token: 0x0600337E RID: 13182
		EventDescriptorCollection GetEvents(object component, Attribute[] attributes);

		// Token: 0x0600337F RID: 13183
		PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes);

		// Token: 0x06003380 RID: 13184
		object GetPropertyValue(object component, string propertyName, ref bool success);

		// Token: 0x06003381 RID: 13185
		object GetPropertyValue(object component, int dispid, ref bool success);
	}
}
