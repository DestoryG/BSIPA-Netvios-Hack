using System;
using System.Globalization;
using System.Text;

namespace System.Net
{
	// Token: 0x02000195 RID: 405
	internal class BasicClient : IAuthenticationModule
	{
		// Token: 0x06000FAF RID: 4015 RVA: 0x0005208C File Offset: 0x0005028C
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || credentials is SystemNetworkCredential)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null || httpWebRequest.ChallengedUri == null)
			{
				return null;
			}
			int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, BasicClient.Signature);
			if (num < 0)
			{
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x000520DA File Offset: 0x000502DA
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x000520E0 File Offset: 0x000502E0
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || credentials is SystemNetworkCredential)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0005210E File Offset: 0x0005030E
		public string AuthenticationType
		{
			get
			{
				return "Basic";
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00052118 File Offset: 0x00050318
		private Authorization Lookup(HttpWebRequest httpWebRequest, ICredentials credentials)
		{
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, BasicClient.Signature);
			if (credential == null)
			{
				return null;
			}
			ICredentialPolicy credentialPolicy = AuthenticationManager.CredentialPolicy;
			if (credentialPolicy != null && !credentialPolicy.ShouldSendCredential(httpWebRequest.ChallengedUri, httpWebRequest, credential, this))
			{
				return null;
			}
			string text = credential.InternalGetUserName();
			string text2 = credential.InternalGetDomain();
			if (ValidationHelper.IsBlankString(text))
			{
				return null;
			}
			string text3 = ((!ValidationHelper.IsBlankString(text2)) ? (text2 + "\\") : "") + text + ":" + credential.InternalGetPassword();
			byte[] array = BasicClient.EncodingRightGetBytes(text3);
			string text4 = "Basic " + Convert.ToBase64String(array);
			return new Authorization(text4, true);
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x000521C4 File Offset: 0x000503C4
		internal static byte[] EncodingRightGetBytes(string rawString)
		{
			byte[] bytes = Encoding.Default.GetBytes(rawString);
			string @string = Encoding.Default.GetString(bytes);
			if (string.Compare(rawString, @string, StringComparison.Ordinal) != 0)
			{
				throw ExceptionHelper.MethodNotSupportedException;
			}
			return bytes;
		}

		// Token: 0x040012D7 RID: 4823
		internal const string AuthType = "Basic";

		// Token: 0x040012D8 RID: 4824
		internal static string Signature = "Basic".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x040012D9 RID: 4825
		internal static int SignatureSize = BasicClient.Signature.Length;
	}
}
