using System;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200060B RID: 1547
	public interface IDesignerSerializationManager : IServiceProvider
	{
		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x060038B3 RID: 14515
		ContextStack Context { get; }

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x060038B4 RID: 14516
		PropertyDescriptorCollection Properties { get; }

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x060038B5 RID: 14517
		// (remove) Token: 0x060038B6 RID: 14518
		event ResolveNameEventHandler ResolveName;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x060038B7 RID: 14519
		// (remove) Token: 0x060038B8 RID: 14520
		event EventHandler SerializationComplete;

		// Token: 0x060038B9 RID: 14521
		void AddSerializationProvider(IDesignerSerializationProvider provider);

		// Token: 0x060038BA RID: 14522
		object CreateInstance(Type type, ICollection arguments, string name, bool addToContainer);

		// Token: 0x060038BB RID: 14523
		object GetInstance(string name);

		// Token: 0x060038BC RID: 14524
		string GetName(object value);

		// Token: 0x060038BD RID: 14525
		object GetSerializer(Type objectType, Type serializerType);

		// Token: 0x060038BE RID: 14526
		Type GetType(string typeName);

		// Token: 0x060038BF RID: 14527
		void RemoveSerializationProvider(IDesignerSerializationProvider provider);

		// Token: 0x060038C0 RID: 14528
		void ReportError(object errorInformation);

		// Token: 0x060038C1 RID: 14529
		void SetName(object instance, string name);
	}
}
