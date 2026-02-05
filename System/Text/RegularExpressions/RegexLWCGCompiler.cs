using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000694 RID: 1684
	internal class RegexLWCGCompiler : RegexCompiler
	{
		// Token: 0x06003EA4 RID: 16036 RVA: 0x001049DB File Offset: 0x00102BDB
		internal RegexLWCGCompiler()
		{
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x001049E4 File Offset: 0x00102BE4
		internal RegexRunnerFactory FactoryInstanceFromCode(RegexCode code, RegexOptions options)
		{
			this._code = code;
			this._codes = code._codes;
			this._strings = code._strings;
			this._fcPrefix = code._fcPrefix;
			this._bmPrefix = code._bmPrefix;
			this._anchors = code._anchors;
			this._trackcount = code._trackcount;
			this._options = options;
			string text = Interlocked.Increment(ref RegexLWCGCompiler._regexCount).ToString(CultureInfo.InvariantCulture);
			DynamicMethod dynamicMethod = this.DefineDynamicMethod("Go" + text, null, typeof(CompiledRegexRunner));
			base.GenerateGo();
			DynamicMethod dynamicMethod2 = this.DefineDynamicMethod("FindFirstChar" + text, typeof(bool), typeof(CompiledRegexRunner));
			base.GenerateFindFirstChar();
			DynamicMethod dynamicMethod3 = this.DefineDynamicMethod("InitTrackCount" + text, null, typeof(CompiledRegexRunner));
			base.GenerateInitTrackCount();
			return new CompiledRegexRunnerFactory(dynamicMethod, dynamicMethod2, dynamicMethod3);
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x00104ADC File Offset: 0x00102CDC
		internal DynamicMethod DefineDynamicMethod(string methname, Type returntype, Type hostType)
		{
			MethodAttributes methodAttributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static;
			CallingConventions callingConventions = CallingConventions.Standard;
			DynamicMethod dynamicMethod = new DynamicMethod(methname, methodAttributes, callingConventions, returntype, RegexLWCGCompiler._paramTypes, hostType, false);
			this._ilg = dynamicMethod.GetILGenerator();
			return dynamicMethod;
		}

		// Token: 0x04002DB6 RID: 11702
		private static int _regexCount = 0;

		// Token: 0x04002DB7 RID: 11703
		private static Type[] _paramTypes = new Type[] { typeof(RegexRunner) };
	}
}
