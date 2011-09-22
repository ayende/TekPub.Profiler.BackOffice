using System;
using NLog;
using Raven.Client;

namespace TekPub.Profiler.BackOffice.Tasks
{
	public abstract class BackgroundTask
	{
		protected Logger log = LogManager.GetCurrentClassLogger();
		protected IDocumentSession documentSession;

		protected virtual void Initialize(IDocumentSession session)
		{
			documentSession = session;
			documentSession.Advanced.UseOptimisticConcurrency = true;
		}

		protected abstract void Execute();

		public bool Run(IDocumentSession openSession)
		{
			Initialize(openSession);
			try
			{
				Execute();
				documentSession.SaveChanges();
				TaskExecuter.StartExecuting();
				return true;
			}
			catch (Exception e)
			{
				log.ErrorException("Error processing task:\r\n" + ToString(), e);
				return false;
			}
			finally
			{
				TaskExecuter.Discard();
			}
		}

		public override abstract string ToString();
	}
}