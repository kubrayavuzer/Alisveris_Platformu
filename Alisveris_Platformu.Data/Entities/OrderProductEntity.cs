using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Data.Entities
{
    public class OrderProductEntity : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        //Relational Property
        public OrderEntity Order{ get; set; }
        public ProductEntity Product{ get; set; }
    }

    public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            builder.Ignore(x => x.Id); // id sütunu tabloya aktarılmayacak
            builder.HasKey(x => new { x.OrderId, x.ProductId }); //composite key oluşturup yeni primary key olarak atandı

            // İlişkilerin tanımlanması
            builder.HasOne(x => x.Order)
                   .WithMany() // İlişkinin tam tanımını yapmak için gerekli
                   .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.Product)
                   .WithMany() // İlişkinin tam tanımını yapmak için gerekli
                   .HasForeignKey(x => x.ProductId);

            base.Configure(builder);
        }
    }

}
