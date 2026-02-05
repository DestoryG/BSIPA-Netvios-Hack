using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E3 RID: 1507
	[ComVisible(true)]
	public interface IComponentChangeService
	{
		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060037D4 RID: 14292
		// (remove) Token: 0x060037D5 RID: 14293
		event ComponentEventHandler ComponentAdded;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060037D6 RID: 14294
		// (remove) Token: 0x060037D7 RID: 14295
		event ComponentEventHandler ComponentAdding;

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060037D8 RID: 14296
		// (remove) Token: 0x060037D9 RID: 14297
		event ComponentChangedEventHandler ComponentChanged;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060037DA RID: 14298
		// (remove) Token: 0x060037DB RID: 14299
		event ComponentChangingEventHandler ComponentChanging;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x060037DC RID: 14300
		// (remove) Token: 0x060037DD RID: 14301
		event ComponentEventHandler ComponentRemoved;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060037DE RID: 14302
		// (remove) Token: 0x060037DF RID: 14303
		event ComponentEventHandler ComponentRemoving;

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x060037E0 RID: 14304
		// (remove) Token: 0x060037E1 RID: 14305
		event ComponentRenameEventHandler ComponentRename;

		// Token: 0x060037E2 RID: 14306
		void OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue);

		// Token: 0x060037E3 RID: 14307
		void OnComponentChanging(object component, MemberDescriptor member);
	}
}
