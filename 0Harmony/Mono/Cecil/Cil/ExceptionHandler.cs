using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C0 RID: 448
	internal sealed class ExceptionHandler
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0002FF9D File Offset: 0x0002E19D
		// (set) Token: 0x06000DE3 RID: 3555 RVA: 0x0002FFA5 File Offset: 0x0002E1A5
		public Instruction TryStart
		{
			get
			{
				return this.try_start;
			}
			set
			{
				this.try_start = value;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x0002FFAE File Offset: 0x0002E1AE
		// (set) Token: 0x06000DE5 RID: 3557 RVA: 0x0002FFB6 File Offset: 0x0002E1B6
		public Instruction TryEnd
		{
			get
			{
				return this.try_end;
			}
			set
			{
				this.try_end = value;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x0002FFBF File Offset: 0x0002E1BF
		// (set) Token: 0x06000DE7 RID: 3559 RVA: 0x0002FFC7 File Offset: 0x0002E1C7
		public Instruction FilterStart
		{
			get
			{
				return this.filter_start;
			}
			set
			{
				this.filter_start = value;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0002FFD0 File Offset: 0x0002E1D0
		// (set) Token: 0x06000DE9 RID: 3561 RVA: 0x0002FFD8 File Offset: 0x0002E1D8
		public Instruction HandlerStart
		{
			get
			{
				return this.handler_start;
			}
			set
			{
				this.handler_start = value;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x0002FFE1 File Offset: 0x0002E1E1
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x0002FFE9 File Offset: 0x0002E1E9
		public Instruction HandlerEnd
		{
			get
			{
				return this.handler_end;
			}
			set
			{
				this.handler_end = value;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0002FFF2 File Offset: 0x0002E1F2
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x0002FFFA File Offset: 0x0002E1FA
		public TypeReference CatchType
		{
			get
			{
				return this.catch_type;
			}
			set
			{
				this.catch_type = value;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x00030003 File Offset: 0x0002E203
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x0003000B File Offset: 0x0002E20B
		public ExceptionHandlerType HandlerType
		{
			get
			{
				return this.handler_type;
			}
			set
			{
				this.handler_type = value;
			}
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00030014 File Offset: 0x0002E214
		public ExceptionHandler(ExceptionHandlerType handlerType)
		{
			this.handler_type = handlerType;
		}

		// Token: 0x04000793 RID: 1939
		private Instruction try_start;

		// Token: 0x04000794 RID: 1940
		private Instruction try_end;

		// Token: 0x04000795 RID: 1941
		private Instruction filter_start;

		// Token: 0x04000796 RID: 1942
		private Instruction handler_start;

		// Token: 0x04000797 RID: 1943
		private Instruction handler_end;

		// Token: 0x04000798 RID: 1944
		private TypeReference catch_type;

		// Token: 0x04000799 RID: 1945
		private ExceptionHandlerType handler_type;
	}
}
