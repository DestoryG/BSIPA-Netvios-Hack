using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Cecil
{
	// Token: 0x02000014 RID: 20
	public class AssemblyNameReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005100 File Offset: 0x00003300
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00005108 File Offset: 0x00003308
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.full_name = null;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005118 File Offset: 0x00003318
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00005120 File Offset: 0x00003320
		public string Culture
		{
			get
			{
				return this.culture;
			}
			set
			{
				this.culture = value;
				this.full_name = null;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005130 File Offset: 0x00003330
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00005138 File Offset: 0x00003338
		public Version Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = Mixin.CheckVersion(value);
				this.full_name = null;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000514D File Offset: 0x0000334D
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00005155 File Offset: 0x00003355
		public AssemblyAttributes Attributes
		{
			get
			{
				return (AssemblyAttributes)this.attributes;
			}
			set
			{
				this.attributes = (uint)value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000515E File Offset: 0x0000335E
		// (set) Token: 0x060000EB RID: 235 RVA: 0x0000516C File Offset: 0x0000336C
		public bool HasPublicKey
		{
			get
			{
				return this.attributes.GetAttributes(1U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1U, value);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005181 File Offset: 0x00003381
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000518F File Offset: 0x0000338F
		public bool IsSideBySideCompatible
		{
			get
			{
				return this.attributes.GetAttributes(0U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(0U, value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000051A4 File Offset: 0x000033A4
		// (set) Token: 0x060000EF RID: 239 RVA: 0x000051B6 File Offset: 0x000033B6
		public bool IsRetargetable
		{
			get
			{
				return this.attributes.GetAttributes(256U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(256U, value);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000051CF File Offset: 0x000033CF
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000051E1 File Offset: 0x000033E1
		public bool IsWindowsRuntime
		{
			get
			{
				return this.attributes.GetAttributes(512U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512U, value);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000051FA File Offset: 0x000033FA
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x0000520B File Offset: 0x0000340B
		public byte[] PublicKey
		{
			get
			{
				return this.public_key ?? Empty<byte>.Array;
			}
			set
			{
				this.public_key = value;
				this.HasPublicKey = !this.public_key.IsNullOrEmpty<byte>();
				this.public_key_token = Empty<byte>.Array;
				this.full_name = null;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000523C File Offset: 0x0000343C
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x0000529D File Offset: 0x0000349D
		public byte[] PublicKeyToken
		{
			get
			{
				if (this.public_key_token.IsNullOrEmpty<byte>() && !this.public_key.IsNullOrEmpty<byte>())
				{
					byte[] array = this.HashPublicKey();
					byte[] array2 = new byte[8];
					Array.Copy(array, array.Length - 8, array2, 0, 8);
					Array.Reverse(array2, 0, 8);
					this.public_key_token = array2;
				}
				return this.public_key_token ?? Empty<byte>.Array;
			}
			set
			{
				this.public_key_token = value;
				this.full_name = null;
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000052B0 File Offset: 0x000034B0
		private byte[] HashPublicKey()
		{
			AssemblyHashAlgorithm assemblyHashAlgorithm = this.hash_algorithm;
			HashAlgorithm hashAlgorithm;
			if (assemblyHashAlgorithm == AssemblyHashAlgorithm.Reserved)
			{
				hashAlgorithm = MD5.Create();
			}
			else
			{
				hashAlgorithm = SHA1.Create();
			}
			byte[] array;
			using (hashAlgorithm)
			{
				array = hashAlgorithm.ComputeHash(this.public_key);
			}
			return array;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.AssemblyNameReference;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005308 File Offset: 0x00003508
		public string FullName
		{
			get
			{
				if (this.full_name != null)
				{
					return this.full_name;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.name);
				stringBuilder.Append(", ");
				stringBuilder.Append("Version=");
				stringBuilder.Append(this.version.ToString(4));
				stringBuilder.Append(", ");
				stringBuilder.Append("Culture=");
				stringBuilder.Append(string.IsNullOrEmpty(this.culture) ? "neutral" : this.culture);
				stringBuilder.Append(", ");
				stringBuilder.Append("PublicKeyToken=");
				byte[] publicKeyToken = this.PublicKeyToken;
				if (!publicKeyToken.IsNullOrEmpty<byte>() && publicKeyToken.Length != 0)
				{
					for (int i = 0; i < publicKeyToken.Length; i++)
					{
						stringBuilder.Append(publicKeyToken[i].ToString("x2"));
					}
				}
				else
				{
					stringBuilder.Append("null");
				}
				if (this.IsRetargetable)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append("Retargetable=Yes");
				}
				return this.full_name = stringBuilder.ToString();
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000542C File Offset: 0x0000362C
		public static AssemblyNameReference Parse(string fullName)
		{
			if (fullName == null)
			{
				throw new ArgumentNullException("fullName");
			}
			if (fullName.Length == 0)
			{
				throw new ArgumentException("Name can not be empty");
			}
			AssemblyNameReference assemblyNameReference = new AssemblyNameReference();
			string[] array = fullName.Split(new char[] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (i == 0)
				{
					assemblyNameReference.Name = text;
				}
				else
				{
					string[] array2 = text.Split(new char[] { '=' });
					if (array2.Length != 2)
					{
						throw new ArgumentException("Malformed name");
					}
					string text2 = array2[0].ToLowerInvariant();
					if (!(text2 == "version"))
					{
						if (!(text2 == "culture"))
						{
							if (text2 == "publickeytoken")
							{
								string text3 = array2[1];
								if (!(text3 == "null"))
								{
									assemblyNameReference.PublicKeyToken = new byte[text3.Length / 2];
									for (int j = 0; j < assemblyNameReference.PublicKeyToken.Length; j++)
									{
										assemblyNameReference.PublicKeyToken[j] = byte.Parse(text3.Substring(j * 2, 2), NumberStyles.HexNumber);
									}
								}
							}
						}
						else
						{
							assemblyNameReference.Culture = ((array2[1] == "neutral") ? "" : array2[1]);
						}
					}
					else
					{
						assemblyNameReference.Version = new Version(array2[1]);
					}
				}
			}
			return assemblyNameReference;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005593 File Offset: 0x00003793
		// (set) Token: 0x060000FB RID: 251 RVA: 0x0000559B File Offset: 0x0000379B
		public AssemblyHashAlgorithm HashAlgorithm
		{
			get
			{
				return this.hash_algorithm;
			}
			set
			{
				this.hash_algorithm = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000055A4 File Offset: 0x000037A4
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000055AC File Offset: 0x000037AC
		public virtual byte[] Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000055B5 File Offset: 0x000037B5
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000055BD File Offset: 0x000037BD
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000055C6 File Offset: 0x000037C6
		internal AssemblyNameReference()
		{
			this.version = Mixin.ZeroVersion;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000055E9 File Offset: 0x000037E9
		public AssemblyNameReference(string name, Version version)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.version = Mixin.CheckVersion(version);
			this.hash_algorithm = AssemblyHashAlgorithm.None;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005621 File Offset: 0x00003821
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x0400002C RID: 44
		private string name;

		// Token: 0x0400002D RID: 45
		private string culture;

		// Token: 0x0400002E RID: 46
		private Version version;

		// Token: 0x0400002F RID: 47
		private uint attributes;

		// Token: 0x04000030 RID: 48
		private byte[] public_key;

		// Token: 0x04000031 RID: 49
		private byte[] public_key_token;

		// Token: 0x04000032 RID: 50
		private AssemblyHashAlgorithm hash_algorithm;

		// Token: 0x04000033 RID: 51
		private byte[] hash;

		// Token: 0x04000034 RID: 52
		internal MetadataToken token;

		// Token: 0x04000035 RID: 53
		private string full_name;
	}
}
