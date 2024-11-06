using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }



        //Relational Property
        public ICollection<OrderProductEntity> OrderProducts { get; set; }
    }

    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.Property(x => x.TotalAmount)
                .IsRequired();
            builder.Property(x => x.Name)
                .IsRequired();

            base.Configure(builder);
        }
    }

}
