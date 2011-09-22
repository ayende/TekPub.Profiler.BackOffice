using System;
using System.Web.Mvc;
using TekPub.Profiler.BackOffice.Models;
using System.Linq;

namespace TekPub.Profiler.BackOffice.Controllers
{
	public class LicenseController : RavenController
	{
		public ActionResult Validate(Guid key)
		{
			var license = documentSession.Query<License>()
				.Customize(x=>x.Include<License>(y=>y.OrderId))
				.Where(x=>x.Key == key)
				.FirstOrDefault();

			if (license == null)
				return HttpNotFound();

			var orderMessage = documentSession.Load<OrderMessage>(license.OrderId);

			switch (orderMessage.OrderType)
			{
				case "Perpetual":
					return Json(new {License = "Valid"});
			}

			return HttpNotFound();
		}
	}
}