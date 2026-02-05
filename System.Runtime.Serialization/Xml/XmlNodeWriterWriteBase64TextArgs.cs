using System;

namespace System.Xml
{
	// Token: 0x02000054 RID: 84
	internal class XmlNodeWriterWriteBase64TextArgs
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001B825 File Offset: 0x00019A25
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0001B82D File Offset: 0x00019A2D
		internal byte[] TrailBuffer { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001B836 File Offset: 0x00019A36
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x0001B83E File Offset: 0x00019A3E
		internal int TrailCount { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001B847 File Offset: 0x00019A47
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x0001B84F File Offset: 0x00019A4F
		internal byte[] Buffer { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0001B858 File Offset: 0x00019A58
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x0001B860 File Offset: 0x00019A60
		internal int Offset { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001B869 File Offset: 0x00019A69
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x0001B871 File Offset: 0x00019A71
		internal int Count { get; set; }
	}
}
