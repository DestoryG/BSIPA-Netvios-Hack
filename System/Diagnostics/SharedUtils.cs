using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000506 RID: 1286
	internal static class SharedUtils
	{
		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x060030E2 RID: 12514 RVA: 0x000DDFDC File Offset: 0x000DC1DC
		private static object InternalSyncObject
		{
			get
			{
				if (SharedUtils.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SharedUtils.s_InternalSyncObject, obj, null);
				}
				return SharedUtils.s_InternalSyncObject;
			}
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000DE008 File Offset: 0x000DC208
		internal static Win32Exception CreateSafeWin32Exception()
		{
			return SharedUtils.CreateSafeWin32Exception(0);
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000DE010 File Offset: 0x000DC210
		internal static Win32Exception CreateSafeWin32Exception(int error)
		{
			Win32Exception ex = null;
			SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
			securityPermission.Assert();
			try
			{
				if (error == 0)
				{
					ex = new Win32Exception();
				}
				else
				{
					ex = new Win32Exception(error);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return ex;
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x000DE058 File Offset: 0x000DC258
		internal static int CurrentEnvironment
		{
			get
			{
				if (SharedUtils.environment == 0)
				{
					object internalSyncObject = SharedUtils.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (SharedUtils.environment == 0)
						{
							if (Environment.OSVersion.Platform == PlatformID.Win32NT)
							{
								if (Environment.OSVersion.Version.Major >= 5)
								{
									SharedUtils.environment = 1;
								}
								else
								{
									SharedUtils.environment = 2;
								}
							}
							else
							{
								SharedUtils.environment = 3;
							}
						}
					}
				}
				return SharedUtils.environment;
			}
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000DE0E8 File Offset: 0x000DC2E8
		internal static void CheckEnvironment()
		{
			if (SharedUtils.CurrentEnvironment == 3)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000DE102 File Offset: 0x000DC302
		internal static void CheckNtEnvironment()
		{
			if (SharedUtils.CurrentEnvironment == 2)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000DE11C File Offset: 0x000DC31C
		internal static void EnterMutex(string name, ref Mutex mutex)
		{
			string text;
			if (SharedUtils.CurrentEnvironment == 1)
			{
				text = "Global\\" + name;
			}
			else
			{
				text = name;
			}
			SharedUtils.EnterMutexWithoutGlobal(text, ref mutex);
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000DE14C File Offset: 0x000DC34C
		[SecurityPermission(SecurityAction.Assert, ControlPrincipal = true)]
		internal static void EnterMutexWithoutGlobal(string mutexName, ref Mutex mutex)
		{
			MutexSecurity mutexSecurity = new MutexSecurity();
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
			mutexSecurity.AddAccessRule(new MutexAccessRule(securityIdentifier, MutexRights.Modify | MutexRights.Synchronize, AccessControlType.Allow));
			bool flag;
			Mutex mutex2 = new Mutex(false, mutexName, out flag, mutexSecurity);
			SharedUtils.SafeWaitForMutex(mutex2, ref mutex);
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x000DE18D File Offset: 0x000DC38D
		private static bool SafeWaitForMutex(Mutex mutexIn, ref Mutex mutexOut)
		{
			while (SharedUtils.SafeWaitForMutexOnce(mutexIn, ref mutexOut))
			{
				if (mutexOut != null)
				{
					return true;
				}
				Thread.Sleep(0);
			}
			return false;
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x000DE1A8 File Offset: 0x000DC3A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool SafeWaitForMutexOnce(Mutex mutexIn, ref Mutex mutexOut)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			bool flag;
			try
			{
			}
			finally
			{
				Thread.BeginCriticalRegion();
				Thread.BeginThreadAffinity();
				int num = SharedUtils.WaitForSingleObjectDontCallThis(mutexIn.SafeWaitHandle, 500);
				if (num != 0 && num != 128)
				{
					flag = num == 258;
				}
				else
				{
					mutexOut = mutexIn;
					flag = true;
				}
				if (mutexOut == null)
				{
					Thread.EndThreadAffinity();
					Thread.EndCriticalRegion();
				}
			}
			return flag;
		}

		// Token: 0x060030EC RID: 12524
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject", ExactSpelling = true, SetLastError = true)]
		private static extern int WaitForSingleObjectDontCallThis(SafeWaitHandle handle, int timeout);

		// Token: 0x060030ED RID: 12525 RVA: 0x000DE21C File Offset: 0x000DC41C
		internal static string GetLatestBuildDllDirectory(string machineName)
		{
			string text = "";
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Assert();
			try
			{
				if (machineName.Equals("."))
				{
					return SharedUtils.GetLocalBuildDirectory();
				}
				registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName);
				if (registryKey == null)
				{
					throw new InvalidOperationException(SR.GetString("RegKeyMissingShort", new object[] { "HKEY_LOCAL_MACHINE", machineName }));
				}
				registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework");
				if (registryKey2 != null)
				{
					string text2 = (string)registryKey2.GetValue("InstallRoot");
					if (text2 != null && text2 != string.Empty)
					{
						string text3 = "v" + Environment.Version.Major.ToString() + "." + Environment.Version.Minor.ToString();
						RegistryKey registryKey3 = registryKey2.OpenSubKey("policy");
						string text4 = null;
						if (registryKey3 != null)
						{
							try
							{
								RegistryKey registryKey4 = registryKey3.OpenSubKey(text3);
								if (registryKey4 != null)
								{
									try
									{
										text4 = text3 + "." + SharedUtils.GetLargestBuildNumberFromKey(registryKey4).ToString();
										goto IL_0284;
									}
									finally
									{
										registryKey4.Close();
									}
								}
								string[] subKeyNames = registryKey3.GetSubKeyNames();
								int[] array = new int[] { -1, -1, -1 };
								foreach (string text5 in subKeyNames)
								{
									if (text5.Length > 1 && text5[0] == 'v' && text5.Contains("."))
									{
										int[] array2 = new int[] { -1, -1, -1 };
										string[] array3 = text5.Substring(1).Split(new char[] { '.' });
										if (array3.Length == 2 && int.TryParse(array3[0], out array2[0]) && int.TryParse(array3[1], out array2[1]))
										{
											RegistryKey registryKey5 = registryKey3.OpenSubKey(text5);
											if (registryKey5 != null)
											{
												try
												{
													array2[2] = SharedUtils.GetLargestBuildNumberFromKey(registryKey5);
													if (array2[0] > array[0] || (array2[0] == array[0] && array2[1] > array[1]))
													{
														array = array2;
													}
												}
												finally
												{
													registryKey5.Close();
												}
											}
										}
									}
								}
								text4 = string.Concat(new string[]
								{
									"v",
									array[0].ToString(),
									".",
									array[1].ToString(),
									".",
									array[2].ToString()
								});
								IL_0284:;
							}
							finally
							{
								registryKey3.Close();
							}
							if (text4 != null && text4 != string.Empty)
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.Append(text2);
								if (!text2.EndsWith("\\", StringComparison.Ordinal))
								{
									stringBuilder.Append("\\");
								}
								stringBuilder.Append(text4);
								text = stringBuilder.ToString();
							}
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return text;
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x000DE5A8 File Offset: 0x000DC7A8
		private static int GetLargestBuildNumberFromKey(RegistryKey rootKey)
		{
			int num = -1;
			string[] valueNames = rootKey.GetValueNames();
			for (int i = 0; i < valueNames.Length; i++)
			{
				int num2;
				if (int.TryParse(valueNames[i], out num2))
				{
					num = ((num > num2) ? num : num2);
				}
			}
			return num;
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000DE5E2 File Offset: 0x000DC7E2
		private static string GetLocalBuildDirectory()
		{
			return RuntimeEnvironment.GetRuntimeDirectory();
		}

		// Token: 0x040028CB RID: 10443
		internal const int UnknownEnvironment = 0;

		// Token: 0x040028CC RID: 10444
		internal const int W2kEnvironment = 1;

		// Token: 0x040028CD RID: 10445
		internal const int NtEnvironment = 2;

		// Token: 0x040028CE RID: 10446
		internal const int NonNtEnvironment = 3;

		// Token: 0x040028CF RID: 10447
		private static volatile int environment;

		// Token: 0x040028D0 RID: 10448
		private static object s_InternalSyncObject;
	}
}
