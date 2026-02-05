using System;
using System.Collections;

namespace System.Net.Mail
{
	// Token: 0x02000277 RID: 631
	internal static class SmtpAuthenticationManager
	{
		// Token: 0x060017AE RID: 6062 RVA: 0x00078BA0 File Offset: 0x00076DA0
		static SmtpAuthenticationManager()
		{
			SmtpAuthenticationManager.Register(new SmtpNegotiateAuthenticationModule());
			SmtpAuthenticationManager.Register(new SmtpNtlmAuthenticationModule());
			SmtpAuthenticationManager.Register(new SmtpDigestAuthenticationModule());
			SmtpAuthenticationManager.Register(new SmtpLoginAuthenticationModule());
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00078BD4 File Offset: 0x00076DD4
		internal static void Register(ISmtpAuthenticationModule module)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			ArrayList arrayList = SmtpAuthenticationManager.modules;
			lock (arrayList)
			{
				SmtpAuthenticationManager.modules.Add(module);
			}
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00078C28 File Offset: 0x00076E28
		internal static ISmtpAuthenticationModule[] GetModules()
		{
			ArrayList arrayList = SmtpAuthenticationManager.modules;
			ISmtpAuthenticationModule[] array2;
			lock (arrayList)
			{
				ISmtpAuthenticationModule[] array = new ISmtpAuthenticationModule[SmtpAuthenticationManager.modules.Count];
				SmtpAuthenticationManager.modules.CopyTo(0, array, 0, SmtpAuthenticationManager.modules.Count);
				array2 = array;
			}
			return array2;
		}

		// Token: 0x040017F4 RID: 6132
		private static ArrayList modules = new ArrayList();
	}
}
