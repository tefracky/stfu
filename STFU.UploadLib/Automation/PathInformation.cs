﻿using STFU.UploadLib.Templates;

namespace STFU.UploadLib.Automation
{
	public class PathInformation
	{
		public string Path { get; set; }
		public string Filter { get; set; }
		public bool SearchRecursively { get; set; }
		public string SelectedTemplate { get; set; }
	}
}
