using System;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000042 RID: 66
	internal class DomainNameHelper
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0001A93B File Offset: 0x00018B3B
		private DomainNameHelper()
		{
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001A944 File Offset: 0x00018B44
		internal static string ParseCanonicalName(string str, int start, int end, ref bool loopback)
		{
			string text = null;
			for (int i = end - 1; i >= start; i--)
			{
				if (str[i] >= 'A' && str[i] <= 'Z')
				{
					text = str.Substring(start, end - start).ToLower(CultureInfo.InvariantCulture);
					break;
				}
				if (str[i] == ':')
				{
					end = i;
				}
			}
			if (text == null)
			{
				text = str.Substring(start, end - start);
			}
			if (text == "localhost" || text == "loopback")
			{
				loopback = true;
				return "localhost";
			}
			return text;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001A9D0 File Offset: 0x00018BD0
		internal unsafe static bool IsValid(char* name, ushort pos, ref int returnedEnd, ref bool notCanonical, bool notImplicitFile)
		{
			char* ptr = name + pos;
			char* ptr2 = ptr;
			char* ptr3 = name + returnedEnd;
			while (ptr2 < ptr3)
			{
				char c = *ptr2;
				if (c > '\u007f')
				{
					return false;
				}
				if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					ptr3 = ptr2;
					break;
				}
				ptr2++;
			}
			if (ptr3 == ptr)
			{
				return false;
			}
			for (;;)
			{
				ptr2 = ptr;
				while (ptr2 < ptr3 && *ptr2 != '.')
				{
					ptr2++;
				}
				if (ptr == ptr2 || (long)(ptr2 - ptr) > 63L || !DomainNameHelper.IsASCIILetterOrDigit(*(ptr++), ref notCanonical))
				{
					break;
				}
				while (ptr < ptr2)
				{
					if (!DomainNameHelper.IsValidDomainLabelCharacter(*(ptr++), ref notCanonical))
					{
						return false;
					}
				}
				ptr++;
				if (ptr >= ptr3)
				{
					goto Block_13;
				}
			}
			return false;
			Block_13:
			returnedEnd = (int)((ushort)((long)(ptr3 - name)));
			return true;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001AA80 File Offset: 0x00018C80
		internal unsafe static bool IsValidByIri(char* name, ushort pos, ref int returnedEnd, ref bool notCanonical, bool notImplicitFile)
		{
			char* ptr = name + pos;
			char* ptr2 = ptr;
			char* ptr3 = name + returnedEnd;
			while (ptr2 < ptr3)
			{
				char c = *ptr2;
				if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					ptr3 = ptr2;
					break;
				}
				ptr2++;
			}
			if (ptr3 == ptr)
			{
				return false;
			}
			for (;;)
			{
				ptr2 = ptr;
				int num = 0;
				bool flag = false;
				while (ptr2 < ptr3 && *ptr2 != '.' && *ptr2 != '。' && *ptr2 != '．' && *ptr2 != '｡')
				{
					num++;
					if (*ptr2 > 'ÿ')
					{
						num++;
					}
					if (*ptr2 >= '\u00a0')
					{
						flag = true;
					}
					ptr2++;
				}
				if (ptr == ptr2 || (flag ? (num + 4) : num) > 63 || (*(ptr++) < '\u00a0' && !DomainNameHelper.IsASCIILetterOrDigit(*(ptr - 1), ref notCanonical)))
				{
					break;
				}
				while (ptr < ptr2)
				{
					if (*(ptr++) < '\u00a0' && !DomainNameHelper.IsValidDomainLabelCharacter(*(ptr - 1), ref notCanonical))
					{
						return false;
					}
				}
				ptr++;
				if (ptr >= ptr3)
				{
					goto Block_20;
				}
			}
			return false;
			Block_20:
			returnedEnd = (int)((ushort)((long)(ptr3 - name)));
			return true;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001AB8C File Offset: 0x00018D8C
		internal unsafe static string IdnEquivalent(string hostname)
		{
			bool flag = true;
			bool flag2 = false;
			char* ptr = hostname;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return DomainNameHelper.IdnEquivalent(ptr, 0, hostname.Length, ref flag, ref flag2);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001ABC0 File Offset: 0x00018DC0
		internal unsafe static string IdnEquivalent(char* hostname, int start, int end, ref bool allAscii, ref bool atLeastOneValidIdn)
		{
			string text = null;
			string text2 = DomainNameHelper.IdnEquivalent(hostname, start, end, ref allAscii, ref text);
			if (text2 != null)
			{
				string text3 = (allAscii ? text2 : text);
				fixed (string text4 = text3)
				{
					char* ptr = text4;
					if (ptr != null)
					{
						ptr += RuntimeHelpers.OffsetToStringData / 2;
					}
					int length = text3.Length;
					int i = 0;
					int num = 0;
					bool flag = false;
					do
					{
						bool flag2 = false;
						bool flag3 = false;
						flag = false;
						for (i = num; i < length; i++)
						{
							char c = ptr[i];
							if (!flag3)
							{
								flag3 = true;
								if (i + 3 < length && DomainNameHelper.IsIdnAce(ptr, i))
								{
									i += 4;
									flag2 = true;
									continue;
								}
							}
							if (c == '.' || c == '。' || c == '．' || c == '｡')
							{
								flag = true;
								break;
							}
						}
						if (flag2)
						{
							try
							{
								IdnMapping idnMapping = new IdnMapping();
								idnMapping.GetUnicode(new string(ptr, num, i - num));
								atLeastOneValidIdn = true;
								break;
							}
							catch (ArgumentException)
							{
							}
						}
						num = i + (flag ? 1 : 0);
					}
					while (num < length);
				}
			}
			else
			{
				atLeastOneValidIdn = false;
			}
			return text2;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001ACDC File Offset: 0x00018EDC
		internal unsafe static string IdnEquivalent(char* hostname, int start, int end, ref bool allAscii, ref string bidiStrippedHost)
		{
			string text = null;
			if (end <= start)
			{
				return text;
			}
			int i = start;
			allAscii = true;
			while (i < end)
			{
				if (hostname[i] > '\u007f')
				{
					allAscii = false;
					break;
				}
				i++;
			}
			if (!allAscii)
			{
				IdnMapping idnMapping = new IdnMapping();
				bidiStrippedHost = Uri.StripBidiControlCharacter(hostname, start, end - start);
				string ascii;
				try
				{
					ascii = idnMapping.GetAscii(bidiStrippedHost);
					if (!ServicePointManager.AllowDangerousUnicodeDecompositions && DomainNameHelper.ContainsCharactersUnsafeForNormalizedHost(ascii))
					{
						throw new UriFormatException("net_uri_BadUnicodeHostForIdn");
					}
				}
				catch (ArgumentException)
				{
					throw new UriFormatException(SR.GetString("net_uri_BadUnicodeHostForIdn"));
				}
				return ascii;
			}
			string text2 = new string(hostname, start, end - start);
			if (text2 == null)
			{
				return null;
			}
			return text2.ToLowerInvariant();
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001AD8C File Offset: 0x00018F8C
		private static bool IsIdnAce(string input, int index)
		{
			return input[index] == 'x' && input[index + 1] == 'n' && input[index + 2] == '-' && input[index + 3] == '-';
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001ADC3 File Offset: 0x00018FC3
		private unsafe static bool IsIdnAce(char* input, int index)
		{
			return input[index] == 'x' && input[index + 1] == 'n' && input[index + 2] == '-' && input[index + 3] == '-';
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001ADFC File Offset: 0x00018FFC
		internal unsafe static string UnicodeEquivalent(string idnHost, char* hostname, int start, int end)
		{
			IdnMapping idnMapping = new IdnMapping();
			try
			{
				return idnMapping.GetUnicode(idnHost);
			}
			catch (ArgumentException)
			{
			}
			bool flag = true;
			return DomainNameHelper.UnicodeEquivalent(hostname, start, end, ref flag, ref flag);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001AE3C File Offset: 0x0001903C
		internal unsafe static string UnicodeEquivalent(char* hostname, int start, int end, ref bool allAscii, ref bool atLeastOneValidIdn)
		{
			IdnMapping idnMapping = new IdnMapping();
			allAscii = true;
			atLeastOneValidIdn = false;
			string text = null;
			if (end <= start)
			{
				return text;
			}
			string text2 = Uri.StripBidiControlCharacter(hostname, start, end - start);
			string text3 = null;
			int num = 0;
			int i = 0;
			int length = text2.Length;
			bool flag = false;
			do
			{
				bool flag2 = true;
				bool flag3 = false;
				bool flag4 = false;
				flag = false;
				for (i = num; i < length; i++)
				{
					char c = text2[i];
					if (!flag4)
					{
						flag4 = true;
						if (i + 3 < length && c == 'x' && DomainNameHelper.IsIdnAce(text2, i))
						{
							flag3 = true;
						}
					}
					if (flag2 && c > '\u007f')
					{
						flag2 = false;
						allAscii = false;
					}
					if (c == '.' || c == '。' || c == '．' || c == '｡')
					{
						flag = true;
						break;
					}
				}
				if (!flag2)
				{
					string text4 = text2.Substring(num, i - num);
					try
					{
						text4 = idnMapping.GetAscii(text4);
					}
					catch (ArgumentException)
					{
						throw new UriFormatException(SR.GetString("net_uri_BadUnicodeHostForIdn"));
					}
					text3 += idnMapping.GetUnicode(text4);
					if (flag)
					{
						text3 += ".";
					}
				}
				else
				{
					bool flag5 = false;
					if (flag3)
					{
						try
						{
							text3 += idnMapping.GetUnicode(text2.Substring(num, i - num));
							if (flag)
							{
								text3 += ".";
							}
							flag5 = true;
							atLeastOneValidIdn = true;
						}
						catch (ArgumentException)
						{
						}
					}
					if (!flag5)
					{
						text3 += text2.Substring(num, i - num).ToLowerInvariant();
						if (flag)
						{
							text3 += ".";
						}
					}
				}
				num = i + (flag ? 1 : 0);
			}
			while (num < length);
			return text3;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001AFF4 File Offset: 0x000191F4
		private static bool IsASCIILetterOrDigit(char character, ref bool notCanonical)
		{
			if ((character >= 'a' && character <= 'z') || (character >= '0' && character <= '9'))
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				notCanonical = true;
				return true;
			}
			return false;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001B01C File Offset: 0x0001921C
		private static bool IsValidDomainLabelCharacter(char character, ref bool notCanonical)
		{
			if ((character >= 'a' && character <= 'z') || (character >= '0' && character <= '9') || character == '-' || character == '_')
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				notCanonical = true;
				return true;
			}
			return false;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001B04E File Offset: 0x0001924E
		internal static bool ContainsCharactersUnsafeForNormalizedHost(string host)
		{
			return host.IndexOfAny(DomainNameHelper.s_UnsafeForNormalizedHost) != -1;
		}

		// Token: 0x0400044C RID: 1100
		private const char c_DummyChar = '\uffff';

		// Token: 0x0400044D RID: 1101
		internal const string Localhost = "localhost";

		// Token: 0x0400044E RID: 1102
		internal const string Loopback = "loopback";

		// Token: 0x0400044F RID: 1103
		private static readonly char[] s_UnsafeForNormalizedHost = new char[] { '\\', '/', '?', '@', '#', ':', '[', ']' };
	}
}
