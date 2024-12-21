using FluentAssertions;
using Moq;
using RentalCar.Manufacturer.Application.Commands.Request;
using RentalCar.Manufacturer.Application.Handlers;
using RentalCar.Manufacturer.Core.Entities;
using RentalCar.Manufacturer.Core.Repositories;
using RentalCar.Manufacturer.Core.Services;

namespace RentalCar.Manufacturer.UnitTest.Application.Commands
{
    public class UpdadeManufacturerHandlerTest
    {
        [Fact]
        public async Task UpdadeManufacturer_Executed_Return_InputManufacturerResponse()
        {
            // Arrange
            var category = new Manufacturers
            {
                Id = "Id",
                Name = "Fabricante",
                Phone = "123456789",
                Email = "email@email.com"
            };

            var updateManufacturerRequest = new UpdateManufacturerRequest 
            {
                Id = "Id",
                Name = "Fabricante",
                Phone = "123456789",
                Email = "email@email.com"
            };

            var categoryRepositoryMock = new Mock<IManufacturerRepository>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var prometheusServiceMock = new Mock<IPrometheusService>();

            categoryRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);
            categoryRepositoryMock.Setup(repo => repo.Update(It.IsAny<Manufacturers>(), It.IsAny<CancellationToken>()));

            var updadeManufacturerHandler = new UpdadeManufacturerHandler(categoryRepositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);

            // Act
            var result = await updadeManufacturerHandler.Handle(updateManufacturerRequest, It.IsAny<CancellationToken>());

            // Assert
            result.Data.Should().NotBeNull();
            result.Message.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();

            categoryRepositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            categoryRepositoryMock.Verify(repo => repo.Update(It.IsAny<Manufacturers>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
