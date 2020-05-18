using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var topic = "test";
            var cts = new CancellationTokenSource();

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "kiran",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(topic);
             
                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(cts.Token);

                        var blob = JsonConvert.SerializeObject(consumeResult);

                        Console.WriteLine($"Message from kafka: {consumeResult.Message.Value}");

                        consumer.Commit(consumeResult);
                    }
                }
                catch
                {
                    //catch bug :P
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}
