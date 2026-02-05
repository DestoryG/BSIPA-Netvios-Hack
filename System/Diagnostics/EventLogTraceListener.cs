using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020004D5 RID: 1237
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public sealed class EventLogTraceListener : TraceListener
	{
		// Token: 0x06002E8F RID: 11919 RVA: 0x000D1B9F File Offset: 0x000CFD9F
		public EventLogTraceListener()
		{
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000D1BA7 File Offset: 0x000CFDA7
		public EventLogTraceListener(EventLog eventLog)
			: base((eventLog != null) ? eventLog.Source : string.Empty)
		{
			this.eventLog = eventLog;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000D1BC6 File Offset: 0x000CFDC6
		public EventLogTraceListener(string source)
		{
			this.eventLog = new EventLog();
			this.eventLog.Source = source;
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06002E92 RID: 11922 RVA: 0x000D1BE5 File Offset: 0x000CFDE5
		// (set) Token: 0x06002E93 RID: 11923 RVA: 0x000D1BED File Offset: 0x000CFDED
		public EventLog EventLog
		{
			get
			{
				return this.eventLog;
			}
			set
			{
				this.eventLog = value;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06002E94 RID: 11924 RVA: 0x000D1BF6 File Offset: 0x000CFDF6
		// (set) Token: 0x06002E95 RID: 11925 RVA: 0x000D1C26 File Offset: 0x000CFE26
		public override string Name
		{
			get
			{
				if (!this.nameSet && this.eventLog != null)
				{
					this.nameSet = true;
					base.Name = this.eventLog.Source;
				}
				return base.Name;
			}
			set
			{
				this.nameSet = true;
				base.Name = value;
			}
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000D1C36 File Offset: 0x000CFE36
		public override void Close()
		{
			if (this.eventLog != null)
			{
				this.eventLog.Close();
			}
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000D1C4C File Offset: 0x000CFE4C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.Close();
				}
				else
				{
					if (this.eventLog != null)
					{
						this.eventLog.Close();
					}
					this.eventLog = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000D1C98 File Offset: 0x000CFE98
		public override void Write(string message)
		{
			if (this.eventLog != null)
			{
				this.eventLog.WriteEntry(message);
			}
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000D1CAE File Offset: 0x000CFEAE
		public override void WriteLine(string message)
		{
			this.Write(message);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000D1CB8 File Offset: 0x000CFEB8
		[ComVisible(false)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, format, args))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
			if (args == null)
			{
				this.eventLog.WriteEvent(eventInstance, new object[] { format });
				return;
			}
			if (string.IsNullOrEmpty(format))
			{
				string[] array = new string[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					array[i] = args[i].ToString();
				}
				EventLog eventLog = this.eventLog;
				EventInstance eventInstance2 = eventInstance;
				object[] array2 = array;
				eventLog.WriteEvent(eventInstance2, array2);
				return;
			}
			this.eventLog.WriteEvent(eventInstance, new object[] { string.Format(CultureInfo.InvariantCulture, format, args) });
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000D1D6C File Offset: 0x000CFF6C
		[ComVisible(false)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, message))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
			this.eventLog.WriteEvent(eventInstance, new object[] { message });
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000D1DB8 File Offset: 0x000CFFB8
		[ComVisible(false)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, data))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
			this.eventLog.WriteEvent(eventInstance, new object[] { data });
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000D1E08 File Offset: 0x000D0008
		[ComVisible(false)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, null, data))
			{
				return;
			}
			EventInstance eventInstance = this.CreateEventInstance(severity, id);
			StringBuilder stringBuilder = new StringBuilder();
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					if (data[i] != null)
					{
						stringBuilder.Append(data[i].ToString());
					}
				}
			}
			this.eventLog.WriteEvent(eventInstance, new object[] { stringBuilder.ToString() });
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000D1E98 File Offset: 0x000D0098
		private EventInstance CreateEventInstance(TraceEventType severity, int id)
		{
			if (id > 65535)
			{
				id = 65535;
			}
			if (id < 0)
			{
				id = 0;
			}
			EventInstance eventInstance = new EventInstance((long)id, 0);
			if (severity == TraceEventType.Error || severity == TraceEventType.Critical)
			{
				eventInstance.EntryType = EventLogEntryType.Error;
			}
			else if (severity == TraceEventType.Warning)
			{
				eventInstance.EntryType = EventLogEntryType.Warning;
			}
			else
			{
				eventInstance.EntryType = EventLogEntryType.Information;
			}
			return eventInstance;
		}

		// Token: 0x04002775 RID: 10101
		private EventLog eventLog;

		// Token: 0x04002776 RID: 10102
		private bool nameSet;
	}
}
