using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using TekPub.Profiler.BackOffice.Models;

namespace TekPub.Profiler.BackOffice.Tasks
{
	public class GetOrderDetailsFromPayPalTask : BackgroundTask
	{
		public string OrderTrackingId { get; set; }

		public override void Execute()
		{
			var newOrderTracking = documentSession.Load<NewOrderTracking>(OrderTrackingId);
			if (newOrderTracking == null)
			{
				log.Warn("Could not find order with id: {0}", OrderTrackingId);
				return;
			}
			if (newOrderTracking.Status != NewOrderTrackingStatus.New)
			{
				log.Info("Order {0} was already processed", OrderTrackingId);
				return;
			}
			var request = WebRequest.Create("https://orders.paypal.com/orders?orderid=" + newOrderTracking.OrderId);
			WebResponse webResponse;
			try
			{
				webResponse = request.GetResponse();
			}
			catch (WebException e)
			{
				var httpWebResponse = e.Response as HttpWebResponse;
				if (httpWebResponse == null ||
					httpWebResponse.StatusCode != HttpStatusCode.NotFound)
					throw;

				// 404 - order not found

				newOrderTracking.Status = NewOrderTrackingStatus.Fraud;

				return;
			}
			using(var response = webResponse)
			using(var stream = response.GetResponseStream())
			using(var reader = new StreamReader(stream))
			{
				var jsonSerializer = new JsonSerializer();
				var orderMessage = jsonSerializer.Deserialize<OrderMessage>(new JsonTextReader(reader));

				newOrderTracking.Status = NewOrderTrackingStatus.Verified;

				documentSession.Store(orderMessage);

				TaskExecuter.ExecuteLater(new ProcessOrderMessageTask
				{
					OrderId = orderMessage.Id
				});
			}
		}

		public override string ToString()
		{
			return string.Format("OrderTrackingId: {0}", OrderTrackingId);
		}
	}
}