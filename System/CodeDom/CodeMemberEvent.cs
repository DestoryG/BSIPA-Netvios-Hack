using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200063D RID: 1597
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMemberEvent : CodeTypeMember
	{
		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06003A03 RID: 14851 RVA: 0x000F3411 File Offset: 0x000F1611
		// (set) Token: 0x06003A04 RID: 14852 RVA: 0x000F3431 File Offset: 0x000F1631
		public CodeTypeReference Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = new CodeTypeReference("");
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06003A05 RID: 14853 RVA: 0x000F343A File Offset: 0x000F163A
		// (set) Token: 0x06003A06 RID: 14854 RVA: 0x000F3442 File Offset: 0x000F1642
		public CodeTypeReference PrivateImplementationType
		{
			get
			{
				return this.privateImplements;
			}
			set
			{
				this.privateImplements = value;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x000F344B File Offset: 0x000F164B
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				if (this.implementationTypes == null)
				{
					this.implementationTypes = new CodeTypeReferenceCollection();
				}
				return this.implementationTypes;
			}
		}

		// Token: 0x04002BCC RID: 11212
		private CodeTypeReference type;

		// Token: 0x04002BCD RID: 11213
		private CodeTypeReference privateImplements;

		// Token: 0x04002BCE RID: 11214
		private CodeTypeReferenceCollection implementationTypes;
	}
}
