using Confluent.Kafka;
using System;
class Program
{
	static void Main(string[] args)
	{
		var config = new ConsumerConfig
		{
			GroupId = "gid-playerbets",
			BootstrapServers = "localhost:29093"
		};

		using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
		{
			consumer.Subscribe("aggregated_table");
			while (true)
			{
				var cr = consumer.Consume();
				Console.WriteLine(cr.Message.Value);
			}
		}
	}
}


Console.WriteLine("Hello, World!");
