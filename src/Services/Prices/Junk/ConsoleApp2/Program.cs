using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace ConsoleApp2
{
	class Program
	{
		static void Main(string[] args)
		{

			// represents the website app

			Console.WriteLine("auto trading app");

			var botId = "superbot27";

			var factory = new ConnectionFactory();
			factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

			var connection = factory.CreateConnection();

			var channel = connection.CreateModel();


			channel.QueueDeclare($"{botId}Queue", true, false, true);

			channel.QueueBind($"{botId}Queue", "exchange.price", $"price.*.Demo");

			var consumer = new EventingBasicConsumer(channel);

			consumer.Received += (sender, args) =>
			{
				var message = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
				Console.WriteLine(message);
			};

			channel.BasicConsume($"{botId}Queue", true, consumer);

			Console.ReadLine();

			channel.Close();
			connection.Close();
		}

	}
}
