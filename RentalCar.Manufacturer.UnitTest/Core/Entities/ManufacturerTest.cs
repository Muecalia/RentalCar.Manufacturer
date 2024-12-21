using FluentAssertions;
using RentalCar.Manufacturer.Core.Entities;

namespace RentalCar.Manufacturer.UnitTest.Core.Entities
{
    public class ManufacturerTest
    {
        [Fact]
        public void Manufacturer_Success()
        {
            // Arrange
            var category = new Manufacturers 
            {
                Id = "Id",
                Name = "Manufacturer",
                Phone = "123456789",
                Email = "email@email.com"
            };

            // Act

            //Assert
            category.Should().NotBeNull();
            category.Name.Should().NotBeNullOrEmpty();
            category.CreatedAt.Date.Should().Be(DateTime.Now.Date);
        }
    }
}
