using System;
using System.Text;

namespace System.Runtime.Versioning
{
	// Token: 0x020003D9 RID: 985
	[global::__DynamicallyInvokable]
	[Serializable]
	public sealed class FrameworkName : IEquatable<FrameworkName>
	{
		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x000AFD8D File Offset: 0x000ADF8D
		[global::__DynamicallyInvokable]
		public string Identifier
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_identifier;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x000AFD95 File Offset: 0x000ADF95
		[global::__DynamicallyInvokable]
		public Version Version
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000AFD9D File Offset: 0x000ADF9D
		[global::__DynamicallyInvokable]
		public string Profile
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.m_profile;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x000AFDA8 File Offset: 0x000ADFA8
		[global::__DynamicallyInvokable]
		public string FullName
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.m_fullName == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.Identifier);
					stringBuilder.Append(',');
					stringBuilder.Append("Version").Append('=');
					stringBuilder.Append('v');
					stringBuilder.Append(this.Version);
					if (!string.IsNullOrEmpty(this.Profile))
					{
						stringBuilder.Append(',');
						stringBuilder.Append("Profile").Append('=');
						stringBuilder.Append(this.Profile);
					}
					this.m_fullName = stringBuilder.ToString();
				}
				return this.m_fullName;
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000AFE4D File Offset: 0x000AE04D
		[global::__DynamicallyInvokable]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as FrameworkName);
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000AFE5B File Offset: 0x000AE05B
		[global::__DynamicallyInvokable]
		public bool Equals(FrameworkName other)
		{
			return other != null && (this.Identifier == other.Identifier && this.Version == other.Version) && this.Profile == other.Profile;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000AFE9B File Offset: 0x000AE09B
		[global::__DynamicallyInvokable]
		public override int GetHashCode()
		{
			return this.Identifier.GetHashCode() ^ this.Version.GetHashCode() ^ this.Profile.GetHashCode();
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000AFEC0 File Offset: 0x000AE0C0
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000AFEC8 File Offset: 0x000AE0C8
		[global::__DynamicallyInvokable]
		public FrameworkName(string identifier, Version version)
			: this(identifier, version, null)
		{
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000AFED4 File Offset: 0x000AE0D4
		[global::__DynamicallyInvokable]
		public FrameworkName(string identifier, Version version, string profile)
		{
			if (identifier == null)
			{
				throw new ArgumentNullException("identifier");
			}
			if (identifier.Trim().Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "identifier" }), "identifier");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this.m_identifier = identifier.Trim();
			this.m_version = (Version)version.Clone();
			this.m_profile = ((profile == null) ? string.Empty : profile.Trim());
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000AFF6C File Offset: 0x000AE16C
		[global::__DynamicallyInvokable]
		public FrameworkName(string frameworkName)
		{
			if (frameworkName == null)
			{
				throw new ArgumentNullException("frameworkName");
			}
			if (frameworkName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "frameworkName" }), "frameworkName");
			}
			string[] array = frameworkName.Split(new char[] { ',' });
			if (array.Length < 2 || array.Length > 3)
			{
				throw new ArgumentException(SR.GetString("Argument_FrameworkNameTooShort"), "frameworkName");
			}
			this.m_identifier = array[0].Trim();
			if (this.m_identifier.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Argument_FrameworkNameInvalid"), "frameworkName");
			}
			bool flag = false;
			this.m_profile = string.Empty;
			int i = 1;
			while (i < array.Length)
			{
				string[] array2 = array[i].Split(new char[] { '=' });
				if (array2.Length != 2)
				{
					throw new ArgumentException(SR.GetString("Argument_FrameworkNameInvalid"), "frameworkName");
				}
				string text = array2[0].Trim();
				string text2 = array2[1].Trim();
				if (text.Equals("Version", StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					if (text2.Length > 0 && (text2[0] == 'v' || text2[0] == 'V'))
					{
						text2 = text2.Substring(1);
					}
					try
					{
						this.m_version = new Version(text2);
						goto IL_0196;
					}
					catch (Exception ex)
					{
						throw new ArgumentException(SR.GetString("Argument_FrameworkNameInvalidVersion"), "frameworkName", ex);
					}
					goto IL_015F;
				}
				goto IL_015F;
				IL_0196:
				i++;
				continue;
				IL_015F:
				if (!text.Equals("Profile", StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(SR.GetString("Argument_FrameworkNameInvalid"), "frameworkName");
				}
				if (!string.IsNullOrEmpty(text2))
				{
					this.m_profile = text2;
					goto IL_0196;
				}
				goto IL_0196;
			}
			if (!flag)
			{
				throw new ArgumentException(SR.GetString("Argument_FrameworkNameMissingVersion"), "frameworkName");
			}
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000B0144 File Offset: 0x000AE344
		[global::__DynamicallyInvokable]
		public static bool operator ==(FrameworkName left, FrameworkName right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000B0155 File Offset: 0x000AE355
		[global::__DynamicallyInvokable]
		public static bool operator !=(FrameworkName left, FrameworkName right)
		{
			return !(left == right);
		}

		// Token: 0x04002067 RID: 8295
		private readonly string m_identifier;

		// Token: 0x04002068 RID: 8296
		private readonly Version m_version;

		// Token: 0x04002069 RID: 8297
		private readonly string m_profile;

		// Token: 0x0400206A RID: 8298
		private string m_fullName;

		// Token: 0x0400206B RID: 8299
		private const char c_componentSeparator = ',';

		// Token: 0x0400206C RID: 8300
		private const char c_keyValueSeparator = '=';

		// Token: 0x0400206D RID: 8301
		private const char c_versionValuePrefix = 'v';

		// Token: 0x0400206E RID: 8302
		private const string c_versionKey = "Version";

		// Token: 0x0400206F RID: 8303
		private const string c_profileKey = "Profile";
	}
}
