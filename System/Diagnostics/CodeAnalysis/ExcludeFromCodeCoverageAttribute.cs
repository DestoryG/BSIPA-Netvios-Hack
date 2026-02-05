using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x0200050B RID: 1291
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
	public sealed class ExcludeFromCodeCoverageAttribute : Attribute
	{
	}
}
