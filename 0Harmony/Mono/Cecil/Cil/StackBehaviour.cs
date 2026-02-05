using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C9 RID: 457
	internal enum StackBehaviour
	{
		// Token: 0x040007D2 RID: 2002
		Pop0,
		// Token: 0x040007D3 RID: 2003
		Pop1,
		// Token: 0x040007D4 RID: 2004
		Pop1_pop1,
		// Token: 0x040007D5 RID: 2005
		Popi,
		// Token: 0x040007D6 RID: 2006
		Popi_pop1,
		// Token: 0x040007D7 RID: 2007
		Popi_popi,
		// Token: 0x040007D8 RID: 2008
		Popi_popi8,
		// Token: 0x040007D9 RID: 2009
		Popi_popi_popi,
		// Token: 0x040007DA RID: 2010
		Popi_popr4,
		// Token: 0x040007DB RID: 2011
		Popi_popr8,
		// Token: 0x040007DC RID: 2012
		Popref,
		// Token: 0x040007DD RID: 2013
		Popref_pop1,
		// Token: 0x040007DE RID: 2014
		Popref_popi,
		// Token: 0x040007DF RID: 2015
		Popref_popi_popi,
		// Token: 0x040007E0 RID: 2016
		Popref_popi_popi8,
		// Token: 0x040007E1 RID: 2017
		Popref_popi_popr4,
		// Token: 0x040007E2 RID: 2018
		Popref_popi_popr8,
		// Token: 0x040007E3 RID: 2019
		Popref_popi_popref,
		// Token: 0x040007E4 RID: 2020
		PopAll,
		// Token: 0x040007E5 RID: 2021
		Push0,
		// Token: 0x040007E6 RID: 2022
		Push1,
		// Token: 0x040007E7 RID: 2023
		Push1_push1,
		// Token: 0x040007E8 RID: 2024
		Pushi,
		// Token: 0x040007E9 RID: 2025
		Pushi8,
		// Token: 0x040007EA RID: 2026
		Pushr4,
		// Token: 0x040007EB RID: 2027
		Pushr8,
		// Token: 0x040007EC RID: 2028
		Pushref,
		// Token: 0x040007ED RID: 2029
		Varpop,
		// Token: 0x040007EE RID: 2030
		Varpush
	}
}
