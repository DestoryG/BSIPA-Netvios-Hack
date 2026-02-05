using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Ionic.Zip;
using NetViosCommon.Utility;
using Newtonsoft.Json;
using PlayerDataPlugin.BSHandler;

namespace PlayerDataPlugin
{
	// Token: 0x0200000A RID: 10
	internal class Recorder
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public void Init()
		{
			string text = "./NVRecords/";
			FileUtil.CreateDir(text);
			text = Path.Combine(text, Singleton<Player>.Instance.ID);
			FileUtil.CreateDir(text);
			text = Path.Combine(text, DateTime.Now.ToString("yyyyMMdd"));
			FileUtil.CreateDir(text);
			this.mLogPath = text;
			this.mCreatedTimeStr = DateTime.Now.ToString("yyyyMMddhhmmss");
			string text2 = Path.Combine(this.mLogPath, Singleton<Player>.Instance.ID + "-" + this.mCreatedTimeStr + ".log");
			this.mFilePath = text2;
			this.mWriter = new StreamWriter(this.mFilePath);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["version"] = Recorder.VERSION;
			dictionary["sdkJson"] = Singleton<Player>.Instance.SdkData;
			string text3 = JsonConvert.SerializeObject(dictionary);
			this.Write(text3);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002CC4 File Offset: 0x00000EC4
		public void Write(string data)
		{
			this.mWriter.WriteLine(data);
			this.mWriter.Flush();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002CE0 File Offset: 0x00000EE0
		private string MakeSignForActionRecord(int timeStamp, int lastModifiedScore)
		{
			string id = Singleton<Player>.Instance.ID;
			string text = "";
			text += lastModifiedScore.ToString();
			text += timeStamp.ToString();
			text += id;
			text += "6xxx6";
			string text2 = "";
			using (MD5 md = MD5.Create())
			{
				text2 = EncryptUtil.GetMd5Hash(md, text);
			}
			return text2;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002D60 File Offset: 0x00000F60
		private string GetUploadUrl(int lastModifiedScore)
		{
			string text = "";
			int num = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			string text2 = num.ToString();
			string id = Singleton<Player>.Instance.ID;
			return text + "https://beatsaberbbs.com/api/" + "mod/v1/operation/record/" + id + "/" + lastModifiedScore.ToString() + "/" + text2 + "/" + this.MakeSignForActionRecord(num, lastModifiedScore);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002DF4 File Offset: 0x00000FF4
		public async void ZipDataAndUpload(int lastModifiedScore)
		{
			string text = Singleton<Player>.Instance.ID + "-" + this.mCreatedTimeStr + ".zip";
			string text2 = Path.Combine(this.mLogPath, text);
			using (ZipFile zipFile = new ZipFile(text2, Encoding.Default))
			{
				zipFile.AddFile(this.mFilePath, "");
				zipFile.Save();
			}
			string uploadUrl = this.GetUploadUrl(lastModifiedScore);
			FileStream fileStream = File.OpenRead(text2);
			string fileName = Path.GetFileName(text2);
			try
			{
				using (HttpClient client = new HttpClient())
				{
					using (MultipartFormDataContent content = new MultipartFormDataContent("BSMOD----" + DateTime.Now.Ticks.ToString("x")))
					{
						content.Add(new StreamContent(fileStream), "record", fileName);
						HttpResponseMessage httpResponseMessage = await client.PostAsync(uploadUrl, content);
						using (HttpResponseMessage message = httpResponseMessage)
						{
							string text3 = await message.Content.ReadAsStringAsync();
							Logger.log.Info("Upload Result: " + text3);
						}
						HttpResponseMessage message = null;
					}
					MultipartFormDataContent content = null;
				}
				HttpClient client = null;
			}
			catch (Exception ex)
			{
				Logger.log.Error("Upload Failed, Exception:" + ex.ToString());
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E33 File Offset: 0x00001033
		public void Close()
		{
			this.mWriter.Close();
		}

		// Token: 0x0400001F RID: 31
		private static int VERSION = 1;

		// Token: 0x04000020 RID: 32
		private string mCreatedTimeStr = "";

		// Token: 0x04000021 RID: 33
		private string mFilePath;

		// Token: 0x04000022 RID: 34
		private StreamWriter mWriter;

		// Token: 0x04000023 RID: 35
		private string mLogPath = "";
	}
}
