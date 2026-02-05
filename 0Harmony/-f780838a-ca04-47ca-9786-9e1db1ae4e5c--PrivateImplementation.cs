using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020001FC RID: 508
[CompilerGenerated]
internal sealed class <f780838a-ca04-47ca-9786-9e1db1ae4e5c><PrivateImplementationDetails>
{
	// Token: 0x06000F5A RID: 3930 RVA: 0x00033D24 File Offset: 0x00031F24
	internal static uint ComputeStringHash(string s)
	{
		uint num;
		if (s != null)
		{
			num = 2166136261U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((uint)s[i] ^ num) * 16777619U;
			}
		}
		return num;
	}

	// Token: 0x04000951 RID: 2385 RVA: 0x00048FC8 File Offset: 0x000471C8
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '-8511746012302509385'.
	internal static readonly long 1B960802B155541DF3837ADE50790DA7E91762D14B8E011FA8223424FF75ACDB;

	// Token: 0x04000952 RID: 2386 RVA: 0x00048FD0 File Offset: 0x000471D0
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly <f780838a-ca04-47ca-9786-9e1db1ae4e5c><PrivateImplementationDetails>.__StaticArrayInitTypeSize=1790 2EF0065A03764C27AE8D5DC3002E10F0426E43BDFA7D8ECFFF633E45DD32376B;

	// Token: 0x04000953 RID: 2387 RVA: 0x000496CE File Offset: 0x000478CE
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly <f780838a-ca04-47ca-9786-9e1db1ae4e5c><PrivateImplementationDetails>.__StaticArrayInitTypeSize=160 933598639CBAA1DE502F80D2FD1DB78F13C8D7BB64A5FDC1BC73AC0B5CE4F5CA;

	// Token: 0x04000954 RID: 2388 RVA: 0x0004976E File Offset: 0x0004796E
	// Note: this field is marked with 'hasfieldrva' and has an initial value of '4182389475095035824'.
	internal static readonly long 971150DD73DC318E68A98CCE9B91AC7DEA2D43C562B4F5A9A2F4272C7E29477E;

	// Token: 0x04000955 RID: 2389 RVA: 0x00049776 File Offset: 0x00047976
	// Note: this field is marked with 'hasfieldrva'.
	internal static readonly <f780838a-ca04-47ca-9786-9e1db1ae4e5c><PrivateImplementationDetails>.__StaticArrayInitTypeSize=128 BFDF5E72651B4EC588BD5FC6A9F17E9E0972248146BBACC10478F48D72F29B81;

	// Token: 0x020001FD RID: 509
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 128)]
	private struct __StaticArrayInitTypeSize=128
	{
	}

	// Token: 0x020001FE RID: 510
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 160)]
	private struct __StaticArrayInitTypeSize=160
	{
	}

	// Token: 0x020001FF RID: 511
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 1790)]
	private struct __StaticArrayInitTypeSize=1790
	{
	}
}
