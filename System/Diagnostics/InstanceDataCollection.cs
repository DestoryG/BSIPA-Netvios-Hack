using System;
using System.Collections;
using System.Globalization;

namespace System.Diagnostics
{
	// Token: 0x020004DA RID: 1242
	public class InstanceDataCollection : DictionaryBase
	{
		// Token: 0x06002EDE RID: 11998 RVA: 0x000D28BC File Offset: 0x000D0ABC
		[Obsolete("This constructor has been deprecated.  Please use System.Diagnostics.InstanceDataCollectionCollection.get_Item to get an instance of this collection instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public InstanceDataCollection(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			this.counterName = counterName;
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06002EDF RID: 11999 RVA: 0x000D28D9 File Offset: 0x000D0AD9
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x000D28E1 File Offset: 0x000D0AE1
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06002EE1 RID: 12001 RVA: 0x000D28EE File Offset: 0x000D0AEE
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x17000B6A RID: 2922
		public InstanceData this[string instanceName]
		{
			get
			{
				if (instanceName == null)
				{
					throw new ArgumentNullException("instanceName");
				}
				if (instanceName.Length == 0)
				{
					instanceName = "systemdiagnosticsperfcounterlibsingleinstance";
				}
				object obj = instanceName.ToLower(CultureInfo.InvariantCulture);
				return (InstanceData)base.Dictionary[obj];
			}
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000D2944 File Offset: 0x000D0B44
		internal void Add(string instanceName, InstanceData value)
		{
			object obj = instanceName.ToLower(CultureInfo.InvariantCulture);
			base.Dictionary.Add(obj, value);
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000D296C File Offset: 0x000D0B6C
		public bool Contains(string instanceName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			object obj = instanceName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(obj);
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000D299F File Offset: 0x000D0B9F
		public void CopyTo(InstanceData[] instances, int index)
		{
			base.Dictionary.Values.CopyTo(instances, index);
		}

		// Token: 0x04002797 RID: 10135
		private string counterName;
	}
}
