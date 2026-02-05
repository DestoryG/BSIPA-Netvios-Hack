using System;
using Mono.Cecil;

// Token: 0x0200031A RID: 794
internal static class MultiTargetShims
{
	// Token: 0x06001231 RID: 4657 RVA: 0x0003CD95 File Offset: 0x0003AF95
	public static TypeReference GetConstraintType(this GenericParameterConstraint constraint)
	{
		return constraint.ConstraintType;
	}

	// Token: 0x04000F32 RID: 3890
	private static readonly object[] _NoArgs = new object[0];
}
