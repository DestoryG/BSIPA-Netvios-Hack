using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x020000A2 RID: 162
	public class Traverse
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000F91A File Offset: 0x0000DB1A
		[MethodImpl(MethodImplOptions.Synchronized)]
		static Traverse()
		{
			if (Traverse.Cache == null)
			{
				Traverse.Cache = new AccessCache();
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000F942 File Offset: 0x0000DB42
		public static Traverse Create(Type type)
		{
			return new Traverse(type);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000F94A File Offset: 0x0000DB4A
		public static Traverse Create<T>()
		{
			return Traverse.Create(typeof(T));
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000F95B File Offset: 0x0000DB5B
		public static Traverse Create(object root)
		{
			return new Traverse(root);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000F963 File Offset: 0x0000DB63
		public static Traverse CreateWithType(string name)
		{
			return new Traverse(AccessTools.TypeByName(name));
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00002AB9 File Offset: 0x00000CB9
		private Traverse()
		{
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000F970 File Offset: 0x0000DB70
		public Traverse(Type type)
		{
			this._type = type;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000F97F File Offset: 0x0000DB7F
		public Traverse(object root)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		private Traverse(object root, MemberInfo info, object[] index)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null) ?? info.GetUnderlyingType();
			this._info = info;
			this._params = index;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000F9D9 File Offset: 0x0000DBD9
		private Traverse(object root, MethodInfo method, object[] parameter)
		{
			this._root = root;
			this._type = method.ReturnType;
			this._method = method;
			this._params = parameter;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000FA04 File Offset: 0x0000DC04
		public object GetValue()
		{
			if (this._info is FieldInfo)
			{
				return ((FieldInfo)this._info).GetValue(this._root);
			}
			if (this._info is PropertyInfo)
			{
				return ((PropertyInfo)this._info).GetValue(this._root, AccessTools.all, null, this._params, CultureInfo.CurrentCulture);
			}
			if (this._method != null)
			{
				return this._method.Invoke(this._root, this._params);
			}
			if (this._root == null && this._type != null)
			{
				return this._type;
			}
			return this._root;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000FAB4 File Offset: 0x0000DCB4
		public T GetValue<T>()
		{
			object value = this.GetValue();
			if (value == null)
			{
				return default(T);
			}
			return (T)((object)value);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000FADB File Offset: 0x0000DCDB
		public object GetValue(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return this._method.Invoke(this._root, arguments);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000FB08 File Offset: 0x0000DD08
		public T GetValue<T>(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return (T)((object)this._method.Invoke(this._root, arguments));
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000FB3C File Offset: 0x0000DD3C
		public Traverse SetValue(object value)
		{
			if (this._info is FieldInfo)
			{
				((FieldInfo)this._info).SetValue(this._root, value, AccessTools.all, null, CultureInfo.CurrentCulture);
			}
			if (this._info is PropertyInfo)
			{
				((PropertyInfo)this._info).SetValue(this._root, value, AccessTools.all, null, this._params, CultureInfo.CurrentCulture);
			}
			if (this._method != null)
			{
				throw new Exception("cannot set value of method " + this._method.FullDescription());
			}
			return this;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000FBD7 File Offset: 0x0000DDD7
		public Type GetValueType()
		{
			if (this._info is FieldInfo)
			{
				return ((FieldInfo)this._info).FieldType;
			}
			if (this._info is PropertyInfo)
			{
				return ((PropertyInfo)this._info).PropertyType;
			}
			return null;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000FC16 File Offset: 0x0000DE16
		private Traverse Resolve()
		{
			if (this._root == null && this._type != null)
			{
				return this;
			}
			return new Traverse(this.GetValue());
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000FC3C File Offset: 0x0000DE3C
		public Traverse Type(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._type == null)
			{
				return new Traverse();
			}
			Type type = AccessTools.Inner(this._type, name);
			if (type == null)
			{
				return new Traverse();
			}
			return new Traverse(type);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000FC90 File Offset: 0x0000DE90
		public Traverse Field(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			FieldInfo fieldInfo = Traverse.Cache.GetFieldInfo(traverse._type, name, AccessCache.MemberType.Any, false);
			if (fieldInfo == null)
			{
				return new Traverse();
			}
			if (!fieldInfo.IsStatic && traverse._root == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, fieldInfo, null);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000FD0C File Offset: 0x0000DF0C
		public Traverse<T> Field<T>(string name)
		{
			return new Traverse<T>(this.Field(name));
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000FD1A File Offset: 0x0000DF1A
		public List<string> Fields()
		{
			return AccessTools.GetFieldNames(this.Resolve()._type);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		public Traverse Property(string name, object[] index = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			PropertyInfo propertyInfo = Traverse.Cache.GetPropertyInfo(traverse._type, name, AccessCache.MemberType.Any, false);
			if (propertyInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, propertyInfo, index);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000FD92 File Offset: 0x0000DF92
		public Traverse<T> Property<T>(string name, object[] index = null)
		{
			return new Traverse<T>(this.Property(name, index));
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000FDA1 File Offset: 0x0000DFA1
		public List<string> Properties()
		{
			return AccessTools.GetPropertyNames(this.Resolve()._type);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
		public Traverse Method(string name, params object[] arguments)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			Type[] types = AccessTools.GetTypes(arguments);
			MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, types, AccessCache.MemberType.Any, false);
			if (methodInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, (MethodInfo)methodInfo, arguments);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000FE28 File Offset: 0x0000E028
		public Traverse Method(string name, Type[] paramTypes, object[] arguments = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, paramTypes, AccessCache.MemberType.Any, false);
			if (methodInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, (MethodInfo)methodInfo, arguments);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000FE94 File Offset: 0x0000E094
		public List<string> Methods()
		{
			return AccessTools.GetMethodNames(this.Resolve()._type);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000FEA6 File Offset: 0x0000E0A6
		public bool FieldExists()
		{
			return this._info != null && this._info is FieldInfo;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000FEC6 File Offset: 0x0000E0C6
		public bool PropertyExists()
		{
			return this._info != null && this._info is PropertyInfo;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000FEE6 File Offset: 0x0000E0E6
		public bool MethodExists()
		{
			return this._method != null;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000FEF4 File Offset: 0x0000E0F4
		public bool TypeExists()
		{
			return this._type != null;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000FF04 File Offset: 0x0000E104
		public static void IterateFields(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f));
			});
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000FF44 File Offset: 0x0000E144
		public static void IterateFields(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000FF90 File Offset: 0x0000E190
		public static void IterateFields(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000FFDC File Offset: 0x0000E1DC
		public static void IterateProperties(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null));
			});
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0001001C File Offset: 0x0000E21C
		public static void IterateProperties(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00010068 File Offset: 0x0000E268
		public static void IterateProperties(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000100B1 File Offset: 0x0000E2B1
		public override string ToString()
		{
			MethodBase methodBase = this._method ?? this.GetValue();
			if (methodBase == null)
			{
				return null;
			}
			return methodBase.ToString();
		}

		// Token: 0x040001C8 RID: 456
		private static readonly AccessCache Cache;

		// Token: 0x040001C9 RID: 457
		private readonly Type _type;

		// Token: 0x040001CA RID: 458
		private readonly object _root;

		// Token: 0x040001CB RID: 459
		private readonly MemberInfo _info;

		// Token: 0x040001CC RID: 460
		private readonly MethodBase _method;

		// Token: 0x040001CD RID: 461
		private readonly object[] _params;

		// Token: 0x040001CE RID: 462
		public static Action<Traverse, Traverse> CopyFields = delegate(Traverse from, Traverse to)
		{
			to.SetValue(from.GetValue());
		};
	}
}
