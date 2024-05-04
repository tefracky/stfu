namespace STFU.Lib.Youtube.Services
{
	public class QuotaErrorResponse
	{
		public QuotaError Error { get; set; }
	}

	public class QuotaError
	{
		public int Code { get; set; }
		public string Message { get; set; }

		public QuotaErrorDetails[] Errors { get; set; }
	}

	public class QuotaErrorDetails
	{
		public string Message { get; set; }
		public string Domain { get; set; }
		public string Reason { get; set; }
		public string DebugInfo { get; set; }
	}

	//	string error = @"{
	//  "error": {

	//    "code": 403,
	//    "message": "The request cannot be completed because you have exceeded your \u003ca href=\"/youtube/v3/getting-started#quota\"\u003equota\u003c/a\u003e.",
	//    "errors": [
	//      {
	//        "message": "The request cannot be completed because you have exceeded your \u003ca href=\"/youtube/v3/getting-started#quota\"\u003equota\u003c/a\u003e.",
	//        "domain": "youtube.quota",
	//        "reason": "quotaExceeded",
	//        "debugInfo": "Code: 8; Description: ?metric=youtube.googleapis.com/default&limit=defaultPerDayPerProject&qs_error_code=INSUFFICIENT_TOKENS"

	//	  }
	//    ]
	//  }
	//}
	//"
}
