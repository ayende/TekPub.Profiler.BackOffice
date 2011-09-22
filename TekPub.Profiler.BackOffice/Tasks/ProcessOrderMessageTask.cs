using System;
using TekPub.Profiler.BackOffice.Models;

namespace TekPub.Profiler.BackOffice.Tasks
{
	public class ProcessOrderMessageTask : BackgroundTask
	{
		public string OrderId { get; set; }
		
		public override void Execute()
		{
			var orderMessage = documentSession.Load<OrderMessage>(OrderId);

			var license = new License
			{
				Key = Guid.NewGuid(),
				OrderId = orderMessage.Id
			};
			documentSession.Store(license);

			TaskExecuter.ExecuteLater(new SendEmailTask
			{
				From = "orders@tekpub.com",
				To = orderMessage.Email,
				Subject = "Congrats on your new tekpub thingie",
				Template = "NewOrder",
				ViewContext = new
				{
					orderMessage.OrderId,
					orderMessage.Name,
					orderMessage.Amount,
					LiceseKey = license.Key
				}
			});

			orderMessage.Handled = true;
		}


		public override string ToString()
		{
			return string.Format("OrderId: {0}", OrderId);
		}
	}
}