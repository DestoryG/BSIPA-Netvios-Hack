using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000085 RID: 133
	internal interface IDataNode
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000974 RID: 2420
		Type DataType { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000975 RID: 2421
		// (set) Token: 0x06000976 RID: 2422
		object Value { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000977 RID: 2423
		// (set) Token: 0x06000978 RID: 2424
		string DataContractName { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000979 RID: 2425
		// (set) Token: 0x0600097A RID: 2426
		string DataContractNamespace { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600097B RID: 2427
		// (set) Token: 0x0600097C RID: 2428
		string ClrTypeName { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600097D RID: 2429
		// (set) Token: 0x0600097E RID: 2430
		string ClrAssemblyName { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600097F RID: 2431
		// (set) Token: 0x06000980 RID: 2432
		string Id { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000981 RID: 2433
		bool PreservesReferences { get; }

		// Token: 0x06000982 RID: 2434
		void GetData(ElementData element);

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000983 RID: 2435
		// (set) Token: 0x06000984 RID: 2436
		bool IsFinalValue { get; set; }

		// Token: 0x06000985 RID: 2437
		void Clear();
	}
}
