using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockPrices.API.Infrastructure.Models
{
	public class LookUpResponse
	{
		public Guid LookUpId { get; set; }
		public double Price { get; set; }
	}
}
