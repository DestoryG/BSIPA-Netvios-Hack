using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004D4 RID: 1236
	[Serializable]
	public class EventLogPermissionEntryCollection : CollectionBase
	{
		// Token: 0x06002E80 RID: 11904 RVA: 0x000D1A24 File Offset: 0x000CFC24
		internal EventLogPermissionEntryCollection(EventLogPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new EventLogPermissionEntry(entries[i]));
			}
		}

		// Token: 0x17000B3F RID: 2879
		public EventLogPermissionEntry this[int index]
		{
			get
			{
				return (EventLogPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000D1A82 File Offset: 0x000CFC82
		public int Add(EventLogPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000D1A90 File Offset: 0x000CFC90
		public void AddRange(EventLogPermissionEntry[] value)
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

		// Token: 0x06002E85 RID: 11909 RVA: 0x000D1AC4 File Offset: 0x000CFCC4
		public void AddRange(EventLogPermissionEntryCollection value)
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

		// Token: 0x06002E86 RID: 11910 RVA: 0x000D1B00 File Offset: 0x000CFD00
		public bool Contains(EventLogPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000D1B0E File Offset: 0x000CFD0E
		public void CopyTo(EventLogPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000D1B1D File Offset: 0x000CFD1D
		public int IndexOf(EventLogPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000D1B2B File Offset: 0x000CFD2B
		public void Insert(int index, EventLogPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000D1B3A File Offset: 0x000CFD3A
		public void Remove(EventLogPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000D1B48 File Offset: 0x000CFD48
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000D1B55 File Offset: 0x000CFD55
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((EventLogPermissionEntry)value);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000D1B68 File Offset: 0x000CFD68
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((EventLogPermissionEntry)value);
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000D1B7B File Offset: 0x000CFD7B
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((EventLogPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((EventLogPermissionEntry)newValue);
		}

		// Token: 0x04002774 RID: 10100
		private EventLogPermission owner;
	}
}
