using System;
using System.ComponentModel;
using System.Security;
using System.StubHelpers;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003F0 RID: 1008
	internal static class PropertyChangedEventArgsMarshaler
	{
		// Token: 0x0600262D RID: 9773 RVA: 0x000B0615 File Offset: 0x000AE815
		[SecurityCritical]
		internal static IntPtr ConvertToNative(PropertyChangedEventArgs managedArgs)
		{
			if (managedArgs == null)
			{
				return IntPtr.Zero;
			}
			return EventArgsMarshaler.CreateNativePCEventArgsInstance(managedArgs.PropertyName);
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000B062C File Offset: 0x000AE82C
		[SecurityCritical]
		internal static PropertyChangedEventArgs ConvertToManaged(IntPtr nativeArgsIP)
		{
			if (nativeArgsIP == IntPtr.Zero)
			{
				return null;
			}
			object obj = InterfaceMarshaler.ConvertToManagedWithoutUnboxing(nativeArgsIP);
			IPropertyChangedEventArgs propertyChangedEventArgs = (IPropertyChangedEventArgs)obj;
			return new PropertyChangedEventArgs(propertyChangedEventArgs.PropertyName);
		}
	}
}
