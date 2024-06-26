﻿using System;
using STFU.Lib.Youtube.Automation.Interfaces.Model;

namespace STFU.Lib.Youtube.Automation.Templates
{
	public class PublishTime : IPublishTime
	{
		public DayOfWeek DayOfWeek { get; set; }

		public TimeSpan Time { get; set; }

		public int SkipDays { get; set; }

		public override string ToString()
		{
			return $"{DayOfWeek} {Time:hh\\:mm} +{SkipDays}";
		}
	}
}
