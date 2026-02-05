using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000333 RID: 819
	public sealed class HttpListenerTimeoutsElement : ConfigurationElement
	{
		// Token: 0x06001D4B RID: 7499 RVA: 0x0008B738 File Offset: 0x00089938
		static HttpListenerTimeoutsElement()
		{
			HttpListenerTimeoutsElement.properties.Add(HttpListenerTimeoutsElement.entityBody);
			HttpListenerTimeoutsElement.properties.Add(HttpListenerTimeoutsElement.drainEntityBody);
			HttpListenerTimeoutsElement.properties.Add(HttpListenerTimeoutsElement.requestQueue);
			HttpListenerTimeoutsElement.properties.Add(HttpListenerTimeoutsElement.idleConnection);
			HttpListenerTimeoutsElement.properties.Add(HttpListenerTimeoutsElement.headerWait);
			HttpListenerTimeoutsElement.properties.Add(HttpListenerTimeoutsElement.minSendBytesPerSecond);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0008B81B File Offset: 0x00089A1B
		private static ConfigurationProperty CreateTimeSpanProperty(string name)
		{
			return new ConfigurationProperty(name, typeof(TimeSpan), TimeSpan.Zero, null, new HttpListenerTimeoutsElement.TimeSpanValidator(), ConfigurationPropertyOptions.None);
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001D4D RID: 7501 RVA: 0x0008B83E File Offset: 0x00089A3E
		[ConfigurationProperty("entityBody", DefaultValue = 0, IsRequired = false)]
		public TimeSpan EntityBody
		{
			get
			{
				return (TimeSpan)base[HttpListenerTimeoutsElement.entityBody];
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001D4E RID: 7502 RVA: 0x0008B850 File Offset: 0x00089A50
		[ConfigurationProperty("drainEntityBody", DefaultValue = 0, IsRequired = false)]
		public TimeSpan DrainEntityBody
		{
			get
			{
				return (TimeSpan)base[HttpListenerTimeoutsElement.drainEntityBody];
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x0008B862 File Offset: 0x00089A62
		[ConfigurationProperty("requestQueue", DefaultValue = 0, IsRequired = false)]
		public TimeSpan RequestQueue
		{
			get
			{
				return (TimeSpan)base[HttpListenerTimeoutsElement.requestQueue];
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001D50 RID: 7504 RVA: 0x0008B874 File Offset: 0x00089A74
		[ConfigurationProperty("idleConnection", DefaultValue = 0, IsRequired = false)]
		public TimeSpan IdleConnection
		{
			get
			{
				return (TimeSpan)base[HttpListenerTimeoutsElement.idleConnection];
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x0008B886 File Offset: 0x00089A86
		[ConfigurationProperty("headerWait", DefaultValue = 0, IsRequired = false)]
		public TimeSpan HeaderWait
		{
			get
			{
				return (TimeSpan)base[HttpListenerTimeoutsElement.headerWait];
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001D52 RID: 7506 RVA: 0x0008B898 File Offset: 0x00089A98
		[ConfigurationProperty("minSendBytesPerSecond", DefaultValue = 0L, IsRequired = false)]
		public long MinSendBytesPerSecond
		{
			get
			{
				return (long)base[HttpListenerTimeoutsElement.minSendBytesPerSecond];
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x0008B8AA File Offset: 0x00089AAA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpListenerTimeoutsElement.properties;
			}
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0008B8B4 File Offset: 0x00089AB4
		internal long[] GetTimeouts()
		{
			return new long[]
			{
				Convert.ToInt64(this.EntityBody.TotalSeconds),
				Convert.ToInt64(this.DrainEntityBody.TotalSeconds),
				Convert.ToInt64(this.RequestQueue.TotalSeconds),
				Convert.ToInt64(this.IdleConnection.TotalSeconds),
				Convert.ToInt64(this.HeaderWait.TotalSeconds),
				this.MinSendBytesPerSecond
			};
		}

		// Token: 0x04001C31 RID: 7217
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C32 RID: 7218
		private static readonly ConfigurationProperty entityBody = HttpListenerTimeoutsElement.CreateTimeSpanProperty("entityBody");

		// Token: 0x04001C33 RID: 7219
		private static readonly ConfigurationProperty drainEntityBody = HttpListenerTimeoutsElement.CreateTimeSpanProperty("drainEntityBody");

		// Token: 0x04001C34 RID: 7220
		private static readonly ConfigurationProperty requestQueue = HttpListenerTimeoutsElement.CreateTimeSpanProperty("requestQueue");

		// Token: 0x04001C35 RID: 7221
		private static readonly ConfigurationProperty idleConnection = HttpListenerTimeoutsElement.CreateTimeSpanProperty("idleConnection");

		// Token: 0x04001C36 RID: 7222
		private static readonly ConfigurationProperty headerWait = HttpListenerTimeoutsElement.CreateTimeSpanProperty("headerWait");

		// Token: 0x04001C37 RID: 7223
		private static readonly ConfigurationProperty minSendBytesPerSecond = new ConfigurationProperty("minSendBytesPerSecond", typeof(long), 0L, null, new HttpListenerTimeoutsElement.LongValidator(), ConfigurationPropertyOptions.None);

		// Token: 0x020007C0 RID: 1984
		private class TimeSpanValidator : ConfigurationValidatorBase
		{
			// Token: 0x0600437D RID: 17277 RVA: 0x0011CAD4 File Offset: 0x0011ACD4
			public override bool CanValidate(Type type)
			{
				return type == typeof(TimeSpan);
			}

			// Token: 0x0600437E RID: 17278 RVA: 0x0011CAE8 File Offset: 0x0011ACE8
			public override void Validate(object value)
			{
				TimeSpan timeSpan = (TimeSpan)value;
				long num = Convert.ToInt64(timeSpan.TotalSeconds);
				if (num < 0L || num > 65535L)
				{
					throw new ArgumentOutOfRangeException("value", timeSpan, SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", new object[] { "0:0:0", "18:12:15" }));
				}
			}
		}

		// Token: 0x020007C1 RID: 1985
		private class LongValidator : ConfigurationValidatorBase
		{
			// Token: 0x06004380 RID: 17280 RVA: 0x0011CB50 File Offset: 0x0011AD50
			public override bool CanValidate(Type type)
			{
				return type == typeof(long);
			}

			// Token: 0x06004381 RID: 17281 RVA: 0x0011CB64 File Offset: 0x0011AD64
			public override void Validate(object value)
			{
				long num = (long)value;
				if (num < 0L || num > (long)((ulong)(-1)))
				{
					throw new ArgumentOutOfRangeException("value", num, SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", new object[] { 0, uint.MaxValue }));
				}
			}
		}
	}
}
