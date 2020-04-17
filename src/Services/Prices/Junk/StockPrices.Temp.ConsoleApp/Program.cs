using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using StockPrices.Temp.ConsoleApp.Protos;

namespace StockPrices.Temp.ConsoleApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			await Task.Delay(TimeSpan.FromSeconds(10));

			using var channel = GrpcChannel.ForAddress("https://localhost:5001");

			var client = new Pricing.PricingClient(channel);

			var req = new StockPriceRequest
			{
				Context = Context.Query,
				Symbol = "TSLA"
			};

			var res = client.GetPriceStream(req);

			int i = 0;
			while(await res.ResponseStream.MoveNext())
			{
				Console.WriteLine($"Stock: Tesla, Price: {res.ResponseStream.Current}");
				if (i++ == 10) break;
			}

			res.Dispose();

			
			Console.ReadLine();


		}
	}
}
