using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000681 RID: 1665
	public class IndentedTextWriter : TextWriter
	{
		// Token: 0x06003D41 RID: 15681 RVA: 0x000FBB53 File Offset: 0x000F9D53
		public IndentedTextWriter(TextWriter writer)
			: this(writer, "    ")
		{
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x000FBB61 File Offset: 0x000F9D61
		public IndentedTextWriter(TextWriter writer, string tabString)
			: base(CultureInfo.InvariantCulture)
		{
			this.writer = writer;
			this.tabString = tabString;
			this.indentLevel = 0;
			this.tabsPending = false;
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06003D43 RID: 15683 RVA: 0x000FBB8A File Offset: 0x000F9D8A
		public override Encoding Encoding
		{
			get
			{
				return this.writer.Encoding;
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06003D44 RID: 15684 RVA: 0x000FBB97 File Offset: 0x000F9D97
		// (set) Token: 0x06003D45 RID: 15685 RVA: 0x000FBBA4 File Offset: 0x000F9DA4
		public override string NewLine
		{
			get
			{
				return this.writer.NewLine;
			}
			set
			{
				this.writer.NewLine = value;
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06003D46 RID: 15686 RVA: 0x000FBBB2 File Offset: 0x000F9DB2
		// (set) Token: 0x06003D47 RID: 15687 RVA: 0x000FBBBA File Offset: 0x000F9DBA
		public int Indent
		{
			get
			{
				return this.indentLevel;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.indentLevel = value;
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06003D48 RID: 15688 RVA: 0x000FBBCA File Offset: 0x000F9DCA
		public TextWriter InnerWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06003D49 RID: 15689 RVA: 0x000FBBD2 File Offset: 0x000F9DD2
		internal string TabString
		{
			get
			{
				return this.tabString;
			}
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x000FBBDA File Offset: 0x000F9DDA
		public override void Close()
		{
			this.writer.Close();
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x000FBBE7 File Offset: 0x000F9DE7
		public override void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x000FBBF4 File Offset: 0x000F9DF4
		protected virtual void OutputTabs()
		{
			if (this.tabsPending)
			{
				for (int i = 0; i < this.indentLevel; i++)
				{
					this.writer.Write(this.tabString);
				}
				this.tabsPending = false;
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x000FBC32 File Offset: 0x000F9E32
		public override void Write(string s)
		{
			this.OutputTabs();
			this.writer.Write(s);
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x000FBC46 File Offset: 0x000F9E46
		public override void Write(bool value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x000FBC5A File Offset: 0x000F9E5A
		public override void Write(char value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x000FBC6E File Offset: 0x000F9E6E
		public override void Write(char[] buffer)
		{
			this.OutputTabs();
			this.writer.Write(buffer);
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x000FBC82 File Offset: 0x000F9E82
		public override void Write(char[] buffer, int index, int count)
		{
			this.OutputTabs();
			this.writer.Write(buffer, index, count);
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x000FBC98 File Offset: 0x000F9E98
		public override void Write(double value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x000FBCAC File Offset: 0x000F9EAC
		public override void Write(float value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x000FBCC0 File Offset: 0x000F9EC0
		public override void Write(int value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x000FBCD4 File Offset: 0x000F9ED4
		public override void Write(long value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x000FBCE8 File Offset: 0x000F9EE8
		public override void Write(object value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x000FBCFC File Offset: 0x000F9EFC
		public override void Write(string format, object arg0)
		{
			this.OutputTabs();
			this.writer.Write(format, arg0);
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x000FBD11 File Offset: 0x000F9F11
		public override void Write(string format, object arg0, object arg1)
		{
			this.OutputTabs();
			this.writer.Write(format, arg0, arg1);
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x000FBD27 File Offset: 0x000F9F27
		public override void Write(string format, params object[] arg)
		{
			this.OutputTabs();
			this.writer.Write(format, arg);
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x000FBD3C File Offset: 0x000F9F3C
		public void WriteLineNoTabs(string s)
		{
			this.writer.WriteLine(s);
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x000FBD4A File Offset: 0x000F9F4A
		public override void WriteLine(string s)
		{
			this.OutputTabs();
			this.writer.WriteLine(s);
			this.tabsPending = true;
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x000FBD65 File Offset: 0x000F9F65
		public override void WriteLine()
		{
			this.OutputTabs();
			this.writer.WriteLine();
			this.tabsPending = true;
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x000FBD7F File Offset: 0x000F9F7F
		public override void WriteLine(bool value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x000FBD9A File Offset: 0x000F9F9A
		public override void WriteLine(char value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x000FBDB5 File Offset: 0x000F9FB5
		public override void WriteLine(char[] buffer)
		{
			this.OutputTabs();
			this.writer.WriteLine(buffer);
			this.tabsPending = true;
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x000FBDD0 File Offset: 0x000F9FD0
		public override void WriteLine(char[] buffer, int index, int count)
		{
			this.OutputTabs();
			this.writer.WriteLine(buffer, index, count);
			this.tabsPending = true;
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x000FBDED File Offset: 0x000F9FED
		public override void WriteLine(double value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x000FBE08 File Offset: 0x000FA008
		public override void WriteLine(float value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x000FBE23 File Offset: 0x000FA023
		public override void WriteLine(int value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x000FBE3E File Offset: 0x000FA03E
		public override void WriteLine(long value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x000FBE59 File Offset: 0x000FA059
		public override void WriteLine(object value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x000FBE74 File Offset: 0x000FA074
		public override void WriteLine(string format, object arg0)
		{
			this.OutputTabs();
			this.writer.WriteLine(format, arg0);
			this.tabsPending = true;
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x000FBE90 File Offset: 0x000FA090
		public override void WriteLine(string format, object arg0, object arg1)
		{
			this.OutputTabs();
			this.writer.WriteLine(format, arg0, arg1);
			this.tabsPending = true;
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x000FBEAD File Offset: 0x000FA0AD
		public override void WriteLine(string format, params object[] arg)
		{
			this.OutputTabs();
			this.writer.WriteLine(format, arg);
			this.tabsPending = true;
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x000FBEC9 File Offset: 0x000FA0C9
		[CLSCompliant(false)]
		public override void WriteLine(uint value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x000FBEE4 File Offset: 0x000FA0E4
		internal void InternalOutputTabs()
		{
			for (int i = 0; i < this.indentLevel; i++)
			{
				this.writer.Write(this.tabString);
			}
		}

		// Token: 0x04002CAE RID: 11438
		private TextWriter writer;

		// Token: 0x04002CAF RID: 11439
		private int indentLevel;

		// Token: 0x04002CB0 RID: 11440
		private bool tabsPending;

		// Token: 0x04002CB1 RID: 11441
		private string tabString;

		// Token: 0x04002CB2 RID: 11442
		public const string DefaultTabString = "    ";
	}
}
