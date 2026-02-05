using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004ED RID: 1261
	[Serializable]
	public class PerformanceCounterPermissionEntryCollection : CollectionBase
	{
		// Token: 0x06002F8E RID: 12174 RVA: 0x000D6E24 File Offset: 0x000D5024
		internal PerformanceCounterPermissionEntryCollection(PerformanceCounterPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new PerformanceCounterPermissionEntry(entries[i]));
			}
		}

		// Token: 0x17000B8B RID: 2955
		public PerformanceCounterPermissionEntry this[int index]
		{
			get
			{
				return (PerformanceCounterPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000D6E82 File Offset: 0x000D5082
		public int Add(PerformanceCounterPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000D6E90 File Offset: 0x000D5090
		public void AddRange(PerformanceCounterPermissionEntry[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000D6EC4 File Offset: 0x000D50C4
		public void AddRange(PerformanceCounterPermissionEntryCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000D6F00 File Offset: 0x000D5100
		public bool Contains(PerformanceCounterPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000D6F0E File Offset: 0x000D510E
		public void CopyTo(PerformanceCounterPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000D6F1D File Offset: 0x000D511D
		public int IndexOf(PerformanceCounterPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000D6F2B File Offset: 0x000D512B
		public void Insert(int index, PerformanceCounterPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000D6F3A File Offset: 0x000D513A
		public void Remove(PerformanceCounterPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000D6F48 File Offset: 0x000D5148
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000D6F55 File Offset: 0x000D5155
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((PerformanceCounterPermissionEntry)value);
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x000D6F68 File Offset: 0x000D5168
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((PerformanceCounterPermissionEntry)value);
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000D6F7B File Offset: 0x000D517B
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((PerformanceCounterPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((PerformanceCounterPermissionEntry)newValue);
		}

		// Token: 0x04002806 RID: 10246
		private PerformanceCounterPermission owner;
	}
}
