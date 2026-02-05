using System;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200000D RID: 13
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public sealed class TupleElementNamesAttribute : Attribute
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00005794 File Offset: 0x00003994
		public IList<string> TransformNames
		{
			get
			{
				return this._transformNames;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000579C File Offset: 0x0000399C
		public TupleElementNamesAttribute(string[] transformNames)
		{
			if (transformNames == null)
			{
				throw new ArgumentNullException("transformNames");
			}
			this._transformNames = transformNames;
		}

		// Token: 0x04000026 RID: 38
		private readonly string[] _transformNames;
	}
}
