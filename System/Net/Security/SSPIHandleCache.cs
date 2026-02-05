using System;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x02000353 RID: 851
	internal static class SSPIHandleCache
	{
		// Token: 0x06001E87 RID: 7815 RVA: 0x0008FB2C File Offset: 0x0008DD2C
		internal static void CacheCredential(SafeFreeCredentials newHandle)
		{
			try
			{
				SafeCredentialReference safeCredentialReference = SafeCredentialReference.CreateReference(newHandle);
				if (safeCredentialReference != null)
				{
					int num = Interlocked.Increment(ref SSPIHandleCache._Current) & 31;
					safeCredentialReference = Interlocked.Exchange<SafeCredentialReference>(ref SSPIHandleCache._CacheSlots[num], safeCredentialReference);
					if (safeCredentialReference != null)
					{
						safeCredentialReference.Close();
					}
				}
			}
			catch (Exception ex)
			{
				NclUtilities.IsFatal(ex);
			}
		}

		// Token: 0x04001CE0 RID: 7392
		private const int c_MaxCacheSize = 31;

		// Token: 0x04001CE1 RID: 7393
		private static SafeCredentialReference[] _CacheSlots = new SafeCredentialReference[32];

		// Token: 0x04001CE2 RID: 7394
		private static int _Current = -1;
	}
}
