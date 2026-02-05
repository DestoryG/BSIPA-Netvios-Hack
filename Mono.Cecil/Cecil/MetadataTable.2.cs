using System;

namespace Mono.Cecil
{
	// Token: 0x0200001D RID: 29
	internal abstract class MetadataTable<TRow> : MetadataTable where TRow : struct
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000ADC0 File Offset: 0x00008FC0
		public sealed override int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000ADC8 File Offset: 0x00008FC8
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

		// Token: 0x06000204 RID: 516 RVA: 0x0000AE10 File Offset: 0x00009010
		private void Grow()
		{
			TRow[] array = new TRow[this.rows.Length * 2];
			Array.Copy(this.rows, array, this.rows.Length);
			this.rows = array;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00002A0D File Offset: 0x00000C0D
		public override void Sort()
		{
		}

		// Token: 0x04000042 RID: 66
		internal TRow[] rows = new TRow[2];

		// Token: 0x04000043 RID: 67
		internal int length;
	}
}
