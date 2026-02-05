using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Mono.Cecil
{
	// Token: 0x020000C1 RID: 193
	internal class AssemblyNameReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x000134D4 File Offset: 0x000116D4
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x000134DC File Offset: 0x000116DC
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000134EC File Offset: 0x000116EC
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x000134F4 File Offset: 0x000116F4
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00013504 File Offset: 0x00011704
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x0001350C File Offset: 0x0001170C
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00013521 File Offset: 0x00011721
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x00013529 File Offset: 0x00011729
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00013532 File Offset: 0x00011732
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00013540 File Offset: 0x00011740
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00013555 File Offset: 0x00011755
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00013563 File Offset: 0x00011763
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00013578 File Offset: 0x00011778
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0001358A File Offset: 0x0001178A
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x000135A3 File Offset: 0x000117A3
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x000135B5 File Offset: 0x000117B5
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x000135CE File Offset: 0x000117CE
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x000135DF File Offset: 0x000117DF
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
				this.public_key_token = null;
				this.full_name = null;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0001360C File Offset: 0x0001180C
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0001366F File Offset: 0x0001186F
		public byte[] PublicKeyToken
		{
			get
			{
				if (this.public_key_token == null && !this.public_key.IsNullOrEmpty<byte>())
				{
					byte[] array = this.HashPublicKey();
					byte[] array2 = new byte[8];
					Array.Copy(array, array.Length - 8, array2, 0, 8);
					Array.Reverse(array2, 0, 8);
					Interlocked.CompareExchange<byte[]>(ref this.public_key_token, array2, null);
				}
				return this.public_key_token ?? Empty<byte>.Array;
			}
			set
			{
				this.public_key_token = value;
				this.full_name = null;
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00013680 File Offset: 0x00011880
		private byte[] HashPublicKey()
		{
			HashAlgorithm hashAlgorithm;
			if (this.hash_algorithm == AssemblyHashAlgorithm.Reserved)
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.AssemblyNameReference;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x000136D4 File Offset: 0x000118D4
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
				Interlocked.CompareExchange<string>(ref this.full_name, stringBuilder.ToString(), null);
				return this.full_name;
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00013800 File Offset: 0x00011A00
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
					if (text2 != null)
					{
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
			}
			return assemblyNameReference;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001396E File Offset: 0x00011B6E
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x00013976 File Offset: 0x00011B76
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0001397F File Offset: 0x00011B7F
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x00013987 File Offset: 0x00011B87
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00013990 File Offset: 0x00011B90
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x00013998 File Offset: 0x00011B98
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

		// Token: 0x06000469 RID: 1129 RVA: 0x000139A1 File Offset: 0x00011BA1
		internal AssemblyNameReference()
		{
			this.version = Mixin.ZeroVersion;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000139C4 File Offset: 0x00011BC4
		public AssemblyNameReference(string name, Version version)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.version = Mixin.CheckVersion(version);
			this.hash_algorithm = AssemblyHashAlgorithm.None;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000139FC File Offset: 0x00011BFC
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x0400022F RID: 559
		private string name;

		// Token: 0x04000230 RID: 560
		private string culture;

		// Token: 0x04000231 RID: 561
		private Version version;

		// Token: 0x04000232 RID: 562
		private uint attributes;

		// Token: 0x04000233 RID: 563
		private byte[] public_key;

		// Token: 0x04000234 RID: 564
		private byte[] public_key_token;

		// Token: 0x04000235 RID: 565
		private AssemblyHashAlgorithm hash_algorithm;

		// Token: 0x04000236 RID: 566
		private byte[] hash;

		// Token: 0x04000237 RID: 567
		internal MetadataToken token;

		// Token: 0x04000238 RID: 568
		private string full_name;
	}
}
