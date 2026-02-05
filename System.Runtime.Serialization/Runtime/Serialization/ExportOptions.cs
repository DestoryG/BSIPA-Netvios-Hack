using System;
using System.Collections.ObjectModel;

namespace System.Runtime.Serialization
{
	// Token: 0x02000082 RID: 130
	public class ExportOptions
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0002A320 File Offset: 0x00028520
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x0002A328 File Offset: 0x00028528
		public IDataContractSurrogate DataContractSurrogate
		{
			get
			{
				return this.dataContractSurrogate;
			}
			set
			{
				this.dataContractSurrogate = value;
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002A331 File Offset: 0x00028531
		internal IDataContractSurrogate GetSurrogate()
		{
			return this.dataContractSurrogate;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0002A339 File Offset: 0x00028539
		public Collection<Type> KnownTypes
		{
			get
			{
				if (this.knownTypes == null)
				{
					this.knownTypes = new Collection<Type>();
				}
				return this.knownTypes;
			}
		}

		// Token: 0x04000398 RID: 920
		private Collection<Type> knownTypes;

		// Token: 0x04000399 RID: 921
		private IDataContractSurrogate dataContractSurrogate;
	}
}
