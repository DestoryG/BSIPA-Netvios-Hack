using System;

namespace System.ComponentModel
{
	// Token: 0x0200056C RID: 1388
	[AttributeUsage(AttributeTargets.Class)]
	public class InstallerTypeAttribute : Attribute
	{
		// Token: 0x060033B2 RID: 13234 RVA: 0x000E3C9B File Offset: 0x000E1E9B
		public InstallerTypeAttribute(Type installerType)
		{
			this._typeName = installerType.AssemblyQualifiedName;
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x000E3CAF File Offset: 0x000E1EAF
		public InstallerTypeAttribute(string typeName)
		{
			this._typeName = typeName;
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x000E3CBE File Offset: 0x000E1EBE
		public virtual Type InstallerType
		{
			get
			{
				return Type.GetType(this._typeName);
			}
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000E3CCC File Offset: 0x000E1ECC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			InstallerTypeAttribute installerTypeAttribute = obj as InstallerTypeAttribute;
			return installerTypeAttribute != null && installerTypeAttribute._typeName == this._typeName;
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000E3CFC File Offset: 0x000E1EFC
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040029B7 RID: 10679
		private string _typeName;
	}
}
