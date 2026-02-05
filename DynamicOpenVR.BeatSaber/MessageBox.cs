using System;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x02000005 RID: 5
	internal class MessageBox
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002100 File Offset: 0x00000300
		internal static DialogResult Show(string message, string title, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxIcon icon = MessageBoxIcon.None)
		{
			uint num = (uint)(buttons | (MessageBoxButtons)icon);
			return (DialogResult)NativeMethods.MessageBox(IntPtr.Zero, message, title, num);
		}
	}
}
