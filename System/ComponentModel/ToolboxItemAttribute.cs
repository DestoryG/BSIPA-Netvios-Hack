using System;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x020005C5 RID: 1477
	[AttributeUsage(AttributeTargets.All)]
	public class ToolboxItemAttribute : Attribute
	{
		// Token: 0x06003739 RID: 14137 RVA: 0x000EFEC3 File Offset: 0x000EE0C3
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ToolboxItemAttribute.Default);
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x000EFED0 File Offset: 0x000EE0D0
		public ToolboxItemAttribute(bool defaultType)
		{
			if (defaultType)
			{
				this.toolboxItemTypeName = "System.Drawing.Design.ToolboxItem, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			}
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000EFEE8 File Offset: 0x000EE0E8
		public ToolboxItemAttribute(string toolboxItemTypeName)
		{
			string text = toolboxItemTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.toolboxItemTypeName = toolboxItemTypeName;
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000EFF0E File Offset: 0x000EE10E
		public ToolboxItemAttribute(Type toolboxItemType)
		{
			this.toolboxItemType = toolboxItemType;
			this.toolboxItemTypeName = toolboxItemType.AssemblyQualifiedName;
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x000EFF2C File Offset: 0x000EE12C
		public Type ToolboxItemType
		{
			get
			{
				if (this.toolboxItemType == null && this.toolboxItemTypeName != null)
				{
					try
					{
						this.toolboxItemType = Type.GetType(this.toolboxItemTypeName, true);
					}
					catch (Exception ex)
					{
						throw new ArgumentException(SR.GetString("ToolboxItemAttributeFailedGetType", new object[] { this.toolboxItemTypeName }), ex);
					}
				}
				return this.toolboxItemType;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x0600373E RID: 14142 RVA: 0x000EFF9C File Offset: 0x000EE19C
		public string ToolboxItemTypeName
		{
			get
			{
				if (this.toolboxItemTypeName == null)
				{
					return string.Empty;
				}
				return this.toolboxItemTypeName;
			}
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000EFFB4 File Offset: 0x000EE1B4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ToolboxItemAttribute toolboxItemAttribute = obj as ToolboxItemAttribute;
			return toolboxItemAttribute != null && toolboxItemAttribute.ToolboxItemTypeName == this.ToolboxItemTypeName;
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x000EFFE4 File Offset: 0x000EE1E4
		public override int GetHashCode()
		{
			if (this.toolboxItemTypeName != null)
			{
				return this.toolboxItemTypeName.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x04002AD7 RID: 10967
		private Type toolboxItemType;

		// Token: 0x04002AD8 RID: 10968
		private string toolboxItemTypeName;

		// Token: 0x04002AD9 RID: 10969
		public static readonly ToolboxItemAttribute Default = new ToolboxItemAttribute("System.Drawing.Design.ToolboxItem, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

		// Token: 0x04002ADA RID: 10970
		public static readonly ToolboxItemAttribute None = new ToolboxItemAttribute(false);
	}
}
