using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020004B3 RID: 1203
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public abstract class TraceListener : MarshalByRefObject, IDisposable
	{
		// Token: 0x06002CB4 RID: 11444 RVA: 0x000C93CC File Offset: 0x000C75CC
		protected TraceListener()
		{
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000C93E2 File Offset: 0x000C75E2
		protected TraceListener(string name)
		{
			this.listenerName = name;
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x000C93FF File Offset: 0x000C75FF
		public StringDictionary Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new StringDictionary();
				}
				return this.attributes;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x000C941A File Offset: 0x000C761A
		// (set) Token: 0x06002CB8 RID: 11448 RVA: 0x000C9430 File Offset: 0x000C7630
		public virtual string Name
		{
			get
			{
				if (this.listenerName != null)
				{
					return this.listenerName;
				}
				return "";
			}
			set
			{
				this.listenerName = value;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06002CB9 RID: 11449 RVA: 0x000C9439 File Offset: 0x000C7639
		public virtual bool IsThreadSafe
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000C943C File Offset: 0x000C763C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000C944B File Offset: 0x000C764B
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000C944D File Offset: 0x000C764D
		public virtual void Close()
		{
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000C944F File Offset: 0x000C764F
		public virtual void Flush()
		{
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06002CBE RID: 11454 RVA: 0x000C9451 File Offset: 0x000C7651
		// (set) Token: 0x06002CBF RID: 11455 RVA: 0x000C9459 File Offset: 0x000C7659
		public int IndentLevel
		{
			get
			{
				return this.indentLevel;
			}
			set
			{
				this.indentLevel = ((value < 0) ? 0 : value);
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06002CC0 RID: 11456 RVA: 0x000C9469 File Offset: 0x000C7669
		// (set) Token: 0x06002CC1 RID: 11457 RVA: 0x000C9471 File Offset: 0x000C7671
		public int IndentSize
		{
			get
			{
				return this.indentSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("IndentSize", value, SR.GetString("TraceListenerIndentSize"));
				}
				this.indentSize = value;
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x000C9499 File Offset: 0x000C7699
		// (set) Token: 0x06002CC3 RID: 11459 RVA: 0x000C94A1 File Offset: 0x000C76A1
		[ComVisible(false)]
		public TraceFilter Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000C94AA File Offset: 0x000C76AA
		// (set) Token: 0x06002CC5 RID: 11461 RVA: 0x000C94B2 File Offset: 0x000C76B2
		protected bool NeedIndent
		{
			get
			{
				return this.needIndent;
			}
			set
			{
				this.needIndent = value;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x000C94BB File Offset: 0x000C76BB
		// (set) Token: 0x06002CC7 RID: 11463 RVA: 0x000C94C3 File Offset: 0x000C76C3
		[ComVisible(false)]
		public TraceOptions TraceOutputOptions
		{
			get
			{
				return this.traceOptions;
			}
			set
			{
				if (value >> 6 != TraceOptions.None)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.traceOptions = value;
			}
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000C94DC File Offset: 0x000C76DC
		internal void SetAttributes(Hashtable attribs)
		{
			TraceUtils.VerifyAttributes(attribs, this.GetSupportedAttributes(), this);
			this.attributes = new StringDictionary();
			this.attributes.ReplaceHashtable(attribs);
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000C9502 File Offset: 0x000C7702
		public virtual void Fail(string message)
		{
			this.Fail(message, null);
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x000C950C File Offset: 0x000C770C
		public virtual void Fail(string message, string detailMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(SR.GetString("TraceListenerFail"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			if (detailMessage != null)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(detailMessage);
			}
			this.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000C9567 File Offset: 0x000C7767
		protected internal virtual string[] GetSupportedAttributes()
		{
			return null;
		}

		// Token: 0x06002CCC RID: 11468
		public abstract void Write(string message);

		// Token: 0x06002CCD RID: 11469 RVA: 0x000C956A File Offset: 0x000C776A
		public virtual void Write(object o)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, null, null, o))
			{
				return;
			}
			if (o == null)
			{
				return;
			}
			this.Write(o.ToString());
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000C95A0 File Offset: 0x000C77A0
		public virtual void Write(string message, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, message))
			{
				return;
			}
			if (category == null)
			{
				this.Write(message);
				return;
			}
			this.Write(category + ": " + ((message == null) ? string.Empty : message));
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000C95F4 File Offset: 0x000C77F4
		public virtual void Write(object o, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, category, null, o))
			{
				return;
			}
			if (category == null)
			{
				this.Write(o);
				return;
			}
			this.Write((o == null) ? "" : o.ToString(), category);
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000C9648 File Offset: 0x000C7848
		protected virtual void WriteIndent()
		{
			this.NeedIndent = false;
			for (int i = 0; i < this.indentLevel; i++)
			{
				if (this.indentSize == 4)
				{
					this.Write("    ");
				}
				else
				{
					for (int j = 0; j < this.indentSize; j++)
					{
						this.Write(" ");
					}
				}
			}
		}

		// Token: 0x06002CD1 RID: 11473
		public abstract void WriteLine(string message);

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000C969F File Offset: 0x000C789F
		public virtual void WriteLine(object o)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, null, null, o))
			{
				return;
			}
			this.WriteLine((o == null) ? "" : o.ToString());
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000C96DC File Offset: 0x000C78DC
		public virtual void WriteLine(string message, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, message))
			{
				return;
			}
			if (category == null)
			{
				this.WriteLine(message);
				return;
			}
			this.WriteLine(category + ": " + ((message == null) ? string.Empty : message));
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000C9730 File Offset: 0x000C7930
		public virtual void WriteLine(object o, string category)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, category, null, o))
			{
				return;
			}
			this.WriteLine((o == null) ? "" : o.ToString(), category);
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000C976C File Offset: 0x000C796C
		[ComVisible(false)]
		public virtual void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			string text = string.Empty;
			if (data != null)
			{
				text = data.ToString();
			}
			this.WriteLine(text);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000C97C4 File Offset: 0x000C79C4
		[ComVisible(false)]
		public virtual void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
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
			this.WriteLine(stringBuilder.ToString());
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000C984C File Offset: 0x000C7A4C
		[ComVisible(false)]
		public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
		{
			this.TraceEvent(eventCache, source, eventType, id, string.Empty);
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000C985E File Offset: 0x000C7A5E
		[ComVisible(false)]
		public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, message))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.WriteLine(message);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000C9898 File Offset: 0x000C7A98
		[ComVisible(false)]
		public virtual void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, format, args))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			if (args != null)
			{
				this.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
			}
			else
			{
				this.WriteLine(format);
			}
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000C98F7 File Offset: 0x000C7AF7
		[ComVisible(false)]
		public virtual void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
		{
			this.TraceEvent(eventCache, source, TraceEventType.Transfer, id, message + ", relatedActivityId=" + relatedActivityId.ToString());
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000C9920 File Offset: 0x000C7B20
		private void WriteHeader(string source, TraceEventType eventType, int id)
		{
			this.Write(string.Format(CultureInfo.InvariantCulture, "{0} {1}: {2} : ", new object[]
			{
				source,
				eventType.ToString(),
				id.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000C9960 File Offset: 0x000C7B60
		private void WriteFooter(TraceEventCache eventCache)
		{
			if (eventCache == null)
			{
				return;
			}
			this.indentLevel++;
			if (this.IsEnabled(TraceOptions.ProcessId))
			{
				this.WriteLine("ProcessId=" + eventCache.ProcessId.ToString());
			}
			if (this.IsEnabled(TraceOptions.LogicalOperationStack))
			{
				this.Write("LogicalOperationStack=");
				Stack logicalOperationStack = eventCache.LogicalOperationStack;
				bool flag = true;
				foreach (object obj in logicalOperationStack)
				{
					if (!flag)
					{
						this.Write(", ");
					}
					else
					{
						flag = false;
					}
					this.Write(obj.ToString());
				}
				this.WriteLine(string.Empty);
			}
			if (this.IsEnabled(TraceOptions.ThreadId))
			{
				this.WriteLine("ThreadId=" + eventCache.ThreadId);
			}
			if (this.IsEnabled(TraceOptions.DateTime))
			{
				this.WriteLine("DateTime=" + eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
			}
			if (this.IsEnabled(TraceOptions.Timestamp))
			{
				this.WriteLine("Timestamp=" + eventCache.Timestamp.ToString());
			}
			if (this.IsEnabled(TraceOptions.Callstack))
			{
				this.WriteLine("Callstack=" + eventCache.Callstack);
			}
			this.indentLevel--;
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000C9AD4 File Offset: 0x000C7CD4
		internal bool IsEnabled(TraceOptions opts)
		{
			return (opts & this.TraceOutputOptions) > TraceOptions.None;
		}

		// Token: 0x040026E2 RID: 9954
		private int indentLevel;

		// Token: 0x040026E3 RID: 9955
		private int indentSize = 4;

		// Token: 0x040026E4 RID: 9956
		private TraceOptions traceOptions;

		// Token: 0x040026E5 RID: 9957
		private bool needIndent = true;

		// Token: 0x040026E6 RID: 9958
		private string listenerName;

		// Token: 0x040026E7 RID: 9959
		private TraceFilter filter;

		// Token: 0x040026E8 RID: 9960
		private StringDictionary attributes;

		// Token: 0x040026E9 RID: 9961
		internal string initializeData;
	}
}
