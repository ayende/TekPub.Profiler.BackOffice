using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TekPub.Profiler.BackOffice.Tasks
{
	public static class TaskExecuter
	{
		private static readonly ThreadLocal<List<BackgroundTask>> tasksToExecure =
			new ThreadLocal<List<BackgroundTask>>(() => new List<BackgroundTask>());

		public static void ExecuteLater(BackgroundTask task)
		{
			tasksToExecure.Value.Add(task);
		}

		public static void StartExecuting()
		{
			//foreach (var backgroundTask in tasksToExecure.Value)
			//{
			//    Task.Factory.StartNew(backgroundTask.Run);
			//    //backgroundTask.Run();
			//}

			using(var documentSession = MvcApplication.DocumentStore.OpenSession())
			{
				foreach (var backgroundTask in tasksToExecure.Value)
				{
					documentSession.Store(backgroundTask);
				}
				documentSession.SaveChanges();
			}
		}

		public static void Discard()
		{
			tasksToExecure.Value.Clear();
		}
	}
}