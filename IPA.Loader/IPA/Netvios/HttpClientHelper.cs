using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IPA.Netvios
{
	/// <summary>
	/// 网络请求类
	/// </summary>
	// Token: 0x02000029 RID: 41
	public class HttpClientHelper
	{
		/// <summary>
		/// Get方法获取Json数据
		/// </summary>
		/// <param name="url"></param>
		/// <param name="webProxy"></param>
		/// <returns></returns>
		// Token: 0x060000E5 RID: 229 RVA: 0x000045BC File Offset: 0x000027BC
		public static string GetHttpResponseJson(string url, IWebProxy webProxy)
		{
			string empty = string.Empty;
			Encoding encoding = Encoding.UTF8;
			return HttpClientHelper.GetStream(HttpClientHelper.CreateGetHttpResponse(new HttpGetParametersModel
			{
				Url = url,
				WebProxy = webProxy,
				Timeout = new int?(20000)
			}), encoding);
		}

		/// <summary>
		/// Post Url获取Json数据
		/// </summary>
		/// <param name="url"></param>
		/// <param name="webProxy"></param>
		/// <returns></returns>
		// Token: 0x060000E6 RID: 230 RVA: 0x00004604 File Offset: 0x00002804
		public static string PostHttpResponseJson(string url, IWebProxy webProxy)
		{
			string empty = string.Empty;
			Encoding encoding = Encoding.UTF8;
			return HttpClientHelper.GetStream(HttpClientHelper.CreatePostHttpResponse(new HttpPostParametersModel
			{
				Url = url,
				RequestEncoding = encoding,
				WebProxy = webProxy,
				Timeout = new int?(20000)
			}), encoding);
		}

		/// <summary>
		///  Post带参数的 Url获取Json数据
		/// </summary>
		/// <param name="url"></param>
		/// <param name="webProxy"></param>
		/// <param name="postDict"></param>
		/// <returns></returns>
		// Token: 0x060000E7 RID: 231 RVA: 0x00004654 File Offset: 0x00002854
		public static string PostHttpResponseJson(string url, IWebProxy webProxy, IDictionary<string, string> postDict)
		{
			string empty = string.Empty;
			Encoding encoding = Encoding.UTF8;
			return HttpClientHelper.GetStream(HttpClientHelper.CreatePostHttpResponse(new HttpPostParametersModel
			{
				Url = url,
				DictParameters = postDict,
				WebProxy = webProxy,
				RequestEncoding = encoding,
				Timeout = new int?(20000)
			}), encoding);
		}

		/// <summary>
		/// 创建GET方式的HTTP请求
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000E8 RID: 232 RVA: 0x000046AC File Offset: 0x000028AC
		public static HttpWebResponse CreateGetHttpResponse(HttpGetParametersModel getParametersModel)
		{
			if (string.IsNullOrEmpty(getParametersModel.Url))
			{
				throw new ArgumentNullException("getParametersModel.Url");
			}
			HttpWebRequest request = WebRequest.Create(getParametersModel.Url) as HttpWebRequest;
			if (getParametersModel.WebProxy != null)
			{
				request.Proxy = getParametersModel.WebProxy;
			}
			request.Method = "GET";
			request.UserAgent = HttpClientHelper.DefaultUserAgent;
			if (!string.IsNullOrEmpty(getParametersModel.UserAgent))
			{
				request.UserAgent = getParametersModel.UserAgent;
			}
			if (getParametersModel.Timeout != null)
			{
				request.Timeout = getParametersModel.Timeout.Value;
			}
			if (getParametersModel.Cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(getParametersModel.Cookies);
			}
			return request.GetResponse() as HttpWebResponse;
		}

		/// <summary>
		/// 创建POST方式的HTTP请求
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000E9 RID: 233 RVA: 0x00004778 File Offset: 0x00002978
		public static HttpWebResponse CreatePostHttpResponse(HttpPostParametersModel postParametersModel)
		{
			if (string.IsNullOrEmpty(postParametersModel.Url))
			{
				throw new ArgumentNullException("postParametersModel.Url");
			}
			if (postParametersModel.RequestEncoding == null)
			{
				throw new ArgumentNullException("postParametersModel.RequestEncoding");
			}
			HttpWebRequest request = null;
			if (postParametersModel.Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpClientHelper.CheckValidationResult);
				request = WebRequest.Create(postParametersModel.Url) as HttpWebRequest;
				request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				request = WebRequest.Create(postParametersModel.Url) as HttpWebRequest;
			}
			if (postParametersModel.WebProxy != null)
			{
				request.Proxy = postParametersModel.WebProxy;
			}
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			if (!string.IsNullOrEmpty(postParametersModel.UserAgent))
			{
				request.UserAgent = postParametersModel.UserAgent;
			}
			else
			{
				request.UserAgent = HttpClientHelper.DefaultUserAgent;
			}
			if (postParametersModel.Timeout != null)
			{
				request.Timeout = postParametersModel.Timeout.Value;
			}
			if (postParametersModel.Cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(postParametersModel.Cookies);
			}
			if (postParametersModel.DictParameters != null && postParametersModel.DictParameters.Count != 0)
			{
				StringBuilder buffer = new StringBuilder();
				int i = 0;
				foreach (string key in postParametersModel.DictParameters.Keys)
				{
					if (i > 0)
					{
						buffer.AppendFormat("&{0}={1}", key, postParametersModel.DictParameters[key]);
					}
					else
					{
						buffer.AppendFormat("{0}={1}", key, postParametersModel.DictParameters[key]);
					}
					i++;
				}
				byte[] data = postParametersModel.RequestEncoding.GetBytes(buffer.ToString());
				using (Stream stream = request.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}
			}
			return request.GetResponse() as HttpWebResponse;
		}

		/// <summary>
		/// 发送Post Json 请求 返回JSon数据
		/// </summary>
		/// <param name="JSONData">要处理的JSON数据</param>
		/// <param name="Url">要提交的URL</param>
		/// <returns>返回的JSON处理字符串</returns>
		// Token: 0x060000EA RID: 234 RVA: 0x00004994 File Offset: 0x00002B94
		public static string GetResponseData(string JSONData, string Url)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentLength = (long)bytes.Length;
			httpWebRequest.ContentType = "application/json;charset=UTF-8";
			httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
			httpWebRequest.Timeout = 60000;
			httpWebRequest.Headers.Set("Pragma", "no-cache");
			Stream streamReceive = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
			Encoding encoding = Encoding.UTF8;
			StreamReader streamReader = new StreamReader(streamReceive, encoding);
			string strResult = streamReader.ReadToEnd();
			streamReceive.Dispose();
			streamReader.Dispose();
			return strResult;
		}

		/// <summary>
		/// 设置https证书校验机制,默认返回True,验证通过
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="certificate"></param>
		/// <param name="chain"></param>
		/// <param name="errors"></param>
		/// <returns></returns>
		// Token: 0x060000EB RID: 235 RVA: 0x00004A38 File Offset: 0x00002C38
		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		/// <summary>
		/// 将response转换成文本
		/// </summary>
		/// <param name="response"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		// Token: 0x060000EC RID: 236 RVA: 0x00004A3C File Offset: 0x00002C3C
		private static string GetStream(HttpWebResponse response, Encoding encoding)
		{
			try
			{
				if (response.StatusCode == HttpStatusCode.OK)
				{
					string text = response.ContentEncoding.ToLower();
					if (text != null && text == "gzip")
					{
						string text2 = HttpClientHelper.Decompress(response.GetResponseStream(), encoding);
						response.Close();
						return text2;
					}
					using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
					{
						string text3 = sr.ReadToEnd();
						sr.Close();
						sr.Dispose();
						response.Close();
						return text3;
					}
				}
				response.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return "";
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004AE8 File Offset: 0x00002CE8
		private static string Decompress(Stream stream, Encoding encoding)
		{
			new byte[100];
			string text;
			using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
			{
				using (StreamReader reader = new StreamReader(gz, encoding))
				{
					text = reader.ReadToEnd();
				}
			}
			return text;
		}

		// Token: 0x0400003E RID: 62
		private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.70 Safari/537.36";
	}
}
