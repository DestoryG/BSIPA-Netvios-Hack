using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005A2 RID: 1442
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class ReflectEventDescriptor : EventDescriptor
	{
		// Token: 0x0600359B RID: 13723 RVA: 0x000E8C28 File Offset: 0x000E6E28
		public ReflectEventDescriptor(Type componentClass, string name, Type type, Attribute[] attributes)
			: base(name, attributes)
		{
			if (componentClass == null)
			{
				throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[] { "componentClass" }));
			}
			if (type == null || !typeof(Delegate).IsAssignableFrom(type))
			{
				throw new ArgumentException(SR.GetString("ErrorInvalidEventType", new object[] { name }));
			}
			this.componentClass = componentClass;
			this.type = type;
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000E8CA8 File Offset: 0x000E6EA8
		public ReflectEventDescriptor(Type componentClass, EventInfo eventInfo)
			: base(eventInfo.Name, new Attribute[0])
		{
			if (componentClass == null)
			{
				throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[] { "componentClass" }));
			}
			this.componentClass = componentClass;
			this.realEvent = eventInfo;
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000E8CFC File Offset: 0x000E6EFC
		public ReflectEventDescriptor(Type componentType, EventDescriptor oldReflectEventDescriptor, Attribute[] attributes)
			: base(oldReflectEventDescriptor, attributes)
		{
			this.componentClass = componentType;
			this.type = oldReflectEventDescriptor.EventType;
			ReflectEventDescriptor reflectEventDescriptor = oldReflectEventDescriptor as ReflectEventDescriptor;
			if (reflectEventDescriptor != null)
			{
				this.addMethod = reflectEventDescriptor.addMethod;
				this.removeMethod = reflectEventDescriptor.removeMethod;
				this.filledMethods = true;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x0600359E RID: 13726 RVA: 0x000E8D4D File Offset: 0x000E6F4D
		public override Type ComponentType
		{
			get
			{
				return this.componentClass;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x0600359F RID: 13727 RVA: 0x000E8D55 File Offset: 0x000E6F55
		public override Type EventType
		{
			get
			{
				this.FillMethods();
				return this.type;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x060035A0 RID: 13728 RVA: 0x000E8D63 File Offset: 0x000E6F63
		public override bool IsMulticast
		{
			get
			{
				return typeof(MulticastDelegate).IsAssignableFrom(this.EventType);
			}
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000E8D7C File Offset: 0x000E6F7C
		public override void AddEventHandler(object component, Delegate value)
		{
			this.FillMethods();
			if (component != null)
			{
				ISite site = MemberDescriptor.GetSite(component);
				IComponentChangeService componentChangeService = null;
				if (site != null)
				{
					componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				}
				if (componentChangeService != null)
				{
					try
					{
						componentChangeService.OnComponentChanging(component, this);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return;
						}
						throw ex;
					}
				}
				bool flag = false;
				if (site != null && site.DesignMode)
				{
					if (this.EventType != value.GetType())
					{
						throw new ArgumentException(SR.GetString("ErrorInvalidEventHandler", new object[] { this.Name }));
					}
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					if (dictionaryService != null)
					{
						Delegate @delegate = (Delegate)dictionaryService.GetValue(this);
						@delegate = Delegate.Combine(@delegate, value);
						dictionaryService.SetValue(this, @delegate);
						flag = true;
					}
				}
				if (!flag)
				{
					SecurityUtils.MethodInfoInvoke(this.addMethod, component, new object[] { value });
				}
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(component, this, null, value);
				}
			}
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000E8E8C File Offset: 0x000E708C
		protected override void FillAttributes(IList attributes)
		{
			this.FillMethods();
			if (this.realEvent != null)
			{
				this.FillEventInfoAttribute(this.realEvent, attributes);
			}
			else
			{
				this.FillSingleMethodAttribute(this.removeMethod, attributes);
				this.FillSingleMethodAttribute(this.addMethod, attributes);
			}
			base.FillAttributes(attributes);
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000E8EE0 File Offset: 0x000E70E0
		private void FillEventInfoAttribute(EventInfo realEventInfo, IList attributes)
		{
			string name = realEventInfo.Name;
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
			Type type = realEventInfo.ReflectedType;
			int num = 0;
			while (type != typeof(object))
			{
				num++;
				type = type.BaseType;
			}
			if (num > 0)
			{
				type = realEventInfo.ReflectedType;
				Attribute[][] array = new Attribute[num][];
				while (type != typeof(object))
				{
					MemberInfo @event = type.GetEvent(name, bindingFlags);
					if (@event != null)
					{
						array[--num] = ReflectTypeDescriptionProvider.ReflectGetAttributes(@event);
					}
					type = type.BaseType;
				}
				foreach (Attribute[] array3 in array)
				{
					if (array3 != null)
					{
						foreach (Attribute attribute in array3)
						{
							attributes.Add(attribute);
						}
					}
				}
			}
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x000E8FBC File Offset: 0x000E71BC
		private void FillMethods()
		{
			if (this.filledMethods)
			{
				return;
			}
			if (this.realEvent != null)
			{
				this.addMethod = this.realEvent.GetAddMethod();
				this.removeMethod = this.realEvent.GetRemoveMethod();
				EventInfo eventInfo = null;
				if (this.addMethod == null || this.removeMethod == null)
				{
					Type baseType = this.componentClass.BaseType;
					while (baseType != null && baseType != typeof(object))
					{
						BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
						EventInfo @event = baseType.GetEvent(this.realEvent.Name, bindingFlags);
						if (@event.GetAddMethod() != null)
						{
							eventInfo = @event;
							break;
						}
					}
				}
				if (eventInfo != null)
				{
					this.addMethod = eventInfo.GetAddMethod();
					this.removeMethod = eventInfo.GetRemoveMethod();
					this.type = eventInfo.EventHandlerType;
				}
				else
				{
					this.type = this.realEvent.EventHandlerType;
				}
			}
			else
			{
				this.realEvent = this.componentClass.GetEvent(this.Name);
				if (this.realEvent != null)
				{
					this.FillMethods();
					return;
				}
				Type[] array = new Type[] { this.type };
				this.addMethod = MemberDescriptor.FindMethod(this.componentClass, "AddOn" + this.Name, array, typeof(void));
				this.removeMethod = MemberDescriptor.FindMethod(this.componentClass, "RemoveOn" + this.Name, array, typeof(void));
				if (this.addMethod == null || this.removeMethod == null)
				{
					throw new ArgumentException(SR.GetString("ErrorMissingEventAccessors", new object[] { this.Name }));
				}
			}
			this.filledMethods = true;
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000E9198 File Offset: 0x000E7398
		private void FillSingleMethodAttribute(MethodInfo realMethodInfo, IList attributes)
		{
			string name = realMethodInfo.Name;
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
			Type type = realMethodInfo.ReflectedType;
			int num = 0;
			while (type != null && type != typeof(object))
			{
				num++;
				type = type.BaseType;
			}
			if (num > 0)
			{
				type = realMethodInfo.ReflectedType;
				Attribute[][] array = new Attribute[num][];
				while (type != null && type != typeof(object))
				{
					MemberInfo method = type.GetMethod(name, bindingFlags);
					if (method != null)
					{
						array[--num] = ReflectTypeDescriptionProvider.ReflectGetAttributes(method);
					}
					type = type.BaseType;
				}
				foreach (Attribute[] array3 in array)
				{
					if (array3 != null)
					{
						foreach (Attribute attribute in array3)
						{
							attributes.Add(attribute);
						}
					}
				}
			}
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000E9288 File Offset: 0x000E7488
		public override void RemoveEventHandler(object component, Delegate value)
		{
			this.FillMethods();
			if (component != null)
			{
				ISite site = MemberDescriptor.GetSite(component);
				IComponentChangeService componentChangeService = null;
				if (site != null)
				{
					componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				}
				if (componentChangeService != null)
				{
					try
					{
						componentChangeService.OnComponentChanging(component, this);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return;
						}
						throw ex;
					}
				}
				bool flag = false;
				if (site != null && site.DesignMode)
				{
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					if (dictionaryService != null)
					{
						Delegate @delegate = (Delegate)dictionaryService.GetValue(this);
						@delegate = Delegate.Remove(@delegate, value);
						dictionaryService.SetValue(this, @delegate);
						flag = true;
					}
				}
				if (!flag)
				{
					SecurityUtils.MethodInfoInvoke(this.removeMethod, component, new object[] { value });
				}
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(component, this, null, value);
				}
			}
		}

		// Token: 0x04002A51 RID: 10833
		private static readonly Type[] argsNone = new Type[0];

		// Token: 0x04002A52 RID: 10834
		private static readonly object noDefault = new object();

		// Token: 0x04002A53 RID: 10835
		private Type type;

		// Token: 0x04002A54 RID: 10836
		private readonly Type componentClass;

		// Token: 0x04002A55 RID: 10837
		private MethodInfo addMethod;

		// Token: 0x04002A56 RID: 10838
		private MethodInfo removeMethod;

		// Token: 0x04002A57 RID: 10839
		private EventInfo realEvent;

		// Token: 0x04002A58 RID: 10840
		private bool filledMethods;
	}
}
