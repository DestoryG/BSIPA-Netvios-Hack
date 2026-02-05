using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Permissions;
using System.Text;

namespace System.ComponentModel
{
	// Token: 0x0200058E RID: 1422
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class MaskedTextProvider : ICloneable
	{
		// Token: 0x06003469 RID: 13417 RVA: 0x000E4D2B File Offset: 0x000E2F2B
		public MaskedTextProvider(string mask)
			: this(mask, null, true, '_', '\0', false)
		{
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x000E4D3A File Offset: 0x000E2F3A
		public MaskedTextProvider(string mask, bool restrictToAscii)
			: this(mask, null, true, '_', '\0', restrictToAscii)
		{
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000E4D49 File Offset: 0x000E2F49
		public MaskedTextProvider(string mask, CultureInfo culture)
			: this(mask, culture, true, '_', '\0', false)
		{
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000E4D58 File Offset: 0x000E2F58
		public MaskedTextProvider(string mask, CultureInfo culture, bool restrictToAscii)
			: this(mask, culture, true, '_', '\0', restrictToAscii)
		{
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x000E4D67 File Offset: 0x000E2F67
		public MaskedTextProvider(string mask, char passwordChar, bool allowPromptAsInput)
			: this(mask, null, allowPromptAsInput, '_', passwordChar, false)
		{
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x000E4D76 File Offset: 0x000E2F76
		public MaskedTextProvider(string mask, CultureInfo culture, char passwordChar, bool allowPromptAsInput)
			: this(mask, culture, allowPromptAsInput, '_', passwordChar, false)
		{
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000E4D88 File Offset: 0x000E2F88
		public MaskedTextProvider(string mask, CultureInfo culture, bool allowPromptAsInput, char promptChar, char passwordChar, bool restrictToAscii)
		{
			if (string.IsNullOrEmpty(mask))
			{
				throw new ArgumentException(SR.GetString("MaskedTextProviderMaskNullOrEmpty"), "mask");
			}
			foreach (char c in mask)
			{
				if (!MaskedTextProvider.IsPrintableChar(c))
				{
					throw new ArgumentException(SR.GetString("MaskedTextProviderMaskInvalidChar"));
				}
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			this.flagState = default(BitVector32);
			this.mask = mask;
			this.promptChar = promptChar;
			this.passwordChar = passwordChar;
			if (culture.IsNeutralCulture)
			{
				foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
				{
					if (culture.Equals(cultureInfo.Parent))
					{
						this.culture = cultureInfo;
						break;
					}
				}
				if (this.culture == null)
				{
					this.culture = CultureInfo.InvariantCulture;
				}
			}
			else
			{
				this.culture = culture;
			}
			if (!this.culture.IsReadOnly)
			{
				this.culture = CultureInfo.ReadOnly(this.culture);
			}
			this.flagState[MaskedTextProvider.ALLOW_PROMPT_AS_INPUT] = allowPromptAsInput;
			this.flagState[MaskedTextProvider.ASCII_ONLY] = restrictToAscii;
			this.flagState[MaskedTextProvider.INCLUDE_PROMPT] = false;
			this.flagState[MaskedTextProvider.INCLUDE_LITERALS] = true;
			this.flagState[MaskedTextProvider.RESET_ON_PROMPT] = true;
			this.flagState[MaskedTextProvider.SKIP_SPACE] = true;
			this.flagState[MaskedTextProvider.RESET_ON_LITERALS] = true;
			this.Initialize();
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000E4F10 File Offset: 0x000E3110
		private void Initialize()
		{
			this.testString = new StringBuilder();
			this.stringDescriptor = new List<MaskedTextProvider.CharDescriptor>();
			MaskedTextProvider.CaseConversion caseConversion = MaskedTextProvider.CaseConversion.None;
			bool flag = false;
			int num = 0;
			MaskedTextProvider.CharType charType = MaskedTextProvider.CharType.Literal;
			string text = string.Empty;
			int i = 0;
			while (i < this.mask.Length)
			{
				char c = this.mask[i];
				if (!flag)
				{
					if (c <= 'C')
					{
						switch (c)
						{
						case '#':
							goto IL_019E;
						case '$':
							text = this.culture.NumberFormat.CurrencySymbol;
							charType = MaskedTextProvider.CharType.Separator;
							goto IL_01BE;
						case '%':
							goto IL_01B8;
						case '&':
							break;
						default:
							switch (c)
							{
							case ',':
								text = this.culture.NumberFormat.NumberGroupSeparator;
								charType = MaskedTextProvider.CharType.Separator;
								goto IL_01BE;
							case '-':
								goto IL_01B8;
							case '.':
								text = this.culture.NumberFormat.NumberDecimalSeparator;
								charType = MaskedTextProvider.CharType.Separator;
								goto IL_01BE;
							case '/':
								text = this.culture.DateTimeFormat.DateSeparator;
								charType = MaskedTextProvider.CharType.Separator;
								goto IL_01BE;
							case '0':
								break;
							default:
								switch (c)
								{
								case '9':
								case '?':
								case 'C':
									goto IL_019E;
								case ':':
									text = this.culture.DateTimeFormat.TimeSeparator;
									charType = MaskedTextProvider.CharType.Separator;
									goto IL_01BE;
								case ';':
								case '=':
								case '@':
								case 'B':
									goto IL_01B8;
								case '<':
									caseConversion = MaskedTextProvider.CaseConversion.ToLower;
									goto IL_022A;
								case '>':
									caseConversion = MaskedTextProvider.CaseConversion.ToUpper;
									goto IL_022A;
								case 'A':
									break;
								default:
									goto IL_01B8;
								}
								break;
							}
							break;
						}
					}
					else if (c <= '\\')
					{
						if (c != 'L')
						{
							if (c != '\\')
							{
								goto IL_01B8;
							}
							flag = true;
							charType = MaskedTextProvider.CharType.Literal;
							goto IL_022A;
						}
					}
					else
					{
						if (c == 'a')
						{
							goto IL_019E;
						}
						if (c != '|')
						{
							goto IL_01B8;
						}
						caseConversion = MaskedTextProvider.CaseConversion.None;
						goto IL_022A;
					}
					this.requiredEditChars++;
					c = this.promptChar;
					charType = MaskedTextProvider.CharType.EditRequired;
					goto IL_01BE;
					IL_019E:
					this.optionalEditChars++;
					c = this.promptChar;
					charType = MaskedTextProvider.CharType.EditOptional;
					goto IL_01BE;
					IL_01B8:
					charType = MaskedTextProvider.CharType.Literal;
					goto IL_01BE;
				}
				flag = false;
				goto IL_01BE;
				IL_022A:
				i++;
				continue;
				IL_01BE:
				MaskedTextProvider.CharDescriptor charDescriptor = new MaskedTextProvider.CharDescriptor(i, charType);
				if (MaskedTextProvider.IsEditPosition(charDescriptor))
				{
					charDescriptor.CaseConversion = caseConversion;
				}
				if (charType != MaskedTextProvider.CharType.Separator)
				{
					text = c.ToString();
				}
				foreach (char c2 in text)
				{
					this.testString.Append(c2);
					this.stringDescriptor.Add(charDescriptor);
					num++;
				}
				goto IL_022A;
			}
			this.testString.Capacity = this.testString.Length;
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x000E5175 File Offset: 0x000E3375
		public bool AllowPromptAsInput
		{
			get
			{
				return this.flagState[MaskedTextProvider.ALLOW_PROMPT_AS_INPUT];
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003472 RID: 13426 RVA: 0x000E5187 File Offset: 0x000E3387
		public int AssignedEditPositionCount
		{
			get
			{
				return this.assignedCharCount;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x000E518F File Offset: 0x000E338F
		public int AvailableEditPositionCount
		{
			get
			{
				return this.EditPositionCount - this.assignedCharCount;
			}
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x000E51A0 File Offset: 0x000E33A0
		public object Clone()
		{
			Type type = base.GetType();
			MaskedTextProvider maskedTextProvider;
			if (type == MaskedTextProvider.maskTextProviderType)
			{
				maskedTextProvider = new MaskedTextProvider(this.Mask, this.Culture, this.AllowPromptAsInput, this.PromptChar, this.PasswordChar, this.AsciiOnly);
			}
			else
			{
				object[] array = new object[] { this.Mask, this.Culture, this.AllowPromptAsInput, this.PromptChar, this.PasswordChar, this.AsciiOnly };
				maskedTextProvider = SecurityUtils.SecureCreateInstance(type, array) as MaskedTextProvider;
			}
			maskedTextProvider.ResetOnPrompt = false;
			maskedTextProvider.ResetOnSpace = false;
			maskedTextProvider.SkipLiterals = false;
			for (int i = 0; i < this.testString.Length; i++)
			{
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
				if (MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned)
				{
					maskedTextProvider.Replace(this.testString[i], i);
				}
			}
			maskedTextProvider.ResetOnPrompt = this.ResetOnPrompt;
			maskedTextProvider.ResetOnSpace = this.ResetOnSpace;
			maskedTextProvider.SkipLiterals = this.SkipLiterals;
			maskedTextProvider.IncludeLiterals = this.IncludeLiterals;
			maskedTextProvider.IncludePrompt = this.IncludePrompt;
			return maskedTextProvider;
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x000E52E7 File Offset: 0x000E34E7
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06003476 RID: 13430 RVA: 0x000E52EF File Offset: 0x000E34EF
		public static char DefaultPasswordChar
		{
			get
			{
				return '*';
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x000E52F3 File Offset: 0x000E34F3
		public int EditPositionCount
		{
			get
			{
				return this.optionalEditChars + this.requiredEditChars;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x000E5304 File Offset: 0x000E3504
		public IEnumerator EditPositions
		{
			get
			{
				List<int> list = new List<int>();
				int num = 0;
				foreach (MaskedTextProvider.CharDescriptor charDescriptor in this.stringDescriptor)
				{
					if (MaskedTextProvider.IsEditPosition(charDescriptor))
					{
						list.Add(num);
					}
					num++;
				}
				return ((IEnumerable)list).GetEnumerator();
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x000E5374 File Offset: 0x000E3574
		// (set) Token: 0x0600347A RID: 13434 RVA: 0x000E5386 File Offset: 0x000E3586
		public bool IncludeLiterals
		{
			get
			{
				return this.flagState[MaskedTextProvider.INCLUDE_LITERALS];
			}
			set
			{
				this.flagState[MaskedTextProvider.INCLUDE_LITERALS] = value;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x000E5399 File Offset: 0x000E3599
		// (set) Token: 0x0600347C RID: 13436 RVA: 0x000E53AB File Offset: 0x000E35AB
		public bool IncludePrompt
		{
			get
			{
				return this.flagState[MaskedTextProvider.INCLUDE_PROMPT];
			}
			set
			{
				this.flagState[MaskedTextProvider.INCLUDE_PROMPT] = value;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x000E53BE File Offset: 0x000E35BE
		public bool AsciiOnly
		{
			get
			{
				return this.flagState[MaskedTextProvider.ASCII_ONLY];
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x000E53D0 File Offset: 0x000E35D0
		// (set) Token: 0x0600347F RID: 13439 RVA: 0x000E53DB File Offset: 0x000E35DB
		public bool IsPassword
		{
			get
			{
				return this.passwordChar > '\0';
			}
			set
			{
				if (this.IsPassword != value)
				{
					this.passwordChar = (value ? MaskedTextProvider.DefaultPasswordChar : '\0');
				}
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06003480 RID: 13440 RVA: 0x000E53F7 File Offset: 0x000E35F7
		public static int InvalidIndex
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x000E53FA File Offset: 0x000E35FA
		public int LastAssignedPosition
		{
			get
			{
				return this.FindAssignedEditPositionFrom(this.testString.Length - 1, false);
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x000E5410 File Offset: 0x000E3610
		public int Length
		{
			get
			{
				return this.testString.Length;
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06003483 RID: 13443 RVA: 0x000E541D File Offset: 0x000E361D
		public string Mask
		{
			get
			{
				return this.mask;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x000E5425 File Offset: 0x000E3625
		public bool MaskCompleted
		{
			get
			{
				return this.requiredCharCount == this.requiredEditChars;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x000E5435 File Offset: 0x000E3635
		public bool MaskFull
		{
			get
			{
				return this.assignedCharCount == this.EditPositionCount;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x000E5445 File Offset: 0x000E3645
		// (set) Token: 0x06003487 RID: 13447 RVA: 0x000E5450 File Offset: 0x000E3650
		public char PasswordChar
		{
			get
			{
				return this.passwordChar;
			}
			set
			{
				if (value == this.promptChar)
				{
					throw new InvalidOperationException(SR.GetString("MaskedTextProviderPasswordAndPromptCharError"));
				}
				if (!MaskedTextProvider.IsValidPasswordChar(value) && value != '\0')
				{
					throw new ArgumentException(SR.GetString("MaskedTextProviderInvalidCharError"));
				}
				if (value != this.passwordChar)
				{
					this.passwordChar = value;
				}
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003488 RID: 13448 RVA: 0x000E54A1 File Offset: 0x000E36A1
		// (set) Token: 0x06003489 RID: 13449 RVA: 0x000E54AC File Offset: 0x000E36AC
		public char PromptChar
		{
			get
			{
				return this.promptChar;
			}
			set
			{
				if (value == this.passwordChar)
				{
					throw new InvalidOperationException(SR.GetString("MaskedTextProviderPasswordAndPromptCharError"));
				}
				if (!MaskedTextProvider.IsPrintableChar(value))
				{
					throw new ArgumentException(SR.GetString("MaskedTextProviderInvalidCharError"));
				}
				if (value != this.promptChar)
				{
					this.promptChar = value;
					for (int i = 0; i < this.testString.Length; i++)
					{
						MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
						if (this.IsEditPosition(i) && !charDescriptor.IsAssigned)
						{
							this.testString[i] = this.promptChar;
						}
					}
				}
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x0600348A RID: 13450 RVA: 0x000E5540 File Offset: 0x000E3740
		// (set) Token: 0x0600348B RID: 13451 RVA: 0x000E5552 File Offset: 0x000E3752
		public bool ResetOnPrompt
		{
			get
			{
				return this.flagState[MaskedTextProvider.RESET_ON_PROMPT];
			}
			set
			{
				this.flagState[MaskedTextProvider.RESET_ON_PROMPT] = value;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x0600348C RID: 13452 RVA: 0x000E5565 File Offset: 0x000E3765
		// (set) Token: 0x0600348D RID: 13453 RVA: 0x000E5577 File Offset: 0x000E3777
		public bool ResetOnSpace
		{
			get
			{
				return this.flagState[MaskedTextProvider.SKIP_SPACE];
			}
			set
			{
				this.flagState[MaskedTextProvider.SKIP_SPACE] = value;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x0600348E RID: 13454 RVA: 0x000E558A File Offset: 0x000E378A
		// (set) Token: 0x0600348F RID: 13455 RVA: 0x000E559C File Offset: 0x000E379C
		public bool SkipLiterals
		{
			get
			{
				return this.flagState[MaskedTextProvider.RESET_ON_LITERALS];
			}
			set
			{
				this.flagState[MaskedTextProvider.RESET_ON_LITERALS] = value;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		public char this[int index]
		{
			get
			{
				if (index < 0 || index >= this.testString.Length)
				{
					throw new IndexOutOfRangeException(index.ToString(CultureInfo.CurrentCulture));
				}
				return this.testString[index];
			}
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000E55E4 File Offset: 0x000E37E4
		public bool Add(char input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Add(input, out num, out maskedTextResultHint);
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000E55FC File Offset: 0x000E37FC
		public bool Add(char input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			int lastAssignedPosition = this.LastAssignedPosition;
			if (lastAssignedPosition == this.testString.Length - 1)
			{
				testPosition = this.testString.Length;
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				return false;
			}
			testPosition = lastAssignedPosition + 1;
			testPosition = this.FindEditPositionFrom(testPosition, true);
			if (testPosition == -1)
			{
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = this.testString.Length;
				return false;
			}
			return this.TestSetChar(input, testPosition, out resultHint);
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000E566C File Offset: 0x000E386C
		public bool Add(string input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Add(input, out num, out maskedTextResultHint);
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000E5684 File Offset: 0x000E3884
		public bool Add(string input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			testPosition = this.LastAssignedPosition + 1;
			if (input.Length == 0)
			{
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			return this.TestSetString(input, testPosition, out testPosition, out resultHint);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000E56B8 File Offset: 0x000E38B8
		public void Clear()
		{
			MaskedTextResultHint maskedTextResultHint;
			this.Clear(out maskedTextResultHint);
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000E56D0 File Offset: 0x000E38D0
		public void Clear(out MaskedTextResultHint resultHint)
		{
			if (this.assignedCharCount == 0)
			{
				resultHint = MaskedTextResultHint.NoEffect;
				return;
			}
			resultHint = MaskedTextResultHint.Success;
			for (int i = 0; i < this.testString.Length; i++)
			{
				this.ResetChar(i);
			}
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000E570C File Offset: 0x000E390C
		public int FindAssignedEditPositionFrom(int position, bool direction)
		{
			if (this.assignedCharCount == 0)
			{
				return -1;
			}
			int num;
			int num2;
			if (direction)
			{
				num = position;
				num2 = this.testString.Length - 1;
			}
			else
			{
				num = 0;
				num2 = position;
			}
			return this.FindAssignedEditPositionInRange(num, num2, direction);
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000E5745 File Offset: 0x000E3945
		public int FindAssignedEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			if (this.assignedCharCount == 0)
			{
				return -1;
			}
			return this.FindEditPositionInRange(startPosition, endPosition, direction, 2);
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000E575C File Offset: 0x000E395C
		public int FindEditPositionFrom(int position, bool direction)
		{
			int num;
			int num2;
			if (direction)
			{
				num = position;
				num2 = this.testString.Length - 1;
			}
			else
			{
				num = 0;
				num2 = position;
			}
			return this.FindEditPositionInRange(num, num2, direction);
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000E578C File Offset: 0x000E398C
		public int FindEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			MaskedTextProvider.CharType charType = MaskedTextProvider.CharType.EditOptional | MaskedTextProvider.CharType.EditRequired;
			return this.FindPositionInRange(startPosition, endPosition, direction, charType);
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000E57A8 File Offset: 0x000E39A8
		private int FindEditPositionInRange(int startPosition, int endPosition, bool direction, byte assignedStatus)
		{
			int num;
			for (;;)
			{
				num = this.FindEditPositionInRange(startPosition, endPosition, direction);
				if (num == -1)
				{
					return -1;
				}
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num];
				if (assignedStatus != 1)
				{
					if (assignedStatus != 2)
					{
						break;
					}
					if (charDescriptor.IsAssigned)
					{
						return num;
					}
				}
				else if (!charDescriptor.IsAssigned)
				{
					return num;
				}
				if (direction)
				{
					startPosition++;
				}
				else
				{
					endPosition--;
				}
				if (startPosition > endPosition)
				{
					return -1;
				}
			}
			return num;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000E5808 File Offset: 0x000E3A08
		public int FindNonEditPositionFrom(int position, bool direction)
		{
			int num;
			int num2;
			if (direction)
			{
				num = position;
				num2 = this.testString.Length - 1;
			}
			else
			{
				num = 0;
				num2 = position;
			}
			return this.FindNonEditPositionInRange(num, num2, direction);
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000E5838 File Offset: 0x000E3A38
		public int FindNonEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			MaskedTextProvider.CharType charType = MaskedTextProvider.CharType.Separator | MaskedTextProvider.CharType.Literal;
			return this.FindPositionInRange(startPosition, endPosition, direction, charType);
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000E5854 File Offset: 0x000E3A54
		private int FindPositionInRange(int startPosition, int endPosition, bool direction, MaskedTextProvider.CharType charTypeFlags)
		{
			if (startPosition < 0)
			{
				startPosition = 0;
			}
			if (endPosition >= this.testString.Length)
			{
				endPosition = this.testString.Length - 1;
			}
			if (startPosition > endPosition)
			{
				return -1;
			}
			while (startPosition <= endPosition)
			{
				int num;
				if (!direction)
				{
					endPosition = (num = endPosition) - 1;
				}
				else
				{
					startPosition = (num = startPosition) + 1;
				}
				int num2 = num;
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num2];
				if ((charDescriptor.CharType & charTypeFlags) == charDescriptor.CharType)
				{
					return num2;
				}
			}
			return -1;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000E58C4 File Offset: 0x000E3AC4
		public int FindUnassignedEditPositionFrom(int position, bool direction)
		{
			int num;
			int num2;
			if (direction)
			{
				num = position;
				num2 = this.testString.Length - 1;
			}
			else
			{
				num = 0;
				num2 = position;
			}
			return this.FindEditPositionInRange(num, num2, direction, 1);
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000E58F4 File Offset: 0x000E3AF4
		public int FindUnassignedEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			for (;;)
			{
				int num = this.FindEditPositionInRange(startPosition, endPosition, direction, 0);
				if (num == -1)
				{
					break;
				}
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num];
				if (!charDescriptor.IsAssigned)
				{
					return num;
				}
				if (direction)
				{
					startPosition++;
				}
				else
				{
					endPosition--;
				}
			}
			return -1;
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000E5939 File Offset: 0x000E3B39
		public static bool GetOperationResultFromHint(MaskedTextResultHint hint)
		{
			return hint > MaskedTextResultHint.Unknown;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000E593F File Offset: 0x000E3B3F
		public bool InsertAt(char input, int position)
		{
			return position >= 0 && position < this.testString.Length && this.InsertAt(input.ToString(), position);
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000E5963 File Offset: 0x000E3B63
		public bool InsertAt(char input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			return this.InsertAt(input.ToString(), position, out testPosition, out resultHint);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000E5978 File Offset: 0x000E3B78
		public bool InsertAt(string input, int position)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.InsertAt(input, position, out num, out maskedTextResultHint);
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000E5991 File Offset: 0x000E3B91
		public bool InsertAt(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (position < 0 || position >= this.testString.Length)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			return this.InsertAtInt(input, position, out testPosition, out resultHint, false);
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000E59CC File Offset: 0x000E3BCC
		private bool InsertAtInt(string input, int position, out int testPosition, out MaskedTextResultHint resultHint, bool testOnly)
		{
			if (input.Length == 0)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			if (!this.TestString(input, position, out testPosition, out resultHint))
			{
				return false;
			}
			int i = this.FindEditPositionFrom(position, true);
			bool flag = this.FindAssignedEditPositionInRange(i, testPosition, true) != -1;
			int lastAssignedPosition = this.LastAssignedPosition;
			if (flag && testPosition == this.testString.Length - 1)
			{
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = this.testString.Length;
				return false;
			}
			int num = this.FindEditPositionFrom(testPosition + 1, true);
			if (flag)
			{
				MaskedTextResultHint maskedTextResultHint = MaskedTextResultHint.Unknown;
				while (num != -1)
				{
					MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
					if (charDescriptor.IsAssigned && !this.TestChar(this.testString[i], num, out maskedTextResultHint))
					{
						resultHint = maskedTextResultHint;
						testPosition = num;
						return false;
					}
					if (i != lastAssignedPosition)
					{
						i = this.FindEditPositionFrom(i + 1, true);
						num = this.FindEditPositionFrom(num + 1, true);
					}
					else
					{
						if (maskedTextResultHint > resultHint)
						{
							resultHint = maskedTextResultHint;
							goto IL_00F3;
						}
						goto IL_00F3;
					}
				}
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = this.testString.Length;
				return false;
			}
			IL_00F3:
			if (testOnly)
			{
				return true;
			}
			if (flag)
			{
				while (i >= position)
				{
					MaskedTextProvider.CharDescriptor charDescriptor2 = this.stringDescriptor[i];
					if (charDescriptor2.IsAssigned)
					{
						this.SetChar(this.testString[i], num);
					}
					else
					{
						this.ResetChar(num);
					}
					num = this.FindEditPositionFrom(num - 1, false);
					i = this.FindEditPositionFrom(i - 1, false);
				}
			}
			this.SetString(input, position);
			return true;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000E5B2D File Offset: 0x000E3D2D
		private static bool IsAscii(char c)
		{
			return c >= '!' && c <= '~';
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000E5B3E File Offset: 0x000E3D3E
		private static bool IsAciiAlphanumeric(char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000E5B65 File Offset: 0x000E3D65
		private static bool IsAlphanumeric(char c)
		{
			return char.IsLetter(c) || char.IsDigit(c);
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000E5B77 File Offset: 0x000E3D77
		private static bool IsAsciiLetter(char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000E5B94 File Offset: 0x000E3D94
		public bool IsAvailablePosition(int position)
		{
			if (position < 0 || position >= this.testString.Length)
			{
				return false;
			}
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			return MaskedTextProvider.IsEditPosition(charDescriptor) && !charDescriptor.IsAssigned;
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000E5BD8 File Offset: 0x000E3DD8
		public bool IsEditPosition(int position)
		{
			if (position < 0 || position >= this.testString.Length)
			{
				return false;
			}
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			return MaskedTextProvider.IsEditPosition(charDescriptor);
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000E5C0C File Offset: 0x000E3E0C
		private static bool IsEditPosition(MaskedTextProvider.CharDescriptor charDescriptor)
		{
			return charDescriptor.CharType == MaskedTextProvider.CharType.EditRequired || charDescriptor.CharType == MaskedTextProvider.CharType.EditOptional;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000E5C22 File Offset: 0x000E3E22
		private static bool IsLiteralPosition(MaskedTextProvider.CharDescriptor charDescriptor)
		{
			return charDescriptor.CharType == MaskedTextProvider.CharType.Literal || charDescriptor.CharType == MaskedTextProvider.CharType.Separator;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000E5C38 File Offset: 0x000E3E38
		private static bool IsPrintableChar(char c)
		{
			return char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c) || c == ' ';
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000E5C59 File Offset: 0x000E3E59
		public static bool IsValidInputChar(char c)
		{
			return MaskedTextProvider.IsPrintableChar(c);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000E5C61 File Offset: 0x000E3E61
		public static bool IsValidMaskChar(char c)
		{
			return MaskedTextProvider.IsPrintableChar(c);
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000E5C69 File Offset: 0x000E3E69
		public static bool IsValidPasswordChar(char c)
		{
			return MaskedTextProvider.IsPrintableChar(c) || c == '\0';
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000E5C7C File Offset: 0x000E3E7C
		public bool Remove()
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Remove(out num, out maskedTextResultHint);
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x000E5C94 File Offset: 0x000E3E94
		public bool Remove(out int testPosition, out MaskedTextResultHint resultHint)
		{
			int lastAssignedPosition = this.LastAssignedPosition;
			if (lastAssignedPosition == -1)
			{
				testPosition = 0;
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			this.ResetChar(lastAssignedPosition);
			testPosition = lastAssignedPosition;
			resultHint = MaskedTextResultHint.Success;
			return true;
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000E5CC2 File Offset: 0x000E3EC2
		public bool RemoveAt(int position)
		{
			return this.RemoveAt(position, position);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000E5CCC File Offset: 0x000E3ECC
		public bool RemoveAt(int startPosition, int endPosition)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.RemoveAt(startPosition, endPosition, out num, out maskedTextResultHint);
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x000E5CE5 File Offset: 0x000E3EE5
		public bool RemoveAt(int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (endPosition >= this.testString.Length)
			{
				testPosition = endPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition < 0 || startPosition > endPosition)
			{
				testPosition = startPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			return this.RemoveAtInt(startPosition, endPosition, out testPosition, out resultHint, false);
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000E5D20 File Offset: 0x000E3F20
		private bool RemoveAtInt(int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint, bool testOnly)
		{
			int lastAssignedPosition = this.LastAssignedPosition;
			int num = this.FindEditPositionInRange(startPosition, endPosition, true);
			resultHint = MaskedTextResultHint.NoEffect;
			if (num == -1 || num > lastAssignedPosition)
			{
				testPosition = startPosition;
				return true;
			}
			testPosition = startPosition;
			bool flag = endPosition < lastAssignedPosition;
			if (this.FindAssignedEditPositionInRange(startPosition, endPosition, true) != -1)
			{
				resultHint = MaskedTextResultHint.Success;
			}
			if (flag)
			{
				int num2 = this.FindEditPositionFrom(endPosition + 1, true);
				int num3 = num2;
				startPosition = num;
				MaskedTextResultHint maskedTextResultHint;
				for (;;)
				{
					char c = this.testString[num2];
					MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num2];
					if ((c != this.PromptChar || charDescriptor.IsAssigned) && !this.TestChar(c, num, out maskedTextResultHint))
					{
						break;
					}
					if (num2 == lastAssignedPosition)
					{
						goto IL_00B3;
					}
					num2 = this.FindEditPositionFrom(num2 + 1, true);
					num = this.FindEditPositionFrom(num + 1, true);
				}
				resultHint = maskedTextResultHint;
				testPosition = num;
				return false;
				IL_00B3:
				if (MaskedTextResultHint.SideEffect > resultHint)
				{
					resultHint = MaskedTextResultHint.SideEffect;
				}
				if (testOnly)
				{
					return true;
				}
				num2 = num3;
				num = startPosition;
				for (;;)
				{
					char c2 = this.testString[num2];
					MaskedTextProvider.CharDescriptor charDescriptor2 = this.stringDescriptor[num2];
					if (c2 == this.PromptChar && !charDescriptor2.IsAssigned)
					{
						this.ResetChar(num);
					}
					else
					{
						this.SetChar(c2, num);
						this.ResetChar(num2);
					}
					if (num2 == lastAssignedPosition)
					{
						break;
					}
					num2 = this.FindEditPositionFrom(num2 + 1, true);
					num = this.FindEditPositionFrom(num + 1, true);
				}
				startPosition = num + 1;
			}
			if (startPosition <= endPosition)
			{
				this.ResetString(startPosition, endPosition);
			}
			return true;
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000E5E6C File Offset: 0x000E406C
		public bool Replace(char input, int position)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Replace(input, position, out num, out maskedTextResultHint);
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000E5E88 File Offset: 0x000E4088
		public bool Replace(char input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (position < 0 || position >= this.testString.Length)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			testPosition = position;
			if (!this.TestEscapeChar(input, testPosition))
			{
				testPosition = this.FindEditPositionFrom(testPosition, true);
			}
			if (testPosition == -1)
			{
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = position;
				return false;
			}
			return this.TestSetChar(input, testPosition, out resultHint);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000E5EEC File Offset: 0x000E40EC
		public bool Replace(char input, int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (endPosition >= this.testString.Length)
			{
				testPosition = endPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition < 0 || startPosition > endPosition)
			{
				testPosition = startPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition == endPosition)
			{
				testPosition = startPosition;
				return this.TestSetChar(input, startPosition, out resultHint);
			}
			return this.Replace(input.ToString(), startPosition, endPosition, out testPosition, out resultHint);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000E5F4C File Offset: 0x000E414C
		public bool Replace(string input, int position)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Replace(input, position, out num, out maskedTextResultHint);
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000E5F68 File Offset: 0x000E4168
		public bool Replace(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (position < 0 || position >= this.testString.Length)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (input.Length == 0)
			{
				return this.RemoveAt(position, position, out testPosition, out resultHint);
			}
			return this.TestSetString(input, position, out testPosition, out resultHint);
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000E5FC4 File Offset: 0x000E41C4
		public bool Replace(string input, int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (endPosition >= this.testString.Length)
			{
				testPosition = endPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition < 0 || startPosition > endPosition)
			{
				testPosition = startPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (input.Length == 0)
			{
				return this.RemoveAt(startPosition, endPosition, out testPosition, out resultHint);
			}
			if (!this.TestString(input, startPosition, out testPosition, out resultHint))
			{
				return false;
			}
			if (this.assignedCharCount > 0)
			{
				if (testPosition < endPosition)
				{
					int num;
					MaskedTextResultHint maskedTextResultHint;
					if (!this.RemoveAtInt(testPosition + 1, endPosition, out num, out maskedTextResultHint, false))
					{
						testPosition = num;
						resultHint = maskedTextResultHint;
						return false;
					}
					if (maskedTextResultHint == MaskedTextResultHint.Success && resultHint != maskedTextResultHint)
					{
						resultHint = MaskedTextResultHint.SideEffect;
					}
				}
				else if (testPosition > endPosition)
				{
					int lastAssignedPosition = this.LastAssignedPosition;
					int i = testPosition + 1;
					int num2 = endPosition + 1;
					MaskedTextResultHint maskedTextResultHint;
					for (;;)
					{
						num2 = this.FindEditPositionFrom(num2, true);
						i = this.FindEditPositionFrom(i, true);
						if (i == -1)
						{
							goto Block_12;
						}
						if (!this.TestChar(this.testString[num2], i, out maskedTextResultHint))
						{
							goto Block_13;
						}
						if (maskedTextResultHint == MaskedTextResultHint.Success && resultHint != maskedTextResultHint)
						{
							resultHint = MaskedTextResultHint.Success;
						}
						if (num2 == lastAssignedPosition)
						{
							break;
						}
						num2++;
						i++;
					}
					while (i > testPosition)
					{
						this.SetChar(this.testString[num2], i);
						num2 = this.FindEditPositionFrom(num2 - 1, false);
						i = this.FindEditPositionFrom(i - 1, false);
					}
					goto IL_0162;
					Block_12:
					testPosition = this.testString.Length;
					resultHint = MaskedTextResultHint.UnavailableEditPosition;
					return false;
					Block_13:
					testPosition = i;
					resultHint = maskedTextResultHint;
					return false;
				}
			}
			IL_0162:
			this.SetString(input, startPosition);
			return true;
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000E613C File Offset: 0x000E433C
		private void ResetChar(int testPosition)
		{
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[testPosition];
			if (this.IsEditPosition(testPosition) && charDescriptor.IsAssigned)
			{
				charDescriptor.IsAssigned = false;
				this.testString[testPosition] = this.promptChar;
				this.assignedCharCount--;
				if (charDescriptor.CharType == MaskedTextProvider.CharType.EditRequired)
				{
					this.requiredCharCount--;
				}
			}
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000E61A5 File Offset: 0x000E43A5
		private void ResetString(int startPosition, int endPosition)
		{
			startPosition = this.FindAssignedEditPositionFrom(startPosition, true);
			if (startPosition != -1)
			{
				endPosition = this.FindAssignedEditPositionFrom(endPosition, false);
				while (startPosition <= endPosition)
				{
					startPosition = this.FindAssignedEditPositionFrom(startPosition, true);
					this.ResetChar(startPosition);
					startPosition++;
				}
			}
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000E61DC File Offset: 0x000E43DC
		public bool Set(string input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Set(input, out num, out maskedTextResultHint);
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000E61F4 File Offset: 0x000E43F4
		public bool Set(string input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			resultHint = MaskedTextResultHint.Unknown;
			testPosition = 0;
			if (input.Length == 0)
			{
				this.Clear(out resultHint);
				return true;
			}
			if (!this.TestSetString(input, testPosition, out testPosition, out resultHint))
			{
				return false;
			}
			int num = this.FindAssignedEditPositionFrom(testPosition + 1, true);
			if (num != -1)
			{
				this.ResetString(num, this.testString.Length - 1);
			}
			return true;
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000E625C File Offset: 0x000E445C
		private void SetChar(char input, int position)
		{
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			this.SetChar(input, position, charDescriptor);
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000E6280 File Offset: 0x000E4480
		private void SetChar(char input, int position, MaskedTextProvider.CharDescriptor charDescriptor)
		{
			MaskedTextProvider.CharDescriptor charDescriptor2 = this.stringDescriptor[position];
			if (this.TestEscapeChar(input, position, charDescriptor))
			{
				this.ResetChar(position);
				return;
			}
			if (char.IsLetter(input))
			{
				if (char.IsUpper(input))
				{
					if (charDescriptor.CaseConversion == MaskedTextProvider.CaseConversion.ToLower)
					{
						input = this.culture.TextInfo.ToLower(input);
					}
				}
				else if (charDescriptor.CaseConversion == MaskedTextProvider.CaseConversion.ToUpper)
				{
					input = this.culture.TextInfo.ToUpper(input);
				}
			}
			this.testString[position] = input;
			if (!charDescriptor.IsAssigned)
			{
				charDescriptor.IsAssigned = true;
				this.assignedCharCount++;
				if (charDescriptor.CharType == MaskedTextProvider.CharType.EditRequired)
				{
					this.requiredCharCount++;
				}
			}
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x000E6338 File Offset: 0x000E4538
		private void SetString(string input, int testPosition)
		{
			foreach (char c in input)
			{
				if (!this.TestEscapeChar(c, testPosition))
				{
					testPosition = this.FindEditPositionFrom(testPosition, true);
				}
				this.SetChar(c, testPosition);
				testPosition++;
			}
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000E6384 File Offset: 0x000E4584
		private bool TestChar(char input, int position, out MaskedTextResultHint resultHint)
		{
			if (!MaskedTextProvider.IsPrintableChar(input))
			{
				resultHint = MaskedTextResultHint.InvalidInput;
				return false;
			}
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			if (MaskedTextProvider.IsLiteralPosition(charDescriptor))
			{
				if (this.SkipLiterals && input == this.testString[position])
				{
					resultHint = MaskedTextResultHint.CharacterEscaped;
					return true;
				}
				resultHint = MaskedTextResultHint.NonEditPosition;
				return false;
			}
			else
			{
				if (input == this.promptChar)
				{
					if (this.ResetOnPrompt)
					{
						if (MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned)
						{
							resultHint = MaskedTextResultHint.SideEffect;
						}
						else
						{
							resultHint = MaskedTextResultHint.CharacterEscaped;
						}
						return true;
					}
					if (!this.AllowPromptAsInput)
					{
						resultHint = MaskedTextResultHint.PromptCharNotAllowed;
						return false;
					}
				}
				if (input == ' ' && this.ResetOnSpace)
				{
					if (MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned)
					{
						resultHint = MaskedTextResultHint.SideEffect;
					}
					else
					{
						resultHint = MaskedTextResultHint.CharacterEscaped;
					}
					return true;
				}
				char c = this.mask[charDescriptor.MaskPosition];
				if (c <= '0')
				{
					if (c != '#')
					{
						if (c != '&')
						{
							if (c == '0')
							{
								if (!char.IsDigit(input))
								{
									resultHint = MaskedTextResultHint.DigitExpected;
									return false;
								}
							}
						}
						else if (!MaskedTextProvider.IsAscii(input) && this.AsciiOnly)
						{
							resultHint = MaskedTextResultHint.AsciiCharacterExpected;
							return false;
						}
					}
					else if (!char.IsDigit(input) && input != '-' && input != '+' && input != ' ')
					{
						resultHint = MaskedTextResultHint.DigitExpected;
						return false;
					}
				}
				else if (c <= 'C')
				{
					if (c != '9')
					{
						switch (c)
						{
						case '?':
							if (!char.IsLetter(input) && input != ' ')
							{
								resultHint = MaskedTextResultHint.LetterExpected;
								return false;
							}
							if (!MaskedTextProvider.IsAsciiLetter(input) && this.AsciiOnly)
							{
								resultHint = MaskedTextResultHint.AsciiCharacterExpected;
								return false;
							}
							break;
						case 'A':
							if (!MaskedTextProvider.IsAlphanumeric(input))
							{
								resultHint = MaskedTextResultHint.AlphanumericCharacterExpected;
								return false;
							}
							if (!MaskedTextProvider.IsAciiAlphanumeric(input) && this.AsciiOnly)
							{
								resultHint = MaskedTextResultHint.AsciiCharacterExpected;
								return false;
							}
							break;
						case 'C':
							if (!MaskedTextProvider.IsAscii(input) && this.AsciiOnly && input != ' ')
							{
								resultHint = MaskedTextResultHint.AsciiCharacterExpected;
								return false;
							}
							break;
						}
					}
					else if (!char.IsDigit(input) && input != ' ')
					{
						resultHint = MaskedTextResultHint.DigitExpected;
						return false;
					}
				}
				else if (c != 'L')
				{
					if (c == 'a')
					{
						if (!MaskedTextProvider.IsAlphanumeric(input) && input != ' ')
						{
							resultHint = MaskedTextResultHint.AlphanumericCharacterExpected;
							return false;
						}
						if (!MaskedTextProvider.IsAciiAlphanumeric(input) && this.AsciiOnly)
						{
							resultHint = MaskedTextResultHint.AsciiCharacterExpected;
							return false;
						}
					}
				}
				else
				{
					if (!char.IsLetter(input))
					{
						resultHint = MaskedTextResultHint.LetterExpected;
						return false;
					}
					if (!MaskedTextProvider.IsAsciiLetter(input) && this.AsciiOnly)
					{
						resultHint = MaskedTextResultHint.AsciiCharacterExpected;
						return false;
					}
				}
				if (input == this.testString[position] && charDescriptor.IsAssigned)
				{
					resultHint = MaskedTextResultHint.NoEffect;
				}
				else
				{
					resultHint = MaskedTextResultHint.Success;
				}
				return true;
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000E65E4 File Offset: 0x000E47E4
		private bool TestEscapeChar(char input, int position)
		{
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			return this.TestEscapeChar(input, position, charDescriptor);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000E6608 File Offset: 0x000E4808
		private bool TestEscapeChar(char input, int position, MaskedTextProvider.CharDescriptor charDex)
		{
			if (MaskedTextProvider.IsLiteralPosition(charDex))
			{
				return this.SkipLiterals && input == this.testString[position];
			}
			return (this.ResetOnPrompt && input == this.promptChar) || (this.ResetOnSpace && input == ' ');
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000E6658 File Offset: 0x000E4858
		private bool TestSetChar(char input, int position, out MaskedTextResultHint resultHint)
		{
			if (this.TestChar(input, position, out resultHint))
			{
				if (resultHint == MaskedTextResultHint.Success || resultHint == MaskedTextResultHint.SideEffect)
				{
					this.SetChar(input, position);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000E667A File Offset: 0x000E487A
		private bool TestSetString(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (this.TestString(input, position, out testPosition, out resultHint))
			{
				this.SetString(input, position);
				return true;
			}
			return false;
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000E6694 File Offset: 0x000E4894
		private bool TestString(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			resultHint = MaskedTextResultHint.Unknown;
			testPosition = position;
			if (input.Length == 0)
			{
				return true;
			}
			MaskedTextResultHint maskedTextResultHint = resultHint;
			foreach (char c in input)
			{
				if (testPosition >= this.testString.Length)
				{
					resultHint = MaskedTextResultHint.UnavailableEditPosition;
					return false;
				}
				if (!this.TestEscapeChar(c, testPosition))
				{
					testPosition = this.FindEditPositionFrom(testPosition, true);
					if (testPosition == -1)
					{
						testPosition = this.testString.Length;
						resultHint = MaskedTextResultHint.UnavailableEditPosition;
						return false;
					}
				}
				if (!this.TestChar(c, testPosition, out maskedTextResultHint))
				{
					resultHint = maskedTextResultHint;
					return false;
				}
				if (maskedTextResultHint > resultHint)
				{
					resultHint = maskedTextResultHint;
				}
				testPosition++;
			}
			testPosition--;
			return true;
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000E6740 File Offset: 0x000E4940
		public string ToDisplayString()
		{
			if (!this.IsPassword || this.assignedCharCount == 0)
			{
				return this.testString.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder(this.testString.Length);
			for (int i = 0; i < this.testString.Length; i++)
			{
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
				stringBuilder.Append((MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned) ? this.passwordChar : this.testString[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000E67CE File Offset: 0x000E49CE
		public override string ToString()
		{
			return this.ToString(true, this.IncludePrompt, this.IncludeLiterals, 0, this.testString.Length);
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000E67EF File Offset: 0x000E49EF
		public string ToString(bool ignorePasswordChar)
		{
			return this.ToString(ignorePasswordChar, this.IncludePrompt, this.IncludeLiterals, 0, this.testString.Length);
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000E6810 File Offset: 0x000E4A10
		public string ToString(int startPosition, int length)
		{
			return this.ToString(true, this.IncludePrompt, this.IncludeLiterals, startPosition, length);
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000E6827 File Offset: 0x000E4A27
		public string ToString(bool ignorePasswordChar, int startPosition, int length)
		{
			return this.ToString(ignorePasswordChar, this.IncludePrompt, this.IncludeLiterals, startPosition, length);
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x000E683E File Offset: 0x000E4A3E
		public string ToString(bool includePrompt, bool includeLiterals)
		{
			return this.ToString(true, includePrompt, includeLiterals, 0, this.testString.Length);
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000E6855 File Offset: 0x000E4A55
		public string ToString(bool includePrompt, bool includeLiterals, int startPosition, int length)
		{
			return this.ToString(true, includePrompt, includeLiterals, startPosition, length);
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000E6864 File Offset: 0x000E4A64
		public string ToString(bool ignorePasswordChar, bool includePrompt, bool includeLiterals, int startPosition, int length)
		{
			if (length <= 0)
			{
				return string.Empty;
			}
			if (startPosition < 0)
			{
				startPosition = 0;
			}
			if (startPosition >= this.testString.Length)
			{
				return string.Empty;
			}
			int num = this.testString.Length - startPosition;
			if (length > num)
			{
				length = num;
			}
			if ((!this.IsPassword || ignorePasswordChar) && (includePrompt && includeLiterals))
			{
				return this.testString.ToString(startPosition, length);
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num2 = startPosition + length - 1;
			if (!includePrompt)
			{
				int num3 = (includeLiterals ? this.FindNonEditPositionInRange(startPosition, num2, false) : MaskedTextProvider.InvalidIndex);
				int num4 = this.FindAssignedEditPositionInRange((num3 == MaskedTextProvider.InvalidIndex) ? startPosition : num3, num2, false);
				num2 = ((num4 != MaskedTextProvider.InvalidIndex) ? num4 : num3);
				if (num2 == MaskedTextProvider.InvalidIndex)
				{
					return string.Empty;
				}
			}
			int i = startPosition;
			while (i <= num2)
			{
				char c = this.testString[i];
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
				MaskedTextProvider.CharType charType = charDescriptor.CharType;
				if (charType - MaskedTextProvider.CharType.EditOptional > 1)
				{
					if (charType != MaskedTextProvider.CharType.Separator && charType != MaskedTextProvider.CharType.Literal)
					{
						goto IL_012F;
					}
					if (includeLiterals)
					{
						goto IL_012F;
					}
				}
				else if (charDescriptor.IsAssigned)
				{
					if (!this.IsPassword || ignorePasswordChar)
					{
						goto IL_012F;
					}
					stringBuilder.Append(this.passwordChar);
				}
				else
				{
					if (includePrompt)
					{
						goto IL_012F;
					}
					stringBuilder.Append(' ');
				}
				IL_0138:
				i++;
				continue;
				IL_012F:
				stringBuilder.Append(c);
				goto IL_0138;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000E69BD File Offset: 0x000E4BBD
		public bool VerifyChar(char input, int position, out MaskedTextResultHint hint)
		{
			hint = MaskedTextResultHint.NoEffect;
			if (position < 0 || position >= this.testString.Length)
			{
				hint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			return this.TestChar(input, position, out hint);
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x000E69E3 File Offset: 0x000E4BE3
		public bool VerifyEscapeChar(char input, int position)
		{
			return position >= 0 && position < this.testString.Length && this.TestEscapeChar(input, position);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000E6A04 File Offset: 0x000E4C04
		public bool VerifyString(string input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.VerifyString(input, out num, out maskedTextResultHint);
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000E6A1C File Offset: 0x000E4C1C
		public bool VerifyString(string input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			testPosition = 0;
			if (input == null || input.Length == 0)
			{
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			return this.TestString(input, 0, out testPosition, out resultHint);
		}

		// Token: 0x040029EC RID: 10732
		private const char spaceChar = ' ';

		// Token: 0x040029ED RID: 10733
		private const char defaultPromptChar = '_';

		// Token: 0x040029EE RID: 10734
		private const char nullPasswordChar = '\0';

		// Token: 0x040029EF RID: 10735
		private const bool defaultAllowPrompt = true;

		// Token: 0x040029F0 RID: 10736
		private const int invalidIndex = -1;

		// Token: 0x040029F1 RID: 10737
		private const byte editAny = 0;

		// Token: 0x040029F2 RID: 10738
		private const byte editUnassigned = 1;

		// Token: 0x040029F3 RID: 10739
		private const byte editAssigned = 2;

		// Token: 0x040029F4 RID: 10740
		private const bool forward = true;

		// Token: 0x040029F5 RID: 10741
		private const bool backward = false;

		// Token: 0x040029F6 RID: 10742
		private static int ASCII_ONLY = BitVector32.CreateMask();

		// Token: 0x040029F7 RID: 10743
		private static int ALLOW_PROMPT_AS_INPUT = BitVector32.CreateMask(MaskedTextProvider.ASCII_ONLY);

		// Token: 0x040029F8 RID: 10744
		private static int INCLUDE_PROMPT = BitVector32.CreateMask(MaskedTextProvider.ALLOW_PROMPT_AS_INPUT);

		// Token: 0x040029F9 RID: 10745
		private static int INCLUDE_LITERALS = BitVector32.CreateMask(MaskedTextProvider.INCLUDE_PROMPT);

		// Token: 0x040029FA RID: 10746
		private static int RESET_ON_PROMPT = BitVector32.CreateMask(MaskedTextProvider.INCLUDE_LITERALS);

		// Token: 0x040029FB RID: 10747
		private static int RESET_ON_LITERALS = BitVector32.CreateMask(MaskedTextProvider.RESET_ON_PROMPT);

		// Token: 0x040029FC RID: 10748
		private static int SKIP_SPACE = BitVector32.CreateMask(MaskedTextProvider.RESET_ON_LITERALS);

		// Token: 0x040029FD RID: 10749
		private static Type maskTextProviderType = typeof(MaskedTextProvider);

		// Token: 0x040029FE RID: 10750
		private BitVector32 flagState;

		// Token: 0x040029FF RID: 10751
		private CultureInfo culture;

		// Token: 0x04002A00 RID: 10752
		private StringBuilder testString;

		// Token: 0x04002A01 RID: 10753
		private int assignedCharCount;

		// Token: 0x04002A02 RID: 10754
		private int requiredCharCount;

		// Token: 0x04002A03 RID: 10755
		private int requiredEditChars;

		// Token: 0x04002A04 RID: 10756
		private int optionalEditChars;

		// Token: 0x04002A05 RID: 10757
		private string mask;

		// Token: 0x04002A06 RID: 10758
		private char passwordChar;

		// Token: 0x04002A07 RID: 10759
		private char promptChar;

		// Token: 0x04002A08 RID: 10760
		private List<MaskedTextProvider.CharDescriptor> stringDescriptor;

		// Token: 0x02000897 RID: 2199
		private enum CaseConversion
		{
			// Token: 0x040037CB RID: 14283
			None,
			// Token: 0x040037CC RID: 14284
			ToLower,
			// Token: 0x040037CD RID: 14285
			ToUpper
		}

		// Token: 0x02000898 RID: 2200
		[Flags]
		private enum CharType
		{
			// Token: 0x040037CF RID: 14287
			EditOptional = 1,
			// Token: 0x040037D0 RID: 14288
			EditRequired = 2,
			// Token: 0x040037D1 RID: 14289
			Separator = 4,
			// Token: 0x040037D2 RID: 14290
			Literal = 8,
			// Token: 0x040037D3 RID: 14291
			Modifier = 16
		}

		// Token: 0x02000899 RID: 2201
		private class CharDescriptor
		{
			// Token: 0x0600459B RID: 17819 RVA: 0x00123131 File Offset: 0x00121331
			public CharDescriptor(int maskPos, MaskedTextProvider.CharType charType)
			{
				this.MaskPosition = maskPos;
				this.CharType = charType;
			}

			// Token: 0x0600459C RID: 17820 RVA: 0x00123148 File Offset: 0x00121348
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "MaskPosition[{0}] <CaseConversion.{1}><CharType.{2}><IsAssigned: {3}", new object[] { this.MaskPosition, this.CaseConversion, this.CharType, this.IsAssigned });
			}

			// Token: 0x040037D4 RID: 14292
			public int MaskPosition;

			// Token: 0x040037D5 RID: 14293
			public MaskedTextProvider.CaseConversion CaseConversion;

			// Token: 0x040037D6 RID: 14294
			public MaskedTextProvider.CharType CharType;

			// Token: 0x040037D7 RID: 14295
			public bool IsAssigned;
		}
	}
}
