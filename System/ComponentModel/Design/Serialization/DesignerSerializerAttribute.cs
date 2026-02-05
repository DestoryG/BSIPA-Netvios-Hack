using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000607 RID: 1543
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public sealed class DesignerSerializerAttribute : Attribute
	{
		// Token: 0x060038A4 RID: 14500 RVA: 0x000F1A43 File Offset: 0x000EFC43
		public DesignerSerializerAttribute(Type serializerType, Type baseSerializerType)
		{
			this.serializerTypeName = serializerType.AssemblyQualifiedName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x000F1A63 File Offset: 0x000EFC63
		public DesignerSerializerAttribute(string serializerTypeName, Type baseSerializerType)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x000F1A7E File Offset: 0x000EFC7E
		public DesignerSerializerAttribute(string serializerTypeName, string baseSerializerTypeName)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerTypeName;
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x060038A7 RID: 14503 RVA: 0x000F1A94 File Offset: 0x000EFC94
		public string SerializerTypeName
		{
			get
			{
				return this.serializerTypeName;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x060038A8 RID: 14504 RVA: 0x000F1A9C File Offset: 0x000EFC9C
		public string SerializerBaseTypeName
		{
			get
			{
				return this.serializerBaseTypeName;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x060038A9 RID: 14505 RVA: 0x000F1AA4 File Offset: 0x000EFCA4
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					string text = this.serializerBaseTypeName;
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

		// Token: 0x04002B6A RID: 11114
		private string serializerTypeName;

		// Token: 0x04002B6B RID: 11115
		private string serializerBaseTypeName;

		// Token: 0x04002B6C RID: 11116
		private string typeId;
	}
}
