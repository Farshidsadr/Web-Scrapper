using Core.Application.Services.Contracts;
using Core.Infrastructure.Domain.Entities;
using Microsoft.Extensions.Logging;
using Sample.Api.Infrastructure;
using HtmlAgilityPack;
using Core.Infrastructure.Domain.Enums;
using MongoDB.Driver;
using Core.Application.Model.CurrencyRate;

namespace Core.Application.Services.Concretes
{
    public class CurrencyRateService : ICurrencyRateService
    {
        #region Properties

        private readonly ILogger<CurrencyRateService> _logger;
        private readonly IMongoBaseRepository<CurrencyRate> _repository;

        #endregion

        #region ctor

        public CurrencyRateService(ILogger<CurrencyRateService> logger,
            IMongoBaseRepository<CurrencyRate> currencyRateRepository)
        {
            _logger = logger;
            _repository = currencyRateRepository;
        }

        #endregion

        #region AddAsync

        public async Task AddAsync(CurrencyRate currencyRate)
        {
            await _repository.AddAsync(currencyRate);
        }

        #endregion

        #region GetAllAsync

        public async Task<List<CurrencyRate>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.ToList();
        }

        #endregion

        #region ExtractRateAsync
        /// <summary>
        /// This function scrapes data from https://mex.co.ir/ url and extract the price of currency symbol that passed
        /// in argument against Rial (currency symbol is optional and its default value is Dollar).
        /// </summary>
        /// <returns>A bool. true if date scraped and saved to database successfully, otherwise false.</returns>
        public async Task<bool> ExtractRateAsync(CurrencySymbol currencySymbol = CurrencySymbol.Dollar)
        {
            var buy_price = string.Empty;
            var sell_price = string.Empty;

            HtmlWeb web = new HtmlWeb();
            try
            {
                var doc = await web.LoadFromWebAsync("https://mex.co.ir/");
                switch (currencySymbol)
                {
                    case CurrencySymbol.Dollar:
                        buy_price = doc.DocumentNode.SelectSingleNode("//div[@id='wrapper']/main/div[3]/div[2]/div/div/div[1]/div/table/tbody/tr[2]/td[3]/span").InnerText.Replace(",", "");
                        sell_price = doc.DocumentNode.SelectSingleNode("//div[@id='wrapper']/main/div[3]/div[2]/div/div/div[1]/div/table/tbody/tr[2]/td[4]/span").InnerText.Replace(",", "");
                        break;
                    case CurrencySymbol.Rial:
                        throw new NotImplementedException();
                    case CurrencySymbol.Euro:
                        throw new NotImplementedException();
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Error happened in getting/parsing data from https://mex.co.ir/ url. Details: {0}", ex.Message));
                return false;
            }

            try
            {
                // When one of prices is 0, we should not add prices to database for consistency of average api
                if (sell_price == "0" || buy_price == "0")
                    return false;

                await _repository.AddAsync(new CurrencyRate()
                {
                    CurrencySymbol = CurrencySymbol.Dollar,
                    BuyRate = int.Parse(buy_price),
                    SellRate = int.Parse(sell_price),
                    Time = DateTime.Now.ToShortTimeString()
                });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("An error happened while adding currency rate in database. Details: {0}", ex.Message));
                return false;
            }
        }
        #endregion

        #region GetAverageRateAsync
        /// <summary>
        /// This function takes a time interval and a currency symbol, then calculate the average buy rate and sell rate of 
        /// the related currency against Rial in that interval.
        /// </summary>
        /// <param name="startDate">A date for interval start</param>
        /// <param name="endDate">A date for interval end</param>
        /// <param name="currencySymbol">Symbol of money (Optional, default value is Dollar)</param>
        /// <returns>A CurrencyRateModel that contains buy rate average and sell rate average if data exist, 
        /// otherwise returns null.</returns>
        public async Task<CurrencyRateModel?> GetAverageRateAsync(DateTime startDate, DateTime endDate,
            CurrencySymbol currencySymbol = CurrencySymbol.Dollar)
        {
            if (startDate > endDate)
            {
                throw new Exception("End date should be greater that start date.");
            }

            var result = await _repository.Set.Aggregate().Match(p => p.Date >= startDate.Date && p.Date <= endDate.Date)
                .Match(p => p.CurrencySymbol == currencySymbol)
                .Project(new ProjectionDefinitionBuilder<CurrencyRate>()
                .Expression(p => new CurrencyRateModel(p.SellRate, p.BuyRate))).ToListAsync();

            if (result.Count == 0)
            {
                return null;
            }

            var model = new CurrencyRateModel()
            {
                BuyRate = Convert.ToInt32(result.Average(p => p.BuyRate)),
                SellRate = Convert.ToInt32(result.Average(p => p.SellRate))
            };

            return model;
        }
        #endregion
    }
}