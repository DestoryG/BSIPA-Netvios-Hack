using System;
using System.Text;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BA RID: 186
	internal sealed class ArrayType : TypeSpecification
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00012FE8 File Offset: 0x000111E8
		public Collection<ArrayDimension> Dimensions
		{
			get
			{
				if (this.dimensions != null)
				{
					return this.dimensions;
				}
				Interlocked.CompareExchange<Collection<ArrayDimension>>(ref this.dimensions, new Collection<ArrayDimension> { default(ArrayDimension) }, null);
				return this.dimensions;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0001302D File Offset: 0x0001122D
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00013044 File Offset: 0x00011244
		public bool IsVector
		{
			get
			{
				return this.dimensions == null || (this.dimensions.Count <= 1 && !this.dimensions[0].IsSized);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00013082 File Offset: 0x00011282
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00013095 File Offset: 0x00011295
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x000130A8 File Offset: 0x000112A8
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001312E File Offset: 0x0001132E
		public ArrayType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Array;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00013148 File Offset: 0x00011348
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

		// Token: 0x0400021B RID: 539
		private Collection<ArrayDimension> dimensions;
	}
}
