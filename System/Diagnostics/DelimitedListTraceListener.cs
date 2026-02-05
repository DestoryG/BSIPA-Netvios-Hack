using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x02000498 RID: 1176
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class DelimitedListTraceListener : TextWriterTraceListener
	{
		// Token: 0x06002B9A RID: 11162 RVA: 0x000C53C4 File Offset: 0x000C35C4
		public DelimitedListTraceListener(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000C53E3 File Offset: 0x000C35E3
		public DelimitedListTraceListener(Stream stream, string name)
			: base(stream, name)
		{
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000C5403 File Offset: 0x000C3603
		public DelimitedListTraceListener(TextWriter writer)
			: base(writer)
		{
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000C5422 File Offset: 0x000C3622
		public DelimitedListTraceListener(TextWriter writer, string name)
			: base(writer, name)
		{
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x000C5442 File Offset: 0x000C3642
		public DelimitedListTraceListener(string fileName)
			: base(fileName)
		{
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000C5461 File Offset: 0x000C3661
		public DelimitedListTraceListener(string fileName, string name)
			: base(fileName, name)
		{
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x000C5484 File Offset: 0x000C3684
		// (set) Token: 0x06002BA1 RID: 11169 RVA: 0x000C54F8 File Offset: 0x000C36F8
		public string Delimiter
		{
			get
			{
				lock (this)
				{
					if (!this.initializedDelim)
					{
						if (base.Attributes.ContainsKey("delimiter"))
						{
							this.delimiter = base.Attributes["delimiter"];
						}
						this.initializedDelim = true;
					}
				}
				return this.delimiter;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Delimiter");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("Generic_ArgCantBeEmptyString", new object[] { "Delimiter" }));
				}
				lock (this)
				{
					this.delimiter = value;
					this.initializedDelim = true;
				}
				if (this.delimiter == ",")
				{
					this.secondaryDelim = ";";
					return;
				}
				this.secondaryDelim = ",";
			}
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000C5598 File Offset: 0x000C3798
		protected internal override string[] GetSupportedAttributes()
		{
			return new string[] { "delimiter" };
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000C55A8 File Offset: 0x000C37A8
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, format, args))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			if (args != null)
			{
				this.WriteEscaped(string.Format(CultureInfo.InvariantCulture, format, args));
			}
			else
			{
				this.WriteEscaped(format);
			}
			this.Write(this.Delimiter);
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000C5620 File Offset: 0x000C3820
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, message))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.WriteEscaped(message);
			this.Write(this.Delimiter);
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000C567C File Offset: 0x000C387C
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.Write(this.Delimiter);
			this.WriteEscaped(data.ToString());
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000C56E0 File Offset: 0x000C38E0
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id);
			this.Write(this.Delimiter);
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (i != 0)
					{
						this.Write(this.secondaryDelim);
					}
					this.WriteEscaped(data[i].ToString());
				}
			}
			this.Write(this.Delimiter);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000C5768 File Offset: 0x000C3968
		private void WriteHeader(string source, TraceEventType eventType, int id)
		{
			this.WriteEscaped(source);
			this.Write(this.Delimiter);
			this.Write(eventType.ToString());
			this.Write(this.Delimiter);
			this.Write(id.ToString(CultureInfo.InvariantCulture));
			this.Write(this.Delimiter);
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000C57C8 File Offset: 0x000C39C8
		private void WriteFooter(TraceEventCache eventCache)
		{
			if (eventCache != null)
			{
				if (base.IsEnabled(TraceOptions.ProcessId))
				{
					this.Write(eventCache.ProcessId.ToString(CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.LogicalOperationStack))
				{
					this.WriteStackEscaped(eventCache.LogicalOperationStack);
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.ThreadId))
				{
					this.WriteEscaped(eventCache.ThreadId.ToString(CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.DateTime))
				{
					this.WriteEscaped(eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.Timestamp))
				{
					this.Write(eventCache.Timestamp.ToString(CultureInfo.InvariantCulture));
				}
				this.Write(this.Delimiter);
				if (base.IsEnabled(TraceOptions.Callstack))
				{
					this.WriteEscaped(eventCache.Callstack);
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					this.Write(this.Delimiter);
				}
			}
			this.WriteLine("");
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000C58F4 File Offset: 0x000C3AF4
		private void WriteEscaped(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				StringBuilder stringBuilder = new StringBuilder("\"");
				int num = 0;
				int num2;
				while ((num2 = message.IndexOf('"', num)) != -1)
				{
					stringBuilder.Append(message, num, num2 - num);
					stringBuilder.Append("\"\"");
					num = num2 + 1;
				}
				stringBuilder.Append(message, num, message.Length - num);
				stringBuilder.Append("\"");
				this.Write(stringBuilder.ToString());
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000C596C File Offset: 0x000C3B6C
		private void WriteStackEscaped(Stack stack)
		{
			StringBuilder stringBuilder = new StringBuilder("\"");
			bool flag = true;
			foreach (object obj in stack)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				else
				{
					flag = false;
				}
				string text = obj.ToString();
				int num = 0;
				int num2;
				while ((num2 = text.IndexOf('"', num)) != -1)
				{
					stringBuilder.Append(text, num, num2 - num);
					stringBuilder.Append("\"\"");
					num = num2 + 1;
				}
				stringBuilder.Append(text, num, text.Length - num);
			}
			stringBuilder.Append("\"");
			this.Write(stringBuilder.ToString());
		}

		// Token: 0x04002681 RID: 9857
		private string delimiter = ";";

		// Token: 0x04002682 RID: 9858
		private string secondaryDelim = ",";

		// Token: 0x04002683 RID: 9859
		private bool initializedDelim;
	}
}
