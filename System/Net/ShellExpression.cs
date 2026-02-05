using System;

namespace System.Net
{
	// Token: 0x0200020B RID: 523
	internal struct ShellExpression
	{
		// Token: 0x06001381 RID: 4993 RVA: 0x000667EF File Offset: 0x000649EF
		internal ShellExpression(string pattern)
		{
			this.pattern = null;
			this.match = null;
			this.Parse(pattern);
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00066808 File Offset: 0x00064A08
		internal bool IsMatch(string target)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			for (;;)
			{
				if (!flag)
				{
					if (num2 > target.Length)
					{
						return flag2;
					}
					switch (this.pattern[num])
					{
					case ShellExpression.ShExpTokens.End:
						if (num2 == target.Length)
						{
							goto Block_10;
						}
						flag = true;
						break;
					case ShellExpression.ShExpTokens.Start:
						if (num2 != 0)
						{
							return flag2;
						}
						this.match[num++] = 0;
						break;
					case ShellExpression.ShExpTokens.AugmentedQuestion:
						if (num2 == target.Length || target[num2] == '.')
						{
							this.match[num++] = num2;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.AugmentedAsterisk:
						if (num2 == target.Length || target[num2] == '.')
						{
							flag = true;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.AugmentedDot:
						if (num2 == target.Length)
						{
							this.match[num++] = num2;
						}
						else if (target[num2] == '.')
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						else
						{
							flag = true;
						}
						break;
					case ShellExpression.ShExpTokens.Question:
						if (num2 == target.Length)
						{
							flag = true;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.Asterisk:
						num2 = (this.match[num++] = target.Length);
						break;
					default:
						if (num2 < target.Length && this.pattern[num] == (ShellExpression.ShExpTokens)char.ToLowerInvariant(target[num2]))
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						else
						{
							flag = true;
						}
						break;
					}
				}
				else
				{
					switch (this.pattern[--num])
					{
					case ShellExpression.ShExpTokens.End:
					case ShellExpression.ShExpTokens.Start:
						return flag2;
					case ShellExpression.ShExpTokens.AugmentedQuestion:
					case ShellExpression.ShExpTokens.Asterisk:
						if (this.match[num] != this.match[num - 1])
						{
							int[] array = this.match;
							int num3 = num++;
							int num4 = array[num3] - 1;
							array[num3] = num4;
							num2 = num4;
							flag = false;
						}
						break;
					}
				}
			}
			Block_10:
			flag2 = true;
			return flag2;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00066A2C File Offset: 0x00064C2C
		private void Parse(string patString)
		{
			this.pattern = new ShellExpression.ShExpTokens[patString.Length + 2];
			this.match = null;
			int num = 0;
			this.pattern[num++] = ShellExpression.ShExpTokens.Start;
			for (int i = 0; i < patString.Length; i++)
			{
				char c = patString[i];
				if (c != '*')
				{
					if (c != '?')
					{
						if (c != '^')
						{
							this.pattern[num++] = (ShellExpression.ShExpTokens)char.ToLowerInvariant(patString[i]);
						}
						else
						{
							if (i >= patString.Length - 1)
							{
								this.pattern = null;
								if (Logging.On)
								{
									Logging.PrintWarning(Logging.Web, SR.GetString("net_log_shell_expression_pattern_format_warning", new object[] { patString }));
								}
								throw new FormatException(SR.GetString("net_format_shexp", new object[] { patString }));
							}
							i++;
							char c2 = patString[i];
							if (c2 != '*')
							{
								if (c2 != '.')
								{
									if (c2 != '?')
									{
										this.pattern = null;
										if (Logging.On)
										{
											Logging.PrintWarning(Logging.Web, SR.GetString("net_log_shell_expression_pattern_format_warning", new object[] { patString }));
										}
										throw new FormatException(SR.GetString("net_format_shexp", new object[] { patString }));
									}
									this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedQuestion;
								}
								else
								{
									this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedDot;
								}
							}
							else
							{
								this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedAsterisk;
							}
						}
					}
					else
					{
						this.pattern[num++] = ShellExpression.ShExpTokens.Question;
					}
				}
				else
				{
					this.pattern[num++] = ShellExpression.ShExpTokens.Asterisk;
				}
			}
			this.pattern[num++] = ShellExpression.ShExpTokens.End;
			this.match = new int[num];
		}

		// Token: 0x0400155E RID: 5470
		private ShellExpression.ShExpTokens[] pattern;

		// Token: 0x0400155F RID: 5471
		private int[] match;

		// Token: 0x02000759 RID: 1881
		private enum ShExpTokens
		{
			// Token: 0x04003218 RID: 12824
			Asterisk = -1,
			// Token: 0x04003219 RID: 12825
			Question = -2,
			// Token: 0x0400321A RID: 12826
			AugmentedDot = -3,
			// Token: 0x0400321B RID: 12827
			AugmentedAsterisk = -4,
			// Token: 0x0400321C RID: 12828
			AugmentedQuestion = -5,
			// Token: 0x0400321D RID: 12829
			Start = -6,
			// Token: 0x0400321E RID: 12830
			End = -7
		}
	}
}
