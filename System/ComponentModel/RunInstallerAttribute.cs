using System;

namespace System.ComponentModel
{
	// Token: 0x020005A7 RID: 1447
	[AttributeUsage(AttributeTargets.Class)]
	public class RunInstallerAttribute : Attribute
	{
		// Token: 0x06003602 RID: 13826 RVA: 0x000EC1BF File Offset: 0x000EA3BF
		public RunInstallerAttribute(bool runInstaller)
		{
			this.runInstaller = runInstaller;
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06003603 RID: 13827 RVA: 0x000EC1CE File Offset: 0x000EA3CE
		public bool RunInstaller
		{
			get
			{
				return this.runInstaller;
			}
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x000EC1D8 File Offset: 0x000EA3D8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RunInstallerAttribute runInstallerAttribute = obj as RunInstallerAttribute;
			return runInstallerAttribute != null && runInstallerAttribute.RunInstaller == this.runInstaller;
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000EC205 File Offset: 0x000EA405
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000EC20D File Offset: 0x000EA40D
		public override bool IsDefaultAttribute()
		{
			return this.Equals(RunInstallerAttribute.Default);
		}

		// Token: 0x04002A87 RID: 10887
		private bool runInstaller;

		// Token: 0x04002A88 RID: 10888
		public static readonly RunInstallerAttribute Yes = new RunInstallerAttribute(true);

		// Token: 0x04002A89 RID: 10889
		public static readonly RunInstallerAttribute No = new RunInstallerAttribute(false);

		// Token: 0x04002A8A RID: 10890
		public static readonly RunInstallerAttribute Default = RunInstallerAttribute.No;
	}
}
