using System;
using System.Collections.Generic;
using System.Text;

namespace Elasticsearch.Net
{
	public class ApiCallDetails : IApiCallDetails
	{
		public bool Success { get; set; }
		public string ResponseMimeType { get; set; }
		public Exception OriginalException { get; set; }
		public ServerError ServerError { get; set; }
		public HttpMethod HttpMethod { get; set; }
		public Uri Uri { get; set; }
		public int? HttpStatusCode { get; set; }
		public bool SuccessOrKnownError =>
			this.Success || (
				HttpStatusCode >= 400 && HttpStatusCode < 599
				&& HttpStatusCode != 504 //Gateway timeout needs to be retried
				&& HttpStatusCode != 503 //service unavailable needs to be retried
				&& HttpStatusCode != 502 //bad gateway needs to be retried
			);
		public byte[] ResponseBodyInBytes { get; set; }
		public byte[] RequestBodyInBytes { get; set; }
		public List<Audit> AuditTrail { get; set; }
		public IEnumerable<string> DeprecationWarnings { get; set; }

		public string DebugInformation
		{
			get
			{
				var sb = new StringBuilder();
				sb.AppendLine(this.ToString());
				return ResponseStatics.DebugInformationBuilder(this, sb);
			}
		}

		public override string ToString() =>
			$"{(Success ? "S" : "Uns")}uccessful low level call on {HttpMethod.GetStringValue()}: {Uri.PathAndQuery}";
	}
}
