using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000FC RID: 252
	public sealed class ExceptionHandler
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x00020E11 File Offset: 0x0001F011
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x00020E19 File Offset: 0x0001F019
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

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00020E22 File Offset: 0x0001F022
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x00020E2A File Offset: 0x0001F02A
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

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00020E33 File Offset: 0x0001F033
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x00020E3B File Offset: 0x0001F03B
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

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00020E44 File Offset: 0x0001F044
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00020E4C File Offset: 0x0001F04C
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

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00020E55 File Offset: 0x0001F055
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x00020E5D File Offset: 0x0001F05D
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

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00020E66 File Offset: 0x0001F066
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x00020E6E File Offset: 0x0001F06E
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

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00020E77 File Offset: 0x0001F077
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x00020E7F File Offset: 0x0001F07F
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

		// Token: 0x06000A0C RID: 2572 RVA: 0x00020E88 File Offset: 0x0001F088
		public ExceptionHandler(ExceptionHandlerType handlerType)
		{
			this.handler_type = handlerType;
		}

		// Token: 0x04000534 RID: 1332
		private Instruction try_start;

		// Token: 0x04000535 RID: 1333
		private Instruction try_end;

		// Token: 0x04000536 RID: 1334
		private Instruction filter_start;

		// Token: 0x04000537 RID: 1335
		private Instruction handler_start;

		// Token: 0x04000538 RID: 1336
		private Instruction handler_end;

		// Token: 0x04000539 RID: 1337
		private TypeReference catch_type;

		// Token: 0x0400053A RID: 1338
		private ExceptionHandlerType handler_type;
	}
}
