using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000017 RID: 23
	public class FileSelector
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00004167 File Offset: 0x00002367
		public FileSelector(string selectionCriteria)
			: this(selectionCriteria, true)
		{
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004171 File Offset: 0x00002371
		public FileSelector(string selectionCriteria, bool traverseDirectoryReparsePoints)
		{
			if (!string.IsNullOrEmpty(selectionCriteria))
			{
				this._Criterion = FileSelector._ParseCriterion(selectionCriteria);
			}
			this.TraverseReparsePoints = traverseDirectoryReparsePoints;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004194 File Offset: 0x00002394
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000041AB File Offset: 0x000023AB
		public string SelectionCriteria
		{
			get
			{
				if (this._Criterion == null)
				{
					return null;
				}
				return this._Criterion.ToString();
			}
			set
			{
				if (value == null)
				{
					this._Criterion = null;
					return;
				}
				if (value.Trim() == "")
				{
					this._Criterion = null;
					return;
				}
				this._Criterion = FileSelector._ParseCriterion(value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000041DE File Offset: 0x000023DE
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000041E6 File Offset: 0x000023E6
		public bool TraverseReparsePoints { get; set; }

		// Token: 0x060000C6 RID: 198 RVA: 0x000041F0 File Offset: 0x000023F0
		private static string NormalizeCriteriaExpression(string source)
		{
			string[][] array = new string[][]
			{
				new string[] { "([^']*)\\(\\(([^']+)", "$1( ($2" },
				new string[] { "(.)\\)\\)", "$1) )" },
				new string[] { "\\((\\S)", "( $1" },
				new string[] { "(\\S)\\)", "$1 )" },
				new string[] { "^\\)", " )" },
				new string[] { "(\\S)\\(", "$1 (" },
				new string[] { "\\)(\\S)", ") $1" },
				new string[] { "(=)('[^']*')", "$1 $2" },
				new string[] { "([^ !><])(>|<|!=|=)", "$1 $2" },
				new string[] { "(>|<|!=|=)([^ =])", "$1 $2" },
				new string[] { "/", "\\" }
			};
			string text = source;
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = FileSelector.RegexAssertions.PrecededByEvenNumberOfSingleQuotes + array[i][0] + FileSelector.RegexAssertions.FollowedByEvenNumberOfSingleQuotesAndLineEnd;
				text = Regex.Replace(text, text2, array[i][1]);
			}
			string text3 = "/" + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			text = Regex.Replace(text, text3, "\\");
			text3 = " " + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			return Regex.Replace(text, text3, "\u0006");
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000043DC File Offset: 0x000025DC
		private static SelectionCriterion _ParseCriterion(string s)
		{
			if (s == null)
			{
				return null;
			}
			s = FileSelector.NormalizeCriteriaExpression(s);
			if (s.IndexOf(" ") == -1)
			{
				s = "name = " + s;
			}
			string[] array = s.Trim().Split(new char[] { ' ', '\t' });
			if (array.Length < 3)
			{
				throw new ArgumentException(s);
			}
			SelectionCriterion selectionCriterion = null;
			Stack<FileSelector.ParseState> stack = new Stack<FileSelector.ParseState>();
			Stack<SelectionCriterion> stack2 = new Stack<SelectionCriterion>();
			stack.Push(FileSelector.ParseState.Start);
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i].ToLower();
				string text2;
				if ((text2 = text) != null)
				{
					if (<PrivateImplementationDetails>{BBD9ABA3-3797-4E5D-B8C5-A361E0F7EC0C}.$$method0x60000c7-1 == null)
					{
						<PrivateImplementationDetails>{BBD9ABA3-3797-4E5D-B8C5-A361E0F7EC0C}.$$method0x60000c7-1 = new Dictionary<string, int>(16)
						{
							{ "and", 0 },
							{ "xor", 1 },
							{ "or", 2 },
							{ "(", 3 },
							{ ")", 4 },
							{ "atime", 5 },
							{ "ctime", 6 },
							{ "mtime", 7 },
							{ "length", 8 },
							{ "size", 9 },
							{ "filename", 10 },
							{ "name", 11 },
							{ "attrs", 12 },
							{ "attributes", 13 },
							{ "type", 14 },
							{ "", 15 }
						};
					}
					int num;
					if (<PrivateImplementationDetails>{BBD9ABA3-3797-4E5D-B8C5-A361E0F7EC0C}.$$method0x60000c7-1.TryGetValue(text2, out num))
					{
						FileSelector.ParseState parseState;
						switch (num)
						{
						case 0:
						case 1:
						case 2:
						{
							parseState = stack.Peek();
							if (parseState != FileSelector.ParseState.CriterionDone)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							if (array.Length <= i + 3)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							LogicalConjunction logicalConjunction = (LogicalConjunction)Enum.Parse(typeof(LogicalConjunction), array[i].ToUpper(), true);
							selectionCriterion = new CompoundCriterion
							{
								Left = selectionCriterion,
								Right = null,
								Conjunction = logicalConjunction
							};
							stack.Push(parseState);
							stack.Push(FileSelector.ParseState.ConjunctionPending);
							stack2.Push(selectionCriterion);
							break;
						}
						case 3:
							parseState = stack.Peek();
							if (parseState != FileSelector.ParseState.Start && parseState != FileSelector.ParseState.ConjunctionPending && parseState != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							if (array.Length <= i + 4)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Push(FileSelector.ParseState.OpenParen);
							break;
						case 4:
							parseState = stack.Pop();
							if (stack.Peek() != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Pop();
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						case 5:
						case 6:
						case 7:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							DateTime dateTime;
							try
							{
								dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd-HH:mm:ss", null);
							}
							catch (FormatException)
							{
								try
								{
									dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd-HH:mm:ss", null);
								}
								catch (FormatException)
								{
									try
									{
										dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd", null);
									}
									catch (FormatException)
									{
										try
										{
											dateTime = DateTime.ParseExact(array[i + 2], "MM/dd/yyyy", null);
										}
										catch (FormatException)
										{
											dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd", null);
										}
									}
								}
							}
							dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
							selectionCriterion = new TimeCriterion
							{
								Which = (WhichTime)Enum.Parse(typeof(WhichTime), array[i], true),
								Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]),
								Time = dateTime
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 8:
						case 9:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							string text3 = array[i + 2];
							long num2;
							if (text3.ToUpper().EndsWith("K"))
							{
								num2 = long.Parse(text3.Substring(0, text3.Length - 1)) * 1024L;
							}
							else if (text3.ToUpper().EndsWith("KB"))
							{
								num2 = long.Parse(text3.Substring(0, text3.Length - 2)) * 1024L;
							}
							else if (text3.ToUpper().EndsWith("M"))
							{
								num2 = long.Parse(text3.Substring(0, text3.Length - 1)) * 1024L * 1024L;
							}
							else if (text3.ToUpper().EndsWith("MB"))
							{
								num2 = long.Parse(text3.Substring(0, text3.Length - 2)) * 1024L * 1024L;
							}
							else if (text3.ToUpper().EndsWith("G"))
							{
								num2 = long.Parse(text3.Substring(0, text3.Length - 1)) * 1024L * 1024L * 1024L;
							}
							else if (text3.ToUpper().EndsWith("GB"))
							{
								num2 = long.Parse(text3.Substring(0, text3.Length - 2)) * 1024L * 1024L * 1024L;
							}
							else
							{
								num2 = long.Parse(array[i + 2]);
							}
							selectionCriterion = new SizeCriterion
							{
								Size = num2,
								Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1])
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 10:
						case 11:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							ComparisonOperator comparisonOperator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
							if (comparisonOperator != ComparisonOperator.NotEqualTo && comparisonOperator != ComparisonOperator.EqualTo)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							string text4 = array[i + 2];
							if (text4.StartsWith("'") && text4.EndsWith("'"))
							{
								text4 = text4.Substring(1, text4.Length - 2).Replace("\u0006", " ");
							}
							selectionCriterion = new NameCriterion
							{
								MatchingFileSpec = text4,
								Operator = comparisonOperator
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 12:
						case 13:
						case 14:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							ComparisonOperator comparisonOperator2 = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
							if (comparisonOperator2 != ComparisonOperator.NotEqualTo && comparisonOperator2 != ComparisonOperator.EqualTo)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							selectionCriterion = ((text == "type") ? new TypeCriterion
							{
								AttributeString = array[i + 2],
								Operator = comparisonOperator2
							} : new AttributesCriterion
							{
								AttributeString = array[i + 2],
								Operator = comparisonOperator2
							});
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 15:
							stack.Push(FileSelector.ParseState.Whitespace);
							break;
						default:
							goto IL_07AF;
						}
						parseState = stack.Peek();
						if (parseState == FileSelector.ParseState.CriterionDone)
						{
							stack.Pop();
							if (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
							{
								while (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
								{
									CompoundCriterion compoundCriterion = stack2.Pop() as CompoundCriterion;
									compoundCriterion.Right = selectionCriterion;
									selectionCriterion = compoundCriterion;
									stack.Pop();
									parseState = stack.Pop();
									if (parseState != FileSelector.ParseState.CriterionDone)
									{
										throw new ArgumentException("??");
									}
								}
							}
							else
							{
								stack.Push(FileSelector.ParseState.CriterionDone);
							}
						}
						if (parseState == FileSelector.ParseState.Whitespace)
						{
							stack.Pop();
						}
						i++;
						continue;
					}
				}
				IL_07AF:
				throw new ArgumentException("'" + array[i] + "'");
			}
			return selectionCriterion;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004C70 File Offset: 0x00002E70
		public override string ToString()
		{
			return "FileSelector(" + this._Criterion.ToString() + ")";
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004C8C File Offset: 0x00002E8C
		private bool Evaluate(string filename)
		{
			return this._Criterion.Evaluate(filename);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004CA7 File Offset: 0x00002EA7
		[Conditional("SelectorTrace")]
		private void SelectorTrace(string format, params object[] args)
		{
			if (this._Criterion != null && this._Criterion.Verbose)
			{
				Console.WriteLine(format, args);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004CC5 File Offset: 0x00002EC5
		public ICollection<string> SelectFiles(string directory)
		{
			return this.SelectFiles(directory, false);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public ReadOnlyCollection<string> SelectFiles(string directory, bool recurseDirectories)
		{
			if (this._Criterion == null)
			{
				throw new ArgumentException("SelectionCriteria has not been set");
			}
			List<string> list = new List<string>();
			try
			{
				if (Directory.Exists(directory))
				{
					string[] files = Directory.GetFiles(directory);
					foreach (string text in files)
					{
						if (this.Evaluate(text))
						{
							list.Add(text);
						}
					}
					if (recurseDirectories)
					{
						string[] directories = Directory.GetDirectories(directory);
						foreach (string text2 in directories)
						{
							if (this.TraverseReparsePoints || (File.GetAttributes(text2) & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								if (this.Evaluate(text2))
								{
									list.Add(text2);
								}
								list.AddRange(this.SelectFiles(text2, recurseDirectories));
							}
						}
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return list.AsReadOnly();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004DC0 File Offset: 0x00002FC0
		private bool Evaluate(ZipEntry entry)
		{
			return this._Criterion.Evaluate(entry);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004DDC File Offset: 0x00002FDC
		public ICollection<ZipEntry> SelectEntries(ZipFile zip)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			foreach (ZipEntry zipEntry in zip)
			{
				if (this.Evaluate(zipEntry))
				{
					list.Add(zipEntry);
				}
			}
			return list;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004E44 File Offset: 0x00003044
		public ICollection<ZipEntry> SelectEntries(ZipFile zip, string directoryPathInArchive)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			string text = ((directoryPathInArchive == null) ? null : directoryPathInArchive.Replace("/", "\\"));
			if (text != null)
			{
				while (text.EndsWith("\\"))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			foreach (ZipEntry zipEntry in zip)
			{
				if ((directoryPathInArchive == null || Path.GetDirectoryName(zipEntry.FileName) == directoryPathInArchive || Path.GetDirectoryName(zipEntry.FileName) == text) && this.Evaluate(zipEntry))
				{
					list.Add(zipEntry);
				}
			}
			return list;
		}

		// Token: 0x0400007C RID: 124
		internal SelectionCriterion _Criterion;

		// Token: 0x02000018 RID: 24
		private enum ParseState
		{
			// Token: 0x0400007F RID: 127
			Start,
			// Token: 0x04000080 RID: 128
			OpenParen,
			// Token: 0x04000081 RID: 129
			CriterionDone,
			// Token: 0x04000082 RID: 130
			ConjunctionPending,
			// Token: 0x04000083 RID: 131
			Whitespace
		}

		// Token: 0x02000019 RID: 25
		private static class RegexAssertions
		{
			// Token: 0x04000084 RID: 132
			public static readonly string PrecededByOddNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*'[^']*)";

			// Token: 0x04000085 RID: 133
			public static readonly string FollowedByOddNumberOfSingleQuotesAndLineEnd = "(?=[^']*'(?:[^']*'[^']*')*[^']*$)";

			// Token: 0x04000086 RID: 134
			public static readonly string PrecededByEvenNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*[^']*)";

			// Token: 0x04000087 RID: 135
			public static readonly string FollowedByEvenNumberOfSingleQuotesAndLineEnd = "(?=(?:[^']*'[^']*')*[^']*$)";
		}
	}
}
