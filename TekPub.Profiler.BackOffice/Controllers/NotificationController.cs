using System.Web.Mvc;
using TekPub.Profiler.BackOffice.Models;
using TekPub.Profiler.BackOffice.Tasks;

namespace TekPub.Profiler.BackOffice.Controllers
{
	public class NotificationController : RavenController 
	{
		public ActionResult NewOrder(string orderId)
		{
			var orderTracking = new NewOrderTracking
			{
				OrderId = orderId,
				Status = NewOrderTrackingStatus.New
			};
			documentSession.Store(orderTracking);
			TaskExecuter.ExecuteLater(new GetOrderDetailsFromPayPalTask
			{
				OrderTrackingId = orderTracking.Id
			});
			return Json(new {Succcess = true},JsonRequestBehavior.AllowGet);
		}
	}
}