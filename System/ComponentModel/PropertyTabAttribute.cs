using System;
using System.Reflection;

namespace System.ComponentModel
{
	// Token: 0x020005C1 RID: 1473
	[AttributeUsage(AttributeTargets.All)]
	public class PropertyTabAttribute : Attribute
	{
		// Token: 0x06003725 RID: 14117 RVA: 0x000EFAB7 File Offset: 0x000EDCB7
		public PropertyTabAttribute()
		{
			this.tabScopes = new PropertyTabScope[0];
			this.tabClassNames = new string[0];
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x000EFAD7 File Offset: 0x000EDCD7
		public PropertyTabAttribute(Type tabClass)
			: this(tabClass, PropertyTabScope.Component)
		{
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000EFAE1 File Offset: 0x000EDCE1
		public PropertyTabAttribute(string tabClassName)
			: this(tabClassName, PropertyTabScope.Component)
		{
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000EFAEC File Offset: 0x000EDCEC
		public PropertyTabAttribute(Type tabClass, PropertyTabScope tabScope)
		{
			this.tabClasses = new Type[] { tabClass };
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyTabAttributeBadPropertyTabScope"), "tabScope");
			}
			this.tabScopes = new PropertyTabScope[] { tabScope };
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000EFB38 File Offset: 0x000EDD38
		public PropertyTabAttribute(string tabClassName, PropertyTabScope tabScope)
		{
			this.tabClassNames = new string[] { tabClassName };
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyTabAttributeBadPropertyTabScope"), "tabScope");
			}
			this.tabScopes = new PropertyTabScope[] { tabScope };
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x0600372A RID: 14122 RVA: 0x000EFB84 File Offset: 0x000EDD84
		public Type[] TabClasses
		{
			get
			{
				if (this.tabClasses == null && this.tabClassNames != null)
				{
					this.tabClasses = new Type[this.tabClassNames.Length];
					for (int i = 0; i < this.tabClassNames.Length; i++)
					{
						int num = this.tabClassNames[i].IndexOf(',');
						string text = null;
						string text2;
						if (num != -1)
						{
							text2 = this.tabClassNames[i].Substring(0, num).Trim();
							text = this.tabClassNames[i].Substring(num + 1).Trim();
						}
						else
						{
							text2 = this.tabClassNames[i];
						}
						this.tabClasses[i] = Type.GetType(text2, false);
						if (this.tabClasses[i] == null)
						{
							if (text == null)
							{
								throw new TypeLoadException(SR.GetString("PropertyTabAttributeTypeLoadException", new object[] { text2 }));
							}
							Assembly assembly = Assembly.Load(text);
							if (assembly != null)
							{
								this.tabClasses[i] = assembly.GetType(text2, true);
							}
						}
					}
				}
				return this.tabClasses;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x0600372B RID: 14123 RVA: 0x000EFC88 File Offset: 0x000EDE88
		protected string[] TabClassNames
		{
			get
			{
				if (this.tabClassNames != null)
				{
					return (string[])this.tabClassNames.Clone();
				}
				return null;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x0600372C RID: 14124 RVA: 0x000EFCA4 File Offset: 0x000EDEA4
		public PropertyTabScope[] TabScopes
		{
			get
			{
				return this.tabScopes;
			}
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x000EFCAC File Offset: 0x000EDEAC
		public override bool Equals(object other)
		{
			return other is PropertyTabAttribute && this.Equals((PropertyTabAttribute)other);
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x000EFCC4 File Offset: 0x000EDEC4
		public bool Equals(PropertyTabAttribute other)
		{
			if (other == this)
			{
				return true;
			}
			if (other.TabClasses.Length != this.TabClasses.Length || other.TabScopes.Length != this.TabScopes.Length)
			{
				return false;
			}
			for (int i = 0; i < this.TabClasses.Length; i++)
			{
				if (this.TabClasses[i] != other.TabClasses[i] || this.TabScopes[i] != other.TabScopes[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x000EFD3C File Offset: 0x000EDF3C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x000EFD44 File Offset: 0x000EDF44
		protected void InitializeArrays(string[] tabClassNames, PropertyTabScope[] tabScopes)
		{
			this.InitializeArrays(tabClassNames, null, tabScopes);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x000EFD4F File Offset: 0x000EDF4F
		protected void InitializeArrays(Type[] tabClasses, PropertyTabScope[] tabScopes)
		{
			this.InitializeArrays(null, tabClasses, tabScopes);
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000EFD5C File Offset: 0x000EDF5C
		private void InitializeArrays(string[] tabClassNames, Type[] tabClasses, PropertyTabScope[] tabScopes)
		{
			if (tabClasses != null)
			{
				if (tabScopes != null && tabClasses.Length != tabScopes.Length)
				{
					throw new ArgumentException(SR.GetString("PropertyTabAttributeArrayLengthMismatch"));
				}
				this.tabClasses = (Type[])tabClasses.Clone();
			}
			else if (tabClassNames != null)
			{
				if (tabScopes != null && tabClasses.Length != tabScopes.Length)
				{
					throw new ArgumentException(SR.GetString("PropertyTabAttributeArrayLengthMismatch"));
				}
				this.tabClassNames = (string[])tabClassNames.Clone();
				this.tabClasses = null;
			}
			else if (this.tabClasses == null && this.tabClassNames == null)
			{
				throw new ArgumentException(SR.GetString("PropertyTabAttributeParamsBothNull"));
			}
			if (tabScopes != null)
			{
				for (int i = 0; i < tabScopes.Length; i++)
				{
					if (tabScopes[i] < PropertyTabScope.Document)
					{
						throw new ArgumentException(SR.GetString("PropertyTabAttributeBadPropertyTabScope"));
					}
				}
				this.tabScopes = (PropertyTabScope[])tabScopes.Clone();
				return;
			}
			this.tabScopes = new PropertyTabScope[tabClasses.Length];
			for (int j = 0; j < this.TabScopes.Length; j++)
			{
				this.tabScopes[j] = PropertyTabScope.Component;
			}
		}

		// Token: 0x04002AC7 RID: 10951
		private PropertyTabScope[] tabScopes;

		// Token: 0x04002AC8 RID: 10952
		private Type[] tabClasses;

		// Token: 0x04002AC9 RID: 10953
		private string[] tabClassNames;
	}
}
