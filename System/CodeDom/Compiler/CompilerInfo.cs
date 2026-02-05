using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000677 RID: 1655
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class CompilerInfo
	{
		// Token: 0x06003CDD RID: 15581 RVA: 0x000FAE67 File Offset: 0x000F9067
		private CompilerInfo()
		{
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x000FAE6F File Offset: 0x000F906F
		public string[] GetLanguages()
		{
			return this.CloneCompilerLanguages();
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x000FAE77 File Offset: 0x000F9077
		public string[] GetExtensions()
		{
			return this.CloneCompilerExtensions();
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x000FAE80 File Offset: 0x000F9080
		public Type CodeDomProviderType
		{
			get
			{
				if (this.type == null)
				{
					lock (this)
					{
						if (this.type == null)
						{
							this.type = Type.GetType(this._codeDomProviderTypeName);
							if (this.type == null)
							{
								if (this.configFileName == null)
								{
									throw new ConfigurationErrorsException(SR.GetString("Unable_To_Locate_Type", new object[]
									{
										this._codeDomProviderTypeName,
										string.Empty,
										0
									}));
								}
								throw new ConfigurationErrorsException(SR.GetString("Unable_To_Locate_Type", new object[] { this._codeDomProviderTypeName }), this.configFileName, this.configFileLineNumber);
							}
						}
					}
				}
				return this.type;
			}
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x000FAF60 File Offset: 0x000F9160
		public bool IsCodeDomProviderTypeValid
		{
			get
			{
				Type type = Type.GetType(this._codeDomProviderTypeName);
				return type != null;
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x000FAF80 File Offset: 0x000F9180
		public CodeDomProvider CreateProvider()
		{
			if (this._providerOptions.Count > 0)
			{
				ConstructorInfo constructor = this.CodeDomProviderType.GetConstructor(new Type[] { typeof(IDictionary<string, string>) });
				if (constructor != null)
				{
					return (CodeDomProvider)constructor.Invoke(new object[] { this._providerOptions });
				}
			}
			return (CodeDomProvider)Activator.CreateInstance(this.CodeDomProviderType);
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x000FAFF0 File Offset: 0x000F91F0
		public CodeDomProvider CreateProvider(IDictionary<string, string> providerOptions)
		{
			if (providerOptions == null)
			{
				throw new ArgumentNullException("providerOptions");
			}
			ConstructorInfo constructor = this.CodeDomProviderType.GetConstructor(new Type[] { typeof(IDictionary<string, string>) });
			if (constructor != null)
			{
				return (CodeDomProvider)constructor.Invoke(new object[] { providerOptions });
			}
			throw new InvalidOperationException(SR.GetString("Provider_does_not_support_options", new object[] { this.CodeDomProviderType.ToString() }));
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x000FB06C File Offset: 0x000F926C
		public CompilerParameters CreateDefaultCompilerParameters()
		{
			return this.CloneCompilerParameters();
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x000FB074 File Offset: 0x000F9274
		internal CompilerInfo(CompilerParameters compilerParams, string codeDomProviderTypeName, string[] compilerLanguages, string[] compilerExtensions)
		{
			this._compilerLanguages = compilerLanguages;
			this._compilerExtensions = compilerExtensions;
			this._codeDomProviderTypeName = codeDomProviderTypeName;
			if (compilerParams == null)
			{
				compilerParams = new CompilerParameters();
			}
			this._compilerParams = compilerParams;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x000FB0A3 File Offset: 0x000F92A3
		internal CompilerInfo(CompilerParameters compilerParams, string codeDomProviderTypeName)
		{
			this._codeDomProviderTypeName = codeDomProviderTypeName;
			if (compilerParams == null)
			{
				compilerParams = new CompilerParameters();
			}
			this._compilerParams = compilerParams;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x000FB0C3 File Offset: 0x000F92C3
		public override int GetHashCode()
		{
			return this._codeDomProviderTypeName.GetHashCode();
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x000FB0D0 File Offset: 0x000F92D0
		public override bool Equals(object o)
		{
			CompilerInfo compilerInfo = o as CompilerInfo;
			return o != null && (this.CodeDomProviderType == compilerInfo.CodeDomProviderType && this.CompilerParams.WarningLevel == compilerInfo.CompilerParams.WarningLevel && this.CompilerParams.IncludeDebugInformation == compilerInfo.CompilerParams.IncludeDebugInformation) && this.CompilerParams.CompilerOptions == compilerInfo.CompilerParams.CompilerOptions;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x000FB14C File Offset: 0x000F934C
		private CompilerParameters CloneCompilerParameters()
		{
			return new CompilerParameters
			{
				IncludeDebugInformation = this._compilerParams.IncludeDebugInformation,
				TreatWarningsAsErrors = this._compilerParams.TreatWarningsAsErrors,
				WarningLevel = this._compilerParams.WarningLevel,
				CompilerOptions = this._compilerParams.CompilerOptions
			};
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x000FB1A4 File Offset: 0x000F93A4
		private string[] CloneCompilerLanguages()
		{
			string[] array = new string[this._compilerLanguages.Length];
			Array.Copy(this._compilerLanguages, array, this._compilerLanguages.Length);
			return array;
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x000FB1D4 File Offset: 0x000F93D4
		private string[] CloneCompilerExtensions()
		{
			string[] array = new string[this._compilerExtensions.Length];
			Array.Copy(this._compilerExtensions, array, this._compilerExtensions.Length);
			return array;
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x000FB204 File Offset: 0x000F9404
		internal CompilerParameters CompilerParams
		{
			get
			{
				return this._compilerParams;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06003CED RID: 15597 RVA: 0x000FB20C File Offset: 0x000F940C
		internal IDictionary<string, string> ProviderOptions
		{
			get
			{
				return this._providerOptions;
			}
		}

		// Token: 0x04002C6F RID: 11375
		internal string _codeDomProviderTypeName;

		// Token: 0x04002C70 RID: 11376
		internal CompilerParameters _compilerParams;

		// Token: 0x04002C71 RID: 11377
		internal string[] _compilerLanguages;

		// Token: 0x04002C72 RID: 11378
		internal string[] _compilerExtensions;

		// Token: 0x04002C73 RID: 11379
		internal string configFileName;

		// Token: 0x04002C74 RID: 11380
		internal IDictionary<string, string> _providerOptions;

		// Token: 0x04002C75 RID: 11381
		internal int configFileLineNumber;

		// Token: 0x04002C76 RID: 11382
		internal bool _mapped;

		// Token: 0x04002C77 RID: 11383
		private Type type;
	}
}
