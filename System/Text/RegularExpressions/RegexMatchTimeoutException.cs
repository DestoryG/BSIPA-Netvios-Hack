using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text.RegularExpressions
{
	// Token: 0x020006A0 RID: 1696
	[global::__DynamicallyInvokable]
	[Serializable]
	public class RegexMatchTimeoutException : TimeoutException, ISerializable
	{
		// Token: 0x06003F23 RID: 16163 RVA: 0x0010787D File Offset: 0x00105A7D
		[global::__DynamicallyInvokable]
		public RegexMatchTimeoutException(string regexInput, string regexPattern, TimeSpan matchTimeout)
			: base(SR.GetString("RegexMatchTimeoutException_Occurred"))
		{
			this.Init(regexInput, regexPattern, matchTimeout);
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x001078A5 File Offset: 0x00105AA5
		[global::__DynamicallyInvokable]
		public RegexMatchTimeoutException()
		{
			this.Init();
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x001078C0 File Offset: 0x00105AC0
		[global::__DynamicallyInvokable]
		public RegexMatchTimeoutException(string message)
			: base(message)
		{
			this.Init();
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x001078DC File Offset: 0x00105ADC
		[global::__DynamicallyInvokable]
		public RegexMatchTimeoutException(string message, Exception inner)
			: base(message, inner)
		{
			this.Init();
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x001078FC File Offset: 0x00105AFC
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		protected RegexMatchTimeoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			string @string = info.GetString("regexInput");
			string string2 = info.GetString("regexPattern");
			TimeSpan timeSpan = TimeSpan.FromTicks(info.GetInt64("timeoutTicks"));
			this.Init(@string, string2, timeSpan);
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x00107950 File Offset: 0x00105B50
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			base.GetObjectData(si, context);
			si.AddValue("regexInput", this.regexInput);
			si.AddValue("regexPattern", this.regexPattern);
			si.AddValue("timeoutTicks", this.matchTimeout.Ticks);
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0010799D File Offset: 0x00105B9D
		private void Init()
		{
			this.Init("", "", TimeSpan.FromTicks(-1L));
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x001079B6 File Offset: 0x00105BB6
		private void Init(string input, string pattern, TimeSpan timeout)
		{
			this.regexInput = input;
			this.regexPattern = pattern;
			this.matchTimeout = timeout;
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06003F2B RID: 16171 RVA: 0x001079CD File Offset: 0x00105BCD
		[global::__DynamicallyInvokable]
		public string Pattern
		{
			[global::__DynamicallyInvokable]
			[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return this.regexPattern;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06003F2C RID: 16172 RVA: 0x001079D5 File Offset: 0x00105BD5
		[global::__DynamicallyInvokable]
		public string Input
		{
			[global::__DynamicallyInvokable]
			[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return this.regexInput;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06003F2D RID: 16173 RVA: 0x001079DD File Offset: 0x00105BDD
		[global::__DynamicallyInvokable]
		public TimeSpan MatchTimeout
		{
			[global::__DynamicallyInvokable]
			[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return this.matchTimeout;
			}
		}

		// Token: 0x04002DFE RID: 11774
		private string regexInput;

		// Token: 0x04002DFF RID: 11775
		private string regexPattern;

		// Token: 0x04002E00 RID: 11776
		private TimeSpan matchTimeout = TimeSpan.FromTicks(-1L);
	}
}
