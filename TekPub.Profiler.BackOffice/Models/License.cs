using System;

namespace TekPub.Profiler.BackOffice.Models
{
	public class License
	{
		public virtual int Id { get; set; }
		public virtual Guid Key { get; set; }
		public virtual string OrderId { get; set; }
	}
}