using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace System.Windows.Input
{
	// Token: 0x020003A2 RID: 930
	[TypeForwardedFrom("PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	[TypeConverter("System.Windows.Input.CommandConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
	[ValueSerializer("System.Windows.Input.CommandValueSerializer, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
	[global::__DynamicallyInvokable]
	public interface ICommand
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060022A1 RID: 8865
		// (remove) Token: 0x060022A2 RID: 8866
		[global::__DynamicallyInvokable]
		event EventHandler CanExecuteChanged;

		// Token: 0x060022A3 RID: 8867
		[global::__DynamicallyInvokable]
		bool CanExecute(object parameter);

		// Token: 0x060022A4 RID: 8868
		[global::__DynamicallyInvokable]
		void Execute(object parameter);
	}
}
