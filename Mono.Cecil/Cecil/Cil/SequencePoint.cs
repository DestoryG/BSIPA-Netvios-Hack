using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000112 RID: 274
	public sealed class SequencePoint
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00023960 File Offset: 0x00021B60
		public int Offset
		{
			get
			{
				return this.offset.Offset;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0002396D File Offset: 0x00021B6D
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x00023975 File Offset: 0x00021B75
		public int StartLine
		{
			get
			{
				return this.start_line;
			}
			set
			{
				this.start_line = value;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0002397E File Offset: 0x00021B7E
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x00023986 File Offset: 0x00021B86
		public int StartColumn
		{
			get
			{
				return this.start_column;
			}
			set
			{
				this.start_column = value;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0002398F File Offset: 0x00021B8F
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x00023997 File Offset: 0x00021B97
		public int EndLine
		{
			get
			{
				return this.end_line;
			}
			set
			{
				this.end_line = value;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x000239A0 File Offset: 0x00021BA0
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x000239A8 File Offset: 0x00021BA8
		public int EndColumn
		{
			get
			{
				return this.end_column;
			}
			set
			{
				this.end_column = value;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x000239B1 File Offset: 0x00021BB1
		public bool IsHidden
		{
			get
			{
				return this.start_line == 16707566 && this.start_line == this.end_line;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x000239D0 File Offset: 0x00021BD0
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x000239D8 File Offset: 0x00021BD8
		public Document Document
		{
			get
			{
				return this.document;
			}
			set
			{
				this.document = value;
			}
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x000239E1 File Offset: 0x00021BE1
		internal SequencePoint(int offset, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(offset);
			this.document = document;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00023A0A File Offset: 0x00021C0A
		public SequencePoint(Instruction instruction, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(instruction);
			this.document = document;
		}

		// Token: 0x04000688 RID: 1672
		internal InstructionOffset offset;

		// Token: 0x04000689 RID: 1673
		private Document document;

		// Token: 0x0400068A RID: 1674
		private int start_line;

		// Token: 0x0400068B RID: 1675
		private int start_column;

		// Token: 0x0400068C RID: 1676
		private int end_line;

		// Token: 0x0400068D RID: 1677
		private int end_column;
	}
}
