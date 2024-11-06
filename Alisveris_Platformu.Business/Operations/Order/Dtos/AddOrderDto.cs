using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Business.Operations.Order.Dtos
{
    public class AddOrderDto
    {
        public string OrderName { get; set; }

        public DateTime OrderDate { get; set; }
       
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }

        public List<int> ProductIds { get; set; }
    }
}
