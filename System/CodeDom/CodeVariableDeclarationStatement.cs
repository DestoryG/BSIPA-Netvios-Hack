using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000668 RID: 1640
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeVariableDeclarationStatement : CodeStatement
	{
		// Token: 0x06003B6D RID: 15213 RVA: 0x000F5860 File Offset: 0x000F3A60
		public CodeVariableDeclarationStatement()
		{
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x000F5868 File Offset: 0x000F3A68
		public CodeVariableDeclarationStatement(CodeTypeReference type, string name)
		{
			this.Type = type;
			this.Name = name;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x000F587E File Offset: 0x000F3A7E
		public CodeVariableDeclarationStatement(string type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x000F5899 File Offset: 0x000F3A99
		public CodeVariableDeclarationStatement(Type type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x000F58B4 File Offset: 0x000F3AB4
		public CodeVariableDeclarationStatement(CodeTypeReference type, string name, CodeExpression initExpression)
		{
			this.Type = type;
			this.Name = name;
			this.InitExpression = initExpression;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x000F58D1 File Offset: 0x000F3AD1
		public CodeVariableDeclarationStatement(string type, string name, CodeExpression initExpression)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
			this.InitExpression = initExpression;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x000F58F3 File Offset: 0x000F3AF3
		public CodeVariableDeclarationStatement(Type type, string name, CodeExpression initExpression)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
			this.InitExpression = initExpression;
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06003B74 RID: 15220 RVA: 0x000F5915 File Offset: 0x000F3B15
		// (set) Token: 0x06003B75 RID: 15221 RVA: 0x000F591D File Offset: 0x000F3B1D
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

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06003B76 RID: 15222 RVA: 0x000F5926 File Offset: 0x000F3B26
		// (set) Token: 0x06003B77 RID: 15223 RVA: 0x000F593C File Offset: 0x000F3B3C
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

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06003B78 RID: 15224 RVA: 0x000F5945 File Offset: 0x000F3B45
		// (set) Token: 0x06003B79 RID: 15225 RVA: 0x000F5965 File Offset: 0x000F3B65
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

		// Token: 0x04002C41 RID: 11329
		private CodeTypeReference type;

		// Token: 0x04002C42 RID: 11330
		private string name;

		// Token: 0x04002C43 RID: 11331
		private CodeExpression initExpression;
	}
}
