using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F7 RID: 503
	internal sealed class SafeCertSelectCritera : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x00064538 File Offset: 0x00062738
		internal int Count
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0006453C File Offset: 0x0006273C
		private IntPtr AllocBuffer(int size)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(size);
			this.unmanagedMemoryList.Add(intPtr);
			return intPtr;
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00064560 File Offset: 0x00062760
		private IntPtr AllocString(string str)
		{
			IntPtr intPtr = Marshal.StringToHGlobalAnsi(str);
			this.unmanagedMemoryList.Add(intPtr);
			return intPtr;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00064584 File Offset: 0x00062784
		internal SafeCertSelectCritera()
			: base(true)
		{
			UnsafeNclNativeMethods.NativePKI.CERT_SELECT_CRITERIA cert_SELECT_CRITERIA = default(UnsafeNclNativeMethods.NativePKI.CERT_SELECT_CRITERIA);
			this.unmanagedMemoryList = new List<IntPtr>();
			IntPtr intPtr = this.AllocBuffer(2 * Marshal.SizeOf(cert_SELECT_CRITERIA));
			base.SetHandle(intPtr);
			cert_SELECT_CRITERIA.dwType = 1U;
			cert_SELECT_CRITERIA.cPara = 1U;
			IntPtr intPtr2 = this.AllocString("1.3.6.1.5.5.7.3.2");
			IntPtr intPtr3 = this.AllocBuffer(Marshal.SizeOf(intPtr2));
			Marshal.WriteIntPtr(intPtr3, intPtr2);
			cert_SELECT_CRITERIA.ppPara = intPtr3;
			Marshal.StructureToPtr(cert_SELECT_CRITERIA, intPtr, false);
			cert_SELECT_CRITERIA = default(UnsafeNclNativeMethods.NativePKI.CERT_SELECT_CRITERIA);
			cert_SELECT_CRITERIA.dwType = 2U;
			cert_SELECT_CRITERIA.cPara = 1U;
			UnsafeNclNativeMethods.NativePKI.CERT_EXTENSION cert_EXTENSION = default(UnsafeNclNativeMethods.NativePKI.CERT_EXTENSION);
			cert_EXTENSION.pszObjId = IntPtr.Zero;
			cert_EXTENSION.fCritical = 0U;
			cert_EXTENSION.Value.cbData = 1U;
			IntPtr intPtr4 = this.AllocBuffer(Marshal.SizeOf(128));
			Marshal.WriteByte(intPtr4, 128);
			cert_EXTENSION.Value.pbData = intPtr4;
			IntPtr intPtr5 = this.AllocBuffer(Marshal.SizeOf(cert_EXTENSION));
			Marshal.StructureToPtr(cert_EXTENSION, intPtr5, false);
			intPtr3 = this.AllocBuffer(Marshal.SizeOf(intPtr5));
			Marshal.WriteIntPtr(intPtr3, intPtr5);
			cert_SELECT_CRITERIA.ppPara = intPtr3;
			Marshal.StructureToPtr(cert_SELECT_CRITERIA, intPtr + Marshal.SizeOf(cert_SELECT_CRITERIA), false);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x000646E8 File Offset: 0x000628E8
		public override string ToString()
		{
			return "0x" + base.DangerousGetHandle().ToString("x");
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00064714 File Offset: 0x00062914
		protected override bool ReleaseHandle()
		{
			try
			{
				foreach (IntPtr intPtr in this.unmanagedMemoryList)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x0400153D RID: 5437
		private const string szOID_PKIX_KP_CLIENT_AUTH = "1.3.6.1.5.5.7.3.2";

		// Token: 0x0400153E RID: 5438
		private const int CERT_SELECT_BY_ENHKEY_USAGE = 1;

		// Token: 0x0400153F RID: 5439
		private const int CERT_SELECT_BY_KEY_USAGE = 2;

		// Token: 0x04001540 RID: 5440
		private const byte CERT_DIGITAL_SIGNATURE_KEY_USAGE = 128;

		// Token: 0x04001541 RID: 5441
		private const int criteriaCount = 2;

		// Token: 0x04001542 RID: 5442
		private List<IntPtr> unmanagedMemoryList;
	}
}
