using System;
using System.Configuration.Internal;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000081 RID: 129
	[Serializable]
	public class ConfigurationException : SystemException
	{
		// Token: 0x06000511 RID: 1297 RVA: 0x000211A8 File Offset: 0x0001F3A8
		private void Init(string filename, int line)
		{
			base.HResult = -2146232062;
			this._filename = filename;
			this._line = line;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000211C3 File Offset: 0x0001F3C3
		protected ConfigurationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Init(info.GetString("filename"), info.GetInt32("line"));
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000211E9 File Offset: 0x0001F3E9
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException()
			: this(null, null, null, 0)
		{
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000211F5 File Offset: 0x0001F3F5
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message)
			: this(message, null, null, 0)
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00021201 File Offset: 0x0001F401
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner)
			: this(message, inner, null, 0)
		{
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0002120D File Offset: 0x0001F40D
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, XmlNode node)
			: this(message, null, ConfigurationException.GetUnsafeXmlNodeFilename(node), ConfigurationException.GetXmlNodeLineNumber(node))
		{
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00021223 File Offset: 0x0001F423
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner, XmlNode node)
			: this(message, inner, ConfigurationException.GetUnsafeXmlNodeFilename(node), ConfigurationException.GetXmlNodeLineNumber(node))
		{
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00021239 File Offset: 0x0001F439
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, string filename, int line)
			: this(message, null, filename, line)
		{
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00021245 File Offset: 0x0001F445
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner, string filename, int line)
			: base(message, inner)
		{
			this.Init(filename, line);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00021258 File Offset: 0x0001F458
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("filename", this._filename);
			info.AddValue("line", this._line);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00021284 File Offset: 0x0001F484
		public override string Message
		{
			get
			{
				string filename = this.Filename;
				if (!string.IsNullOrEmpty(filename))
				{
					if (this.Line != 0)
					{
						return string.Concat(new string[]
						{
							this.BareMessage,
							" (",
							filename,
							" line ",
							this.Line.ToString(CultureInfo.InvariantCulture),
							")"
						});
					}
					return this.BareMessage + " (" + filename + ")";
				}
				else
				{
					if (this.Line != 0)
					{
						return this.BareMessage + " (line " + this.Line.ToString("G", CultureInfo.InvariantCulture) + ")";
					}
					return this.BareMessage;
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00021342 File Offset: 0x0001F542
		public virtual string BareMessage
		{
			get
			{
				return base.Message;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0002134A File Offset: 0x0001F54A
		public virtual string Filename
		{
			get
			{
				return ConfigurationException.SafeFilename(this._filename);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00021357 File Offset: 0x0001F557
		public virtual int Line
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0002135F File Offset: 0x0001F55F
		[Obsolete("This class is obsolete, use System.Configuration!System.Configuration.ConfigurationErrorsException.GetFilename instead")]
		public static string GetXmlNodeFilename(XmlNode node)
		{
			return ConfigurationException.SafeFilename(ConfigurationException.GetUnsafeXmlNodeFilename(node));
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0002136C File Offset: 0x0001F56C
		[Obsolete("This class is obsolete, use System.Configuration!System.Configuration.ConfigurationErrorsException.GetLinenumber instead")]
		public static int GetXmlNodeLineNumber(XmlNode node)
		{
			IConfigErrorInfo configErrorInfo = node as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.LineNumber;
			}
			return 0;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0002138C File Offset: 0x0001F58C
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static string FullPathWithAssert(string filename)
		{
			string text = null;
			try
			{
				text = Path.GetFullPath(filename);
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000213B8 File Offset: 0x0001F5B8
		internal static string SafeFilename(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return filename;
			}
			if (filename.StartsWith("http:", StringComparison.OrdinalIgnoreCase))
			{
				return filename;
			}
			try
			{
				if (!Path.IsPathRooted(filename))
				{
					return filename;
				}
			}
			catch
			{
				return null;
			}
			try
			{
				string fullPath = Path.GetFullPath(filename);
			}
			catch (SecurityException)
			{
				try
				{
					string text = ConfigurationException.FullPathWithAssert(filename);
					filename = Path.GetFileName(text);
				}
				catch
				{
					filename = null;
				}
			}
			catch
			{
				filename = null;
			}
			return filename;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00021454 File Offset: 0x0001F654
		private static string GetUnsafeXmlNodeFilename(XmlNode node)
		{
			IConfigErrorInfo configErrorInfo = node as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.Filename;
			}
			return string.Empty;
		}

		// Token: 0x04000C1A RID: 3098
		private const string HTTP_PREFIX = "http:";

		// Token: 0x04000C1B RID: 3099
		private string _filename;

		// Token: 0x04000C1C RID: 3100
		private int _line;
	}
}
