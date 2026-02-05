using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IPA.Logging;
using IPA.Utilities;

namespace IPA.Loader
{
	// Token: 0x02000041 RID: 65
	internal class PluginExecutor
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000688A File Offset: 0x00004A8A
		public PluginMetadata Metadata { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00006892 File Offset: 0x00004A92
		public PluginExecutor.Special SpecialType { get; }

		// Token: 0x06000193 RID: 403 RVA: 0x0000689C File Offset: 0x00004A9C
		public PluginExecutor(PluginMetadata meta, PluginExecutor.Special specialType = PluginExecutor.Special.None)
		{
			this.Metadata = meta;
			this.SpecialType = specialType;
			if (specialType != PluginExecutor.Special.None)
			{
				this.CreatePlugin = (PluginMetadata m) => null;
				this.LifecycleEnable = delegate(object o)
				{
				};
				this.LifecycleDisable = (object o) => Task.WhenAll(Array.Empty<Task>());
				return;
			}
			this.PrepareDelegates();
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00006936 File Offset: 0x00004B36
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000693E File Offset: 0x00004B3E
		public object Instance { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00006947 File Offset: 0x00004B47
		// (set) Token: 0x06000197 RID: 407 RVA: 0x0000694F File Offset: 0x00004B4F
		private Func<PluginMetadata, object> CreatePlugin { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00006958 File Offset: 0x00004B58
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00006960 File Offset: 0x00004B60
		private Action<object> LifecycleEnable { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00006969 File Offset: 0x00004B69
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00006971 File Offset: 0x00004B71
		private Func<object, Task> LifecycleDisable { get; set; }

		// Token: 0x0600019C RID: 412 RVA: 0x0000697A File Offset: 0x00004B7A
		public void Create()
		{
			if (this.Instance != null)
			{
				return;
			}
			this.Instance = this.CreatePlugin(this.Metadata);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000699C File Offset: 0x00004B9C
		public void Enable()
		{
			this.LifecycleEnable(this.Instance);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000069AF File Offset: 0x00004BAF
		public Task Disable()
		{
			return this.LifecycleDisable(this.Instance);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000069C4 File Offset: 0x00004BC4
		private void PrepareDelegates()
		{
			PluginLoader.Load(this.Metadata);
			Type type = this.Metadata.Assembly.GetType(this.Metadata.PluginType.FullName);
			this.CreatePlugin = PluginExecutor.MakeCreateFunc(type, this.Metadata.Name);
			this.LifecycleEnable = PluginExecutor.MakeLifecycleEnableFunc(type, this.Metadata.Name);
			this.LifecycleDisable = PluginExecutor.MakeLifecycleDisableFunc(type, this.Metadata.Name);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006A44 File Offset: 0x00004C44
		private static Func<PluginMetadata, object> MakeCreateFunc(Type type, string name)
		{
			ConstructorInfo[] ctors = (from t in (from c in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
					select new global::System.ValueTuple<ConstructorInfo, InitAttribute>(c, c.GetCustomAttribute<InitAttribute>())).NonNull(([global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "c", "attr" })] global::System.ValueTuple<ConstructorInfo, InitAttribute> t) => t.Item2)
				orderby t.Item1.GetParameters().Length descending
				select t.Item1).ToArray<ConstructorInfo>();
			if (ctors.Length > 1)
			{
				Logger.loader.Warn("Plugin " + name + " has multiple [Init] constructors. Picking the one with the most parameters.");
			}
			bool usingDefaultCtor = false;
			ConstructorInfo ctor = ctors.FirstOrDefault<ConstructorInfo>();
			if (ctor == null)
			{
				usingDefaultCtor = true;
				ctor = type.GetConstructor(Type.EmptyTypes);
				if (ctor == null)
				{
					throw new InvalidOperationException(type.FullName + " does not expose a public default constructor and has no constructors marked [Init]");
				}
			}
			MethodInfo[] initMethods = (from t in (from m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
					select new global::System.ValueTuple<MethodInfo, InitAttribute>(m, m.GetCustomAttribute<InitAttribute>())).NonNull(([global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "m", "attr" })] global::System.ValueTuple<MethodInfo, InitAttribute> t) => t.Item2)
				select t.Item1).ToArray<MethodInfo>();
			foreach (MethodInfo method in initMethods)
			{
				if (method.GetCustomAttributes(typeof(IEdgeLifecycleAttribute), false).Length != 0)
				{
					throw new InvalidOperationException(string.Format("Method {0} on {1} has both an [Init] attribute and a lifecycle attribute.", method, type.FullName));
				}
			}
			ParameterExpression metaParam = Expression.Parameter(typeof(PluginMetadata), "meta");
			ParameterExpression objVar = Expression.Variable(type, "objVar");
			ParameterExpression persistVar = Expression.Variable(typeof(object), "persistVar");
			return Expression.Lambda<Func<PluginMetadata, object>>(Expression.Block(new ParameterExpression[] { objVar, persistVar }, initMethods.Select((MethodInfo m) => PluginInitInjector.InjectedCallExpr(m.GetParameters(), metaParam, persistVar, (IEnumerable<Expression> es) => Expression.Call(objVar, m, es))).Prepend(Expression.Assign(objVar, usingDefaultCtor ? Expression.New(ctor) : PluginInitInjector.InjectedCallExpr(ctor.GetParameters(), metaParam, persistVar, (IEnumerable<Expression> es) => Expression.New(ctor, es)))).Append(Expression.Convert(objVar, typeof(object)))), new ParameterExpression[] { metaParam }).Compile();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006D1C File Offset: 0x00004F1C
		private static Action<object> MakeLifecycleEnableFunc(Type type, string name)
		{
			MethodInfo[] enableMethods = (from m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
				select new global::System.ValueTuple<MethodInfo, object[]>(m, m.GetCustomAttributes(typeof(IEdgeLifecycleAttribute), false)) into t
				select new global::System.ValueTuple<MethodInfo, IEnumerable<IEdgeLifecycleAttribute>>(t.Item1, t.Item2.Cast<IEdgeLifecycleAttribute>()) into t
				where t.Item2.Any((IEdgeLifecycleAttribute a) => a.Type == EdgeLifecycleType.Enable)
				select t.Item1).ToArray<MethodInfo>();
			if (enableMethods.Length == 0)
			{
				Logger.loader.Notice("Plugin " + name + " has no methods marked [OnStart] or [OnEnable]. Is this intentional?");
				return delegate(object o)
				{
				};
			}
			foreach (MethodInfo i in enableMethods)
			{
				if (i.GetParameters().Length != 0)
				{
					throw new InvalidOperationException(string.Format("Method {0} on {1} is marked [OnStart] or [OnEnable] and has parameters.", i, type.FullName));
				}
				if (i.ReturnType != typeof(void))
				{
					Logger.loader.Warn(string.Format("Method {0} on {1} is marked [OnStart] or [OnEnable] and returns a value. It will be ignored.", i, type.FullName));
				}
			}
			ParameterExpression objParam = Expression.Parameter(typeof(object), "obj");
			ParameterExpression instVar = Expression.Variable(type, "inst");
			return Expression.Lambda<Action<object>>(Expression.Block(new ParameterExpression[] { instVar }, enableMethods.Select((MethodInfo m) => Expression.Call(instVar, m)).Prepend(Expression.Assign(instVar, Expression.Convert(objParam, type)))), new ParameterExpression[] { objParam }).Compile();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006EF4 File Offset: 0x000050F4
		private static Func<object, Task> MakeLifecycleDisableFunc(Type type, string name)
		{
			MethodInfo[] disableMethods = (from m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
				select new global::System.ValueTuple<MethodInfo, object[]>(m, m.GetCustomAttributes(typeof(IEdgeLifecycleAttribute), false)) into t
				select new global::System.ValueTuple<MethodInfo, IEnumerable<IEdgeLifecycleAttribute>>(t.Item1, t.Item2.Cast<IEdgeLifecycleAttribute>()) into t
				where t.Item2.Any((IEdgeLifecycleAttribute a) => a.Type == EdgeLifecycleType.Disable)
				select t.Item1).ToArray<MethodInfo>();
			if (disableMethods.Length == 0)
			{
				Logger.loader.Notice("Plugin " + name + " has no methods marked [OnExit] or [OnDisable]. Is this intentional?");
				return (object o) => Task.WhenAll(Array.Empty<Task>());
			}
			List<MethodInfo> taskMethods = new List<MethodInfo>();
			List<MethodInfo> nonTaskMethods = new List<MethodInfo>();
			MethodInfo[] array = disableMethods;
			int j = 0;
			while (j < array.Length)
			{
				MethodInfo i = array[j];
				if (i.GetParameters().Length != 0)
				{
					throw new InvalidOperationException(string.Format("Method {0} on {1} is marked [OnExit] or [OnDisable] and has parameters.", i, type.FullName));
				}
				if (!(i.ReturnType != typeof(void)))
				{
					goto IL_0178;
				}
				if (!typeof(Task).IsAssignableFrom(i.ReturnType))
				{
					Logger.loader.Warn(string.Format("Method {0} on {1} is marked [OnExit] or [OnDisable] and returns a non-Task value. It will be ignored.", i, type.FullName));
					goto IL_0178;
				}
				taskMethods.Add(i);
				IL_0180:
				j++;
				continue;
				IL_0178:
				nonTaskMethods.Add(i);
				goto IL_0180;
			}
			Expression getCompletedTask = Expression.Lambda<Func<Task>>(Expression.Call(null, methodof(Task.WhenAll(Task[])), new Expression[] { Expression.NewArrayInit(typeof(Task), Array.Empty<Expression>()) }), Array.Empty<ParameterExpression>()).Body;
			MethodInfo taskWhenAll = typeof(Task).GetMethod("WhenAll", new Type[] { typeof(Task[]) });
			ParameterExpression objParam = Expression.Parameter(typeof(object), "obj");
			ParameterExpression instVar = Expression.Variable(type, "inst");
			return Expression.Lambda<Func<object, Task>>(Expression.Block(new ParameterExpression[] { instVar }, nonTaskMethods.Select((MethodInfo m) => Expression.Call(instVar, m)).Prepend(Expression.Assign(instVar, Expression.Convert(objParam, type))).Append((taskMethods.Count == 0) ? getCompletedTask : Expression.Call(taskWhenAll, Expression.NewArrayInit(typeof(Task), taskMethods.Select((MethodInfo m) => Expression.Convert(Expression.Call(instVar, m), typeof(Task))))))), new ParameterExpression[] { objParam }).Compile();
		}

		// Token: 0x020000ED RID: 237
		public enum Special
		{
			// Token: 0x04000329 RID: 809
			None,
			// Token: 0x0400032A RID: 810
			Self,
			// Token: 0x0400032B RID: 811
			Bare
		}
	}
}
