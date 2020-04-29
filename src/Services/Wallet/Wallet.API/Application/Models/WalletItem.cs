using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TradingBot.Services.Wallet.API.Application.Models
{
    public class WalletItem
    {
        
        public Guid Id { get; set; }

        //[BsonRepresentation(BsonType.Double)]
        public double Amount { get; set; } 
        public Guid Owner { get; set; }
        public DateTime Created { get; set; }       
    }
}
