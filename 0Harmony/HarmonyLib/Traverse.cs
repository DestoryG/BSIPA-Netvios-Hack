using System;

namespace HarmonyLib
{
	// Token: 0x020000A1 RID: 161
	public class Traverse<T>
	{
		// Token: 0x06000320 RID: 800 RVA: 0x00002AB9 File Offset: 0x00000CB9
		private Traverse()
		{
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000F8EA File Offset: 0x0000DAEA
		public Traverse(Traverse traverse)
		{
			this.traverse = traverse;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000F8F9 File Offset: 0x0000DAF9
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0000F906 File Offset: 0x0000DB06
		public T Value
		{
			get
			{
				return this.traverse.GetValue<T>();
			}
			set
			{
				this.traverse.SetValue(value);
			}
		}

		// Token: 0x040001C7 RID: 455
		private readonly Traverse traverse;
	}
}
