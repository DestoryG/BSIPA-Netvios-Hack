using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000178 RID: 376
	internal class InterfaceImplementationCollection : Collection<InterfaceImplementation>
	{
		// Token: 0x06000BB5 RID: 2997 RVA: 0x00026F59 File Offset: 0x00025159
		internal InterfaceImplementationCollection(TypeDefinition type)
		{
			this.type = type;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00026F68 File Offset: 0x00025168
		internal InterfaceImplementationCollection(TypeDefinition type, int length)
			: base(length)
		{
			this.type = type;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00026F78 File Offset: 0x00025178
		protected override void OnAdd(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00026F78 File Offset: 0x00025178
		protected override void OnInsert(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00026F78 File Offset: 0x00025178
		protected override void OnSet(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00026F86 File Offset: 0x00025186
		protected override void OnRemove(InterfaceImplementation item, int index)
		{
			item.type = null;
		}

		// Token: 0x040004F0 RID: 1264
		private readonly TypeDefinition type;
	}
}
