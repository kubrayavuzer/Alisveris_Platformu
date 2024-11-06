using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
       
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }


        //relational property
        public ICollection<OrderProductEntity> OrderProducts { get; set; }
    }

    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired();
            builder.Property(x => x.Price)
                .IsRequired();
            builder.Property(x => x.StockQuantity)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
