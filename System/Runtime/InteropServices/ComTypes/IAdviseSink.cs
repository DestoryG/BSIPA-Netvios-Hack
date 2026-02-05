using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020003E1 RID: 993
	[Guid("0000010F-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[global::__DynamicallyInvokable]
	[ComImport]
	public interface IAdviseSink
	{
		// Token: 0x060025FF RID: 9727
		[global::__DynamicallyInvokable]
		[PreserveSig]
		void OnDataChange([In] ref FORMATETC format, [In] ref STGMEDIUM stgmedium);

		// Token: 0x06002600 RID: 9728
		[global::__DynamicallyInvokable]
		[PreserveSig]
		void OnViewChange(int aspect, int index);

		// Token: 0x06002601 RID: 9729
		[global::__DynamicallyInvokable]
		[PreserveSig]
		void OnRename(IMoniker moniker);

		// Token: 0x06002602 RID: 9730
		[global::__DynamicallyInvokable]
		[PreserveSig]
		void OnSave();

		// Token: 0x06002603 RID: 9731
		[global::__DynamicallyInvokable]
		[PreserveSig]
		void OnClose();
	}
}
