using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001A2 RID: 418
	internal class ConnectionReturnResult
	{
		// Token: 0x06000FF6 RID: 4086 RVA: 0x00053823 File Offset: 0x00051A23
		internal ConnectionReturnResult()
		{
			this.m_Context = new List<ConnectionReturnResult.RequestContext>(5);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00053837 File Offset: 0x00051A37
		internal ConnectionReturnResult(int capacity)
		{
			this.m_Context = new List<ConnectionReturnResult.RequestContext>(capacity);
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0005384B File Offset: 0x00051A4B
		internal bool IsNotEmpty
		{
			get
			{
				return this.m_Context.Count != 0;
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0005385B File Offset: 0x00051A5B
		internal static void Add(ref ConnectionReturnResult returnResult, HttpWebRequest request, CoreResponseData coreResponseData)
		{
			if (coreResponseData == null)
			{
				throw new InternalException();
			}
			if (returnResult == null)
			{
				returnResult = new ConnectionReturnResult();
			}
			returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(request, coreResponseData));
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00053884 File Offset: 0x00051A84
		internal static void AddExceptionRange(ref ConnectionReturnResult returnResult, HttpWebRequest[] requests, Exception exception)
		{
			ConnectionReturnResult.AddExceptionRange(ref returnResult, requests, 0, exception, exception);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00053890 File Offset: 0x00051A90
		internal static void AddExceptionRange(ref ConnectionReturnResult returnResult, HttpWebRequest[] requests, int abortedPipelinedRequestIndex, Exception exception, Exception firstRequestException)
		{
			if (exception == null)
			{
				throw new InternalException();
			}
			if (returnResult == null)
			{
				returnResult = new ConnectionReturnResult(requests.Length);
			}
			for (int i = 0; i < requests.Length; i++)
			{
				if (i == abortedPipelinedRequestIndex)
				{
					returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(requests[i], firstRequestException));
				}
				else
				{
					returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(requests[i], exception));
				}
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x000538F4 File Offset: 0x00051AF4
		internal static void SetResponses(ConnectionReturnResult returnResult)
		{
			if (returnResult == null)
			{
				return;
			}
			for (int i = 0; i < returnResult.m_Context.Count; i++)
			{
				try
				{
					HttpWebRequest request = returnResult.m_Context[i].Request;
					request.SetAndOrProcessResponse(returnResult.m_Context[i].CoreResponse);
				}
				catch (Exception ex)
				{
					returnResult.m_Context.RemoveRange(0, i + 1);
					if (returnResult.m_Context.Count > 0)
					{
						ThreadPool.UnsafeQueueUserWorkItem(ConnectionReturnResult.s_InvokeConnectionCallback, returnResult);
					}
					throw;
				}
			}
			returnResult.m_Context.Clear();
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00053990 File Offset: 0x00051B90
		private static void InvokeConnectionCallback(object objectReturnResult)
		{
			ConnectionReturnResult connectionReturnResult = (ConnectionReturnResult)objectReturnResult;
			ConnectionReturnResult.SetResponses(connectionReturnResult);
		}

		// Token: 0x0400132C RID: 4908
		private static readonly WaitCallback s_InvokeConnectionCallback = new WaitCallback(ConnectionReturnResult.InvokeConnectionCallback);

		// Token: 0x0400132D RID: 4909
		private List<ConnectionReturnResult.RequestContext> m_Context;

		// Token: 0x02000747 RID: 1863
		private struct RequestContext
		{
			// Token: 0x060041E3 RID: 16867 RVA: 0x00111EA3 File Offset: 0x001100A3
			internal RequestContext(HttpWebRequest request, object coreResponse)
			{
				this.Request = request;
				this.CoreResponse = coreResponse;
			}

			// Token: 0x040031DB RID: 12763
			internal HttpWebRequest Request;

			// Token: 0x040031DC RID: 12764
			internal object CoreResponse;
		}
	}
}
