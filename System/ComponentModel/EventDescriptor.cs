using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200054F RID: 1359
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class EventDescriptor : MemberDescriptor
	{
		// Token: 0x060032FE RID: 13054 RVA: 0x000E307C File Offset: 0x000E127C
		protected EventDescriptor(string name, Attribute[] attrs)
			: base(name, attrs)
		{
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x000E3086 File Offset: 0x000E1286
		protected EventDescriptor(MemberDescriptor descr)
			: base(descr)
		{
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x000E308F File Offset: 0x000E128F
		protected EventDescriptor(MemberDescriptor descr, Attribute[] attrs)
			: base(descr, attrs)
		{
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06003301 RID: 13057
		public abstract Type ComponentType { get; }

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06003302 RID: 13058
		public abstract Type EventType { get; }

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06003303 RID: 13059
		public abstract bool IsMulticast { get; }

		// Token: 0x06003304 RID: 13060
		public abstract void AddEventHandler(object component, Delegate value);

		// Token: 0x06003305 RID: 13061
		public abstract void RemoveEventHandler(object component, Delegate value);
	}
}
