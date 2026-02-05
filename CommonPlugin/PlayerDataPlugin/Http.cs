using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PlayerDataPlugin
{
	// Token: 0x02000003 RID: 3
	public class Http : WebClient
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002078 File Offset: 0x00000278
		public Http()
			: this("application/json")
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002085 File Offset: 0x00000285
		public Http(string contentType)
		{
			base.Encoding = Encoding.UTF8;
			base.Headers[HttpRequestHeader.ContentType] = contentType;
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(Http.ValidateServerCertificate);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020B7 File Offset: 0x000002B7
		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = base.GetWebRequest(address);
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			webRequest.Timeout = 15000;
			return webRequest;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020D2 File Offset: 0x000002D2
		protected static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		// Token: 0x04000002 RID: 2
		private const int TIME_OUT = 15000;
	}
}
