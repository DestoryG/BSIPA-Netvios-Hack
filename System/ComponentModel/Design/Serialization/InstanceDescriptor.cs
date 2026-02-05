using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200060F RID: 1551
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class InstanceDescriptor
	{
		// Token: 0x060038C8 RID: 14536 RVA: 0x000F1AF3 File Offset: 0x000EFCF3
		public InstanceDescriptor(MemberInfo member, ICollection arguments)
			: this(member, arguments, true)
		{
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x000F1B00 File Offset: 0x000EFD00
		public InstanceDescriptor(MemberInfo member, ICollection arguments, bool isComplete)
		{
			this.member = member;
			this.isComplete = isComplete;
			if (arguments == null)
			{
				this.arguments = new object[0];
			}
			else
			{
				object[] array = new object[arguments.Count];
				arguments.CopyTo(array, 0);
				this.arguments = array;
			}
			if (member is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (!fieldInfo.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeStatic"));
				}
				if (this.arguments.Count != 0)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorLengthMismatch"));
				}
			}
			else if (member is ConstructorInfo)
			{
				ConstructorInfo constructorInfo = (ConstructorInfo)member;
				if (constructorInfo.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorCannotBeStatic"));
				}
				if (this.arguments.Count != constructorInfo.GetParameters().Length)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorLengthMismatch"));
				}
			}
			else if (member is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)member;
				if (!methodInfo.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeStatic"));
				}
				if (this.arguments.Count != methodInfo.GetParameters().Length)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorLengthMismatch"));
				}
			}
			else if (member is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)member;
				if (!propertyInfo.CanRead)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeReadable"));
				}
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != null && !getMethod.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeStatic"));
				}
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x000F1C84 File Offset: 0x000EFE84
		public ICollection Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x060038CB RID: 14539 RVA: 0x000F1C8C File Offset: 0x000EFE8C
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x060038CC RID: 14540 RVA: 0x000F1C94 File Offset: 0x000EFE94
		public MemberInfo MemberInfo
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x000F1C9C File Offset: 0x000EFE9C
		public object Invoke()
		{
			object[] array = new object[this.arguments.Count];
			this.arguments.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is InstanceDescriptor)
				{
					array[i] = ((InstanceDescriptor)array[i]).Invoke();
				}
			}
			if (this.member is ConstructorInfo)
			{
				return ((ConstructorInfo)this.member).Invoke(array);
			}
			if (this.member is MethodInfo)
			{
				return ((MethodInfo)this.member).Invoke(null, array);
			}
			if (this.member is PropertyInfo)
			{
				return ((PropertyInfo)this.member).GetValue(null, array);
			}
			if (this.member is FieldInfo)
			{
				return ((FieldInfo)this.member).GetValue(null);
			}
			return null;
		}

		// Token: 0x04002B6D RID: 11117
		private MemberInfo member;

		// Token: 0x04002B6E RID: 11118
		private ICollection arguments;

		// Token: 0x04002B6F RID: 11119
		private bool isComplete;
	}
}
