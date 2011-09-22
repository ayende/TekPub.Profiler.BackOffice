namespace TekPub.Profiler.BackOffice.Models
{
	public class NewOrderTracking
	{
		public string Id { get; set; }
		public string OrderId { get; set; }
		public NewOrderTrackingStatus Status { get; set; }
	}
}