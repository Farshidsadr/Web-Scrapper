using Core.Infrastructure.Domain.BaseEntities;
using Core.Infrastructure.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Infrastructure.Domain.Entities
{
    public class CurrencyRate : BaseEntity<Guid>
    {
        #region Properties

        public int SellRate { get; set; }

        public int BuyRate { get; set; }

        public string Time { get; set; } = string.Empty;

        [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; }

        [BsonRepresentation(BsonType.String)]
        public CurrencySymbol CurrencySymbol { get; set; }

        #endregion

        #region Ctor
        public CurrencyRate()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now.Date;
        }
        #endregion
    }
}