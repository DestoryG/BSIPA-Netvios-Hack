using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F2 RID: 498
	[Serializable]
	internal sealed class SymbolsNotFoundException : FileNotFoundException
	{
		// Token: 0x06000F3C RID: 3900 RVA: 0x00033870 File Offset: 0x00031A70
		public SymbolsNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0001E102 File Offset: 0x0001C302
		private SymbolsNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
