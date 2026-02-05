using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IPA
{
	// Token: 0x02000002 RID: 2
	public class Arguments
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private Arguments(string[] args)
		{
			this.toParse = args.Skip(1).ToArray<string>();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A4 File Offset: 0x000002A4
		public Arguments Flags(params ArgumentFlag[] toAdd)
		{
			foreach (ArgumentFlag argumentFlag in toAdd)
			{
				this.AddFlag(argumentFlag);
			}
			return this;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020CD File Offset: 0x000002CD
		public void AddFlag(ArgumentFlag toAdd)
		{
			if (this.toParse == null)
			{
				throw new InvalidOperationException();
			}
			this.flagObjects.Add(toAdd);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020EC File Offset: 0x000002EC
		public void Process()
		{
			foreach (string text in this.toParse)
			{
				if (text.StartsWith("--"))
				{
					string text2 = text.Substring(2);
					string text3 = null;
					if (text2.Contains('='))
					{
						string[] array2 = text2.Split(new char[] { '=' });
						text2 = array2[0];
						text3 = string.Join("=", array2, 1, array2.Length - 1);
					}
					this.longFlags.Add(text2, text3);
				}
				else if (text.StartsWith("-"))
				{
					string text4 = text.Substring(1);
					StringBuilder stringBuilder = new StringBuilder();
					bool flag = false;
					bool flag2 = false;
					char c = ' ';
					foreach (char c2 in text4)
					{
						if (!flag)
						{
							if (c2 == '=')
							{
								flag = true;
							}
							else
							{
								c = c2;
								this.flags.Add(c2, null);
							}
						}
						else
						{
							if (!flag2)
							{
								if (c2 == ',')
								{
									flag = false;
									this.flags[c] = stringBuilder.ToString();
									stringBuilder = new StringBuilder();
									goto IL_0110;
								}
								if (c2 == '\\')
								{
									flag2 = true;
									goto IL_0110;
								}
							}
							stringBuilder.Append(c2);
						}
						IL_0110:;
					}
					if (flag)
					{
						this.flags[c] = stringBuilder.ToString();
						stringBuilder = new StringBuilder();
					}
				}
				else
				{
					this.positional.Add(text);
				}
			}
			this.toParse = null;
			using (List<ArgumentFlag>.Enumerator enumerator = this.flagObjects.GetEnumerator())
			{
				IL_0232:
				while (enumerator.MoveNext())
				{
					ArgumentFlag argumentFlag = enumerator.Current;
					foreach (char c3 in argumentFlag.ShortFlags)
					{
						if (argumentFlag.exists_ = this.HasFlag(c3))
						{
							argumentFlag.value_ = this.GetFlagValue(c3);
							goto IL_0232;
						}
					}
					foreach (string text6 in argumentFlag.LongFlags)
					{
						if (argumentFlag.exists_ = this.HasLongFlag(text6))
						{
							argumentFlag.value_ = this.GetLongFlagValue(text6);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002370 File Offset: 0x00000570
		public bool HasLongFlag(string flag)
		{
			return this.longFlags.ContainsKey(flag);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000237E File Offset: 0x0000057E
		public bool HasFlag(char flag)
		{
			return this.flags.ContainsKey(flag);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000238C File Offset: 0x0000058C
		public string GetLongFlagValue(string flag)
		{
			return this.longFlags[flag];
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000239A File Offset: 0x0000059A
		public string GetFlagValue(char flag)
		{
			return this.flags[flag];
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023A8 File Offset: 0x000005A8
		public void PrintHelp()
		{
			string fileName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ArgumentFlag argumentFlag in this.flagObjects)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string text = "{2}{0}{3}{1}";
				object[] array = new object[4];
				array[0] = string.Join(", ", argumentFlag.ShortFlags.Select((char s) => string.Format("-{0}", s)).Concat(argumentFlag.LongFlags.Select((string s) => "--" + s)));
				array[1] = Environment.NewLine;
				array[2] = "    ";
				array[3] = ((argumentFlag.ValueString != null) ? ("=" + argumentFlag.ValueString) : "");
				stringBuilder2.AppendFormat(text, array);
				stringBuilder.AppendFormat("{2}{2}{0}{1}", argumentFlag.DocString, Environment.NewLine, "    ");
			}
			Console.Write("usage:\n{2}{0} [FLAGS] [ARGUMENTS]\n\nflags:\n{1}", fileName, stringBuilder, "    ");
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000024E4 File Offset: 0x000006E4
		public IReadOnlyList<string> PositionalArgs
		{
			get
			{
				return this.positional;
			}
		}

		// Token: 0x04000001 RID: 1
		public static readonly Arguments CmdLine = new Arguments(Environment.GetCommandLineArgs());

		// Token: 0x04000002 RID: 2
		private readonly List<string> positional = new List<string>();

		// Token: 0x04000003 RID: 3
		private readonly Dictionary<string, string> longFlags = new Dictionary<string, string>();

		// Token: 0x04000004 RID: 4
		private readonly Dictionary<char, string> flags = new Dictionary<char, string>();

		// Token: 0x04000005 RID: 5
		private readonly List<ArgumentFlag> flagObjects = new List<ArgumentFlag>();

		// Token: 0x04000006 RID: 6
		private string[] toParse;
	}
}
