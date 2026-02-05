using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000617 RID: 1559
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeArrayCreateExpression : CodeExpression
	{
		// Token: 0x060038F8 RID: 14584 RVA: 0x000F218D File Offset: 0x000F038D
		public CodeArrayCreateExpression()
		{
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x000F21A0 File Offset: 0x000F03A0
		public CodeArrayCreateExpression(CodeTypeReference createType, params CodeExpression[] initializers)
		{
			this.createType = createType;
			this.initializers.AddRange(initializers);
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x000F21C6 File Offset: 0x000F03C6
		public CodeArrayCreateExpression(string createType, params CodeExpression[] initializers)
		{
			this.createType = new CodeTypeReference(createType);
			this.initializers.AddRange(initializers);
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x000F21F1 File Offset: 0x000F03F1
		public CodeArrayCreateExpression(Type createType, params CodeExpression[] initializers)
		{
			this.createType = new CodeTypeReference(createType);
			this.initializers.AddRange(initializers);
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x000F221C File Offset: 0x000F041C
		public CodeArrayCreateExpression(CodeTypeReference createType, int size)
		{
			this.createType = createType;
			this.size = size;
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x000F223D File Offset: 0x000F043D
		public CodeArrayCreateExpression(string createType, int size)
		{
			this.createType = new CodeTypeReference(createType);
			this.size = size;
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x000F2263 File Offset: 0x000F0463
		public CodeArrayCreateExpression(Type createType, int size)
		{
			this.createType = new CodeTypeReference(createType);
			this.size = size;
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x000F2289 File Offset: 0x000F0489
		public CodeArrayCreateExpression(CodeTypeReference createType, CodeExpression size)
		{
			this.createType = createType;
			this.sizeExpression = size;
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x000F22AA File Offset: 0x000F04AA
		public CodeArrayCreateExpression(string createType, CodeExpression size)
		{
			this.createType = new CodeTypeReference(createType);
			this.sizeExpression = size;
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x000F22D0 File Offset: 0x000F04D0
		public CodeArrayCreateExpression(Type createType, CodeExpression size)
		{
			this.createType = new CodeTypeReference(createType);
			this.sizeExpression = size;
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06003902 RID: 14594 RVA: 0x000F22F6 File Offset: 0x000F04F6
		// (set) Token: 0x06003903 RID: 14595 RVA: 0x000F2316 File Offset: 0x000F0516
		public CodeTypeReference CreateType
		{
			get
			{
				if (this.createType == null)
				{
					this.createType = new CodeTypeReference("");
				}
				return this.createType;
			}
			set
			{
				this.createType = value;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x000F231F File Offset: 0x000F051F
		public CodeExpressionCollection Initializers
		{
			get
			{
				return this.initializers;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06003905 RID: 14597 RVA: 0x000F2327 File Offset: 0x000F0527
		// (set) Token: 0x06003906 RID: 14598 RVA: 0x000F232F File Offset: 0x000F052F
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06003907 RID: 14599 RVA: 0x000F2338 File Offset: 0x000F0538
		// (set) Token: 0x06003908 RID: 14600 RVA: 0x000F2340 File Offset: 0x000F0540
		public CodeExpression SizeExpression
		{
			get
			{
				return this.sizeExpression;
			}
			set
			{
				this.sizeExpression = value;
			}
		}

		// Token: 0x04002B7B RID: 11131
		private CodeTypeReference createType;

		// Token: 0x04002B7C RID: 11132
		private CodeExpressionCollection initializers = new CodeExpressionCollection();

		// Token: 0x04002B7D RID: 11133
		private CodeExpression sizeExpression;

		// Token: 0x04002B7E RID: 11134
		private int size;
	}
}
