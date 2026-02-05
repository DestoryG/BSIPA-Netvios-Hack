using System;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x020005B3 RID: 1459
	[AttributeUsage(AttributeTargets.All)]
	public sealed class TypeConverterAttribute : Attribute
	{
		// Token: 0x0600365E RID: 13918 RVA: 0x000ECAA7 File Offset: 0x000EACA7
		public TypeConverterAttribute()
		{
			this.typeName = string.Empty;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000ECABA File Offset: 0x000EACBA
		public TypeConverterAttribute(Type type)
		{
			this.typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000ECAD0 File Offset: 0x000EACD0
		public TypeConverterAttribute(string typeName)
		{
			string text = typeName.ToUpper(CultureInfo.InvariantCulture);
			this.typeName = typeName;
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06003661 RID: 13921 RVA: 0x000ECAF6 File Offset: 0x000EACF6
		public string ConverterTypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000ECB00 File Offset: 0x000EAD00
		public override bool Equals(object obj)
		{
			TypeConverterAttribute typeConverterAttribute = obj as TypeConverterAttribute;
			return typeConverterAttribute != null && typeConverterAttribute.ConverterTypeName == this.typeName;
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000ECB2A File Offset: 0x000EAD2A
		public override int GetHashCode()
		{
			return this.typeName.GetHashCode();
		}

		// Token: 0x04002A9B RID: 10907
		private string typeName;

		// Token: 0x04002A9C RID: 10908
		public static readonly TypeConverterAttribute Default = new TypeConverterAttribute();
	}
}
