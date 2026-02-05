using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Cecil;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200006B RID: 107
	public class InlineSignature : ICallSiteGenerator
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000A7B3 File Offset: 0x000089B3
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000A7BB File Offset: 0x000089BB
		public bool HasThis { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000A7C4 File Offset: 0x000089C4
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000A7CC File Offset: 0x000089CC
		public bool ExplicitThis { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000A7D5 File Offset: 0x000089D5
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000A7DD File Offset: 0x000089DD
		public CallingConvention CallingConvention { get; set; } = CallingConvention.Winapi;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000A7E6 File Offset: 0x000089E6
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000A7EE File Offset: 0x000089EE
		public List<object> Parameters { get; set; } = new List<object>();

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000A7F7 File Offset: 0x000089F7
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000A7FF File Offset: 0x000089FF
		public object ReturnType { get; set; } = typeof(void);

		// Token: 0x060001E3 RID: 483 RVA: 0x0000A808 File Offset: 0x00008A08
		public override string ToString()
		{
			Type type = this.ReturnType as Type;
			string text;
			if (type == null)
			{
				object returnType = this.ReturnType;
				text = ((returnType != null) ? returnType.ToString() : null);
			}
			else
			{
				text = type.FullDescription();
			}
			return text + " (" + this.Parameters.Join(delegate(object p)
			{
				Type type2 = p as Type;
				if (type2 != null)
				{
					return type2.FullDescription();
				}
				if (p == null)
				{
					return null;
				}
				return p.ToString();
			}, ", ") + ")";
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000A87C File Offset: 0x00008A7C
		internal static TypeReference GetTypeReference(ModuleDefinition module, object param)
		{
			Type type = param as Type;
			if (type != null)
			{
				return module.ImportReference(type);
			}
			InlineSignature inlineSignature = param as InlineSignature;
			if (inlineSignature != null)
			{
				return inlineSignature.ToFunctionPointer(module);
			}
			InlineSignature.ModifierType modifierType = param as InlineSignature.ModifierType;
			if (modifierType == null)
			{
				throw new NotSupportedException(string.Format("Unsupported inline signature parameter type: {0} ({1})", param, (param != null) ? param.GetType().FullDescription() : null));
			}
			return modifierType.ToTypeReference(module);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000A8E4 File Offset: 0x00008AE4
		CallSite ICallSiteGenerator.ToCallSite(ModuleDefinition module)
		{
			CallSite callSite = new CallSite(InlineSignature.GetTypeReference(module, this.ReturnType))
			{
				HasThis = this.HasThis,
				ExplicitThis = this.ExplicitThis,
				CallingConvention = (MethodCallingConvention)((byte)this.CallingConvention - 1)
			};
			foreach (object obj in this.Parameters)
			{
				callSite.Parameters.Add(new ParameterDefinition(InlineSignature.GetTypeReference(module, obj)));
			}
			return callSite;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000A984 File Offset: 0x00008B84
		private FunctionPointerType ToFunctionPointer(ModuleDefinition module)
		{
			FunctionPointerType functionPointerType = new FunctionPointerType
			{
				ReturnType = InlineSignature.GetTypeReference(module, this.ReturnType),
				HasThis = this.HasThis,
				ExplicitThis = this.ExplicitThis,
				CallingConvention = (MethodCallingConvention)((byte)this.CallingConvention - 1)
			};
			foreach (object obj in this.Parameters)
			{
				functionPointerType.Parameters.Add(new ParameterDefinition(InlineSignature.GetTypeReference(module, obj)));
			}
			return functionPointerType;
		}

		// Token: 0x0200006C RID: 108
		public class ModifierType
		{
			// Token: 0x060001E8 RID: 488 RVA: 0x0000AA54 File Offset: 0x00008C54
			public override string ToString()
			{
				string[] array = new string[6];
				int num = 0;
				Type type = this.Type as Type;
				string text;
				if (type == null)
				{
					object type2 = this.Type;
					text = ((type2 != null) ? type2.ToString() : null);
				}
				else
				{
					text = type.FullDescription();
				}
				array[num] = text;
				array[1] = " mod";
				array[2] = (this.IsOptional ? "opt" : "req");
				array[3] = "(";
				int num2 = 4;
				Type modifier = this.Modifier;
				array[num2] = ((modifier != null) ? modifier.FullDescription() : null);
				array[5] = ")";
				return string.Concat(array);
			}

			// Token: 0x060001E9 RID: 489 RVA: 0x0000AADC File Offset: 0x00008CDC
			internal TypeReference ToTypeReference(ModuleDefinition module)
			{
				if (this.IsOptional)
				{
					return new OptionalModifierType(module.ImportReference(this.Modifier), InlineSignature.GetTypeReference(module, this.Type));
				}
				return new RequiredModifierType(module.ImportReference(this.Modifier), InlineSignature.GetTypeReference(module, this.Type));
			}

			// Token: 0x0400012C RID: 300
			public bool IsOptional;

			// Token: 0x0400012D RID: 301
			public Type Modifier;

			// Token: 0x0400012E RID: 302
			public object Type;
		}
	}
}
