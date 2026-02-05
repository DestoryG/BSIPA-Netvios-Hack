using System;

namespace IPA.Config.Stores.Attributes
{
	/// <summary>
	/// Indicates that the generated subclass of the attribute's target should implement <see cref="T:System.ComponentModel.INotifyPropertyChanged" />.
	/// If the type this is applied to already inherits it, this is implied.
	/// </summary>
	// Token: 0x0200008F RID: 143
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class NotifyPropertyChangesAttribute : Attribute
	{
	}
}
