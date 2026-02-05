using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;

namespace System.Net.Mime
{
	// Token: 0x0200024A RID: 586
	internal class MimeMultiPart : MimeBasePart
	{
		// Token: 0x06001634 RID: 5684 RVA: 0x00073174 File Offset: 0x00071374
		internal MimeMultiPart(MimeMultiPartType type)
		{
			this.MimeMultiPartType = type;
		}

		// Token: 0x170004A9 RID: 1193
		// (set) Token: 0x06001635 RID: 5685 RVA: 0x00073183 File Offset: 0x00071383
		internal MimeMultiPartType MimeMultiPartType
		{
			set
			{
				if (value > MimeMultiPartType.Related || value < MimeMultiPartType.Mixed)
				{
					throw new NotSupportedException(value.ToString());
				}
				this.SetType(value);
			}
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000731A7 File Offset: 0x000713A7
		private void SetType(MimeMultiPartType type)
		{
			base.ContentType.MediaType = "multipart/" + type.ToString().ToLower(CultureInfo.InvariantCulture);
			base.ContentType.Boundary = this.GetNextBoundary();
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001637 RID: 5687 RVA: 0x000731E6 File Offset: 0x000713E6
		internal Collection<MimeBasePart> Parts
		{
			get
			{
				if (this.parts == null)
				{
					this.parts = new Collection<MimeBasePart>();
				}
				return this.parts;
			}
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00073204 File Offset: 0x00071404
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			if (mimePartContext.completed)
			{
				throw e;
			}
			try
			{
				mimePartContext.outputStream.Close();
			}
			catch (Exception ex)
			{
				if (e == null)
				{
					e = ex;
				}
			}
			mimePartContext.completed = true;
			mimePartContext.result.InvokeCallback(e);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00073264 File Offset: 0x00071464
		internal void MimeWriterCloseCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.MimeWriterCloseCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x000732B0 File Offset: 0x000714B0
		private void MimeWriterCloseCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			((MimeWriter)mimePartContext.writer).EndClose(result);
			this.Complete(result, null);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000732E4 File Offset: 0x000714E4
		internal void MimePartSentCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.MimePartSentCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00073330 File Offset: 0x00071530
		private void MimePartSentCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			MimeBasePart mimeBasePart = mimePartContext.partsEnumerator.Current;
			mimeBasePart.EndSend(result);
			if (mimePartContext.partsEnumerator.MoveNext())
			{
				mimeBasePart = mimePartContext.partsEnumerator.Current;
				IAsyncResult asyncResult = mimeBasePart.BeginSend(mimePartContext.writer, this.mimePartSentCallback, this.allowUnicode, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext.writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x000733CC File Offset: 0x000715CC
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ContentStreamCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00073418 File Offset: 0x00071618
		private void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			mimePartContext.outputStream = mimePartContext.writer.EndGetContentStream(result);
			mimePartContext.writer = new MimeWriter(mimePartContext.outputStream, base.ContentType.Boundary);
			if (mimePartContext.partsEnumerator.MoveNext())
			{
				MimeBasePart mimeBasePart = mimePartContext.partsEnumerator.Current;
				this.mimePartSentCallback = new AsyncCallback(this.MimePartSentCallback);
				IAsyncResult asyncResult = mimeBasePart.BeginSend(mimePartContext.writer, this.mimePartSentCallback, this.allowUnicode, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext.writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x000734E4 File Offset: 0x000716E4
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, bool allowUnicode, object state)
		{
			this.allowUnicode = allowUnicode;
			base.PrepareHeaders(allowUnicode);
			writer.WriteHeaders(base.Headers, allowUnicode);
			MimeBasePart.MimePartAsyncResult mimePartAsyncResult = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimeMultiPart.MimePartContext mimePartContext = new MimeMultiPart.MimePartContext(writer, mimePartAsyncResult, this.Parts.GetEnumerator());
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return mimePartAsyncResult;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00073550 File Offset: 0x00071750
		internal override void Send(BaseWriter writer, bool allowUnicode)
		{
			base.PrepareHeaders(allowUnicode);
			writer.WriteHeaders(base.Headers, allowUnicode);
			Stream contentStream = writer.GetContentStream();
			MimeWriter mimeWriter = new MimeWriter(contentStream, base.ContentType.Boundary);
			foreach (MimeBasePart mimeBasePart in this.Parts)
			{
				mimeBasePart.Send(mimeWriter, allowUnicode);
			}
			mimeWriter.Close();
			contentStream.Close();
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x000735D8 File Offset: 0x000717D8
		internal string GetNextBoundary()
		{
			return "--boundary_" + (Interlocked.Increment(ref MimeMultiPart.boundary) - 1).ToString(CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x04001721 RID: 5921
		private Collection<MimeBasePart> parts;

		// Token: 0x04001722 RID: 5922
		private static int boundary;

		// Token: 0x04001723 RID: 5923
		private AsyncCallback mimePartSentCallback;

		// Token: 0x04001724 RID: 5924
		private bool allowUnicode;

		// Token: 0x02000797 RID: 1943
		internal class MimePartContext
		{
			// Token: 0x060042D3 RID: 17107 RVA: 0x00117D3B File Offset: 0x00115F3B
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result, IEnumerator<MimeBasePart> partsEnumerator)
			{
				this.writer = writer;
				this.result = result;
				this.partsEnumerator = partsEnumerator;
			}

			// Token: 0x04003381 RID: 13185
			internal IEnumerator<MimeBasePart> partsEnumerator;

			// Token: 0x04003382 RID: 13186
			internal Stream outputStream;

			// Token: 0x04003383 RID: 13187
			internal LazyAsyncResult result;

			// Token: 0x04003384 RID: 13188
			internal BaseWriter writer;

			// Token: 0x04003385 RID: 13189
			internal bool completed;

			// Token: 0x04003386 RID: 13190
			internal bool completedSynchronously = true;
		}
	}
}
