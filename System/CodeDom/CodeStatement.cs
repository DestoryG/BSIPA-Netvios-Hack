using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000656 RID: 1622
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeStatement : CodeObject
	{
		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06003AC5 RID: 15045 RVA: 0x000F451A File Offset: 0x000F271A
		// (set) Token: 0x06003AC6 RID: 15046 RVA: 0x000F4522 File Offset: 0x000F2722
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
			set
			{
				this.linePragma = value;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x000F452B File Offset: 0x000F272B
		public CodeDirectiveCollection StartDirectives
		{
			get
			{
				if (this.startDirectives == null)
				{
					this.startDirectives = new CodeDirectiveCollection();
				}
				return this.startDirectives;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x000F4546 File Offset: 0x000F2746
		public CodeDirectiveCollection EndDirectives
		{
			get
			{
				if (this.endDirectives == null)
				{
					this.endDirectives = new CodeDirectiveCollection();
				}
				return this.endDirectives;
			}
		}

		// Token: 0x04002C15 RID: 11285
		private CodeLinePragma linePragma;

		// Token: 0x04002C16 RID: 11286
		[OptionalField]
		private CodeDirectiveCollection startDirectives;

		// Token: 0x04002C17 RID: 11287
		[OptionalField]
		private CodeDirectiveCollection endDirectives;
	}
}
