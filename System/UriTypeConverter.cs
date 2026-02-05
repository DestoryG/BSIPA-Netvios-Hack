using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System
{
	// Token: 0x02000047 RID: 71
	public class UriTypeConverter : TypeConverter
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x0001BE93 File Offset: 0x0001A093
		public UriTypeConverter()
			: this(UriKind.RelativeOrAbsolute)
		{
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001BE9C File Offset: 0x0001A09C
		internal UriTypeConverter(UriKind uriKind)
		{
			this.m_UriKind = uriKind;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001BEAC File Offset: 0x0001A0AC
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return sourceType == typeof(string) || typeof(Uri).IsAssignableFrom(sourceType) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001BF00 File Offset: 0x0001A100
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || destinationType == typeof(Uri) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001BF54 File Offset: 0x0001A154
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				return new Uri(text, this.m_UriKind);
			}
			Uri uri = value as Uri;
			if (uri != null)
			{
				return new Uri(uri.OriginalString, (this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001BFBC File Offset: 0x0001A1BC
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			Uri uri = value as Uri;
			if (uri != null && destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo constructor = typeof(Uri).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[]
				{
					typeof(string),
					typeof(UriKind)
				}, null);
				return new InstanceDescriptor(constructor, new object[]
				{
					uri.OriginalString,
					(this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind
				});
			}
			if (uri != null && destinationType == typeof(string))
			{
				return uri.OriginalString;
			}
			if (uri != null && destinationType == typeof(Uri))
			{
				return new Uri(uri.OriginalString, (this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001C0D0 File Offset: 0x0001A2D0
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string text = value as string;
			if (text != null)
			{
				Uri uri;
				return Uri.TryCreate(text, this.m_UriKind, out uri);
			}
			return value is Uri;
		}

		// Token: 0x0400047A RID: 1146
		private UriKind m_UriKind;
	}
}
