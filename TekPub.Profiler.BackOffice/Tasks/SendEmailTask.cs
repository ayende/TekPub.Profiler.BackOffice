using System;

namespace TekPub.Profiler.BackOffice.Tasks
{
	public class SendEmailTask : BackgroundTask
	{
		public string To { get; set; }
		public string From { get; set; }
		public string Template { get; set; }
		public object ViewContext { get; set; }
		public string Subject { get; set; }
		public string[] Attachments { get; set; }

		public override string ToString()
		{
			return string.Format("To: {0}, From: {1}, Template: {2}, ViewContext: {3}, Subject: {4}", To, From, Template, ViewContext, Subject);
		}

		public override void Execute()
		{
			
		}

	}
}