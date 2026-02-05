using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020003E2 RID: 994
	[Guid("0000010E-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IDataObject
	{
		// Token: 0x06002604 RID: 9732
		void GetData([In] ref FORMATETC format, out STGMEDIUM medium);

		// Token: 0x06002605 RID: 9733
		void GetDataHere([In] ref FORMATETC format, ref STGMEDIUM medium);

		// Token: 0x06002606 RID: 9734
		[PreserveSig]
		int QueryGetData([In] ref FORMATETC format);

		// Token: 0x06002607 RID: 9735
		[PreserveSig]
		int GetCanonicalFormatEtc([In] ref FORMATETC formatIn, out FORMATETC formatOut);

		// Token: 0x06002608 RID: 9736
		void SetData([In] ref FORMATETC formatIn, [In] ref STGMEDIUM medium, [MarshalAs(UnmanagedType.Bool)] bool release);

		// Token: 0x06002609 RID: 9737
		IEnumFORMATETC EnumFormatEtc(DATADIR direction);

		// Token: 0x0600260A RID: 9738
		[PreserveSig]
		int DAdvise([In] ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection);

		// Token: 0x0600260B RID: 9739
		void DUnadvise(int connection);

		// Token: 0x0600260C RID: 9740
		[PreserveSig]
		int EnumDAdvise(out IEnumSTATDATA enumAdvise);
	}
}
