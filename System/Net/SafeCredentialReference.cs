using System;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001FB RID: 507
	internal sealed class SafeCredentialReference : CriticalHandleMinusOneIsInvalid
	{
		// Token: 0x0600132C RID: 4908 RVA: 0x00064AB4 File Offset: 0x00062CB4
		internal static SafeCredentialReference CreateReference(SafeFreeCredentials target)
		{
			SafeCredentialReference safeCredentialReference = new SafeCredentialReference(target);
			if (safeCredentialReference.IsInvalid)
			{
				return null;
			}
			return safeCredentialReference;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00064AD4 File Offset: 0x00062CD4
		private SafeCredentialReference(SafeFreeCredentials target)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				target.DangerousAddRef(ref flag);
			}
			catch
			{
				if (flag)
				{
					target.DangerousRelease();
					flag = false;
				}
			}
			finally
			{
				if (flag)
				{
					this._Target = target;
					base.SetHandle(new IntPtr(0));
				}
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00064B3C File Offset: 0x00062D3C
		protected override bool ReleaseHandle()
		{
			SafeFreeCredentials target = this._Target;
			if (target != null)
			{
				target.DangerousRelease();
			}
			this._Target = null;
			return true;
		}

		// Token: 0x04001549 RID: 5449
		internal SafeFreeCredentials _Target;
	}
}
