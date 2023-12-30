using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites.OrderAggregate;
using Talabat.DAL.Entities;
using Talabat.Repository.Context;

namespace Talabat.Repository.Data
{
    public static class ApplicationContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context , ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                    foreach (var brand in brands)
                    {
                        await context.Set<ProductBrand>().AddAsync(brand);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.ProductTypes.Any())
                {
                    var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                    foreach (var type in types)
                    {
                        await context.Set<ProductType>().AddAsync(type);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    foreach (var product in products)
                    {
                        await context.Set<Product>().AddAsync(product);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.DeliveryMethods.Any())
                {
                    var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                    foreach (var deliveryMethod in DeliveryMethods)
                    {
                        context.Set<DeliveryMethod>().Add(deliveryMethod);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
