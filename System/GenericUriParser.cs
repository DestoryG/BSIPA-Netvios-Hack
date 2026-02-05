using System;

namespace System
{
	// Token: 0x02000051 RID: 81
	public class GenericUriParser : UriParser
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x0001D3FC File Offset: 0x0001B5FC
		public GenericUriParser(GenericUriParserOptions options)
			: base(GenericUriParser.MapGenericParserOptions(options))
		{
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001D40C File Offset: 0x0001B60C
		private static UriSyntaxFlags MapGenericParserOptions(GenericUriParserOptions options)
		{
			UriSyntaxFlags uriSyntaxFlags = UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.AllowDnsHost | UriSyntaxFlags.AllowIPv4Host | UriSyntaxFlags.AllowIPv6Host | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.UnEscapeDotsAndSlashes;
			if ((options & GenericUriParserOptions.GenericAuthority) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~(UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.AllowDnsHost | UriSyntaxFlags.AllowIPv4Host | UriSyntaxFlags.AllowIPv6Host);
				uriSyntaxFlags |= UriSyntaxFlags.AllowAnyOtherHost;
			}
			if ((options & GenericUriParserOptions.AllowEmptyAuthority) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags |= UriSyntaxFlags.AllowEmptyHost;
			}
			if ((options & GenericUriParserOptions.NoUserInfo) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~UriSyntaxFlags.MayHaveUserInfo;
			}
			if ((options & GenericUriParserOptions.NoPort) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~UriSyntaxFlags.MayHavePort;
			}
			if ((options & GenericUriParserOptions.NoQuery) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~UriSyntaxFlags.MayHaveQuery;
			}
			if ((options & GenericUriParserOptions.NoFragment) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~UriSyntaxFlags.MayHaveFragment;
			}
			if ((options & GenericUriParserOptions.DontConvertPathBackslashes) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~UriSyntaxFlags.ConvertPathSlashes;
			}
			if ((options & GenericUriParserOptions.DontCompressPath) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~(UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath);
			}
			if ((options & GenericUriParserOptions.DontUnescapePathDotsAndSlashes) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags &= ~UriSyntaxFlags.UnEscapeDotsAndSlashes;
			}
			if ((options & GenericUriParserOptions.Idn) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags |= UriSyntaxFlags.AllowIdn;
			}
			if ((options & GenericUriParserOptions.IriParsing) != GenericUriParserOptions.Default)
			{
				uriSyntaxFlags |= UriSyntaxFlags.AllowIriParsing;
			}
			return uriSyntaxFlags;
		}

		// Token: 0x040004C7 RID: 1223
		private const UriSyntaxFlags DefaultGenericUriParserFlags = UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.AllowDnsHost | UriSyntaxFlags.AllowIPv4Host | UriSyntaxFlags.AllowIPv6Host | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.UnEscapeDotsAndSlashes;
	}
}
