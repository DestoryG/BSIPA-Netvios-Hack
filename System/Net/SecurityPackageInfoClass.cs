using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000217 RID: 535
	internal class SecurityPackageInfoClass
	{
		// Token: 0x060013C1 RID: 5057 RVA: 0x000686B0 File Offset: 0x000668B0
		internal SecurityPackageInfoClass(SafeHandle safeHandle, int index)
		{
			if (safeHandle.IsInvalid)
			{
				return;
			}
			IntPtr intPtr = IntPtrHelper.Add(safeHandle.DangerousGetHandle(), SecurityPackageInfo.Size * index);
			this.Capabilities = Marshal.ReadInt32(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Capabilities"));
			this.Version = Marshal.ReadInt16(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Version"));
			this.RPCID = Marshal.ReadInt16(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "RPCID"));
			this.MaxToken = Marshal.ReadInt32(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "MaxToken"));
			IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Name"));
			if (intPtr2 != IntPtr.Zero)
			{
				this.Name = Marshal.PtrToStringUni(intPtr2);
			}
			intPtr2 = Marshal.ReadIntPtr(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Comment"));
			if (intPtr2 != IntPtr.Zero)
			{
				this.Comment = Marshal.PtrToStringUni(intPtr2);
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x000687E8 File Offset: 0x000669E8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Capabilities:",
				string.Format(CultureInfo.InvariantCulture, "0x{0:x}", new object[] { this.Capabilities }),
				" Version:",
				this.Version.ToString(NumberFormatInfo.InvariantInfo),
				" RPCID:",
				this.RPCID.ToString(NumberFormatInfo.InvariantInfo),
				" MaxToken:",
				this.MaxToken.ToString(NumberFormatInfo.InvariantInfo),
				" Name:",
				(this.Name == null) ? "(null)" : this.Name,
				" Comment:",
				(this.Comment == null) ? "(null)" : this.Comment
			});
		}

		// Token: 0x040015BF RID: 5567
		internal int Capabilities;

		// Token: 0x040015C0 RID: 5568
		internal short Version;

		// Token: 0x040015C1 RID: 5569
		internal short RPCID;

		// Token: 0x040015C2 RID: 5570
		internal int MaxToken;

		// Token: 0x040015C3 RID: 5571
		internal string Name;

		// Token: 0x040015C4 RID: 5572
		internal string Comment;
	}
}
