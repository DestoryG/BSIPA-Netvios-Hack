using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000C0 RID: 192
	public class AuthenticationManager
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x000242EA File Offset: 0x000224EA
		private AuthenticationManager()
		{
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x000242F4 File Offset: 0x000224F4
		private static IAuthenticationManager Instance
		{
			get
			{
				if (AuthenticationManager.internalInstance == null)
				{
					object obj = AuthenticationManager.instanceLock;
					lock (obj)
					{
						if (AuthenticationManager.internalInstance == null)
						{
							AuthenticationManager.internalInstance = AuthenticationManager.SelectAuthenticationManagerInstance();
						}
					}
				}
				return AuthenticationManager.internalInstance;
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0002434C File Offset: 0x0002254C
		private static IAuthenticationManager SelectAuthenticationManagerInstance()
		{
			bool flag = false;
			try
			{
				if (RegistryConfiguration.GlobalConfigReadInt("System.Net.AuthenticationManager.HighPerformance", 0) == 1)
				{
					flag = true;
				}
				else if (RegistryConfiguration.AppConfigReadInt("System.Net.AuthenticationManager.HighPerformance", 0) == 1)
				{
					flag = true;
				}
				if (flag)
				{
					int? num = AuthenticationManager.ReadPrefixLookupMaxEntriesConfig();
					if (num != null)
					{
						int? num2 = num;
						int num3 = 0;
						if ((num2.GetValueOrDefault() > num3) & (num2 != null))
						{
							return new AuthenticationManager2(num.Value);
						}
					}
					return new AuthenticationManager2();
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
			}
			return new AuthenticationManagerDefault();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x000243FC File Offset: 0x000225FC
		private static int? ReadPrefixLookupMaxEntriesConfig()
		{
			int? num = null;
			int num2 = RegistryConfiguration.GlobalConfigReadInt("System.Net.AuthenticationManager.PrefixLookupMaxCount", -1);
			if (num2 > 0)
			{
				num = new int?(num2);
			}
			num2 = RegistryConfiguration.AppConfigReadInt("System.Net.AuthenticationManager.PrefixLookupMaxCount", -1);
			if (num2 > 0)
			{
				num = new int?(num2);
			}
			return num;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00024442 File Offset: 0x00022642
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x0002444E File Offset: 0x0002264E
		public static ICredentialPolicy CredentialPolicy
		{
			get
			{
				return AuthenticationManager.Instance.CredentialPolicy;
			}
			set
			{
				ExceptionHelper.ControlPolicyPermission.Demand();
				AuthenticationManager.Instance.CredentialPolicy = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00024465 File Offset: 0x00022665
		public static StringDictionary CustomTargetNameDictionary
		{
			get
			{
				return AuthenticationManager.Instance.CustomTargetNameDictionary;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00024471 File Offset: 0x00022671
		internal static SpnDictionary SpnDictionary
		{
			get
			{
				return AuthenticationManager.Instance.SpnDictionary;
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002447D File Offset: 0x0002267D
		internal static void EnsureConfigLoaded()
		{
			AuthenticationManager.Instance.EnsureConfigLoaded();
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00024489 File Offset: 0x00022689
		internal static bool OSSupportsExtendedProtection
		{
			get
			{
				return AuthenticationManager.Instance.OSSupportsExtendedProtection;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00024495 File Offset: 0x00022695
		internal static bool SspSupportsExtendedProtection
		{
			get
			{
				return AuthenticationManager.Instance.SspSupportsExtendedProtection;
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000244A1 File Offset: 0x000226A1
		public static Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials)
		{
			return AuthenticationManager.Instance.Authenticate(challenge, request, credentials);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000244B0 File Offset: 0x000226B0
		public static Authorization PreAuthenticate(WebRequest request, ICredentials credentials)
		{
			return AuthenticationManager.Instance.PreAuthenticate(request, credentials);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000244BE File Offset: 0x000226BE
		public static void Register(IAuthenticationModule authenticationModule)
		{
			ExceptionHelper.UnmanagedPermission.Demand();
			AuthenticationManager.Instance.Register(authenticationModule);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000244D5 File Offset: 0x000226D5
		public static void Unregister(IAuthenticationModule authenticationModule)
		{
			ExceptionHelper.UnmanagedPermission.Demand();
			AuthenticationManager.Instance.Unregister(authenticationModule);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000244EC File Offset: 0x000226EC
		public static void Unregister(string authenticationScheme)
		{
			ExceptionHelper.UnmanagedPermission.Demand();
			AuthenticationManager.Instance.Unregister(authenticationScheme);
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00024503 File Offset: 0x00022703
		public static IEnumerator RegisteredModules
		{
			get
			{
				return AuthenticationManager.Instance.RegisteredModules;
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002450F File Offset: 0x0002270F
		internal static void BindModule(Uri uri, Authorization response, IAuthenticationModule module)
		{
			AuthenticationManager.Instance.BindModule(uri, response, module);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00024520 File Offset: 0x00022720
		internal static int FindSubstringNotInQuotes(string challenge, string signature)
		{
			int num = -1;
			if (challenge != null && signature != null && challenge.Length >= signature.Length)
			{
				int num2 = -1;
				int num3 = -1;
				int num4 = 0;
				while (num4 < challenge.Length && num < 0)
				{
					if (challenge[num4] == '"')
					{
						if (num2 <= num3)
						{
							num2 = num4;
						}
						else
						{
							num3 = num4;
						}
					}
					if (num4 == challenge.Length - 1 || (challenge[num4] == '"' && num2 > num3))
					{
						if (num4 == challenge.Length - 1)
						{
							num2 = challenge.Length;
						}
						if (num2 >= num3 + 3)
						{
							int num5 = num3 + 1;
							int num6 = num2 - num3 - 1;
							do
							{
								num = AuthenticationManager.IndexOf(challenge, signature, num5, num6);
								if (num >= 0)
								{
									if ((num == 0 || challenge[num - 1] == ' ' || challenge[num - 1] == ',') && (num + signature.Length == challenge.Length || challenge[num + signature.Length] == ' ' || challenge[num + signature.Length] == ','))
									{
										break;
									}
									num6 -= num - num5 + 1;
									num5 = num + 1;
								}
							}
							while (num >= 0);
						}
					}
					num4++;
				}
			}
			return num;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00024640 File Offset: 0x00022840
		private static int IndexOf(string challenge, string lwrCaseSignature, int start, int count)
		{
			count += start + 1 - lwrCaseSignature.Length;
			while (start < count)
			{
				int num = 0;
				while (num < lwrCaseSignature.Length && (challenge[start + num] | ' ') == lwrCaseSignature[num])
				{
					num++;
				}
				if (num == lwrCaseSignature.Length)
				{
					return start;
				}
				start++;
			}
			return -1;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00024698 File Offset: 0x00022898
		internal static int SplitNoQuotes(string challenge, ref int offset)
		{
			int num = offset;
			offset = -1;
			if (challenge != null && num < challenge.Length)
			{
				int num2 = -1;
				int num3 = -1;
				for (int i = num; i < challenge.Length; i++)
				{
					if (num2 > num3 && challenge[i] == '\\' && i + 1 < challenge.Length && challenge[i + 1] == '"')
					{
						i++;
					}
					else if (challenge[i] == '"')
					{
						if (num2 <= num3)
						{
							num2 = i;
						}
						else
						{
							num3 = i;
						}
					}
					else if (challenge[i] == '=' && num2 <= num3 && offset < 0)
					{
						offset = i;
					}
					else if (challenge[i] == ',' && num2 <= num3)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00024741 File Offset: 0x00022941
		internal static Authorization GetGroupAuthorization(IAuthenticationModule thisModule, string token, bool finished, NTAuthentication authSession, bool shareAuthenticatedConnections, bool mutualAuth)
		{
			return new Authorization(token, finished, shareAuthenticatedConnections ? null : (thisModule.GetType().FullName + "/" + authSession.UniqueUserId), mutualAuth);
		}

		// Token: 0x04000C72 RID: 3186
		private static object instanceLock = new object();

		// Token: 0x04000C73 RID: 3187
		private static IAuthenticationManager internalInstance = null;

		// Token: 0x04000C74 RID: 3188
		internal const string authenticationManagerRoot = "System.Net.AuthenticationManager";

		// Token: 0x04000C75 RID: 3189
		internal const string configHighPerformance = "System.Net.AuthenticationManager.HighPerformance";

		// Token: 0x04000C76 RID: 3190
		internal const string configPrefixLookupMaxCount = "System.Net.AuthenticationManager.PrefixLookupMaxCount";
	}
}
