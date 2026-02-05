using System;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x0200054B RID: 1355
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public sealed class EditorAttribute : Attribute
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x000E28A9 File Offset: 0x000E0AA9
		public EditorAttribute()
		{
			this.typeName = string.Empty;
			this.baseTypeName = string.Empty;
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000E28C8 File Offset: 0x000E0AC8
		public EditorAttribute(string typeName, string baseTypeName)
		{
			string text = typeName.ToUpper(CultureInfo.InvariantCulture);
			this.typeName = typeName;
			this.baseTypeName = baseTypeName;
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000E28F8 File Offset: 0x000E0AF8
		public EditorAttribute(string typeName, Type baseType)
		{
			string text = typeName.ToUpper(CultureInfo.InvariantCulture);
			this.typeName = typeName;
			this.baseTypeName = baseType.AssemblyQualifiedName;
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000E292A File Offset: 0x000E0B2A
		public EditorAttribute(Type type, Type baseType)
		{
			this.typeName = type.AssemblyQualifiedName;
			this.baseTypeName = baseType.AssemblyQualifiedName;
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x000E294A File Offset: 0x000E0B4A
		public string EditorBaseTypeName
		{
			get
			{
				return this.baseTypeName;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x060032E8 RID: 13032 RVA: 0x000E2952 File Offset: 0x000E0B52
		public string EditorTypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x000E295C File Offset: 0x000E0B5C
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					string text = this.baseTypeName;
					int num = text.IndexOf(',');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					this.typeId = base.GetType().FullName + text;
				}
				return this.typeId;
			}
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x000E29AC File Offset: 0x000E0BAC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			EditorAttribute editorAttribute = obj as EditorAttribute;
			return editorAttribute != null && editorAttribute.typeName == this.typeName && editorAttribute.baseTypeName == this.baseTypeName;
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x000E29EF File Offset: 0x000E0BEF
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04002998 RID: 10648
		private string baseTypeName;

		// Token: 0x04002999 RID: 10649
		private string typeName;

		// Token: 0x0400299A RID: 10650
		private string typeId;
	}
}
