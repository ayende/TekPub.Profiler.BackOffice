using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using TekPub.Profiler.BackOffice;
using TekPub.Profiler.BackOffice.Tasks;

namespace TaskRunner
{
	class Program
	{
		static void Main()
		{
			using(var documentStore = MvcApplication.InitailizeDocumentStore())
			{
				while (true)
				{
					using(var session = documentStore.OpenSession())
					{
						var backgroundTasks = session.Query<BackgroundTask>()
							.ToList();

						if (backgroundTasks.Count == 0)
							return;

						foreach (var backgroundTask in backgroundTasks)
						{
							backgroundTask.Run(session);
							session.Delete(backgroundTask);
						}

						session.SaveChanges();
					}
				}
			}
		}
	}
}
