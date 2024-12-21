using FluentAssertions;
using Moq;
using RentalCar.Manufacturer.Application.Commands.Request;
using RentalCar.Manufacturer.Application.Handlers;
using RentalCar.Manufacturer.Core.Entities;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;

namespace RentalCar.Manufacturer.UnitTest.Application.Commands
{
    public class CreateManufacturerHandlerTest
    {
        [Fact]
        public async Task CreateManufacturer_Executed_return_InputManufacturerResponse()
        {
            // Arrange
            var category = new Manufacturers
            {
                Id = "123",
                Name = "Fabricante",
                Phone = "123456789",
                Email = "email@email.com"
            };

            var createManufacturerRequest = new CreateManufacturerRequest 
            {
                Name = "Fabricante",
                Phone = "123456789",
                Email = "email@email.com"
            };

            var categoryRepositoryMock = new Mock<IManufacturerRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var prometheusServiceMock = new Mock<IPrometheusService>();

            categoryRepositoryMock.Setup(repo => repo.IsManufacturerExist(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            categoryRepositoryMock.Setup(repo => repo.Create(It.IsAny<Manufacturers>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);

            var createManufacturerHandler = new CreateManufacturerHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);

            // Act
            var result = await createManufacturerHandler.Handle(createManufacturerRequest, It.IsAny<CancellationToken>());

            // Assert
            result.Data.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();

            categoryRepositoryMock.Verify(repo => repo.IsManufacturerExist(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            categoryRepositoryMock.Verify(repo => repo.Create(It.IsAny<Manufacturers>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
