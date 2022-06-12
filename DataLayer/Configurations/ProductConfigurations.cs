using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class ProductConfigurations:IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Price).HasPrecision(9, 2);
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
