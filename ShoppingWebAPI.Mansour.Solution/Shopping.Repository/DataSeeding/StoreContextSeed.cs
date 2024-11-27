using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Shopping.Core.Models;
using Shopping.Core.Models.OrderComponents;
using Shopping.Repository.Data;
using Product = Shopping.Core.Models.Product;
namespace Shopping.Repository.DataSeeding
{
    public class StoreContextSeed
    {
        public static async Task SeedProducts(StoreDbContext _dbcontext)
        {
            await SeedDataGenrics<Product>(_dbcontext, "products.json");
        }
        public static async Task SeedDeliveryMethods(StoreDbContext _dbcontext)
        {
            await SeedDataGenrics<DeliveryMethod>(_dbcontext, "delivery.json");
        }
        public static async Task seedData(StoreDbContext _dbcontext)
        {
            //check if Table has Data or not
            if (!_dbcontext.Brands.Any())
            {
                // Load Data from json file as string
                var brands = File.ReadAllText("../Shopping.Repository/DataSeedingJSONFiles/brands.json");
                var brandsAsJson = JsonSerializer.Deserialize<List<ProductBrand>>(brands);

                if (brandsAsJson != null)
                {
                    ///To Custom Json Data Second Soluation
                    ///brandsAsJson =  brandsAsJson.Select(B => new ProductBrand
                    ///{
                    ///    Name = B.Name
                    ///}).ToList();
                    foreach (var Brand in brandsAsJson)
                    {
                        /// To Custom Json Data First Soluation
                        ///var Brand = new ProductBrand
                        ///{
                        ///    Name = brand.Name
                        ///};
                        await _dbcontext.Brands.AddAsync(Brand);
                    }
                }
                await _dbcontext.SaveChangesAsync();
            }
        }
        public static async Task SeedDataGenrics<T>(StoreDbContext _dbContext, string fileName) where T : BaseEntity
        {
            if (_dbContext.Set<T>().Count() == 0)
            {
                // Load Data From Json file as Text
                var data = File.ReadAllText($"../Shopping.Repository/DataSeedingJSONFiles/{fileName}");
                var jsonData = JsonSerializer.Deserialize<List<T>>(data);

                if (jsonData?.Count() > 0)
                {
                    foreach (var item in jsonData)
                    {
                        await _dbContext.Set<T>().AddAsync(item);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
