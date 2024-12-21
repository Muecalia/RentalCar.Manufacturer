using FluentAssertions;
using Moq;
using RentalCar.Manufacturer.Application.Handlers;
using RentalCar.Manufacturer.Application.Queries.Request;
using RentalCar.Manufacturer.Core.Entities;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;

namespace RentalCar.Manufacturer.UnitTest.Application.Queries
{
    public class FindManufacturerByIdHandlerTest
    {
        [Fact]
        public async Task FindManufacturerById_Executed_Return_FindManufacturerResponse()
        {
            // Arrange
            var manufacturer = new Manufacturers
            {
                Id = "Id",
                Name = "Fabricante",
                Phone = "123456789",
                Email = "email@email.com"
            };

            var categoryRepositoryMock = new Mock<IManufacturerRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var prometheusServiceMock = new Mock<IPrometheusService>();

            categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(manufacturer);

            var findManufacturerByIdHandler = new FindManufacturerByIdHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);

            // Act
            var result = await findManufacturerByIdHandler.Handle(new FindManufacturerByIdRequest("Id"), It.IsAny<CancellationToken>());

            // Assert
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
