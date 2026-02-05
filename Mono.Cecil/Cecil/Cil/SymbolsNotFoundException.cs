using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200012D RID: 301
	[Serializable]
	public sealed class SymbolsNotFoundException : FileNotFoundException
	{
		// Token: 0x06000B4C RID: 2892 RVA: 0x000244B8 File Offset: 0x000226B8
		public SymbolsNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0000FB66 File Offset: 0x0000DD66
		private SymbolsNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
