using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200000E RID: 14
	public sealed class ArrayType : TypeSpecification
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004C30 File Offset: 0x00002E30
		public Collection<ArrayDimension> Dimensions
		{
			get
			{
				if (this.dimensions != null)
				{
					return this.dimensions;
				}
				this.dimensions = new Collection<ArrayDimension>();
				this.dimensions.Add(default(ArrayDimension));
				return this.dimensions;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004C71 File Offset: 0x00002E71
		public int Rank
		{
			get
			{
				if (this.dimensions != null)
				{
					return this.dimensions.Count;
				}
				return 1;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004C88 File Offset: 0x00002E88
		public bool IsVector
		{
			get
			{
				return this.dimensions == null || (this.dimensions.Count <= 1 && !this.dimensions[0].IsSized);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00002C55 File Offset: 0x00000E55
		public override bool IsValueType
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004CC6 File Offset: 0x00002EC6
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004CD9 File Offset: 0x00002ED9
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004CEC File Offset: 0x00002EEC
		private string Suffix
		{
			get
			{
				if (this.IsVector)
				{
					return "[]";
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("[");
				for (int i = 0; i < this.dimensions.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(this.dimensions[i].ToString());
				}
				stringBuilder.Append("]");
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004D72 File Offset: 0x00002F72
		public ArrayType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Array;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004D8C File Offset: 0x00002F8C
		public ArrayType(TypeReference type, int rank)
			: this(type)
		{
			Mixin.CheckType(type);
			if (rank == 1)
			{
				return;
			}
			this.dimensions = new Collection<ArrayDimension>(rank);
			for (int i = 0; i < rank; i++)
			{
				this.dimensions.Add(default(ArrayDimension));
			}
			this.etype = Mono.Cecil.Metadata.ElementType.Array;
		}

		// Token: 0x0400001A RID: 26
		private Collection<ArrayDimension> dimensions;
	}
}
