using System;
using System.Reflection;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200007D RID: 125
	internal static class ReflectionUtil
	{
		// Token: 0x06000816 RID: 2070 RVA: 0x0001CC5C File Offset: 0x0001AE5C
		static ReflectionUtil()
		{
			ReflectionUtil.ForceInitialize<string>();
			ReflectionUtil.ForceInitialize<int>();
			ReflectionUtil.ForceInitialize<long>();
			ReflectionUtil.ForceInitialize<uint>();
			ReflectionUtil.ForceInitialize<ulong>();
			ReflectionUtil.ForceInitialize<float>();
			ReflectionUtil.ForceInitialize<double>();
			ReflectionUtil.ForceInitialize<bool>();
			ReflectionUtil.ForceInitialize<int?>();
			ReflectionUtil.ForceInitialize<long?>();
			ReflectionUtil.ForceInitialize<uint?>();
			ReflectionUtil.ForceInitialize<ulong?>();
			ReflectionUtil.ForceInitialize<float?>();
			ReflectionUtil.ForceInitialize<double?>();
			ReflectionUtil.ForceInitialize<bool?>();
			ReflectionUtil.ForceInitialize<ReflectionUtil.SampleEnum>();
			ReflectionUtil.SampleEnumMethod();
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001CCD4 File Offset: 0x0001AED4
		internal static void ForceInitialize<T>()
		{
			new ReflectionUtil.ReflectionHelper<IMessage, T>();
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001CCDC File Offset: 0x0001AEDC
		internal static Func<IMessage, object> CreateFuncIMessageObject(MethodInfo method)
		{
			return ReflectionUtil.GetReflectionHelper(method.DeclaringType, method.ReturnType).CreateFuncIMessageObject(method);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001CCF5 File Offset: 0x0001AEF5
		internal static Func<IMessage, int> CreateFuncIMessageInt32(MethodInfo method)
		{
			return ReflectionUtil.GetReflectionHelper(method.DeclaringType, method.ReturnType).CreateFuncIMessageInt32(method);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001CD0E File Offset: 0x0001AF0E
		internal static Action<IMessage, object> CreateActionIMessageObject(MethodInfo method)
		{
			return ReflectionUtil.GetReflectionHelper(method.DeclaringType, method.GetParameters()[0].ParameterType).CreateActionIMessageObject(method);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001CD2E File Offset: 0x0001AF2E
		internal static Action<IMessage> CreateActionIMessage(MethodInfo method)
		{
			return ReflectionUtil.GetReflectionHelper(method.DeclaringType, typeof(object)).CreateActionIMessage(method);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001CD4B File Offset: 0x0001AF4B
		internal static Func<IMessage, bool> CreateFuncIMessageBool(MethodInfo method)
		{
			return ReflectionUtil.GetReflectionHelper(method.DeclaringType, method.ReturnType).CreateFuncIMessageBool(method);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001CD64 File Offset: 0x0001AF64
		internal static Func<IMessage, bool> CreateIsInitializedCaller(Type msg)
		{
			return ((ReflectionUtil.IExtensionSetReflector)Activator.CreateInstance(typeof(ReflectionUtil.ExtensionSetReflector<>).MakeGenericType(new Type[] { msg }))).CreateIsInitializedCaller();
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001CD90 File Offset: 0x0001AF90
		internal static ReflectionUtil.IExtensionReflectionHelper CreateExtensionHelper(Extension extension)
		{
			return (ReflectionUtil.IExtensionReflectionHelper)Activator.CreateInstance(typeof(ReflectionUtil.ExtensionReflectionHelper<, >).MakeGenericType(new Type[]
			{
				extension.TargetType,
				extension.GetType().GenericTypeArguments[1]
			}), new object[] { extension });
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001CDDF File Offset: 0x0001AFDF
		private static ReflectionUtil.IReflectionHelper GetReflectionHelper(Type t1, Type t2)
		{
			return (ReflectionUtil.IReflectionHelper)Activator.CreateInstance(typeof(ReflectionUtil.ReflectionHelper<, >).MakeGenericType(new Type[] { t1, t2 }));
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0001CE08 File Offset: 0x0001B008
		private static bool CanConvertEnumFuncToInt32Func { get; } = ReflectionUtil.CheckCanConvertEnumFuncToInt32Func();

		// Token: 0x06000821 RID: 2081 RVA: 0x0001CE10 File Offset: 0x0001B010
		private static bool CheckCanConvertEnumFuncToInt32Func()
		{
			bool flag;
			try
			{
				typeof(ReflectionUtil).GetMethod("SampleEnumMethod").CreateDelegate(typeof(Func<int>));
				flag = true;
			}
			catch (ArgumentException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001CE5C File Offset: 0x0001B05C
		public static ReflectionUtil.SampleEnum SampleEnumMethod()
		{
			return ReflectionUtil.SampleEnum.X;
		}

		// Token: 0x04000340 RID: 832
		internal static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x020000F3 RID: 243
		private interface IReflectionHelper
		{
			// Token: 0x06000A1E RID: 2590
			Func<IMessage, int> CreateFuncIMessageInt32(MethodInfo method);

			// Token: 0x06000A1F RID: 2591
			Action<IMessage> CreateActionIMessage(MethodInfo method);

			// Token: 0x06000A20 RID: 2592
			Func<IMessage, object> CreateFuncIMessageObject(MethodInfo method);

			// Token: 0x06000A21 RID: 2593
			Action<IMessage, object> CreateActionIMessageObject(MethodInfo method);

			// Token: 0x06000A22 RID: 2594
			Func<IMessage, bool> CreateFuncIMessageBool(MethodInfo method);
		}

		// Token: 0x020000F4 RID: 244
		internal interface IExtensionReflectionHelper
		{
			// Token: 0x06000A23 RID: 2595
			object GetExtension(IMessage message);

			// Token: 0x06000A24 RID: 2596
			void SetExtension(IMessage message, object value);

			// Token: 0x06000A25 RID: 2597
			bool HasExtension(IMessage message);

			// Token: 0x06000A26 RID: 2598
			void ClearExtension(IMessage message);
		}

		// Token: 0x020000F5 RID: 245
		private interface IExtensionSetReflector
		{
			// Token: 0x06000A27 RID: 2599
			Func<IMessage, bool> CreateIsInitializedCaller();
		}

		// Token: 0x020000F6 RID: 246
		private class ReflectionHelper<T1, T2> : ReflectionUtil.IReflectionHelper
		{
			// Token: 0x06000A28 RID: 2600 RVA: 0x00020950 File Offset: 0x0001EB50
			public Func<IMessage, int> CreateFuncIMessageInt32(MethodInfo method)
			{
				if (ReflectionUtil.CanConvertEnumFuncToInt32Func)
				{
					Func<T1, int> del2 = (Func<T1, int>)method.CreateDelegate(typeof(Func<T1, int>));
					return (IMessage message) => del2((T1)((object)message));
				}
				Func<T1, T2> del = (Func<T1, T2>)method.CreateDelegate(typeof(Func<T1, T2>));
				return (IMessage message) => (int)((object)del((T1)((object)message)));
			}

			// Token: 0x06000A29 RID: 2601 RVA: 0x000209BB File Offset: 0x0001EBBB
			public Action<IMessage> CreateActionIMessage(MethodInfo method)
			{
				Action<T1> del = (Action<T1>)method.CreateDelegate(typeof(Action<T1>));
				return delegate(IMessage message)
				{
					del((T1)((object)message));
				};
			}

			// Token: 0x06000A2A RID: 2602 RVA: 0x000209E8 File Offset: 0x0001EBE8
			public Func<IMessage, object> CreateFuncIMessageObject(MethodInfo method)
			{
				Func<T1, T2> del = (Func<T1, T2>)method.CreateDelegate(typeof(Func<T1, T2>));
				return (IMessage message) => del((T1)((object)message));
			}

			// Token: 0x06000A2B RID: 2603 RVA: 0x00020A15 File Offset: 0x0001EC15
			public Action<IMessage, object> CreateActionIMessageObject(MethodInfo method)
			{
				Action<T1, T2> del = (Action<T1, T2>)method.CreateDelegate(typeof(Action<T1, T2>));
				return delegate(IMessage message, object arg)
				{
					del((T1)((object)message), (T2)((object)arg));
				};
			}

			// Token: 0x06000A2C RID: 2604 RVA: 0x00020A42 File Offset: 0x0001EC42
			public Func<IMessage, bool> CreateFuncIMessageBool(MethodInfo method)
			{
				Func<T1, bool> del = (Func<T1, bool>)method.CreateDelegate(typeof(Func<T1, bool>));
				return (IMessage message) => del((T1)((object)message));
			}
		}

		// Token: 0x020000F7 RID: 247
		private class ExtensionReflectionHelper<T1, T3> : ReflectionUtil.IExtensionReflectionHelper where T1 : IExtendableMessage<T1>
		{
			// Token: 0x06000A2E RID: 2606 RVA: 0x00020A77 File Offset: 0x0001EC77
			public ExtensionReflectionHelper(Extension extension)
			{
				this.extension = extension;
			}

			// Token: 0x06000A2F RID: 2607 RVA: 0x00020A88 File Offset: 0x0001EC88
			public object GetExtension(IMessage message)
			{
				if (!(message is T1))
				{
					throw new InvalidCastException("Cannot access extension on message that isn't IExtensionMessage");
				}
				T1 t = (T1)((object)message);
				if (this.extension is Extension<T1, T3>)
				{
					return t.GetExtension<T3>(this.extension as Extension<T1, T3>);
				}
				if (this.extension is RepeatedExtension<T1, T3>)
				{
					return t.GetOrInitializeExtension<T3>(this.extension as RepeatedExtension<T1, T3>);
				}
				throw new InvalidCastException("The provided extension is not a valid extension identifier type");
			}

			// Token: 0x06000A30 RID: 2608 RVA: 0x00020B0C File Offset: 0x0001ED0C
			public bool HasExtension(IMessage message)
			{
				if (!(message is T1))
				{
					throw new InvalidCastException("Cannot access extension on message that isn't IExtensionMessage");
				}
				T1 t = (T1)((object)message);
				if (this.extension is Extension<T1, T3>)
				{
					return t.HasExtension<T3>(this.extension as Extension<T1, T3>);
				}
				if (this.extension is RepeatedExtension<T1, T3>)
				{
					throw new InvalidOperationException("HasValue is not implemented for repeated extensions");
				}
				throw new InvalidCastException("The provided extension is not a valid extension identifier type");
			}

			// Token: 0x06000A31 RID: 2609 RVA: 0x00020B7C File Offset: 0x0001ED7C
			public void SetExtension(IMessage message, object value)
			{
				if (!(message is T1))
				{
					throw new InvalidCastException("Cannot access extension on message that isn't IExtensionMessage");
				}
				T1 t = (T1)((object)message);
				if (this.extension is Extension<T1, T3>)
				{
					t.SetExtension<T3>(this.extension as Extension<T1, T3>, (T3)((object)value));
					return;
				}
				if (this.extension is RepeatedExtension<T1, T3>)
				{
					throw new InvalidOperationException("SetValue is not implemented for repeated extensions");
				}
				throw new InvalidCastException("The provided extension is not a valid extension identifier type");
			}

			// Token: 0x06000A32 RID: 2610 RVA: 0x00020BF4 File Offset: 0x0001EDF4
			public void ClearExtension(IMessage message)
			{
				if (!(message is T1))
				{
					throw new InvalidCastException("Cannot access extension on message that isn't IExtensionMessage");
				}
				T1 t = (T1)((object)message);
				if (this.extension is Extension<T1, T3>)
				{
					t.ClearExtension<T3>(this.extension as Extension<T1, T3>);
					return;
				}
				if (this.extension is RepeatedExtension<T1, T3>)
				{
					t.GetExtension<T3>(this.extension as RepeatedExtension<T1, T3>).Clear();
					return;
				}
				throw new InvalidCastException("The provided extension is not a valid extension identifier type");
			}

			// Token: 0x0400041D RID: 1053
			private readonly Extension extension;
		}

		// Token: 0x020000F8 RID: 248
		private class ExtensionSetReflector<T1> : ReflectionUtil.IExtensionSetReflector where T1 : IExtendableMessage<T1>
		{
			// Token: 0x06000A33 RID: 2611 RVA: 0x00020C78 File Offset: 0x0001EE78
			public Func<IMessage, bool> CreateIsInitializedCaller()
			{
				PropertyInfo declaredProperty = typeof(T1).GetTypeInfo().GetDeclaredProperty("_Extensions");
				Func<T1, ExtensionSet<T1>> getFunc = (Func<T1, ExtensionSet<T1>>)declaredProperty.GetMethod.CreateDelegate(typeof(Func<T1, ExtensionSet<T1>>));
				Func<ExtensionSet<T1>, bool> initializedFunc = (Func<ExtensionSet<T1>, bool>)typeof(ExtensionSet<T1>).GetTypeInfo().GetDeclaredMethod("IsInitialized").CreateDelegate(typeof(Func<ExtensionSet<T1>, bool>));
				return delegate(IMessage m)
				{
					ExtensionSet<T1> extensionSet = getFunc((T1)((object)m));
					return extensionSet == null || initializedFunc(extensionSet);
				};
			}
		}

		// Token: 0x020000F9 RID: 249
		public enum SampleEnum
		{
			// Token: 0x0400041F RID: 1055
			X
		}
	}
}
