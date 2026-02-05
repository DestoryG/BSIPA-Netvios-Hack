using System;
using System.Collections;
using System.Globalization;

namespace System.Diagnostics
{
	// Token: 0x020004DB RID: 1243
	public class InstanceDataCollectionCollection : DictionaryBase
	{
		// Token: 0x06002EE6 RID: 12006 RVA: 0x000D29B3 File Offset: 0x000D0BB3
		[Obsolete("This constructor has been deprecated.  Please use System.Diagnostics.PerformanceCounterCategory.ReadCategory() to get an instance of this collection instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public InstanceDataCollectionCollection()
		{
		}

		// Token: 0x17000B6B RID: 2923
		public InstanceDataCollection this[string counterName]
		{
			get
			{
				if (counterName == null)
				{
					throw new ArgumentNullException("counterName");
				}
				object obj = counterName.ToLower(CultureInfo.InvariantCulture);
				return (InstanceDataCollection)base.Dictionary[obj];
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06002EE8 RID: 12008 RVA: 0x000D29F4 File Offset: 0x000D0BF4
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x000D2A01 File Offset: 0x000D0C01
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000D2A10 File Offset: 0x000D0C10
		internal void Add(string counterName, InstanceDataCollection value)
		{
			object obj = counterName.ToLower(CultureInfo.InvariantCulture);
			base.Dictionary.Add(obj, value);
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000D2A38 File Offset: 0x000D0C38
		public bool Contains(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			object obj = counterName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(obj);
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000D2A6B File Offset: 0x000D0C6B
		public void CopyTo(InstanceDataCollection[] counters, int index)
		{
			base.Dictionary.Values.CopyTo(counters, index);
		}
	}
}
