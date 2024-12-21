using FluentAssertions;
using Moq;
using RentalCar.Manufacturer.Application.Handlers;
using RentalCar.Manufacturer.Application.Queries.Request;
using RentalCar.Manufacturer.Core.Entities;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;

namespace RentalCar.Manufacturer.UnitTest.Application.Queries
{
    public class FindAllManufacturerHandlerTest
    {
        [Fact]
        public async Task FindAllManufacturers_Executed_Return_List_FindManufacturerResponse()
        {
            // Arrange
            var categories = new List<Manufacturers> 
            {
                new() { Id = "Id 1", Name = "Fabricante 1", Phone = "123456789", Email = "email@email.com"},
                new() { Id = "Id 2", Name = "Fabricante 2", Phone = "123456789", Email = "email@email.com" },
                new() { Id = "Id 3", Name = "Fabricante 3", Phone = "123456789", Email = "email@email.com" },
                new() { Id = "Id 4", Name = "Fabricante 4", Phone = "123456789", Email = "email@email.com" },
            };

            var categoryRepositoryMock = new Mock<IManufacturerRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var prometheusServiceMock = new Mock<IPrometheusService>();

            categoryRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(categories);

            var findAllManufacturersHandler = new FindAllManufacturersHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);

            // Act
            var result = await findAllManufacturersHandler.Handle(new FindAllManufacturersRequest(1,10), It.IsAny<CancellationToken>());

            // Assert
            result.Datas.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();
            result.Datas.Count.Should().Be(categories.Count);

            categoryRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
