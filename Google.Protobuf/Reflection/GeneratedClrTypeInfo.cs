using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000073 RID: 115
	public sealed class GeneratedClrTypeInfo
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0001BFA9 File Offset: 0x0001A1A9
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x0001BFB1 File Offset: 0x0001A1B1
		public Type ClrType { get; private set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0001BFBA File Offset: 0x0001A1BA
		public MessageParser Parser { get; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0001BFC2 File Offset: 0x0001A1C2
		public string[] PropertyNames { get; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0001BFCA File Offset: 0x0001A1CA
		public Extension[] Extensions { get; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0001BFD2 File Offset: 0x0001A1D2
		public string[] OneofNames { get; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0001BFDA File Offset: 0x0001A1DA
		public GeneratedClrTypeInfo[] NestedTypes { get; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x0001BFE2 File Offset: 0x0001A1E2
		public Type[] NestedEnums { get; }

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001BFEC File Offset: 0x0001A1EC
		public GeneratedClrTypeInfo(Type clrType, MessageParser parser, string[] propertyNames, string[] oneofNames, Type[] nestedEnums, Extension[] extensions, GeneratedClrTypeInfo[] nestedTypes)
		{
			this.NestedTypes = nestedTypes ?? GeneratedClrTypeInfo.EmptyCodeInfo;
			this.NestedEnums = nestedEnums ?? ReflectionUtil.EmptyTypes;
			this.ClrType = clrType;
			this.Extensions = extensions ?? GeneratedClrTypeInfo.EmptyExtensions;
			this.Parser = parser;
			this.PropertyNames = propertyNames ?? GeneratedClrTypeInfo.EmptyNames;
			this.OneofNames = oneofNames ?? GeneratedClrTypeInfo.EmptyNames;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001C061 File Offset: 0x0001A261
		public GeneratedClrTypeInfo(Type clrType, MessageParser parser, string[] propertyNames, string[] oneofNames, Type[] nestedEnums, GeneratedClrTypeInfo[] nestedTypes)
			: this(clrType, parser, propertyNames, oneofNames, nestedEnums, null, nestedTypes)
		{
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001C073 File Offset: 0x0001A273
		public GeneratedClrTypeInfo(Type[] nestedEnums, Extension[] extensions, GeneratedClrTypeInfo[] nestedTypes)
			: this(null, null, null, null, nestedEnums, extensions, nestedTypes)
		{
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001C082 File Offset: 0x0001A282
		public GeneratedClrTypeInfo(Type[] nestedEnums, GeneratedClrTypeInfo[] nestedTypes)
			: this(null, null, null, null, nestedEnums, nestedTypes)
		{
		}

		// Token: 0x04000316 RID: 790
		private static readonly string[] EmptyNames = new string[0];

		// Token: 0x04000317 RID: 791
		private static readonly GeneratedClrTypeInfo[] EmptyCodeInfo = new GeneratedClrTypeInfo[0];

		// Token: 0x04000318 RID: 792
		private static readonly Extension[] EmptyExtensions = new Extension[0];
	}
}
