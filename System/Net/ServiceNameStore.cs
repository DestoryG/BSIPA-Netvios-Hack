using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000209 RID: 521
	internal class ServiceNameStore
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x0006638D File Offset: 0x0006458D
		public ServiceNameCollection ServiceNames
		{
			get
			{
				if (this.serviceNameCollection == null)
				{
					this.serviceNameCollection = new ServiceNameCollection(this.serviceNames);
				}
				return this.serviceNameCollection;
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000663AE File Offset: 0x000645AE
		public ServiceNameStore()
		{
			this.serviceNames = new List<string>();
			this.serviceNameCollection = null;
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x000663C8 File Offset: 0x000645C8
		private bool AddSingleServiceName(string spn)
		{
			spn = ServiceNameCollection.NormalizeServiceName(spn);
			if (this.Contains(spn))
			{
				return false;
			}
			this.serviceNames.Add(spn);
			return true;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x000663EC File Offset: 0x000645EC
		public bool Add(string uriPrefix)
		{
			string[] array = this.BuildServiceNames(uriPrefix);
			bool flag = false;
			foreach (string text in array)
			{
				if (this.AddSingleServiceName(text))
				{
					flag = true;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, "ServiceNameStore#" + ValidationHelper.HashString(this) + "::Add() " + SR.GetString("net_log_listener_spn_add", new object[] { text, uriPrefix }));
					}
				}
			}
			if (flag)
			{
				this.serviceNameCollection = null;
			}
			else if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, "ServiceNameStore#" + ValidationHelper.HashString(this) + "::Add() " + SR.GetString("net_log_listener_spn_not_add", new object[] { uriPrefix }));
			}
			return flag;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000664AC File Offset: 0x000646AC
		public bool Remove(string uriPrefix)
		{
			string text = this.BuildSimpleServiceName(uriPrefix);
			text = ServiceNameCollection.NormalizeServiceName(text);
			bool flag = this.Contains(text);
			if (flag)
			{
				this.serviceNames.Remove(text);
				this.serviceNameCollection = null;
			}
			if (Logging.On)
			{
				if (flag)
				{
					Logging.PrintInfo(Logging.HttpListener, "ServiceNameStore#" + ValidationHelper.HashString(this) + "::Remove() " + SR.GetString("net_log_listener_spn_remove", new object[] { text, uriPrefix }));
				}
				else
				{
					Logging.PrintInfo(Logging.HttpListener, "ServiceNameStore#" + ValidationHelper.HashString(this) + "::Remove() " + SR.GetString("net_log_listener_spn_not_remove", new object[] { uriPrefix }));
				}
			}
			return flag;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0006655E File Offset: 0x0006475E
		private bool Contains(string newServiceName)
		{
			return newServiceName != null && ServiceNameCollection.Contains(newServiceName, this.serviceNames);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00066571 File Offset: 0x00064771
		public void Clear()
		{
			this.serviceNames.Clear();
			this.serviceNameCollection = null;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00066588 File Offset: 0x00064788
		private string ExtractHostname(string uriPrefix, bool allowInvalidUriStrings)
		{
			if (Uri.IsWellFormedUriString(uriPrefix, UriKind.Absolute))
			{
				Uri uri = new Uri(uriPrefix);
				return uri.Host;
			}
			if (allowInvalidUriStrings)
			{
				int num = uriPrefix.IndexOf("://") + 3;
				int num2 = num;
				bool flag = false;
				while (num2 < uriPrefix.Length && uriPrefix[num2] != '/' && (uriPrefix[num2] != ':' || flag))
				{
					if (uriPrefix[num2] == '[')
					{
						if (flag)
						{
							num2 = num;
							break;
						}
						flag = true;
					}
					if (flag && uriPrefix[num2] == ']')
					{
						flag = false;
					}
					num2++;
				}
				return uriPrefix.Substring(num, num2 - num);
			}
			return null;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00066620 File Offset: 0x00064820
		public string BuildSimpleServiceName(string uriPrefix)
		{
			string text = this.ExtractHostname(uriPrefix, false);
			if (text != null)
			{
				return "HTTP/" + text;
			}
			return null;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00066648 File Offset: 0x00064848
		public string[] BuildServiceNames(string uriPrefix)
		{
			string text = this.ExtractHostname(uriPrefix, true);
			IPAddress ipaddress = null;
			if (string.Compare(text, "*", StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(text, "+", StringComparison.InvariantCultureIgnoreCase) == 0 || IPAddress.TryParse(text, out ipaddress))
			{
				try
				{
					string hostName = Dns.GetHostEntry(string.Empty).HostName;
					return new string[] { "HTTP/" + hostName };
				}
				catch (SocketException)
				{
					return new string[0];
				}
				catch (SecurityException)
				{
					return new string[0];
				}
			}
			if (!text.Contains("."))
			{
				try
				{
					string hostName2 = Dns.GetHostEntry(text).HostName;
					return new string[]
					{
						"HTTP/" + text,
						"HTTP/" + hostName2
					};
				}
				catch (SocketException)
				{
					return new string[] { "HTTP/" + text };
				}
				catch (SecurityException)
				{
					return new string[] { "HTTP/" + text };
				}
			}
			return new string[] { "HTTP/" + text };
		}

		// Token: 0x0400155C RID: 5468
		private List<string> serviceNames;

		// Token: 0x0400155D RID: 5469
		private ServiceNameCollection serviceNameCollection;
	}
}
