using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000585 RID: 1413
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ListChangedEventArgs : EventArgs
	{
		// Token: 0x06003427 RID: 13351 RVA: 0x000E4816 File Offset: 0x000E2A16
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex)
			: this(listChangedType, newIndex, -1)
		{
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000E4821 File Offset: 0x000E2A21
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, PropertyDescriptor propDesc)
			: this(listChangedType, newIndex)
		{
			this.propDesc = propDesc;
			this.oldIndex = newIndex;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000E4839 File Offset: 0x000E2A39
		public ListChangedEventArgs(ListChangedType listChangedType, PropertyDescriptor propDesc)
		{
			this.listChangedType = listChangedType;
			this.propDesc = propDesc;
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000E484F File Offset: 0x000E2A4F
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex)
		{
			this.listChangedType = listChangedType;
			this.newIndex = newIndex;
			this.oldIndex = oldIndex;
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x000E486C File Offset: 0x000E2A6C
		public ListChangedType ListChangedType
		{
			get
			{
				return this.listChangedType;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x0600342C RID: 13356 RVA: 0x000E4874 File Offset: 0x000E2A74
		public int NewIndex
		{
			get
			{
				return this.newIndex;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x000E487C File Offset: 0x000E2A7C
		public int OldIndex
		{
			get
			{
				return this.oldIndex;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x000E4884 File Offset: 0x000E2A84
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propDesc;
			}
		}

		// Token: 0x040029CD RID: 10701
		private ListChangedType listChangedType;

		// Token: 0x040029CE RID: 10702
		private int newIndex;

		// Token: 0x040029CF RID: 10703
		private int oldIndex;

		// Token: 0x040029D0 RID: 10704
		private PropertyDescriptor propDesc;
	}
}
