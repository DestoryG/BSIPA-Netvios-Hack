using System;
using System.CodeDom;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x0200006B RID: 107
	internal class ContractCodeDomInfo
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x000260FB File Offset: 0x000242FB
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0002610D File Offset: 0x0002430D
		internal string ClrNamespace
		{
			get
			{
				if (!this.ReferencedTypeExists)
				{
					return this.clrNamespace;
				}
				return null;
			}
			set
			{
				if (this.ReferencedTypeExists)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Cannot set namespace for already referenced type. Base type is '{0}'.", new object[] { this.TypeReference.BaseType })));
				}
				this.clrNamespace = value;
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00026148 File Offset: 0x00024348
		internal Dictionary<string, object> GetMemberNames()
		{
			if (this.ReferencedTypeExists)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Cannot set members for already referenced type. Base type is '{0}'.", new object[] { this.TypeReference.BaseType })));
			}
			if (this.memberNames == null)
			{
				this.memberNames = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			}
			return this.memberNames;
		}

		// Token: 0x0400030A RID: 778
		internal bool IsProcessed;

		// Token: 0x0400030B RID: 779
		internal CodeTypeDeclaration TypeDeclaration;

		// Token: 0x0400030C RID: 780
		internal CodeTypeReference TypeReference;

		// Token: 0x0400030D RID: 781
		internal CodeNamespace CodeNamespace;

		// Token: 0x0400030E RID: 782
		internal bool ReferencedTypeExists;

		// Token: 0x0400030F RID: 783
		internal bool UsesWildcardNamespace;

		// Token: 0x04000310 RID: 784
		private string clrNamespace;

		// Token: 0x04000311 RID: 785
		private Dictionary<string, object> memberNames;
	}
}
