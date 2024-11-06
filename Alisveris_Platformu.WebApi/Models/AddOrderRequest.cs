using System.ComponentModel.DataAnnotations;

namespace Alisveris_Platformu.WebApi.Models
{
    public class AddOrderRequest
    {
        [Required]
        public string OrderName { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }

        public int CustomerId { get; set; }

        public List<int> ProductIds { get; set; }
    }
}
