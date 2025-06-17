using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProId { get; set; }

		[Required]
		[MaxLength(20)]
		public string? ProCode { get; set; }

		[Required]
		[MaxLength(100)]
		public string? ProName { get; set; }

		public string? ProDescription { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal ProPrice { get; set; }

		[MaxLength(50)]
		public string? ProCategory { get; set; }

		public int? ProQuantity { get; set; }

		[MaxLength(20)]
		public string? ProInventoryStatus { get; set; }
	}
}
