using System;
using System.Globalization;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000CA RID: 202
	internal sealed class ErrorHandling
	{
		// Token: 0x0600067F RID: 1663 RVA: 0x0001E9F8 File Offset: 0x0001CBF8
		public ErrorHandling(GlobalSymbolContext globalSymbols)
		{
			this._userStringBuilder = new UserStringBuilder(globalSymbols);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001EA0C File Offset: 0x0001CC0C
		public RuntimeBinderException Error(ErrorCode id, params ErrArg[] args)
		{
			string[] array = new string[args.Length];
			int[] array2 = new int[args.Length];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < args.Length; i++)
			{
				ErrArg errArg = args[i];
				if ((errArg.eaf & ErrArgFlags.NoStr) == ErrArgFlags.None)
				{
					bool flag;
					if (!this._userStringBuilder.ErrArgToString(out array[num], errArg, out flag) && errArg.eak == ErrArgKind.Int)
					{
						array[num] = errArg.n.ToString(CultureInfo.InvariantCulture);
					}
					num++;
					int num4;
					if (!flag || (errArg.eaf & ErrArgFlags.Unique) == ErrArgFlags.None)
					{
						num4 = -1;
					}
					else
					{
						num4 = i;
						num3++;
					}
					array2[num2] = num4;
					num2++;
				}
			}
			int num5 = num;
			if (num3 > 1)
			{
				string[] array3 = new string[num5];
				Array.Copy(array, 0, array3, 0, num5);
				for (int j = 0; j < num5; j++)
				{
					if (array2[j] >= 0 && !(array3[j] != array[j]))
					{
						ErrArg errArg2 = args[array2[j]];
						Symbol symbol = null;
						CType ctype = null;
						ErrArgKind errArgKind = errArg2.eak;
						if (errArgKind <= ErrArgKind.Type)
						{
							if (errArgKind != ErrArgKind.Sym)
							{
								if (errArgKind != ErrArgKind.Type)
								{
									goto IL_0235;
								}
								ctype = errArg2.pType;
							}
							else
							{
								symbol = errArg2.sym;
							}
						}
						else if (errArgKind != ErrArgKind.SymWithType)
						{
							if (errArgKind != ErrArgKind.MethWithInst)
							{
								goto IL_0235;
							}
							symbol = errArg2.mpwiMemo.sym;
						}
						else
						{
							symbol = errArg2.swtMemo.sym;
						}
						bool flag2 = false;
						for (int k = j + 1; k < num5; k++)
						{
							if (array2[k] >= 0 && !(array[j] != array[k]))
							{
								if (array3[k] != array[k])
								{
									flag2 = true;
								}
								else
								{
									ErrArg errArg3 = args[array2[k]];
									Symbol symbol2 = null;
									CType ctype2 = null;
									errArgKind = errArg3.eak;
									if (errArgKind <= ErrArgKind.Type)
									{
										if (errArgKind != ErrArgKind.Sym)
										{
											if (errArgKind != ErrArgKind.Type)
											{
												goto IL_0219;
											}
											ctype2 = errArg3.pType;
										}
										else
										{
											symbol2 = errArg3.sym;
										}
									}
									else if (errArgKind != ErrArgKind.SymWithType)
									{
										if (errArgKind != ErrArgKind.MethWithInst)
										{
											goto IL_0219;
										}
										symbol2 = errArg3.mpwiMemo.sym;
									}
									else
									{
										symbol2 = errArg3.swtMemo.sym;
									}
									if (symbol2 != symbol || ctype2 != ctype || flag2)
									{
										array3[k] = array[k];
										flag2 = true;
									}
								}
							}
							IL_0219:;
						}
						if (flag2)
						{
							array3[j] = array[j];
						}
					}
					IL_0235:;
				}
				array = array3;
			}
			return new RuntimeBinderException(string.Format(CultureInfo.InvariantCulture, ErrorFacts.GetMessage(id), array));
		}

		// Token: 0x04000626 RID: 1574
		private readonly UserStringBuilder _userStringBuilder;
	}
}
