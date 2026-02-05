using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F2 RID: 1522
	[ComVisible(true)]
	public interface IMenuCommandService
	{
		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06003831 RID: 14385
		DesignerVerbCollection Verbs { get; }

		// Token: 0x06003832 RID: 14386
		void AddCommand(MenuCommand command);

		// Token: 0x06003833 RID: 14387
		void AddVerb(DesignerVerb verb);

		// Token: 0x06003834 RID: 14388
		MenuCommand FindCommand(CommandID commandID);

		// Token: 0x06003835 RID: 14389
		bool GlobalInvoke(CommandID commandID);

		// Token: 0x06003836 RID: 14390
		void RemoveCommand(MenuCommand command);

		// Token: 0x06003837 RID: 14391
		void RemoveVerb(DesignerVerb verb);

		// Token: 0x06003838 RID: 14392
		void ShowContextMenu(CommandID menuID, int x, int y);
	}
}
