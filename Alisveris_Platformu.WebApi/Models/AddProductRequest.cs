using System.ComponentModel.DataAnnotations;

namespace Alisveris_Platformu.WebApi.Models
{
    public class AddProductRequest
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}
