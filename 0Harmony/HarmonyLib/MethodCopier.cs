using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000020 RID: 32
	internal class MethodCopier
	{
		// Token: 0x060000AB RID: 171 RVA: 0x000051D8 File Offset: 0x000033D8
		internal MethodCopier(MethodBase fromMethod, ILGenerator toILGenerator, LocalBuilder[] existingVariables = null)
		{
			if (fromMethod == null)
			{
				throw new ArgumentNullException("fromMethod");
			}
			this.reader = new MethodBodyReader(fromMethod, toILGenerator);
			this.reader.DeclareVariables(existingVariables);
			this.reader.ReadInstructions();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000522E File Offset: 0x0000342E
		internal void SetArgumentShift(bool useShift)
		{
			this.reader.SetArgumentShift(useShift);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000523C File Offset: 0x0000343C
		internal void AddTranspiler(MethodInfo transpiler)
		{
			this.transpilers.Add(transpiler);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000524A File Offset: 0x0000344A
		internal void Finalize(Emitter emitter, List<Label> endLabels, out bool hasReturnCode)
		{
			this.reader.FinalizeILCodes(emitter, this.transpilers, endLabels, out hasReturnCode);
		}

		// Token: 0x04000058 RID: 88
		private readonly MethodBodyReader reader;

		// Token: 0x04000059 RID: 89
		private readonly List<MethodInfo> transpilers = new List<MethodInfo>();
	}
}
