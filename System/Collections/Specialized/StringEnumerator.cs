using System;

namespace System.Collections.Specialized
{
	// Token: 0x020003B6 RID: 950
	public class StringEnumerator
	{
		// Token: 0x060023C6 RID: 9158 RVA: 0x000A8870 File Offset: 0x000A6A70
		internal StringEnumerator(StringCollection mappings)
		{
			this.temp = mappings;
			this.baseEnumerator = this.temp.GetEnumerator();
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x000A8890 File Offset: 0x000A6A90
		public string Current
		{
			get
			{
				return (string)this.baseEnumerator.Current;
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x000A88A2 File Offset: 0x000A6AA2
		public bool MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000A88AF File Offset: 0x000A6AAF
		public void Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x04001FE7 RID: 8167
		private IEnumerator baseEnumerator;

		// Token: 0x04001FE8 RID: 8168
		private IEnumerable temp;
	}
}
