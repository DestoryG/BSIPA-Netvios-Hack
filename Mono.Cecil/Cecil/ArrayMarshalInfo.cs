using System;

namespace Mono.Cecil
{
	// Token: 0x0200007A RID: 122
	public sealed class ArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0001312F File Offset: 0x0001132F
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00013137 File Offset: 0x00011337
		public NativeType ElementType
		{
			get
			{
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00013140 File Offset: 0x00011340
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00013148 File Offset: 0x00011348
		public int SizeParameterIndex
		{
			get
			{
				return this.size_parameter_index;
			}
			set
			{
				this.size_parameter_index = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00013151 File Offset: 0x00011351
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00013159 File Offset: 0x00011359
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00013162 File Offset: 0x00011362
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x0001316A File Offset: 0x0001136A
		public int SizeParameterMultiplier
		{
			get
			{
				return this.size_parameter_multiplier;
			}
			set
			{
				this.size_parameter_multiplier = value;
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00013173 File Offset: 0x00011373
		public ArrayMarshalInfo()
			: base(NativeType.Array)
		{
			this.element_type = NativeType.None;
			this.size_parameter_index = -1;
			this.size = -1;
			this.size_parameter_multiplier = -1;
		}

		// Token: 0x040000ED RID: 237
		internal NativeType element_type;

		// Token: 0x040000EE RID: 238
		internal int size_parameter_index;

		// Token: 0x040000EF RID: 239
		internal int size;

		// Token: 0x040000F0 RID: 240
		internal int size_parameter_multiplier;
	}
}
