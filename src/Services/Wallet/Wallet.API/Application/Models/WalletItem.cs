using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Wallet.API.Application.Models
{
    public class WalletItem
    {
        
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; } 
        public Guid Owner { get; set; }
        public DateTime Created { get; set; }       
    }
}
