using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005C6 RID: 1478
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakHashtable : Hashtable
	{
		// Token: 0x06003742 RID: 14146 RVA: 0x000F001C File Offset: 0x000EE21C
		internal WeakHashtable()
			: base(WeakHashtable._comparer)
		{
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000F0029 File Offset: 0x000EE229
		public override void Clear()
		{
			base.Clear();
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x000F0031 File Offset: 0x000EE231
		public override void Remove(object key)
		{
			base.Remove(key);
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x000F003A File Offset: 0x000EE23A
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x000F0050 File Offset: 0x000EE250
		private void ScavengeKeys()
		{
			int count = this.Count;
			if (count == 0)
			{
				return;
			}
			if (this._lastHashCount == 0)
			{
				this._lastHashCount = count;
				return;
			}
			long totalMemory = GC.GetTotalMemory(false);
			if (this._lastGlobalMem == 0L)
			{
				this._lastGlobalMem = totalMemory;
				return;
			}
			float num = (float)(totalMemory - this._lastGlobalMem) / (float)this._lastGlobalMem;
			float num2 = (float)(count - this._lastHashCount) / (float)this._lastHashCount;
			if (num < 0f && num2 >= 0f)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakReference weakReference = obj as WeakReference;
					if (weakReference != null && !weakReference.IsAlive)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(weakReference);
					}
				}
				if (arrayList != null)
				{
					foreach (object obj2 in arrayList)
					{
						this.Remove(obj2);
					}
				}
			}
			this._lastGlobalMem = totalMemory;
			this._lastHashCount = count;
		}

		// Token: 0x04002ADB RID: 10971
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x04002ADC RID: 10972
		private long _lastGlobalMem;

		// Token: 0x04002ADD RID: 10973
		private int _lastHashCount;

		// Token: 0x020008AB RID: 2219
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x060045F5 RID: 17909 RVA: 0x00124390 File Offset: 0x00122590
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					return y == null;
				}
				if (y != null && x.GetHashCode() == y.GetHashCode())
				{
					WeakReference weakReference = x as WeakReference;
					WeakReference weakReference2 = y as WeakReference;
					if (weakReference != null)
					{
						if (!weakReference.IsAlive)
						{
							return false;
						}
						x = weakReference.Target;
					}
					if (weakReference2 != null)
					{
						if (!weakReference2.IsAlive)
						{
							return false;
						}
						y = weakReference2.Target;
					}
					return x == y;
				}
				return false;
			}

			// Token: 0x060045F6 RID: 17910 RVA: 0x001243F4 File Offset: 0x001225F4
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x020008AC RID: 2220
		private sealed class EqualityWeakReference : WeakReference
		{
			// Token: 0x060045F8 RID: 17912 RVA: 0x00124404 File Offset: 0x00122604
			internal EqualityWeakReference(object o)
				: base(o)
			{
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x060045F9 RID: 17913 RVA: 0x00124419 File Offset: 0x00122619
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || (this.IsAlive && o == this.Target));
			}

			// Token: 0x060045FA RID: 17914 RVA: 0x00124448 File Offset: 0x00122648
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x040037F5 RID: 14325
			private int _hashCode;
		}
	}
}
