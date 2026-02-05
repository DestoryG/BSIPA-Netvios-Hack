using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B3 RID: 179
	internal struct KeyPair<Key1, Key2> : IEquatable<KeyPair<Key1, Key2>>
	{
		// Token: 0x0600062C RID: 1580 RVA: 0x0001DBE7 File Offset: 0x0001BDE7
		public KeyPair(Key1 pKey1, Key2 pKey2)
		{
			this._pKey1 = pKey1;
			this._pKey2 = pKey2;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001DBF7 File Offset: 0x0001BDF7
		public bool Equals(KeyPair<Key1, Key2> other)
		{
			return object.Equals(this._pKey1, other._pKey1) && object.Equals(this._pKey2, other._pKey2);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001DC33 File Offset: 0x0001BE33
		public override bool Equals(object obj)
		{
			return obj is KeyPair<Key1, Key2> && this.Equals((KeyPair<Key1, Key2>)obj);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001DC4C File Offset: 0x0001BE4C
		public override int GetHashCode()
		{
			int num;
			if (this._pKey1 != null)
			{
				Key1 pKey = this._pKey1;
				num = pKey.GetHashCode();
			}
			else
			{
				num = 0;
			}
			int num2;
			if (this._pKey2 != null)
			{
				Key2 pKey2 = this._pKey2;
				num2 = pKey2.GetHashCode();
			}
			else
			{
				num2 = 0;
			}
			return num + num2;
		}

		// Token: 0x040005AA RID: 1450
		private readonly Key1 _pKey1;

		// Token: 0x040005AB RID: 1451
		private readonly Key2 _pKey2;
	}
}
