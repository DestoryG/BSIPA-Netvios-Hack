using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BB RID: 187
	internal class InterfaceImplementationCollection : Collection<InterfaceImplementation>
	{
		// Token: 0x060007EA RID: 2026 RVA: 0x00018243 File Offset: 0x00016443
		internal InterfaceImplementationCollection(TypeDefinition type)
		{
			this.type = type;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00018252 File Offset: 0x00016452
		internal InterfaceImplementationCollection(TypeDefinition type, int length)
			: base(length)
		{
			this.type = type;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00018262 File Offset: 0x00016462
		protected override void OnAdd(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00018262 File Offset: 0x00016462
		protected override void OnInsert(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00018262 File Offset: 0x00016462
		protected override void OnSet(InterfaceImplementation item, int index)
		{
			item.type = this.type;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00018270 File Offset: 0x00016470
		protected override void OnRemove(InterfaceImplementation item, int index)
		{
			item.type = null;
		}

		// Token: 0x040002A7 RID: 679
		private readonly TypeDefinition type;
	}
}
