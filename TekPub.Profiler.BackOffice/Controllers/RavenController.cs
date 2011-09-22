using System;
using System.Web.Mvc;
using Raven.Client;
using TekPub.Profiler.BackOffice.Tasks;

namespace TekPub.Profiler.BackOffice.Controllers
{
	public class RavenController : Controller
	{
		protected IDocumentSession documentSession;

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			documentSession = MvcApplication.DocumentStore.OpenSession();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			try
			{
				using(documentSession)
				{
					if(filterContext.Exception == null)
					{
						documentSession.SaveChanges();
						TaskExecuter.StartExecuting();
					}
				}
			}
			finally
			{
				TaskExecuter.Discard();
			}
		}
	}
}