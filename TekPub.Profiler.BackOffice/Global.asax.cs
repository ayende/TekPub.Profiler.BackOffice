using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using TekPub.Profiler.BackOffice.Tasks;

namespace TekPub.Profiler.BackOffice
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static IDocumentStore DocumentStore;

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			DocumentStore = DocumentStoreInitializer();
		}

		public static IDocumentStore DocumentStoreInitializer()
		{
			var documentStore = new DocumentStore
			{
				Url="http://localhost:8080"
			}.Initialize();

			var defaultBehavior = DocumentStore.Conventions.FindTypeTagName;
			DocumentStore.Conventions.FindTypeTagName = type =>
			{
				if (typeof(BackgroundTask).IsAssignableFrom(type))
					return defaultBehavior(typeof (BackgroundTask));
				return defaultBehavior(type);
			};

			return documentStore;
		}
	}
}