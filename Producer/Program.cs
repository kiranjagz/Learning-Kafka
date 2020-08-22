using Confluent.Kafka;
using Newtonsoft.Json;
using Producer.Model;
using Producer.Topic.Creator;
using System;
using System.Collections.Generic;
using System.Net;

namespace Producer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            int count = 10;
            var key = string.Empty;
            List<string> topics = new List<string>();
            topics.Add("test");
            topics.Add("test2");

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = Dns.GetHostName()
            };

            //Create a topic with 3 partions with a replication factor of 1
            //var createTopic = TopicHelper.CreateTopic(config, topic, 3, 1);

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    topics.ForEach(async topic =>
                    {
                        for (var i = 0; i <= count; i++)
                        {
                            if (i % 2 == 0)
                            {
                                key = "ev";
                            }
                            else { key = "od"; }

                            var randomModel = new RandomModel
                            {
                                Key = key,
                                Message = $"Message with count: {i}",
                                RandomNumber = new Random().Next()
                            };

                            await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = JsonConvert.SerializeObject(randomModel) });

                            Console.WriteLine($"{count} messages were produced to on topic {topic}");
                        };

                        producer.Flush(TimeSpan.FromSeconds(35));
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error has occurred: {e.Message}");
                }

                Console.Read();
            }
        }
    }
}

