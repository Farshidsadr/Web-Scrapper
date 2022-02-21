namespace Core.Application.Model.CurrencyRate
{
    public class CurrencyRateModel
    {
        #region Properties

        public int SellRate { get; set; }

        public int BuyRate { get; set; }

        #endregion

        #region Ctor

        public CurrencyRateModel(int sellRate, int buyRate)
        {
            SellRate = sellRate;
            BuyRate = buyRate;
        }

        public CurrencyRateModel()
        {

        }
        #endregion

    }
}
