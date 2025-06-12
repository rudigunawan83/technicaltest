using Grpc.Core;
using Moq;
using NUnit.Framework;
using technicaltest.Models;
using technicaltest.Protos;
using technicaltest.Repositories;
using technicaltest.Repositories.Cache;
using technicaltest.Repositories.MySql;
using Tests.Server.UnitTests.Helpers;
using FluentValidation;
using AutoMapper;
using technicaltest.UseCases;

namespace technicaltest.Tests.UnitTests.UseCases
{
   public class ProductUseCaseTest
   {
       private Mock<IProductDb>? mockDatabase;
       private Mock<IProductCache>? mockCache;
       private Mock<IProductRepository> mockRepo;
       private Mock<IValidator<Models.Product>> mockValidator;
       private Mock<IMapper> mockMapper;
       private TestServerCallContext? context;
       private ProductUseCase? useCase;

       [SetUp]
       public void Setup()
       {
           mockDatabase = new Mock<IProductDb>();
           mockCache = new Mock<IProductCache>();
           mockRepo = new Mock<IProductRepository>();
           mockValidator = new Mock<IValidator<Models.Product>>();
           mockMapper = new Mock<IMapper>();
           context = TestServerCallContext.Create();
           useCase = new ProductUseCase(mockRepo.Object, mockValidator.Object, mockMapper.Object);
       }

       [Test]
       public async Task CreateProduct_ReturnOk()
       {
          //Arrange
          ProductModel request = new();
          Product product = new() { Id = 0,Name = "",Description = "",Price = 1000000,CreatedAt = new DateTime() };

          //mock
          mockDatabase?.Setup(r => r.Add(product)).ReturnsAsync(product);
          mockDatabase?.Setup(r => r.Update(product)).ReturnsAsync(product);
          mockCache?.Setup(c => c.SetCache(product)).ReturnsAsync(true);

          // Act
          var response = await useCase!.Add(request, context!);

          // Assert
          mockDatabase?.Verify(d => d.Add(It.IsAny<Product>()), Times.Once);
          mockDatabase?.Verify(d => d.Update(It.IsAny<Product>()), Times.Once);
          mockCache?.Verify(c => c.SetCache(It.IsAny<Product>()), Times.Once);
          Assert.IsNotNull(response);
          Assert.AreEqual(product, response.ToString());
          Assert.AreEqual(StatusCode.OK, context?.Status.StatusCode);
       }
   }
}
