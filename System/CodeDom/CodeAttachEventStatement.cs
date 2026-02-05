using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200061A RID: 1562
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeAttachEventStatement : CodeStatement
	{
		// Token: 0x06003914 RID: 14612 RVA: 0x000F23E3 File Offset: 0x000F05E3
		public CodeAttachEventStatement()
		{
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x000F23EB File Offset: 0x000F05EB
		public CodeAttachEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
		{
			this.eventRef = eventRef;
			this.listener = listener;
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x000F2401 File Offset: 0x000F0601
		public CodeAttachEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
		{
			this.eventRef = new CodeEventReferenceExpression(targetObject, eventName);
			this.listener = listener;
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06003917 RID: 14615 RVA: 0x000F241D File Offset: 0x000F061D
		// (set) Token: 0x06003918 RID: 14616 RVA: 0x000F2433 File Offset: 0x000F0633
		public CodeEventReferenceExpression Event
		{
			get
			{
				if (this.eventRef == null)
				{
					return new CodeEventReferenceExpression();
				}
				return this.eventRef;
			}
			set
			{
				this.eventRef = value;
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x000F243C File Offset: 0x000F063C
		// (set) Token: 0x0600391A RID: 14618 RVA: 0x000F2444 File Offset: 0x000F0644
		public CodeExpression Listener
		{
			get
			{
				return this.listener;
			}
			set
			{
				this.listener = value;
			}
		}

		// Token: 0x04002B83 RID: 11139
		private CodeEventReferenceExpression eventRef;

		// Token: 0x04002B84 RID: 11140
		private CodeExpression listener;
	}
}
