namespace TekPub.Profiler.BackOffice.Models
{
	public class OrderMessage
	{
		public string Id { get; set; }
		public string OrderId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string OrderType { get; set; }
		public decimal Amount { get; set; }
		public bool Handled { get; set; }
	}
}