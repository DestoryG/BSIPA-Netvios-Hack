using System;
using System.Reflection;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000080 RID: 128
	internal sealed class SingleFieldAccessor : FieldAccessorBase
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x0001D044 File Offset: 0x0001B244
		internal SingleFieldAccessor(PropertyInfo property, FieldDescriptor descriptor)
			: base(property, descriptor)
		{
			SingleFieldAccessor <>4__this = this;
			if (!property.CanWrite)
			{
				throw new ArgumentException("Not all required properties/methods available");
			}
			this.setValueDelegate = ReflectionUtil.CreateActionIMessageObject(property.GetSetMethod());
			if (descriptor.FieldType == FieldType.Message)
			{
				this.hasDelegate = (IMessage message) => <>4__this.GetValue(message) != null;
				this.clearDelegate = delegate(IMessage message)
				{
					<>4__this.SetValue(message, null);
				};
				return;
			}
			if (descriptor.RealContainingOneof != null)
			{
				OneofAccessor oneofAccessor = descriptor.RealContainingOneof.Accessor;
				this.hasDelegate = (IMessage message) => oneofAccessor.GetCaseFieldDescriptor(message) == descriptor;
				this.clearDelegate = delegate(IMessage message)
				{
					if (oneofAccessor.GetCaseFieldDescriptor(message) == descriptor)
					{
						oneofAccessor.Clear(message);
					}
				};
				return;
			}
			if (descriptor.File.Syntax != Syntax.Proto2 && !descriptor.Proto.Proto3Optional)
			{
				this.hasDelegate = delegate(IMessage message)
				{
					throw new InvalidOperationException("Presence is not implemented for this field");
				};
				Type propertyType = property.PropertyType;
				object defaultValue = ((propertyType == typeof(string)) ? "" : ((propertyType == typeof(ByteString)) ? ByteString.Empty : Activator.CreateInstance(propertyType)));
				this.clearDelegate = delegate(IMessage message)
				{
					<>4__this.SetValue(message, defaultValue);
				};
				return;
			}
			MethodInfo getMethod = property.DeclaringType.GetRuntimeProperty("Has" + property.Name).GetMethod;
			if (getMethod == null)
			{
				throw new ArgumentException("Not all required properties/methods are available");
			}
			this.hasDelegate = ReflectionUtil.CreateFuncIMessageBool(getMethod);
			MethodInfo runtimeMethod = property.DeclaringType.GetRuntimeMethod("Clear" + property.Name, ReflectionUtil.EmptyTypes);
			if (runtimeMethod == null)
			{
				throw new ArgumentException("Not all required properties/methods are available");
			}
			this.clearDelegate = ReflectionUtil.CreateActionIMessage(runtimeMethod);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001D25F File Offset: 0x0001B45F
		public override void Clear(IMessage message)
		{
			this.clearDelegate(message);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001D26D File Offset: 0x0001B46D
		public override bool HasValue(IMessage message)
		{
			return this.hasDelegate(message);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001D27B File Offset: 0x0001B47B
		public override void SetValue(IMessage message, object value)
		{
			this.setValueDelegate(message, value);
		}

		// Token: 0x04000344 RID: 836
		private readonly Action<IMessage, object> setValueDelegate;

		// Token: 0x04000345 RID: 837
		private readonly Action<IMessage> clearDelegate;

		// Token: 0x04000346 RID: 838
		private readonly Func<IMessage, bool> hasDelegate;
	}
}
