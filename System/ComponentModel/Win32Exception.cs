using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.ComponentModel
{
	// Token: 0x020005BC RID: 1468
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class Win32Exception : ExternalException, ISerializable
	{
		// Token: 0x06003706 RID: 14086 RVA: 0x000EF793 File Offset: 0x000ED993
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception()
			: this(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x000EF7A0 File Offset: 0x000ED9A0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(int error)
			: this(error, Win32Exception.GetErrorMessage(error))
		{
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x000EF7AF File Offset: 0x000ED9AF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(int error, string message)
			: base(message)
		{
			this.nativeErrorCode = error;
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x000EF7BF File Offset: 0x000ED9BF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(string message)
			: this(Marshal.GetLastWin32Error(), message)
		{
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x000EF7CD File Offset: 0x000ED9CD
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(string message, Exception innerException)
			: base(message, innerException)
		{
			this.nativeErrorCode = Marshal.GetLastWin32Error();
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x000EF7E2 File Offset: 0x000ED9E2
		protected Win32Exception(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.nativeErrorCode = info.GetInt32("NativeErrorCode");
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600370C RID: 14092 RVA: 0x000EF807 File Offset: 0x000EDA07
		public int NativeErrorCode
		{
			get
			{
				return this.nativeErrorCode;
			}
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x000EF810 File Offset: 0x000EDA10
		private static bool TryGetErrorMessage(int error, StringBuilder sb, out string errorMsg)
		{
			errorMsg = "";
			int num = SafeNativeMethods.FormatMessage(12800, IntPtr.Zero, (uint)error, 0, sb, sb.Capacity + 1, null);
			if (num != 0)
			{
				int i;
				for (i = sb.Length; i > 0; i--)
				{
					char c = sb[i - 1];
					if (c > ' ' && c != '.')
					{
						break;
					}
				}
				errorMsg = sb.ToString(0, i);
			}
			else
			{
				if (Marshal.GetLastWin32Error() == 122)
				{
					return false;
				}
				errorMsg = "Unknown error (0x" + Convert.ToString(error, 16) + ")";
			}
			return true;
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x000EF89C File Offset: 0x000EDA9C
		private static string GetErrorMessage(int error)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			string text;
			while (!Win32Exception.TryGetErrorMessage(error, stringBuilder, out text))
			{
				stringBuilder.Capacity *= 4;
				if (stringBuilder.Capacity >= 66560)
				{
					return "Unknown error (0x" + Convert.ToString(error, 16) + ")";
				}
			}
			return text;
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x000EF8F3 File Offset: 0x000EDAF3
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("NativeErrorCode", this.nativeErrorCode);
			base.GetObjectData(info, context);
		}

		// Token: 0x04002AB6 RID: 10934
		private readonly int nativeErrorCode;

		// Token: 0x04002AB7 RID: 10935
		private const int MaxAllowedBufferSize = 66560;
	}
}
