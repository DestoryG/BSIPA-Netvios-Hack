using System;
using System.Globalization;
using System.Reflection;

namespace System.Net
{
	// Token: 0x02000137 RID: 311
	internal class WebRequestPrefixElement
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0003DDC0 File Offset: 0x0003BFC0
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0003DE40 File Offset: 0x0003C040
		public IWebRequestCreate Creator
		{
			get
			{
				if (this.creator == null && this.creatorType != null)
				{
					lock (this)
					{
						if (this.creator == null)
						{
							this.creator = (IWebRequestCreate)Activator.CreateInstance(this.creatorType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture);
						}
					}
				}
				return this.creator;
			}
			set
			{
				this.creator = value;
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0003DE4C File Offset: 0x0003C04C
		public WebRequestPrefixElement(string P, Type creatorType)
		{
			if (!typeof(IWebRequestCreate).IsAssignableFrom(creatorType))
			{
				throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[] { creatorType.AssemblyQualifiedName, "IWebRequestCreate" }));
			}
			this.Prefix = P;
			this.creatorType = creatorType;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0003DEA6 File Offset: 0x0003C0A6
		public WebRequestPrefixElement(string P, IWebRequestCreate C)
		{
			this.Prefix = P;
			this.Creator = C;
		}

		// Token: 0x04001070 RID: 4208
		public string Prefix;

		// Token: 0x04001071 RID: 4209
		internal IWebRequestCreate creator;

		// Token: 0x04001072 RID: 4210
		internal Type creatorType;
	}
}
