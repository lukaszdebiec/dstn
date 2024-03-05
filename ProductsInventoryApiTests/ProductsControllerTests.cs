
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsInventoryApi.Controllers;
using ProductsInventoryApi.Data;
using ProductsInventoryApi.DataMapping;
using ProductsInventoryApi.DTOs;
using ProductsInventoryApi.Models;

namespace ProductsInventoryApi.Tests
{
    public class ProductControllerTests
    {
        private DbContextOptions<ProductDbContext> _options;

        readonly List<Product> products = new List<Product> {
                new() { Name = "Pavilion DV6", Brand = "HP", Price = 999 },
                new() { Name = "Xperia", Brand = "Sony", Price = 699 },
                new() { Name = "iPhone 12", Brand = "Apple", Price = 999 },
                new() { Name = "Galaxy S21", Brand = "Samsung", Price = 899 },
                new() { Name = "ThinkPad X1 Carbon", Brand = "Lenovo", Price = 1499 },
                new() { Name = "Surface Pro 7", Brand = "Microsoft", Price = 1299 },
                new() { Name = "MacBook Air", Brand = "Apple", Price = 999 },
                new() { Name = "Pixel 5", Brand = "Google", Price = 699 },
                new() { Name = "ROG Strix Scar 15", Brand = "Asus", Price = 1999 },
                new() { Name = "PlayStation 5", Brand = "Sony", Price = 499 }
            };

        

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ProductDbContext(_options))
            {
                context.Products.RemoveRange(context.Products);
                context.SaveChanges();

                foreach (var product in products)
                {
                    context.Products.Add(product);
                }
                context.SaveChanges();
            }
        }

        [Test]
        public async Task GetProductsTest()
        {
            // Arrange
            using (var context = new ProductDbContext(_options))
            {
                var mapper = new MapperConfiguration(config => config.AddProfile<MappingProfiles>()).CreateMapper();
                var controller = new ProductController(context, mapper);

                // Act
                var actionResult = await controller.GetProducts();

                var result = actionResult.Value;

                // Assert
                Assert.That(result, Is.Not.Null);

                TestContext.WriteLine($"Products amount from action result is equal to {result.Count}. Expected: {products.Count}");

                Assert.That(result.Count, Is.EqualTo(products.Count));
            }
        }

        [Test]
        public async Task GetProductByIdTest()
        {
            // Arrange
            using (var context = new ProductDbContext(_options))
            {
                var mapper = new MapperConfiguration(config => config.AddProfile<MappingProfiles>()).CreateMapper();
                var controller = new ProductController(context, mapper);

                // Act
                var actionResult = await controller.GetProducts();

                var products = actionResult.Value;

                // Assert
                Assert.That(products, Is.Not.Null);

                var productToReturn = products.First();

                var getProductResult = controller.GetProductById(productToReturn.Id);

                var returnedProduct = getProductResult.Result.Value;

                Assert.Multiple(() =>
                {
                    Assert.That(returnedProduct, Is.Not.Null);

                    Assert.That(products.Any(p => p.Id == returnedProduct.Id));
                });

                Assert.Multiple(() =>
                {
                    Assert.That(returnedProduct.Name, Is.EqualTo(productToReturn.Name));

                    Assert.That(returnedProduct.Brand, Is.EqualTo(productToReturn.Brand));

                    Assert.That(returnedProduct.Price, Is.EqualTo(productToReturn.Price));
                });


            }
        }

        [Test]
        public async Task CreateProduct()
        {
            // Arrange
            using (var context = new ProductDbContext(_options))
            {
                var mapper = new MapperConfiguration(config => config.AddProfile<MappingProfiles>()).CreateMapper();
                var controller = new ProductController(context, mapper);

                // Act
                var actionResult = await controller.GetProducts();

                var uniqueName = $"UniqueProduct_{Guid.NewGuid}";

                var productToCreate = new DTOs.CreateProductDto { Name = uniqueName, Brand = "NewBrand", Price = 1999 };

                await controller.CreateProduct(productToCreate);

                actionResult = await controller.GetProducts();

                var updatedProductList = actionResult.Value;

                // Assert
                Assert.That(updatedProductList, Is.Not.Null);

                TestContext.WriteLine($"Products amount from action result is equal to {updatedProductList.Count}. Expected: {products.Count + 1}");

                Assert.Multiple(() =>
                {
                    Assert.That(updatedProductList.Count, Is.EqualTo(products.Count + 1));

                    Assert.That(updatedProductList.Any(p => p.Name == uniqueName));
                });

                var addedProduct = updatedProductList.First(p => p.Name == uniqueName);

                Assert.Multiple(() =>
                {
                    Assert.That(addedProduct.Name, Is.EqualTo(productToCreate.Name));

                    Assert.That(addedProduct.Brand, Is.EqualTo(productToCreate.Brand));

                    Assert.That(addedProduct.Price, Is.EqualTo(productToCreate.Price));
                });
            }
        }

        [Test]
        public async Task UpdateProduct()
        {
            // Arrange
            using (var context = new ProductDbContext(_options))
            {
                var mapper = new MapperConfiguration(config => config.AddProfile<MappingProfiles>()).CreateMapper();

                var controller = new ProductController(context, mapper);

                Random random = new();

                UpdateProductDto updateProductDto = new() 
                {
                    Name = $"UpdateName_{Guid.NewGuid}",
                    Brand = $"UpdateName_{Guid.NewGuid}",
                    Price = random.Next(),
                };

                var actionResult = await controller.GetProducts();

                var products = actionResult.Value;

                Assert.That(products, Is.Not.Null);

                var returnedFirstProduct = products.First();

            // Act
                await controller.UpdateProduct(returnedFirstProduct.Id, updateProductDto);

                var updateResult = controller.GetProductById(returnedFirstProduct.Id);

            // Assert
                Assert.That(updateResult, Is.Not.Null);

                var updatedProduct = updateResult.Result.Value;

                Assert.That(updatedProduct, Is.Not.Null);

                Assert.Multiple(() =>
                {
                    Assert.That(updatedProduct.Name == updateProductDto.Name);
                    Assert.That(updatedProduct.Brand == updateProductDto.Brand);
                    Assert.That(updatedProduct.Price == updateProductDto.Price);
                });
            }
        }

        [Test]
        public async Task DeleteProduct()
        {
            // Arrange
            using (var context = new ProductDbContext(_options))
            {
                var mapper = new MapperConfiguration(config => config.AddProfile<MappingProfiles>()).CreateMapper();
                var controller = new ProductController(context, mapper);

            // Act
                var actionResult = await controller.GetProducts();

                var productToDelete = products[0];

                await controller.DeleteProduct(productToDelete.Id);

                actionResult = await controller.GetProducts();

                var updatedProductList = actionResult.Value;

            // Assert

                Assert.That(updatedProductList, Is.Not.Null);

                TestContext.WriteLine($"Products amount from action result is equal to {updatedProductList.Count}. Expected: {products.Count - 1}");

                Assert.Multiple(() =>
                {
                    Assert.That(updatedProductList.Count, Is.EqualTo(products.Count - 1));

                    Assert.That(!updatedProductList.Any(p => p.Id == productToDelete.Id));
                });
            }
        }
    }
}
