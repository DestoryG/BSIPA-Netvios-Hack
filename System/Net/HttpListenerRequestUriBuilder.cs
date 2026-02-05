using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Configuration;
using System.Text;

namespace System.Net
{
	// Token: 0x020000FF RID: 255
	internal sealed class HttpListenerRequestUriBuilder
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x0003425C File Offset: 0x0003245C
		private HttpListenerRequestUriBuilder(string rawUri, string cookedUriScheme, string cookedUriHost, string cookedUriPath, string cookedUriQuery)
		{
			this.rawUri = rawUri;
			this.cookedUriScheme = cookedUriScheme;
			this.cookedUriHost = cookedUriHost;
			this.cookedUriPath = HttpListenerRequestUriBuilder.AddSlashToAsteriskOnlyPath(cookedUriPath);
			if (cookedUriQuery == null)
			{
				this.cookedUriQuery = string.Empty;
				return;
			}
			this.cookedUriQuery = cookedUriQuery;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000342AC File Offset: 0x000324AC
		public static Uri GetRequestUri(string rawUri, string cookedUriScheme, string cookedUriHost, string cookedUriPath, string cookedUriQuery)
		{
			HttpListenerRequestUriBuilder httpListenerRequestUriBuilder = new HttpListenerRequestUriBuilder(rawUri, cookedUriScheme, cookedUriHost, cookedUriPath, cookedUriQuery);
			return httpListenerRequestUriBuilder.Build();
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000342CC File Offset: 0x000324CC
		private Uri Build()
		{
			if (HttpListenerRequestUriBuilder.useCookedRequestUrl)
			{
				this.BuildRequestUriUsingCookedPath();
				if (this.requestUri == null)
				{
					this.BuildRequestUriUsingRawPath();
				}
			}
			else
			{
				this.BuildRequestUriUsingRawPath();
				if (this.requestUri == null)
				{
					this.BuildRequestUriUsingCookedPath();
				}
			}
			return this.requestUri;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0003431C File Offset: 0x0003251C
		private void BuildRequestUriUsingCookedPath()
		{
			if (!Uri.TryCreate(string.Concat(new string[]
			{
				this.cookedUriScheme,
				Uri.SchemeDelimiter,
				this.cookedUriHost,
				this.cookedUriPath,
				this.cookedUriQuery
			}), UriKind.Absolute, out this.requestUri))
			{
				this.LogWarning("BuildRequestUriUsingCookedPath", "net_log_listener_cant_create_uri", new object[] { this.cookedUriScheme, this.cookedUriHost, this.cookedUriPath, this.cookedUriQuery });
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000343AC File Offset: 0x000325AC
		private void BuildRequestUriUsingRawPath()
		{
			this.rawPath = HttpListenerRequestUriBuilder.GetPath(this.rawUri);
			bool flag;
			if (!HttpSysSettings.EnableNonUtf8 || this.rawPath == string.Empty)
			{
				string text = this.rawPath;
				if (text == string.Empty)
				{
					text = "/";
				}
				flag = Uri.TryCreate(string.Concat(new string[]
				{
					this.cookedUriScheme,
					Uri.SchemeDelimiter,
					this.cookedUriHost,
					text,
					this.cookedUriQuery
				}), UriKind.Absolute, out this.requestUri);
			}
			else
			{
				HttpListenerRequestUriBuilder.ParsingResult parsingResult = this.BuildRequestUriUsingRawPath(HttpListenerRequestUriBuilder.GetEncoding(HttpListenerRequestUriBuilder.EncodingType.Primary));
				if (parsingResult == HttpListenerRequestUriBuilder.ParsingResult.EncodingError)
				{
					Encoding encoding = HttpListenerRequestUriBuilder.GetEncoding(HttpListenerRequestUriBuilder.EncodingType.Secondary);
					parsingResult = this.BuildRequestUriUsingRawPath(encoding);
				}
				flag = parsingResult == HttpListenerRequestUriBuilder.ParsingResult.Success;
			}
			if (!flag)
			{
				this.LogWarning("BuildRequestUriUsingRawPath", "net_log_listener_cant_create_uri", new object[] { this.cookedUriScheme, this.cookedUriHost, this.rawPath, this.cookedUriQuery });
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000344A5 File Offset: 0x000326A5
		private static Encoding GetEncoding(HttpListenerRequestUriBuilder.EncodingType type)
		{
			if ((type == HttpListenerRequestUriBuilder.EncodingType.Primary && !HttpSysSettings.FavorUtf8) || (type == HttpListenerRequestUriBuilder.EncodingType.Secondary && HttpSysSettings.FavorUtf8))
			{
				return HttpListenerRequestUriBuilder.ansiEncoding;
			}
			return HttpListenerRequestUriBuilder.utf8Encoding;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000344C8 File Offset: 0x000326C8
		private HttpListenerRequestUriBuilder.ParsingResult BuildRequestUriUsingRawPath(Encoding encoding)
		{
			this.rawOctets = new List<byte>();
			this.requestUriString = new StringBuilder();
			this.requestUriString.Append(this.cookedUriScheme);
			this.requestUriString.Append(Uri.SchemeDelimiter);
			this.requestUriString.Append(this.cookedUriHost);
			HttpListenerRequestUriBuilder.ParsingResult parsingResult = this.ParseRawPath(encoding);
			if (parsingResult == HttpListenerRequestUriBuilder.ParsingResult.Success)
			{
				this.requestUriString.Append(this.cookedUriQuery);
				if (!Uri.TryCreate(this.requestUriString.ToString(), UriKind.Absolute, out this.requestUri))
				{
					parsingResult = HttpListenerRequestUriBuilder.ParsingResult.InvalidString;
				}
			}
			if (parsingResult != HttpListenerRequestUriBuilder.ParsingResult.Success)
			{
				this.LogWarning("BuildRequestUriUsingRawPath", "net_log_listener_cant_convert_raw_path", new object[] { this.rawPath, encoding.EncodingName });
			}
			return parsingResult;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00034584 File Offset: 0x00032784
		private HttpListenerRequestUriBuilder.ParsingResult ParseRawPath(Encoding encoding)
		{
			int i = 0;
			while (i < this.rawPath.Length)
			{
				char c = this.rawPath[i];
				if (c == '%')
				{
					i++;
					c = this.rawPath[i];
					if (c == 'u' || c == 'U')
					{
						if (!this.EmptyDecodeAndAppendRawOctetsList(encoding))
						{
							return HttpListenerRequestUriBuilder.ParsingResult.EncodingError;
						}
						if (!this.AppendUnicodeCodePointValuePercentEncoded(this.rawPath.Substring(i + 1, 4)))
						{
							return HttpListenerRequestUriBuilder.ParsingResult.InvalidString;
						}
						i += 5;
					}
					else
					{
						if (!this.AddPercentEncodedOctetToRawOctetsList(encoding, this.rawPath.Substring(i, 2)))
						{
							return HttpListenerRequestUriBuilder.ParsingResult.InvalidString;
						}
						i += 2;
					}
				}
				else
				{
					if (!this.EmptyDecodeAndAppendRawOctetsList(encoding))
					{
						return HttpListenerRequestUriBuilder.ParsingResult.EncodingError;
					}
					this.requestUriString.Append(c);
					i++;
				}
			}
			if (!this.EmptyDecodeAndAppendRawOctetsList(encoding))
			{
				return HttpListenerRequestUriBuilder.ParsingResult.EncodingError;
			}
			return HttpListenerRequestUriBuilder.ParsingResult.Success;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00034648 File Offset: 0x00032848
		private bool AppendUnicodeCodePointValuePercentEncoded(string codePoint)
		{
			int num;
			if (!int.TryParse(codePoint, NumberStyles.HexNumber, null, out num))
			{
				this.LogWarning("AppendUnicodeCodePointValuePercentEncoded", "net_log_listener_cant_convert_percent_value", new object[] { codePoint });
				return false;
			}
			string text = null;
			try
			{
				text = char.ConvertFromUtf32(num);
				HttpListenerRequestUriBuilder.AppendOctetsPercentEncoded(this.requestUriString, HttpListenerRequestUriBuilder.utf8Encoding.GetBytes(text));
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				this.LogWarning("AppendUnicodeCodePointValuePercentEncoded", "net_log_listener_cant_convert_percent_value", new object[] { codePoint });
			}
			catch (EncoderFallbackException ex)
			{
				this.LogWarning("AppendUnicodeCodePointValuePercentEncoded", "net_log_listener_cant_convert_to_utf8", new object[] { text, ex.Message });
			}
			return false;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00034708 File Offset: 0x00032908
		private bool AddPercentEncodedOctetToRawOctetsList(Encoding encoding, string escapedCharacter)
		{
			byte b;
			if (!byte.TryParse(escapedCharacter, NumberStyles.HexNumber, null, out b))
			{
				this.LogWarning("AddPercentEncodedOctetToRawOctetsList", "net_log_listener_cant_convert_percent_value", new object[] { escapedCharacter });
				return false;
			}
			this.rawOctets.Add(b);
			return true;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00034750 File Offset: 0x00032950
		private bool EmptyDecodeAndAppendRawOctetsList(Encoding encoding)
		{
			if (this.rawOctets.Count == 0)
			{
				return true;
			}
			string text = null;
			try
			{
				text = encoding.GetString(this.rawOctets.ToArray());
				if (encoding == HttpListenerRequestUriBuilder.utf8Encoding)
				{
					HttpListenerRequestUriBuilder.AppendOctetsPercentEncoded(this.requestUriString, this.rawOctets.ToArray());
				}
				else
				{
					HttpListenerRequestUriBuilder.AppendOctetsPercentEncoded(this.requestUriString, HttpListenerRequestUriBuilder.utf8Encoding.GetBytes(text));
				}
				this.rawOctets.Clear();
				return true;
			}
			catch (DecoderFallbackException ex)
			{
				this.LogWarning("EmptyDecodeAndAppendRawOctetsList", "net_log_listener_cant_convert_bytes", new object[]
				{
					HttpListenerRequestUriBuilder.GetOctetsAsString(this.rawOctets),
					ex.Message
				});
			}
			catch (EncoderFallbackException ex2)
			{
				this.LogWarning("EmptyDecodeAndAppendRawOctetsList", "net_log_listener_cant_convert_to_utf8", new object[] { text, ex2.Message });
			}
			return false;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0003483C File Offset: 0x00032A3C
		private static void AppendOctetsPercentEncoded(StringBuilder target, IEnumerable<byte> octets)
		{
			foreach (byte b in octets)
			{
				target.Append('%');
				target.Append(b.ToString("X2", CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000348A0 File Offset: 0x00032AA0
		private static string GetOctetsAsString(IEnumerable<byte> octets)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (byte b in octets)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0003491C File Offset: 0x00032B1C
		private static string GetPath(string uriString)
		{
			int num = 0;
			if (uriString[0] != '/')
			{
				int num2 = 0;
				if (uriString.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
				{
					num2 = 7;
				}
				else if (uriString.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				{
					num2 = 8;
				}
				if (num2 > 0)
				{
					num = uriString.IndexOf('/', num2);
					if (num == -1)
					{
						num = uriString.Length;
					}
				}
				else
				{
					uriString = "/" + uriString;
				}
			}
			int num3 = uriString.IndexOf('?');
			if (num3 == -1)
			{
				num3 = uriString.Length;
			}
			return HttpListenerRequestUriBuilder.AddSlashToAsteriskOnlyPath(uriString.Substring(num, num3 - num));
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000349A5 File Offset: 0x00032BA5
		private static string AddSlashToAsteriskOnlyPath(string path)
		{
			if (path.Length == 1 && path[0] == '*')
			{
				return "/*";
			}
			return path;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000349C2 File Offset: 0x00032BC2
		private void LogWarning(string methodName, string message, params object[] args)
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.HttpListener, this, methodName, SR.GetString(message, args));
			}
		}

		// Token: 0x04000E32 RID: 3634
		private static readonly bool useCookedRequestUrl = SettingsSectionInternal.Section.HttpListenerUnescapeRequestUrl;

		// Token: 0x04000E33 RID: 3635
		private static readonly Encoding utf8Encoding = new UTF8Encoding(false, true);

		// Token: 0x04000E34 RID: 3636
		private static readonly Encoding ansiEncoding = Encoding.GetEncoding(0, new EncoderExceptionFallback(), new DecoderExceptionFallback());

		// Token: 0x04000E35 RID: 3637
		private readonly string rawUri;

		// Token: 0x04000E36 RID: 3638
		private readonly string cookedUriScheme;

		// Token: 0x04000E37 RID: 3639
		private readonly string cookedUriHost;

		// Token: 0x04000E38 RID: 3640
		private readonly string cookedUriPath;

		// Token: 0x04000E39 RID: 3641
		private readonly string cookedUriQuery;

		// Token: 0x04000E3A RID: 3642
		private StringBuilder requestUriString;

		// Token: 0x04000E3B RID: 3643
		private List<byte> rawOctets;

		// Token: 0x04000E3C RID: 3644
		private string rawPath;

		// Token: 0x04000E3D RID: 3645
		private Uri requestUri;

		// Token: 0x02000704 RID: 1796
		private enum ParsingResult
		{
			// Token: 0x040030CD RID: 12493
			Success,
			// Token: 0x040030CE RID: 12494
			InvalidString,
			// Token: 0x040030CF RID: 12495
			EncodingError
		}

		// Token: 0x02000705 RID: 1797
		private enum EncodingType
		{
			// Token: 0x040030D1 RID: 12497
			Primary,
			// Token: 0x040030D2 RID: 12498
			Secondary
		}
	}
}
