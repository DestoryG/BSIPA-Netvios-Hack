using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000015 RID: 21
	internal class AttributesCriterion : SelectionCriterion
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003BFC File Offset: 0x00001DFC
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003CA0 File Offset: 0x00001EA0
		internal string AttributeString
		{
			get
			{
				string text = "";
				if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes)0)
				{
					text += "H";
				}
				if ((this._Attributes & FileAttributes.System) != (FileAttributes)0)
				{
					text += "S";
				}
				if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
				{
					text += "R";
				}
				if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
				{
					text += "A";
				}
				if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
				{
					text += "L";
				}
				if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes)0)
				{
					text += "I";
				}
				return text;
			}
			set
			{
				this._Attributes = FileAttributes.Normal;
				foreach (char c in value.ToUpper())
				{
					char c2 = c;
					if (c2 != 'A')
					{
						switch (c2)
						{
						case 'H':
							if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes)0)
							{
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							this._Attributes |= FileAttributes.Hidden;
							goto IL_01C1;
						case 'I':
							if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes)0)
							{
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							this._Attributes |= FileAttributes.NotContentIndexed;
							goto IL_01C1;
						case 'J':
						case 'K':
							break;
						case 'L':
							if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
							{
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							this._Attributes |= FileAttributes.ReparsePoint;
							goto IL_01C1;
						default:
							switch (c2)
							{
							case 'R':
								if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
								{
									throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
								}
								this._Attributes |= FileAttributes.ReadOnly;
								goto IL_01C1;
							case 'S':
								if ((this._Attributes & FileAttributes.System) != (FileAttributes)0)
								{
									throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
								}
								this._Attributes |= FileAttributes.System;
								goto IL_01C1;
							}
							break;
						}
						throw new ArgumentException(value);
					}
					if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
					{
						throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
					}
					this._Attributes |= FileAttributes.Archive;
					IL_01C1:;
				}
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003E80 File Offset: 0x00002080
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("attributes ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ")
				.Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003ED0 File Offset: 0x000020D0
		private bool _EvaluateOne(FileAttributes fileAttrs, FileAttributes criterionAttrs)
		{
			return (this._Attributes & criterionAttrs) != criterionAttrs || (fileAttrs & criterionAttrs) == criterionAttrs;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003EF8 File Offset: 0x000020F8
		internal override bool Evaluate(string filename)
		{
			if (Directory.Exists(filename))
			{
				return this.Operator != ComparisonOperator.EqualTo;
			}
			FileAttributes attributes = File.GetAttributes(filename);
			return this._Evaluate(attributes);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003F28 File Offset: 0x00002128
		private bool _Evaluate(FileAttributes fileAttrs)
		{
			bool flag = this._EvaluateOne(fileAttrs, FileAttributes.Hidden);
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.System);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReadOnly);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.Archive);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.NotContentIndexed);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReparsePoint);
			}
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003F94 File Offset: 0x00002194
		internal override bool Evaluate(ZipEntry entry)
		{
			FileAttributes attributes = entry.Attributes;
			return this._Evaluate(attributes);
		}

		// Token: 0x04000077 RID: 119
		private FileAttributes _Attributes;

		// Token: 0x04000078 RID: 120
		internal ComparisonOperator Operator;
	}
}
