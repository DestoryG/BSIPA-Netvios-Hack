using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x020000FD RID: 253
	public class DataContractJsonSerializerSettings
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x00040F28 File Offset: 0x0003F128
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x00040F30 File Offset: 0x0003F130
		public string RootName { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00040F39 File Offset: 0x0003F139
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x00040F41 File Offset: 0x0003F141
		public IEnumerable<Type> KnownTypes { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00040F4A File Offset: 0x0003F14A
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x00040F52 File Offset: 0x0003F152
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

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00040F5B File Offset: 0x0003F15B
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x00040F63 File Offset: 0x0003F163
		public bool IgnoreExtensionDataObject { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x00040F6C File Offset: 0x0003F16C
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x00040F74 File Offset: 0x0003F174
		public IDataContractSurrogate DataContractSurrogate { get; set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x00040F7D File Offset: 0x0003F17D
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x00040F85 File Offset: 0x0003F185
		public EmitTypeInformation EmitTypeInformation { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x00040F8E File Offset: 0x0003F18E
		// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x00040F96 File Offset: 0x0003F196
		public DateTimeFormat DateTimeFormat { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00040F9F File Offset: 0x0003F19F
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x00040FA7 File Offset: 0x0003F1A7
		public bool SerializeReadOnlyTypes { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00040FB0 File Offset: 0x0003F1B0
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x00040FB8 File Offset: 0x0003F1B8
		public bool UseSimpleDictionaryFormat { get; set; }

		// Token: 0x040007D2 RID: 2002
		private int maxItemsInObjectGraph = int.MaxValue;
	}
}
