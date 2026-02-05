using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000633 RID: 1587
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeEventReferenceExpression : CodeExpression
	{
		// Token: 0x060039C5 RID: 14789 RVA: 0x000F306C File Offset: 0x000F126C
		public CodeEventReferenceExpression()
		{
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x000F3074 File Offset: 0x000F1274
		public CodeEventReferenceExpression(CodeExpression targetObject, string eventName)
		{
			this.targetObject = targetObject;
			this.eventName = eventName;
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x000F308A File Offset: 0x000F128A
		// (set) Token: 0x060039C8 RID: 14792 RVA: 0x000F3092 File Offset: 0x000F1292
		public CodeExpression TargetObject
		{
			get
			{
				return this.targetObject;
			}
			set
			{
				this.targetObject = value;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x060039C9 RID: 14793 RVA: 0x000F309B File Offset: 0x000F129B
		// (set) Token: 0x060039CA RID: 14794 RVA: 0x000F30B1 File Offset: 0x000F12B1
		public string EventName
		{
			get
			{
				if (this.eventName != null)
				{
					return this.eventName;
				}
				return string.Empty;
			}
			set
			{
				this.eventName = value;
			}
		}

		// Token: 0x04002BBC RID: 11196
		private CodeExpression targetObject;

		// Token: 0x04002BBD RID: 11197
		private string eventName;
	}
}
