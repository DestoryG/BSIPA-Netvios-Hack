using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000078 RID: 120
	public class DataContractSerializerSettings
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00028973 File Offset: 0x00026B73
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x0002897B File Offset: 0x00026B7B
		public XmlDictionaryString RootName { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00028984 File Offset: 0x00026B84
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x0002898C File Offset: 0x00026B8C
		public XmlDictionaryString RootNamespace { get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00028995 File Offset: 0x00026B95
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x0002899D File Offset: 0x00026B9D
		public IEnumerable<Type> KnownTypes { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x000289A6 File Offset: 0x00026BA6
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x000289AE File Offset: 0x00026BAE
		public int MaxItemsInObjectGraph
		{
			get
			{
				return this.maxItemsInObjectGraph;
			}
			set
			{
				this.maxItemsInObjectGraph = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x000289B7 File Offset: 0x00026BB7
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x000289BF File Offset: 0x00026BBF
		public bool IgnoreExtensionDataObject { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x000289C8 File Offset: 0x00026BC8
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x000289D0 File Offset: 0x00026BD0
		public bool PreserveObjectReferences { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x000289D9 File Offset: 0x00026BD9
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x000289E1 File Offset: 0x00026BE1
		public IDataContractSurrogate DataContractSurrogate { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x000289EA File Offset: 0x00026BEA
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x000289F2 File Offset: 0x00026BF2
		public DataContractResolver DataContractResolver { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x000289FB File Offset: 0x00026BFB
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x00028A03 File Offset: 0x00026C03
		public bool SerializeReadOnlyTypes { get; set; }

		// Token: 0x04000337 RID: 823
		private int maxItemsInObjectGraph = int.MaxValue;
	}
}
