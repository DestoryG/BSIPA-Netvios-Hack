using System;

namespace Mono.Cecil
{
	// Token: 0x020000CC RID: 204
	internal abstract class MetadataTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00019265 File Offset: 0x00017465
		public sealed override int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00019270 File Offset: 0x00017470
		public int AddRow(TRow row)
		{
			if (this.rows.Length == this.length)
			{
				this.Grow();
			}
			TRow[] array = this.rows;
			int num = this.length;
			this.length = num + 1;
			array[num] = row;
			return this.length;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000192B8 File Offset: 0x000174B8
		private void Grow()
		{
			TRow[] array = new TRow[this.rows.Length * 2];
			Array.Copy(this.rows, array, this.rows.Length);
			this.rows = array;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00010C51 File Offset: 0x0000EE51
		public override void Sort()
		{
		}

		// Token: 0x0400024A RID: 586
		internal TRow[] rows = new TRow[2];

		// Token: 0x0400024B RID: 587
		internal int length;
	}
}
