using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;

namespace IPA.Logging
{
	// Token: 0x02000030 RID: 48
	internal class StdoutInterceptor : TextWriter
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004E29 File Offset: 0x00003029
		public override Encoding Encoding
		{
			get
			{
				return Encoding.Default;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004E30 File Offset: 0x00003030
		public override void Write(char value)
		{
			this.Write(value.ToString());
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004E40 File Offset: 0x00003040
		public override void Write(string value)
		{
			object obj = this.bufferLock;
			lock (obj)
			{
				this.lineBuffer += value;
				string[] parts = this.lineBuffer.Split(new string[]
				{
					Environment.NewLine,
					"\n",
					"\r"
				}, StringSplitOptions.None);
				for (int i = 0; i < parts.Length; i++)
				{
					if (i + 1 == parts.Length)
					{
						this.lineBuffer = parts[i];
					}
					else
					{
						string str = parts[i];
						if (!string.IsNullOrEmpty(str))
						{
							if (!this.isStdErr && WinConsole.IsInitialized)
							{
								str = StdoutInterceptor.ConsoleColorToForegroundSet(this.currentColor) + str;
							}
							if (this.isStdErr)
							{
								Logger.stdout.Error(str);
							}
							else
							{
								Logger.stdout.Info(str);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004F2C File Offset: 0x0000312C
		private static string ConsoleColorToForegroundSet(ConsoleColor col)
		{
			if (!WinConsole.UseVTEscapes)
			{
				return "";
			}
			string code = "0";
			switch (col)
			{
			case ConsoleColor.Black:
				code = "30";
				break;
			case ConsoleColor.DarkBlue:
				code = "34";
				break;
			case ConsoleColor.DarkGreen:
				code = "32";
				break;
			case ConsoleColor.DarkCyan:
				code = "36";
				break;
			case ConsoleColor.DarkRed:
				code = "31";
				break;
			case ConsoleColor.DarkMagenta:
				code = "35";
				break;
			case ConsoleColor.DarkYellow:
				code = "33";
				break;
			case ConsoleColor.Gray:
				code = "37";
				break;
			case ConsoleColor.DarkGray:
				code = "90";
				break;
			case ConsoleColor.Blue:
				code = "94";
				break;
			case ConsoleColor.Green:
				code = "92";
				break;
			case ConsoleColor.Cyan:
				code = "96";
				break;
			case ConsoleColor.Red:
				code = "91";
				break;
			case ConsoleColor.Magenta:
				code = "95";
				break;
			case ConsoleColor.Yellow:
				code = "93";
				break;
			case ConsoleColor.White:
				code = "97";
				break;
			}
			return "\u001b[" + code + "m";
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005024 File Offset: 0x00003224
		public static void Intercept()
		{
			if (!StdoutInterceptor.usingInterceptor)
			{
				StdoutInterceptor.usingInterceptor = true;
				if (StdoutInterceptor.harmony == null)
				{
					StdoutInterceptor.harmony = new Harmony("BSIPA Console Redirector Patcher");
				}
				if (StdoutInterceptor.stdoutInterceptor == null)
				{
					StdoutInterceptor.stdoutInterceptor = new StdoutInterceptor();
				}
				if (StdoutInterceptor.stderrInterceptor == null)
				{
					StdoutInterceptor.stderrInterceptor = new StdoutInterceptor
					{
						isStdErr = true
					};
				}
				StdoutInterceptor.RedirectConsole();
				StdoutInterceptor.ConsoleHarmonyPatches.Patch(StdoutInterceptor.harmony);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000508C File Offset: 0x0000328C
		public static void RedirectConsole()
		{
			if (StdoutInterceptor.usingInterceptor)
			{
				Console.SetOut(StdoutInterceptor.stdoutInterceptor);
				Console.SetError(StdoutInterceptor.stderrInterceptor);
			}
		}

		// Token: 0x0400005B RID: 91
		private bool isStdErr;

		// Token: 0x0400005C RID: 92
		private string lineBuffer = "";

		// Token: 0x0400005D RID: 93
		private readonly object bufferLock = new object();

		// Token: 0x0400005E RID: 94
		private const ConsoleColor defaultColor = ConsoleColor.Gray;

		// Token: 0x0400005F RID: 95
		private ConsoleColor currentColor = ConsoleColor.Gray;

		// Token: 0x04000060 RID: 96
		private static StdoutInterceptor stdoutInterceptor;

		// Token: 0x04000061 RID: 97
		private static StdoutInterceptor stderrInterceptor;

		// Token: 0x04000062 RID: 98
		private static Harmony harmony;

		// Token: 0x04000063 RID: 99
		private static bool usingInterceptor;

		// Token: 0x020000E1 RID: 225
		private static class ConsoleHarmonyPatches
		{
			// Token: 0x060004E9 RID: 1257 RVA: 0x000164D0 File Offset: 0x000146D0
			public static void Patch(Harmony harmony)
			{
				Type typeFromHandle = typeof(Console);
				MethodInfo resetColor = typeFromHandle.GetMethod("ResetColor");
				PropertyInfo property = typeFromHandle.GetProperty("ForegroundColor");
				MethodInfo setFg = ((property != null) ? property.GetSetMethod() : null);
				MethodInfo getFg = ((property != null) ? property.GetGetMethod() : null);
				if (resetColor != null)
				{
					harmony.Patch(resetColor, null, null, new HarmonyMethod(typeof(StdoutInterceptor.ConsoleHarmonyPatches), "PatchResetColor", null), null);
				}
				if (property != null)
				{
					harmony.Patch(setFg, null, null, new HarmonyMethod(typeof(StdoutInterceptor.ConsoleHarmonyPatches), "PatchSetForegroundColor", null), null);
					harmony.Patch(getFg, null, null, new HarmonyMethod(typeof(StdoutInterceptor.ConsoleHarmonyPatches), "PatchGetForegroundColor", null), null);
				}
			}

			// Token: 0x060004EA RID: 1258 RVA: 0x0001658A File Offset: 0x0001478A
			public static ConsoleColor GetColor()
			{
				return StdoutInterceptor.stdoutInterceptor.currentColor;
			}

			// Token: 0x060004EB RID: 1259 RVA: 0x00016596 File Offset: 0x00014796
			public static void SetColor(ConsoleColor col)
			{
				StdoutInterceptor.stdoutInterceptor.currentColor = col;
			}

			// Token: 0x060004EC RID: 1260 RVA: 0x000165A3 File Offset: 0x000147A3
			public static void ResetColor()
			{
				StdoutInterceptor.stdoutInterceptor.currentColor = ConsoleColor.Gray;
			}

			// Token: 0x060004ED RID: 1261 RVA: 0x000165B0 File Offset: 0x000147B0
			public static IEnumerable<CodeInstruction> PatchGetForegroundColor(IEnumerable<CodeInstruction> _)
			{
				MethodInfo getColorM = typeof(StdoutInterceptor.ConsoleHarmonyPatches).GetMethod("GetColor");
				return new CodeInstruction[]
				{
					new CodeInstruction(OpCodes.Tailcall, null),
					new CodeInstruction(OpCodes.Call, getColorM),
					new CodeInstruction(OpCodes.Ret, null)
				};
			}

			// Token: 0x060004EE RID: 1262 RVA: 0x00016604 File Offset: 0x00014804
			public static IEnumerable<CodeInstruction> PatchSetForegroundColor(IEnumerable<CodeInstruction> _)
			{
				MethodInfo setColorM = typeof(StdoutInterceptor.ConsoleHarmonyPatches).GetMethod("SetColor");
				return new CodeInstruction[]
				{
					new CodeInstruction(OpCodes.Ldarg_0, null),
					new CodeInstruction(OpCodes.Tailcall, null),
					new CodeInstruction(OpCodes.Call, setColorM),
					new CodeInstruction(OpCodes.Ret, null)
				};
			}

			// Token: 0x060004EF RID: 1263 RVA: 0x00016664 File Offset: 0x00014864
			public static IEnumerable<CodeInstruction> PatchResetColor(IEnumerable<CodeInstruction> _)
			{
				MethodInfo resetColor = typeof(StdoutInterceptor.ConsoleHarmonyPatches).GetMethod("ResetColor");
				return new CodeInstruction[]
				{
					new CodeInstruction(OpCodes.Tailcall, null),
					new CodeInstruction(OpCodes.Call, resetColor),
					new CodeInstruction(OpCodes.Ret, null)
				};
			}
		}
	}
}
