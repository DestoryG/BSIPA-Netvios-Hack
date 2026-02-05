using System;
using System.Collections.Generic;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x0200008F RID: 143
	internal sealed class GenericParameterDataContract : DataContract
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0002BDE7 File Offset: 0x00029FE7
		[SecuritySafeCritical]
		internal GenericParameterDataContract(Type type)
			: base(new GenericParameterDataContract.GenericParameterDataContractCriticalHelper(type))
		{
			this.helper = base.Helper as GenericParameterDataContract.GenericParameterDataContractCriticalHelper;
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0002BE06 File Offset: 0x0002A006
		internal int ParameterPosition
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ParameterPosition;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0002BE13 File Offset: 0x0002A013
		internal override bool IsBuiltInDataContract
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0002BE16 File Offset: 0x0002A016
		internal override DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
		{
			return paramContracts[this.ParameterPosition];
		}

		// Token: 0x040003D2 RID: 978
		[SecurityCritical]
		private GenericParameterDataContract.GenericParameterDataContractCriticalHelper helper;

		// Token: 0x02000175 RID: 373
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class GenericParameterDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x060014B6 RID: 5302 RVA: 0x000541C6 File Offset: 0x000523C6
			internal GenericParameterDataContractCriticalHelper(Type type)
				: base(type)
			{
				base.SetDataContractName(DataContract.GetStableName(type));
				this.parameterPosition = type.GenericParameterPosition;
			}

			// Token: 0x1700044D RID: 1101
			// (get) Token: 0x060014B7 RID: 5303 RVA: 0x000541E7 File Offset: 0x000523E7
			internal int ParameterPosition
			{
				get
				{
					return this.parameterPosition;
				}
			}

			// Token: 0x04000A20 RID: 2592
			private int parameterPosition;
		}
	}
}
