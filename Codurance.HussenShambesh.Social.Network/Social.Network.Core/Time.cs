namespace Social.Network.Core
{
	using System;

	public static class Time
	{
		public static string GetTimeDiff(DateTime dateTime)
		{
			string output;

			TimeSpan span = DateTime.UtcNow - dateTime;

			if (span.TotalSeconds <= 60)
			{
				output = (int)span.TotalSeconds + " second(s) ago";
			}
			else if (span.TotalMinutes <= 60)
			{
				output = (int)span.TotalMinutes + " minute(s) ago";
			}
			else if (span.TotalHours <= 60)
			{
				output = (int) span.TotalMinutes + " hour(s) ago";
			}
			else
			{
				output = (int)span.TotalDays + " day(s) ago";
			}

			return output;
		}
	}
}
