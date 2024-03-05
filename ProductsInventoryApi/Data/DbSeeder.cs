
using Microsoft.EntityFrameworkCore;
using ProductsInventoryApi.Models;

namespace ProductsInventoryApi.Data
{
    public class DbSeeder
    {
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            SeedData(scope.ServiceProvider.GetService<ProductDbContext>());
        }

        private static void SeedData(ProductDbContext context)
        {
            context.Database.Migrate();

            if(context.Products.Any())
            {
                Console.WriteLine("Alreadyt have data - no need to seed!");
                return;
            }

            var products = new List<Product>()
            {
                new() 
                {
                    Name = "Iphone 6 64gb",
                    Brand = "Apple",
                    Price = 680
                },
                new() 
                {
                    Name = "Macbook pro 13'",
                    Brand = "Apple",
                    Price = 1200
                },
                new() 
                {
                    Name = "ProBook 450 G6 15.6'",
                    Brand = "HP",
                    Price = 615
                },
                new() 
                {
                    Name = "360 1030 13.3'",
                    Brand = "HP",
                    Price = 1309
                },
                new() 
                {
                    Name = "P2219H 21.5",
                    Brand = "Dell",
                    Price = 129
                },
                new() 
                {
                    Name = "UltraSharp U2412M 24",
                    Brand = "Dell",
                    Price = 168
                },
                new() 
                {
                    Name = "ProLiant MicroGen10 Server",
                    Brand = "HP",
                    Price = 321
                },
                new() 
                {
                    Name = "Enterprise ML350 Gen10 Server",
                    Brand = "HP",
                    Price = 1591
                },
                new() 
                {
                    Name = "R440 Server",
                    Brand = "Dell",
                    Price = 1591
                }
                ,
                new()
                {
                    Name = "ThinkPad T14",
                    Brand = "Lenovo",
                    Price = 1299
                },
                new()
                {
                    Name = "ThinkPad T11",
                    Brand = "Dell",
                    Price = 999
                },
                new()
                {
                    Name = "Eizo 27' 4K",
                    Brand = "Eizo",
                    Price = 1199
                }
                ,
                new()
                {
                    Name = "Elitebook G5 15'",
                    Brand = "HP",
                    Price = 1322
                }
            };

            context.AddRange(products);

            context.SaveChanges();
        }

    }
}