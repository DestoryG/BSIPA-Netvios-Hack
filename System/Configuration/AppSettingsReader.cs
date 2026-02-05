using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x020000BB RID: 187
	public class AppSettingsReader
	{
		// Token: 0x06000641 RID: 1601 RVA: 0x0002414E File Offset: 0x0002234E
		public AppSettingsReader()
		{
			this.map = ConfigurationManager.AppSettings;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00024164 File Offset: 0x00022364
		public object GetValue(string key, Type type)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string text = this.map[key];
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("AppSettingsReaderNoKey", new object[] { key }));
			}
			if (!(type == AppSettingsReader.stringType))
			{
				object obj;
				try
				{
					obj = Convert.ChangeType(text, type, CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					string text2 = ((text.Length == 0) ? "AppSettingsReaderEmptyString" : text);
					throw new InvalidOperationException(SR.GetString("AppSettingsReaderCantParse", new object[]
					{
						text2,
						key,
						type.ToString()
					}));
				}
				return obj;
			}
			int noneNesting = this.GetNoneNesting(text);
			if (noneNesting == 0)
			{
				return text;
			}
			if (noneNesting == 1)
			{
				return null;
			}
			return text.Substring(1, text.Length - 2);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00024248 File Offset: 0x00022448
		private int GetNoneNesting(string val)
		{
			int num = 0;
			int length = val.Length;
			if (length > 1)
			{
				while (val[num] == '(' && val[length - num - 1] == ')')
				{
					num++;
				}
				if (num > 0 && string.Compare(AppSettingsReader.NullString, 0, val, num, length - 2 * num, StringComparison.Ordinal) != 0)
				{
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x04000C6E RID: 3182
		private NameValueCollection map;

		// Token: 0x04000C6F RID: 3183
		private static Type stringType = typeof(string);

		// Token: 0x04000C70 RID: 3184
		private static Type[] paramsArray = new Type[] { AppSettingsReader.stringType };

		// Token: 0x04000C71 RID: 3185
		private static string NullString = "None";
	}
}
