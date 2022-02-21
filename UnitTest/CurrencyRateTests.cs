using Core.Application.Services.Concretes;
using Core.Infrastructure.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Sample.Api.Infrastructure;
using Xunit;
namespace UnitTest
{
    public class CurrencyRateTests
    {
        private readonly CurrencyRateService _sut;
        private readonly Mock<ILogger<CurrencyRateService>> _loggerMock = new Mock<ILogger<CurrencyRateService>>();
        private readonly Mock<IMongoBaseRepository<CurrencyRate>> _repository = new Mock<IMongoBaseRepository<CurrencyRate>>();
        public CurrencyRateTests()
        {
            _sut = new CurrencyRateService(_loggerMock.Object, _repository.Object);
        }

        [Fact]
        public async void AddAsync_should()
        {
            // Arrange
            CurrencyRate currencyRateNull = null;
            _repository.Setup(r => r.AddAsync(currencyRateNull)).ThrowsAsync(new ArgumentNullException());

            // Act
            await _sut.AddAsync(currencyRateNull);
            var exceptionThrown = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.AddAsync(currencyRateNull));

            // Assert
            Assert.Equal("CurrencyRate", exceptionThrown.Message);
        }

        //[Fact]
        //public async Task GetAverageRateAsync()
        //{
        //    // Arrange

        //    // Act
        //    _sut.GetAverageRateAsync()
        //}
    }
}