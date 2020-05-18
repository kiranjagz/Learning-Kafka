using Confluent.Kafka;
using Newtonsoft.Json;
using Producer.Model;
using Producer.Topic.Creator;
using System;
using System.Net;

namespace Producer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            int count = 10;
            var topic = "random";
            var key = $"kiran-{topic}";

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = Dns.GetHostName()
            };

            var createTopic = TopicHelper.CreateTopic(config, topic, 1, 1);

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                for (var i = 0; i <= count; i++)
                {
                    var randomModel = new RandomModel
                    {
                        Key = key,
                        Message = $"Message with count: {i}",
                        RandomNumber = new Random().Next()
                    };

                    await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = JsonConvert.SerializeObject(randomModel) });
                };

                producer.Flush(TimeSpan.FromSeconds(35));
            }

            Console.WriteLine($"{count} messages were produced to topic {topic}");

            Console.Read();
        }
    }
}

