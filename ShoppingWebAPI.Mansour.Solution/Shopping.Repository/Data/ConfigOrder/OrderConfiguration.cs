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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Make Address class as composite Attribute
            builder.OwnsOne(order => order.ShippingAddress);

            //Make enum OrderStatus save in Database as String
            builder.Property(order => order.OrderStatus)
                .HasConversion(status => status.ToString(),
                status => (OrderStatus)Enum.Parse(typeof(OrderStatus), status));

            // Set DeliveryMethod Relation
            builder.HasOne(order => order.DeliveryMethod)
                .WithMany()
                .HasForeignKey(order => order.DeliveryMethodId);

            builder.HasMany(order => order.OrderItems)
                .WithOne();

            //Configure decimal property
            builder.Property(order => order.SubTotal)
               .HasColumnType("decimal(18,2)");

        }
    }
}
