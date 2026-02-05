using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace System.Diagnostics
{
	// Token: 0x020004BB RID: 1211
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class XmlWriterTraceListener : TextWriterTraceListener
	{
		// Token: 0x06002D33 RID: 11571 RVA: 0x000CB6C0 File Offset: 0x000C98C0
		public XmlWriterTraceListener(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000CB6D4 File Offset: 0x000C98D4
		public XmlWriterTraceListener(Stream stream, string name)
			: base(stream, name)
		{
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000CB6E9 File Offset: 0x000C98E9
		public XmlWriterTraceListener(TextWriter writer)
			: base(writer)
		{
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000CB6FD File Offset: 0x000C98FD
		public XmlWriterTraceListener(TextWriter writer, string name)
			: base(writer, name)
		{
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000CB712 File Offset: 0x000C9912
		public XmlWriterTraceListener(string filename)
			: base(filename)
		{
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000CB726 File Offset: 0x000C9926
		public XmlWriterTraceListener(string filename, string name)
			: base(filename, name)
		{
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000CB73B File Offset: 0x000C993B
		public override void Write(string message)
		{
			this.WriteLine(message);
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000CB744 File Offset: 0x000C9944
		public override void WriteLine(string message)
		{
			this.TraceEvent(null, SR.GetString("TraceAsTraceSource"), TraceEventType.Information, 0, message);
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000CB75C File Offset: 0x000C995C
		public override void Fail(string message, string detailMessage)
		{
			StringBuilder stringBuilder = new StringBuilder(message);
			if (detailMessage != null)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(detailMessage);
			}
			this.TraceEvent(null, SR.GetString("TraceAsTraceSource"), TraceEventType.Error, 0, stringBuilder.ToString());
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000CB7A0 File Offset: 0x000C99A0
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, format, args))
			{
				return;
			}
			this.WriteHeader(source, eventType, id, eventCache);
			string text;
			if (args != null)
			{
				text = string.Format(CultureInfo.InvariantCulture, format, args);
			}
			else
			{
				text = format;
			}
			this.WriteEscaped(text);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000CB7FD File Offset: 0x000C99FD
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, message))
			{
				return;
			}
			this.WriteHeader(source, eventType, id, eventCache);
			this.WriteEscaped(message);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000CB838 File Offset: 0x000C9A38
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id, eventCache);
			this.InternalWrite("<TraceData>");
			if (data != null)
			{
				this.InternalWrite("<DataItem>");
				this.WriteData(data);
				this.InternalWrite("</DataItem>");
			}
			this.InternalWrite("</TraceData>");
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000CB8B0 File Offset: 0x000C9AB0
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
			{
				return;
			}
			this.WriteHeader(source, eventType, id, eventCache);
			this.InternalWrite("<TraceData>");
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					this.InternalWrite("<DataItem>");
					if (data[i] != null)
					{
						this.WriteData(data[i]);
					}
					this.InternalWrite("</DataItem>");
				}
			}
			this.InternalWrite("</TraceData>");
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000CB940 File Offset: 0x000C9B40
		private void WriteData(object data)
		{
			XPathNavigator xpathNavigator = data as XPathNavigator;
			if (xpathNavigator == null)
			{
				this.WriteEscaped(data.ToString());
				return;
			}
			if (this.strBldr == null)
			{
				this.strBldr = new StringBuilder();
				this.xmlBlobWriter = new XmlTextWriter(new StringWriter(this.strBldr, CultureInfo.CurrentCulture));
			}
			else
			{
				this.strBldr.Length = 0;
			}
			try
			{
				xpathNavigator.MoveToRoot();
				this.xmlBlobWriter.WriteNode(xpathNavigator, false);
				this.InternalWrite(this.strBldr.ToString());
			}
			catch (Exception)
			{
				this.InternalWrite(data.ToString());
			}
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000CB9E8 File Offset: 0x000C9BE8
		public override void Close()
		{
			base.Close();
			if (this.xmlBlobWriter != null)
			{
				this.xmlBlobWriter.Close();
			}
			this.xmlBlobWriter = null;
			this.strBldr = null;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000CBA14 File Offset: 0x000C9C14
		public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
		{
			if (this.shouldRespectFilterOnTraceTransfer && base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, TraceEventType.Transfer, id, message))
			{
				return;
			}
			this.WriteHeader(source, TraceEventType.Transfer, id, eventCache, relatedActivityId);
			this.WriteEscaped(message);
			this.WriteFooter(eventCache);
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000CBA68 File Offset: 0x000C9C68
		private void WriteHeader(string source, TraceEventType eventType, int id, TraceEventCache eventCache, Guid relatedActivityId)
		{
			this.WriteStartHeader(source, eventType, id, eventCache);
			this.InternalWrite("\" RelatedActivityID=\"");
			this.InternalWrite(relatedActivityId.ToString("B"));
			this.WriteEndHeader(eventCache);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000CBA9A File Offset: 0x000C9C9A
		private void WriteHeader(string source, TraceEventType eventType, int id, TraceEventCache eventCache)
		{
			this.WriteStartHeader(source, eventType, id, eventCache);
			this.WriteEndHeader(eventCache);
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000CBAB0 File Offset: 0x000C9CB0
		private void WriteStartHeader(string source, TraceEventType eventType, int id, TraceEventCache eventCache)
		{
			this.InternalWrite("<E2ETraceEvent xmlns=\"http://schemas.microsoft.com/2004/06/E2ETraceEvent\"><System xmlns=\"http://schemas.microsoft.com/2004/06/windows/eventlog/system\">");
			this.InternalWrite("<EventID>");
			uint num = (uint)id;
			this.InternalWrite(num.ToString(CultureInfo.InvariantCulture));
			this.InternalWrite("</EventID>");
			this.InternalWrite("<Type>3</Type>");
			this.InternalWrite("<SubType Name=\"");
			this.InternalWrite(eventType.ToString());
			this.InternalWrite("\">0</SubType>");
			this.InternalWrite("<Level>");
			int num2 = (int)eventType;
			if (num2 > 255)
			{
				num2 = 255;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			this.InternalWrite(num2.ToString(CultureInfo.InvariantCulture));
			this.InternalWrite("</Level>");
			this.InternalWrite("<TimeCreated SystemTime=\"");
			if (eventCache != null)
			{
				this.InternalWrite(eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
			}
			else
			{
				this.InternalWrite(DateTime.Now.ToString("o", CultureInfo.InvariantCulture));
			}
			this.InternalWrite("\" />");
			this.InternalWrite("<Source Name=\"");
			this.WriteEscaped(source);
			this.InternalWrite("\" />");
			this.InternalWrite("<Correlation ActivityID=\"");
			if (eventCache != null)
			{
				this.InternalWrite(eventCache.ActivityId.ToString("B"));
				return;
			}
			this.InternalWrite(Guid.Empty.ToString("B"));
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000CBC1C File Offset: 0x000C9E1C
		private void WriteEndHeader(TraceEventCache eventCache)
		{
			this.InternalWrite("\" />");
			this.InternalWrite("<Execution ProcessName=\"");
			this.InternalWrite(TraceEventCache.GetProcessName());
			this.InternalWrite("\" ProcessID=\"");
			this.InternalWrite(((uint)TraceEventCache.GetProcessId()).ToString(CultureInfo.InvariantCulture));
			this.InternalWrite("\" ThreadID=\"");
			if (eventCache != null)
			{
				this.WriteEscaped(eventCache.ThreadId.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				this.WriteEscaped(TraceEventCache.GetThreadId().ToString(CultureInfo.InvariantCulture));
			}
			this.InternalWrite("\" />");
			this.InternalWrite("<Channel/>");
			this.InternalWrite("<Computer>");
			this.InternalWrite(this.machineName);
			this.InternalWrite("</Computer>");
			this.InternalWrite("</System>");
			this.InternalWrite("<ApplicationData>");
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000CBCFC File Offset: 0x000C9EFC
		private void WriteFooter(TraceEventCache eventCache)
		{
			bool flag = base.IsEnabled(TraceOptions.LogicalOperationStack);
			bool flag2 = base.IsEnabled(TraceOptions.Callstack);
			if (eventCache != null && (flag || flag2))
			{
				this.InternalWrite("<System.Diagnostics xmlns=\"http://schemas.microsoft.com/2004/08/System.Diagnostics\">");
				if (flag)
				{
					this.InternalWrite("<LogicalOperationStack>");
					Stack logicalOperationStack = eventCache.LogicalOperationStack;
					if (logicalOperationStack != null)
					{
						foreach (object obj in logicalOperationStack)
						{
							this.InternalWrite("<LogicalOperation>");
							this.WriteEscaped(obj.ToString());
							this.InternalWrite("</LogicalOperation>");
						}
					}
					this.InternalWrite("</LogicalOperationStack>");
				}
				this.InternalWrite("<Timestamp>");
				this.InternalWrite(eventCache.Timestamp.ToString(CultureInfo.InvariantCulture));
				this.InternalWrite("</Timestamp>");
				if (flag2)
				{
					this.InternalWrite("<Callstack>");
					this.WriteEscaped(eventCache.Callstack);
					this.InternalWrite("</Callstack>");
				}
				this.InternalWrite("</System.Diagnostics>");
			}
			this.InternalWrite("</ApplicationData></E2ETraceEvent>");
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000CBE24 File Offset: 0x000CA024
		private void WriteEscaped(string str)
		{
			if (str == null)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c <= '"')
				{
					if (c != '\n')
					{
						if (c != '\r')
						{
							if (c == '"')
							{
								this.InternalWrite(str.Substring(num, i - num));
								this.InternalWrite("&quot;");
								num = i + 1;
							}
						}
						else
						{
							this.InternalWrite(str.Substring(num, i - num));
							this.InternalWrite("&#xD;");
							num = i + 1;
						}
					}
					else
					{
						this.InternalWrite(str.Substring(num, i - num));
						this.InternalWrite("&#xA;");
						num = i + 1;
					}
				}
				else if (c <= '\'')
				{
					if (c != '&')
					{
						if (c == '\'')
						{
							this.InternalWrite(str.Substring(num, i - num));
							this.InternalWrite("&apos;");
							num = i + 1;
						}
					}
					else
					{
						this.InternalWrite(str.Substring(num, i - num));
						this.InternalWrite("&amp;");
						num = i + 1;
					}
				}
				else if (c != '<')
				{
					if (c == '>')
					{
						this.InternalWrite(str.Substring(num, i - num));
						this.InternalWrite("&gt;");
						num = i + 1;
					}
				}
				else
				{
					this.InternalWrite(str.Substring(num, i - num));
					this.InternalWrite("&lt;");
					num = i + 1;
				}
			}
			this.InternalWrite(str.Substring(num, str.Length - num));
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000CBFA1 File Offset: 0x000CA1A1
		private void InternalWrite(string message)
		{
			if (!base.EnsureWriter())
			{
				return;
			}
			this.writer.Write(message);
		}

		// Token: 0x04002705 RID: 9989
		private const string fixedHeader = "<E2ETraceEvent xmlns=\"http://schemas.microsoft.com/2004/06/E2ETraceEvent\"><System xmlns=\"http://schemas.microsoft.com/2004/06/windows/eventlog/system\">";

		// Token: 0x04002706 RID: 9990
		private readonly string machineName = Environment.MachineName;

		// Token: 0x04002707 RID: 9991
		private StringBuilder strBldr;

		// Token: 0x04002708 RID: 9992
		private XmlTextWriter xmlBlobWriter;

		// Token: 0x04002709 RID: 9993
		internal bool shouldRespectFilterOnTraceTransfer;
	}
}
