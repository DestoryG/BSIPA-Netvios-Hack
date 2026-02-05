using System;

namespace System.Diagnostics
{
	// Token: 0x020004D6 RID: 1238
	public class EventSourceCreationData
	{
		// Token: 0x06002E9F RID: 11935 RVA: 0x000D1EEA File Offset: 0x000D00EA
		private EventSourceCreationData()
		{
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000D1F08 File Offset: 0x000D0108
		public EventSourceCreationData(string source, string logName)
		{
			this._source = source;
			this._logName = logName;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000D1F34 File Offset: 0x000D0134
		internal EventSourceCreationData(string source, string logName, string machineName)
		{
			this._source = source;
			this._logName = logName;
			this._machineName = machineName;
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000D1F68 File Offset: 0x000D0168
		private EventSourceCreationData(string source, string logName, string machineName, string messageResourceFile, string parameterResourceFile, string categoryResourceFile, short categoryCount)
		{
			this._source = source;
			this._logName = logName;
			this._machineName = machineName;
			this._messageResourceFile = messageResourceFile;
			this._parameterResourceFile = parameterResourceFile;
			this._categoryResourceFile = categoryResourceFile;
			this.CategoryCount = (int)categoryCount;
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x000D1FC6 File Offset: 0x000D01C6
		// (set) Token: 0x06002EA4 RID: 11940 RVA: 0x000D1FCE File Offset: 0x000D01CE
		public string LogName
		{
			get
			{
				return this._logName;
			}
			set
			{
				this._logName = value;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x000D1FD7 File Offset: 0x000D01D7
		// (set) Token: 0x06002EA6 RID: 11942 RVA: 0x000D1FDF File Offset: 0x000D01DF
		public string MachineName
		{
			get
			{
				return this._machineName;
			}
			set
			{
				this._machineName = value;
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06002EA7 RID: 11943 RVA: 0x000D1FE8 File Offset: 0x000D01E8
		// (set) Token: 0x06002EA8 RID: 11944 RVA: 0x000D1FF0 File Offset: 0x000D01F0
		public string Source
		{
			get
			{
				return this._source;
			}
			set
			{
				this._source = value;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06002EA9 RID: 11945 RVA: 0x000D1FF9 File Offset: 0x000D01F9
		// (set) Token: 0x06002EAA RID: 11946 RVA: 0x000D2001 File Offset: 0x000D0201
		public string MessageResourceFile
		{
			get
			{
				return this._messageResourceFile;
			}
			set
			{
				this._messageResourceFile = value;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06002EAB RID: 11947 RVA: 0x000D200A File Offset: 0x000D020A
		// (set) Token: 0x06002EAC RID: 11948 RVA: 0x000D2012 File Offset: 0x000D0212
		public string ParameterResourceFile
		{
			get
			{
				return this._parameterResourceFile;
			}
			set
			{
				this._parameterResourceFile = value;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x000D201B File Offset: 0x000D021B
		// (set) Token: 0x06002EAE RID: 11950 RVA: 0x000D2023 File Offset: 0x000D0223
		public string CategoryResourceFile
		{
			get
			{
				return this._categoryResourceFile;
			}
			set
			{
				this._categoryResourceFile = value;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x000D202C File Offset: 0x000D022C
		// (set) Token: 0x06002EB0 RID: 11952 RVA: 0x000D2034 File Offset: 0x000D0234
		public int CategoryCount
		{
			get
			{
				return this._categoryCount;
			}
			set
			{
				if (value > 65535 || value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._categoryCount = value;
			}
		}

		// Token: 0x04002777 RID: 10103
		private string _logName = "Application";

		// Token: 0x04002778 RID: 10104
		private string _machineName = ".";

		// Token: 0x04002779 RID: 10105
		private string _source;

		// Token: 0x0400277A RID: 10106
		private string _messageResourceFile;

		// Token: 0x0400277B RID: 10107
		private string _parameterResourceFile;

		// Token: 0x0400277C RID: 10108
		private string _categoryResourceFile;

		// Token: 0x0400277D RID: 10109
		private int _categoryCount;
	}
}
