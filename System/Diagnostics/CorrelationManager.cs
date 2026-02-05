using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace System.Diagnostics
{
	// Token: 0x02000495 RID: 1173
	public class CorrelationManager
	{
		// Token: 0x06002B5E RID: 11102 RVA: 0x000C4E52 File Offset: 0x000C3052
		internal CorrelationManager()
		{
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x000C4E5C File Offset: 0x000C305C
		// (set) Token: 0x06002B60 RID: 11104 RVA: 0x000C4E83 File Offset: 0x000C3083
		public Guid ActivityId
		{
			get
			{
				object obj = CallContext.LogicalGetData("E2ETrace.ActivityID");
				if (obj != null)
				{
					return (Guid)obj;
				}
				return Guid.Empty;
			}
			set
			{
				CallContext.LogicalSetData("E2ETrace.ActivityID", value);
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x000C4E95 File Offset: 0x000C3095
		public Stack LogicalOperationStack
		{
			get
			{
				return this.GetLogicalOperationStack();
			}
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000C4EA0 File Offset: 0x000C30A0
		public void StartLogicalOperation(object operationId)
		{
			if (operationId == null)
			{
				throw new ArgumentNullException("operationId");
			}
			Stack logicalOperationStack = this.GetLogicalOperationStack();
			logicalOperationStack.Push(operationId);
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000C4EC9 File Offset: 0x000C30C9
		public void StartLogicalOperation()
		{
			this.StartLogicalOperation(Guid.NewGuid());
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000C4EDC File Offset: 0x000C30DC
		public void StopLogicalOperation()
		{
			Stack logicalOperationStack = this.GetLogicalOperationStack();
			logicalOperationStack.Pop();
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000C4EF8 File Offset: 0x000C30F8
		private Stack GetLogicalOperationStack()
		{
			Stack stack = CallContext.LogicalGetData("System.Diagnostics.Trace.CorrelationManagerSlot") as Stack;
			if (stack == null)
			{
				stack = new Stack();
				CallContext.LogicalSetData("System.Diagnostics.Trace.CorrelationManagerSlot", stack);
			}
			return stack;
		}

		// Token: 0x0400267B RID: 9851
		private const string transactionSlotName = "System.Diagnostics.Trace.CorrelationManagerSlot";

		// Token: 0x0400267C RID: 9852
		private const string activityIdSlotName = "E2ETrace.ActivityID";
	}
}
