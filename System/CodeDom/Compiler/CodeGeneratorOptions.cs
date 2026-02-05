using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000672 RID: 1650
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class CodeGeneratorOptions
	{
		// Token: 0x17000E66 RID: 3686
		public object this[string index]
		{
			get
			{
				return this.options[index];
			}
			set
			{
				this.options[index] = value;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06003C5C RID: 15452 RVA: 0x000F9340 File Offset: 0x000F7540
		// (set) Token: 0x06003C5D RID: 15453 RVA: 0x000F936D File Offset: 0x000F756D
		public string IndentString
		{
			get
			{
				object obj = this.options["IndentString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "    ";
			}
			set
			{
				this.options["IndentString"] = value;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06003C5E RID: 15454 RVA: 0x000F9380 File Offset: 0x000F7580
		// (set) Token: 0x06003C5F RID: 15455 RVA: 0x000F93AD File Offset: 0x000F75AD
		public string BracingStyle
		{
			get
			{
				object obj = this.options["BracingStyle"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "Block";
			}
			set
			{
				this.options["BracingStyle"] = value;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06003C60 RID: 15456 RVA: 0x000F93C0 File Offset: 0x000F75C0
		// (set) Token: 0x06003C61 RID: 15457 RVA: 0x000F93E9 File Offset: 0x000F75E9
		public bool ElseOnClosing
		{
			get
			{
				object obj = this.options["ElseOnClosing"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.options["ElseOnClosing"] = value;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x000F9404 File Offset: 0x000F7604
		// (set) Token: 0x06003C63 RID: 15459 RVA: 0x000F942D File Offset: 0x000F762D
		public bool BlankLinesBetweenMembers
		{
			get
			{
				object obj = this.options["BlankLinesBetweenMembers"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.options["BlankLinesBetweenMembers"] = value;
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x000F9448 File Offset: 0x000F7648
		// (set) Token: 0x06003C65 RID: 15461 RVA: 0x000F9471 File Offset: 0x000F7671
		[ComVisible(false)]
		public bool VerbatimOrder
		{
			get
			{
				object obj = this.options["VerbatimOrder"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.options["VerbatimOrder"] = value;
			}
		}

		// Token: 0x04002C66 RID: 11366
		private IDictionary options = new ListDictionary();
	}
}
