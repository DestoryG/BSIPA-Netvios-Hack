using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000651 RID: 1617
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeRemoveEventStatement : CodeStatement
	{
		// Token: 0x06003AAC RID: 15020 RVA: 0x000F43C2 File Offset: 0x000F25C2
		public CodeRemoveEventStatement()
		{
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x000F43CA File Offset: 0x000F25CA
		public CodeRemoveEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
		{
			this.eventRef = eventRef;
			this.listener = listener;
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x000F43E0 File Offset: 0x000F25E0
		public CodeRemoveEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
		{
			this.eventRef = new CodeEventReferenceExpression(targetObject, eventName);
			this.listener = listener;
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003AAF RID: 15023 RVA: 0x000F43FC File Offset: 0x000F25FC
		// (set) Token: 0x06003AB0 RID: 15024 RVA: 0x000F4417 File Offset: 0x000F2617
		public CodeEventReferenceExpression Event
		{
			get
			{
				if (this.eventRef == null)
				{
					this.eventRef = new CodeEventReferenceExpression();
				}
				return this.eventRef;
			}
			set
			{
				this.eventRef = value;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x000F4420 File Offset: 0x000F2620
		// (set) Token: 0x06003AB2 RID: 15026 RVA: 0x000F4428 File Offset: 0x000F2628
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

		// Token: 0x04002C0E RID: 11278
		private CodeEventReferenceExpression eventRef;

		// Token: 0x04002C0F RID: 11279
		private CodeExpression listener;
	}
}
