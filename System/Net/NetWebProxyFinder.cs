using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Cache;
using System.Net.Configuration;
using System.Security;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000153 RID: 339
	internal sealed class NetWebProxyFinder : BaseWebProxyFinder
	{
		// Token: 0x06000BE6 RID: 3046 RVA: 0x0004048C File Offset: 0x0003E68C
		public NetWebProxyFinder(AutoWebProxyScriptEngine engine)
			: base(engine)
		{
			this.backupCache = new SingleItemRequestCache(RequestCacheManager.IsCachingEnabled);
			this.lockObject = new object();
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x000404B0 File Offset: 0x0003E6B0
		public override bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			bool flag;
			try
			{
				proxyList = null;
				this.EnsureEngineAvailable();
				if (base.State != BaseWebProxyFinder.AutoWebProxyState.Completed)
				{
					flag = false;
				}
				else
				{
					bool flag2 = false;
					try
					{
						string text = this.scriptInstance.FindProxyForURL(destination.ToString(), destination.Host);
						proxyList = NetWebProxyFinder.ParseScriptResult(text);
						flag2 = true;
					}
					catch (Exception ex)
					{
						if (Logging.On)
						{
							Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_script_execution_error", new object[] { ex }));
						}
					}
					flag = flag2;
				}
			}
			finally
			{
				this.aborted = false;
			}
			return flag;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0004054C File Offset: 0x0003E74C
		public override void Abort()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.aborted = true;
				if (this.request != null)
				{
					ThreadPool.UnsafeQueueUserWorkItem(NetWebProxyFinder.abortWrapper, this.request);
				}
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x000405AC File Offset: 0x0003E7AC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.scriptInstance != null)
			{
				this.scriptInstance.Close();
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000405C4 File Offset: 0x0003E7C4
		private void EnsureEngineAvailable()
		{
			if (base.State == BaseWebProxyFinder.AutoWebProxyState.Uninitialized || this.engineScriptLocation == null)
			{
				if (base.Engine.AutomaticallyDetectSettings)
				{
					this.DetectScriptLocation();
					if (this.scriptLocation != null)
					{
						if (this.scriptLocation.Equals(this.engineScriptLocation))
						{
							base.State = BaseWebProxyFinder.AutoWebProxyState.Completed;
							return;
						}
						BaseWebProxyFinder.AutoWebProxyState autoWebProxyState = this.DownloadAndCompile(this.scriptLocation);
						if (autoWebProxyState == BaseWebProxyFinder.AutoWebProxyState.Completed)
						{
							base.State = BaseWebProxyFinder.AutoWebProxyState.Completed;
							this.engineScriptLocation = this.scriptLocation;
							return;
						}
					}
				}
				if (base.Engine.AutomaticConfigurationScript != null && !this.aborted)
				{
					if (base.Engine.AutomaticConfigurationScript.Equals(this.engineScriptLocation))
					{
						base.State = BaseWebProxyFinder.AutoWebProxyState.Completed;
						return;
					}
					base.State = this.DownloadAndCompile(base.Engine.AutomaticConfigurationScript);
					if (base.State == BaseWebProxyFinder.AutoWebProxyState.Completed)
					{
						this.engineScriptLocation = base.Engine.AutomaticConfigurationScript;
						return;
					}
				}
			}
			else
			{
				base.State = this.DownloadAndCompile(this.engineScriptLocation);
				if (base.State == BaseWebProxyFinder.AutoWebProxyState.Completed)
				{
					return;
				}
				if (!this.engineScriptLocation.Equals(base.Engine.AutomaticConfigurationScript) && !this.aborted)
				{
					base.State = this.DownloadAndCompile(base.Engine.AutomaticConfigurationScript);
					if (base.State == BaseWebProxyFinder.AutoWebProxyState.Completed)
					{
						this.engineScriptLocation = base.Engine.AutomaticConfigurationScript;
						return;
					}
				}
			}
			base.State = BaseWebProxyFinder.AutoWebProxyState.DiscoveryFailure;
			if (this.scriptInstance != null)
			{
				this.scriptInstance.Close();
				this.scriptInstance = null;
			}
			this.engineScriptLocation = null;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0004075C File Offset: 0x0003E95C
		private BaseWebProxyFinder.AutoWebProxyState DownloadAndCompile(Uri location)
		{
			BaseWebProxyFinder.AutoWebProxyState autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.DownloadFailure;
			WebResponse webResponse = null;
			TimerThread.Timer timer = null;
			AutoWebProxyScriptWrapper autoWebProxyScriptWrapper = null;
			ExceptionHelper.WebPermissionUnrestricted.Assert();
			try
			{
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.aborted)
					{
						throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					}
					this.request = WebRequest.Create(location);
				}
				this.request.Timeout = -1;
				this.request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);
				this.request.ConnectionGroupName = "__WebProxyScript";
				if (this.request.CacheProtocol != null)
				{
					this.request.CacheProtocol = new RequestCacheProtocol(this.backupCache, this.request.CacheProtocol.Validator);
				}
				HttpWebRequest httpWebRequest = this.request as HttpWebRequest;
				if (httpWebRequest != null)
				{
					httpWebRequest.Accept = "*/*";
					HttpWebRequest httpWebRequest2 = httpWebRequest;
					string fullName = base.GetType().FullName;
					string text = "/";
					Version version = Environment.Version;
					httpWebRequest2.UserAgent = fullName + text + ((version != null) ? version.ToString() : null);
					httpWebRequest.KeepAlive = false;
					httpWebRequest.Pipelined = false;
					httpWebRequest.InternalConnectionGroup = true;
				}
				else
				{
					FtpWebRequest ftpWebRequest = this.request as FtpWebRequest;
					if (ftpWebRequest != null)
					{
						ftpWebRequest.KeepAlive = false;
					}
				}
				this.request.Proxy = null;
				this.request.Credentials = base.Engine.Credentials;
				if (NetWebProxyFinder.timerQueue == null)
				{
					NetWebProxyFinder.timerQueue = TimerThread.GetOrCreateQueue(SettingsSectionInternal.Section.DownloadTimeout);
				}
				timer = NetWebProxyFinder.timerQueue.CreateTimer(NetWebProxyFinder.timerCallback, this.request);
				webResponse = this.request.GetResponse();
				DateTime dateTime = DateTime.MinValue;
				HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
				if (httpWebResponse != null)
				{
					dateTime = httpWebResponse.LastModified;
				}
				else
				{
					FtpWebResponse ftpWebResponse = webResponse as FtpWebResponse;
					if (ftpWebResponse != null)
					{
						dateTime = ftpWebResponse.LastModified;
					}
				}
				if (this.scriptInstance != null && dateTime != DateTime.MinValue && this.scriptInstance.LastModified == dateTime)
				{
					autoWebProxyScriptWrapper = this.scriptInstance;
					autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
				}
				else
				{
					string text2 = null;
					byte[] array = null;
					using (Stream responseStream = webResponse.GetResponseStream())
					{
						SingleItemRequestCache.ReadOnlyStream readOnlyStream = responseStream as SingleItemRequestCache.ReadOnlyStream;
						if (readOnlyStream != null)
						{
							array = readOnlyStream.Buffer;
						}
						if (this.scriptInstance != null && array != null && array == this.scriptInstance.Buffer)
						{
							this.scriptInstance.LastModified = dateTime;
							autoWebProxyScriptWrapper = this.scriptInstance;
							autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
						}
						else
						{
							using (StreamReader streamReader = new StreamReader(responseStream))
							{
								text2 = streamReader.ReadToEnd();
							}
						}
					}
					WebResponse webResponse2 = webResponse;
					webResponse = null;
					webResponse2.Close();
					timer.Cancel();
					timer = null;
					if (autoWebProxyState != BaseWebProxyFinder.AutoWebProxyState.Completed)
					{
						if (this.scriptInstance != null && text2 == this.scriptInstance.ScriptBody)
						{
							this.scriptInstance.LastModified = dateTime;
							if (array != null)
							{
								this.scriptInstance.Buffer = array;
							}
							autoWebProxyScriptWrapper = this.scriptInstance;
							autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
						}
						else
						{
							autoWebProxyScriptWrapper = new AutoWebProxyScriptWrapper();
							autoWebProxyScriptWrapper.LastModified = dateTime;
							if (autoWebProxyScriptWrapper.Compile(location, text2, array))
							{
								autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
							}
							else
							{
								autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.CompilationFailure;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_script_download_compile_error", new object[] { ex }));
				}
			}
			finally
			{
				if (timer != null)
				{
					timer.Cancel();
				}
				try
				{
					if (webResponse != null)
					{
						webResponse.Close();
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
					this.request = null;
				}
			}
			if (autoWebProxyState == BaseWebProxyFinder.AutoWebProxyState.Completed && this.scriptInstance != autoWebProxyScriptWrapper)
			{
				if (this.scriptInstance != null)
				{
					this.scriptInstance.Close();
				}
				this.scriptInstance = autoWebProxyScriptWrapper;
			}
			return autoWebProxyState;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00040B98 File Offset: 0x0003ED98
		private static IList<string> ParseScriptResult(string scriptReturn)
		{
			IList<string> list = new List<string>();
			if (scriptReturn == null)
			{
				return list;
			}
			string[] array = scriptReturn.Split(NetWebProxyFinder.splitChars);
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				string text2 = text.Trim(new char[] { ' ' });
				string text3;
				if (!text2.StartsWith("PROXY ", StringComparison.OrdinalIgnoreCase))
				{
					if (string.Compare("DIRECT", text2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						text3 = null;
						goto IL_00F6;
					}
				}
				else
				{
					text3 = text2.Substring(6).TrimStart(new char[] { ' ' });
					Uri uri = null;
					bool flag = Uri.TryCreate("http://" + text3, UriKind.Absolute, out uri);
					if (flag && uri.UserInfo.Length <= 0 && uri.HostNameType != UriHostNameType.Basic && uri.AbsolutePath.Length == 1 && text3[text3.Length - 1] != '/' && text3[text3.Length - 1] != '#' && text3[text3.Length - 1] != '?')
					{
						goto IL_00F6;
					}
				}
				IL_00FD:
				i++;
				continue;
				IL_00F6:
				list.Add(text3);
				goto IL_00FD;
			}
			return list;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00040CB4 File Offset: 0x0003EEB4
		private void DetectScriptLocation()
		{
			if (this.scriptDetectionFailed || this.scriptLocation != null)
			{
				return;
			}
			this.scriptLocation = NetWebProxyFinder.SafeDetectAutoProxyUrl(UnsafeNclNativeMethods.WinHttp.AutoDetectType.Dhcp);
			if (this.scriptLocation == null)
			{
				this.scriptLocation = NetWebProxyFinder.SafeDetectAutoProxyUrl(UnsafeNclNativeMethods.WinHttp.AutoDetectType.DnsA);
			}
			if (this.scriptLocation == null)
			{
				this.scriptDetectionFailed = true;
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00040D14 File Offset: 0x0003EF14
		private unsafe static Uri SafeDetectAutoProxyUrl(UnsafeNclNativeMethods.WinHttp.AutoDetectType discoveryMethod)
		{
			Uri uri = null;
			string text = null;
			SafeGlobalFree safeGlobalFree;
			if (!UnsafeNclNativeMethods.WinHttp.WinHttpDetectAutoProxyConfigUrl(discoveryMethod, out safeGlobalFree))
			{
				if (safeGlobalFree != null)
				{
					safeGlobalFree.SetHandleAsInvalid();
				}
			}
			else
			{
				text = new string((char*)(void*)safeGlobalFree.DangerousGetHandle());
				safeGlobalFree.Close();
			}
			if (text != null)
			{
				if (!Uri.TryCreate(text, UriKind.Absolute, out uri) && Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_autodetect_script_location_parse_error", new object[] { ValidationHelper.ToString(text) }));
				}
			}
			else if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_autodetect_failed"));
			}
			return uri;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00040DAD File Offset: 0x0003EFAD
		private static void RequestTimeoutCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			ThreadPool.UnsafeQueueUserWorkItem(NetWebProxyFinder.abortWrapper, context);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00040DBB File Offset: 0x0003EFBB
		private static void AbortWrapper(object context)
		{
			if (context != null)
			{
				((WebRequest)context).Abort();
			}
		}

		// Token: 0x04001125 RID: 4389
		private static readonly char[] splitChars = new char[] { ';' };

		// Token: 0x04001126 RID: 4390
		private static TimerThread.Queue timerQueue;

		// Token: 0x04001127 RID: 4391
		private static readonly TimerThread.Callback timerCallback = new TimerThread.Callback(NetWebProxyFinder.RequestTimeoutCallback);

		// Token: 0x04001128 RID: 4392
		private static readonly WaitCallback abortWrapper = new WaitCallback(NetWebProxyFinder.AbortWrapper);

		// Token: 0x04001129 RID: 4393
		private RequestCache backupCache;

		// Token: 0x0400112A RID: 4394
		private AutoWebProxyScriptWrapper scriptInstance;

		// Token: 0x0400112B RID: 4395
		private Uri engineScriptLocation;

		// Token: 0x0400112C RID: 4396
		private Uri scriptLocation;

		// Token: 0x0400112D RID: 4397
		private bool scriptDetectionFailed;

		// Token: 0x0400112E RID: 4398
		private object lockObject;

		// Token: 0x0400112F RID: 4399
		private volatile WebRequest request;

		// Token: 0x04001130 RID: 4400
		private volatile bool aborted;

		// Token: 0x04001131 RID: 4401
		private const int MaximumProxyStringLength = 2058;
	}
}
