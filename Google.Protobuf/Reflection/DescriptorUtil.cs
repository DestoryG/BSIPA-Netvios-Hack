using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000068 RID: 104
	internal static class DescriptorUtil
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x0001A924 File Offset: 0x00018B24
		internal static IList<TOutput> ConvertAndMakeReadOnly<TInput, TOutput>(IList<TInput> input, DescriptorUtil.IndexedConverter<TInput, TOutput> converter)
		{
			TOutput[] array = new TOutput[input.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = converter(input[i], i);
			}
			return new ReadOnlyCollection<TOutput>(array);
		}

		// Token: 0x020000E7 RID: 231
		// (Invoke) Token: 0x060009EF RID: 2543
		internal delegate TOutput IndexedConverter<TInput, TOutput>(TInput element, int index);
	}
}
