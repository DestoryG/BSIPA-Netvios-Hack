using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using IPA.Config;
using IPA.Logging;
using IPA.Utilities;

namespace IPA.Loader
{
	/// <summary>
	/// The type that handles value injecting into a plugin's initialization methods.
	/// </summary>
	/// <remarks>
	/// The default injectors and what they provide are shown in this table.
	/// <list type="table">
	/// <listheader>
	/// <term>Parameter Type</term>
	/// <description>Injected Value</description>
	/// </listheader>
	/// <item>
	/// <term><see cref="T:IPA.Logging.Logger" /></term>
	/// <description>A <see cref="T:IPA.Logging.StandardLogger" /> specialized for the plugin being injected</description>
	/// </item>
	/// <item>
	/// <term><see cref="T:IPA.Loader.PluginMetadata" /></term>
	/// <description>The <see cref="T:IPA.Loader.PluginMetadata" /> of the plugin being injected</description>
	/// </item>
	/// <item>
	/// <term><see cref="T:IPA.Config.Config" /></term>
	/// <description>
	/// <para>A <see cref="T:IPA.Config.Config" /> object for the plugin being injected.</para>
	/// <para>
	/// These parameters may have <see cref="T:IPA.Config.Config.NameAttribute" /> and <see cref="T:IPA.Config.Config.PreferAttribute" /> to control
	/// how it is constructed.
	/// </para>
	/// </description>
	/// </item>
	/// </list>
	/// For all of the default injectors, only one of each will be generated, and any later parameters will recieve the same value as the first one.
	/// </remarks>
	// Token: 0x02000042 RID: 66
	public static class PluginInitInjector
	{
		/// <summary>
		/// Adds an injector to be used when calling future plugins' Init methods.
		/// </summary>
		/// <param name="type">the type of the parameter.</param>
		/// <param name="injector">the function to call for injection.</param>
		// Token: 0x060001A3 RID: 419 RVA: 0x000071B1 File Offset: 0x000053B1
		public static void AddInjector(Type type, PluginInitInjector.InjectParameter injector)
		{
			PluginInitInjector.injectors.Add(new PluginInitInjector.TypedInjector(type, injector));
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000071C4 File Offset: 0x000053C4
		private static int? MatchPriority(Type target, Type source)
		{
			if (target == source)
			{
				return new int?(int.MaxValue);
			}
			if (!target.IsAssignableFrom(source))
			{
				return null;
			}
			if (!target.IsInterface && !source.IsSubclassOf(target))
			{
				return new int?(int.MinValue);
			}
			int value = 0;
			while (!(source == null))
			{
				if (target.IsInterface && source.GetInterfaces().Contains(target))
				{
					return new int?(value);
				}
				if (target == source)
				{
					return new int?(value);
				}
				value--;
				source = source.BaseType;
			}
			return new int?(value);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007260 File Offset: 0x00005460
		internal static Expression InjectedCallExpr(ParameterInfo[] initParams, Expression meta, Expression persistVar, Func<IEnumerable<Expression>, Expression> exprGen)
		{
			ParameterExpression arr = Expression.Variable(typeof(object[]), "initArr");
			IEnumerable<ParameterExpression> enumerable = new ParameterExpression[] { arr };
			Expression[] array = new Expression[2];
			array[0] = Expression.Assign(arr, Expression.Call(PluginInitInjector.InjectMethod, Expression.Constant(initParams), meta, persistVar));
			array[1] = exprGen(initParams.Select((ParameterInfo p) => p.ParameterType).Select((Type t, int i) => Expression.Convert(Expression.ArrayIndex(arr, Expression.Constant(i)), t)));
			return Expression.Block(enumerable, array);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007308 File Offset: 0x00005508
		internal static object[] Inject(ParameterInfo[] initParams, PluginMetadata meta, ref object persist)
		{
			List<object> initArgs = new List<object>();
			Dictionary<PluginInitInjector.TypedInjector, object> previousValues = persist as Dictionary<PluginInitInjector.TypedInjector, object>;
			if (previousValues == null)
			{
				previousValues = new Dictionary<PluginInitInjector.TypedInjector, object>(PluginInitInjector.injectors.Count);
				persist = previousValues;
			}
			for (int j = 0; j < initParams.Length; j++)
			{
				ParameterInfo param = initParams[j];
				Type paramType = param.ParameterType;
				object value = paramType.GetDefault();
				foreach (PluginInitInjector.TypedInjector pair in from i in PluginInitInjector.injectors
					select new global::System.ValueTuple<PluginInitInjector.TypedInjector, int?>(i, PluginInitInjector.MatchPriority(paramType, i.Type)) into t
					where t.Item2 != null
					select new global::System.ValueTuple<PluginInitInjector.TypedInjector, int>(t.Item1, t.Item2.Value) into t
					orderby t.Item2 descending
					select t.Item1)
				{
					object prev = null;
					if (previousValues.ContainsKey(pair))
					{
						prev = previousValues[pair];
					}
					object val = pair.Inject(prev, param, meta);
					if (previousValues.ContainsKey(pair))
					{
						previousValues[pair] = val;
					}
					else
					{
						previousValues.Add(pair, val);
					}
					if (val != null)
					{
						value = val;
						break;
					}
				}
				initArgs.Add(value);
			}
			return initArgs.ToArray();
		}

		// Token: 0x0400009D RID: 157
		private static readonly List<PluginInitInjector.TypedInjector> injectors = new List<PluginInitInjector.TypedInjector>
		{
			new PluginInitInjector.TypedInjector(typeof(Logger), (object prev, ParameterInfo param, PluginMetadata meta) => prev ?? new StandardLogger(meta.Name)),
			new PluginInitInjector.TypedInjector(typeof(PluginMetadata), (object prev, ParameterInfo param, PluginMetadata meta) => prev ?? meta),
			new PluginInitInjector.TypedInjector(typeof(Config), delegate(object prev, ParameterInfo param, PluginMetadata meta)
			{
				if (prev != null)
				{
					return prev;
				}
				return Config.GetConfigFor(meta.Name, param);
			})
		};

		// Token: 0x0400009E RID: 158
		private static readonly MethodInfo InjectMethod = typeof(PluginInitInjector).GetMethod("Inject", BindingFlags.Static | BindingFlags.NonPublic);

		/// <summary>
		/// A typed injector for a plugin's Init method. When registered, called for all associated types. If it returns null, the default for the type will be used.
		/// </summary>
		/// <param name="previous">the previous return value of the function, or <see langword="null" /> if never called for plugin.</param>
		/// <param name="param">the <see cref="T:System.Reflection.ParameterInfo" /> of the parameter being injected.</param>
		/// <param name="meta">the <see cref="T:IPA.Loader.PluginMetadata" /> for the plugin being loaded.</param>
		/// <returns>the value to inject into that parameter.</returns>
		// Token: 0x020000F3 RID: 243
		// (Invoke) Token: 0x06000520 RID: 1312
		public delegate object InjectParameter(object previous, ParameterInfo param, PluginMetadata meta);

		// Token: 0x020000F4 RID: 244
		private struct TypedInjector : IEquatable<PluginInitInjector.TypedInjector>
		{
			// Token: 0x06000523 RID: 1315 RVA: 0x00016B99 File Offset: 0x00014D99
			public TypedInjector(Type t, PluginInitInjector.InjectParameter i)
			{
				this.Type = t;
				this.Injector = i;
			}

			// Token: 0x06000524 RID: 1316 RVA: 0x00016BA9 File Offset: 0x00014DA9
			public object Inject(object prev, ParameterInfo info, PluginMetadata meta)
			{
				return this.Injector(prev, info, meta);
			}

			// Token: 0x06000525 RID: 1317 RVA: 0x00016BB9 File Offset: 0x00014DB9
			public bool Equals(PluginInitInjector.TypedInjector other)
			{
				return this.Type == other.Type && this.Injector == other.Injector;
			}

			// Token: 0x06000526 RID: 1318 RVA: 0x00016BE4 File Offset: 0x00014DE4
			public override bool Equals(object obj)
			{
				if (obj is PluginInitInjector.TypedInjector)
				{
					PluginInitInjector.TypedInjector i = (PluginInitInjector.TypedInjector)obj;
					return this.Equals(i);
				}
				return false;
			}

			// Token: 0x06000527 RID: 1319 RVA: 0x00016C09 File Offset: 0x00014E09
			public override int GetHashCode()
			{
				return this.Type.GetHashCode() ^ this.Injector.GetHashCode();
			}

			// Token: 0x06000528 RID: 1320 RVA: 0x00016C22 File Offset: 0x00014E22
			public static bool operator ==(PluginInitInjector.TypedInjector a, PluginInitInjector.TypedInjector b)
			{
				return a.Equals(b);
			}

			// Token: 0x06000529 RID: 1321 RVA: 0x00016C2C File Offset: 0x00014E2C
			public static bool operator !=(PluginInitInjector.TypedInjector a, PluginInitInjector.TypedInjector b)
			{
				return !a.Equals(b);
			}

			// Token: 0x0400034B RID: 843
			public Type Type;

			// Token: 0x0400034C RID: 844
			public PluginInitInjector.InjectParameter Injector;
		}
	}
}
