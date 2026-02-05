using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004E4 RID: 1252
	internal class PerformanceMonitor
	{
		// Token: 0x06002F65 RID: 12133 RVA: 0x000D5C9D File Offset: 0x000D3E9D
		internal PerformanceMonitor(string machineName)
		{
			this.machineName = machineName;
			this.Init();
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000D5CB4 File Offset: 0x000D3EB4
		private void Init()
		{
			try
			{
				if (this.machineName != "." && string.Compare(this.machineName, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) != 0)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					this.perfDataKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.PerformanceData, this.machineName);
				}
				else
				{
					this.perfDataKey = Registry.PerformanceData;
				}
			}
			catch (UnauthorizedAccessException)
			{
				throw new Win32Exception(5);
			}
			catch (IOException ex)
			{
				throw new Win32Exception(Marshal.GetHRForException(ex));
			}
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000D5D48 File Offset: 0x000D3F48
		internal void Close()
		{
			if (this.perfDataKey != null)
			{
				this.perfDataKey.Close();
			}
			this.perfDataKey = null;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000D5D64 File Offset: 0x000D3F64
		internal byte[] GetData(string item)
		{
			int i = 17;
			int num = 0;
			int num2 = 0;
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			while (i > 0)
			{
				try
				{
					return (byte[])this.perfDataKey.GetValue(item);
				}
				catch (IOException ex)
				{
					num2 = Marshal.GetHRForException(ex);
					if (num2 <= 167)
					{
						if (num2 != 6)
						{
							if (num2 != 21 && num2 != 167)
							{
								goto IL_00A1;
							}
							goto IL_0089;
						}
					}
					else if (num2 <= 258)
					{
						if (num2 != 170 && num2 != 258)
						{
							goto IL_00A1;
						}
						goto IL_0089;
					}
					else if (num2 != 1722 && num2 != 1726)
					{
						goto IL_00A1;
					}
					this.Init();
					IL_0089:
					i--;
					if (num == 0)
					{
						num = 10;
					}
					else
					{
						Thread.Sleep(num);
						num *= 2;
					}
					continue;
					IL_00A1:
					throw SharedUtils.CreateSafeWin32Exception(num2);
				}
				catch (InvalidCastException ex2)
				{
					throw new InvalidOperationException(SR.GetString("CounterDataCorrupt", new object[] { this.perfDataKey.ToString() }), ex2);
				}
			}
			throw SharedUtils.CreateSafeWin32Exception(num2);
		}

		// Token: 0x040027E1 RID: 10209
		private RegistryKey perfDataKey;

		// Token: 0x040027E2 RID: 10210
		private string machineName;
	}
}
