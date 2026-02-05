using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D6 RID: 470
	internal sealed class SequencePoint
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00032B6C File Offset: 0x00030D6C
		public int Offset
		{
			get
			{
				return this.offset.Offset;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x00032B79 File Offset: 0x00030D79
		// (set) Token: 0x06000EA8 RID: 3752 RVA: 0x00032B81 File Offset: 0x00030D81
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

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x00032B8A File Offset: 0x00030D8A
		// (set) Token: 0x06000EAA RID: 3754 RVA: 0x00032B92 File Offset: 0x00030D92
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

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x00032B9B File Offset: 0x00030D9B
		// (set) Token: 0x06000EAC RID: 3756 RVA: 0x00032BA3 File Offset: 0x00030DA3
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

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00032BAC File Offset: 0x00030DAC
		// (set) Token: 0x06000EAE RID: 3758 RVA: 0x00032BB4 File Offset: 0x00030DB4
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

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x00032BBD File Offset: 0x00030DBD
		public bool IsHidden
		{
			get
			{
				return this.start_line == 16707566 && this.start_line == this.end_line;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00032BDC File Offset: 0x00030DDC
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x00032BE4 File Offset: 0x00030DE4
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

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00032BED File Offset: 0x00030DED
		internal SequencePoint(int offset, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(offset);
			this.document = document;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00032C16 File Offset: 0x00030E16
		public SequencePoint(Instruction instruction, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(instruction);
			this.document = document;
		}

		// Token: 0x040008E7 RID: 2279
		internal InstructionOffset offset;

		// Token: 0x040008E8 RID: 2280
		private Document document;

		// Token: 0x040008E9 RID: 2281
		private int start_line;

		// Token: 0x040008EA RID: 2282
		private int start_column;

		// Token: 0x040008EB RID: 2283
		private int end_line;

		// Token: 0x040008EC RID: 2284
		private int end_column;
	}
}
