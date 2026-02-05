using System;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000D7 RID: 215
	internal sealed class SpecialTypeDataContract : DataContract
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x00034382 File Offset: 0x00032582
		[SecuritySafeCritical]
		public SpecialTypeDataContract(Type type)
			: base(new SpecialTypeDataContract.SpecialTypeDataContractCriticalHelper(type))
		{
			this.helper = base.Helper as SpecialTypeDataContract.SpecialTypeDataContractCriticalHelper;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000343A1 File Offset: 0x000325A1
		[SecuritySafeCritical]
		public SpecialTypeDataContract(Type type, XmlDictionaryString name, XmlDictionaryString ns)
			: base(new SpecialTypeDataContract.SpecialTypeDataContractCriticalHelper(type, name, ns))
		{
			this.helper = base.Helper as SpecialTypeDataContract.SpecialTypeDataContractCriticalHelper;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x000343C2 File Offset: 0x000325C2
		internal override bool IsBuiltInDataContract
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040004F4 RID: 1268
		[SecurityCritical]
		private SpecialTypeDataContract.SpecialTypeDataContractCriticalHelper helper;

		// Token: 0x02000177 RID: 375
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class SpecialTypeDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x060014BF RID: 5311 RVA: 0x00054233 File Offset: 0x00052433
			internal SpecialTypeDataContractCriticalHelper(Type type)
				: base(type)
			{
			}

			// Token: 0x060014C0 RID: 5312 RVA: 0x0005423C File Offset: 0x0005243C
			internal SpecialTypeDataContractCriticalHelper(Type type, XmlDictionaryString name, XmlDictionaryString ns)
				: base(type)
			{
				base.SetDataContractName(name, ns);
			}
		}
	}
}
