namespace technicaltest.Models
{
	public class Product 
	{
		public Int32 Id {get; set;}
		public string? Name {get; set;}
		public string? Description {get; set;}
		public decimal Price {get; set;}
		public DateTime? CreatedAt {get; set;}
	}
}
