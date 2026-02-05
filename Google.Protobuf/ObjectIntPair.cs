using System;

namespace Google.Protobuf
{
	// Token: 0x02000022 RID: 34
	internal struct ObjectIntPair<T> : IEquatable<ObjectIntPair<T>> where T : class
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x00009AFB File Offset: 0x00007CFB
		internal ObjectIntPair(T obj, int number)
		{
			this.number = number;
			this.obj = obj;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009B0B File Offset: 0x00007D0B
		public bool Equals(ObjectIntPair<T> other)
		{
			return this.obj == other.obj && this.number == other.number;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009B35 File Offset: 0x00007D35
		public override bool Equals(object obj)
		{
			return obj is ObjectIntPair<T> && this.Equals((ObjectIntPair<T>)obj);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009B4D File Offset: 0x00007D4D
		public override int GetHashCode()
		{
			return this.obj.GetHashCode() * 65535 + this.number;
		}

		// Token: 0x04000062 RID: 98
		private readonly int number;

		// Token: 0x04000063 RID: 99
		private readonly T obj;
	}
}
