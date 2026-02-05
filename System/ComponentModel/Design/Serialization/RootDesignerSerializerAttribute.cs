using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000614 RID: 1556
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	[Obsolete("This attribute has been deprecated. Use DesignerSerializerAttribute instead.  For example, to specify a root designer for CodeDom, use DesignerSerializerAttribute(...,typeof(TypeCodeDomSerializer)).  http://go.microsoft.com/fwlink/?linkid=14202")]
	public sealed class RootDesignerSerializerAttribute : Attribute
	{
		// Token: 0x060038E7 RID: 14567 RVA: 0x000F206D File Offset: 0x000F026D
		public RootDesignerSerializerAttribute(Type serializerType, Type baseSerializerType, bool reloadable)
		{
			this.serializerTypeName = serializerType.AssemblyQualifiedName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
			this.reloadable = reloadable;
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x000F2094 File Offset: 0x000F0294
		public RootDesignerSerializerAttribute(string serializerTypeName, Type baseSerializerType, bool reloadable)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
			this.reloadable = reloadable;
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x000F20B6 File Offset: 0x000F02B6
		public RootDesignerSerializerAttribute(string serializerTypeName, string baseSerializerTypeName, bool reloadable)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerTypeName;
			this.reloadable = reloadable;
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x060038EA RID: 14570 RVA: 0x000F20D3 File Offset: 0x000F02D3
		public bool Reloadable
		{
			get
			{
				return this.reloadable;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x060038EB RID: 14571 RVA: 0x000F20DB File Offset: 0x000F02DB
		public string SerializerTypeName
		{
			get
			{
				return this.serializerTypeName;
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x060038EC RID: 14572 RVA: 0x000F20E3 File Offset: 0x000F02E3
		public string SerializerBaseTypeName
		{
			get
			{
				return this.serializerBaseTypeName;
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x060038ED RID: 14573 RVA: 0x000F20EC File Offset: 0x000F02EC
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

		// Token: 0x04002B76 RID: 11126
		private bool reloadable;

		// Token: 0x04002B77 RID: 11127
		private string serializerTypeName;

		// Token: 0x04002B78 RID: 11128
		private string serializerBaseTypeName;

		// Token: 0x04002B79 RID: 11129
		private string typeId;
	}
}
