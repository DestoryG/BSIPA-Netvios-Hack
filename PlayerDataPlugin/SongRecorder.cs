using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PlayerDataPlugin.BSHandler;

namespace PlayerDataPlugin
{
	// Token: 0x0200000B RID: 11
	internal class SongRecorder : Singleton<SongRecorder>
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00002E68 File Offset: 0x00001068
		public SongRecorder Init()
		{
			if (!Directory.Exists("./SongRecords/"))
			{
				Directory.CreateDirectory("./SongRecords/");
			}
			string text = Path.Combine("./SongRecords/", "songs-" + DateTime.Now.ToString("yyyyMMdd") + ".log");
			this.mWriter = File.AppendText(text);
			return this;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002EC8 File Offset: 0x000010C8
		public SongRecorder Write(Dictionary<string, object> saveData)
		{
			string text = JsonConvert.SerializeObject(saveData);
			this.mWriter.WriteLine(text);
			this.mWriter.Flush();
			return this;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002EF4 File Offset: 0x000010F4
		public void Close()
		{
			this.mWriter.Close();
		}

		// Token: 0x04000024 RID: 36
		private StreamWriter mWriter;

		// Token: 0x04000025 RID: 37
		private const string songRecordPath = "./SongRecords/";

		// Token: 0x04000026 RID: 38
		private const string songRecordFileName = "songs";
	}
}
