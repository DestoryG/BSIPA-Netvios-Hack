using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200063E RID: 1598
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMemberField : CodeTypeMember
	{
		// Token: 0x06003A08 RID: 14856 RVA: 0x000F3466 File Offset: 0x000F1666
		public CodeMemberField()
		{
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x000F346E File Offset: 0x000F166E
		public CodeMemberField(CodeTypeReference type, string name)
		{
			this.Type = type;
			base.Name = name;
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x000F3484 File Offset: 0x000F1684
		public CodeMemberField(string type, string name)
		{
			this.Type = new CodeTypeReference(type);
			base.Name = name;
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x000F349F File Offset: 0x000F169F
		public CodeMemberField(Type type, string name)
		{
			this.Type = new CodeTypeReference(type);
			base.Name = name;
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06003A0C RID: 14860 RVA: 0x000F34BA File Offset: 0x000F16BA
		// (set) Token: 0x06003A0D RID: 14861 RVA: 0x000F34DA File Offset: 0x000F16DA
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

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003A0E RID: 14862 RVA: 0x000F34E3 File Offset: 0x000F16E3
		// (set) Token: 0x06003A0F RID: 14863 RVA: 0x000F34EB File Offset: 0x000F16EB
		public CodeExpression InitExpression
		{
			get
			{
				return this.initExpression;
			}
			set
			{
				this.initExpression = value;
			}
		}

		// Token: 0x04002BCF RID: 11215
		private CodeTypeReference type;

		// Token: 0x04002BD0 RID: 11216
		private CodeExpression initExpression;
	}
}
