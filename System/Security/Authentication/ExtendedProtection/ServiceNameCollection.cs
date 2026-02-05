using System;
using System.Collections;
using System.Globalization;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000446 RID: 1094
	[Serializable]
	public class ServiceNameCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06002890 RID: 10384 RVA: 0x000BA2D4 File Offset: 0x000B84D4
		public ServiceNameCollection(ICollection items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			foreach (object obj in items)
			{
				string text = (string)obj;
				ServiceNameCollection.AddIfNew(base.InnerList, text);
			}
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000BA344 File Offset: 0x000B8544
		public ServiceNameCollection Merge(string serviceName)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(base.InnerList);
			ServiceNameCollection.AddIfNew(arrayList, serviceName);
			return new ServiceNameCollection(arrayList);
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000BA374 File Offset: 0x000B8574
		public ServiceNameCollection Merge(IEnumerable serviceNames)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(base.InnerList);
			foreach (object obj in serviceNames)
			{
				ServiceNameCollection.AddIfNew(arrayList, obj as string);
			}
			return new ServiceNameCollection(arrayList);
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000BA3E8 File Offset: 0x000B85E8
		private static void AddIfNew(ArrayList newServiceNames, string serviceName)
		{
			if (string.IsNullOrEmpty(serviceName))
			{
				throw new ArgumentException(SR.GetString("security_ServiceNameCollection_EmptyServiceName"));
			}
			serviceName = ServiceNameCollection.NormalizeServiceName(serviceName);
			if (!ServiceNameCollection.Contains(serviceName, newServiceNames))
			{
				newServiceNames.Add(serviceName);
			}
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000BA41C File Offset: 0x000B861C
		internal static bool Contains(string searchServiceName, ICollection serviceNames)
		{
			foreach (object obj in serviceNames)
			{
				string text = (string)obj;
				if (ServiceNameCollection.Match(text, searchServiceName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000BA47C File Offset: 0x000B867C
		public bool Contains(string searchServiceName)
		{
			string text = ServiceNameCollection.NormalizeServiceName(searchServiceName);
			return ServiceNameCollection.Contains(text, base.InnerList);
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000BA49C File Offset: 0x000B869C
		internal static string NormalizeServiceName(string inputServiceName)
		{
			if (string.IsNullOrWhiteSpace(inputServiceName))
			{
				return inputServiceName;
			}
			int num = inputServiceName.IndexOf('/');
			if (num < 0)
			{
				return inputServiceName;
			}
			string text = inputServiceName.Substring(0, num + 1);
			string text2 = inputServiceName.Substring(num + 1);
			if (string.IsNullOrWhiteSpace(text2))
			{
				return inputServiceName;
			}
			string text3 = text2;
			string text4 = string.Empty;
			string text5 = string.Empty;
			UriHostNameType uriHostNameType = Uri.CheckHostName(text2);
			if (uriHostNameType == UriHostNameType.Unknown)
			{
				string text6 = text2;
				int num2 = text2.IndexOf('/');
				if (num2 >= 0)
				{
					text6 = text2.Substring(0, num2);
					text5 = text2.Substring(num2);
					text3 = text6;
				}
				int num3 = text6.LastIndexOf(':');
				if (num3 >= 0)
				{
					text3 = text6.Substring(0, num3);
					text4 = text6.Substring(num3 + 1);
					ushort num4;
					if (!ushort.TryParse(text4, NumberStyles.Integer, CultureInfo.InvariantCulture, out num4))
					{
						return inputServiceName;
					}
					text4 = text6.Substring(num3);
				}
				uriHostNameType = Uri.CheckHostName(text3);
			}
			if (uriHostNameType != UriHostNameType.Dns)
			{
				return inputServiceName;
			}
			Uri uri;
			if (!Uri.TryCreate(Uri.UriSchemeHttp + Uri.SchemeDelimiter + text3, UriKind.Absolute, out uri))
			{
				return inputServiceName;
			}
			string components = uri.GetComponents(UriComponents.NormalizedHost, UriFormat.SafeUnescaped);
			string text7 = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", new object[] { text, components, text4, text5 });
			if (ServiceNameCollection.Match(inputServiceName, text7))
			{
				return inputServiceName;
			}
			return text7;
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000BA5DF File Offset: 0x000B87DF
		internal static bool Match(string serviceName1, string serviceName2)
		{
			return string.Compare(serviceName1, serviceName2, StringComparison.OrdinalIgnoreCase) == 0;
		}
	}
}
