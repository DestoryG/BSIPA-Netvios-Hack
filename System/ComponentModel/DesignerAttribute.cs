using System;
using System.ComponentModel.Design;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x02000541 RID: 1345
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public sealed class DesignerAttribute : Attribute
	{
		// Token: 0x060032A6 RID: 12966 RVA: 0x000E2350 File Offset: 0x000E0550
		public DesignerAttribute(string designerTypeName)
		{
			string text = designerTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.designerTypeName = designerTypeName;
			this.designerBaseTypeName = typeof(IDesigner).FullName;
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x000E238B File Offset: 0x000E058B
		public DesignerAttribute(Type designerType)
		{
			this.designerTypeName = designerType.AssemblyQualifiedName;
			this.designerBaseTypeName = typeof(IDesigner).FullName;
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x000E23B4 File Offset: 0x000E05B4
		public DesignerAttribute(string designerTypeName, string designerBaseTypeName)
		{
			string text = designerTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.designerTypeName = designerTypeName;
			this.designerBaseTypeName = designerBaseTypeName;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x000E23E4 File Offset: 0x000E05E4
		public DesignerAttribute(string designerTypeName, Type designerBaseType)
		{
			string text = designerTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.designerTypeName = designerTypeName;
			this.designerBaseTypeName = designerBaseType.AssemblyQualifiedName;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x000E2416 File Offset: 0x000E0616
		public DesignerAttribute(Type designerType, Type designerBaseType)
		{
			this.designerTypeName = designerType.AssemblyQualifiedName;
			this.designerBaseTypeName = designerBaseType.AssemblyQualifiedName;
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000E2436 File Offset: 0x000E0636
		public string DesignerBaseTypeName
		{
			get
			{
				return this.designerBaseTypeName;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x000E243E File Offset: 0x000E063E
		public string DesignerTypeName
		{
			get
			{
				return this.designerTypeName;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x000E2448 File Offset: 0x000E0648
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					string text = this.designerBaseTypeName;
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

		// Token: 0x060032AE RID: 12974 RVA: 0x000E2498 File Offset: 0x000E0698
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignerAttribute designerAttribute = obj as DesignerAttribute;
			return designerAttribute != null && designerAttribute.designerBaseTypeName == this.designerBaseTypeName && designerAttribute.designerTypeName == this.designerTypeName;
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000E24DB File Offset: 0x000E06DB
		public override int GetHashCode()
		{
			return this.designerTypeName.GetHashCode() ^ this.designerBaseTypeName.GetHashCode();
		}

		// Token: 0x0400297A RID: 10618
		private readonly string designerTypeName;

		// Token: 0x0400297B RID: 10619
		private readonly string designerBaseTypeName;

		// Token: 0x0400297C RID: 10620
		private string typeId;
	}
}
