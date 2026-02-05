using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005A1 RID: 1441
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ReferenceConverter : TypeConverter
	{
		// Token: 0x06003592 RID: 13714 RVA: 0x000E8992 File Offset: 0x000E6B92
		public ReferenceConverter(Type type)
		{
			this.type = type;
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000E89A1 File Offset: 0x000E6BA1
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return (sourceType == typeof(string) && context != null) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000E89C4 File Offset: 0x000E6BC4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (!string.Equals(text, ReferenceConverter.none) && context != null)
				{
					IReferenceService referenceService = (IReferenceService)context.GetService(typeof(IReferenceService));
					if (referenceService != null)
					{
						object reference = referenceService.GetReference(text);
						if (reference != null)
						{
							return reference;
						}
					}
					IContainer container = context.Container;
					if (container != null)
					{
						object obj = container.Components[text];
						if (obj != null)
						{
							return obj;
						}
					}
				}
				return null;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000E8A48 File Offset: 0x000E6C48
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(destinationType == typeof(string)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value != null)
			{
				if (context != null)
				{
					IReferenceService referenceService = (IReferenceService)context.GetService(typeof(IReferenceService));
					if (referenceService != null)
					{
						string name = referenceService.GetName(value);
						if (name != null)
						{
							return name;
						}
					}
				}
				if (!Marshal.IsComObject(value) && value is IComponent)
				{
					IComponent component = (IComponent)value;
					ISite site = component.Site;
					if (site != null)
					{
						string name2 = site.Name;
						if (name2 != null)
						{
							return name2;
						}
					}
				}
				return string.Empty;
			}
			return ReferenceConverter.none;
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000E8AF0 File Offset: 0x000E6CF0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			object[] array = null;
			if (context != null)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(null);
				IReferenceService referenceService = (IReferenceService)context.GetService(typeof(IReferenceService));
				if (referenceService != null)
				{
					object[] references = referenceService.GetReferences(this.type);
					int num = references.Length;
					for (int i = 0; i < num; i++)
					{
						if (this.IsValueAllowed(context, references[i]))
						{
							arrayList.Add(references[i]);
						}
					}
				}
				else
				{
					IContainer container = context.Container;
					if (container != null)
					{
						ComponentCollection components = container.Components;
						foreach (object obj in components)
						{
							IComponent component = (IComponent)obj;
							if (component != null && this.type.IsInstanceOfType(component) && this.IsValueAllowed(context, component))
							{
								arrayList.Add(component);
							}
						}
					}
				}
				array = arrayList.ToArray();
				Array.Sort(array, 0, array.Length, new ReferenceConverter.ReferenceComparer(this));
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000E8C0C File Offset: 0x000E6E0C
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000E8C0F File Offset: 0x000E6E0F
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000E8C12 File Offset: 0x000E6E12
		protected virtual bool IsValueAllowed(ITypeDescriptorContext context, object value)
		{
			return true;
		}

		// Token: 0x04002A4F RID: 10831
		private static readonly string none = SR.GetString("toStringNone");

		// Token: 0x04002A50 RID: 10832
		private Type type;

		// Token: 0x0200089C RID: 2204
		private class ReferenceComparer : IComparer
		{
			// Token: 0x060045AC RID: 17836 RVA: 0x00123364 File Offset: 0x00121564
			public ReferenceComparer(ReferenceConverter converter)
			{
				this.converter = converter;
			}

			// Token: 0x060045AD RID: 17837 RVA: 0x00123374 File Offset: 0x00121574
			public int Compare(object item1, object item2)
			{
				string text = this.converter.ConvertToString(item1);
				string text2 = this.converter.ConvertToString(item2);
				return string.Compare(text, text2, false, CultureInfo.InvariantCulture);
			}

			// Token: 0x040037DD RID: 14301
			private ReferenceConverter converter;
		}
	}
}
