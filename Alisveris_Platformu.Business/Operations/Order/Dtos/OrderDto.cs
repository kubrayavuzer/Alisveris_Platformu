﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Business.Operations.Order.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderProductDto> Products { get; set; }
    }
}
