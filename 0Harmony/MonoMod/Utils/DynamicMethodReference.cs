using System;
using System.Reflection.Emit;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x0200032A RID: 810
	internal class DynamicMethodReference : MethodReference
	{
		// Token: 0x06001280 RID: 4736 RVA: 0x00040380 File Offset: 0x0003E580
		public DynamicMethodReference(ModuleDefinition module, DynamicMethod dm)
			: base("", module.TypeSystem.Void)
		{
			this.DynamicMethod = dm;
		}

		// Token: 0x04000F6D RID: 3949
		public DynamicMethod DynamicMethod;
	}
}
