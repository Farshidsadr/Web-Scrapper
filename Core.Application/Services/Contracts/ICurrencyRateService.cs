using Core.Application.Model.CurrencyRate;
using Core.Infrastructure.Domain.Entities;
using Core.Infrastructure.Domain.Enums;

namespace Core.Application.Services.Contracts
{
    public interface ICurrencyRateService
    {
        Task<List<CurrencyRate>> GetAllAsync();

        Task AddAsync(CurrencyRate currencyRate);

        Task<bool> ExtractRateAsync(CurrencySymbol currencySymbol = CurrencySymbol.Dollar);

        Task<CurrencyRateModel?> GetAverageRateAsync(DateTime startDate, DateTime endDate,
            CurrencySymbol currencySymbol = CurrencySymbol.Dollar);
    }
}
