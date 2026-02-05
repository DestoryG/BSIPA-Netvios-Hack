using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200061B RID: 1563
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeAttributeArgument
	{
		// Token: 0x0600391B RID: 14619 RVA: 0x000F244D File Offset: 0x000F064D
		public CodeAttributeArgument()
		{
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x000F2455 File Offset: 0x000F0655
		public CodeAttributeArgument(CodeExpression value)
		{
			this.Value = value;
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x000F2464 File Offset: 0x000F0664
		public CodeAttributeArgument(string name, CodeExpression value)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x0600391E RID: 14622 RVA: 0x000F247A File Offset: 0x000F067A
		// (set) Token: 0x0600391F RID: 14623 RVA: 0x000F2490 File Offset: 0x000F0690
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003920 RID: 14624 RVA: 0x000F2499 File Offset: 0x000F0699
		// (set) Token: 0x06003921 RID: 14625 RVA: 0x000F24A1 File Offset: 0x000F06A1
		public CodeExpression Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04002B85 RID: 11141
		private string name;

		// Token: 0x04002B86 RID: 11142
		private CodeExpression value;
	}
}
