using System;
using System.Configuration;

namespace System.Net
{
	// Token: 0x02000223 RID: 547
	internal sealed class TimeoutValidator : ConfigurationValidatorBase
	{
		// Token: 0x06001419 RID: 5145 RVA: 0x0006A888 File Offset: 0x00068A88
		internal TimeoutValidator(bool zeroValid)
		{
			this._zeroValid = zeroValid;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0006A897 File Offset: 0x00068A97
		public override bool CanValidate(Type type)
		{
			return type == typeof(int) || type == typeof(long);
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0006A8C0 File Offset: 0x00068AC0
		public override void Validate(object value)
		{
			if (value == null)
			{
				return;
			}
			int num = (int)value;
			if (this._zeroValid && num == 0)
			{
				return;
			}
			if (num <= 0 && num != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_io_timeout_use_gt_zero"));
			}
		}

		// Token: 0x04001612 RID: 5650
		private bool _zeroValid;
	}
}
