﻿using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace STFU.Lib.Youtube.Interfaces.Enums
{
	public enum GoogleScope
	{
		[JsonProperty(PropertyName = "https://www.googleapis.com/auth/youtube")]
		[EnumMember(Value = "https://www.googleapis.com/auth/youtube")]
		Manage = 1,

		[JsonProperty(PropertyName = "https://www.googleapis.com/auth/youtube.force-ssl")]
		[EnumMember(Value = "https://www.googleapis.com/auth/youtube.force-ssl")]
		ManageSsl = 2,

		[JsonProperty(PropertyName = "https://www.googleapis.com/auth/youtube.readonly")]
		[EnumMember(Value = "https://www.googleapis.com/auth/youtube.readonly")]
		View = 4,

		[JsonProperty(PropertyName = "https://www.googleapis.com/auth/youtube.upload")]
		[EnumMember(Value = "https://www.googleapis.com/auth/youtube.upload")]
		Upload = 8,

		[JsonProperty(PropertyName = "https://www.googleapis.com/auth/youtubepartner")]
		[EnumMember(Value = "https://www.googleapis.com/auth/youtubepartner")]
		Partner = 16,

		[JsonProperty(PropertyName = "https://www.googleapis.com/auth/youtubepartner-channel-audit")]
		[EnumMember(Value = "https://www.googleapis.com/auth/youtubepartner-channel-audit")]
		Audit = 32,

		[JsonProperty(PropertyName = "https://www.googleapis.com/auth/gmail.send")]
		[EnumMember(Value = "https://www.googleapis.com/auth/gmail.send")]
		SendMail = 64
	}
}
