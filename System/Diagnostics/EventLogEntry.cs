using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004CD RID: 1229
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[Serializable]
	public sealed class EventLogEntry : Component, ISerializable
	{
		// Token: 0x06002E4A RID: 11850 RVA: 0x000D0E4D File Offset: 0x000CF04D
		internal EventLogEntry(byte[] buf, int offset, EventLogInternal log)
		{
			this.dataBuf = buf;
			this.bufOffset = offset;
			this.owner = log;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000D0E70 File Offset: 0x000CF070
		private EventLogEntry(SerializationInfo info, StreamingContext context)
		{
			this.dataBuf = (byte[])info.GetValue("DataBuffer", typeof(byte[]));
			string @string = info.GetString("LogName");
			string string2 = info.GetString("MachineName");
			this.owner = new EventLogInternal(@string, string2, "");
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06002E4C RID: 11852 RVA: 0x000D0ED4 File Offset: 0x000CF0D4
		[MonitoringDescription("LogEntryMachineName")]
		public string MachineName
		{
			get
			{
				int num = this.bufOffset + 56;
				while (this.CharFrom(this.dataBuf, num) != '\0')
				{
					num += 2;
				}
				num += 2;
				char c = this.CharFrom(this.dataBuf, num);
				StringBuilder stringBuilder = new StringBuilder();
				while (c != '\0')
				{
					stringBuilder.Append(c);
					num += 2;
					c = this.CharFrom(this.dataBuf, num);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06002E4D RID: 11853 RVA: 0x000D0F40 File Offset: 0x000CF140
		[MonitoringDescription("LogEntryData")]
		public byte[] Data
		{
			get
			{
				int num = this.IntFrom(this.dataBuf, this.bufOffset + 48);
				byte[] array = new byte[num];
				Array.Copy(this.dataBuf, this.bufOffset + this.IntFrom(this.dataBuf, this.bufOffset + 52), array, 0, num);
				return array;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06002E4E RID: 11854 RVA: 0x000D0F95 File Offset: 0x000CF195
		[MonitoringDescription("LogEntryIndex")]
		public int Index
		{
			get
			{
				return this.IntFrom(this.dataBuf, this.bufOffset + 8);
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x000D0FAC File Offset: 0x000CF1AC
		[MonitoringDescription("LogEntryCategory")]
		public string Category
		{
			get
			{
				if (this.category == null)
				{
					string messageLibraryNames = this.GetMessageLibraryNames("CategoryMessageFile");
					string text = this.owner.FormatMessageWrapper(messageLibraryNames, (uint)this.CategoryNumber, null);
					if (text == null)
					{
						this.category = "(" + this.CategoryNumber.ToString(CultureInfo.CurrentCulture) + ")";
					}
					else
					{
						this.category = text;
					}
				}
				return this.category;
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x000D101B File Offset: 0x000CF21B
		[MonitoringDescription("LogEntryCategoryNumber")]
		public short CategoryNumber
		{
			get
			{
				return this.ShortFrom(this.dataBuf, this.bufOffset + 28);
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x000D1032 File Offset: 0x000CF232
		[MonitoringDescription("LogEntryEventID")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.EventLogEntry.InstanceId instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public int EventID
		{
			get
			{
				return this.IntFrom(this.dataBuf, this.bufOffset + 20) & 1073741823;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06002E52 RID: 11858 RVA: 0x000D104F File Offset: 0x000CF24F
		[MonitoringDescription("LogEntryEntryType")]
		public EventLogEntryType EntryType
		{
			get
			{
				return (EventLogEntryType)this.ShortFrom(this.dataBuf, this.bufOffset + 24);
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x000D1068 File Offset: 0x000CF268
		[MonitoringDescription("LogEntryMessage")]
		[Editor("System.ComponentModel.Design.BinaryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Message
		{
			get
			{
				if (this.message == null)
				{
					string messageLibraryNames = this.GetMessageLibraryNames("EventMessageFile");
					int num = this.IntFrom(this.dataBuf, this.bufOffset + 20);
					string text = this.owner.FormatMessageWrapper(messageLibraryNames, (uint)num, this.ReplacementStrings);
					if (text == null)
					{
						StringBuilder stringBuilder = new StringBuilder(SR.GetString("MessageNotFormatted", new object[] { num, this.Source }));
						string[] replacementStrings = this.ReplacementStrings;
						for (int i = 0; i < replacementStrings.Length; i++)
						{
							if (i != 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append("'");
							stringBuilder.Append(replacementStrings[i]);
							stringBuilder.Append("'");
						}
						text = stringBuilder.ToString();
					}
					else
					{
						text = this.ReplaceMessageParameters(text, this.ReplacementStrings);
					}
					this.message = text;
				}
				return this.message;
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06002E54 RID: 11860 RVA: 0x000D1158 File Offset: 0x000CF358
		[MonitoringDescription("LogEntrySource")]
		public string Source
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = this.bufOffset + 56;
				for (char c = this.CharFrom(this.dataBuf, num); c != '\0'; c = this.CharFrom(this.dataBuf, num))
				{
					stringBuilder.Append(c);
					num += 2;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06002E55 RID: 11861 RVA: 0x000D11A8 File Offset: 0x000CF3A8
		[MonitoringDescription("LogEntryReplacementStrings")]
		public string[] ReplacementStrings
		{
			get
			{
				string[] array = new string[(int)this.ShortFrom(this.dataBuf, this.bufOffset + 26)];
				int i = 0;
				int num = this.bufOffset + this.IntFrom(this.dataBuf, this.bufOffset + 36);
				StringBuilder stringBuilder = new StringBuilder();
				while (i < array.Length)
				{
					char c = this.CharFrom(this.dataBuf, num);
					if (c != '\0')
					{
						stringBuilder.Append(c);
					}
					else
					{
						array[i] = stringBuilder.ToString();
						i++;
						stringBuilder = new StringBuilder();
					}
					num += 2;
				}
				return array;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x000D1233 File Offset: 0x000CF433
		[MonitoringDescription("LogEntryResourceId")]
		[ComVisible(false)]
		public long InstanceId
		{
			get
			{
				return (long)((ulong)this.IntFrom(this.dataBuf, this.bufOffset + 20));
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06002E57 RID: 11863 RVA: 0x000D124C File Offset: 0x000CF44C
		[MonitoringDescription("LogEntryTimeGenerated")]
		public DateTime TimeGenerated
		{
			get
			{
				return EventLogEntry.beginningOfTime.AddSeconds((double)this.IntFrom(this.dataBuf, this.bufOffset + 12)).ToLocalTime();
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x000D1284 File Offset: 0x000CF484
		[MonitoringDescription("LogEntryTimeWritten")]
		public DateTime TimeWritten
		{
			get
			{
				return EventLogEntry.beginningOfTime.AddSeconds((double)this.IntFrom(this.dataBuf, this.bufOffset + 16)).ToLocalTime();
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06002E59 RID: 11865 RVA: 0x000D12BC File Offset: 0x000CF4BC
		[MonitoringDescription("LogEntryUserName")]
		public string UserName
		{
			get
			{
				int num = this.IntFrom(this.dataBuf, this.bufOffset + 40);
				if (num == 0)
				{
					return null;
				}
				byte[] array = new byte[num];
				Array.Copy(this.dataBuf, this.bufOffset + this.IntFrom(this.dataBuf, this.bufOffset + 44), array, 0, array.Length);
				int num2 = 256;
				int num3 = 256;
				int num4 = 0;
				StringBuilder stringBuilder = new StringBuilder(num2);
				StringBuilder stringBuilder2 = new StringBuilder(num3);
				StringBuilder stringBuilder3 = new StringBuilder();
				if (Microsoft.Win32.UnsafeNativeMethods.LookupAccountSid(this.MachineName, array, stringBuilder, ref num2, stringBuilder2, ref num3, ref num4) != 0)
				{
					stringBuilder3.Append(stringBuilder2.ToString());
					stringBuilder3.Append("\\");
					stringBuilder3.Append(stringBuilder.ToString());
				}
				return stringBuilder3.ToString();
			}
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000D1387 File Offset: 0x000CF587
		private char CharFrom(byte[] buf, int offset)
		{
			return (char)this.ShortFrom(buf, offset);
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000D1394 File Offset: 0x000CF594
		public bool Equals(EventLogEntry otherEntry)
		{
			if (otherEntry == null)
			{
				return false;
			}
			int num = this.IntFrom(this.dataBuf, this.bufOffset);
			int num2 = this.IntFrom(otherEntry.dataBuf, otherEntry.bufOffset);
			if (num != num2)
			{
				return false;
			}
			int num3 = this.bufOffset;
			int num4 = this.bufOffset + num;
			int num5 = otherEntry.bufOffset;
			int i = num3;
			while (i < num4)
			{
				if (this.dataBuf[i] != otherEntry.dataBuf[num5])
				{
					return false;
				}
				i++;
				num5++;
			}
			return true;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000D1417 File Offset: 0x000CF617
		private int IntFrom(byte[] buf, int offset)
		{
			return (-16777216 & ((int)buf[offset + 3] << 24)) | (16711680 & ((int)buf[offset + 2] << 16)) | (65280 & ((int)buf[offset + 1] << 8)) | (int)(byte.MaxValue & buf[offset]);
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000D1450 File Offset: 0x000CF650
		internal string ReplaceMessageParameters(string msg, string[] insertionStrings)
		{
			int i = msg.IndexOf('%');
			if (i < 0)
			{
				return msg;
			}
			int num = 0;
			int length = msg.Length;
			StringBuilder stringBuilder = new StringBuilder();
			string messageLibraryNames = this.GetMessageLibraryNames("ParameterMessageFile");
			while (i >= 0)
			{
				string text = null;
				int num2 = i + 1;
				while (num2 < length && char.IsDigit(msg, num2))
				{
					num2++;
				}
				uint num3 = 0U;
				if (num2 != i + 1)
				{
					uint.TryParse(msg.Substring(i + 1, num2 - i - 1), out num3);
				}
				if (num3 != 0U)
				{
					text = this.owner.FormatMessageWrapper(messageLibraryNames, num3, insertionStrings);
				}
				if (text != null)
				{
					if (i > num)
					{
						stringBuilder.Append(msg, num, i - num);
					}
					stringBuilder.Append(text);
					num = num2;
				}
				i = msg.IndexOf('%', i + 1);
			}
			if (length - num > 0)
			{
				stringBuilder.Append(msg, num, length - num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000D1530 File Offset: 0x000CF730
		private static RegistryKey GetSourceRegKey(string logName, string source, string machineName)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryKey registryKey3;
			try
			{
				registryKey = EventLog.GetEventLogRegKey(machineName, false);
				if (registryKey == null)
				{
					registryKey3 = null;
				}
				else
				{
					if (logName == null)
					{
						registryKey2 = registryKey.OpenSubKey("Application", false);
					}
					else
					{
						registryKey2 = registryKey.OpenSubKey(logName, false);
					}
					if (registryKey2 == null)
					{
						registryKey3 = null;
					}
					else
					{
						registryKey3 = registryKey2.OpenSubKey(source, false);
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
			}
			return registryKey3;
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000D15A4 File Offset: 0x000CF7A4
		private string GetMessageLibraryNames(string libRegKey)
		{
			string text = null;
			RegistryKey registryKey = null;
			try
			{
				registryKey = EventLogEntry.GetSourceRegKey(this.owner.Log, this.Source, this.owner.MachineName);
				if (registryKey != null)
				{
					text = (string)registryKey.GetValue(libRegKey);
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			if (text == null)
			{
				return null;
			}
			if (!(this.owner.MachineName != "."))
			{
				return text;
			}
			string[] array = text.Split(new char[] { ';' });
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length >= 2 && array[i][1] == ':')
				{
					stringBuilder.Append("\\\\");
					stringBuilder.Append(this.owner.MachineName);
					stringBuilder.Append("\\");
					stringBuilder.Append(array[i][0]);
					stringBuilder.Append("$");
					stringBuilder.Append(array[i], 2, array[i].Length - 2);
					stringBuilder.Append(';');
				}
			}
			if (stringBuilder.Length == 0)
			{
				return null;
			}
			return stringBuilder.ToString(0, stringBuilder.Length - 1);
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000D16F0 File Offset: 0x000CF8F0
		private short ShortFrom(byte[] buf, int offset)
		{
			return (short)((65280 & ((int)buf[offset + 1] << 8)) | (int)(byte.MaxValue & buf[offset]));
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000D170C File Offset: 0x000CF90C
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			int num = this.IntFrom(this.dataBuf, this.bufOffset);
			byte[] array = new byte[num];
			Array.Copy(this.dataBuf, this.bufOffset, array, 0, num);
			info.AddValue("DataBuffer", array, typeof(byte[]));
			info.AddValue("LogName", this.owner.Log);
			info.AddValue("MachineName", this.owner.MachineName);
		}

		// Token: 0x0400275A RID: 10074
		internal byte[] dataBuf;

		// Token: 0x0400275B RID: 10075
		internal int bufOffset;

		// Token: 0x0400275C RID: 10076
		private EventLogInternal owner;

		// Token: 0x0400275D RID: 10077
		private string category;

		// Token: 0x0400275E RID: 10078
		private string message;

		// Token: 0x0400275F RID: 10079
		private static readonly DateTime beginningOfTime = new DateTime(1970, 1, 1, 0, 0, 0);

		// Token: 0x04002760 RID: 10080
		private const int OFFSETFIXUP = 56;

		// Token: 0x0200087E RID: 2174
		private static class FieldOffsets
		{
			// Token: 0x04003725 RID: 14117
			internal const int LENGTH = 0;

			// Token: 0x04003726 RID: 14118
			internal const int RESERVED = 4;

			// Token: 0x04003727 RID: 14119
			internal const int RECORDNUMBER = 8;

			// Token: 0x04003728 RID: 14120
			internal const int TIMEGENERATED = 12;

			// Token: 0x04003729 RID: 14121
			internal const int TIMEWRITTEN = 16;

			// Token: 0x0400372A RID: 14122
			internal const int EVENTID = 20;

			// Token: 0x0400372B RID: 14123
			internal const int EVENTTYPE = 24;

			// Token: 0x0400372C RID: 14124
			internal const int NUMSTRINGS = 26;

			// Token: 0x0400372D RID: 14125
			internal const int EVENTCATEGORY = 28;

			// Token: 0x0400372E RID: 14126
			internal const int RESERVEDFLAGS = 30;

			// Token: 0x0400372F RID: 14127
			internal const int CLOSINGRECORDNUMBER = 32;

			// Token: 0x04003730 RID: 14128
			internal const int STRINGOFFSET = 36;

			// Token: 0x04003731 RID: 14129
			internal const int USERSIDLENGTH = 40;

			// Token: 0x04003732 RID: 14130
			internal const int USERSIDOFFSET = 44;

			// Token: 0x04003733 RID: 14131
			internal const int DATALENGTH = 48;

			// Token: 0x04003734 RID: 14132
			internal const int DATAOFFSET = 52;

			// Token: 0x04003735 RID: 14133
			internal const int RAWDATA = 56;
		}
	}
}
