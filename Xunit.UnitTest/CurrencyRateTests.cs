using Core.Application.Services.Concretes;
using Core.Infrastructure.Domain.Entities;
using Core.Infrastructure.Domain.Enums;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Sample.Api.Infrastructure;
using System;

namespace Xunit.UnitTest
{
    public class CurrencyRateTests
    {
        #region Properties

        private readonly CurrencyRateService _sut;
        private readonly Mock<ILogger<CurrencyRateService>> _loggerMock = new Mock<ILogger<CurrencyRateService>>();
        private readonly Mock<IMongoBaseRepository<CurrencyRate>> _repository = new Mock<IMongoBaseRepository<CurrencyRate>>();

        #endregion

        #region Ctor

        public CurrencyRateTests()
        {
            _sut = new CurrencyRateService(_loggerMock.Object, _repository.Object);
        }

        #endregion

        #region AddAsync_null_entity_should_throws_exception

        [Fact]
        public async void AddAsync_null_entity_should_throws_exception()
        {
            // Arrange
            CurrencyRate currencyRate = null;
            _repository.Setup(r => r.AddAsync(currencyRate)).ThrowsAsync(new ArgumentNullException());

            // Act
            var exceptionThrown = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.AddAsync(currencyRate));

            // Assert
            Assert.Equal("Value cannot be null.", exceptionThrown.Message);
        }

        #endregion

        #region GetAverageRateAsync_endDate_should_be_greater_than_startDate

        [Fact]
        public async void GetAverageRateAsync_endDate_should_be_greater_than_startDate()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now;

            // Act
            var exceptionThrown = await Assert.ThrowsAsync<Exception>(async () => await _sut.GetAverageRateAsync(startDate, endDate));

            //Assert
            Assert.Equal("End date should be greater that start date.", exceptionThrown.Message);
        }

        #endregion

    }
}