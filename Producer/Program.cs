using Confluent.Kafka;
using System;
using System.Net;

namespace Producer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            int count = 10;
            var topic = "test";
            var key = "kiran";

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = Dns.GetHostName()
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                for (var i = 0; i <= count; i++)
                {
                    await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = $"This is a test message with count: {i}" });
                }

                producer.Flush(TimeSpan.FromSeconds(35));
            }

            Console.WriteLine($"{count} messages were produced to topic {topic}");

            Console.Read();
        }
    }
}
