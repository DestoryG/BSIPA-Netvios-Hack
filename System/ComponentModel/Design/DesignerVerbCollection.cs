using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D9 RID: 1497
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerVerbCollection : CollectionBase
	{
		// Token: 0x0600379E RID: 14238 RVA: 0x000F0709 File Offset: 0x000EE909
		public DesignerVerbCollection()
		{
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x000F0711 File Offset: 0x000EE911
		public DesignerVerbCollection(DesignerVerb[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000D62 RID: 3426
		public DesignerVerb this[int index]
		{
			get
			{
				return (DesignerVerb)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x000F0742 File Offset: 0x000EE942
		public int Add(DesignerVerb value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x000F0750 File Offset: 0x000EE950
		public void AddRange(DesignerVerb[] value)
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

		// Token: 0x060037A4 RID: 14244 RVA: 0x000F0784 File Offset: 0x000EE984
		public void AddRange(DesignerVerbCollection value)
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

		// Token: 0x060037A5 RID: 14245 RVA: 0x000F07C0 File Offset: 0x000EE9C0
		public void Insert(int index, DesignerVerb value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000F07CF File Offset: 0x000EE9CF
		public int IndexOf(DesignerVerb value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000F07DD File Offset: 0x000EE9DD
		public bool Contains(DesignerVerb value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000F07EB File Offset: 0x000EE9EB
		public void Remove(DesignerVerb value)
		{
			base.List.Remove(value);
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000F07F9 File Offset: 0x000EE9F9
		public void CopyTo(DesignerVerb[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000F0808 File Offset: 0x000EEA08
		protected override void OnSet(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000F080A File Offset: 0x000EEA0A
		protected override void OnInsert(int index, object value)
		{
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x000F080C File Offset: 0x000EEA0C
		protected override void OnClear()
		{
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x000F080E File Offset: 0x000EEA0E
		protected override void OnRemove(int index, object value)
		{
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x000F0810 File Offset: 0x000EEA10
		protected override void OnValidate(object value)
		{
		}
	}
}
