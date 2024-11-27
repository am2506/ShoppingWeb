using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models.OrderComponents;

namespace Shopping.Repository.Data.ConfigOrder
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(Item => Item.Product);
            builder.Property(Item => Item.Price)
                .HasColumnType("decimal(18,2)");
            builder.HasOne<Order>()
                .WithMany(order => order.OrderItems)
                .HasForeignKey(item => item.OrderID);
        }
    }
}
