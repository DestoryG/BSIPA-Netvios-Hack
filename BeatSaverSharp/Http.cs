using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BeatSaverSharp.Exceptions;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x02000007 RID: 7
	internal sealed class Http
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000029E0 File Offset: 0x00000BE0
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000029E8 File Offset: 0x00000BE8
		internal HttpOptions Options { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000029F1 File Offset: 0x00000BF1
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000029F9 File Offset: 0x00000BF9
		internal HttpClient Client { get; private set; }

		// Token: 0x06000034 RID: 52 RVA: 0x00002A04 File Offset: 0x00000C04
		internal Http(HttpOptions options = default(HttpOptions))
		{
			this.Options = options;
			if ((options.ApplicationName != null && options.Version == null) || (options.ApplicationName == null && options.Version != null))
			{
				throw new ArgumentException("You must specify either both or none of ApplicationName and Version");
			}
			HttpClientHandler httpClientHandler = new HttpClientHandler
			{
				AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate)
			};
			this.Client = new HttpClient(httpClientHandler)
			{
				BaseAddress = new Uri("https://beatsaberbbs.com/api/"),
				Timeout = (options.Timeout ?? TimeSpan.FromSeconds(30.0))
			};
			string text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			string text2 = "BeatSaverSharp/" + text;
			if (options.ApplicationName != null)
			{
				text2 = string.Format("{0}/{1} {2}", options.ApplicationName, options.Version, text2);
			}
			foreach (ApplicationAgent applicationAgent in options.Agents ?? new ApplicationAgent[0])
			{
				if (applicationAgent.Name == null || applicationAgent.Version == null)
				{
					throw new ArgumentException("All application agents must specify both name and version");
				}
				text2 = string.Concat(new string[]
				{
					text2,
					" ",
					applicationAgent.Name,
					"/",
					applicationAgent.Version.ToString()
				});
			}
			this.Client.DefaultRequestHeaders.Add("User-Agent", text2);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B94 File Offset: 0x00000D94
		internal async Task<HttpResponse> GetAsync(string url, CancellationToken token, IProgress<double> progress = null)
		{
			HttpResponseMessage httpResponseMessage = await this.Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
			HttpResponseMessage resp = httpResponseMessage;
			HttpResponse httpResponse;
			if (resp.StatusCode == (HttpStatusCode)429)
			{
				RateLimitExceededException ex = new RateLimitExceededException(resp);
				if (!this.Options.HandleRateLimits)
				{
					throw ex;
				}
				int num = (int)(ex.RateLimit.Reset - DateTime.Now).TotalMilliseconds;
				if (num > 0)
				{
					await Task.Delay(num).ConfigureAwait(false);
				}
				httpResponse = await this.GetAsync(url, token, progress).ConfigureAwait(false);
			}
			else
			{
				if (token.IsCancellationRequested)
				{
					throw new TaskCanceledException();
				}
				using (MemoryStream ms = new MemoryStream())
				{
					using (Stream s = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false))
					{
						byte[] buffer = new byte[8192];
						long? contentLength = resp.Content.Headers.ContentLength;
						long totalRead = 0L;
						if (progress != null)
						{
							progress.Report(0.0);
						}
						int bytesRead;
						while ((bytesRead = await s.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
						{
							if (token.IsCancellationRequested)
							{
								throw new TaskCanceledException();
							}
							if (contentLength != null)
							{
								double num2 = (double)totalRead / (double)contentLength.Value;
								if (progress != null)
								{
									progress.Report(num2);
								}
							}
							await ms.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
							totalRead += (long)bytesRead;
						}
						if (progress != null)
						{
							progress.Report(1.0);
						}
						byte[] array = ms.ToArray();
						httpResponse = new HttpResponse(resp, array);
					}
				}
			}
			return httpResponse;
		}

		// Token: 0x0400000B RID: 11
		internal static readonly JsonSerializer Serializer = new JsonSerializer();

		// Token: 0x0400000C RID: 12
		internal static readonly Http Default = new Http(new HttpOptions
		{
			ApplicationName = null,
			Version = null,
			Timeout = new TimeSpan?(TimeSpan.FromSeconds(30.0)),
			HandleRateLimits = false,
			Agents = new ApplicationAgent[0]
		});
	}
}
