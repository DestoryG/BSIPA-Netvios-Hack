using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BS_Utils.Utilities
{
	// Token: 0x02000004 RID: 4
	public class Config
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00003750 File Offset: 0x00001950
		public Config(string name)
		{
			this._instance = new IniFile(Path.Combine(Environment.CurrentDirectory, "UserData/" + name + ".ini"));
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000380C File Offset: 0x00001A0C
		public string GetString(string section, string name, string defaultValue = "", bool autoSave = false)
		{
			string text = this._instance.IniReadValue(section, name);
			if (text != "")
			{
				return text;
			}
			if (autoSave)
			{
				this.SetString(section, name, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003844 File Offset: 0x00001A44
		public int GetInt(string section, string name, int defaultValue = 0, bool autoSave = false)
		{
			int num;
			if (int.TryParse(this._instance.IniReadValue(section, name), NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out num))
			{
				return num;
			}
			if (autoSave)
			{
				this.SetInt(section, name, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003880 File Offset: 0x00001A80
		public float GetFloat(string section, string name, float defaultValue = 0f, bool autoSave = false)
		{
			float num;
			if (float.TryParse(this._instance.IniReadValue(section, name), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num))
			{
				return num;
			}
			if (autoSave)
			{
				this.SetFloat(section, name, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000038BD File Offset: 0x00001ABD
		public bool GetBool(string section, string name, bool defaultValue = false, bool autoSave = false)
		{
			return this.GetBool(section, name, Config.BoolSavingMode.TrueFalse, defaultValue, autoSave);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000038CC File Offset: 0x00001ACC
		public bool GetBool(string section, string name, Config.BoolSavingMode mode, bool defaultValue = false, bool autoSave = false)
		{
			string @string = this.GetString(section, name, "", false);
			try
			{
				int num = this.yesAlts.IndexOf(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@string));
				int num2 = this.noAlts.IndexOf(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@string));
				if (num != -1 && num == (int)mode)
				{
					return true;
				}
				if (num2 != -1 && num2 == (int)mode)
				{
					return false;
				}
				if (autoSave)
				{
					this.SetBool(section, name, defaultValue);
				}
			}
			catch
			{
				this.SetBool(section, name, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000396C File Offset: 0x00001B6C
		public bool HasKey(string section, string name)
		{
			return this._instance.data.Sections.ContainsSection(section) && this._instance.data[section].ContainsKey(name);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000399F File Offset: 0x00001B9F
		public void SetFloat(string section, string name, float value)
		{
			this._instance.IniWriteValue(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000039BA File Offset: 0x00001BBA
		public void SetInt(string section, string name, int value)
		{
			this._instance.IniWriteValue(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000039D5 File Offset: 0x00001BD5
		public void SetString(string section, string name, string value)
		{
			this._instance.IniWriteValue(section, name, value);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000039E5 File Offset: 0x00001BE5
		public void SetBool(string section, string name, bool value)
		{
			this.SetBool(section, name, Config.BoolSavingMode.TrueFalse, value);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000039F1 File Offset: 0x00001BF1
		public void SetBool(string section, string name, Config.BoolSavingMode mode, bool value)
		{
			this._instance.IniWriteValue(section, name, value ? this.yesAlts[(int)mode] : this.noAlts[(int)mode]);
		}

		// Token: 0x04000028 RID: 40
		private IniFile _instance;

		// Token: 0x04000029 RID: 41
		private List<string> yesAlts = new List<string> { "True", "1", "Yes", "Enabled", "On" };

		// Token: 0x0400002A RID: 42
		private List<string> noAlts = new List<string> { "False", "0", "No", "Disabled", "Off" };

		// Token: 0x02000017 RID: 23
		public enum BoolSavingMode
		{
			// Token: 0x04000052 RID: 82
			TrueFalse,
			// Token: 0x04000053 RID: 83
			OneZero,
			// Token: 0x04000054 RID: 84
			YesNo,
			// Token: 0x04000055 RID: 85
			EnabledDisabled,
			// Token: 0x04000056 RID: 86
			OnOff
		}
	}
}
